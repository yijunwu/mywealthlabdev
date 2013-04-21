using System;
using WealthLab;
internal class EventArgs1 : EventArgs0
{
	public readonly Bars bars_0;
	public readonly Class3 class3_0;
	public EventArgs1(Class2 request, Bars bars, Class3 fundamental) : base(request)
	{
		this.bars_0 = bars;
		this.class3_0 = fundamental;
	}
}
