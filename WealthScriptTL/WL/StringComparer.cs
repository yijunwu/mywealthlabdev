namespace WL
{
    using System;
    using System.Collections;

    public class StringComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            string str = (string) x;
            string strB = (string) y;
            return str.CompareTo(strB);
        }
    }
}

