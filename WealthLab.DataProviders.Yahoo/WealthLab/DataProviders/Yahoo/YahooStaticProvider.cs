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
        private BarDataStore barDataStore_0;
        private static Class16 class16_0;
        private Class22 class22_0;
        private Class26 class26_0;
        private Dictionary<string, string> dictionary_0 = new Dictionary<string, string>();
        private IDataUpdateMessage idataUpdateMessage_0;
        private int int_0;
        private int int_1;
        private List<string> list_1 = new List<string>();
        private object object_0 = new object();
        private static string string_0;
        private static YahooClientSettings yahooClientSettings_0 = null;
        private YahooFundamentalProvider yahooFundamentalProvider_0;
        private YahooWizardPageClassification yahooWizardPageClassification_0;
        private YahooWizardPageStart yahooWizardPageStart_0;
        private YahooWizardPageSymbols yahooWizardPageSymbols_0;

        public YahooStaticProvider()
        {
            Class21.smethod_8(new object[0]);
        }

        public static void AddUserEditedDates(Bars toBars, Bars fromBars)
        {
            toBars.UserEditedDates.Clear();
            toBars.UserEditedDates.AddRange(fromBars.UserEditedDates);
        }

        public override void CancelUpdate()
        {
            Class21.smethod_8(new object[0]);
            this.class26_0.method_14();
        }

        public override DataSource CreateDataSource()
        {
            Class21.smethod_8(new object[0]);
            DataSource source = new DataSource(this);
            YahooStaticSettings settings = new YahooStaticSettings();
            if (this.yahooWizardPageStart_0.method_2())
            {
                settings.Symbols = this.yahooWizardPageClassification_0.method_0().ToString();
                settings.Groups = this.yahooWizardPageClassification_0.method_2();
                settings.UpdateGroups = this.yahooWizardPageStart_0.method_3();
            }
            else
            {
                settings.Symbols = this.yahooWizardPageSymbols_0.method_0().ToString();
            }
            settings.StartDate = this.yahooWizardPageStart_0.method_1();
            source.DSString = settings.SerializeToString();
            source.Scale = BarScale.Daily;
            source.BarInterval = 0;
            return source;
        }

        public override void DeleteSymbolDataFile(DataSource dataSource_0, string symbol)
        {
            Class21.smethod_8(new object[0]);
            this.barDataStore_0.RemoveFile(Class23.smethod_1(symbol), dataSource_0.Scale, dataSource_0.BarInterval);
        }

        public override void Initialize(IDataHost dataHost)
        {
            Class21.smethod_8(new object[0]);
            base.Initialize(dataHost);
            this.barDataStore_0 = new BarDataStore(dataHost, this);
            string_0 = this.barDataStore_0.RootPath;
            this.class26_0 = new Class26();
            this.class26_0.method_0(new Class26.Delegate1(this.method_1));
            this.class26_0.method_2(new Class26.Delegate2(this.method_0));
            class16_0 = new Class16(string_0 + "YahooClassification.xml", "http://67.199.28.171/Classification/Yahoo/YahooClassification.xml");
            if (yahooClientSettings_0 == null)
            {
                yahooClientSettings_0 = YahooClientSettings.Deserialize(string_0);
            }
            ServicePointManager.MaxServicePointIdleTime = 0x2710;
            ServicePointManager.UseNagleAlgorithm = true;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.FindServicePoint(new Uri("http://ichart.finance.yahoo.com"));
        }

        private void method_0(object sender, EventArgs3 e)
        {
            Class21.smethod_8(new object[0]);
            if (e.class27_0 != null)
            {
                this.int_1++;
                this.method_4(e.class27_0.method_0(), "Error: " + e.exception_0.Message, Thread.CurrentThread.Name);
                this.method_2();
            }
            else
            {
                this.method_3("Thread (" + Thread.CurrentThread.Name + ") execution error! " + e.exception_0.Message);
            }
        }

        private void method_1(object sender, EventArgs2 e)
        {
            Class21.smethod_8(new object[0]);
            if ((e.class27_0.method_1() & Enum4.flag_0) != 0)
            {
                if (e.bars_0 != null)
                {
                    int num2;
                    lock (this.list_1)
                    {
                        if (this.list_1.Contains(e.class27_0.method_0()))
                        {
                            this.barDataStore_0.RemoveFile(Class23.smethod_1(e.class27_0.method_0()), BarScale.Daily, 0);
                            this.list_1.Remove(e.class27_0.method_0());
                        }
                    }
                    Bars bars = new Bars(Class23.smethod_1(e.class27_0.method_0()), BarScale.Daily, 0);
                    this.barDataStore_0.LoadBarsObject(bars);
                    int count = bars.Count;
                    bars.AppendWithCorrections(e.bars_0, out num2);
                    if (this.dictionary_0.ContainsKey(e.class27_0.method_0()))
                    {
                        string str = this.dictionary_0[e.class27_0.method_0()];
                        if (str != string.Empty)
                        {
                            bars.SecurityName = str.ToUpper();
                        }
                    }
                    lock (this.object_0)
                    {
                        this.barDataStore_0.SaveBarsObject(bars);
                    }
                    this.int_1++;
                    this.method_5(bars, count, num2, Thread.CurrentThread.Name);
                    this.method_2();
                }
                if (e.class28_0 != null)
                {
                    if (this.yahooFundamentalProvider_0 == null)
                    {
                        this.yahooFundamentalProvider_0 = new YahooFundamentalProvider();
                        this.yahooFundamentalProvider_0.Initialize(base.DataHost);
                    }
                    this.yahooFundamentalProvider_0.UpdateData(e.class27_0.method_0(), e.class28_0.method_0(), "Dividend (Yahoo! Finance)");
                    this.yahooFundamentalProvider_0.UpdateData(e.class27_0.method_0(), e.class28_0.method_2(), "Split (Yahoo! Finance)");
                }
            }
        }

        private void method_10(Bars bars_0)
        {
            bars_0.UserEditedDates.Clear();
        }

        private Class19 method_11(string string_1, DateTime dateTime_0)
        {
            if (!yahooClientSettings_0.DividendAdj && !yahooClientSettings_0.SplitAdj)
            {
                return null;
            }
            Enum1 enum2 = Enum1.flag_1;
            if (yahooClientSettings_0.SplitAdj)
            {
                enum2 = Enum1.flag_0;
            }
            if (yahooClientSettings_0.DividendAdj && yahooClientSettings_0.SplitAdj)
            {
                enum2 = Enum1.flag_1 | Enum1.flag_0;
            }
            this.yahooFundamentalProvider_0 = new YahooFundamentalProvider();
            this.yahooFundamentalProvider_0.Initialize(base.DataHost);
            IList<FundamentalItem> list = this.yahooFundamentalProvider_0.RequestItems(string_1, "Split (Yahoo! Finance)");
            IList<FundamentalItem> list2 = this.yahooFundamentalProvider_0.RequestItems(string_1, "Dividend (Yahoo! Finance)");
            if (dateTime_0 != DateTime.MaxValue)
            {
                return new Class19(list, list2, enum2, yahooClientSettings_0.AdjModeWhenDataRange, dateTime_0);
            }
            return new Class19(list, list2, enum2);
        }

        private void method_2()
        {
            if (this.idataUpdateMessage_0 != null)
            {
                this.idataUpdateMessage_0.ReportUpdateProgress((this.int_1 * 100) / this.int_0);
            }
        }

        private void method_3(string string_1)
        {
            if (this.idataUpdateMessage_0 != null)
            {
                this.idataUpdateMessage_0.DisplayUpdateMessage(string_1);
            }
        }

        private void method_4(string string_1, string string_2, string string_3)
        {
            if (this.idataUpdateMessage_0 != null)
            {
                this.idataUpdateMessage_0.DisplayUpdateMessage(string.Format("{0,-4} {1,-9} {2}", "[" + string_3 + "]", string_1, string_2));
            }
        }

        private void method_5(Bars bars_0, int int_2, int int_3, string string_1)
        {
            if (this.idataUpdateMessage_0 != null)
            {
                string str = string.Empty;
                if (bars_0.Count > 0)
                {
                    string str2 = bars_0.Date[bars_0.Count - 1].ToString("MM.dd.yyyy");
                    str = string.Format("{0,-4} {1,-9} {2,-14} {3,-15} {4,-18}", new object[] { "[" + string_1 + "]", bars_0.Symbol, bars_0.Count + " bars", str2, (bars_0.Count - int_2) + " bars added" });
                    if (int_3 > 0)
                    {
                        str = string.Format("{0} {1,-18}", str, int_3 + " bars corrected");
                    }
                }
                else
                {
                    str = string.Format("{0,-4} {1,-9} {2}", "[" + string_1 + "]", bars_0.Symbol, "Error: No data");
                }
                this.idataUpdateMessage_0.DisplayUpdateMessage(str);
            }
        }

        private bool method_6()
        {
            if (yahooClientSettings_0.AdjModeWhenDataRange == AdjustedModeWhenDataRange.Ignore)
            {
                MessageBox.Show("Editing Bars with the \"Ignore splits and dividends which fall out of the range\"\r\noption selected is not possible.\r\n\r\nPlease open Data Manager, change this setting of the Yahoo! provider,\r\nre-open the chart and try again.", "Yahoo! Provider", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        private void method_7(DataSource dataSource_0)
        {
            Class21.smethod_8(new object[0]);
            YahooStaticSettings settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(dataSource_0.DSString);
            if (((this.class22_0 == null) || !class16_0.method_5()) || class16_0.method_8(1))
            {
                if (!class16_0.method_5() || class16_0.method_8(1))
                {
                    this.method_3("Updating the Classification Groups file...");
                    class16_0.method_7(false);
                    if (class16_0.method_4() != null)
                    {
                        this.method_3("Error: " + class16_0.method_4().Message);
                        return;
                    }
                    this.method_3("Classification file updated.");
                }
                this.class22_0 = new Class22(class16_0.method_6());
            }
            this.method_3("Checking the DataSet's Classification groups composition:");
            Class17 class2 = new Class17(settings.Groups, Enum0.const_0);
            Class17 class3 = new Class17(settings.Symbols, Enum0.const_0);
            Class17 class4 = this.class22_0.method_3(class3, class2.list_0.ToArray());
            this.method_3(string.Format("Symbols deleted {0}: {1}", this.class22_0.method_0().list_0.Count, this.class22_0.method_0().ToString()));
            this.method_3(string.Format("Symbols added {0}: {1}", this.class22_0.method_1().list_0.Count, this.class22_0.method_1().ToString()));
            if ((this.class22_0.method_0().list_0.Count > 0) || (this.class22_0.method_1().list_0.Count > 0))
            {
                string str = Directory.GetParent(string_0).Parent.FullName + @"\DataSets\";
                settings.Symbols = class4.ToString();
                dataSource_0.DSString = settings.SerializeToString();
                dataSource_0.SaveToFile(str + SymbolFileNameConverter.SymbolToFileName(dataSource_0.Name) + ".xml");
            }
        }

        private void method_8(ref Class27 class27_0, SymbolInfoList symbolInfoList_0, DateTime dateTime_0)
        {
            DateTime time = dateTime_0;
            DateTime time2 = symbolInfoList_0.Search(class27_0.method_0(), BarScale.Daily, 0);
            if (time < time2)
            {
                class27_0.method_6(time);
                if (time2 != DateTime.MaxValue)
                {
                    this.list_1.Add(class27_0.method_0());
                }
                symbolInfoList_0.Add(class27_0.method_0(), BarScale.Daily, 0, time);
            }
            else
            {
                class27_0.method_6(time2);
                DateTime time3 = this.barDataStore_0.SymbolLastUpdated(Class23.smethod_1(class27_0.method_0()), BarScale.Daily, 0);
                if (time3 != DateTime.MinValue)
                {
                    time = time3.AddDays(-10.0);
                }
            }
            class27_0.method_4(time);
        }

        private void method_9(List<Class27> list_2)
        {
            try
            {
                this.method_3("Updating Security Names for " + list_2.Count + " symbols...");
                this.dictionary_0 = this.class26_0.method_32(list_2);
                Class21.smethod_2("Sec. Names");
                foreach (KeyValuePair<string, string> pair in this.dictionary_0)
                {
                    Class21.smethod_2(pair.Key + " " + pair.Value);
                }
                Class21.smethod_2("-----");
                this.method_3("Security Names updated.");
            }
            catch (Exception exception)
            {
                this.method_3("Error updating Security Names. " + exception.Message);
            }
        }

        public override string ModifySymbols(DataSource dataSource_0, List<string> symbols)
        {
            Class21.smethod_8(new object[0]);
            Class17 class2 = new Class17(symbols);
            YahooStaticSettings settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(dataSource_0.DSString);
            settings.Symbols = class2.ToString().ToUpper();
            return settings.SerializeToString();
        }

        public override void PopulateSymbols(DataSource dataSource_0, List<string> symbols)
        {
            Class21.smethod_8(new object[] { dataSource_0.Name });
            if (!string.IsNullOrEmpty(dataSource_0.Name))
            {
                YahooStaticSettings settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(dataSource_0.DSString);
                Class17 class2 = new Class17(settings.Symbols, Enum0.const_0);
                symbols.AddRange(class2.list_0);
            }
        }

        public override Bars RequestData(DataSource dataSource_0, string symbol, DateTime startDate, DateTime symbolEndDate, int maxBars, bool includePartialBar)
        {
            Class21.smethod_7(new object[] { dataSource_0.DSString, symbol, startDate, symbolEndDate, maxBars, includePartialBar });
            symbol = symbol.Trim(new char[] { ' ', '"' });
            Class21.smethod_2("On Demand Update Enabled: " + base.DataHost.OnDemandUpdateEnabled);
            if (base.DataHost.OnDemandUpdateEnabled && !ClientSettings.NeverPerformOnDemandUpdates)
            {
                try
                {
                    YahooStaticSettings settings = new YahooStaticSettings();
                    if (dataSource_0.DSString != string.Empty)
                    {
                        settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(dataSource_0.DSString);
                    }
                    SymbolInfoList list = SymbolInfoList.Deserialize(string_0 + "SymbolsStartDate.xml");
                    list.Synchronize(this.barDataStore_0);
                    Class27 class2 = new Class27(symbol);
                    this.method_8(ref class2, list, settings.StartDate);
                    class2.method_8(DateTime.Now.AddDays(2.0));
                    class2.method_2(Enum4.flag_1 | Enum4.flag_0);
                    if (includePartialBar)
                    {
                        class2.method_2(class2.method_1() | Enum4.flag_2);
                    }
                    List<Class27> list2 = new List<Class27> {
                        class2
                    };
                    this.method_9(list2);
                    this.class26_0.method_27(list2);
                }
                catch (Exception exception)
                {
                    string str = "On Demand Update Error: " + exception.Message;
                    Class21.smethod_4(Enum2.const_3, str);
                    MessageBox.Show(str, "On Demand Update Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            Bars bars2 = new Bars(Class23.smethod_1(symbol), dataSource_0.Scale, dataSource_0.BarInterval);
            if (this.barDataStore_0.ContainsSymbol(Class23.smethod_1(symbol), dataSource_0.Scale, dataSource_0.BarInterval))
            {
                this.barDataStore_0.LoadBarsObject(bars2, startDate, DateTime.MaxValue, maxBars);
            }
            if (symbol != Class23.smethod_1(symbol))
            {
                Bars toBars = new Bars(symbol, dataSource_0.Scale, dataSource_0.BarInterval);
                toBars.Append(bars2);
                toBars.SecurityName = bars2.SecurityName;
                if (VersionContainsUserEditedDates())
                {
                    AddUserEditedDates(toBars, bars2);
                }
                bars2 = toBars;
            }
            Class19 class3 = this.method_11(symbol, symbolEndDate);
            if (class3 != null)
            {
                bars2 = class3.method_4(bars2);
            }
            return bars2;
        }

        public Bars RequestHistoricalData(string symbol, DateTime startDate, DateTime endDate)
        {
            return this.class26_0.method_29(symbol, startDate, endDate);
        }

        public override void SaveEditedSymbolDataFile(DataSource dataSource_0, Bars bars)
        {
            Bars bars2 = new Bars(bars.Symbol, bars.Scale, bars.BarInterval);
            Class19 class2 = this.method_11(bars.Symbol, DateTime.MaxValue);
            this.barDataStore_0.LoadBarsObject(bars2);
            if (bars.Count >= bars2.Count)
            {
                if (bars.Count > bars2.Count)
                {
                    if ((class2 != null) && !this.method_6())
                    {
                        return;
                    }
                    foreach (DateTime time in bars.UserEditedDates)
                    {
                        if (bars2.ConvertDateToBar(time, true) == -1)
                        {
                            int num5 = bars.ConvertDateToBar(time, true);
                            if (class2 != null)
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
                                class2.method_4(bars);
                                Class20 class4 = class2.method_3(time);
                                (series = bars.Open)[num6 = num5] = series[num6] / class4.method_2();
                                (series2 = bars.High)[num7 = num5] = series2[num7] / class4.method_2();
                                (series3 = bars.Low)[num8 = num5] = series3[num8] / class4.method_2();
                                (series4 = bars.Close)[num9 = num5] = series4[num9] / class4.method_2();
                                (series5 = bars.Volume)[num10 = num5] = series5[num10] / class4.method_4();
                            }
                        }
                    }
                    bars2 = bars;
                }
                else if (class2 == null)
                {
                    bars2 = bars;
                }
                else
                {
                    if (!this.method_6())
                    {
                        return;
                    }
                    class2.method_4(bars2);
                    int num2 = 0;
                    for (int i = 0; i < bars.Count; i++)
                    {
                        class2.method_3(bars.Date[i]);
                        if (((bars2.Open[i] != bars.Open[i]) || (bars2.High[i] != bars.High[i])) || (((bars2.Low[i] != bars.Low[i]) || (bars2.Close[i] != bars.Close[i])) || (bars2.Volume[i] != bars.Volume[i])))
                        {
                            num2++;
                        }
                    }
                    if (num2 == 1)
                    {
                        foreach (DateTime time2 in bars.UserEditedDates)
                        {
                            Class20 class5 = class2.method_3(time2);
                            int num11 = bars.ConvertDateToBar(time2, true);
                            if (((bars2.Open[num11] != bars.Open[num11]) || (bars2.High[num11] != bars.High[num11])) || (((bars2.Low[num11] != bars.Low[num11]) || (bars2.Close[num11] != bars.Close[num11])) || (bars2.Volume[num11] != bars.Volume[num11])))
                            {
                                bars2.Open[num11] = bars.Open[num11] / class5.method_2();
                                bars2.High[num11] = bars.High[num11] / class5.method_2();
                                bars2.Low[num11] = bars.Low[num11] / class5.method_2();
                                bars2.Close[num11] = bars.Close[num11] / class5.method_2();
                                bars2.Volume[num11] = bars.Volume[num11] / class5.method_4();
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < bars.Count; j++)
                        {
                            Class20 class3 = class2.method_3(bars.Date[j]);
                            bars.Open[j] /= class3.method_2();
                            bars.High[j] /= class3.method_2();
                            bars.Low[j] /= class3.method_2();
                            bars.Close[j] /= class3.method_2();
                            bars.Volume[j] /= class3.method_4();
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
            this.barDataStore_0.SaveBarsObject(bars2);
        }

        public override bool SupportsDynamicUpdate(BarScale scale)
        {
            Class21.smethod_8(new object[0]);
            return false;
        }

        public override void UpdateDataSource(DataSource dataSource_0, IDataUpdateMessage dataUpdateMsg)
        {
            Class21.smethod_8(new object[] { dataSource_0.Name, dataSource_0.Symbols[0] });
            this.class26_0.method_10(false);
            SymbolInfoList list = null;
            this.idataUpdateMessage_0 = dataUpdateMsg;
            try
            {
                Class17 class2;
                YahooStaticSettings settings = null;
                if (!string.IsNullOrEmpty(dataSource_0.Name))
                {
                    settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(dataSource_0.DSString);
                    if (settings.UpdateGroups)
                    {
                        this.method_7(dataSource_0);
                        settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(dataSource_0.DSString);
                    }
                    class2 = new Class17(settings.Symbols, Enum0.const_0);
                }
                else
                {
                    class2 = new Class17(dataSource_0.Symbols);
                }
                list = SymbolInfoList.Deserialize(string_0 + "SymbolsStartDate.xml");
                list.Synchronize(this.barDataStore_0);
                List<Class27> list2 = new List<Class27>();
                this.method_3("Preparing requests ...");
                foreach (string str in class2.list_0)
                {
                    Class27 class3 = new Class27(str);
                    if (!string.IsNullOrEmpty(dataSource_0.Name))
                    {
                        this.method_8(ref class3, list, settings.StartDate);
                    }
                    else
                    {
                        this.method_8(ref class3, list, new DateTime(0x7d0, 1, 1));
                    }
                    class3.method_8(DateTime.Now.AddDays(2.0));
                    class3.method_2(Enum4.flag_1 | Enum4.flag_0);
                    list2.Add(class3);
                    if (this.class26_0.method_9())
                    {
                        return;
                    }
                }
                this.method_3("Requests are ready to go.");
                if (list2.Count > 0)
                {
                    this.int_0 = list2.Count;
                    this.int_1 = 0;
                    this.method_9(list2);
                    this.class26_0.method_27(list2);
                }
            }
            catch (Exception exception)
            {
                Class21.smethod_4(Enum2.const_3, "UpdateDataSource " + exception.Message);
                this.idataUpdateMessage_0.DisplayUpdateMessage("Error: " + exception.Message);
            }
            finally
            {
                list.Serialize(string_0 + "SymbolsStartDate.xml");
                this.idataUpdateMessage_0 = null;
            }
        }

        public override void UpdateProvider(IDataUpdateMessage dataUpdateMsg, List<DataSource> dataSources, bool updateNonDSSymbols, bool deleteNonDSSymbols)
        {
            Class21.smethod_8(new object[0]);
            this.class26_0.method_10(false);
            this.idataUpdateMessage_0 = dataUpdateMsg;
            List<Class27> list = new List<Class27>();
            SymbolInfoList list2 = null;
            try
            {
                list2 = SymbolInfoList.Deserialize(string_0 + "SymbolsStartDate.xml");
                list2.Synchronize(this.barDataStore_0);
                this.method_3("Preparing requests ...");
                foreach (DataSource source in dataSources)
                {
                    YahooStaticSettings settings = (YahooStaticSettings) DataSetSettings.DeserializeFromString(source.DSString);
                    Class17 class3 = new Class17(settings.Symbols, Enum0.const_0);
                    foreach (string str3 in class3.list_0)
                    {
                        Class27 class4 = new Class27(str3);
                        this.method_8(ref class4, list2, settings.StartDate);
                        class4.method_8(DateTime.Now.AddDays(2.0));
                        class4.method_2(Enum4.flag_1 | Enum4.flag_0);
                        for (int i = list.Count - 1; i >= 0; i--)
                        {
                            if (list[i].method_0() == class4.method_0())
                            {
                                list.RemoveAt(i);
                            }
                        }
                        list.Add(class4);
                        if (this.class26_0.method_9())
                        {
                            return;
                        }
                    }
                }
                if (updateNonDSSymbols)
                {
                    IList<string> existingSymbols = this.barDataStore_0.GetExistingSymbols(BarScale.Daily, 0);
                    Class21.smethod_2("symbolsInDataStore.Count " + existingSymbols.Count);
                    foreach (string str4 in existingSymbols)
                    {
                        bool flag2 = true;
                        for (int j = list.Count - 1; j >= 0; j--)
                        {
                            if (list[j].method_0() == str4)
                            {
                                goto Label_01E9;
                            }
                        }
                        goto Label_01EC;
                    Label_01E9:
                        flag2 = false;
                    Label_01EC:
                        if (flag2)
                        {
                            Class27 item = new Class27(str4);
                            DateTime time2 = this.barDataStore_0.SymbolLastUpdated(Class23.smethod_1(item.method_0()), BarScale.Daily, 0);
                            item.method_4(time2.AddDays(-10.0));
                            item.method_8(DateTime.Now.AddDays(2.0));
                            item.method_2(Enum4.flag_1 | Enum4.flag_0);
                            list.Add(item);
                        }
                        if (this.class26_0.method_9())
                        {
                            return;
                        }
                    }
                }
                list.Sort();
                this.method_3("Requests are ready to go.");
                if (list.Count > 0)
                {
                    this.int_0 = list.Count;
                    this.int_1 = 0;
                    this.method_9(list);
                    this.class26_0.method_27(list);
                }
                if (deleteNonDSSymbols)
                {
                    IList<string> list4 = this.barDataStore_0.GetExistingSymbols(BarScale.Daily, 0);
                    string str = string.Empty;
                    int num = 0;
                    foreach (string str2 in list4)
                    {
                        bool flag = false;
                        using (List<Class27>.Enumerator enumerator4 = list.GetEnumerator())
                        {
                            while (enumerator4.MoveNext())
                            {
                                Class27 current = enumerator4.Current;
                                if (current.method_0() == str2)
                                {
                                    goto Label_032F;
                                }
                            }
                            goto Label_0342;
                        Label_032F:
                            flag = true;
                        }
                    Label_0342:
                        if (!flag)
                        {
                            num++;
                            this.barDataStore_0.RemoveFile(Class23.smethod_1(str2), BarScale.Daily, 0);
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
                Class21.smethod_4(Enum2.const_3, "UpdateDataSource " + exception.Message);
                this.idataUpdateMessage_0.DisplayUpdateMessage("Error: " + exception.Message);
            }
            finally
            {
                list2.Serialize(string_0 + "SymbolsStartDate.xml");
                this.idataUpdateMessage_0 = null;
            }
        }

        public static bool VersionContainsUserEditedDates()
        {
            return (Assembly.GetEntryAssembly().GetName().Version >= new Version("5.3"));
        }

        public override UserControl WizardFirstPage()
        {
            if (this.yahooWizardPageStart_0 == null)
            {
                this.yahooWizardPageStart_0 = new YahooWizardPageStart();
                this.yahooWizardPageSymbols_0 = new YahooWizardPageSymbols();
                this.yahooWizardPageClassification_0 = new YahooWizardPageClassification();
            }
            this.yahooWizardPageStart_0.method_0();
            this.yahooWizardPageSymbols_0.method_1();
            this.yahooWizardPageClassification_0.method_8(string_0);
            return this.yahooWizardPageStart_0;
        }

        public override UserControl WizardNextPage(UserControl currentPage)
        {
            if (currentPage == this.yahooWizardPageStart_0)
            {
                if (this.yahooWizardPageStart_0.method_2())
                {
                    return this.yahooWizardPageClassification_0;
                }
                return this.yahooWizardPageSymbols_0;
            }
            if ((currentPage == this.yahooWizardPageSymbols_0) && (this.yahooWizardPageSymbols_0.method_0().list_0.Count == 0))
            {
                throw new WizardValidationException("Symbols are not specified");
            }
            if ((currentPage == this.yahooWizardPageClassification_0) && (this.yahooWizardPageClassification_0.method_0().list_0.Count == 0))
            {
                throw new WizardValidationException("Group is not selected");
            }
            return null;
        }

        public override UserControl WizardPreviousPage(UserControl currentPage)
        {
            if (currentPage == this.yahooWizardPageStart_0)
            {
                return null;
            }
            if ((currentPage != this.yahooWizardPageSymbols_0) && (currentPage != this.yahooWizardPageClassification_0))
            {
                return null;
            }
            return this.yahooWizardPageStart_0;
        }

        public override bool CanDeleteSymbolDataFile
        {
            get
            {
                Class21.smethod_8(new object[0]);
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

        internal static Class16 ClassificationFile
        {
            get
            {
                return class16_0;
            }
        }

        public static YahooClientSettings ClientSettings
        {
            get
            {
                return yahooClientSettings_0;
            }
            set
            {
                yahooClientSettings_0 = value;
            }
        }

        public static string DataPath
        {
            get
            {
                return string_0;
            }
        }

        public override BarDataStore DataStore
        {
            get
            {
                return this.barDataStore_0;
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
                item.method_1();
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
                Class21.smethod_8(new object[0]);
                if (this.yahooWizardPageStart_0.method_2())
                {
                    return this.yahooWizardPageClassification_0.method_1();
                }
                return base.SuggestedDataSourceName;
            }
        }

        public override bool SupportsDataSourceUpdate
        {
            get
            {
                Class21.smethod_8(new object[0]);
                return true;
            }
        }

        public override bool SupportsProviderUpdate
        {
            get
            {
                Class21.smethod_8(new object[0]);
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

