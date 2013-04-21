using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.DataProviders.Helper;
using WealthLab.DataProviders.Msn.Properties;
namespace WealthLab.DataProviders.Msn
{
	public class MsnStaticProvider : StaticDataProvider
	{
		private Control0 control0_0;
		private Control2 control2_0;
		private Control1 control1_0;
		private int int_0;
		private int int_1;
		private IDataUpdateMessage idataUpdateMessage_0;
		private BarDataStore barDataStore_0;
		private bool bool_0;
		private Class1 class1_0;
		private object object_0 = Delegate3.smethod_0();
		private List<string> list_0 = new List<string>();
		private MsnFundamentalProvider msnFundamentalProvider_0;
		private Class12 class12_0;
		private static string string_0;
		private static MsnClientSettings msnClientSettings_0;
		private static Class8 class8_0;
		public static string DataPath
		{
			get
			{
				return MsnStaticProvider.string_0;
			}
		}
		public static MsnClientSettings ProviderSettings
		{
			get
			{
				return MsnStaticProvider.msnClientSettings_0;
			}
		}
		internal static Class8 ClassificationFile
		{
			get
			{
				return MsnStaticProvider.class8_0;
			}
		}
		public override bool CanModifySymbols
		{
			get
			{
				return true;
			}
		}
		public override bool CanDeleteSymbolDataFile
		{
			get
			{
				return true;
			}
		}
		public override bool CanEditSymbolDataFile
		{
			get
			{
				return true;
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
		public override string SuggestedDataSourceName
		{
			get
			{
				if (this.control0_0.Boolean_0)
				{
					return this.control1_0.String_0;
				}
				return Delegate204.smethod_0(this);
			}
		}
		public override BarDataStore DataStore
		{
			get
			{
				return this.barDataStore_0;
			}
		}
		public override string Description
		{
			get
			{
				return "MSN MoneyCentral end-of-day data";
			}
		}
		public override string FriendlyName
		{
			get
			{
				return "MSN";
			}
		}
		public override Bitmap Glyph
		{
			get
			{
				return Resources.MSN;
			}
		}
		public override string URL
		{
			get
			{
				return "http://moneycentral.msn.com";
			}
		}
		public MsnStaticProvider()
		{
			Log.TraceMethod(new object[0]);
		}
		private void method_0(object sender, EventArgs2 e)
		{
			Log.TraceMethod(new object[0]);
			if (e.class2_0 != null)
			{
				this.int_1++;
				this.method_5(e.class2_0.String_0, Delegate114.smethod_0("Error: ", Delegate127.smethod_0(e.exception_0)), Delegate169.smethod_0(Delegate168.smethod_0()));
				this.method_3();
				return;
			}
			this.method_4(Delegate120.smethod_0("Thread (", Delegate169.smethod_0(Delegate168.smethod_0()), ") execution error! ", Delegate127.smethod_0(e.exception_0)));
		}
		private void method_1(object sender, EventArgs1 e)
		{
			Log.TraceMethod(new object[0]);
			if ((e.class2_0.Enum1_0 & Enum1.History) != (Enum1)0)
			{
				if (e.bars_0 != null)
				{
					bool flag = false;
					try
					{
						List<string> list;
						Delegate54.smethod_0(list = this.list_0, ref flag);
						if (this.list_0.Contains(e.class2_0.String_0))
						{
							Delegate170.smethod_0(this.barDataStore_0, Class11.smethod_1(e.class2_0.String_0), BarScale.Daily, 0);
							this.list_0.Remove(e.class2_0.String_0);
						}
					}
					finally
					{
						if (flag)
						{
							List<string> list;
							Delegate58.smethod_0(list);
						}
					}
					Bars bars_ = Delegate24.smethod_0(Class11.smethod_1(e.class2_0.String_0), BarScale.Daily, 0);
					Delegate171.smethod_0(this.barDataStore_0, bars_);
					if (e.class3_0 != null)
					{
						this.method_2(ref bars_, e.class2_0.String_0, e.class3_0.List_1);
					}
					int int_ = Delegate172.smethod_0(bars_);
					int int_2;
					Delegate173.smethod_0(bars_, e.bars_0, ref int_2);
					Delegate175.smethod_0(bars_, Delegate174.smethod_0(e.bars_0));
					bool flag2 = false;
					try
					{
						object obj;
						Delegate54.smethod_0(obj = this.object_0, ref flag2);
						Delegate171.smethod_1(this.barDataStore_0, bars_);
					}
					finally
					{
						if (flag2)
						{
							object obj;
							Delegate58.smethod_0(obj);
						}
					}
					this.int_1++;
					this.method_6(bars_, int_, int_2, Delegate169.smethod_0(Delegate168.smethod_0()));
					this.method_3();
				}
				if (e.class3_0 != null)
				{
					this.msnFundamentalProvider_0.UpdateData(e.class2_0.String_0, e.class3_0.List_0);
					this.msnFundamentalProvider_0.UpdateData(e.class2_0.String_0, e.class3_0.List_1);
				}
			}
		}
		private void method_2(ref Bars bars_0, string string_1, IList<FundamentalItem> ilist_0)
		{
			IList<FundamentalItem> list = null;
			bool flag = false;
			try
			{
				MsnFundamentalProvider msnFundamentalProvider;
				Delegate54.smethod_0(msnFundamentalProvider = this.msnFundamentalProvider_0, ref flag);
				list = Delegate176.smethod_0(this.msnFundamentalProvider_0, string_1, MsnFundamentalProvider.SplitName);
			}
			finally
			{
				if (flag)
				{
					MsnFundamentalProvider msnFundamentalProvider;
					Delegate58.smethod_0(msnFundamentalProvider);
				}
			}
			if (list == null)
			{
				return;
			}
			DateTime dateTime = DateTime.MinValue;
			foreach (FundamentalItem current in list)
			{
				if (Delegate178.smethod_0(Delegate177.smethod_0(current), dateTime))
				{
					dateTime = Delegate177.smethod_0(current);
				}
			}
			double num = 1.0;
			if (Delegate178.smethod_0(dateTime, DateTime.MinValue))
			{
				foreach (FundamentalItem current2 in ilist_0)
				{
					if (Delegate178.smethod_0(Delegate177.smethod_0(current2), dateTime))
					{
						num *= Delegate179.smethod_0(current2);
					}
				}
			}
			if (num != 1.0)
			{
				Log.Trace(new object[]
				{
					"Split History ",
					string_1,
					num
				});
				for (int i = 0; i < Delegate172.smethod_0(bars_0); i++)
				{
					DataSeries dataSeries;
					int num2;
					Delegate182.smethod_0(dataSeries = Delegate180.smethod_0(bars_0), num2 = i, Delegate181.smethod_0(dataSeries, num2) * num);
					DataSeries dataSeries2;
					int num3;
					Delegate182.smethod_0(dataSeries2 = Delegate180.smethod_1(bars_0), num3 = i, Delegate181.smethod_0(dataSeries2, num3) * num);
					DataSeries dataSeries3;
					int num4;
					Delegate182.smethod_0(dataSeries3 = Delegate180.smethod_2(bars_0), num4 = i, Delegate181.smethod_0(dataSeries3, num4) * num);
					DataSeries dataSeries4;
					int num5;
					Delegate182.smethod_0(dataSeries4 = Delegate180.smethod_3(bars_0), num5 = i, Delegate181.smethod_0(dataSeries4, num5) * num);
					DataSeries dataSeries5;
					int num6;
					Delegate182.smethod_0(dataSeries5 = Delegate180.smethod_4(bars_0), num6 = i, Delegate181.smethod_0(dataSeries5, num6) * num);
				}
			}
		}
		private void method_3()
		{
			if (this.idataUpdateMessage_0 != null)
			{
				Delegate183.smethod_0(this.idataUpdateMessage_0, this.int_1 * 100 / this.int_0);
			}
		}
		private void method_4(string string_1)
		{
			if (this.idataUpdateMessage_0 != null)
			{
				Delegate184.smethod_0(this.idataUpdateMessage_0, string_1);
			}
		}
		private void method_5(string string_1, string string_2, string string_3)
		{
			if (this.idataUpdateMessage_0 != null)
			{
				Delegate184.smethod_0(this.idataUpdateMessage_0, Delegate186.smethod_0("{0,-4} {1,-9} {2}", Delegate185.smethod_0("[", string_3, "]"), string_1, string_2));
			}
		}
		private void method_6(Bars bars_0, int int_2, int int_3, string string_1)
		{
			if (this.idataUpdateMessage_0 != null)
			{
				string text = string.Empty;
				if (Delegate172.smethod_0(bars_0) > 0)
				{
					string text2 = Delegate187.smethod_0(bars_0)[Delegate172.smethod_0(bars_0) - 1].ToString("MM.dd.yyyy");
					text = Delegate188.smethod_0("{0,-4} {1,-9} {2,-14} {3,-15} {4,-18}", new object[]
					{
						Delegate185.smethod_0("[", string_1, "]"),
						Delegate174.smethod_1(bars_0),
						Delegate167.smethod_0(Delegate172.smethod_0(bars_0), " bars"),
						text2,
						Delegate167.smethod_0(Delegate172.smethod_0(bars_0) - int_2, " bars added")
					});
					if (int_3 > 0)
					{
						text = Delegate140.smethod_0("{0} {1,-18}", text, Delegate167.smethod_0(int_3, " bars corrected"));
					}
				}
				else
				{
					text = Delegate186.smethod_0("{0,-4} {1,-9} {2}", Delegate185.smethod_0("[", string_1, "]"), Delegate174.smethod_1(bars_0), "Error: No data");
				}
				Delegate184.smethod_0(this.idataUpdateMessage_0, text);
			}
		}
		public override DataSource CreateDataSource()
		{
			DataSource result = Delegate25.smethod_0(this);
			MsnStaticSettings msnStaticSettings = new MsnStaticSettings();
			if (this.control0_0.Boolean_0)
			{
				msnStaticSettings.Symbols = Delegate63.smethod_0(this.control1_0.Class13_0);
				msnStaticSettings.Groups = this.control1_0.String_1;
				msnStaticSettings.UpdateGroups = this.control0_0.Boolean_1;
			}
			else
			{
				msnStaticSettings.Symbols = Delegate63.smethod_0(this.control2_0.Class13_0);
			}
			msnStaticSettings.StartDate = this.control0_0.DateTime_0;
			Delegate189.smethod_0(result, msnStaticSettings.SerializeToString());
			Delegate190.smethod_0(result, BarScale.Daily);
			Delegate191.smethod_0(result, 0);
			return result;
		}
		public override void CancelUpdate()
		{
			this.class1_0.method_0();
		}
		public override string ModifySymbols(DataSource ds, List<string> symbols)
		{
			Class13 @class = new Class13(symbols);
			MsnStaticSettings msnStaticSettings = (MsnStaticSettings)DataSetSettings.DeserializeFromString(Delegate192.smethod_0(ds));
			msnStaticSettings.Symbols = Delegate193.smethod_0(Delegate63.smethod_0(@class));
			return msnStaticSettings.SerializeToString();
		}
		public override void DeleteSymbolDataFile(DataSource ds, string symbol)
		{
			Delegate170.smethod_0(this.barDataStore_0, Class11.smethod_1(symbol), Delegate194.smethod_0(ds), Delegate195.smethod_0(ds));
		}
		public override void SaveEditedSymbolDataFile(DataSource ds, Bars bars)
		{
			if (MsnStaticProvider.msnClientSettings_0.DividendAdj)
			{
				Delegate131.smethod_0("Adjustment options in the provider's settings are enabled, changes will not be saved.\r\n\r\nTo save your changes to the file, perform these steps:\r\n1. Close all active charts.\r\n2. Disable the Split and Dividend adjustment options in the settings.\r\n3. Open the symbol's chart and perform modification.\r\n4. If required, turn the adjustment options back on.", "The provider's configured with adjustment options", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			Delegate171.smethod_1(this.barDataStore_0, bars);
		}
		private void method_7(DataSource dataSource_0)
		{
			Log.TraceMethod(new object[0]);
			MsnStaticSettings msnStaticSettings = (MsnStaticSettings)DataSetSettings.DeserializeFromString(Delegate192.smethod_0(dataSource_0));
			if (this.class12_0 == null || !MsnStaticProvider.class8_0.Boolean_0 || MsnStaticProvider.class8_0.method_3(1))
			{
				if (!MsnStaticProvider.class8_0.Boolean_0 || MsnStaticProvider.class8_0.method_3(1))
				{
					this.method_4("Updating the Classification Groups file...");
					MsnStaticProvider.class8_0.method_2(false);
					if (MsnStaticProvider.class8_0.Exception_0 != null)
					{
						this.method_4(Delegate114.smethod_0("Error: ", Delegate127.smethod_0(MsnStaticProvider.class8_0.Exception_0)));
						return;
					}
					this.method_4("Classification file updated.");
				}
				this.class12_0 = new Class12(MsnStaticProvider.class8_0.ClassificationGroup_0);
			}
			this.method_4("Checking the DataSet's Classification groups composition:");
			Class13 @class = new Class13(msnStaticSettings.Groups, Enum4.DSString);
			Class13 class13_ = new Class13(msnStaticSettings.Symbols, Enum4.DSString);
			Class13 class2 = this.class12_0.method_1(class13_, @class.list_0.ToArray());
			this.method_4(Delegate140.smethod_0("Symbols deleted {0}: {1}", this.class12_0.Class13_0.list_0.Count, Delegate63.smethod_0(this.class12_0.Class13_0)));
			this.method_4(Delegate140.smethod_0("Symbols added {0}: {1}", this.class12_0.Class13_1.list_0.Count, Delegate63.smethod_0(this.class12_0.Class13_1)));
			if (this.class12_0.Class13_0.list_0.Count > 0 || this.class12_0.Class13_1.list_0.Count > 0)
			{
				string text = Delegate114.smethod_0(Delegate198.smethod_0(Delegate197.smethod_0(Delegate196.smethod_0(MsnStaticProvider.string_0))), "\\DataSets\\");
				msnStaticSettings.Symbols = Delegate63.smethod_0(class2);
				Delegate189.smethod_0(dataSource_0, msnStaticSettings.SerializeToString());
				Delegate189.smethod_1(dataSource_0, Delegate185.smethod_0(text, Delegate199.smethod_0(Delegate192.smethod_1(dataSource_0)), ".xml"));
			}
		}
		public override void UpdateProvider(IDataUpdateMessage dataUpdateMsg, List<DataSource> dataSources, bool updateNonDSSymbols, bool deleteNonDSSymbols)
		{
			Log.TraceMethod(new object[0]);
			this.class1_0.Boolean_0 = false;
			this.idataUpdateMessage_0 = dataUpdateMsg;
			List<Class2> list = new List<Class2>();
			SymbolInfoList symbolInfoList = null;
			try
			{
				symbolInfoList = SymbolInfoList.Deserialize(Delegate114.smethod_0(MsnStaticProvider.string_0, "SymbolsStartDate.xml"));
				symbolInfoList.Synchronize(this.barDataStore_0);
				this.method_4("Preparing requests ...");
				foreach (DataSource current in dataSources)
				{
					MsnStaticSettings msnStaticSettings = (MsnStaticSettings)DataSetSettings.DeserializeFromString(Delegate192.smethod_0(current));
					Class13 @class = new Class13(msnStaticSettings.Symbols, Enum4.DSString);
					foreach (string current2 in @class.list_0)
					{
						Class2 class2 = new Class2(current2);
						this.method_8(ref class2, symbolInfoList, msnStaticSettings.StartDate);
						class2.DateTime_2 = Delegate200.smethod_0().AddDays(2.0);
						class2.Enum1_0 = (Enum1.History | Enum1.Fundamental);
						for (int i = list.Count - 1; i >= 0; i--)
						{
							if (Delegate51.smethod_0(list[i].String_0, class2.String_0))
							{
								list.RemoveAt(i);
							}
						}
						list.Add(class2);
						if (this.class1_0.Boolean_0)
						{
							return;
						}
					}
				}
				list.Sort();
				this.method_4("Requests are ready to go.");
				if (list.Count > 0)
				{
					this.int_0 = list.Count;
					this.int_1 = 0;
					this.class1_0.method_6(list);
				}
				if (deleteNonDSSymbols)
				{
					IList<string> list2 = Delegate201.smethod_0(this.barDataStore_0, BarScale.Daily, 0);
					string string_ = string.Empty;
					int num = 0;
					foreach (string current3 in list2)
					{
						bool flag = false;
						foreach (Class2 current4 in list)
						{
							if (Delegate51.smethod_0(current4.String_0, current3))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							num++;
							Delegate170.smethod_0(this.barDataStore_0, Class11.smethod_1(current3), BarScale.Daily, 0);
							if (Delegate51.smethod_1(string_, string.Empty))
							{
								string_ = Delegate114.smethod_0(string_, ",");
							}
							string_ = Delegate114.smethod_0(string_, current3);
						}
					}
					if (num > 0)
					{
						Delegate184.smethod_0(dataUpdateMsg, Delegate114.smethod_0("Deleted symbols: ", string_));
						Delegate184.smethod_0(dataUpdateMsg, Delegate202.smethod_0("Deleted ", num, " symbol data files"));
					}
				}
			}
			catch (Exception ex)
			{
				Log.TraceEvent(Enum0.Error, Delegate114.smethod_0("UpdateDataSource ", Delegate127.smethod_0(ex)));
				Delegate184.smethod_0(this.idataUpdateMessage_0, Delegate114.smethod_0("Error: ", Delegate127.smethod_0(ex)));
			}
			finally
			{
				symbolInfoList.Serialize(Delegate114.smethod_0(MsnStaticProvider.string_0, "SymbolsStartDate.xml"));
				this.idataUpdateMessage_0 = null;
			}
		}
		private void method_8(ref Class2 class2_0, SymbolInfoList symbolInfoList_0, DateTime dateTime_0)
		{
			DateTime dateTime = dateTime_0;
			DateTime dateTime2 = symbolInfoList_0.Search(class2_0.String_0, BarScale.Daily, 0);
			if (Delegate178.smethod_1(dateTime, dateTime2))
			{
				class2_0.DateTime_1 = dateTime;
				if (Delegate178.smethod_2(dateTime2, DateTime.MaxValue))
				{
					this.list_0.Add(class2_0.String_0);
				}
				symbolInfoList_0.Add(class2_0.String_0, BarScale.Daily, 0, dateTime);
			}
			else
			{
				class2_0.DateTime_1 = dateTime2;
				DateTime dateTime_ = Delegate203.smethod_0(this.barDataStore_0, Class11.smethod_1(class2_0.String_0), BarScale.Daily, 0);
				if (Delegate178.smethod_2(dateTime_, DateTime.MinValue))
				{
					dateTime = dateTime_.AddDays(-5.0);
				}
			}
			class2_0.DateTime_0 = dateTime;
		}
		private static void smethod_0(DataSource dataSource_0)
		{
			if (Delegate194.smethod_0(dataSource_0) == BarScale.Weekly || Delegate194.smethod_0(dataSource_0) == BarScale.Monthly || Delegate194.smethod_0(dataSource_0) == BarScale.Quarterly || Delegate194.smethod_0(dataSource_0) == BarScale.Yearly)
			{
				Delegate190.smethod_0(dataSource_0, BarScale.Daily);
			}
		}
		public override void UpdateDataSource(DataSource ds, IDataUpdateMessage dataUpdateMsg)
		{
			Log.TraceMethod(new object[0]);
			this.class1_0.Boolean_0 = false;
			SymbolInfoList symbolInfoList = null;
			this.idataUpdateMessage_0 = dataUpdateMsg;
			try
			{
				MsnStaticSettings msnStaticSettings = (MsnStaticSettings)DataSetSettings.DeserializeFromString(Delegate192.smethod_0(ds));
				if (msnStaticSettings.UpdateGroups)
				{
					this.method_7(ds);
					msnStaticSettings = (MsnStaticSettings)DataSetSettings.DeserializeFromString(Delegate192.smethod_0(ds));
				}
				Class13 @class = new Class13(msnStaticSettings.Symbols, Enum4.DSString);
				symbolInfoList = SymbolInfoList.Deserialize(Delegate114.smethod_0(MsnStaticProvider.string_0, "SymbolsStartDate.xml"));
				symbolInfoList.Synchronize(this.barDataStore_0);
				List<Class2> list = new List<Class2>();
				this.method_4("Preparing requests ...");
				foreach (string current in @class.list_0)
				{
					Class2 class2 = new Class2(current);
					this.method_8(ref class2, symbolInfoList, msnStaticSettings.StartDate);
					class2.DateTime_2 = Delegate200.smethod_0().AddDays(2.0);
					class2.Enum1_0 = (Enum1.History | Enum1.Fundamental);
					list.Add(class2);
					if (this.class1_0.Boolean_0)
					{
						return;
					}
				}
				this.method_4("Requests are ready to go.");
				if (list.Count > 0)
				{
					this.int_0 = list.Count;
					this.int_1 = 0;
					this.class1_0.method_6(list);
				}
			}
			catch (Exception ex)
			{
				Log.TraceEvent(Enum0.Error, Delegate114.smethod_0("UpdateDataSource ", Delegate127.smethod_0(ex)));
				Delegate184.smethod_0(this.idataUpdateMessage_0, Delegate114.smethod_0("Error: ", Delegate127.smethod_0(ex)));
			}
			finally
			{
				symbolInfoList.Serialize(Delegate114.smethod_0(MsnStaticProvider.string_0, "SymbolsStartDate.xml"));
				this.idataUpdateMessage_0 = null;
			}
		}
		public override void PopulateSymbols(DataSource ds, List<string> symbols)
		{
			MsnStaticSettings msnStaticSettings = (MsnStaticSettings)DataSetSettings.DeserializeFromString(Delegate192.smethod_0(ds));
			Class13 @class = new Class13(msnStaticSettings.Symbols, Enum4.DSString);
			symbols.AddRange(@class.list_0);
		}
		public override Bars RequestData(DataSource ds, string symbol, DateTime startDate, DateTime symbolEndDate, int maxBars, bool includePartialBar)
		{
			Log.TraceMethodInfo(new object[]
			{
				Delegate192.smethod_0(ds),
				symbol,
				startDate,
				symbolEndDate,
				maxBars,
				includePartialBar
			});
			symbol = Delegate115.smethod_0(symbol, new char[]
			{
				' ',
				'"'
			});
			if (this.bool_0)
			{
				try
				{
					MsnStaticSettings msnStaticSettings = new MsnStaticSettings();
					if (Delegate51.smethod_1(Delegate192.smethod_0(ds), string.Empty))
					{
						msnStaticSettings = (MsnStaticSettings)DataSetSettings.DeserializeFromString(Delegate192.smethod_0(ds));
					}
					SymbolInfoList symbolInfoList = SymbolInfoList.Deserialize(Delegate114.smethod_0(MsnStaticProvider.string_0, "SymbolsStartDate.xml"));
					symbolInfoList.Synchronize(this.barDataStore_0);
					Class2 @class = new Class2(symbol);
					this.method_8(ref @class, symbolInfoList, msnStaticSettings.StartDate);
					@class.DateTime_2 = Delegate200.smethod_0().AddDays(2.0);
					@class.Enum1_0 = (Enum1.History | Enum1.Fundamental);
					List<Class2> list = new List<Class2>();
					list.Add(@class);
					this.class1_0.method_6(list);
				}
				catch (Exception ex)
				{
					string value = Delegate114.smethod_0("On Demand Update Error: ", Delegate127.smethod_0(ex));
					Log.TraceEvent(Enum0.Error, value);
					Delegate131.smethod_0(value, "On Demand Update Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			Bars bars = Delegate24.smethod_0(Class11.smethod_1(symbol), Delegate194.smethod_0(ds), Delegate195.smethod_0(ds));
			if (Delegate205.smethod_0(this.barDataStore_0, Class11.smethod_1(symbol), Delegate194.smethod_0(ds), Delegate195.smethod_0(ds)))
			{
				Delegate206.smethod_0(this.barDataStore_0, bars, startDate, symbolEndDate, maxBars);
			}
			if (Delegate51.smethod_1(symbol, Class11.smethod_1(symbol)))
			{
				Bars bars2 = Delegate24.smethod_0(symbol, Delegate194.smethod_0(ds), Delegate195.smethod_0(ds));
				Delegate207.smethod_0(bars2, bars);
				Delegate175.smethod_0(bars2, Delegate174.smethod_0(bars));
				bars = bars2;
			}
			if (MsnStaticProvider.msnClientSettings_0.DividendAdj)
			{
				if (this.msnFundamentalProvider_0 == null)
				{
					this.msnFundamentalProvider_0 = new MsnFundamentalProvider();
					Delegate52.smethod_1(this.msnFundamentalProvider_0, Delegate208.smethod_0(this));
				}
				IList<FundamentalItem> dividends = Delegate176.smethod_0(this.msnFundamentalProvider_0, symbol, MsnFundamentalProvider.DividendName);
				Class10 class2 = new Class10(null, dividends, Enum3.Dividend);
				bars = class2.method_2(bars);
			}
			return bars;
		}
		public override bool SupportsDynamicUpdate(BarScale scale)
		{
			return false;
		}
		public override UserControl WizardFirstPage()
		{
			if (this.control0_0 == null)
			{
				this.control0_0 = new Control0();
				this.control2_0 = new Control2();
				this.control1_0 = new Control1();
			}
			this.control0_0.method_0();
			this.control2_0.method_0();
			this.control1_0.method_7(MsnStaticProvider.string_0);
			return this.control0_0;
		}
		public override UserControl WizardNextPage(UserControl currentPage)
		{
			if (currentPage == this.control0_0)
			{
				if (this.control0_0.Boolean_0)
				{
					return this.control1_0;
				}
				return this.control2_0;
			}
			else
			{
				if (currentPage == this.control2_0 && this.control2_0.Class13_0.list_0.Count == 0)
				{
					throw Delegate26.smethod_0("Symbols are not specified");
				}
				if (currentPage == this.control1_0 && this.control1_0.Class13_0.list_0.Count == 0)
				{
					throw Delegate26.smethod_0("Group is not selected");
				}
				return null;
			}
		}
		public override UserControl WizardPreviousPage(UserControl currentPage)
		{
			if (currentPage == this.control0_0)
			{
				return null;
			}
			if (currentPage != this.control2_0)
			{
				if (currentPage != this.control1_0)
				{
					return null;
				}
			}
			return this.control0_0;
		}
		public override void Initialize(IDataHost dataHost)
		{
			Log.TraceMethod(new object[0]);
			Delegate153.smethod_1(this, dataHost);
			this.barDataStore_0 = Delegate22.smethod_0(dataHost, this);
			MsnStaticProvider.string_0 = Delegate154.smethod_0(this.barDataStore_0);
			this.bool_0 = Delegate209.smethod_0(dataHost);
			this.class1_0 = new Class1(MsnStaticProvider.string_0);
			this.class1_0.Event_0 += new Class1.Delegate1(this.method_1);
			this.class1_0.Event_1 += new Class1.Delegate2(this.method_0);
			MsnStaticProvider.class8_0 = new Class8(Delegate114.smethod_0(MsnStaticProvider.string_0, "MsnClassification.xml"), "http://67.199.28.171/Classification/Msn/MsnClassification.xml");
			MsnStaticProvider.msnClientSettings_0 = MsnClientSettings.Deserialize(MsnStaticProvider.string_0);
			this.msnFundamentalProvider_0 = new MsnFundamentalProvider();
			Delegate52.smethod_1(this.msnFundamentalProvider_0, Delegate208.smethod_0(this));
			Delegate155.smethod_1(10000);
			Delegate210.smethod_0(true);
			Delegate210.smethod_1(true);
			Delegate155.smethod_2(100);
			Delegate211.smethod_0(Delegate27.smethod_0("http://data.moneycentral.msn.com"));
		}
	}
}
