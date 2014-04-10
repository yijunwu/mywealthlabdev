namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class BarGenerator
    {
        private BarData barData_0;
        private BarData barData_1;
        [CompilerGenerated]
        private int interval;

        public BarGenerator(int interval)
        {
            this.barData_0 = new BarData();
            this.barData_1 = new BarData();
            this.Interval = interval;
            this.barData_0.Open = -1.0;
            this.barData_0.High = double.MinValue;
            this.barData_0.Low = double.MaxValue;
            this.barData_0.Volume = 0.0;
            DateTime now = DateTime.Now;
            this.barData_0.Timestamp = new DateTime(now.Year, now.Month, now.Day, 9, 30, 0, 0);
            this.barData_0.Timestamp = this.barData_0.Timestamp.AddMinutes((double) this.Interval);
        }

        public BarGenerator(int interval, double open, double high, double double_0, double close, double volume, DateTime dateTime_0)
        {
            this.barData_0 = new BarData();
            this.barData_1 = new BarData();
            this.Interval = interval;
            this.barData_0.Open = open;
            this.barData_0.High = high;
            this.barData_0.Low = double_0;
            this.barData_0.Close = close;
            this.barData_0.Volume = volume;
            this.barData_0.Timestamp = new DateTime(dateTime_0.Year, dateTime_0.Month, dateTime_0.Day, 9, 30, 0, 0);
            while (this.barData_0.Timestamp < dateTime_0)
            {
                this.barData_0.Timestamp = this.barData_0.Timestamp.AddMinutes((double) this.Interval);
            }
        }

        public bool AppendData(double open, double high, double double_0, double close, double volume, DateTime dateTime_0)
        {
            if (this.barData_0.Open == -1.0)
            {
                this.barData_0.Open = open;
            }
            if (high > this.barData_0.High)
            {
                this.barData_0.High = high;
            }
            if ((double_0 < this.barData_0.Low) && (double_0 != 0.0))
            {
                this.barData_0.Low = double_0;
            }
            if (close != 0.0)
            {
                this.barData_0.Close = close;
            }
            this.barData_0.Volume += volume;
            if (dateTime_0 < this.barData_0.Timestamp)
            {
                return false;
            }
            this.barData_1.Open = this.barData_0.Open;
            this.barData_1.High = this.barData_0.High;
            this.barData_1.Low = this.barData_0.Low;
            this.barData_1.Close = this.barData_0.Close;
            this.barData_1.Volume = this.barData_0.Volume;
            this.barData_1.Timestamp = this.barData_0.Timestamp;
            this.barData_0.Open = -1.0;
            this.barData_0.High = double.MinValue;
            this.barData_0.Low = double.MaxValue;
            this.barData_0.Volume = 0.0;
            DateTime time2 = this.barData_0.Timestamp.AddMinutes((double) this.Interval);
            if (time2.Hour >= 0x10)
            {
                time2 = new DateTime(time2.Year, time2.Month, time2.Day, 0x10, 0, 0);
            }
            this.barData_0.Timestamp = time2;
            return true;
        }

        public int Interval
        {
            [CompilerGenerated]
            get
            {
                return this.interval;
            }
            [CompilerGenerated]
            set
            {
                this.interval = value;
            }
        }

        public BarData LastBar
        {
            get
            {
                return this.barData_1;
            }
        }
    }
}

