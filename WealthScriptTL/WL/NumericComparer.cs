namespace WL
{
    using System;
    using System.Collections;

    public class NumericComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            double num = (double) x;
            double num2 = (double) y;
            return num.CompareTo(num2);
        }
    }
}

