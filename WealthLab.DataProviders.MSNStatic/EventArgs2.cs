using System;
internal class EventArgs2 : EventArgs0
{
	public readonly Exception exception_0;
	public EventArgs2(Class2 request, Exception error) : base(request)
	{
		this.exception_0 = error;
	}
}
