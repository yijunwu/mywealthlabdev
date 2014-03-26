using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WealthLab;

namespace WLDDatabaseProvider
{
    class DatabaseProvider : StaticDataProvider
    {
        private BarDataStore barDataStore;

        private IDataUpdateMessage idataUpdateMessage;

        private DataProviderUserControl userControl = null;
        public override DataSource CreateDataSource()
        {
            DataSource ds = new DataSource(this);
            ds.BarDataScale = new BarDataScale(BarScale.Daily, 1);

            DBProviderSettings settings = new DBProviderSettings();

            //if (this.userControl != null)
            //    settings.Symbols = this.userControl.generateSymbolList().ToString();
            
            //settings.StartDate = this.yahooWizardPageStart.optStartDate();
            settings.ConnStr = userControl.getConnectionString();
            settings.QueryStr = userControl.getQueryStr();
            settings.DateCol = userControl.getDateColumnName();
            settings.OpenCol = userControl.getOpenColumnName();
            settings.HighCol = userControl.getHighColumnName();
            settings.LowCol = userControl.getLowColumnName();
            settings.CloseCol = userControl.getCloseColumnName();
            settings.VolumeCol = userControl.getVolumeColumnName();

            ds.DSString = settings.SerializeToString();
            ds.Scale = BarScale.Daily;
            ds.BarInterval = 0;
            return ds;
        }

        public override void Initialize(IDataHost dataHost)
        {
            base.Initialize(dataHost);
            this.barDataStore = new BarDataStore(dataHost, this);
        }
        

        public override void PopulateSymbols(DataSource ds, List<string> symbols)
        {
            DBProviderSettings settings = (DBProviderSettings)DBProviderSettings.DeserializeFromString(ds.DSString);

            string[] symbol = settings.Symbols.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string code in symbol)
            {
                symbols.Add(code);
            }
        }

        public override bool SupportsDataSourceUpdate
        {
            get
            {
                return true;
            }
        }


        public override void UpdateDataSource(DataSource dataSource, IDataUpdateMessage dataUpdateMsg)
        {
            //SymbolInfoList symbolInfoList = null;
            this.idataUpdateMessage = dataUpdateMsg;
            try
            {
                SymbolList list;
                DBProviderSettings settings = null;
                if (!string.IsNullOrEmpty(dataSource.Name))
                {
                    settings = (DBProviderSettings)DataSetSettings.DeserializeFromString(dataSource.DSString);
                    list = new SymbolList(settings.Symbols, DelimeterSetEnum.ForProgram);
                }
                else
                {
                    list = new SymbolList(dataSource.Symbols);
                }
                this.DisplayUpdateMessage("Requests are ready to go.");
                foreach (string s in list.list)
                {
                    this.DisplayUpdateMessage("Requesting data for " + s + " ...");
                    Bars bars = RequestData(dataSource, s, new DateTime(1980, 1, 1), new DateTime(2100, 1, 1), 100000, true);
                    this.barDataStore.SaveBarsObject(bars);
                }
                this.DisplayUpdateMessage("Requests are finished.");
            }
            catch (Exception exception)
            {
                //Logger.Log(LogLevel.ERROR, "UpdateDataSource " + exception.Message);
                this.idataUpdateMessage.DisplayUpdateMessage("Error: " + exception.Message);
            }
            finally
            {
                this.idataUpdateMessage = null;
            }
        }


