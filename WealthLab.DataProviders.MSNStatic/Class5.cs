using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using WealthLab;
internal class Class5
{
	private enum Enum2
	{
		BalanceSheet,
		IncomeStatement,
		CashFlow,
		TenYrSummary
	}
	private Dictionary<string, List<FundamentalItem>> dictionary_0 = new Dictionary<string, List<FundamentalItem>>();
	private Dictionary<string, List<FundamentalItem>> dictionary_1 = new Dictionary<string, List<FundamentalItem>>();
	private Dictionary<string, List<FundamentalItem>> dictionary_2 = new Dictionary<string, List<FundamentalItem>>();
	private Dictionary<string, List<FundamentalItem>> dictionary_3 = new Dictionary<string, List<FundamentalItem>>();
	private ManualResetEvent[] manualResetEvent_0;
	private IFormatProvider iformatProvider_0 = Delegate32.smethod_0("en-US");
	private NumberFormatInfo numberFormatInfo_0;
	public int int_0;
	public ManualResetEvent[] ManualResetEvent_0
	{
		get
		{
			return this.manualResetEvent_0;
		}
		set
		{
			this.manualResetEvent_0 = value;
		}
	}
	public Class5()
	{
		this.numberFormatInfo_0 = Delegate49.smethod_0();
		Delegate312.smethod_0(this.numberFormatInfo_0, ".");
		Delegate312.smethod_1(this.numberFormatInfo_0, ",");
		Delegate155.smethod_1(10000);
		Delegate210.smethod_0(true);
		Delegate210.smethod_1(true);
		Delegate155.smethod_2(100);
		Delegate211.smethod_0(Delegate27.smethod_0("http://investing.money.msn.com/"));
	}
	public string method_0(string string_0, int int_1)
	{
		string string_ = "http://investing.money.msn.com/investments/{1}?symbol={0}&";
		string object_ = (int_1 == 1) ? "stock-balance-sheet" : ((int_1 == 2) ? "stock-income-statement" : ((int_1 == 3) ? "stock-cash-flow" : ((int_1 == 4) ? "financial-statements" : string.Empty)));
		return Delegate140.smethod_0(string_, string_0, object_);
	}
	private string method_1(string string_0, int int_1)
	{
		string result = null;
		string string_ = this.method_0(string_0, int_1);
		HttpWebRequest object_ = (HttpWebRequest)Delegate226.smethod_0(string_);
		Delegate228.smethod_0(object_, 10000);
		Delegate227.smethod_0(object_, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:15.0) Gecko/20120403211507 Firefox/15.0.1");
		Delegate227.smethod_1(object_, Delegate199.smethod_2(Delegate114.smethod_0("http://investing.money.msn.com/investments/stock-price?symbol=", string_0)));
		try
		{
			HttpWebResponse object_2 = (HttpWebResponse)Delegate229.smethod_0(object_);
			using (Stream stream = Delegate230.smethod_0(object_2))
			{
				using (StreamReader streamReader = Delegate33.smethod_0(stream))
				{
					result = Delegate231.smethod_0(streamReader);
				}
			}
		}
		catch
		{
			result = string.Empty;
		}
		return result;
	}
	internal string method_2(string string_0)
	{
		string empty = string.Empty;
		char[] expr_0D = new char[3];
		Delegate306.smethod_0(expr_0D, fieldof(<PrivateImplementationDetails>{A425EAFD-A689-4AF5-9D5E-3AB6BB6F6CF6}.$$method0x6000104-1).FieldHandle);
		return Delegate241.smethod_0(Delegate115.smethod_0(string_0, expr_0D), "<br>", " ");
	}
	private void method_3(List<FundamentalItem> list_0, string string_0, string string_1, string string_2, int int_1)
	{
		if (Delegate161.smethod_0(string_2))
		{
			return;
		}
		HtmlDocument object_ = Delegate50.smethod_0();
		this.int_0 = 0;
		Delegate313.smethod_0(object_, string_2);
		if (Delegate314.smethod_0(object_) != null)
		{
			if (int_1 == 3)
			{
				string string_3 = "//div/table[@class=' mnytbl']/tbody";
				HtmlNodeCollection htmlNodeCollection = Delegate315.smethod_0(Delegate314.smethod_0(object_), string_3);
				string string_4 = "//div/table[@class=' mnytbl']/thead";
				HtmlNodeCollection object_2 = Delegate315.smethod_0(Delegate314.smethod_0(object_), string_4);
				int num = 0;
				if (htmlNodeCollection == null)
				{
					return;
				}
				using (IEnumerator<HtmlNode> enumerator = ((IEnumerable<HtmlNode>)htmlNodeCollection).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						HtmlNode current = enumerator.Current;
						if (current != null)
						{
							HtmlNodeCollection object_3 = Delegate315.smethod_0(current, "tr/td[1]");
							int num2 = Delegate316.smethod_0(Delegate315.smethod_0(current, "tr[1]/td"));
							HtmlNode object_4 = Delegate317.smethod_0(object_2, num);
							for (int i = 2; i <= num2; i++)
							{
								string string_5 = Delegate126.smethod_0("tr/td[{0}]", i);
								HtmlNodeCollection htmlNodeCollection2 = Delegate315.smethod_0(current, string_5);
								int num3 = 0;
								foreach (HtmlNode current2 in (IEnumerable<HtmlNode>)htmlNodeCollection2)
								{
									try
									{
										string string_6 = this.method_2(Delegate319.smethod_0(Delegate317.smethod_0(Delegate318.smethod_0(Delegate317.smethod_0(object_3, num3)), 1)));
										DateTime dateTime_ = Delegate239.smethod_0(string_6, "MM/yy", Delegate320.smethod_0());
										int num4 = Delegate321.smethod_0(dateTime_.Year, dateTime_.Month);
										dateTime_ = dateTime_.AddDays((double)(num4 - 1));
										string string_7 = this.method_2(Delegate319.smethod_0(Delegate317.smethod_0(Delegate318.smethod_0(current2), 1)));
										HtmlNodeCollection object_5 = Delegate315.smethod_0(Delegate317.smethod_0(Delegate318.smethod_0(object_4), 1), "th");
										string string_8 = Delegate114.smethod_0("[msn] ", Delegate193.smethod_2(this.method_2(Delegate319.smethod_0(Delegate317.smethod_0(Delegate318.smethod_0(Delegate317.smethod_0(object_5, i - 1)), 1)))));
										if (Delegate51.smethod_0(Delegate193.smethod_2(string_1), string_8))
										{
											this.method_5(list_0, string_1, dateTime_, this.method_4(string_7));
										}
										num3++;
									}
									catch (Exception)
									{
										string string_9 = Delegate140.smethod_0("Data parsing error. Symbol: {0}, Item name: {1}", string_0, string_1);
										Delegate166.smethod_0(string_9);
									}
								}
								this.int_0++;
							}
						}
						num++;
					}
					return;
				}
			}
			string string_10 = "//tbody/tr[@class='mnfh'][1]";
			string string_11 = "//tbody/tr[@class='mnboldtopline']";
			string string_12 = "MM/dd/yyyy";
			HtmlNodeCollection htmlNodeCollection3 = Delegate315.smethod_0(Delegate314.smethod_0(object_), string_10);
			if (htmlNodeCollection3 != null)
			{
				HtmlNodeCollection htmlNodeCollection4 = Delegate315.smethod_0(Delegate314.smethod_0(object_), string_11);
				foreach (HtmlNode current3 in (IEnumerable<HtmlNode>)htmlNodeCollection4)
				{
					if (current3 != null)
					{
						string string_13 = Delegate114.smethod_0("[msn] ", Delegate193.smethod_2(this.method_2(Delegate319.smethod_1(Delegate317.smethod_0(Delegate318.smethod_0(current3), 1)))));
						if (Delegate51.smethod_0(string_1, string_13))
						{
							for (int j = 3; j < Delegate316.smethod_0(Delegate318.smethod_0(current3)); j += 2)
							{
								try
								{
									Delegate317.smethod_0(Delegate318.smethod_0(current3), j);
									string string_14 = this.method_2(Delegate319.smethod_1(Delegate317.smethod_0(Delegate318.smethod_0(Delegate317.smethod_0(htmlNodeCollection3, 0)), j)));
									string string_15 = this.method_2(Delegate319.smethod_1(Delegate317.smethod_0(Delegate318.smethod_0(current3), j)));
									DateTime dateTime_2;
									if (Delegate322.smethod_0(string_14, string_12, this.iformatProvider_0, DateTimeStyles.None, ref dateTime_2))
									{
										this.method_5(list_0, string_1, dateTime_2, this.method_4(string_15));
									}
									this.int_0++;
								}
								catch (Exception)
								{
									string string_16 = Delegate140.smethod_0("Data parsing error. Symbol: {0}, Item name: {1}", string_0, string_1);
									Delegate166.smethod_0(string_16);
								}
							}
						}
					}
				}
			}
		}
	}
	private double method_4(string string_0)
	{
		int num = 1;
		if (Delegate238.smethod_2(string_0, "Mil"))
		{
			string_0 = Delegate241.smethod_0(string_0, "&nbsp;", "");
			string_0 = Delegate241.smethod_0(string_0, "Mil", "");
		}
		if (Delegate238.smethod_2(string_0, "Bil"))
		{
			num = 1000;
			string_0 = Delegate241.smethod_0(string_0, "&nbsp;", "");
			string_0 = Delegate241.smethod_0(string_0, "Bil", "");
		}
		string_0 = Delegate193.smethod_1(string_0);
		return Delegate242.smethod_0(string_0, this.numberFormatInfo_0) * (double)num;
	}
	private void method_5(List<FundamentalItem> list_0, string string_0, DateTime dateTime_0, double double_0)
	{
		FundamentalItem fundamentalItem = Delegate4.smethod_0(string_0);
		Delegate240.smethod_0(fundamentalItem, dateTime_0);
		Delegate243.smethod_0(fundamentalItem, double_0);
		list_0.Add(fundamentalItem);
	}
	private Dictionary<string, List<FundamentalItem>> method_6(Class5.Enum2 enum2_0)
	{
		Dictionary<string, List<FundamentalItem>> dictionary = new Dictionary<string, List<FundamentalItem>>();
		string[] array;
		switch (enum2_0)
		{
		case Class5.Enum2.BalanceSheet:
			array = Class0.String_0;
			break;
		case Class5.Enum2.IncomeStatement:
			array = Class0.String_1;
			break;
		case Class5.Enum2.CashFlow:
			array = Class0.String_2;
			break;
		case Class5.Enum2.TenYrSummary:
			array = Class0.String_3;
			break;
		default:
			array = Class0.String_0;
			break;
		}
		for (int i = 0; i < array.Length; i++)
		{
			dictionary.Add(array[i], new List<FundamentalItem>());
		}
		return dictionary;
	}
	public Class7 method_7(string string_0, IDataUpdateMessage idataUpdateMessage_0)
	{
		this.dictionary_0 = this.method_6(Class5.Enum2.BalanceSheet);
		this.dictionary_1 = this.method_6(Class5.Enum2.IncomeStatement);
		this.dictionary_2 = this.method_6(Class5.Enum2.CashFlow);
		this.dictionary_3 = this.method_6(Class5.Enum2.TenYrSummary);
		List<Class6> list = new List<Class6>();
		list.Add(new Class6(this.dictionary_0, string_0, "Balance Sheet", 1));
		list.Add(new Class6(this.dictionary_1, string_0, "Income Statement", 2));
		list.Add(new Class6(this.dictionary_2, string_0, "Cash Flow", 3));
		list.Add(new Class6(this.dictionary_3, string_0, "10 Year Summary", 4));
		this.ManualResetEvent_0 = new ManualResetEvent[4];
		foreach (Class6 current in list)
		{
			this.ManualResetEvent_0[current.Int32_0 - 1] = Delegate37.smethod_0(false);
			Delegate323.smethod_0(new WaitCallback(this.method_8), current);
		}
		Delegate247.smethod_0(this.ManualResetEvent_0);
		return new Class7(this.dictionary_0, this.dictionary_1, this.dictionary_2, this.dictionary_3);
	}
	private void method_8(object object_0)
	{
		Class6 @class = (Class6)object_0;
		Dictionary<string, List<FundamentalItem>> dictionary = @class.Dictionary_0;
		int num = @class.Int32_0 - 1;
		string string_ = @class.String_0;
		if (dictionary.Count > 0)
		{
			foreach (KeyValuePair<string, List<FundamentalItem>> current in dictionary)
			{
				string string_2 = this.method_1(string_, num + 1);
				this.method_3(current.Value, string_, current.Key, string_2, num);
			}
			foreach (KeyValuePair<string, List<FundamentalItem>> current2 in dictionary)
			{
				current2.Value.Reverse();
			}
		}
		Delegate245.smethod_1(this.ManualResetEvent_0[num]);
	}
}
