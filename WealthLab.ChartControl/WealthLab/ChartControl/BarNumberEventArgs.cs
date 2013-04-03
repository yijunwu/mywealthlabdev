namespace WealthLab.ChartControl
{
    using System;
    using WealthLab;

    public class BarNumberEventArgs : EventArgs
    {
        private ChartPane chartPane_0;
        private double double_0;
        private int int_0;

        public BarNumberEventArgs(int barNumber, double value, ChartPane pane)
        {
            this.int_0 = barNumber;
            this.double_0 = value;
            this.chartPane_0 = pane;
        }

        public int BarNumber
        {
            get
            {
                return this.int_0;
            }
        }

        public ChartPane Pane
        {
            get
            {
                return this.chartPane_0;
            }
        }

        public double Value
        {
            get
            {
                return this.double_0;
            }
        }
    }
}