        private void DisplayUpdateProgress()
        {
            if (this.idataUpdateMessage != null)
            {
                //this.idataUpdateMessage.ReportUpdateProgress((this.numberOfSymbolsUpdated * 100) / this.numberOfSymbolsToUpdate);
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

        public override Bars RequestData(DataSource ds, string symbol, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar)
        {
            Bars bars = new Bars(symbol, ds.BarDataScale.Scale, 1);

            if (this.barDataStore.ContainsSymbol(symbol, ds.Scale, ds.BarInterval) && !includePartialBar)
            {
                this.barDataStore.LoadBarsObject(bars, startDate, DateTime.MaxValue, maxBars);
                return bars;
            }

            symbol = symbol.Trim(new char[] { ' ', '"' });
            DBProviderSettings settings = new DBProviderSettings();
            if (ds.DSString != string.Empty)
            {
                settings = (DBProviderSettings)DataSetSettings.DeserializeFromString(ds.DSString);
            }

            //Bars bars = new Bars(symbol, ds.BarDataScale.Scale, 1);

            //string source = "server=(local);" + "integrated security=SSPI;" + "database = Stock";
            //string source = "Server=192.168.1.101;Database=financedbdev;User Id=dbuser;Password=Pa88w0rd;";
            string source = settings.ConnStr;
            //string select = "SELECT Trddt ,Opnprc,Hiprc,Loprc,Clsprc,Dnshrtrd,Dnvaltrd ,Adjprcnd FROM TRD_Dalyr where stkcd='" + symbol + "'";
            //string select = "SELECT Date,[Open],High,Low,[Close],Volume,TradeMoney FROM IndexDailyTradeData where IndexCode='" + symbol + "'";
            string select = settings.QueryStr.Replace("$SYMBOL$", symbol);
            using (SqlConnection conn = new SqlConnection(source))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                //DataSeries adjustCls = bars.RegisterNamedSeries("adjustCls", false);
                

                int i=0;
                while (reader.Read())
                {
                    try
                    {
                        string vol = reader[settings.VolumeCol].ToString();
                        double volume = double.Parse(vol);

                        /*
                        DateTime dt = reader.GetDateTime(0);
                        double d1 = (double)reader.GetDecimal(1);
                        double d2 = (double)reader.GetDecimal(2);
                        double d3 = (double)reader.GetDecimal(3);
                        double d4 = (double)reader.GetDecimal(4);
                        */

                        DateTime dt = reader.GetDateTime(reader.GetOrdinal(settings.DateCol));
                        double d1 = (double)reader.GetDecimal(reader.GetOrdinal(settings.OpenCol));
                        double d2 = (double)reader.GetDecimal(reader.GetOrdinal(settings.HighCol));
                        double d3 = (double)reader.GetDecimal(reader.GetOrdinal(settings.LowCol));
                        double d4 = (double)reader.GetDecimal(reader.GetOrdinal(settings.CloseCol));

                        bars.Add(dt, d1, d2, d3, d4, volume);
                    }
                    catch
                    {
                        continue;
                    }
                    //adjustCls[i++] = reader.GetDouble(7);
                }
                reader.Close();
                conn.Close();
            }


            return bars;
        }
                
        public override bool SupportsDynamicUpdate(BarScale scale)
        {
            return false;
        }

        public override bool CanModifySymbols
        {
            get
            {
                return true;
            }
        }
        public  string ModifySymbols2(DataSource ds, List<string> symbols)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string symbol in symbols)
            {
                sb.Append(symbol).Append(",");
            }
            if (sb.ToString().EndsWith(","))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public override string ModifySymbols(DataSource ds, List<string> symbols)
        {
            ///Logger.LogParameters(new object[0]);
            StringBuilder sList = new StringBuilder("");
            foreach (string s in symbols)
            {
                sList.Append(s + ",");
            }
            if (sList.ToString().EndsWith(","))
            {
                sList.Remove(sList.Length - 1, 1);
            }
            //SymbolList sList = new SymbolList(symbols);
            DBProviderSettings settings = (DBProviderSettings)DataSetSettings.DeserializeFromString(ds.DSString);
            settings.Symbols = sList.ToString().ToUpper();
            return settings.SerializeToString();
        }

        public override System.Windows.Forms.UserControl WizardFirstPage() 
        {
            userControl = new DataProviderUserControl();
            return userControl;
        }

        public override System.Windows.Forms.UserControl WizardNextPage(System.Windows.Forms.UserControl currentPage)
        {
            if (currentPage == userControl)
            {
                if (userControl.getDatabaseName().Trim().Length == 0)
                {
                    ///MessageBox.Show("Database name can not be blank");
                    ///return currentPage;
                }
            }
            return null;
        }

        public override System.Windows.Forms.UserControl WizardPreviousPage(System.Windows.Forms.UserControl currentPage)
        {
            return null;
        }

        public override string Description
        {
            get { return "this is a developing SQL server data provider"; }
        }

        public override string FriendlyName
        {
            get { return "SQL Server Data Provider"; }
        }

        public override System.Drawing.Bitmap Glyph
        {
            get
            {
                Icon typeIcon = new Icon(SystemIcons.Shield, 40, 40);
                return typeIcon.ToBitmap();;
            }
        }
    }
}
