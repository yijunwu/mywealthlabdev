namespace WealthLab.PosSizers
{
    using System;
    using System.Collections.Generic;
    using WealthLab;

    public class CompareByFriendlyName : IComparer<PosSizer>
    {
        public int Compare(PosSizer posSizer_0, PosSizer posSizer_1)
        {
            return posSizer_0.FriendlyName.CompareTo(posSizer_1.FriendlyName);
        }
    }
}

