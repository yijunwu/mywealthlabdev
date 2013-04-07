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
    private string filePath;
    private string tempFilePath;
    private string classificationXmlUrl;
    private WebClient webClient_0 = new WebClient();

    public Class16(string path, string url)
    {
        this.filePath = path;
        this.tempFilePath = Path.ChangeExtension(path, ".tmp");
        this.classificationXmlUrl = url;
        this.webClient_0.Headers.Add(HttpRequestHeader.UserAgent, Path.GetFileNameWithoutExtension(path));
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

    public Exception GetException()
    {
        return this.exception_0;
    }

    public bool FileExists()
    {
        return System.IO.File.Exists(this.filePath);
    }

    ///WYJ fix, original name: method_6
    public ClassificationGroup ReadClassificationGroupFromFile()
    {
        return ClassificationGroup.Deserealize(this.filePath);
    }

    public void UpdateClassificationGroupsFile(bool bool_0)
    {
        this.exception_0 = null;
        this.webClient_0.DownloadFileAsync(new Uri(this.classificationXmlUrl), this.tempFilePath);
        this.manualResetEvent_0.Reset();
        if (!bool_0)
        {
            this.manualResetEvent_0.WaitOne();
        }
    }

    public bool IsLastUpdatedDaysAgo(int int_0)
    {
        return (DateTime.Now.ToUniversalTime().Subtract(System.IO.File.GetLastWriteTimeUtc(this.filePath)).Days >= int_0);
    }

    private void webClient_0_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
        this.exception_0 = e.Error;
        if (this.exception_0 == null)
        {
            if (System.IO.File.Exists(this.filePath))
            {
                System.IO.File.Delete(this.filePath);
            }
            System.IO.File.Move(this.tempFilePath, this.filePath);
            System.IO.File.Delete(this.tempFilePath);
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

