using System;
using System.Collections.Generic;
using System.Drawing;
using WealthLab.DataProviders.Msn.Properties;
namespace WealthLab.DataProviders.Msn
{
	public class MsnFundamentalProvider : FundamentalDataProvider
	{
		private static readonly string string_0 = "Split (MSN)";
		private static readonly string string_1 = "Dividend (MSN)";
		public FundamentalDataStore _fds;
		private string[] string_2;
		private string[] string_3 = new string[0];
		private object object_0 = Delegate3.smethod_0();
		public static string SplitName
		{
			get
			{
				return MsnFundamentalProvider.string_0;
			}
		}
		public static string DividendName
		{
			get
			{
				return MsnFundamentalProvider.string_1;
			}
		}
		public override IList<string> NonSymbolSpecificItemsProvided
		{
			get
			{
				return this.string_3;
			}
		}
		public override IList<string> SymbolSpecificItemsProvided
		{
			get
			{
				return this.string_2;
			}
		}
		public override string Description
		{
			get
			{
				return "Delivers historical split and dividend data for securities.";
			}
		}
		public override string FriendlyName
		{
			get
			{
				return "MSN! Fundamental data";
			}
		}
		public override Bitmap Glyph
		{
			get
			{
				return Resources.MSN;
			}
		}
		public MsnFundamentalProvider()
		{
			this.string_2 = new string[]
			{
				MsnFundamentalProvider.string_0,
				MsnFundamentalProvider.string_1
			};
		}
		public override FundamentalItem CreateItem(string itemName)
		{
			if (Delegate51.smethod_0(itemName, MsnFundamentalProvider.string_0))
			{
				return new FundamentalItemMsnSplit(MsnFundamentalProvider.string_0);
			}
			if (Delegate51.smethod_0(itemName, MsnFundamentalProvider.string_1))
			{
				return new FundamentalItemMsnDividend(MsnFundamentalProvider.string_1);
			}
			return Delegate4.smethod_0(itemName);
		}
		public override void Initialize(IDataHost dataHost)
		{
			Delegate52.smethod_0(this, dataHost);
			this._fds = Delegate5.smethod_0(dataHost, this);
		}
		public override IList<FundamentalItem> RequestItems(string itemName)
		{
			return Delegate53.smethod_0(this._fds, itemName);
		}
		public void UpdateData(string symbol, List<FundamentalItem> items)
		{
			bool flag = false;
			try
			{
				object obj;
				Delegate54.smethod_0(obj = this.object_0, ref flag);
				if (items.Count > 0)
				{
					string text = Delegate55.smethod_0(items[0]);
					Delegate56.smethod_0(this._fds, symbol, text, items);
					Delegate57.smethod_0(this._fds);
				}
			}
			finally
			{
				if (flag)
				{
					object obj;
					Delegate58.smethod_0(obj);
				}
			}
		}
		public override IList<FundamentalItem> RequestItems(string symbol, string itemName)
		{
			return Delegate59.smethod_0(this._fds, symbol, itemName);
		}
	}
}
