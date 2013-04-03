using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using WealthLab.DataProviders.Helper;

internal class Class16
{
    private AsyncCompletedEventHandler asyncCompletedEventHandler_0;
    private DownloadProgressChangedEventHandler downloadProgressChangedEventHandler_0;
    private Exception exception_0;
    private ManualResetEvent manualResetEvent_0 = new ManualResetEvent(false);
    private string string_0;
    private string string_1;
    private string string_2;
    private WebClient webClient_0 = new WebClient();

    public Class16(string string_3, string string_4)
    {
        this.string_0 = string_3;
        this.string_1 = Path.ChangeExtension(string_3, ".tmp");
        this.string_2 = string_4;
        this.webClient_0.Headers.Add(HttpRequestHeader.UserAgent, Path.GetFileNameWithoutExtension(string_3));
        this.webClient_0.DownloadFileCompleted += new AsyncCompletedEventHandler(this.webClient_0_DownloadFileCompleted);
        this.webClient_0.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.webClient_0_DownloadProgressChanged);
    }

    public void method_0(DownloadProgressChangedEventHandler downloadProgressChangedEventHandler_1)
    {
        DownloadProgressChangedEventHandler handler2;
        DownloadProgressChangedEventHandler handler = this.downloadProgressChangedEventHandler_0;
        do
        {
            handler2 = handler;
            DownloadProgressChangedEventHandler handler3 = (DownloadProgressChangedEventHandler) Delegate.Combine(handler2, downloadProgressChangedEventHandler_1);
            handler = Interlocked.CompareExchange<DownloadProgressChangedEventHandler>(ref this.downloadProgressChangedEventHandler_0, handler3, handler2);
        }
        while (handler != handler2);
    }

    public void method_1(DownloadProgressChangedEventHandler downloadProgressChangedEventHandler_1)
    {
        DownloadProgressChangedEventHandler handler2;
        DownloadProgressChangedEventHandler handler = this.downloadProgressChangedEventHandler_0;
        do
        {
            handler2 = handler;
            DownloadProgressChangedEventHandler handler3 = (DownloadProgressChangedEventHandler) Delegate.Remove(handler2, downloadProgressChangedEventHandler_1);
            handler = Interlocked.CompareExchange<DownloadProgressChangedEventHandler>(ref this.downloadProgressChangedEventHandler_0, handler3, handler2);
        }
        while (handler != handler2);
    }

    public void method_2(AsyncCompletedEventHandler asyncCompletedEventHandler_1)
    {
        AsyncCompletedEventHandler handler2;
        AsyncCompletedEventHandler handler = this.asyncCompletedEventHandler_0;
        do
        {
            handler2 = handler;
            AsyncCompletedEventHandler handler3 = (AsyncCompletedEventHandler) Delegate.Combine(handler2, asyncCompletedEventHandler_1);
            handler = Interlocked.CompareExchange<AsyncCompletedEventHandler>(ref this.asyncCompletedEventHandler_0, handler3, handler2);
        }
        while (handler != handler2);
    }

    public void method_3(AsyncCompletedEventHandler asyncCompletedEventHandler_1)
    {
        AsyncCompletedEventHandler handler2;
        AsyncCompletedEventHandler handler = this.asyncCompletedEventHandler_0;
        do
        {
            handler2 = handler;
            AsyncCompletedEventHandler handler3 = (AsyncCompletedEventHandler) Delegate.Remove(handler2, asyncCompletedEventHandler_1);
            handler = Interlocked.CompareExchange<AsyncCompletedEventHandler>(ref this.asyncCompletedEventHandler_0, handler3, handler2);
        }
        while (handler != handler2);
    }

    public Exception method_4()
    {
        return this.exception_0;
    }

    public bool method_5()
    {
        return System.IO.File.Exists(this.string_0);
    }

    public ClassificationGroup method_6()
    {
        return ClassificationGroup.Deserealize(this.string_0);
    }

    public void method_7(bool bool_0)
    {
        this.exception_0 = null;
        this.webClient_0.DownloadFileAsync(new Uri(this.string_2), this.string_1);
        this.manualResetEvent_0.Reset();
        if (!bool_0)
        {
            this.manualResetEvent_0.WaitOne();
        }
    }

    public bool method_8(int int_0)
    {
        return (DateTime.Now.ToUniversalTime().Subtract(System.IO.File.GetLastWriteTimeUtc(this.string_0)).Days >= int_0);
    }

    private void webClient_0_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
        this.exception_0 = e.Error;
        if (this.exception_0 == null)
        {
            if (System.IO.File.Exists(this.string_0))
            {
                System.IO.File.Delete(this.string_0);
            }
            System.IO.File.Move(this.string_1, this.string_0);
            System.IO.File.Delete(this.string_1);
        }
        if (this.asyncCompletedEventHandler_0 != null)
        {
            this.asyncCompletedEventHandler_0(this, e);
        }
        this.manualResetEvent_0.Set();
    }

    private void webClient_0_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        if (this.downloadProgressChangedEventHandler_0 != null)
        {
            this.downloadProgressChangedEventHandler_0(this, e);
        }
    }
}

