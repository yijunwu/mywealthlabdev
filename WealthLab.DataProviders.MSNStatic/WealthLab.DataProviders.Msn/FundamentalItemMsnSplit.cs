using System;
using System.Drawing;
using WealthLab.DataProviders.Msn.Properties;
namespace WealthLab.DataProviders.Msn
{
	public class FundamentalItemMsnSplit : FundamentalItem
	{
		public override Bitmap Glyph
		{
			get
			{
				return Resources.Split;
			}
		}
		public FundamentalItemMsnSplit(string name) : base(name)
		{
		}
		public override string FormatValue()
		{
			int num = 1;
			double value = base.Value;
			double num2 = (double)1 * value - (double)((int)((double)1 * value + 0.5));
			while (num2 * num2 > 0.00025)
			{
				num++;
				num2 = (double)num * value - (double)((int)((double)num * value + 0.5));
				if (num > 1000)
				{
					break;
				}
			}
			object[] args = new object[]
			{
				((int)((double)num * value + 0.5)).ToString(),
				':',
				num,
				" Stock Split (" + base.Date.ToShortDateString() + ")"
			};
			return string.Concat(args);
		}
	}
}
