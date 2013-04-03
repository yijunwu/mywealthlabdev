namespace WealthLab.Rules
{
    using System;
    using System.Drawing;
    using System.Text;
    using WealthLab;
    using WealthLab.Indicators;

    public class NewHighLow : MarketSentiment
    {
        private DataSeries dataSeries_0;
        private DataSeries dataSeries_1;
        private DataSeries dataSeries_2;
        private DataSeries dataSeries_3;
        private DataSeries dataSeries_4;
        private DataSeries dataSeries_5;
        private DataSeries dataSeries_6;
        private DataSeries dataSeries_7;
        private double double_0;
        private int int_0;
        private int int_1;
        private string string_0;
        private string string_1;
        private string string_2;

        public NewHighLow(WealthScript wealthScript_0, string exchange, string index, int period, int smaPeriod)
        {
            base.m_ws = wealthScript_0;
            base.m_sExchange = exchange;
            this.int_0 = period;
            this.int_1 = smaPeriod;
            this.string_0 = index;
            if (this.createNewHighLowRatioSeries() && (period > 0))
            {
                this.createMomentumSMARatioSeries();
                if (index != "")
                {
                    this.createMomentumSMAIndexSeries();
                }
            }
        }

        public bool createMomentumIndexSeries()
        {
            try
            {
                this.dataSeries_3 = base.getExternalSeries(this.string_0, "Close", true);
                this.dataSeries_6 = Momentum.Series(this.dataSeries_3, this.int_0);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool createMomentumRatioSeries()
        {
            try
            {
                this.dataSeries_4 = Momentum.Series(this.dataSeries_2, this.int_0);
                StringBuilder builder = new StringBuilder();
                builder.Append(base.m_sExchange);
                builder.Append(" New High/Low Ratio");
                builder.Append(" Momentum(");
                builder.Append(this.int_0.ToString());
                builder.Append(")");
                this.dataSeries_4.Description = builder.ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool createMomentumSMAIndexSeries()
        {
            if (!this.createMomentumIndexSeries())
            {
                return false;
            }
            try
            {
                this.dataSeries_7 = SMA.Series(this.dataSeries_6, this.int_1);
                StringBuilder builder = new StringBuilder();
                builder.Append(this.string_0);
                builder.Append(" Momentum(");
                builder.Append(this.int_0.ToString());
                builder.Append(")");
                builder.Append(" SMA(");
                builder.Append(this.int_1.ToString());
                builder.Append(")");
                this.dataSeries_7.Description = builder.ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool createMomentumSMARatioSeries()
        {
            try
            {
                if (this.createNewHighLowRatioSeries())
                {
                    if (this.createMomentumRatioSeries())
                    {
                        this.dataSeries_5 = SMA.Series(this.dataSeries_4, this.int_1);
                        StringBuilder builder = new StringBuilder();
                        builder.Append(base.m_sExchange);
                        builder.Append(" New High/Low Ratio");
                        builder.Append(" Momentum(");
                        builder.Append(this.int_0.ToString());
                        builder.Append(")");
                        builder.Append(" SMA(");
                        builder.Append(this.int_1.ToString());
                        builder.Append(")");
                        this.dataSeries_5.Description = builder.ToString();
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool createNewHighLowRatioSeries()
        {
            if (!this.createNewHighSeries() || !this.createNewLowSeries())
            {
                return false;
            }
            try
            {
                this.dataSeries_2 = this.dataSeries_0 / this.dataSeries_1;
                this.dataSeries_2.Description = base.m_sExchange + " New Highs/Lows Ratio";
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool createNewHighSeries()
        {
            try
            {
                this.string_1 = this.method_0();
                this.dataSeries_0 = base.getExternalSeries(this.string_1, "Close", true);
                this.dataSeries_0.Description = base.m_sExchange + " New Highs";
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool createNewLowSeries()
        {
            try
            {
                this.string_2 = this.method_1();
                this.dataSeries_1 = base.getExternalSeries(this.string_2, "Close", true);
                this.dataSeries_1.Description = base.m_sExchange + " New Lows";
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string method_0()
        {
            if (base.m_sExchange == "AMEX")
            {
                return ".MB_NH.A";
            }
            if (base.m_sExchange == "NYSE")
            {
                return ".MB_NH.N";
            }
            if (base.m_sExchange == "Nasdaq")
            {
                return ".MB_NH.Q";
            }
            return "";
        }

        private string method_1()
        {
            if (base.m_sExchange == "AMEX")
            {
                return ".MB_NL.A";
            }
            if (base.m_sExchange == "NYSE")
            {
                return ".MB_NL.N";
            }
            if (base.m_sExchange == "Nasdaq")
            {
                return ".MB_NL.Q";
            }
            return "";
        }

        public void plotIndex()
        {
            this.plotIndex(50, true, true);
        }

        public void plotIndex(ChartPane paneIndex)
        {
            base.m_ws.PlotSeries(paneIndex, this.dataSeries_3, Color.Green, LineStyle.Histogram, 50);
        }

        public void plotIndex(int height, bool abovePrice, bool showGrid)
        {
            ChartPane paneIndex = base.m_ws.CreatePane(height, abovePrice, showGrid);
            this.plotIndex(paneIndex);
        }

        public void plotMomentumRatioToIndex()
        {
            this.plotMomentumRatioToIndex(50, true, true);
        }

        public void plotMomentumRatioToIndex(ChartPane paneMomentum)
        {
            base.m_ws.PlotSeries(paneMomentum, this.dataSeries_4, Color.Purple, LineStyle.Solid, 2);
            base.m_ws.PlotSeries(paneMomentum, this.dataSeries_6, Color.Green, LineStyle.Solid, 2);
            base.m_ws.DrawHorzLine(paneMomentum, 0.0, Color.Black, LineStyle.Dotted, 2);
        }

        public void plotMomentumRatioToIndex(int height, bool abovePrice, bool showGrid)
        {
            ChartPane paneMomentum = base.m_ws.CreatePane(height, abovePrice, showGrid);
            this.plotMomentumRatioToIndex(paneMomentum);
        }

        public void plotMomentumSMA()
        {
            this.plotMomentumSMA(50, true, true);
        }

        public void plotMomentumSMA(ChartPane paneMomentumSMA)
        {
            base.m_ws.PlotSeries(paneMomentumSMA, this.dataSeries_5, Color.Purple, LineStyle.Solid, 2);
            base.m_ws.PlotSeries(paneMomentumSMA, this.dataSeries_7, Color.Green, LineStyle.Solid, 2);
            base.m_ws.DrawHorzLine(paneMomentumSMA, 0.0, Color.Black, LineStyle.Dotted, 2);
        }

        public void plotMomentumSMA(int height, bool abovePrice, bool showGrid)
        {
            ChartPane paneMomentumSMA = base.m_ws.CreatePane(height, abovePrice, showGrid);
            this.plotMomentumSMA(paneMomentumSMA);
        }

        public void plotNewHighLowRatio()
        {
            this.plotNewHighLowRatio(50, true, true);
        }

        public void plotNewHighLowRatio(ChartPane paneRatio)
        {
            base.m_ws.PlotSeries(paneRatio, this.dataSeries_2, Color.Purple, LineStyle.Solid, 2);
            base.m_ws.DrawHorzLine(paneRatio, this.double_0, Color.Black, LineStyle.Dotted, 2);
        }

        public void plotNewHighLowRatio(int height, bool abovePrice, bool showGrid)
        {
            ChartPane paneRatio = base.m_ws.CreatePane(height, abovePrice, showGrid);
            this.plotNewHighLowRatio(paneRatio);
        }

        public void plotNewHighs()
        {
            this.plotNewHighs(50, true, true);
        }

        public void plotNewHighs(ChartPane paneNH)
        {
            base.m_ws.PlotSeries(paneNH, this.dataSeries_0, Color.Blue, LineStyle.Histogram, 50);
        }

        public void plotNewHighs(int height, bool abovePrice, bool showGrid)
        {
            ChartPane paneNH = base.m_ws.CreatePane(height, abovePrice, showGrid);
            this.plotNewHighs(paneNH);
        }

        public void plotNewLows()
        {
            this.plotNewLows(50, true, true);
        }

        public void plotNewLows(ChartPane paneNL)
        {
            base.m_ws.PlotSeries(paneNL, this.dataSeries_1, Color.Red, LineStyle.Histogram, 50);
        }

        public void plotNewLows(int height, bool abovePrice, bool showGrid)
        {
            ChartPane paneNL = base.m_ws.CreatePane(height, abovePrice, showGrid);
            this.plotNewLows(paneNL);
        }

        public string Index
        {
            set
            {
                this.string_0 = value;
            }
        }

        public DataSeries MomentumIndexSeries
        {
            get
            {
                return this.dataSeries_6;
            }
        }

        public DataSeries MomentumRatioSeries
        {
            get
            {
                return this.dataSeries_4;
            }
        }

        public DataSeries MomentumSMAIndexSeries
        {
            get
            {
                return this.dataSeries_7;
            }
        }

        public DataSeries MomentumSMARatioSeries
        {
            get
            {
                return this.dataSeries_5;
            }
        }

        public DataSeries NewHighLowRatioSeries
        {
            get
            {
                return this.dataSeries_2;
            }
        }

        public int Period
        {
            set
            {
                this.int_0 = value;
            }
        }

        public int SMAPeriod
        {
            set
            {
                this.int_1 = value;
            }
        }

        public double Threshold
        {
            set
            {
                this.double_0 = value;
            }
        }
    }
}

