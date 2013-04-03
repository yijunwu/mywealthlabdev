namespace WealthLab
{
    using System;
    using System.Text;

    public class BarDataRange
    {
        private BarRange barRange_0;
        private bool bool_0;
        private DateTime dateTime_0 = DateTime.Now.Date.AddYears(-5);
        private DateTime dateTime_1 = DateTime.Now.Date;
        private int int_0 = 500;
        private int int_1 = 10;

        public void ConfigureBarsLoader(BarsLoader loader)
        {
            loader.StartDate = DateTime.MinValue;
            loader.EndDate = DateTime.MaxValue;
            loader.MaxBars = 0;
            switch (this.Range)
            {
                case BarRange.FixedBars:
                    loader.MaxBars = this.FixedBars;
                    return;

                case BarRange.RecentYears:
                    loader.StartDate = DateTime.Now.Date.AddYears(-this.RecentValue).AddDays(1.0);
                    return;

                case BarRange.RecentMonths:
                    loader.StartDate = DateTime.Now.Date.AddMonths(-this.RecentValue).AddDays(1.0);
                    return;

                case BarRange.RecentWeeks:
                    loader.StartDate = DateTime.Now.Date.AddDays((double) (-this.RecentValue * 7)).AddDays(1.0);
                    return;

                case BarRange.RecentDays:
                    loader.StartDate = DateTime.Now.Date.AddDays((double) (-this.RecentValue + 1));
                    return;

                case BarRange.DateRange:
                    loader.StartDate = this.StartDate;
                    loader.EndDate = this.EndDate;
                    return;
            }
        }

        public static BarDataRange Parse(string string_0)
        {
            string[] strArray = string_0.Split(new char[] { ';' });
            return new BarDataRange { Range = (BarRange) Enum.Parse(typeof(BarRange), strArray[0]), FixedBars = int.Parse(strArray[1]), RecentValue = int.Parse(strArray[2]), StartDate = DateTime.Parse(strArray[3]), EndDate = DateTime.Parse(strArray[4]) };
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Range);
            builder.Append(";");
            builder.Append(this.FixedBars);
            builder.Append(";");
            builder.Append(this.RecentValue);
            builder.Append(";");
            builder.Append(this.StartDate);
            builder.Append(";");
            builder.Append(this.EndDate);
            return builder.ToString();
        }

        public DateTime EndDate
        {
            get
            {
                return this.dateTime_1;
            }
            set
            {
                this.dateTime_1 = value;
            }
        }

        public int FixedBars
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }

        public bool IsStreaming
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public BarRange Range
        {
            get
            {
                return this.barRange_0;
            }
            set
            {
                this.barRange_0 = value;
            }
        }

        public int RecentValue
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.dateTime_0;
            }
            set
            {
                this.dateTime_0 = value;
            }
        }

        public string Text
        {
            get
            {
                switch (this.Range)
                {
                    case BarRange.AllData:
                        return "All Data";

                    case BarRange.FixedBars:
                        return (this.FixedBars.ToString("N0") + " Bars");

                    case BarRange.RecentYears:
                        return (this.RecentValue.ToString("N0") + " Years");

                    case BarRange.RecentMonths:
                        return (this.RecentValue.ToString("N0") + " Months");

                    case BarRange.RecentWeeks:
                        return (this.RecentValue.ToString("N0") + " Weeks");

                    case BarRange.RecentDays:
                        return (this.RecentValue.ToString("N0") + " Days");

                    case BarRange.DateRange:
                    {
                        string str = this.StartDate.ToShortDateString() + " to ";
                        if (!this.IsStreaming)
                        {
                            return (str + this.EndDate.ToShortDateString());
                        }
                        return (str + "current");
                    }
                }
                return "";
            }
        }
    }
}

