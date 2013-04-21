using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WealthLab;
using WealthLab.DataProviders.Helper;
using WealthLab.DataProviders.Msn;
internal class Class1
{
	public delegate void Delegate1(object sender, EventArgs1 e);
	public delegate void Delegate2(object sender, EventArgs2 e);
	private IFormatProvider iformatProvider_0 = Delegate32.smethod_0("en-US");
	private bool bool_0;
	private MsnClientSettings msnClientSettings_0;
	private Queue<Class2> queue_0 = new Queue<Class2>();
	private List<Class4> list_0 = new List<Class4>();
	private Class1.Delegate1 delegate1_0;
	private Class1.Delegate2 delegate2_0;
	public event Class1.Delegate1 Event_0
	{
		add
		{
			Class1.Delegate1 @delegate = this.delegate1_0;
			Class1.Delegate1 delegate2;
			do
			{
				delegate2 = @delegate;
				Class1.Delegate1 value2 = (Class1.Delegate1)Delegate223.smethod_0(delegate2, value);
				@delegate = Interlocked.CompareExchange<Class1.Delegate1>(ref this.delegate1_0, value2, delegate2);
			}
			while (@delegate != delegate2);
		}
		remove
		{
			Class1.Delegate1 @delegate = this.delegate1_0;
			Class1.Delegate1 delegate2;
			do
			{
				delegate2 = @delegate;
				Class1.Delegate1 value2 = (Class1.Delegate1)Delegate223.smethod_1(delegate2, value);
				@delegate = Interlocked.CompareExchange<Class1.Delegate1>(ref this.delegate1_0, value2, delegate2);
			}
			while (@delegate != delegate2);
		}
	}
	public event Class1.Delegate2 Event_1
	{
		add
		{
			Class1.Delegate2 @delegate = this.delegate2_0;
			Class1.Delegate2 delegate2;
			do
			{
				delegate2 = @delegate;
				Class1.Delegate2 value2 = (Class1.Delegate2)Delegate223.smethod_0(delegate2, value);
				@delegate = Interlocked.CompareExchange<Class1.Delegate2>(ref this.delegate2_0, value2, delegate2);
			}
			while (@delegate != delegate2);
		}
		remove
		{
			Class1.Delegate2 @delegate = this.delegate2_0;
			Class1.Delegate2 delegate2;
			do
			{
				delegate2 = @delegate;
				Class1.Delegate2 value2 = (Class1.Delegate2)Delegate223.smethod_1(delegate2, value);
				@delegate = Interlocked.CompareExchange<Class1.Delegate2>(ref this.delegate2_0, value2, delegate2);
			}
			while (@delegate != delegate2);
		}
	}
	public bool Boolean_0
	{
		get
		{
			return this.bool_0;
		}
		set
		{
			this.bool_0 = value;
		}
	}
	public Class1(string _settingsFolderName)
	{
		this.msnClientSettings_0 = MsnClientSettings.Deserialize(_settingsFolderName);
	}
	public void method_0()
	{
		Log.TraceMethod(new object[0]);
		if (this.bool_0)
		{
			return;
		}
		this.bool_0 = true;
		bool flag = false;
		try
		{
			List<Class4> object_;
			Delegate54.smethod_0(object_ = this.list_0, ref flag);
			foreach (Class4 current in this.list_0)
			{
				Delegate224.smethod_0(current.ManualResetEvent_0);
				Delegate225.smethod_0(current.Thread_0);
			}
		}
		finally
		{
			if (flag)
			{
				List<Class4> object_;
				Delegate58.smethod_0(object_);
			}
		}
	}
	private string method_1(string string_0, bool bool_1)
	{
		string text = null;
		int num = 0;
		while (true)
		{
			num++;
			Log.TraceMethod(new object[]
			{
				string_0,
				Delegate167.smethod_0("Attempt ", num)
			});
			HttpWebRequest object_ = (HttpWebRequest)Delegate226.smethod_0(string_0);
			Delegate227.smethod_0(object_, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322");
			Delegate228.smethod_0(object_, 10000);
			try
			{
				HttpWebResponse object_2 = (HttpWebResponse)Delegate229.smethod_0(object_);
				using (Stream stream = Delegate230.smethod_0(object_2))
				{
					using (StreamReader streamReader = Delegate33.smethod_0(stream))
					{
						text = Delegate231.smethod_0(streamReader);
						Log.Trace(Delegate120.smethod_0("Result  ", string_0, "\r\n", text));
					}
				}
				goto IL_11C;
			}
			catch (Exception ex)
			{
				Log.TraceEvent(Enum0.Error, Delegate127.smethod_0(ex));
				if (!(ex is WebException))
				{
					throw ex;
				}
				if (num < this.msnClientSettings_0.AttemptCount)
				{
					Log.TraceEvent(Enum0.Warning, Delegate114.smethod_0("New attempt ", string_0));
					goto IL_11C;
				}
				if (!bool_1)
				{
					goto IL_11C;
				}
				if (Delegate232.smethod_0(ex as WebException) == WebExceptionStatus.ConnectionClosed)
				{
					throw Delegate34.smethod_0("No Data");
				}
				throw ex;
			}
			IL_109:
			if (num >= this.msnClientSettings_0.AttemptCount)
			{
				break;
			}
			continue;
			IL_11C:
			if (text != null)
			{
				break;
			}
			goto IL_109;
		}
		return text;
	}
	public Bars method_2(Class2 class2_0)
	{
		Log.TraceMethod(new object[]
		{
			class2_0.String_0
		});
		int num = 3;
		string text = string.Empty;
		StringBuilder object_ = Delegate20.smethod_0();
		DateTime dateTime = class2_0.DateTime_0;
		do
		{
			if (Delegate233.smethod_0(class2_0.DateTime_2, dateTime).Days > num * 365)
			{
				DateTime dateTime2 = dateTime.AddYears(num);
				string string_ = this.method_8(class2_0.String_0, dateTime, dateTime2, Enum1.History);
				if (Delegate178.smethod_1(dateTime2.AddMonths(1), class2_0.DateTime_2))
				{
					dateTime = dateTime2.AddMonths(1);
				}
				else
				{
					dateTime = dateTime2;
				}
				text = this.method_1(string_, false);
			}
			else
			{
				string string_ = this.method_8(class2_0.String_0, dateTime, class2_0.DateTime_2, Enum1.History);
				dateTime = class2_0.DateTime_2;
				text = this.method_1(string_, true);
			}
			if (!Delegate161.smethod_0(text))
			{
				string[] array = Delegate234.smethod_0(text, new string[]
				{
					"\n"
				}, StringSplitOptions.RemoveEmptyEntries);
				for (int i = array.Length - 1; i >= 0; i--)
				{
					Delegate111.smethod_1(object_, array[i]);
				}
			}
		}
		while (Delegate178.smethod_2(dateTime, class2_0.DateTime_2));
		return this.method_7(class2_0.String_0, Delegate63.smethod_0(object_));
	}
	private Class3 method_3(string string_0)
	{
		Class3 @class = new Class3();
		if (string_0 == null)
		{
			return @class;
		}
		Regex object_ = Delegate35.smethod_0("td>(?<date>[0-9/]+)</td><td colspan=\"3\">(?<action>[0-9.\\w\\s]+)");
		MatchCollection object_2 = Delegate235.smethod_0(object_, string_0);
		IEnumerator enumerator = Delegate236.smethod_0(object_2);
		try
		{
			while (enumerator.MoveNext())
			{
				Match object_3 = (Match)enumerator.Current;
				string string_ = Delegate193.smethod_1(Delegate237.smethod_0(object_3, "${date}"));
				string text = Delegate193.smethod_1(Delegate237.smethod_0(object_3, "${action}"));
				Log.Trace(Delegate185.smethod_0(string_, " ", text));
				if (!Delegate161.smethod_0(string_) && !Delegate161.smethod_0(text))
				{
					if (Delegate238.smethod_0(text, "Dividend"))
					{
						FundamentalItem fundamentalItem = new FundamentalItemMsnDividend(MsnFundamentalProvider.DividendName);
						Delegate240.smethod_0(fundamentalItem, Delegate239.smethod_0(string_, "M/d/yyyy", this.iformatProvider_0));
						Delegate243.smethod_0(fundamentalItem, Delegate242.smethod_0(Delegate193.smethod_1(Delegate241.smethod_0(text, "Dividend", "")), this.iformatProvider_0));
						@class.List_0.Add(fundamentalItem);
					}
					if (Delegate238.smethod_0(text, "Split"))
					{
						FundamentalItem fundamentalItem2 = new FundamentalItemMsnSplit(MsnFundamentalProvider.SplitName);
						Delegate240.smethod_0(fundamentalItem2, Delegate239.smethod_0(string_, "M/d/yyyy", this.iformatProvider_0));
						string[] array = Delegate244.smethod_0(text, new char[]
						{
							' '
						});
						double num = Delegate242.smethod_0(array[0], this.iformatProvider_0);
						double num2 = Delegate242.smethod_0(array[2], this.iformatProvider_0);
						Delegate243.smethod_0(fundamentalItem2, num / num2);
						@class.List_1.Add(fundamentalItem2);
					}
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return @class;
	}
	private Class3 method_4(Class2 class2_0)
	{
		string string_ = this.method_8(class2_0.String_0, class2_0.DateTime_1, class2_0.DateTime_2, Enum1.Fundamental);
		string string_2 = this.method_1(string_, false);
		return this.method_3(string_2);
	}
	private void method_5()
	{
		Class4 @class = null;
		try
		{
			bool flag = false;
			try
			{
				List<Class4> object_;
				Delegate54.smethod_0(object_ = this.list_0, ref flag);
				@class = this.list_0[Delegate138.smethod_0(Delegate169.smethod_0(Delegate168.smethod_0()))];
			}
			finally
			{
				if (flag)
				{
					List<Class4> object_;
					Delegate58.smethod_0(object_);
				}
			}
			while (!this.bool_0)
			{
				Class2 class2 = null;
				bool flag2 = false;
				try
				{
					Queue<Class2> object_2;
					Delegate54.smethod_0(object_2 = this.queue_0, ref flag2);
					if (this.queue_0.Count <= 0)
					{
						break;
					}
					class2 = this.queue_0.Dequeue();
				}
				finally
				{
					if (flag2)
					{
						Queue<Class2> object_2;
						Delegate58.smethod_0(object_2);
					}
				}
				if (class2 != null)
				{
					Exception ex = null;
					Bars bars = null;
					Class3 fundamental = null;
					try
					{
						bars = this.method_2(class2);
						if ((class2.Enum1_0 & Enum1.Fundamental) != (Enum1)0)
						{
							fundamental = this.method_4(class2);
						}
						goto IL_124;
					}
					catch (Exception ex2)
					{
						Log.TraceEvent(Enum0.Warning, Delegate185.smethod_0(class2.String_0, " ", Delegate127.smethod_0(ex2)));
						ex = ex2;
						goto IL_124;
					}
					goto IL_D6;
					IL_113:
					Delegate245.smethod_1(@class.ManualResetEvent_0);
					continue;
					IL_D6:
					if (ex == null)
					{
						if (this.delegate1_0 != null)
						{
							this.delegate1_0(this, new EventArgs1(class2, bars, fundamental));
							goto IL_113;
						}
						goto IL_113;
					}
					else
					{
						if (this.delegate2_0 != null)
						{
							this.delegate2_0(this, new EventArgs2(class2, ex));
							goto IL_113;
						}
						goto IL_113;
					}
					IL_124:
					Delegate245.smethod_0(@class.ManualResetEvent_0);
					if (!this.bool_0)
					{
						goto IL_D6;
					}
					goto IL_113;
				}
			}
		}
		catch (ThreadAbortException)
		{
			Log.Trace("Thread Abort");
		}
		catch (Exception ex3)
		{
			Log.TraceEvent(Enum0.Error, Delegate114.smethod_0("Thread execution error. ", Delegate127.smethod_0(ex3)));
			if (this.delegate2_0 != null)
			{
				this.delegate2_0(this, new EventArgs2(null, ex3));
			}
		}
		finally
		{
			if (@class != null)
			{
				Delegate245.smethod_1(@class.ManualResetEvent_1);
			}
		}
	}
	public void method_6(List<Class2> list_1)
	{
		Log.TraceMethod(new object[0]);
		this.list_0.Clear();
		List<ManualResetEvent> list = new List<ManualResetEvent>();
		foreach (Class2 current in list_1)
		{
			this.queue_0.Enqueue(current);
		}
		int num = (this.queue_0.Count < this.msnClientSettings_0.ThreadCount) ? this.queue_0.Count : this.msnClientSettings_0.ThreadCount;
		for (int i = 0; i < num; i++)
		{
			Thread thread = Delegate36.smethod_0(new ThreadStart(this.method_5));
			Delegate246.smethod_0(thread, i.ToString());
			Delegate162.smethod_0(thread, true);
			Class4 @class = new Class4(thread);
			this.list_0.Add(@class);
			list.Add(@class.ManualResetEvent_1);
		}
		foreach (Class4 current2 in this.list_0)
		{
			Delegate225.smethod_1(current2.Thread_0);
		}
		Delegate247.smethod_0(list.ToArray());
	}
	private Bars method_7(string string_0, string string_1)
	{
		Log.TraceMethod(new object[]
		{
			string_0
		});
		Bars bars = Delegate24.smethod_0(Class11.smethod_1(string_0), BarScale.Daily, 0);
		string empty = string.Empty;
		int i = -1;
		try
		{
			string[] array = Delegate234.smethod_0(string_1, new string[]
			{
				"\n"
			}, StringSplitOptions.RemoveEmptyEntries);
			for (i = 0; i < array.Length; i++)
			{
				if (!Delegate238.smethod_1(array[i], "\"") && !Delegate238.smethod_2(array[i], "(") && !Delegate238.smethod_2(array[i], "prices"))
				{
					if (Delegate238.smethod_1(array[i], "DATE"))
					{
						if (i + 2 < array.Length)
						{
							string[] array2 = Delegate244.smethod_0(array[i + 2], new char[]
							{
								'('
							});
							if (array2.Length > 1)
							{
								Delegate175.smethod_0(bars, Delegate193.smethod_0(Delegate193.smethod_1(array2[0])));
							}
						}
					}
					else
					{
						string[] array2 = Delegate244.smethod_0(array[i], new char[]
						{
							','
						});
						if (array2.Length == 6)
						{
							DateTime dateTime_ = Delegate248.smethod_0(array2[0], this.iformatProvider_0);
							double num = Delegate242.smethod_0(array2[1], this.iformatProvider_0);
							double double_ = Delegate242.smethod_0(array2[2], this.iformatProvider_0);
							double double_2 = Delegate242.smethod_0(array2[3], this.iformatProvider_0);
							double double_3 = Delegate242.smethod_0(array2[4], this.iformatProvider_0);
							double double_4 = Delegate242.smethod_0(array2[5], this.iformatProvider_0);
							if (num != 0.0)
							{
								Delegate249.smethod_0(bars, dateTime_, num, double_, double_2, double_3, double_4);
							}
						}
					}
				}
			}
		}
		catch (Exception object_)
		{
			string text = Delegate188.smethod_0("Data parsing error. Symbol: {0}, LineNumber: {1}\r\nString: {2}\r\nMessage: {3}", new object[]
			{
				string_0,
				i,
				empty,
				Delegate127.smethod_0(object_)
			});
			Log.TraceEvent(Enum0.Error, text);
			throw new ParsingException(text);
		}
		return bars;
	}
	private string method_8(string string_0, DateTime dateTime_0, DateTime dateTime_1, Enum1 enum1_0)
	{
		int day = dateTime_0.Day;
		int month = dateTime_0.Month;
		int year = dateTime_0.Year;
		int day2 = dateTime_1.Day;
		int month2 = dateTime_1.Month;
		int year2 = dateTime_1.Year;
		int num = 0;
		switch (enum1_0)
		{
		case Enum1.History:
			return Delegate188.smethod_0("http://data.moneycentral.msn.com/scripts/chrtsrv.dll?Symbol={0}&FileDownload=&C1=2&C5D={1}&C5={2}&C6={3}&C7D={4}&C7={5}&C8={6}&C9={7}", new object[]
			{
				string_0,
				day,
				month,
				year,
				day2,
				month2,
				year2,
				num
			});
		case Enum1.Fundamental:
			return Delegate126.smethod_0("http://moneycentral.msn.com/investor/charts/chartdl.aspx?PT=11&compsyms=&D4=1&D5=0&DCS=2&MA0=0&MA1=0&D7=&D6=&showtablbt=View+price+history+with+dividends%2Fsplits&symbol={0}&nocookie=1&SZ=0", string_0);
		default:
			return string.Empty;
		}
	}
}
