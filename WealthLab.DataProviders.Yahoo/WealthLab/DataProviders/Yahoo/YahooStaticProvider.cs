namespace WealthLab.DataProviders.Yahoo
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.DataProviders.Helper;
    using WealthLab.DataProviders.Yahoo.Properties;

    public class YahooStaticProvider : StaticDataProvider
    {
        private BarDataStore barDataStore;
        private static ClassificationGroupsFileUpdater classificationGroupsUpdater;
        private GroupsToSymbols groupsToSymbolsUpdater;
        private DataFetcher dataFetcher;
        private Dictionary<string, string> symbolNames = new Dictionary<string, string>();
        private IDataUpdateMessage idataUpdateMessage;
        private int numberOfSymbolsToUpdate;
        private int numberOfSymbolsUpdated;
        private List<string> symbolsLoadedBefore = new List<string>();  ///WYJ fix, original name list_1
        private object lockForSaveBars = new object();
        private static string providerDataDir;
        private static YahooClientSettings yahooClientSettings = null;
        private YahooFundamentalProvider yahooFundamentalProvider;
        private YahooWizardPageClassification yahooWizardPageClassification;
        private YahooWizardPageStart yahooWizardPageStart;
        private YahooWizardPageSymbols yahooWizardPageSymbols;

        public YahooStaticProvider()
        {
            Logger.LogParameters(new object[0]);
        }

        public static void AddUserEditedDates(Bars toBars, Bars fromBars)
        {
            toBars.UserEditedDates.Clear();
            toBars.UserEditedDates.AddRange(fromBars.UserEditedDates);
        }

        public override void CancelUpdate()
        {
            Logger.LogParameters(new object[0]);
            this.dataFetcher.CancelUpdate();
        }

        public override DataSource CreateDataSource()
        {
            Logger.LogParameters(new object[0]);
            DataSource source = new DataSource(this);
            YahooStaticSettings settings = new YahooStaticSettings();
            if (this.yahooWizardPageStart.optChooseFromClassification())
            {
                settings.Symbols = this.yahooWizardPageClassification.generateSymbolList().ToString();
                settings.Groups = this.yahooWizardPageClassification.getGroupListString();
                settings.UpdateGroups = this.yahooWizardPageStart.optUpdateGroup();
            }
            else
            {
                settings.Symbols = this.yahooWizardPageSymbols.generateSymbolList().ToString();
            }
            settings.StartDate = this.yahooWizardPageStart.optStartDate();
            source.DSString = settings.SerializeToString();
            source.Scale = BarScale.Daily;
            source.BarInterval = 0;
            return source;
        }

        public override void DeleteSymbolDataFile(DataSource dataSource_0, string symbol)
        {
            Logger.LogParameters(new object[0]);
            this.barDataStore.RemoveFile(Encoder.encode(symbol), dataSource_0.Scale, dataSource_0.BarInterval);
        }

        public override void Initialize(IDataHost dataHost)
        {
            Logger.LogParameters(new object[0]);
            base.Initialize(dataHost);
            this.barDataStore = new BarDataStore(dataHost, this);
            providerDataDir = this.barDataStore.RootPath;
            this.dataFetcher = new DataFetcher();
            this.dataFetcher.AddStaticDataHandler(new DataFetcher.StaticDataHandler(this.onData));
            this.dataFetcher.AddStaticErrorHandler(new DataFetcher.StaticErrorHandler(this.onError));
            classificationGroupsUpdater = new ClassificationGroupsFileUpdater(providerDataDir + "YahooClassification.xml", "http://67.199.28.171/Classification/Yahoo/YahooClassification.xml");
            if (yahooClientSettings == null)
            {
                yahooClientSettings = YahooClientSettings.Deserialize(providerDataDir);
            }
            ServicePointManager.MaxServicePointIdleTime = 0x2710;
            ServicePointManager.UseNagleAlgorithm = true;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.FindServicePoint(new Uri("http://ichart.finance.yahoo.com"));
        }

        private void onError(object sender, StaticErrorEventArgs e)  ///WYJ note: looks like an error handler
        {
            Logger.LogParameters(new object[0]);
            if (e.request != null)
            {
                this.numberOfSymbolsUpdated++;
                this.DisplayUpdateMessage(e.request.getSymbol(), "Error: " + e.exception.Message, Thread.CurrentThread.Name);
                this.DisplayUpdateProgress();
            }
            else
            {
                this.DisplayUpdateMessage("Thread (" + Thread.CurrentThread.Name + ") execution error! " + e.exception.Message);
            }
        }

        private void onData(object sender, StaticDataEventArgs e) ///WYJ fix, update Bars for security
        {
            Logger.LogParameters(new object[0]);
            if ((e.request.getDataType() & DataTypeEnum.Quote) != 0)
            {
                if (e.bars != null)
                {
                    int correctionCount;
                    lock (this.symbolsLoadedBefore)
                    {
                        if (this.symbolsLoadedBefore.Contains(e.request.getSymbol())) ///WYJ fix, if symbol is loaded before, need to remove the file first
                        {
                            this.barDataStore.RemoveFile(Encoder.encode(e.request.getSymbol()), BarScale.Daily, 0);
                            this.symbolsLoadedBefore.Remove(e.request.getSymbol());
                        }
                    }
                    Bars bars = new Bars(Encoder.encode(e.request.getSymbol()), BarScale.Daily, 0);
                    this.barDataStore.LoadBarsObject(bars);
                    int count = bars.Count;
                    bars.AppendWithCorrections(e.bars, out correctionCount);
                    if (this.symbolNames.ContainsKey(e.request.getSymbol()))
                    {
                        string symbolName = this.symbolNames[e.request.getSymbol()];
                        if (symbolName != string.Empty)
                        {
                            bars.SecurityName = symbolName.ToUpper();
                        }
                    }
                    lock (this.lockForSaveBars)
                    {
                        this.barDataStore.SaveBarsObject(bars);
                    }
                    this.numberOfSymbolsUpdated++;
                    this.DisplayUpdateMessage(bars, count, correctionCount, Thread.CurrentThread.Name);
                    this.DisplayUpdateProgress();
                }
                if (e.splitAndDividend != null)
                {
                    if (this.yahooFundamentalProvider == null)
                    {
                        this.yahooFundamentalProvider = new YahooFundamentalProvider();
                        this.yahooFundamentalProvider.Initialize(base.DataHost);
                    }
                    this.yahooFundamentalProvider.UpdateData(e.request.getSymbol(), e.splitAndDividend.getDividend(), "Dividend (Yahoo! Finance)");
                    this.yahooFundamentalProvider.UpdateData(e.request.getSymbol(), e.splitAndDividend.getSplit(), "Split (Yahoo! Finance)");
                }
            }
        }

        ///WYJ fix, original name: method_10
        private void clearUserEditedDates(Bars bars_0)
        {
            bars_0.UserEditedDates.Clear();
        }

        private SnDHandler getSnDHandler(string symbol, DateTime dateTime_0)
        {
            if (!yahooClientSettings.DividendAdj && !yahooClientSettings.SplitAdj)
            {
                return null;
            }
            SnDEnum enum2 = SnDEnum.Dividend;
            if (yahooClientSettings.SplitAdj)
            {
                enum2 = SnDEnum.Split;
            }
            if (yahooClientSettings.DividendAdj && yahooClientSettings.SplitAdj)
            {
                enum2 = SnDEnum.Dividend | SnDEnum.Split;
            }
            this.yahooFundamentalProvider = new YahooFundamentalProvider();
            this.yahooFundamentalProvider.Initialize(base.DataHost);
            IList<FundamentalItem> splitList = this.yahooFundamentalProvider.RequestItems(symbol, "Split (Yahoo! Finance)");
            IList<FundamentalItem> divList = this.yahooFundamentalProvider.RequestItems(symbol, "Dividend (Yahoo! Finance)");
            if (dateTime_0 != DateTime.MaxValue)
            {
                return new SnDHandler(splitList, divList, enum2, yahooClientSettings.AdjModeWhenDataRange, dateTime_0);
            }
            return new SnDHandler(splitList, divList, enum2);
        }

        private void DisplayUpdateProgress()
        {
            if (this.idataUpdateMessage != null)
            {
                this.idataUpdateMessage.ReportUpdateProgress((this.numberOfSymbolsUpdated * 100) / this.numberOfSymbolsToUpdate);
            }
        }

        private void DisplayUpdateMessage(string string_1)
        {
            if (this.idataUpdateMessage != null)
            {
                this.idataUpdateMessage.DisplayUpdateMessage(string_1);
            }
        }

        private void DisplayUpdateMessage(string string_1, string string_2, string string_3)
        {
            if (this.idataUpdateMessage != null)
            {
                this.idataUpdateMessage.DisplayUpdateMessage(string.Format("{0,-4} {1,-9} {2}", "[" + string_3 + "]", string_1, string_2));
            }
        }

        private void DisplayUpdateMessage(Bars bars, int count, int correctionCount, string string_1)
        {   
            if (this.idataUpdateMessage != null)
            {
                string str = string.Empty;
                if (bars.Count > 0)
                {
                    string str2 = bars.Date[bars.Count - 1].ToString("MM.dd.yyyy");
                    str = string.Format("{0,-4} {1,-9} {2,-14} {3,-15} {4,-18}", new object[] { "[" + string_1 + "]", bars.Symbol, bars.Count + " bars", str2, (bars.Count - count) + " bars added" });
                    if (correctionCount > 0)
                    {
                        str = string.Format("{0} {1,-18}", str, correctionCount + " bars corrected");
                    }
                }
                else
                {
                    str = string.Format("{0,-4} {1,-9} {2}", "[" + string_1 + "]", bars.Symbol, "Error: No data");
                }
                this.idataUpdateMessage.DisplayUpdateMessage(str);
            }
        }

        ///WYJ fix, original name: method_6
        private bool adjModeAllowingEditing()
        {
            if (yahooClientSettings.AdjModeWhenDataRange == AdjustedModeWhenDataRange.Ignore)
            {
                MessageBox.Show("Editing Bars with the \"Ignore splits and dividends which fall out of the range\"\r\noption selected is not possible.\r\n\r\nPlease open Data Manager, change this setting of the Yahoo! provider,\r\nre-open the chart and try again.", "Yahoo! Provider", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        ///WYJ fix, original name: method_7
        private void updateClassificationGroups(DataSource ds)  ///WYJ note: update group specification for data source
        {
            Logger.LogParameters(new object[0]);
            YahooStaticSettings settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(ds.DSString);
            if (((this.groupsToSymbolsUpdater == null) || !classificationGroupsUpdater.FileExists()) || classificationGroupsUpdater.IsLastUpdatedDaysAgo(1))
            {
                if (!classificationGroupsUpdater.FileExists() || classificationGroupsUpdater.IsLastUpdatedDaysAgo(1))
                {
                    this.DisplayUpdateMessage("Updating the Classification Groups file...");
                    classificationGroupsUpdater.UpdateClassificationGroupsFile(false);
                    if (classificationGroupsUpdater.GetException() != null)
                    {
                        this.DisplayUpdateMessage("Error: " + classificationGroupsUpdater.GetException().Message);
                        return;
                    }
                    this.DisplayUpdateMessage("Classification file updated.");
                }
                this.groupsToSymbolsUpdater = new GroupsToSymbols(classificationGroupsUpdater.ReadClassificationGroupFromFile());
            }
            this.DisplayUpdateMessage("Checking the DataSet's Classification groups composition:");
            SymbolList groupList = new SymbolList(settings.Groups, DelimeterSetEnum.ForProgram);
            SymbolList symbolList = new SymbolList(settings.Symbols, DelimeterSetEnum.ForProgram);
            SymbolList symbolListFromGroups = this.groupsToSymbolsUpdater.generateNewSymbolList(symbolList, groupList.list.ToArray());
            this.DisplayUpdateMessage(string.Format("Symbols deleted {0}: {1}", this.groupsToSymbolsUpdater.GetDeleteList().list.Count, this.groupsToSymbolsUpdater.GetDeleteList().ToString()));
            this.DisplayUpdateMessage(string.Format("Symbols added {0}: {1}", this.groupsToSymbolsUpdater.GetAddList().list.Count, this.groupsToSymbolsUpdater.GetAddList().ToString()));
            if ((this.groupsToSymbolsUpdater.GetDeleteList().list.Count > 0) || (this.groupsToSymbolsUpdater.GetAddList().list.Count > 0))
            {
                string str = Directory.GetParent(providerDataDir).Parent.FullName + @"\DataSets\";
                settings.Symbols = symbolListFromGroups.ToString();
                ds.DSString = settings.SerializeToString();
                ds.SaveToFile(str + SymbolFileNameConverter.SymbolToFileName(ds.Name) + ".xml");
            }
        }

        ///WYJ note, update start date for request; update the symbols start date
        private void setStartDateForRequest(ref DataRequest dataRequest, SymbolInfoList symbolsStartDate, DateTime startDateTime)
        {
            //This method does three things:
            //1. set the startDate of data request
            //2. mark the symbol in this.symbolsLoadedBefore
            //3. update symbolsStartDate
            DateTime time = startDateTime;
            DateTime timeInList = symbolsStartDate.Search(dataRequest.getSymbol(), BarScale.Daily, 0);   ///WYJ note, get start date of the symbol if existing in Symbol info list

            if (time >= timeInList) ///WYJ note, when timeInList <= time, that means it's in the symbol info list
            {   
                dataRequest.setSnDStartDate(timeInList);
                DateTime lastUpdated = this.barDataStore.SymbolLastUpdated(Encoder.encode(dataRequest.getSymbol()), BarScale.Daily, 0);
                if (lastUpdated != DateTime.MinValue)
                {
                    time = lastUpdated.AddDays(-10.0);
                }

                //1. this.symbolsLoadedBefore should need to change, but somehow it doesn't matter? because in onData method, 
                //whether it removes the file or not, the file is going to be overwritten. 
                //2. symbolsStartDate doesn't need to change
            }
            else  ///if (timeInList > time)
            {
                ///time is earlier that start time in list, that means we can't use cashed data and only fetch for recent days, we have to start from time
                dataRequest.setSnDStartDate(time);
                if (timeInList != DateTime.MaxValue)
                {
                    this.symbolsLoadedBefore.Add(dataRequest.getSymbol());
                }
                symbolsStartDate.Add(dataRequest.getSymbol(), BarScale.Daily, 0, time);
            }
            
            dataRequest.setStartDate(time);
        }

        private void updateSecurityNames(List<DataRequest> reqList)
        {
            try
            {
                this.DisplayUpdateMessage("Updating Security Names for " + reqList.Count + " symbols...");
                this.symbolNames = this.dataFetcher.GetSymbolNames(reqList);
                Logger.Log("Sec. Names");
                foreach (KeyValuePair<string, string> pair in this.symbolNames)
                {
                    Logger.Log(pair.Key + " " + pair.Value);
                }
                Logger.Log("-----");
                this.DisplayUpdateMessage("Security Names updated.");
            }
            catch (Exception exception)
            {
                this.DisplayUpdateMessage("Error updating Security Names. " + exception.Message);
            }
        }

        public override string ModifySymbols(DataSource ds, List<string> symbols)
        {
            Logger.LogParameters(new object[0]);
            SymbolList sList = new SymbolList(symbols);
            YahooStaticSettings settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(ds.DSString);
            settings.Symbols = sList.ToString().ToUpper();
            return settings.SerializeToString();
        }

        public override void PopulateSymbols(DataSource ds, List<string> symbols)
        {
            Logger.LogParameters(new object[] { ds.Name });
            if (!string.IsNullOrEmpty(ds.Name))
            {
                YahooStaticSettings settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(ds.DSString);
                SymbolList sList = new SymbolList(settings.Symbols, DelimeterSetEnum.ForProgram);
                symbols.AddRange(sList.list);
            }
        }

        public override Bars RequestData(DataSource dataSource, string symbol, DateTime startDate, DateTime symbolEndDate, int maxBars, bool includePartialBar)
        {
            Logger.LogWithStackTrace(new object[] { dataSource.DSString, symbol, startDate, symbolEndDate, maxBars, includePartialBar });
            symbol = symbol.Trim(new char[] { ' ', '"' });
            Logger.Log("On Demand Update Enabled: " + base.DataHost.OnDemandUpdateEnabled);
            if (base.DataHost.OnDemandUpdateEnabled && !ClientSettings.NeverPerformOnDemandUpdates)
            {
                try
                {
                    YahooStaticSettings settings = new YahooStaticSettings();
                    if (dataSource.DSString != string.Empty)
                    {
                        settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(dataSource.DSString);
                    }
                    SymbolInfoList list = SymbolInfoList.Deserialize(providerDataDir + "SymbolsStartDate.xml");
                    list.Synchronize(this.barDataStore);
                    DataRequest req = new DataRequest(symbol);
                    this.setStartDateForRequest(ref req, list, settings.StartDate);
                    req.setEndDate(DateTime.Now.AddDays(2.0));
                    req.setDataType(DataTypeEnum.SnD | DataTypeEnum.Quote);
                    if (includePartialBar)
                    {
                        req.setDataType(req.getDataType() | DataTypeEnum.RealTime);
                    }
                    List<DataRequest> reqList = new List<DataRequest> { req };
                    this.updateSecurityNames(reqList);
                    this.dataFetcher.updateSecurityData(reqList);
                }
                catch (Exception exception)
                {
                    string str = "On Demand Update Error: " + exception.Message;
                    Logger.Log(LogLevel.ERROR, str);
                    MessageBox.Show(str, "On Demand Update Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            Bars bars = new Bars(Encoder.encode(symbol), dataSource.Scale, dataSource.BarInterval);
            if (this.barDataStore.ContainsSymbol(Encoder.encode(symbol), dataSource.Scale, dataSource.BarInterval))
            {
                this.barDataStore.LoadBarsObject(bars, startDate, DateTime.MaxValue, maxBars);
            }
            if (symbol != Encoder.encode(symbol))
            {
                Bars toBars = new Bars(symbol, dataSource.Scale, dataSource.BarInterval);
                toBars.Append(bars);
                toBars.SecurityName = bars.SecurityName;
                if (VersionContainsUserEditedDates())
                {
                    AddUserEditedDates(toBars, bars);
                }
                bars = toBars;
            }
            SnDHandler snDHandler = this.getSnDHandler(symbol, symbolEndDate);
            if (snDHandler != null)
            {
                bars = snDHandler.ProcessSplitAndDividend(bars);
            }
            return bars;
        }

        public Bars RequestHistoricalData(string symbol, DateTime startDate, DateTime endDate)
        {
            return this.dataFetcher.method_29(symbol, startDate, endDate);
        }

        public override void SaveEditedSymbolDataFile(DataSource dataSource_0, Bars bars)
        {
            Bars bars2 = new Bars(bars.Symbol, bars.Scale, bars.BarInterval);
            SnDHandler snDHandler = this.getSnDHandler(bars.Symbol, DateTime.MaxValue);
            this.barDataStore.LoadBarsObject(bars2);
            if (bars.Count >= bars2.Count)
            {
                if (bars.Count > bars2.Count)
                {
                    if ((snDHandler != null) && !this.adjModeAllowingEditing())
                    {
                        return;
                    }
                    foreach (DateTime time in bars.UserEditedDates)
                    {
                        if (bars2.ConvertDateToBar(time, true) == -1)
                        {
                            int num5 = bars.ConvertDateToBar(time, true);
                            if (snDHandler != null)
                            {
                                DataSeries series;
                                int num6;
                                DataSeries series2;
                                int num7;
                                DataSeries series3;
                                int num8;
                                DataSeries series4;
                                int num9;
                                DataSeries series5;
                                int num10;
                                snDHandler.ProcessSplitAndDividend(bars);
                                SnDFactor class4 = snDHandler.GetFactorForDateTime(time);
                                (series = bars.Open)[num6 = num5] = series[num6] / class4.FactorForSnD();
                                (series2 = bars.High)[num7 = num5] = series2[num7] / class4.FactorForSnD();
                                (series3 = bars.Low)[num8 = num5] = series3[num8] / class4.FactorForSnD();
                                (series4 = bars.Close)[num9 = num5] = series4[num9] / class4.FactorForSnD();
                                (series5 = bars.Volume)[num10 = num5] = series5[num10] / class4.FactorForSplit();
                            }
                        }
                    }
                    bars2 = bars;
                }
                else if (snDHandler == null)
                {
                    bars2 = bars;
                }
                else
                {
                    if (!this.adjModeAllowingEditing())
                    {
                        return;
                    }
                    snDHandler.ProcessSplitAndDividend(bars2);
                    int num2 = 0;
                    for (int i = 0; i < bars.Count; i++)
                    {
                        snDHandler.GetFactorForDateTime(bars.Date[i]);
                        if (((bars2.Open[i] != bars.Open[i]) || (bars2.High[i] != bars.High[i])) || (((bars2.Low[i] != bars.Low[i]) || (bars2.Close[i] != bars.Close[i])) || (bars2.Volume[i] != bars.Volume[i])))
                        {
                            num2++;
                        }
                    }
                    if (num2 == 1)
                    {
                        foreach (DateTime time2 in bars.UserEditedDates)
                        {
                            SnDFactor class5 = snDHandler.GetFactorForDateTime(time2);
                            int num11 = bars.ConvertDateToBar(time2, true);
                            if (((bars2.Open[num11] != bars.Open[num11]) || (bars2.High[num11] != bars.High[num11])) || (((bars2.Low[num11] != bars.Low[num11]) || (bars2.Close[num11] != bars.Close[num11])) || (bars2.Volume[num11] != bars.Volume[num11])))
                            {
                                bars2.Open[num11] = bars.Open[num11] / class5.FactorForSnD();
                                bars2.High[num11] = bars.High[num11] / class5.FactorForSnD();
                                bars2.Low[num11] = bars.Low[num11] / class5.FactorForSnD();
                                bars2.Close[num11] = bars.Close[num11] / class5.FactorForSnD();
                                bars2.Volume[num11] = bars.Volume[num11] / class5.FactorForSplit();
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < bars.Count; j++)
                        {
                            SnDFactor class3 = snDHandler.GetFactorForDateTime(bars.Date[j]);
                            bars.Open[j] /= class3.FactorForSnD();
                            bars.High[j] /= class3.FactorForSnD();
                            bars.Low[j] /= class3.FactorForSnD();
                            bars.Close[j] /= class3.FactorForSnD();
                            bars.Volume[j] /= class3.FactorForSplit();
                        }
                        bars2 = bars;
                    }
                }
            }
            else
            {
                for (int k = bars2.Count - 1; k >= 0; k--)
                {
                    if (!bars.Date.Contains(bars2.Date[k]))
                    {
                        bars2.Delete(k);
                        break;
                    }
                }
            }
            bars2.UserEditedDates.Clear();
            bars2.UserEditedDates.AddRange(bars.UserEditedDates);
            this.barDataStore.SaveBarsObject(bars2);
        }

        public override bool SupportsDynamicUpdate(BarScale scale)
        {
            Logger.LogParameters(new object[0]);
            return false;
        }

        public override void UpdateDataSource(DataSource dataSource, IDataUpdateMessage dataUpdateMsg)
        {
            Logger.LogParameters(new object[] { dataSource.Name, dataSource.Symbols[0] });
            this.dataFetcher.SetCancelFlag(false);
            SymbolInfoList symbolInfoList = null;
            this.idataUpdateMessage = dataUpdateMsg;
            try
            {
                SymbolList list;
                YahooStaticSettings settings = null;
                if (!string.IsNullOrEmpty(dataSource.Name))
                {
                    settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(dataSource.DSString);
                    if (settings.UpdateGroups)
                    {
                        this.updateClassificationGroups(dataSource);
                        settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(dataSource.DSString);
                    }
                    list = new SymbolList(settings.Symbols, DelimeterSetEnum.ForProgram);
                }
                else
                {
                    list = new SymbolList(dataSource.Symbols);
                }
                symbolInfoList = SymbolInfoList.Deserialize(providerDataDir + "SymbolsStartDate.xml");
                symbolInfoList.Synchronize(this.barDataStore);
                List<DataRequest> requestList = new List<DataRequest>();
                this.DisplayUpdateMessage("Preparing requests ...");
                foreach (string s in list.list)
                {
                    DataRequest request = new DataRequest(s);
                    if (!string.IsNullOrEmpty(dataSource.Name))
                    {
                        this.setStartDateForRequest(ref request, symbolInfoList, settings.StartDate);
                    }
                    else
                    {
                        this.setStartDateForRequest(ref request, symbolInfoList, new DateTime(2000, 1, 1));
                    }
                    request.setEndDate(DateTime.Now.AddDays(2.0));
                    request.setDataType(DataTypeEnum.SnD | DataTypeEnum.Quote);
                    requestList.Add(request);
                    if (this.dataFetcher.GetCancelFlag())
                    {
                        return;
                    }
                }
                this.DisplayUpdateMessage("Requests are ready to go.");
                if (requestList.Count > 0)
                {
                    this.numberOfSymbolsToUpdate = requestList.Count;
                    this.numberOfSymbolsUpdated = 0;
                    this.updateSecurityNames(requestList);
                    this.dataFetcher.updateSecurityData(requestList);
                }
            }
            catch (Exception exception)
            {
                Logger.Log(LogLevel.ERROR, "UpdateDataSource " + exception.Message);
                this.idataUpdateMessage.DisplayUpdateMessage("Error: " + exception.Message);
            }
            finally
            {
                symbolInfoList.Serialize(providerDataDir + "SymbolsStartDate.xml");
                this.idataUpdateMessage = null;
            }
        }

        public override void UpdateProvider(IDataUpdateMessage dataUpdateMsg, List<DataSource> dataSources, bool updateNonDSSymbols, bool deleteNonDSSymbols)
        {
            Logger.LogParameters(new object[0]);
            this.dataFetcher.SetCancelFlag(false);
            this.idataUpdateMessage = dataUpdateMsg;
            List<DataRequest> requestList = new List<DataRequest>();
            SymbolInfoList symbolInfoList = null;
            try
            {
                symbolInfoList = SymbolInfoList.Deserialize(providerDataDir + "SymbolsStartDate.xml");
                symbolInfoList.Synchronize(this.barDataStore);
                this.DisplayUpdateMessage("Preparing requests ...");
                foreach (DataSource source in dataSources)
                {
                    YahooStaticSettings settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(source.DSString);
                    SymbolList symbolList = new SymbolList(settings.Symbols, DelimeterSetEnum.ForProgram);
                    foreach (string symbol in symbolList.list)
                    {
                        DataRequest class4 = new DataRequest(symbol);
                        this.setStartDateForRequest(ref class4, symbolInfoList, settings.StartDate);
                        class4.setEndDate(DateTime.Now.AddDays(2.0));
                        class4.setDataType(DataTypeEnum.SnD | DataTypeEnum.Quote);
                        for (int i = requestList.Count - 1; i >= 0; i--)
                        {
                            if (requestList[i].getSymbol() == class4.getSymbol())
                            {
                                requestList.RemoveAt(i);
                            }
                        }
                        requestList.Add(class4);
                        if (this.dataFetcher.GetCancelFlag())
                        {
                            return;
                        }
                    }
                }
                if (updateNonDSSymbols)
                {
                    IList<string> existingSymbols = this.barDataStore.GetExistingSymbols(BarScale.Daily, 0);
                    Logger.Log("symbolsInDataStore.Count " + existingSymbols.Count);
                    foreach (string s in existingSymbols)
                    {
                        bool flag2 = true;
                        for (int j = requestList.Count - 1; j >= 0; j--)
                        {
                            if (requestList[j].getSymbol() == s)
                            {
                                //goto  Label_01E9; ///WYJ fix, simplify the flow
                                flag2 = false; 
                                break;
                            }
                        }
                        if (flag2)
                        {
                            DataRequest item = new DataRequest(s);
                            DateTime time2 = this.barDataStore.SymbolLastUpdated(Encoder.encode(item.getSymbol()), BarScale.Daily, 0);
                            item.setStartDate(time2.AddDays(-10.0));
                            item.setEndDate(DateTime.Now.AddDays(2.0));
                            item.setDataType(DataTypeEnum.SnD | DataTypeEnum.Quote);
                            requestList.Add(item);
                        }
                        if (this.dataFetcher.GetCancelFlag())
                        {
                            return;
                        }
                    }
                }
                requestList.Sort();
                this.DisplayUpdateMessage("Requests are ready to go.");
                if (requestList.Count > 0)
                {
                    this.numberOfSymbolsToUpdate = requestList.Count;
                    this.numberOfSymbolsUpdated = 0;
                    this.updateSecurityNames(requestList);
                    this.dataFetcher.updateSecurityData(requestList);
                }
                if (deleteNonDSSymbols)
                {
                    IList<string> list4 = this.barDataStore.GetExistingSymbols(BarScale.Daily, 0);
                    string str = string.Empty;
                    int num = 0;
                    foreach (string str2 in list4)
                    {
                        bool flag = false;
                        using (List<DataRequest>.Enumerator enumerator4 = requestList.GetEnumerator())
                        {
                            while (enumerator4.MoveNext())
                            {
                                DataRequest current = enumerator4.Current;
                                if (current.getSymbol() == str2)
                                {
                                    ///goto  Label_032F;  ///WYJ fix, simplify the flow
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        if (!flag)
                        {
                            num++;
                            this.barDataStore.RemoveFile(Encoder.encode(str2), BarScale.Daily, 0);
                            if (str != string.Empty)
                            {
                                str = str + ",";
                            }
                            str = str + str2;
                        }
                    }
                    if (num > 0)
                    {
                        dataUpdateMsg.DisplayUpdateMessage("Deleted symbols: " + str);
                        dataUpdateMsg.DisplayUpdateMessage("Deleted " + num + " symbol data files");
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Log(LogLevel.ERROR, "UpdateDataSource " + exception.Message);
                this.idataUpdateMessage.DisplayUpdateMessage("Error: " + exception.Message);
            }
            finally
            {
                symbolInfoList.Serialize(providerDataDir + "SymbolsStartDate.xml");
                this.idataUpdateMessage = null;
            }
        }

        public static bool VersionContainsUserEditedDates()
        {
            return (Assembly.GetEntryAssembly().GetName().Version >= new Version("5.3"));
        }

        public override UserControl WizardFirstPage()
        {
            if (this.yahooWizardPageStart == null)
            {
                this.yahooWizardPageStart = new YahooWizardPageStart();
                this.yahooWizardPageSymbols = new YahooWizardPageSymbols();
                this.yahooWizardPageClassification = new YahooWizardPageClassification();
            }
            this.yahooWizardPageStart.InitStates();
            this.yahooWizardPageSymbols.InitStates();
            this.yahooWizardPageClassification.InitStates(providerDataDir);
            return this.yahooWizardPageStart;
        }

        public override UserControl WizardNextPage(UserControl currentPage)
        {
            if (currentPage == this.yahooWizardPageStart)
            {
                if (this.yahooWizardPageStart.optChooseFromClassification())
                {
                    return this.yahooWizardPageClassification;
                }
                return this.yahooWizardPageSymbols;
            }
            if ((currentPage == this.yahooWizardPageSymbols) && (this.yahooWizardPageSymbols.generateSymbolList().list.Count == 0))
            {
                throw new WizardValidationException("Symbols are not specified");
            }
            if ((currentPage == this.yahooWizardPageClassification) && (this.yahooWizardPageClassification.generateSymbolList().list.Count == 0))
            {
                throw new WizardValidationException("Group is not selected");
            }
            return null;
        }

        public override UserControl WizardPreviousPage(UserControl currentPage)
        {
            if (currentPage == this.yahooWizardPageStart)
            {
                return null;
            }
            if ((currentPage != this.yahooWizardPageSymbols) && (currentPage != this.yahooWizardPageClassification))
            {
                return null;
            }
            return this.yahooWizardPageStart;
        }

        public override bool CanDeleteSymbolDataFile
        {
            get
            {
                Logger.LogParameters(new object[0]);
                return true;
            }
        }

        public override bool CanEditSymbolDataFile
        {
            get
            {
                return VersionContainsUserEditedDates();
            }
        }

        public override bool CanModifySymbols
        {
            get
            {
                return true;
            }
        }

        internal static ClassificationGroupsFileUpdater ClassificationFile
        {
            get
            {
                return classificationGroupsUpdater;
            }
        }

        public static YahooClientSettings ClientSettings
        {
            get
            {
                return yahooClientSettings;
            }
            set
            {
                yahooClientSettings = value;
            }
        }

        public static string DataPath
        {
            get
            {
                return providerDataDir;
            }
        }

        public override BarDataStore DataStore
        {
            get
            {
                return this.barDataStore;
            }
        }

        public override string Description
        {
            get
            {
                return "Yahoo! Finance provides end-of-day data for U.S. and world equities, mutual funds, indices and futures.";
            }
        }

        public override IList<DataBehaviorUserControl> ExtendedBehaviors
        {
            get
            {
                List<DataBehaviorUserControl> list = new List<DataBehaviorUserControl>();
                ProviderSettingsControl item = new ProviderSettingsControl();
                item.SetStates();
                list.Add(item);
                return list;
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Yahoo! Finance";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Yahoo;
            }
        }

        public override string SuggestedDataSourceName
        {
            get
            {
                Logger.LogParameters(new object[0]);
                if (this.yahooWizardPageStart.optChooseFromClassification())
                {
                    return this.yahooWizardPageClassification.SuggestedDataSourceName();
                }
                return base.SuggestedDataSourceName;
            }
        }

        public override bool SupportsDataSourceUpdate
        {
            get
            {
                Logger.LogParameters(new object[0]);
                return true;
            }
        }

        public override bool SupportsProviderUpdate
        {
            get
            {
                Logger.LogParameters(new object[0]);
                return true;
            }
        }

        public override string URL
        {
            get
            {
                return "http://finance.yahoo.com/";
            }
        }
    }
}

