using System;
using System.Threading;

internal class ThreadControl  ///WYJ note, thread control for DataFetcher
{
    private ManualResetEvent manualResetEvent_0;
    private ManualResetEvent manualResetEvent_1;
    private Thread thread;

    public ThreadControl(Thread thread_1)
    {
        this.thread = thread_1;
        this.manualResetEvent_0 = new ManualResetEvent(true);
        this.manualResetEvent_1 = new ManualResetEvent(false);
    }

    public Thread GetThread()
    {
        return this.thread;
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

