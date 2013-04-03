namespace WealthLab.Rules
{
    using System;
    using WealthLab;

    public class OptionExpiryDate
    {
        private Bars bars_0;
        private MarketHours marketHours_0;

        public OptionExpiryDate(Bars bars)
        {
            this.bars_0 = bars;
            this.marketHours_0 = new MarketHours();
        }

        public int DaysSinceOptionExpiryDate(int int_0)
        {
            DateTime time = this.bars_0.Date[int_0];
            DateTime date = time.Date;
            DateTime time3 = this.NextOptionExpiryDate(date.Year, date.Month);
            if (date < time3)
            {
                int year = date.Year;
                int month = date.Month;
                if (month == 1)
                {
                    year--;
                    month = 12;
                }
                else
                {
                    month--;
                }
                time3 = this.NextOptionExpiryDate(year, month);
            }
            DateTime time4 = this.bars_0.Date[int_0];
            return time4.Subtract(time3).Days;
        }

        public int DaysSinceTripleWitchingDate(int int_0)
        {
            DateTime time = this.bars_0.Date[int_0];
            DateTime date = time.Date;
            DateTime time3 = this.NextTripleWitchingDate(date.Year, date.Month);
            if (date < time3)
            {
                int year = date.Year;
                int month = date.Month;
                if (month <= 3)
                {
                    year--;
                    month = 12;
                }
                else
                {
                    month -= 3;
                }
                time3 = this.NextTripleWitchingDate(year, month);
            }
            DateTime time4 = this.bars_0.Date[int_0];
            return time4.Subtract(time3).Days;
        }

        public int DaysToOptionExpiryDate(int int_0)
        {
            DateTime time = this.bars_0.Date[int_0];
            DateTime date = time.Date;
            DateTime time3 = this.NextOptionExpiryDate(date.Year, date.Month);
            if (date > time3)
            {
                int year = date.Year;
                int month = date.Month;
                if (month == 12)
                {
                    year++;
                    month = 1;
                }
                else
                {
                    month++;
                }
                time3 = this.NextOptionExpiryDate(year, month);
            }
            return time3.Subtract(this.bars_0.Date[int_0]).Days;
        }

        public int DaysToTripleWitchingDate(int int_0)
        {
            DateTime time = this.bars_0.Date[int_0];
            DateTime date = time.Date;
            DateTime time3 = this.NextTripleWitchingDate(date.Year, date.Month);
            if (date > time3)
            {
                int year = date.Year;
                int month = date.Month;
                if (month == 12)
                {
                    year++;
                    month = 1;
                }
                else
                {
                    month++;
                }
                time3 = this.NextTripleWitchingDate(year, month);
            }
            return time3.Subtract(this.bars_0.Date[int_0]).Days;
        }

        public DateTime NextOptionExpiryDate(int year, int month)
        {
            DateTime time = new DateTime(year, month, 1);
            int num = 0;
            for (int i = 1; i <= 0x1f; i++)
            {
                time = new DateTime(year, month, i);
                if (time.DayOfWeek == DayOfWeek.Friday)
                {
                    num++;
                    if (num == 3)
                    {
                        break;
                    }
                }
            }
            if (!this.marketHours_0.IsTradingDay(time))
            {
                time = time.Subtract(new TimeSpan(1, 0, 0, 0));
            }
            return time;
        }

        public DateTime NextTripleWitchingDate(int year, int month)
        {
            int num;
            if (month <= 3)
            {
                num = 3;
            }
            else if (month <= 6)
            {
                num = 6;
            }
            else if (month <= 9)
            {
                num = 9;
            }
            else
            {
                num = 12;
            }
            return this.NextOptionExpiryDate(year, num);
        }
    }
}

