using System;
using System.Threading;

internal class ThreadControl  ///WYJ note, thread control for DataFetcher
{
    private ManualResetEvent manualResetEvent_NotErrorHandling;
    private ManualResetEvent manualResetEvent_ThreadFinish;  ///WYJ note, thread finish event
    private Thread thread;

    public ThreadControl(Thread thread_1)
    {
        this.thread = thread_1;
        this.manualResetEvent_NotErrorHandling = new ManualResetEvent(true);
        this.manualResetEvent_ThreadFinish = new ManualResetEvent(false);
    }

    public Thread GetThread()
    {
        return this.thread;
    }

    public ManualResetEvent notErrorHandling()
    {
        return this.manualResetEvent_NotErrorHandling;
    }

    public ManualResetEvent getThreadFinishEvent()
    {
        return this.manualResetEvent_ThreadFinish;
    }
}

