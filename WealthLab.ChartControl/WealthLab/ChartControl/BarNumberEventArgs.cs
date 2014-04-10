namespace WealthLab.ChartControl
{
    using System;
    using WealthLab;

    public class BarNumberEventArgs : EventArgs
    {
        private ChartPane chartPane;
        private double doubleValue;
        private int barNumber;

        public BarNumberEventArgs(int barNumber, double value, ChartPane pane)
        {
            this.barNumber = barNumber;
            this.doubleValue = value;
            this.chartPane = pane;
        }

        public int BarNumber
        {
            get
            {
                return this.barNumber;
            }
        }

        public ChartPane Pane
        {
            get
            {
                return this.chartPane;
            }
        }

        public double Value
        {
            get
            {
                return this.doubleValue;
            }
        }
    }
}

