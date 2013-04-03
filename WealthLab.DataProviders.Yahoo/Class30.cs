using System;
using System.Threading;

internal class Class30
{
    private ManualResetEvent manualResetEvent_0;
    private ManualResetEvent manualResetEvent_1;
    private Thread thread_0;

    public Class30(Thread thread_1)
    {
        this.thread_0 = thread_1;
        this.manualResetEvent_0 = new ManualResetEvent(true);
        this.manualResetEvent_1 = new ManualResetEvent(false);
    }

    public Thread method_0()
    {
        return this.thread_0;
    }

    public ManualResetEvent method_1()
    {
        return this.manualResetEvent_0;
    }

    public ManualResetEvent method_2()
    {
        return this.manualResetEvent_1;
    }
}

