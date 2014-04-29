namespace WealthLab
{
    using System;
    using System.Text;

    public class BarDataRange
    {
        private BarRange barRange;
        private bool isStreaming;
        private DateTime startDate = DateTime.Now.Date.AddYears(-5);
        private DateTime endDate = DateTime.Now.Date;
        private int fixedBars = 500;
        private int recentValue = 10;

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
            return new BarDataRange { Range = (BarRange) Enum.Parse(typeof(BarRange), strArray[0]), 
                FixedBars = int.Parse(strArray[1]), 
                RecentValue = int.Parse(strArray[2]), 
                StartDate = DateTime.Parse(strArray[3]), 
                EndDate = DateTime.Parse(strArray[4]) };
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
                return this.endDate;
            }
            set
            {
                this.endDate = value;
            }
        }

        public int FixedBars
        {
            get
            {
                return this.fixedBars;
            }
            set
            {
                this.fixedBars = value;
            }
        }

        public bool IsStreaming
        {
            get
            {
                return this.isStreaming;
            }
            set
            {
                this.isStreaming = value;
            }
        }

        public BarRange Range
        {
            get
            {
                return this.barRange;
            }
            set
            {
                this.barRange = value;
            }
        }

        public int RecentValue
        {
            get
            {
                return this.recentValue;
            }
            set
            {
                this.recentValue = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                this.startDate = value;
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

