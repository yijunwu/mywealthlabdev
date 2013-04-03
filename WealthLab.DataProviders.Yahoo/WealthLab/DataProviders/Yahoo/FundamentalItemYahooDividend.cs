namespace WealthLab.DataProviders.Yahoo
{
    using System;
    using System.Drawing;
    using WealthLab;
    using WealthLab.DataProviders.Yahoo.Properties;

    public class FundamentalItemYahooDividend : FundamentalItem
    {
        public FundamentalItemYahooDividend(string name) : base(name)
        {
        }

        public override string FormatValue()
        {
            if (base.Value < 0.01)
            {
                return (base.Value.ToString("C4") + " per share Dividend (" + base.Date.ToShortDateString() + ")");
            }
            return (base.Value.ToString("C") + " per share Dividend (" + base.Date.ToShortDateString() + ")");
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Dividend;
            }
        }
    }
}

