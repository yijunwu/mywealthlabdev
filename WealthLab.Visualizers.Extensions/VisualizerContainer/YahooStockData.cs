namespace VisualizerContainer
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    internal class YahooStockData
    {
        private string urlTemplate = "http://ichart.finance.yahoo.com/table.csv?s=[symbol]&a=[startMonth]&b=[startDay]&c=[startYear]&d=[endMonth]&e=[endDay]&f=[endYear]&g=d&ignore=.csv";

        public List<StockData> GetStockData(string symbol, DateTime? startDate, DateTime? endDate)
        {
            if (!endDate.HasValue)
            {
                endDate = new DateTime?(DateTime.Now);
            }
            if (!startDate.HasValue)
            {
                startDate = new DateTime?(DateTime.Now.AddYears(-2));
            }
            if (string.IsNullOrEmpty(symbol))
            {
                throw new ArgumentException("Symbol invalid: " + symbol);
            }
            string newValue = (startDate.Value.Month - 1).ToString();
            string str2 = startDate.Value.Day.ToString();
            string str3 = startDate.Value.Year.ToString();
            string str4 = (endDate.Value.Month - 1).ToString();
            string str5 = endDate.Value.Day.ToString();
            string str6 = endDate.Value.Year.ToString();
            this.urlTemplate = this.urlTemplate.Replace("[symbol]", symbol);
            this.urlTemplate = this.urlTemplate.Replace("[startMonth]", newValue);
            this.urlTemplate = this.urlTemplate.Replace("[startDay]", str2);
            this.urlTemplate = this.urlTemplate.Replace("[startYear]", str3);
            this.urlTemplate = this.urlTemplate.Replace("[endMonth]", str4);
            this.urlTemplate = this.urlTemplate.Replace("[endDay]", str5);
            this.urlTemplate = this.urlTemplate.Replace("[endYear]", str6);
            string str7 = string.Empty;
            WebClient client = new WebClient();
            try
            {
                str7 = client.DownloadString(this.urlTemplate);
            }
            catch (WebException exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                client.Dispose();
            }
            string[] strArray = str7.Replace("\r", "").Split(new char[] { '\n' });
            StockData item = new StockData();
            List<StockData> list = new List<StockData>();
            for (int i = strArray.Length - 1; i > 0; i--)
            {
                string[] strArray2 = strArray[i].Split(new char[] { ',' });
                if ((strArray2[0] != null) && (strArray2[0] != ""))
                {
                    item = new StockData(DateTime.Parse(strArray2[0]), double.Parse(strArray2[1]), double.Parse(strArray2[2]), double.Parse(strArray2[3]), double.Parse(strArray2[4]), long.Parse(strArray2[5]), double.Parse(strArray2[6]));
                    list.Add(item);
                }
            }
            return list;
        }
    }
}

