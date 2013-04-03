namespace WealthLab
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct BarDataScale
    {
        private BarScale barScale_0;
        private int int_0;
        public BarDataScale(BarScale scale, int barInterval)
        {
            this.barScale_0 = scale;
            this.int_0 = barInterval;
        }

        public BarScale Scale
        {
            get
            {
                return this.barScale_0;
            }
            set
            {
                this.barScale_0 = value;
            }
        }
        public int BarInterval
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
        public bool IsIntraday
        {
            get
            {
                if ((this.Scale != BarScale.Minute) && (this.Scale != BarScale.Second))
                {
                    return (this.Scale == BarScale.Tick);
                }
                return true;
            }
        }
        public override string ToString()
        {
            string str;
            if (this.IsIntraday)
            {
                str = this.int_0 + " ";
            }
            else
            {
                str = "";
            }
            return (str + this.Scale.ToString());
        }

        public static BarDataScale Parse(string string_0)
        {
            BarScale scale;
            if (string_0 == null)
            {
                throw new ArgumentNullException();
            }
            string[] strArray = string_0.Split(new char[] { ' ' });
            if (strArray.Length == 0)
            {
                throw new ArgumentException();
            }
            int barInterval = 0;
            if (strArray.Length == 1)
            {
                scale = (BarScale) Enum.Parse(typeof(BarScale), strArray[0]);
            }
            else
            {
                barInterval = int.Parse(strArray[0]);
                scale = (BarScale) Enum.Parse(typeof(BarScale), strArray[1]);
            }
            return new BarDataScale(scale, barInterval);
        }

        public bool CanConvertTo(BarDataScale barDataScale_0)
        {
            if ((this.Scale == barDataScale_0.Scale) && !this.IsIntraday)
            {
                return true;
            }
            switch (this.Scale)
            {
                case BarScale.Daily:
                    return ((((barDataScale_0.Scale == BarScale.Weekly) || (barDataScale_0.Scale == BarScale.Monthly)) || (barDataScale_0.Scale == BarScale.Quarterly)) || (barDataScale_0.Scale == BarScale.Yearly));

                case BarScale.Weekly:
                    return (((barDataScale_0.Scale == BarScale.Monthly) || (barDataScale_0.Scale == BarScale.Quarterly)) || (barDataScale_0.Scale == BarScale.Yearly));

                case BarScale.Monthly:
                    return ((barDataScale_0.Scale == BarScale.Quarterly) || (barDataScale_0.Scale == BarScale.Yearly));

                case BarScale.Quarterly:
                    return (barDataScale_0.Scale == BarScale.Yearly);

                case BarScale.Yearly:
                    return false;
            }
            return (!barDataScale_0.IsIntraday || ((barDataScale_0.Scale == this.Scale) && ((barDataScale_0.BarInterval % this.BarInterval) == 0)));
        }

        public override bool Equals(object object_0)
        {
            if (!(object_0 is BarDataScale))
            {
                return base.Equals(object_0);
            }
            BarDataScale scale = (BarDataScale) object_0;
            return ((scale.Scale == this.Scale) && (scale.BarInterval == this.BarInterval));
        }

        public static bool operator ==(BarDataScale bds1, BarDataScale bds2)
        {
            return ((bds1.Scale == bds2.Scale) && (bds1.BarInterval == bds2.BarInterval));
        }

        public static bool operator !=(BarDataScale bds1, BarDataScale bds2)
        {
            if (bds1.Scale == bds2.Scale)
            {
                return (bds1.BarInterval != bds2.BarInterval);
            }
            return true;
        }

        public static bool operator <(BarDataScale barDataScale_0, BarDataScale barDataScale_1)
        {
            return ((barDataScale_0 != barDataScale_1) && (barDataScale_0 <= barDataScale_1));
        }

        public static bool operator >(BarDataScale barDataScale_0, BarDataScale barDataScale_1)
        {
            switch (barDataScale_0.Scale)
            {
                case BarScale.Daily:
                    return barDataScale_1.IsIntraday;

                case BarScale.Weekly:
                    return (barDataScale_1.IsIntraday || (barDataScale_1.Scale == BarScale.Daily));

                case BarScale.Monthly:
                    return ((barDataScale_1.IsIntraday || (barDataScale_1.Scale == BarScale.Daily)) || (barDataScale_1.Scale == BarScale.Weekly));

                case BarScale.Minute:
                    if ((barDataScale_1.Scale == BarScale.Second) || (barDataScale_1.Scale == BarScale.Tick))
                    {
                        return true;
                    }
                    if (barDataScale_1.Scale != BarScale.Minute)
                    {
                        return false;
                    }
                    return (barDataScale_0.BarInterval > barDataScale_1.BarInterval);

                case BarScale.Second:
                    if (barDataScale_1.Scale != BarScale.Tick)
                    {
                        return ((barDataScale_1.Scale == BarScale.Second) && (barDataScale_0.BarInterval > barDataScale_1.BarInterval));
                    }
                    return true;

                case BarScale.Tick:
                    if (barDataScale_1.Scale != BarScale.Tick)
                    {
                        return false;
                    }
                    return (barDataScale_0.BarInterval > barDataScale_1.BarInterval);

                case BarScale.Quarterly:
                    return (((barDataScale_1.IsIntraday || (barDataScale_1.Scale == BarScale.Daily)) || (barDataScale_1.Scale == BarScale.Weekly)) || (barDataScale_1.Scale == BarScale.Monthly));

                case BarScale.Yearly:
                    return (barDataScale_1.Scale != BarScale.Yearly);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (this.Scale.GetHashCode() * this.BarInterval.GetHashCode());
        }
    }
}

