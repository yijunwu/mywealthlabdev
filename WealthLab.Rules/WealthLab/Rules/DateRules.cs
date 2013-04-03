namespace WealthLab.Rules
{
    using System;
    using WealthLab;

    public class DateRules
    {
        private static MarketHours marketHours_0 = new MarketHours();

        public static bool IsLastTradingDayOfMonth(DateTime dateTime_0)
        {
            if (!marketHours_0.IsTradingDay(dateTime_0))
            {
                return false;
            }
            DateTime time = new DateTime(dateTime_0.Year, dateTime_0.Month, dateTime_0.Day);
            for (time = time.AddDays(1.0); time.Month == dateTime_0.Month; time = time.AddDays(1.0))
            {
                if (marketHours_0.IsTradingDay(time))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsLastTradingDayOfQuarter(DateTime dateTime_0)
        {
            if ((dateTime_0.Month % 3) != 0)
            {
                return false;
            }
            return IsLastTradingDayOfMonth(dateTime_0);
        }
    }
}

