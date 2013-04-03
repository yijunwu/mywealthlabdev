namespace WealthLab.DataProviders.Msn.SecurityFundamental
{
    using System;
    using System.Drawing;
    using WealthLab;
    using WealthLab.DataProviders.Msn.Properties;

    internal class MSNFItem : FundamentalItem
    {
        public MSNFItem(string name) : base(name)
        {
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.MSN;
            }
        }
    }
}

