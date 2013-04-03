namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Xml.Serialization;

    public class TimeZoneInfo
    {
        private bool bool_0;
        private int int_0;
        private int int_1;
        private List<AdjustmentRule> list_0 = new List<AdjustmentRule>();
        private string string_0;

        public static DateTime ConvertTime(DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
        {
            if (sourceTimeZone == null)
            {
                throw new ArgumentNullException("sourceTimeZone");
            }
            if (destinationTimeZone == null)
            {
                throw new ArgumentNullException("destinationTimeZone");
            }
            AdjustmentRule rule = sourceTimeZone.method_0(dateTime);
            TimeSpan baseUtcOffset = sourceTimeZone.BaseUtcOffset;
            if (rule != null)
            {
                bool flag = false;
                DaylightTime time = smethod_1(dateTime.Year, rule);
                flag = smethod_6(dateTime, rule, time);
                baseUtcOffset += flag ? rule.DaylightDelta : TimeSpan.Zero;
            }
            long num = dateTime.Ticks - baseUtcOffset.Ticks;
            return new DateTime(smethod_2(num, destinationTimeZone).Ticks, DateTimeKind.Unspecified);
        }

        private AdjustmentRule method_0(DateTime dateTime_0)
        {
            if ((this.list_0 != null) && (this.list_0.Count != 0))
            {
                DateTime date = dateTime_0.Date;
                for (int i = 0; i < this.list_0.Count; i++)
                {
                    if ((this.list_0[i].DateStart <= date) && (this.list_0[i].DateEnd >= date))
                    {
                        return this.list_0[i];
                    }
                }
            }
            return null;
        }

        private static DateTime smethod_0(int int_2, TransitionTime transitionTime_0)
        {
            DateTime time2;
            DateTime timeOfDay = transitionTime_0.TimeOfDay;
            if (transitionTime_0.IsFixedRule)
            {
                int num = DateTime.DaysInMonth(int_2, transitionTime_0.Month);
                return new DateTime(int_2, transitionTime_0.Month, (num < transitionTime_0.Day) ? num : transitionTime_0.Day, timeOfDay.Hour, timeOfDay.Minute, timeOfDay.Second, timeOfDay.Millisecond);
            }
            if (transitionTime_0.Week <= 4)
            {
                time2 = new DateTime(int_2, transitionTime_0.Month, 1, timeOfDay.Hour, timeOfDay.Minute, timeOfDay.Second, timeOfDay.Millisecond);
                int dayOfWeek = (int) time2.DayOfWeek;
                int num3 = ((int) transitionTime_0.DayOfWeek) - dayOfWeek;
                if (num3 < 0)
                {
                    num3 += 7;
                }
                num3 += 7 * (transitionTime_0.Week - 1);
                if (num3 > 0)
                {
                    time2 = time2.AddDays((double) num3);
                }
                return time2;
            }
            int day = DateTime.DaysInMonth(int_2, transitionTime_0.Month);
            time2 = new DateTime(int_2, transitionTime_0.Month, day, timeOfDay.Hour, timeOfDay.Minute, timeOfDay.Second, timeOfDay.Millisecond);
            int num4 = (int) (time2.DayOfWeek - transitionTime_0.DayOfWeek);
            if (num4 < 0)
            {
                num4 += 7;
            }
            if (num4 > 0)
            {
                time2 = time2.AddDays((double) -num4);
            }
            return time2;
        }

        private static DaylightTime smethod_1(int int_2, AdjustmentRule adjustmentRule_0)
        {
            return new DaylightTime(smethod_0(int_2, adjustmentRule_0.DaylightTransitionStart), smethod_0(int_2, adjustmentRule_0.DaylightTransitionEnd), adjustmentRule_0.DaylightDelta);
        }

        private static DateTime smethod_2(long long_0, TimeZoneInfo timeZoneInfo_0)
        {
            DateTime maxValue;
            if (long_0 > DateTime.MaxValue.Ticks)
            {
                maxValue = DateTime.MaxValue;
            }
            else if (long_0 < DateTime.MinValue.Ticks)
            {
                maxValue = DateTime.MinValue;
            }
            else
            {
                maxValue = new DateTime(long_0);
            }
            TimeSpan span = smethod_3(maxValue, timeZoneInfo_0);
            long_0 += span.Ticks;
            if (long_0 > DateTime.MaxValue.Ticks)
            {
                return DateTime.MaxValue;
            }
            if (long_0 < DateTime.MinValue.Ticks)
            {
                return DateTime.MinValue;
            }
            return new DateTime(long_0);
        }

        private static TimeSpan smethod_3(DateTime dateTime_0, TimeZoneInfo timeZoneInfo_0)
        {
            bool flag;
            return smethod_4(dateTime_0, timeZoneInfo_0, out flag);
        }

        private static TimeSpan smethod_4(DateTime dateTime_0, TimeZoneInfo timeZoneInfo_0, out bool bool_1)
        {
            AdjustmentRule rule;
            int year;
            bool_1 = false;
            TimeSpan baseUtcOffset = timeZoneInfo_0.BaseUtcOffset;
            if (dateTime_0 > new DateTime(0x270f, 12, 0x1f))
            {
                rule = timeZoneInfo_0.method_0(DateTime.MaxValue);
                year = 0x270f;
            }
            else if (dateTime_0 < new DateTime(1, 1, 2))
            {
                rule = timeZoneInfo_0.method_0(DateTime.MinValue);
                year = 1;
            }
            else
            {
                DateTime time = dateTime_0 + baseUtcOffset;
                year = dateTime_0.Year;
                rule = timeZoneInfo_0.method_0(time);
            }
            if (rule != null)
            {
                bool_1 = smethod_5(dateTime_0, year, timeZoneInfo_0.BaseUtcOffset, rule);
                baseUtcOffset += bool_1 ? rule.DaylightDelta : TimeSpan.Zero;
            }
            return baseUtcOffset;
        }

        private static bool smethod_5(DateTime dateTime_0, int int_2, TimeSpan timeSpan_0, AdjustmentRule adjustmentRule_0)
        {
            if (adjustmentRule_0 == null)
            {
                return false;
            }
            TimeSpan span = timeSpan_0;
            DaylightTime time = smethod_1(int_2, adjustmentRule_0);
            DateTime time2 = time.Start - span;
            DateTime time3 = (time.End - span) - adjustmentRule_0.DaylightDelta;
            return smethod_7(time2, dateTime_0, time3);
        }

        private static bool smethod_6(DateTime dateTime_0, AdjustmentRule adjustmentRule_0, DaylightTime daylightTime_0)
        {
            if (adjustmentRule_0 == null)
            {
                return false;
            }
            bool flag = adjustmentRule_0.DaylightDelta > TimeSpan.Zero;
            DateTime time = daylightTime_0.Start + (flag ? adjustmentRule_0.DaylightDelta : TimeSpan.Zero);
            DateTime time2 = daylightTime_0.End + (flag ? -adjustmentRule_0.DaylightDelta : TimeSpan.Zero);
            return smethod_7(time, dateTime_0, time2);
        }

        private static bool smethod_7(DateTime dateTime_0, DateTime dateTime_1, DateTime dateTime_2)
        {
            if (dateTime_0.Year != dateTime_2.Year)
            {
                dateTime_2 = dateTime_2.AddYears(dateTime_0.Year - dateTime_2.Year);
            }
            if (dateTime_0.Year != dateTime_1.Year)
            {
                dateTime_1 = dateTime_1.AddYears(dateTime_0.Year - dateTime_1.Year);
            }
            if (dateTime_0 > dateTime_2)
            {
                if (dateTime_1 >= dateTime_2)
                {
                    return (dateTime_1 >= dateTime_0);
                }
                return true;
            }
            return ((dateTime_1 >= dateTime_0) && (dateTime_1 < dateTime_2));
        }

        public List<AdjustmentRule> AdjustmentRules
        {
            get
            {
                return this.list_0;
            }
            set
            {
                this.list_0 = value;
            }
        }

        [XmlIgnore]
        public TimeSpan BaseUtcOffset
        {
            get
            {
                return new TimeSpan(this.int_0, this.int_1, 0);
            }
        }

        public int BaseUtcOffsetHours
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

        public int BaseUtcOffsetMinutes
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

        public string Id
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        public bool IsDefaultTimeZone
        {
            get
            {
                return (this.Id == "By default");
            }
        }

        public bool SupportsDaylightSavingTime
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

        public class AdjustmentRule
        {
            private DateTime dateTime_0;
            private DateTime dateTime_1;
            private int int_0;
            private int int_1;
            private TimeZoneInfo.TransitionTime transitionTime_0;
            private TimeZoneInfo.TransitionTime transitionTime_1;

            public DateTime DateEnd
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

            public DateTime DateStart
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

            [XmlIgnore]
            public TimeSpan DaylightDelta
            {
                get
                {
                    return new TimeSpan(this.int_0, this.int_1, 0);
                }
            }

            public int DaylightDeltaHours
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

            public int DaylightDeltaMinutes
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

            public TimeZoneInfo.TransitionTime DaylightTransitionEnd
            {
                get
                {
                    return this.transitionTime_0;
                }
                set
                {
                    this.transitionTime_0 = value;
                }
            }

            public TimeZoneInfo.TransitionTime DaylightTransitionStart
            {
                get
                {
                    return this.transitionTime_1;
                }
                set
                {
                    this.transitionTime_1 = value;
                }
            }
        }

        public class TransitionTime
        {
            private bool bool_0;
            private DateTime dateTime_0;
            private System.DayOfWeek dayOfWeek_0;
            private int int_0;
            private int int_1;
            private int int_2;

            public int Day
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

            public System.DayOfWeek DayOfWeek
            {
                get
                {
                    return this.dayOfWeek_0;
                }
                set
                {
                    this.dayOfWeek_0 = value;
                }
            }

            public bool IsFixedRule
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

            public int Month
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

            public DateTime TimeOfDay
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

            public int Week
            {
                get
                {
                    return this.int_2;
                }
                set
                {
                    this.int_2 = value;
                }
            }
        }
    }
}

