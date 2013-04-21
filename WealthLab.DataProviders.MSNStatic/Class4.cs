using System;
using System.Threading;
internal class Class4
{
	private Thread thread_0;
	private ManualResetEvent manualResetEvent_0;
	private ManualResetEvent manualResetEvent_1;
	public Thread Thread_0
	{
		get
		{
			return this.thread_0;
		}
	}
	public ManualResetEvent ManualResetEvent_0
	{
		get
		{
			return this.manualResetEvent_0;
		}
	}
	public ManualResetEvent ManualResetEvent_1
	{
		get
		{
			return this.manualResetEvent_1;
		}
	}
	public Class4(Thread thread)
	{
		this.thread_0 = thread;
		this.manualResetEvent_0 = Delegate37.smethod_0(true);
		this.manualResetEvent_1 = Delegate37.smethod_0(false);
	}
}
