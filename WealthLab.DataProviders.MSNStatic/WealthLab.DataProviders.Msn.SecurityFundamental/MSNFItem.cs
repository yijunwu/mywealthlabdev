using System;
using System.Drawing;
using WealthLab.DataProviders.Msn.Properties;
namespace WealthLab.DataProviders.Msn.SecurityFundamental
{
	internal class MSNFItem : FundamentalItem
	{
		public override Bitmap Glyph
		{
			get
			{
				return Resources.MSN;
			}
		}
		public MSNFItem(string name) : base(name)
		{
		}
	}
}
