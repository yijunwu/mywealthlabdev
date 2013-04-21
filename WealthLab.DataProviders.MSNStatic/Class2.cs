using System;
internal class Class2 : IComparable
{
	private string string_0;
	private DateTime dateTime_0;
	private DateTime dateTime_1;
	private DateTime dateTime_2;
	private Enum1 enum1_0;
	public string String_0
	{
		get
		{
			return this.string_0;
		}
	}
	public Enum1 Enum1_0
	{
		get
		{
			return this.enum1_0;
		}
		set
		{
			this.enum1_0 = value;
		}
	}
	public DateTime DateTime_0
	{
		get
		{
			return this.dateTime_0;
		}
		set
		{
			this.dateTime_0 = value;
		}
	}
	public DateTime DateTime_1
	{
		get
		{
			return this.dateTime_1;
		}
		set
		{
			this.dateTime_1 = value;
		}
	}
	public DateTime DateTime_2
	{
		get
		{
			return this.dateTime_2;
		}
		set
		{
			this.dateTime_2 = value;
		}
	}
	public Class2(string symbol, DateTime startDate, DateTime endDate, DateTime startDateFund, Enum1 mode) : this(symbol)
	{
		this.dateTime_0 = startDate;
		this.dateTime_2 = endDate;
		this.dateTime_1 = startDateFund;
		this.enum1_0 = mode;
	}
	public Class2(string symbol)
	{
		this.string_0 = symbol;
	}
	public int CompareTo(object obj)
	{
		return Delegate250.smethod_0(this.String_0, (obj as Class2).String_0);
	}
}
