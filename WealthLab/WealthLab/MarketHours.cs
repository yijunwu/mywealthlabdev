namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    [ToolboxBitmap(typeof(MarketHours), "MarketHours")]
    public class MarketHours : Component
    {
        private static bool bool_0 = false;
        private DateTime dateTime_0;
        private IContainer components;
        private static List<MarketInfo> markets = new List<MarketInfo>();
        private MarketInfo marketInfo;
        private static string rootPath = "";

        public MarketHours()
        {
            this.dateTime_0 = DateTime.Now.Date;
            this.method_0();
        }

        public MarketHours(IContainer container)
        {
            this.dateTime_0 = DateTime.Now.Date;
            container.Add(this);
            this.method_0();
        }

        public DateTime AdvanceToNextMarketOpen(DateTime dateTime_1)
        {
            DateTime time = new DateTime(dateTime_1.Year, dateTime_1.Month, dateTime_1.Day, this.MarketOpenTimeNative.Hour, this.MarketOpenTimeNative.Minute, this.MarketOpenTimeNative.Second);
            do
            {
                time = time.AddDays(1.0);
            }
            while (!this.IsTradingDay(time));
            MarketSpecialHours hours = this.method_1(time);
            if (hours != null)
            {
                DateTime openTimeNative = hours.OpenTimeNative;
                time = new DateTime(time.Year, time.Month, time.Day, openTimeNative.Hour, openTimeNative.Minute, openTimeNative.Second);
            }
            return time;
        }

        public bool AfterMarketClose(DateTime dateTime_1)
        {
            DateTime time = TimeZoneInformation.ToLocalTime(TimeZoneInformation.CurrentTimeZone.Name, dateTime_1, this.Market.TimeZoneName);
            return this.AfterMarketCloseNative(time);
        }

        public bool AfterMarketCloseNative(DateTime dateTime_1)
        {
            DateTime marketCloseTimeNative = this.MarketCloseTimeNative;
            MarketSpecialHours hours = this.method_1(dateTime_1);
            if (hours != null)
            {
                marketCloseTimeNative = hours.CloseTimeNative;
            }
            if (!this.IsTradingDay(dateTime_1))
            {
                return false;
            }
            if (dateTime_1.Hour == marketCloseTimeNative.Hour)
            {
                return (dateTime_1.Minute > marketCloseTimeNative.Minute);
            }
            return (dateTime_1.Hour > marketCloseTimeNative.Hour);
        }

        public DateTime ConvertLocalTimeToNative(DateTime localTime)
        {
            return TimeZoneInformation.ToLocalTime(localTime.ToUniversalTime(), this.Market.TimeZoneName);
        }

        public DateTime ConvertNativeTimeToLocal(DateTime nativeTime)
        {
            nativeTime = TimeZoneInformation.ToUniversalTime(this.Market.TimeZoneName, nativeTime);
            return nativeTime.ToLocalTime();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void GetMarketHoursForDate(DateTime dateTime_1, ref DateTime openTime, ref DateTime closeTime)
        {
            dateTime_1 = dateTime_1.Date;
            using (List<MarketSpecialHours>.Enumerator enumerator = this.Market.SpecialHours.GetEnumerator())
            {
                MarketSpecialHours current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Date == dateTime_1)
                    {
                        ///goto  Label_003E; ///WYJ fix, simplify the flow
                        openTime = current.OpenTimeNative;
                        closeTime = current.CloseTimeNative;
                        return;
                    }
                }
            }
            openTime = this.MarketOpenTimeNative;
            closeTime = this.MarketCloseTimeNative;
        }

        public DateTime GetNextTimeStamp(DateTime timeStamp, BarDataScale scale)
        {
            int num;
            int num5;
            if (timeStamp == DateTime.MinValue)
            {
                timeStamp = TimeZoneInformation.ToLocalTime(DateTime.Now.ToUniversalTime(), this.Market.TimeZoneName);
            }
            DateTime time2 = timeStamp;
            switch (scale.Scale)
            {
                case BarScale.Daily:
                    return this.AdvanceToNextMarketOpen(time2);

                case BarScale.Weekly:
                    time2 = this.AdvanceToNextMarketOpen(time2);
                    while (time2.DayOfWeek != DayOfWeek.Friday)
                    {
                        time2 = time2.AddDays(1.0);
                    }
                    if (this.IsTradingDay(time2))
                    {
                        return time2;
                    }
                    if (time2.Day == (timeStamp.Day - 1))
                    {
                        time2 = time2.AddDays(7.0);
                        if (!this.IsTradingDay(time2))
                        {
                            time2 = time2.AddDays(-1.0);
                        }
                        return time2;
                    }
                    return time2.AddDays(-1.0);

                case BarScale.Monthly:
                {
                    int year = time2.Year;
                    int month = time2.Month + 1;
                    if (month == 13)
                    {
                        month = 1;
                        year++;
                    }
                    time2 = new DateTime(year, month, 1, 0, 0, 0);
                    time2 = this.AdvanceToNextMarketOpen(time2).AddDays(-1.0);
                    while (!this.IsTradingDay(time2))
                    {
                        time2 = time2.AddDays(-1.0);
                    }
                    return time2;
                }
                case BarScale.Minute:
                {
                    DateTime time9 = time2;
                    DateTime time8 = time2;
                    time2 = new DateTime(time8.Year, time8.Month, time8.Day, this.MarketOpenTimeNative.Hour, this.MarketOpenTimeNative.Minute, this.MarketOpenTimeNative.Second);
                    while (time2 <= time8)
                    {
                        time2 = time2.AddMinutes((double) scale.BarInterval);
                    }
                    if (!this.AfterMarketCloseNative(time2))
                    {
                        return time2;
                    }
                    if ((time9.TimeOfDay < this.MarketCloseTimeNative.TimeOfDay) && this.IsMarketOpenNow)
                    {
                        return (time2.Date + this.MarketCloseTimeNative.TimeOfDay);
                    }
                    return this.AdvanceToNextMarketOpen(time2).AddMinutes((double) scale.BarInterval);
                }
                case BarScale.Second:
                {
                    DateTime time4 = time2;
                    time2 = new DateTime(time4.Year, time4.Month, time4.Day, this.MarketOpenTimeNative.Hour, this.MarketOpenTimeNative.Minute, this.MarketOpenTimeNative.Second);
                    while (time2 <= time4)
                    {
                        time2 = time2.AddSeconds((double) scale.BarInterval);
                    }
                    if (this.AfterMarketCloseNative(time2))
                    {
                        time2 = this.AdvanceToNextMarketOpen(time2).AddSeconds((double) scale.BarInterval);
                    }
                    return time2;
                }
                case BarScale.Tick:
                    throw new ArgumentException("Tick scale is not supported");

                case BarScale.Quarterly:
                    num5 = time2.Year;
                    num = time2.Month + 1;
                    switch (num)
                    {
                        case 4:
                        case 5:
                        case 6:
                            num = 6;
                            //goto  Label_0342;
                            break;

                        case 7:
                        case 8:
                        case 9:
                            num = 9;
                            //goto  Label_0342;
                            break;

                        case 13:
                            num = 1;
                            num5++;
                            ///goto  Label_0337; ///WYJ fix, simplify the flow
                            num = 3;
                            //goto  Label_0342;
                            break;
                        default:
                            num = 12;
                            //goto  Label_0342;
                            break;
                    }
                    
                    //Label_0342:
                    time2 = new DateTime(num5, num, 0x1c);
                    do
                    {
                        time2 = time2.AddDays(1.0);
                    }
                    while (time2.Month == num);
                    time2 = time2.AddDays(-1.0);
                    time2 = this.AdvanceToNextMarketOpen(time2);
                    while (!this.IsTradingDay(time2))
                    {
                        time2 = time2.AddDays(-1.0);
                    }
                    return time2;

                case BarScale.Yearly:
                    time2 = new DateTime(time2.Year + 1, 12, 0x1f);
                    time2 = this.AdvanceToNextMarketOpen(time2);
                    while (!this.IsTradingDay(time2))
                    {
                        time2 = time2.AddDays(-1.0);
                    }
                    return time2;

                default:
                    return time2;
            }
        }

        public bool IsMarketOpen(DateTime dateTime_1)
        {
            if (!this.IsTodayTradingDay)
            {
                return false;
            }
            dateTime_1 = this.ConvertLocalTimeToNative(dateTime_1);
            DateTime marketOpenTimeNative = this.MarketOpenTimeNative;
            DateTime marketCloseTimeNative = this.MarketCloseTimeNative;
            MarketSpecialHours hours = this.method_1(dateTime_1);
            if (hours != null)
            {
                marketOpenTimeNative = hours.OpenTimeNative;
                marketCloseTimeNative = hours.CloseTimeNative;
            }
            return ((dateTime_1.TimeOfDay >= marketOpenTimeNative.TimeOfDay) && (dateTime_1.TimeOfDay < marketCloseTimeNative.TimeOfDay));
        }

        public bool IsTradingDay(DateTime dateTime_1)
        {
            if ((dateTime_1.DayOfWeek == DayOfWeek.Saturday) || (dateTime_1.DayOfWeek == DayOfWeek.Sunday))
            {
                return false;
            }
            if (this.Market != null)
            {
                return !this.Market.Holidays.Contains(dateTime_1.Date);
            }
            return true;
        }

        public static void LoadConfiguration()
        {
            markets.Clear();
            string path = rootPath + @"\Markets.xml";
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<MarketInfo>));
                TextReader textReader = new StreamReader(path);
                markets = (List<MarketInfo>) serializer.Deserialize(textReader);
                textReader.Close();
            }
        }

        private void method_0()
        {
            this.components = new Container();
        }

        private MarketSpecialHours method_1(DateTime dateTime_1)
        {
            MarketSpecialHours hours2;
            using (List<MarketSpecialHours>.Enumerator enumerator = this.Market.SpecialHours.GetEnumerator())
            {
                MarketSpecialHours current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Date == dateTime_1.Date)
                    {
                        ///goto  Label_003B;  ///WYJ fix, simplify the flow
                        hours2 = current;
                        return hours2;
                    }
                }
                return null;
            }
        }

        public static void SaveConfiguration()
        {
            if ((rootPath != null) && (rootPath != ""))
            {
                string path = rootPath + @"\Markets.xml";
                XmlSerializer serializer = new XmlSerializer(typeof(List<MarketInfo>));
                TextWriter textWriter = new StreamWriter(path);
                serializer.Serialize(textWriter, markets);
                textWriter.Close();
            }
        }

        public bool AfterMarketCloseNow
        {
            get
            {
                return this.AfterMarketClose(DateTime.Now);
            }
        }

        public bool IsMarketOpenNow
        {
            get
            {
                return this.IsMarketOpen(DateTime.Now);
            }
        }

        public bool IsTodayTradingDay
        {
            get
            {
                return this.IsTradingDay(DateTime.Now);
            }
        }

        public DateTime LastTradingSessionEnded
        {
            get
            {
                DateTime now = DateTime.Now;
                if (this.IsTradingDay(now) && !this.AfterMarketClose(now))
                {
                    now = now.AddDays(-1.0);
                }
                while (!this.IsTradingDay(now))
                {
                    now = now.AddDays(-1.0);
                }
                now = new DateTime(now.Year, now.Month, now.Day, this.MarketCloseTimeNative.Hour, this.MarketCloseTimeNative.Minute, 0);
                return this.ConvertNativeTimeToLocal(now).ToUniversalTime();
            }
        }

        public DateTime LastTradingSessionEndedLocal
        {
            get
            {
                return this.LastTradingSessionEnded.ToLocalTime();
            }
        }

        public DateTime LastTradingSessionEndedNative
        {
            get
            {
                return TimeZoneInformation.ToLocalTime(this.LastTradingSessionEnded, this.Market.TimeZoneName);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MarketInfo Market
        {
            get
            {
                if (this.marketInfo == null)
                {
                    using (IEnumerator<MarketInfo> enumerator = Markets.GetEnumerator())
                    {
                        MarketInfo current;
                        while (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            if (current.Name == "US Equities")
                            {
                                ///goto  Label_003B; ///WYJ fix, simplify the flow
                                this.marketInfo = current;
                                break;
                            }
                        }
                    }
                }
                if (this.marketInfo == null)
                {
                    MarketInfo info = new MarketInfo {
                        Name = "US Equities",
                        Description = "NYSE, Nasdaq and Amex equities",
                        OpenTimeNative = new DateTime(this.dateTime_0.Year, this.dateTime_0.Month, this.dateTime_0.Day, 9, 30, 0),
                        CloseTimeNative = new DateTime(this.dateTime_0.Year, this.dateTime_0.Month, this.dateTime_0.Day, 0x10, 0, 0),
                        TimeZoneName = "Eastern Standard Time"
                    };
                    for (int i = 0x7d7; i < 0x7e4; i++)
                    {
                        DateTime time;
                        info.Holidays.Add(new DateTime(i, 7, 4));
                        info.Holidays.Add(new DateTime(i, 12, 0x19));
                        for (int j = 1; j < 30; j++)
                        {
                            time = new DateTime(i, 9, j);
                            if (time.DayOfWeek == DayOfWeek.Monday)
                            {
                                ///goto  Label_0140;  ///WYJ fix, simplify the flow
                                info.Holidays.Add(time);
                                break;
                            }
                        }
                    }
                    this.marketInfo = info;
                    Markets.Add(this.marketInfo);
                }
                return this.marketInfo;
            }
            set
            {
                this.marketInfo = value;
            }
        }

        public DateTime MarketCloseTimeLocal
        {
            get
            {
                return this.ConvertNativeTimeToLocal(this.MarketCloseTimeNative);
            }
        }

        public DateTime MarketCloseTimeNative
        {
            get
            {
                return this.Market.CloseTimeNative;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public string MarketName
        {
            get
            {
                return this.Market.Name;
            }
            set
            {
                using (IEnumerator<MarketInfo> enumerator = Markets.GetEnumerator())
                {
                    MarketInfo current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (current.Name == value)
                        {
                            ///goto  Label_002D;  ///WYJ fix, simplify the flow
                            this.marketInfo = current;
                            return;
                        }
                    }
                    return;
                }
            }
        }

        public DateTime MarketOpenTimeLocal
        {
            get
            {
                return this.ConvertNativeTimeToLocal(this.MarketOpenTimeNative);
            }
        }

        public DateTime MarketOpenTimeNative
        {
            get
            {
                return this.Market.OpenTimeNative;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public static IList<MarketInfo> Markets
        {
            get
            {
                if ((markets.Count == 0) && !bool_0)
                {
                    bool_0 = true;
                    if (RootPath == "")
                    {
                        RootPath = Path.GetDirectoryName(Application.ExecutablePath);
                    }
                }
                return markets;
            }
        }

        public static string RootPath
        {
            get
            {
                return rootPath;
            }
            set
            {
                rootPath = value;
                if ((rootPath != null) && (rootPath != ""))
                {
                    LoadConfiguration();
                }
            }
        }
    }
}

