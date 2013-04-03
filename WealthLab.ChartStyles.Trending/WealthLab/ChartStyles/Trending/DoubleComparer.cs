namespace WealthLab.ChartStyles.Trending
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DoubleComparer
    {
        public const double PrecisionDigits = 10000000000000;
        public double double_0;
        public DoubleComparer(double double_1)
        {
            this.double_0 = double_1;
        }

        public static implicit operator DoubleComparer(double value)
        {
            return new DoubleComparer(value);
        }

        public static explicit operator double(DoubleComparer mydbl)
        {
            return mydbl.double_0;
        }

        public static bool operator ==(DoubleComparer dbl1, DoubleComparer dbl2)
        {
            if (Math.Abs((double) (dbl1.double_0 - dbl2.double_0)) >= (Math.Abs(dbl1.double_0) / 10000000000000))
            {
                return false;
            }
            return true;
        }

        public static bool operator !=(DoubleComparer dbl1, DoubleComparer dbl2)
        {
            if (Math.Abs((double) (dbl1.double_0 - dbl2.double_0)) <= (Math.Abs(dbl1.double_0) / 10000000000000))
            {
                return false;
            }
            return true;
        }

        public static bool operator >=(DoubleComparer dbl1, DoubleComparer dbl2)
        {
            return ((Math.Abs((double) (dbl1.double_0 - dbl2.double_0)) < (Math.Abs(dbl1.double_0) / 10000000000000)) || (dbl1 > dbl2));
        }

        public static bool operator <=(DoubleComparer dbl1, DoubleComparer dbl2)
        {
            return ((Math.Abs((double) (dbl1.double_0 - dbl2.double_0)) < (Math.Abs(dbl1.double_0) / 10000000000000)) || (dbl1 < dbl2));
        }

        public static bool operator >(DoubleComparer dbl1, DoubleComparer dbl2)
        {
            if (dbl1.double_0 <= dbl2.double_0)
            {
                return false;
            }
            return true;
        }

        public static bool operator <(DoubleComparer dbl1, DoubleComparer dbl2)
        {
            if (dbl1.double_0 >= dbl2.double_0)
            {
                return false;
            }
            return true;
        }

        public static DoubleComparer operator -(DoubleComparer dbl1, DoubleComparer dbl2)
        {
            return (dbl1.double_0 - dbl2.double_0);
        }

        public static DoubleComparer operator +(DoubleComparer dbl1, DoubleComparer dbl2)
        {
            return (dbl1.double_0 + dbl2.double_0);
        }

        public static DoubleComparer operator /(DoubleComparer dbl1, DoubleComparer dbl2)
        {
            return (dbl1.double_0 / dbl2.double_0);
        }

        public static DoubleComparer operator *(DoubleComparer dbl1, DoubleComparer dbl2)
        {
            return (dbl1.double_0 * dbl2.double_0);
        }

        public override bool Equals(object object_0)
        {
            try
            {
                return (this == ((DoubleComparer) object_0));
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.double_0.GetHashCode();
        }
    }
}

