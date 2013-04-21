using System;
using System.Collections.Generic;
using WealthLab;
using WealthLab.DataProviders.Helper;
internal class Class10
{
	private IList<FundamentalItem> ilist_0;
	private IList<FundamentalItem> ilist_1;
	private List<FundamentalItem> list_0;
	private Enum3 enum3_0;
	public Class10(IList<FundamentalItem> splits, IList<FundamentalItem> dividends, Enum3 mode)
	{
		Log.TraceMethodInfo(new object[]
		{
			mode
		});
		this.ilist_0 = splits;
		this.ilist_1 = dividends;
		this.enum3_0 = mode;
	}
	private int method_0(FundamentalItem fundamentalItem_0, FundamentalItem fundamentalItem_1)
	{
		return Delegate177.smethod_0(fundamentalItem_0).CompareTo(Delegate177.smethod_0(fundamentalItem_1));
	}
	private void method_1()
	{
		this.list_0 = new List<FundamentalItem>();
		if ((this.enum3_0 & Enum3.Dividend) != (Enum3)0)
		{
			this.list_0.AddRange(this.ilist_1);
		}
		if ((this.enum3_0 & Enum3.Split) != (Enum3)0)
		{
			this.list_0.AddRange(this.ilist_0);
		}
		this.list_0.Sort(new Comparison<FundamentalItem>(this.method_0));
	}
	public Bars method_2(Bars bars_0)
	{
		Log.TraceMethodInfo(new object[]
		{
			Delegate174.smethod_1(bars_0)
		});
		this.method_1();
		Bars object_ = Delegate24.smethod_0(Delegate174.smethod_1(bars_0), Delegate305.smethod_0(bars_0), Delegate172.smethod_1(bars_0));
		double num = 1.0;
		double num2 = 1.0;
		for (int i = Delegate172.smethod_0(bars_0) - 1; i >= 0; i--)
		{
			if (this.list_0.Count > 0 && Delegate178.smethod_1(Delegate187.smethod_0(bars_0)[i].Date, Delegate177.smethod_0(this.list_0[this.list_0.Count - 1]).Date))
			{
				while (Delegate178.smethod_1(Delegate187.smethod_0(bars_0)[i].Date, Delegate177.smethod_0(this.list_0[this.list_0.Count - 1]).Date))
				{
					if (Delegate238.smethod_1(Delegate55.smethod_0(this.list_0[this.list_0.Count - 1]), "D"))
					{
						double num3 = Delegate179.smethod_0(this.list_0[this.list_0.Count - 1]);
						num *= 1.0 - num3 / Delegate181.smethod_0(Delegate180.smethod_3(bars_0), i);
					}
					if (Delegate238.smethod_1(Delegate55.smethod_0(this.list_0[this.list_0.Count - 1]), "S"))
					{
						double num4 = 1.0 / Delegate179.smethod_0(this.list_0[this.list_0.Count - 1]);
						num *= num4;
						num2 *= num4;
					}
					this.list_0.RemoveAt(this.list_0.Count - 1);
					if (this.list_0.Count == 0)
					{
						break;
					}
				}
			}
			Delegate249.smethod_0(object_, Delegate187.smethod_0(bars_0)[i], Delegate181.smethod_0(Delegate180.smethod_0(bars_0), i) * num, Delegate181.smethod_0(Delegate180.smethod_1(bars_0), i) * num, Delegate181.smethod_0(Delegate180.smethod_2(bars_0), i) * num, Delegate181.smethod_0(Delegate180.smethod_3(bars_0), i) * num, Delegate181.smethod_0(Delegate180.smethod_4(bars_0), i) * num2);
		}
		Bars bars = Delegate24.smethod_0(Delegate174.smethod_1(bars_0), Delegate305.smethod_0(bars_0), Delegate172.smethod_1(bars_0));
		Delegate175.smethod_0(bars, Delegate174.smethod_0(bars_0));
		for (int j = Delegate172.smethod_0(object_) - 1; j >= 0; j--)
		{
			Delegate249.smethod_0(bars, Delegate187.smethod_0(object_)[j], Delegate181.smethod_0(Delegate180.smethod_0(object_), j), Delegate181.smethod_0(Delegate180.smethod_1(object_), j), Delegate181.smethod_0(Delegate180.smethod_2(object_), j), Delegate181.smethod_0(Delegate180.smethod_3(object_), j), Delegate181.smethod_0(Delegate180.smethod_4(object_), j));
		}
		return bars;
	}
}
