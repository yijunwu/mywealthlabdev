using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
namespace WealthLab.DataProviders.Msn
{
	public class MSNDataClient
	{
		public string MSNStreamingRequest(string url, string symbol)
		{
			string result = null;
			HttpWebRequest object_ = (HttpWebRequest)Delegate226.smethod_0(url);
			Delegate228.smethod_0(object_, 5000);
			Delegate227.smethod_0(object_, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:15.0) Gecko/20120403211507 Firefox/15.0.1");
			Delegate227.smethod_1(object_, Delegate126.smethod_0("http://investing.money.msn.com/investments/stock-price?symbol={0}&ocid=qbes", symbol));
			HttpWebResponse object_2 = null;
			try
			{
				object_2 = (HttpWebResponse)Delegate229.smethod_0(object_);
			}
			catch (Exception object_3)
			{
				Delegate166.smethod_0(Delegate127.smethod_0(object_3));
			}
			try
			{
				using (Stream stream = Delegate230.smethod_0(object_2))
				{
					using (StreamReader streamReader = Delegate33.smethod_0(stream))
					{
						result = Delegate231.smethod_0(streamReader);
					}
				}
			}
			catch (Exception object_4)
			{
				Delegate166.smethod_0(Delegate127.smethod_0(object_4));
			}
			return result;
		}
		public string GetStreamingUrl(string symbol)
		{
			return Delegate126.smethod_0("http://services.money.msn.com/quoteservice/streaming?symbol={0}&format=json", symbol);
		}
		public Quote DownloadStreamingQuote(string symbol)
		{
			string streamingUrl = this.GetStreamingUrl(symbol);
			string string_ = this.MSNStreamingRequest(streamingUrl, symbol);
			return this.method_0(string_);
		}
		private Quote method_0(string string_0)
		{
			Quote quote = Delegate38.smethod_0();
			string object_ = string.Empty;
			if (Delegate161.smethod_0(string_0))
			{
				return quote;
			}
			try
			{
				string_0 = Delegate115.smethod_0(string_0, new char[]
				{
					'[',
					']'
				});
				MemoryStream stream_ = Delegate39.smethod_0(Delegate252.smethod_0(Delegate251.smethod_0(), string_0));
				DataContractJsonSerializer object_2 = Delegate40.smethod_0(Delegate253.smethod_0(typeof(MSNQuoteResponse).TypeHandle));
				MSNQuoteResponse mSNQuoteResponse = (MSNQuoteResponse)Delegate254.smethod_0(object_2, stream_);
				if (quote != null)
				{
					Delegate255.smethod_0(quote, mSNQuoteResponse.RTLast);
					string symbol;
					Delegate256.smethod_0(quote, symbol = mSNQuoteResponse.Symbol);
					object_ = symbol;
					Delegate257.smethod_0(quote, mSNQuoteResponse.RTTimeOfLastSale);
					Delegate255.smethod_1(quote, mSNQuoteResponse.RTLast - mSNQuoteResponse.RTChange);
				}
			}
			catch (Exception object_3)
			{
				string string_ = Delegate126.smethod_0("Data parsing error in ParseStreamingQuote. Symbol: {0}", object_);
				Delegate166.smethod_0(Delegate127.smethod_0(object_3));
				Delegate166.smethod_0(string_);
			}
			return quote;
		}
	}
}
