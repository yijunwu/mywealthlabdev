using System;
using System.Collections.Generic;
using WealthLab;
internal class Class7
{
	private readonly Dictionary<string, List<FundamentalItem>> dictionary_0;
	private readonly Dictionary<string, List<FundamentalItem>> dictionary_1;
	private readonly Dictionary<string, List<FundamentalItem>> dictionary_2;
	private readonly Dictionary<string, List<FundamentalItem>> dictionary_3;
	public Dictionary<string, List<FundamentalItem>> Dictionary_0
	{
		get
		{
			return this.dictionary_0;
		}
	}
	public Dictionary<string, List<FundamentalItem>> Dictionary_1
	{
		get
		{
			return this.dictionary_1;
		}
	}
	public Dictionary<string, List<FundamentalItem>> Dictionary_2
	{
		get
		{
			return this.dictionary_2;
		}
	}
	public Dictionary<string, List<FundamentalItem>> Dictionary_3
	{
		get
		{
			return this.dictionary_3;
		}
	}
	public Class7(Dictionary<string, List<FundamentalItem>> BalanceSheetItems, Dictionary<string, List<FundamentalItem>> IncomeStatementItems, Dictionary<string, List<FundamentalItem>> CashFlowItems, Dictionary<string, List<FundamentalItem>> TenYrSummary)
	{
		this.dictionary_0 = BalanceSheetItems;
		this.dictionary_1 = IncomeStatementItems;
		this.dictionary_2 = CashFlowItems;
		this.dictionary_3 = TenYrSummary;
	}
}
