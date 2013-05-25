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

        private DataProviderUserControl userControl = null;
        public override DataSource CreateDataSource()
        {
            DataSource ds = new DataSource(this);
            ds.BarDataScale = new BarDataScale(BarScale.Daily, 1);

            DBProviderSettings settings = new DBProviderSettings();

            //if (this.userControl != null)
            //    settings.Symbols = this.userControl.generateSymbolList().ToString();
            
            //settings.StartDate = this.yahooWizardPageStart.optStartDate();
            //settings.QueryStr = userControl.
            ds.DSString = settings.SerializeToString();
            ds.Scale = BarScale.Daily;
            ds.BarInterval = 0;
            return ds;
        }

        public override void PopulateSymbols(DataSource ds, List<string> symbols)
        {

            string[] symbol = ds.DSString.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string code in symbol)
            {
                symbols.Add(code);
            }
        }

        public override Bars RequestData(DataSource ds, string symbol, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar)
        {
            Bars bars = new Bars(symbol, ds.BarDataScale.Scale, 1);

            //string source = "server=(local);" + "integrated security=SSPI;" + "database = Stock";
            string source = "Server=192.168.1.101;Database=financedbdev;User Id=dbuser;Password=Pa88w0rd;";
            //string select = "SELECT Trddt ,Opnprc,Hiprc,Loprc,Clsprc,Dnshrtrd,Dnvaltrd ,Adjprcnd FROM TRD_Dalyr where stkcd='" + symbol + "'";
            string select = "SELECT Date,[Open],High,Low,[Close],Volume,TradeMoney FROM IndexDailyTradeData where IndexCode='" + symbol + "'";
            using (SqlConnection conn = new SqlConnection(source))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                //DataSeries adjustCls = bars.RegisterNamedSeries("adjustCls", false);
                

                int i=0;
                while (reader.Read())
                {
                    string vol = reader["Volume"].ToString();
                    double volume = double.Parse(vol);

                    DateTime dt = reader.GetDateTime(0);
                    double d1 = (double)reader.GetDecimal(1);
                    double d2 = (double)reader.GetDecimal(2);
                    double d3 = (double)reader.GetDecimal(3);
                    double d4 = (double)reader.GetDecimal(4);

                    bars.Add(dt, d1, d2, d3, d4, volume);
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
        public override string ModifySymbols(DataSource ds, List<string> symbols)
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
                    MessageBox.Show("Database name can not be blank");
                    return currentPage;
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
