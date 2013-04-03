namespace WealthLab.Rules
{
    using System;
    using System.Drawing;
    using System.Text;
    using WealthLab;

    public class AdvDecRatio : MarketSentiment
    {
        private DataSeries dataSeries_0;
        private DataSeries dataSeries_1;
        private DataSeries dataSeries_2;
        private double double_0;
        private string string_0;

        public AdvDecRatio(WealthScript wealthScript_0, string exchange, string series, double value)
        {
            base.m_ws = wealthScript_0;
            base.m_sExchange = exchange;
            this.string_0 = series;
            this.double_0 = value;
            this.dataSeries_0 = this.getAdvancingSeries(exchange, series, true);
            this.dataSeries_1 = this.getDecliningSeries(exchange, series, true);
            this.dataSeries_2 = this.dataSeries_0 / this.dataSeries_1;
            StringBuilder builder = new StringBuilder();
            builder.Append(base.m_sExchange);
            builder.Append(" Advancing ");
            builder.Append(base.getSeriesType(this.string_0));
            this.dataSeries_0.Description = builder.ToString();
            builder.Remove(0, builder.Length);
            builder.Append(base.m_sExchange);
            builder.Append(" Declining ");
            builder.Append(base.getSeriesType(this.string_0));
            this.dataSeries_1.Description = builder.ToString();
            builder.Remove(0, builder.Length);
            builder.Append(base.m_sExchange);
            builder.Append(" Advance/Decline ");
            builder.Append(base.getSeriesType(this.string_0));
            builder.Append(" Ratio");
            this.dataSeries_2.Description = builder.ToString();
        }

        public DataSeries getAdvancingSeries(string exchange, string series, bool synch)
        {
            string symbol = this.getAdvancingSymbol(exchange);
            DataSeries series2 = base.getExternalSeries(symbol, series, synch);
            series2.Description = "Advancing " + base.getSeriesType(series);
            return series2;
        }

        public string getAdvancingSymbol(string exchange)
        {
            if (exchange == "AMEX")
            {
                return ".MB_ADV.A";
            }
            if (exchange == "NYSE")
            {
                return ".MB_ADV.N";
            }
            if (exchange == "Nasdaq")
            {
                return ".MB_ADV.Q";
            }
            return "";
        }

        public DataSeries getAdvDecRatioSeries(DataSeries dsAdv, DataSeries dsDec, string series)
        {
            DataSeries series2 = dsAdv / dsDec;
            series2.Description = "Advancing/Declining " + base.getSeriesType(series);
            return series2;
        }

        public DataSeries getDecliningSeries(string exchange, string series, bool synch)
        {
            string symbol = this.getDecliningSymbol(exchange);
            DataSeries series2 = base.getExternalSeries(symbol, series, synch);
            series2.Description = "Declining " + base.getSeriesType(series);
            return series2;
        }

        public string getDecliningSymbol(string exchange)
        {
            if (exchange == "AMEX")
            {
                return ".MB_DEC.A";
            }
            if (exchange == "NYSE")
            {
                return ".MB_DEC.N";
            }
            if (exchange == "Nasdaq")
            {
                return ".MB_DEC.Q";
            }
            return "";
        }

        public void plotAdvancingSeries()
        {
            this.plotAdvancingSeries(0x19, true, true);
        }

        public void plotAdvancingSeries(ChartPane paneAdv)
        {
            base.m_ws.PlotSeries(paneAdv, this.dataSeries_0, Color.DarkBlue, LineStyle.Histogram, 50);
        }

        public void plotAdvancingSeries(int height, bool abovePrice, bool showGrid)
        {
            ChartPane paneAdv = base.m_ws.CreatePane(height, abovePrice, showGrid);
            this.plotAdvancingSeries(paneAdv);
        }

        public void plotAdvDecRatio()
        {
            this.plotAdvDecRatio(50, true, true);
        }

        public void plotAdvDecRatio(ChartPane paneRatio)
        {
            base.m_ws.PlotSeries(paneRatio, this.dataSeries_2, Color.Purple, LineStyle.Solid, 2);
            base.m_ws.DrawHorzLine(paneRatio, this.double_0, Color.Black, LineStyle.Dashed, 2);
        }

        public void plotAdvDecRatio(int height, bool abovePrice, bool showGrid)
        {
            ChartPane paneRatio = base.m_ws.CreatePane(height, abovePrice, showGrid);
            this.plotAdvDecRatio(paneRatio);
        }

        public void plotDecliningSeries()
        {
            this.plotDecliningSeries(0x19, true, true);
        }

        public void plotDecliningSeries(ChartPane paneDec)
        {
            base.m_ws.PlotSeries(paneDec, this.dataSeries_1, Color.Red, LineStyle.Histogram, 2);
        }

        public void plotDecliningSeries(int height, bool abovePrice, bool showGrid)
        {
            ChartPane paneDec = base.m_ws.CreatePane(height, abovePrice, showGrid);
            this.plotDecliningSeries(paneDec);
        }

        public DataSeries AdvancingSeries
        {
            get
            {
                return this.dataSeries_0;
            }
        }

        public DataSeries AdvDecRatioSeries
        {
            get
            {
                return this.dataSeries_2;
            }
        }

        public DataSeries DecliningSeries
        {
            get
            {
                return this.dataSeries_1;
            }
        }

        public string Series
        {
            set
            {
                this.string_0 = value;
            }
        }

        public double Value
        {
            set
            {
                this.double_0 = value;
            }
        }
    }
}

