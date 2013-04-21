using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using WealthLab.DataProviders.Msn.Properties;
namespace WealthLab.DataProviders.Msn
{
	public class MSNStreamingProvider : StreamingDataProvider
	{
		private delegate void Delegate0(DateTime timeStamp);
		private IConnectionStatus iconnectionStatus_0;
		private Thread thread_0;
		private static bool bool_0 = false;
		private static List<string> list_0 = new List<string>();
		private MsnStaticProvider msnStaticProvider_0 = new MsnStaticProvider();
		private MSNStreamingProvider.Delegate0 delegate0_0;
		private readonly int int_0 = 5000;
		private readonly string string_0 = "MSN";
		private BarDataStore barDataStore_0;
		private static string string_1;
		public static string DataPath
		{
			get
			{
				return MSNStreamingProvider.string_1;
			}
		}
		public override string Description
		{
			get
			{
				return "Provides free streaming data by MSN MoneyCentral.";
			}
		}
		public override string FriendlyName
		{
			get
			{
				return "MSN MoneyCentral Streaming Provider";
			}
		}
		public override Bitmap Glyph
		{
			get
			{
				return Resources.MSN;
			}
		}
		public override bool ProvidesOpen
		{
			get
			{
				return false;
			}
		}
		public override bool IsConnected
		{
			get
			{
				return MSNStreamingProvider.bool_0;
			}
		}
		public override StaticDataProvider GetStaticProvider()
		{
			return this.msnStaticProvider_0;
		}
		public override MarketInfo GetMarketInfo(string symbol)
		{
			return Delegate150.smethod_0(symbol, Delegate149.smethod_0(Delegate148.smethod_0()), this.string_0);
		}
		public override void Initialize(IDataHost dataHost)
		{
			Delegate151.smethod_0(this, dataHost);
			if (this.msnStaticProvider_0 == null)
			{
				this.msnStaticProvider_0 = new MsnStaticProvider();
				Delegate153.smethod_0(this.msnStaticProvider_0, Delegate152.smethod_0(this));
			}
			this.barDataStore_0 = Delegate22.smethod_0(dataHost, this.msnStaticProvider_0);
			MSNStreamingProvider.string_1 = Delegate154.smethod_0(this.barDataStore_0);
			this.delegate0_0 = new MSNStreamingProvider.Delegate0(this.method_0);
		}
		private void method_0(DateTime dateTime_0)
		{
			Delegate155.smethod_0(this.int_0);
			Delegate156.smethod_0(this, dateTime_0.AddSeconds((double)(this.int_0 / 1000)));
		}
		public override void ConnectStreaming(IConnectionStatus connStatus)
		{
			this.iconnectionStatus_0 = connStatus;
			if (!MSNStreamingProvider.bool_0)
			{
				Delegate157.smethod_0(this, this.iconnectionStatus_0);
				MSNStreamingProvider.bool_0 = true;
				if (connStatus != null)
				{
					Delegate158.smethod_0(connStatus);
				}
				Delegate159.smethod_0(this.iconnectionStatus_0, ConnStatus.OK, 0, "Streaming OK");
			}
		}
		public override void DisconnectStreaming()
		{
			if (MSNStreamingProvider.bool_0)
			{
				MSNStreamingProvider.bool_0 = false;
				if (this.iconnectionStatus_0 != null)
				{
					Delegate158.smethod_1(this.iconnectionStatus_0);
				}
				this.iconnectionStatus_0 = null;
				Delegate160.smethod_0(this);
			}
		}
		protected override void Subscribe(string symbol)
		{
			if (Delegate161.smethod_0(symbol))
			{
				return;
			}
			if (MSNStreamingProvider.bool_0 && Delegate51.smethod_1(symbol, string.Empty) && !MSNStreamingProvider.list_0.Contains(symbol))
			{
				MSNStreamingProvider.list_0.Add(symbol);
				this.thread_0 = Delegate23.smethod_0(new ParameterizedThreadStart(this.method_1));
				Delegate162.smethod_0(this.thread_0, true);
				Delegate163.smethod_0(this.thread_0, symbol);
				this.method_2();
			}
		}
		protected override void UnSubscribe(string symbol)
		{
			if (MSNStreamingProvider.bool_0 && Delegate51.smethod_1(symbol, string.Empty) && MSNStreamingProvider.list_0.Contains(symbol))
			{
				MSNStreamingProvider.list_0.Remove(symbol);
				this.method_2();
			}
		}
		private void method_1(object object_0)
		{
			string text = (string)object_0;
			MSNDataClient mSNDataClient = new MSNDataClient();
			while (MSNStreamingProvider.list_0.Contains(text))
			{
				if (!Delegate161.smethod_0(text))
				{
					Quote quote = mSNDataClient.DownloadStreamingQuote(text);
					if (quote != null)
					{
						try
						{
							if (!Delegate164.smethod_0(quote, this.string_0))
							{
								Delegate165.smethod_0(this, quote);
							}
						}
						catch (Exception object_)
						{
							Delegate166.smethod_0(Delegate114.smethod_0("Updater error: ", Delegate127.smethod_0(object_)));
						}
					}
				}
				Delegate155.smethod_0(this.int_0);
			}
			mSNDataClient = null;
		}
		private void method_2()
		{
			Delegate159.smethod_0(this.iconnectionStatus_0, ConnStatus.OK, 0, Delegate167.smethod_0(MSNStreamingProvider.list_0.Count, " Symbols Subscribed"));
		}
	}
}
