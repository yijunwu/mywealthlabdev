namespace WealthLab.Rules
{
    using System;
    using System.Drawing;
    using System.Text;
    using WealthLab;

    public class ArmsIndex : MarketSentiment
    {
        private DataSeries dataSeries_0;
        private double double_0;

        public ArmsIndex(WealthScript wealthScript_0, string exchange, double level)
        {
            base.m_ws = wealthScript_0;
            base.m_sExchange = exchange;
            this.double_0 = level;
            string symbol = this.getArmsIndexSymbol(exchange);
            this.dataSeries_0 = base.getExternalSeries(symbol, "Close", true);
            StringBuilder builder = new StringBuilder();
            builder.Append(base.m_sExchange);
            builder.Append(" Arms index: ");
            builder.Append(symbol);
            this.dataSeries_0.Description = builder.ToString();
        }

        public string getArmsIndexSymbol(string exchange)
        {
            if (exchange == "AMEX")
            {
                return ".STI.A";
            }
            if (exchange == "NYSE")
            {
                return ".STI.N";
            }
            if (exchange == "Nasdaq")
            {
                return ".STI.O";
            }
            return "";
        }

        public void plotArmsIndexSeries()
        {
            this.plotArmsIndexSeries(50, true, true);
        }

        public void plotArmsIndexSeries(ChartPane paneSTI)
        {
            base.m_ws.PlotSeries(paneSTI, this.dataSeries_0, Color.Blue, LineStyle.Solid, 2);
            base.m_ws.DrawHorzLine(paneSTI, this.double_0, Color.Black, LineStyle.Dotted, 2);
        }

        public void plotArmsIndexSeries(int height, bool abovePrice, bool showGrid)
        {
            ChartPane paneSTI = base.m_ws.CreatePane(height, abovePrice, showGrid);
            this.plotArmsIndexSeries(paneSTI);
        }

        public DataSeries ArmsIndexSeries
        {
            get
            {
                return this.dataSeries_0;
            }
        }
    }
}

