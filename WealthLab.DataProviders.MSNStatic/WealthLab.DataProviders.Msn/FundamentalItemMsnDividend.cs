using System;
using System.Drawing;
using WealthLab.DataProviders.Msn.Properties;
namespace WealthLab.DataProviders.Msn
{
	public class FundamentalItemMsnDividend : FundamentalItem
	{
		public override Bitmap Glyph
		{
			get
			{
				return Resources.Dividend;
			}
		}
		public FundamentalItemMsnDividend(string name) : base(name)
		{
		}
		public override string FormatValue()
		{
			if (base.Value < 0.01)
			{
				return base.Value.ToString("C4") + " per share Dividend (" + base.Date.ToShortDateString() + ")";
			}
			return base.Value.ToString("C") + " per share Dividend (" + base.Date.ToShortDateString() + ")";
		}
	}
}
