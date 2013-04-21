using System;
using System.Collections.Generic;
using WealthLab;
internal class Class6
{
	private Dictionary<string, List<FundamentalItem>> dictionary_0;
	public string string_0;
	public string string_1;
	public int int_0;
	public Dictionary<string, List<FundamentalItem>> Dictionary_0
	{
		get
		{
			return this.dictionary_0;
		}
	}
	public string String_0
	{
		get
		{
			return this.string_0;
		}
	}
	public string String_1
	{
		get
		{
			return this.string_1;
		}
	}
	public int Int32_0
	{
		get
		{
			return this.int_0;
		}
	}
	public Class6(Dictionary<string, List<FundamentalItem>> list, string symbolName, string name, int threadNum)
	{
		this.dictionary_0 = list;
		this.string_0 = symbolName;
		this.string_1 = name;
		this.int_0 = threadNum;
	}
}
