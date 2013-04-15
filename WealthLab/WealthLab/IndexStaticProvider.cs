namespace WealthLab
{
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab.Properties;

    public class IndexStaticProvider : StaticDataProvider
    {
        private BarDataStore barDataStore_0;
        private volatile bool bool_1;
        private bool bool_2 = true;
        private static readonly ILog ilog_0 = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int int_0;

        public override void CancelUpdate()
        {
            this.bool_1 = true;
        }

        public override DataSource CreateDataSource()
        {
            return null;
        }

        public override void DeleteSymbolDataFile(DataSource dataSource_0, string symbol)
        {
            this.barDataStore_0.RemoveFile(symbol, dataSource_0.Scale, dataSource_0.BarInterval);
        }

        public override MarketInfo GetMarketInfo(string symbol)
        {
            CustomIndex index = CustomIndexManager.Instance.FindCustomIndex(symbol);
            if (index != null)
            {
                DataSource dataSource = index.DataSource;
                if ((dataSource != null) && (dataSource.Symbols.Count > 0))
                {
                    return dataSource.Provider.GetMarketInfo(dataSource.Symbols[0]);
                }
            }
            return new MarketHours().Market;
        }

        public override void Initialize(IDataHost dataHost)
        {
            base.Initialize(dataHost);
            this.barDataStore_0 = new BarDataStore(dataHost, this);
        }

        private void method_0()
        {
            if (this.barDataStore_0 == null)
            {
                this.barDataStore_0 = new BarDataStore(base.DataHost, this);
            }
        }

        private List<Bars> method_1(CustomIndex customIndex_0, bool bool_3)
        {
            List<Bars> list = new List<Bars>();
            DataSource dataSource = customIndex_0.DataSource;
            foreach (string str in dataSource.Symbols)
            {
                Bars item = dataSource.Provider.RequestData(dataSource, str, DateTime.MinValue, DateTime.MaxValue, 0, bool_3);
                list.Add(item);
            }
            return list;
        }

        private Bars method_2(CustomIndex customIndex_0, List<Bars> list_1, DateTime dateTime_0)
        {
            IndexDefinition indexDefinition = customIndex_0.IndexDefinition;
            indexDefinition.ParameterString = customIndex_0.Parameters;
            return this.method_3(list_1, customIndex_0.Symbol, indexDefinition, customIndex_0.Scale, customIndex_0.BarInterval, dateTime_0);
        }

        private Bars method_3(List<Bars> list_1, string string_0, IndexDefinition indexDefinition_0, BarScale barScale_0, int int_1, DateTime dateTime_0)
        {
            try
            {
                if (indexDefinition_0 == null)
                {
                    return null;
                }
                List<int> list3 = new List<int>();
                if (dateTime_0 == DateTime.MinValue)
                {
                    for (int j = 0; j < list_1.Count; j++)
                    {
                        list3.Add(0);
                    }
                }
                else
                {
                    for (int k = 0; k < list_1.Count; k++)
                    {
                        int num6;
                        Bars bars3 = list_1[k];
                        if (bars3.Date[0] >= dateTime_0)
                        {
                            num6 = 0;
                        }
                        else if (dateTime_0 == bars3.Date[bars3.Count - 1])
                        {
                            num6 = bars3.Count - 1;
                        }
                        else if (dateTime_0 > bars3.Date[bars3.Count - 1])
                        {
                            num6 = bars3.Count;
                        }
                        else
                        {
                            TimeSpan span = (TimeSpan) (dateTime_0 - bars3.Date[0]);
                            TimeSpan span2 = bars3.Date[bars3.Count - 1] - dateTime_0;
                            if (span < span2)
                            {
                                num6 = 0;
                                while (bars3.Date[num6] < dateTime_0)
                                {
                                    num6++;
                                }
                            }
                            else
                            {
                                num6 = bars3.Count - 1;
                                while (bars3.Date[num6 - 1] >= dateTime_0)
                                {
                                    num6--;
                                }
                            }
                        }
                        list3.Add(num6);
                    }
                }
                Bars bars = new Bars(string_0, barScale_0, int_1);
                this.barDataStore_0.LoadBarsObject(bars);
                int count = bars.Count;
                DateTime time3 = dateTime_0;
                int num3 = -1;
                bool flag = false;
                while (true)
                {
                    if (!(time3 != DateTime.MaxValue))
                    {
                        break;
                    }
                    List<Bars> list = new List<Bars>();
                    List<int> barNumbers = new List<int>();
                    DateTime maxValue = DateTime.MaxValue;
                    for (int m = 0; m < list_1.Count; m++)
                    {
                        int item = list3[m];
                        Bars bars4 = list_1[m];
                        if (item < bars4.Count)
                        {
                            DateTime time2 = bars4.Date[item];
                            if (time2 == time3)
                            {
                                List<int> list4;
                                int num8;
                                list.Add(bars4);
                                barNumbers.Add(item);
                                (list4 = list3)[num8 = m] = list4[num8] + 1;
                                item = list3[m];
                                if (item < bars4.Count)
                                {
                                    time2 = bars4.Date[item];
                                }
                            }
                            if (time2 < maxValue)
                            {
                                maxValue = time2;
                            }
                        }
                    }
                    if (list.Count > 0)
                    {
                        try
                        {
                            if (!flag)
                            {
                                num3++;
                            }
                            indexDefinition_0.SetIndexValues(list, barNumbers, time3, bars);
                            if (!flag)
                            {
                                flag = true;
                            }
                        }
                        catch
                        {
                            if (!flag)
                            {
                                bars.Add(time3, 0.0, 0.0, 0.0, 0.0, 0.0);
                            }
                        }
                    }
                    time3 = maxValue;
                }
                for (int i = 0; i < num3; i++)
                {
                    if ((count < 0) || (count >= bars.Count))
                    {
                        break;
                    }
                    bars.Delete(count);
                }
                return bars;
            }
            catch
            {
                return null;
            }
        }

        private Bars method_4(CustomIndex customIndex_0)
        {
            return this.method_5(customIndex_0, this.method_1(customIndex_0, false));
        }

        private Bars method_5(CustomIndex customIndex_0, List<Bars> list_1)
        {
            DateTime minValue;
            Bars bars = new Bars(customIndex_0.Symbol, customIndex_0.Scale, customIndex_0.BarInterval);
            this.barDataStore_0.LoadBarsObject(bars);
            int count = bars.Count;
            if (bars.Count == 0)
            {
                minValue = DateTime.MinValue;
            }
            else
            {
                minValue = bars.Date[bars.Count - 1];
            }
            Bars bars2 = this.method_2(customIndex_0, list_1, minValue);
            if ((bars2 != null) && (bars2.Count > 0))
            {
                if (count == 0)
                {
                    bars = bars2;
                }
                else
                {
                    bars.Append(bars2);
                }
            }
            this.barDataStore_0.SaveBarsObject(bars);
            this.int_0 = bars.Count - count;
            return bars;
        }

        private DateTime method_6(CustomIndex customIndex_0)
        {
            return this.barDataStore_0.SymbolLastUpdated(customIndex_0.Symbol, customIndex_0.Scale, customIndex_0.BarInterval);
        }

        private bool method_7(CustomIndex customIndex_0)
        {
            bool flag;
            if (customIndex_0 == null)
            {
                return false;
            }
            if (!this.barDataStore_0.ContainsSymbol(customIndex_0.Symbol, customIndex_0.Scale, customIndex_0.BarInterval))
            {
                return true;
            }
            DateTime time2 = this.method_6(customIndex_0);
            if (time2 == DateTime.MinValue)
            {
                return true;
            }
            DataSource dataSource = customIndex_0.DataSource;
            using (List<string>.Enumerator enumerator = dataSource.Symbols.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    if (dataSource.Provider.DataStore != null)
                    {
                        DateTime time = dataSource.Provider.DataStore.SymbolLastUpdated(current, dataSource.Scale, dataSource.BarInterval);
                        if (time2 < time)
                        {
                            ///goto  Label_009D;  ///WYJ fix, simplify the flow
                            flag = true;
                            return flag;
                        }
                    }
                }
                return false;
            }
        }

        private void method_8(IDataUpdateMessage idataUpdateMessage_0, Bars bars_0)
        {
            string str;
            if (bars_0.Count > 0)
            {
                str = bars_0.Date[bars_0.Count - 1].ToShortDateString();
                if (bars_0.IsIntraday)
                {
                    str = str + " " + bars_0.Date[bars_0.Count - 1].ToShortTimeString();
                }
            }
            else
            {
                str = "No data";
            }
            string message = string.Concat(new object[] { bars_0.Symbol, "\t", bars_0.Count, " bars\t\t", str });
            if (this.int_0 >= 0)
            {
                object obj2 = message;
                message = string.Concat(new object[] { obj2, "\t\t", this.int_0, " bars added" });
            }
            idataUpdateMessage_0.DisplayUpdateMessage(message);
        }

        public override string ModifySymbols(DataSource dataSource_0, List<string> symbols)
        {
            List<string> list = new List<string>();
            foreach (string str2 in dataSource_0.Symbols)
            {
                if (!symbols.Contains(str2))
                {
                    list.Add(str2);
                    this.DeleteSymbolDataFile(dataSource_0, str2);
                    string str3 = this.RootDataLocation + CustomIndex.FolderName + @"\" + str2;
                    if (File.Exists(str3 + ".xml"))
                    {
                        File.Delete(str3 + ".xml");
                    }
                }
            }
            CustomIndexManager.Instance.method_0(list);
            StringBuilder builder = new StringBuilder();
            foreach (string str in symbols)
            {
                builder.Append(str);
                builder.Append(",");
            }
            return builder.ToString();
        }

        public override void PopulateSymbols(DataSource dataSource_0, List<string> symbols)
        {
            this.method_0();
            List<string> existingSymbols = this.barDataStore_0.GetExistingSymbols(dataSource_0.Scale, dataSource_0.BarInterval);
            symbols.AddRange(existingSymbols);
        }

        public override Bars RequestData(DataSource dataSource_0, string symbol, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar)
        {
            this.method_0();
            symbol = symbol.ToUpper();
            BarScale scale = dataSource_0.Scale;
            int barInterval = dataSource_0.BarInterval;
            this.int_0 = 0;
            CustomIndex index = CustomIndexManager.Instance.FindCustomIndex(symbol);
            if (index == null)
            {
                return new Bars(symbol, scale, barInterval);
            }
            List<Bars> list = this.method_1(index, includePartialBar);
            if (this.method_7(index))
            {
                try
                {
                    this.method_5(index, list);
                }
                catch (ThreadAbortException exception)
                {
                    ilog_0.Error("Abort called on thread.  A user running a strategy could want to abort the thread.", exception);
                }
                catch (OutOfMemoryException exception2)
                {
                    if (!this.bool_2)
                    {
                        throw exception2;
                    }
                    MessageBox.Show("Out of memory:\nUnable to get Data for index: " + symbol + ".\nTry closing applications or running fewer tasks.");
                    ilog_0.Error("Memory error: ", exception2);
                }
                catch (Exception exception3)
                {
                    if (!this.bool_2)
                    {
                        throw exception3;
                    }
                    MessageBox.Show(exception3.Message + "\nUnable to get Data for index: " + symbol, Application.ProductName);
                    ilog_0.Error("Request Data Error: ", exception3);
                }
            }
            Bars bars = new Bars(symbol, scale, barInterval);
            this.barDataStore_0.LoadBarsObject(bars, startDate, endDate, maxBars);
            return bars;
        }

        public override void RequestUpdates(List<string> symbols, DateTime startDate, DateTime endDate, BarScale scale, int barInterval, IUpdateRequestCompleted requestCompleted)
        {
            List<string> list = new List<string>();
            foreach (string str in symbols)
            {
                if (this.bool_1)
                {
                    return;
                }
                try
                {
                    CustomIndex index = CustomIndexManager.Instance.FindCustomIndex(str);
                    DataSource dataSource = index.DataSource;
                    Class32 class2 = new Class32(scale, barInterval);
                    dataSource.Provider.RequestUpdates(dataSource.Symbols, startDate, endDate, scale, barInterval, class2);
                    IndexDefinition indexDefinition = index.IndexDefinition;
                    indexDefinition.ParameterString = index.Parameters;
                    Bars bars = this.method_3(class2.method_0(), index.Symbol, indexDefinition, scale, barInterval, DateTime.MinValue);
                    requestCompleted.UpdateCompleted(bars);
                }
                catch
                {
                    list.Add(str);
                }
            }
            foreach (string str2 in list)
            {
                Bars bars2 = new Bars(str2, scale, barInterval);
                requestCompleted.UpdateCompleted(bars2);
            }
            requestCompleted.ProcessingCompleted();
        }

        public override void SaveEditedSymbolDataFile(DataSource dataSource_0, Bars bars)
        {
            this.barDataStore_0.SaveBarsObject(bars);
        }

        public override bool SupportsDynamicUpdate(BarScale scale)
        {
            return false;
        }

        public override void UpdateDataSource(DataSource dataSource_0, IDataUpdateMessage dataUpdateMsg)
        {
            if (dataSource_0.IsIndexLabDataset)
            {
                this.bool_1 = false;
                int num2 = 0;
                int num5 = 0;
                this.bool_2 = false;
                try
                {
                    List<CustomIndex> list = new List<CustomIndex>();
                    List<CustomIndex> list2 = new List<CustomIndex>();
                    List<CustomIndex> list3 = new List<CustomIndex>();
                    foreach (string str in dataSource_0.Symbols)
                    {
                        try
                        {
                            CustomIndex index2 = CustomIndexManager.Instance.FindCustomIndex(str);
                            DataSource dataSource = index2.DataSource;
                            dataSource.Provider.UpdateDataSource(dataSource, dataUpdateMsg);
                            if (this.barDataStore_0.ContainsSymbol(index2.Symbol, index2.Scale, index2.BarInterval) && (this.method_6(index2).Date != DateTime.MinValue))
                            {
                                if (this.method_7(index2))
                                {
                                    list3.Add(index2);
                                }
                                else
                                {
                                    list.Add(index2);
                                }
                            }
                            else
                            {
                                list2.Add(index2);
                            }
                        }
                        catch (Exception exception)
                        {
                            dataUpdateMsg.DisplayUpdateMessage(str + "\tError: " + exception.Message);
                            ilog_0.Error("Error: ", exception);
                        }
                    }
                    if (list.Count > 0)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append(list[0].Symbol);
                        for (int j = 1; j < list.Count; j++)
                        {
                            builder.Append(",");
                            builder.Append(list[j].Symbol);
                        }
                        dataUpdateMsg.DisplayUpdateMessage("Symbols already up to date: " + builder.ToString());
                        num2 = list.Count;
                        dataUpdateMsg.ReportUpdateProgress((num2 * 100) / dataSource_0.Symbols.Count);
                    }
                    int count = list2.Count;
                    foreach (CustomIndex index3 in list3)
                    {
                        if (this.bool_1)
                        {
                            break;
                        }
                        dataUpdateMsg.DisplayUpdateMessage("Requesting updates for " + index3.Symbol);
                        this.int_0 = 0;
                        try
                        {
                            Bars bars2 = this.method_4(index3);
                            this.method_8(dataUpdateMsg, bars2);
                            if (bars2.Count == 0)
                            {
                                list2.Add(index3);
                                num5++;
                            }
                            else
                            {
                                num2++;
                                dataUpdateMsg.ReportUpdateProgress((num2 * 100) / dataSource_0.Symbols.Count);
                            }
                        }
                        catch (IndexOutOfRangeException exception6)
                        {
                            dataUpdateMsg.DisplayUpdateMessage(index3.Symbol + "\tInternal Error processing: " + index3.Symbol);
                            ilog_0.Error("Index Error: ", exception6);
                        }
                        catch (ArgumentOutOfRangeException exception7)
                        {
                            dataUpdateMsg.DisplayUpdateMessage(index3.Symbol + "\tInternal Error processing: " + index3.Symbol);
                            ilog_0.Error("Argument Error: ", exception7);
                        }
                        catch (OutOfMemoryException exception8)
                        {
                            dataUpdateMsg.DisplayUpdateMessage(index3.Symbol + "\tOut of Memory to process request.\nTry closing applications or running fewer tasks.");
                            ilog_0.Error("Memory Error: ", exception8);
                        }
                        catch (Exception exception9)
                        {
                            dataUpdateMsg.DisplayUpdateMessage(index3.Symbol + "\tError Processing: " + index3.Symbol);
                            ilog_0.Error("Error: ", exception9);
                        }
                    }
                    for (int i = 0; (i < list2.Count) && !this.bool_1; i++)
                    {
                        CustomIndex index = list2[i];
                        try
                        {
                            if ((num5 > 0) && (i == count))
                            {
                                dataUpdateMsg.DisplayUpdateMessage("Re-requesting updates for " + num5.ToString() + " indices");
                            }
                            Bars bars = this.RequestData(dataSource_0, index.Symbol, DateTime.MinValue, DateTime.MaxValue, 0, false);
                            this.method_8(dataUpdateMsg, bars);
                        }
                        catch (IndexOutOfRangeException exception2)
                        {
                            dataUpdateMsg.DisplayUpdateMessage(index.Symbol + "\tInternal Error requesting: " + num5.ToString());
                            ilog_0.Error("Error: ", exception2);
                        }
                        catch (ArgumentOutOfRangeException exception3)
                        {
                            dataUpdateMsg.DisplayUpdateMessage(index.Symbol + "\tInternal Error requesting: " + num5.ToString());
                            ilog_0.Error("Error: ", exception3);
                        }
                        catch (OutOfMemoryException exception4)
                        {
                            dataUpdateMsg.DisplayUpdateMessage(index.Symbol + "\tOut of Memory to process request.\nTry closing applications or running fewer tasks.");
                            ilog_0.Error("Error: ", exception4);
                        }
                        catch (Exception exception5)
                        {
                            dataUpdateMsg.DisplayUpdateMessage(index.Symbol + "\tError: " + exception5.Message);
                            ilog_0.Error("Error: ", exception5);
                        }
                        num2++;
                        dataUpdateMsg.ReportUpdateProgress((num2 * 100) / dataSource_0.Symbols.Count);
                    }
                }
                finally
                {
                    this.bool_2 = true;
                }
            }
        }

        public override void UpdateProvider(IDataUpdateMessage dataUpdateMsg, List<DataSource> dataSources, bool updateNonDSSymbols, bool deleteNonDSSymbols)
        {
            this.bool_1 = false;
            List<DataSource> list = new List<DataSource>();
            if (dataSources != null)
            {
                foreach (DataSource source2 in dataSources)
                {
                    if (source2.IsIndexLabDataset)
                    {
                        list.Add(source2);
                    }
                }
            }
            foreach (DataSource source in list)
            {
                if (this.bool_1)
                {
                    break;
                }
                dataUpdateMsg.DisplayUpdateMessage("Updating " + source.Name + " data ...");
                this.UpdateDataSource(source, dataUpdateMsg);
                dataUpdateMsg.DisplayUpdateMessage("");
            }
        }

        public override UserControl WizardFirstPage()
        {
            return null;
        }

        public override UserControl WizardNextPage(UserControl currentPage)
        {
            return null;
        }

        public override UserControl WizardPreviousPage(UserControl currentPage)
        {
            return null;
        }

        public override bool CanDeleteSymbolDataFile
        {
            get
            {
                return true;
            }
        }

        public override bool CanEditSymbolDataFile
        {
            get
            {
                return true;
            }
        }

        public override bool CanModifySymbols
        {
            get
            {
                return true;
            }
        }

        public override bool CanRequestUpdates
        {
            get
            {
                return true;
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
                return "Indices and Aggregate Indicators defined by the Index Manager tool.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Custom Indexes";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.wl_index;
            }
        }

        public override bool InternalUseOnly
        {
            get
            {
                return true;
            }
        }

        private string RootDataLocation
        {
            get
            {
                if (!base.DataHost.BaseDataFolder.EndsWith(@"\"))
                {
                    return (base.DataHost.BaseDataFolder + @"\");
                }
                return base.DataHost.BaseDataFolder;
            }
        }

        public override bool SupportsDataSourceUpdate
        {
            get
            {
                return true;
            }
        }

        public override bool SupportsProviderUpdate
        {
            get
            {
                return true;
            }
        }
    }
}

