using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
namespace WealthLab.DataProviders.Msn
{
	[DataContract]
	public class MSNQuoteResponse
	{
		[CompilerGenerated]
		private string string_0;
		[CompilerGenerated]
		private string string_1;
		[CompilerGenerated]
		private string string_2;
		[CompilerGenerated]
		private string string_3;
		[CompilerGenerated]
		private int int_0;
		[CompilerGenerated]
		private int int_1;
		[CompilerGenerated]
		private int int_2;
		[CompilerGenerated]
		private object object_0;
		[CompilerGenerated]
		private double double_0;
		[CompilerGenerated]
		private string string_4;
		[CompilerGenerated]
		private double double_1;
		[CompilerGenerated]
		private double double_2;
		[CompilerGenerated]
		private int int_3;
		[CompilerGenerated]
		private double double_3;
		[CompilerGenerated]
		private double double_4;
		[CompilerGenerated]
		private double double_5;
		[CompilerGenerated]
		private DateTime dateTime_0;
		[CompilerGenerated]
		private int int_4;
		[CompilerGenerated]
		private DateTime dateTime_1;
		[CompilerGenerated]
		private int int_5;
		[CompilerGenerated]
		private object object_1;
		[DataMember]
		public string Symbol
		{
			get;
			set;
		}
		public string CompanyName
		{
			get;
			set;
		}
		public string Country
		{
			get;
			set;
		}
		public string Type
		{
			get;
			set;
		}
		public int AHChange
		{
			get;
			set;
		}
		public int AHLastPrice
		{
			get;
			set;
		}
		public int AHVolume
		{
			get;
			set;
		}
		public object AHTimeOfLastSale
		{
			get;
			set;
		}
		public double Change
		{
			get;
			set;
		}
		public string Currency
		{
			get;
			set;
		}
		public double Last
		{
			get;
			set;
		}
		public double PercentChange
		{
			get;
			set;
		}
		public int QuoteDelay
		{
			get;
			set;
		}
		[DataMember]
		public double RTChange
		{
			get;
			set;
		}
		[DataMember]
		public double RTPercentChange
		{
			get;
			set;
		}
		[DataMember]
		public double RTLast
		{
			get;
			set;
		}
		[DataMember]
		public DateTime RTTimeOfLastSale
		{
			get;
			set;
		}
		public int StreamingInterval
		{
			get;
			set;
		}
		public DateTime TimeOfLastSale
		{
			get;
			set;
		}
		[DataMember]
		public int Volume
		{
			get;
			set;
		}
		public object IsNewsAvailable
		{
			get;
			set;
		}
	}
}
