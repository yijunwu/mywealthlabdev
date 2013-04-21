using System;
using System.Collections.Generic;
using System.Drawing;
using WealthLab.DataProviders.Msn.Properties;
namespace WealthLab.DataProviders.Msn.SecurityFundamental
{
	public class MsnSecurityFundamentalProvider : FundamentalDataProvider
	{
		private FundamentalDataStore fundamentalDataStore_0;
		private Class5 class5_0;
		private bool bool_0;
		public override IList<string> NonSymbolSpecificItemsProvided
		{
			get
			{
				return new List<string>();
			}
		}
		public override IList<string> SymbolSpecificItemsProvided
		{
			get
			{
				List<string> list = new List<string>();
				list.AddRange(Class0.String_0);
				list.AddRange(Class0.String_1);
				list.AddRange(Class0.String_2);
				list.AddRange(Class0.String_3);
				return list;
			}
		}
		public override string Description
		{
			get
			{
				return "Fundamental items by MSN Moneycentral";
			}
		}
		public override string FriendlyName
		{
			get
			{
				return "MSN Moneycentral";
			}
		}
		public override Bitmap Glyph
		{
			get
			{
				return Resources.MSN;
			}
		}
		public override bool SupportsDataSourceUpdate
		{
			get
			{
				return true;
			}
		}
		public override bool SupportsProviderUpdate
		{
			get
			{
				return true;
			}
		}
		public override string URL
		{
			get
			{
				return "http://www2.wealth-lab.com/WL5WIKI/MSNFundamentalProvider.ashx";
			}
		}
		public override FundamentalItem CreateItem(string itemName)
		{
			return new MSNFItem(itemName);
		}
		public override IList<FundamentalItem> RequestItems(string itemName)
		{
			throw Delegate31.smethod_0("The method or operation is not implemented.");
		}
		public override IList<FundamentalItem> RequestItems(string symbol, string itemName)
		{
			return Delegate59.smethod_0(this.fundamentalDataStore_0, symbol, itemName);
		}
		private void method_0(string string_0, Dictionary<string, List<FundamentalItem>> dictionary_0)
		{
			foreach (KeyValuePair<string, List<FundamentalItem>> current in dictionary_0)
			{
				Delegate56.smethod_0(this.fundamentalDataStore_0, string_0, current.Key, current.Value);
			}
		}
		public override bool CanDragDropItem(string itemName)
		{
			return true;
		}
		public override string ItemPane(string itemName)
		{
			return itemName;
		}
		public override string ItemDescription(string itemName)
		{
			return Class0.smethod_0(itemName);
		}
		public override void Initialize(IDataHost dataHost)
		{
			Delegate52.smethod_0(this, dataHost);
			this.fundamentalDataStore_0 = Delegate5.smethod_0(dataHost, this);
		}
		private void method_1(string string_0, IDataUpdateMessage idataUpdateMessage_0)
		{
			if (this.class5_0 == null)
			{
				this.class5_0 = new Class5();
			}
			try
			{
				Class7 @class = this.class5_0.method_7(string_0, idataUpdateMessage_0);
				if (!this.bool_0)
				{
					this.method_0(string_0, @class.Dictionary_0);
					this.method_0(string_0, @class.Dictionary_1);
					this.method_0(string_0, @class.Dictionary_2);
					this.method_0(string_0, @class.Dictionary_3);
					Delegate57.smethod_0(this.fundamentalDataStore_0);
					Delegate184.smethod_0(idataUpdateMessage_0, Delegate140.smethod_0("{0}\t{1} fundamental items updated", string_0, @class.Dictionary_0.Count + @class.Dictionary_1.Count + @class.Dictionary_2.Count + @class.Dictionary_3.Count));
				}
			}
			catch (Exception object_)
			{
				Delegate184.smethod_0(idataUpdateMessage_0, Delegate140.smethod_0("{0}\t{1}", string_0, Delegate127.smethod_0(object_)));
			}
		}
		public override void UpdateDataSource(DataSource ds, IDataUpdateMessage dataUpdateMsg)
		{
			int num = 0;
			this.bool_0 = false;
			foreach (string current in Delegate311.smethod_0(ds))
			{
				this.method_1(current, dataUpdateMsg);
				num++;
				Delegate183.smethod_0(dataUpdateMsg, num * 100 / Delegate311.smethod_0(ds).Count);
				if (this.bool_0)
				{
					break;
				}
			}
		}
		public override void UpdateProvider(IDataUpdateMessage dataUpdateMsg, List<DataSource> dataSources, bool updateNonDSSymbols, bool deleteNonDSSymbols)
		{
			int num = 0;
			int num2 = 0;
			List<string> list = new List<string>();
			this.bool_0 = false;
			foreach (DataSource current in dataSources)
			{
				foreach (string current2 in Delegate311.smethod_0(current))
				{
					if (!list.Contains(current2))
					{
						list.Add(current2);
						num += Delegate311.smethod_0(current).Count;
					}
				}
			}
			list.Sort();
			foreach (string current3 in list)
			{
				this.method_1(current3, dataUpdateMsg);
				num2++;
				Delegate183.smethod_0(dataUpdateMsg, num2 * 100 / num);
				if (this.bool_0)
				{
					break;
				}
			}
		}
		public override void CancelUpdate()
		{
			this.bool_0 = true;
		}
	}
}
