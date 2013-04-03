namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class BarData
    {
        [CompilerGenerated]
        private DateTime dateTime_0;
        [CompilerGenerated]
        private double double_0;
        [CompilerGenerated]
        private double double_1;
        [CompilerGenerated]
        private double double_2;
        [CompilerGenerated]
        private double double_3;
        [CompilerGenerated]
        private double double_4;

        public double Close
        {
            [CompilerGenerated]
            get
            {
                return this.double_3;
            }
            [CompilerGenerated]
            set
            {
                this.double_3 = value;
            }
        }

        public double High
        {
            [CompilerGenerated]
            get
            {
                return this.double_1;
            }
            [CompilerGenerated]
            set
            {
                this.double_1 = value;
            }
        }

        public double Low
        {
            [CompilerGenerated]
            get
            {
                return this.double_2;
            }
            [CompilerGenerated]
            set
            {
                this.double_2 = value;
            }
        }

        public double Open
        {
            [CompilerGenerated]
            get
            {
                return this.double_0;
            }
            [CompilerGenerated]
            set
            {
                this.double_0 = value;
            }
        }

        public DateTime Timestamp
        {
            [CompilerGenerated]
            get
            {
                return this.dateTime_0;
            }
            [CompilerGenerated]
            set
            {
                this.dateTime_0 = value;
            }
        }

        public double Volume
        {
            [CompilerGenerated]
            get
            {
                return this.double_4;
            }
            [CompilerGenerated]
            set
            {
                this.double_4 = value;
            }
        }
    }
}

