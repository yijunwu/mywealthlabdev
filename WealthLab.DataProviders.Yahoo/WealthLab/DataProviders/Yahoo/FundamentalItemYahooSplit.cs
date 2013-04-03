namespace WealthLab.DataProviders.Yahoo
{
    using System;
    using System.Drawing;
    using WealthLab;
    using WealthLab.DataProviders.Yahoo.Properties;

    public class FundamentalItemYahooSplit : FundamentalItem
    {
        public FundamentalItemYahooSplit(string name) : base(name)
        {
        }

        public override string FormatValue()
        {
            int num = 1;
            double num2 = base.Value;
            double num3 = (1 * num2) - ((int) ((1 * num2) + 0.5));
            while ((num3 * num3) > 0.00025)
            {
                num++;
                num3 = (num * num2) - ((int) ((num * num2) + 0.5));
                if (num > 0x3e8)
                {
                    break;
                }
            }
            object[] objArray2 = new object[] { ((int) ((num * num2) + 0.5)).ToString(), ':', num, " Stock Split (" + base.Date.ToShortDateString() + ")" };
            return string.Concat(objArray2);
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Split;
            }
        }
    }
}

