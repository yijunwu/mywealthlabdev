namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class BarData
    {
        [CompilerGenerated]
        private DateTime timestamp;
        [CompilerGenerated]
        private double open;
        [CompilerGenerated]
        private double high;
        [CompilerGenerated]
        private double low;
        [CompilerGenerated]
        private double close;
        [CompilerGenerated]
        private double volume;

        public double Close
        {
            [CompilerGenerated]
            get
            {
                return this.close;
            }
            [CompilerGenerated]
            set
            {
                this.close = value;
            }
        }

        public double High
        {
            [CompilerGenerated]
            get
            {
                return this.high;
            }
            [CompilerGenerated]
            set
            {
                this.high = value;
            }
        }

        public double Low
        {
            [CompilerGenerated]
            get
            {
                return this.low;
            }
            [CompilerGenerated]
            set
            {
                this.low = value;
            }
        }

        public double Open
        {
            [CompilerGenerated]
            get
            {
                return this.open;
            }
            [CompilerGenerated]
            set
            {
                this.open = value;
            }
        }

        public DateTime Timestamp
        {
            [CompilerGenerated]
            get
            {
                return this.timestamp;
            }
            [CompilerGenerated]
            set
            {
                this.timestamp = value;
            }
        }

        public double Volume
        {
            [CompilerGenerated]
            get
            {
                return this.volume;
            }
            [CompilerGenerated]
            set
            {
                this.volume = value;
            }
        }
    }
}

