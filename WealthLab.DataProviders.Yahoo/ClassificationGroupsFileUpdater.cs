using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using WealthLab.DataProviders.Helper;

///WYJ fix, original name Class16
internal class ClassificationGroupsFileUpdater
{
    private AsyncCompletedEventHandler asyncCompletedEventHandler_0;
    private DownloadProgressChangedEventHandler downloadProgressChangedEventHandler_0;
    private Exception exception_0;
    private ManualResetEvent manualResetEvent_0 = new ManualResetEvent(false);
    private string filePath;
    private string tempFilePath;
    private string classificationXmlUrl;
    private WebClient webClient_0 = new WebClient();

    public ClassificationGroupsFileUpdater(string path, string url)
    {
        this.filePath = path;
        this.tempFilePath = Path.ChangeExtension(path, ".tmp");
        this.classificationXmlUrl = url;
        this.webClient_0.Headers.Add(HttpRequestHeader.UserAgent, Path.GetFileNameWithoutExtension(path));
        this.webClient_0.DownloadFileCompleted += new AsyncCompletedEventHandler(this.webClient_0_DownloadFileCompleted);
        this.webClient_0.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.webClient_0_DownloadProgressChanged);
    }

    ///WYJ fix, original name: method_0
    public void AddDownloadProgressChangedEventHandler(DownloadProgressChangedEventHandler handler)
    {
        DownloadProgressChangedEventHandler prevHandler;
        DownloadProgressChangedEventHandler tmp = this.downloadProgressChangedEventHandler_0;
        do
        {
            prevHandler = tmp;
            DownloadProgressChangedEventHandler handler3 = (DownloadProgressChangedEventHandler) Delegate.Combine(prevHandler, handler);
            tmp = Interlocked.CompareExchange<DownloadProgressChangedEventHandler>(ref this.downloadProgressChangedEventHandler_0, handler3, prevHandler);
        }
        while (tmp != prevHandler);
    }

    ///WYJ fix, original name: method_1
    public void DeleteDownloadProgressChangedEventHandler(DownloadProgressChangedEventHandler handler)
    {
        DownloadProgressChangedEventHandler prevHandler;
        DownloadProgressChangedEventHandler tmp = this.downloadProgressChangedEventHandler_0;
        do
        {
            prevHandler = tmp;
            DownloadProgressChangedEventHandler handler3 = (DownloadProgressChangedEventHandler) Delegate.Remove(prevHandler, handler);
            tmp = Interlocked.CompareExchange<DownloadProgressChangedEventHandler>(ref this.downloadProgressChangedEventHandler_0, handler3, prevHandler);
        }
        while (tmp != prevHandler);
    }

    ///WYJ fix, original name: method_2
    public void AddAsyncCompletedEventHandler(AsyncCompletedEventHandler handler)
    {
        AsyncCompletedEventHandler prevHandler;
        AsyncCompletedEventHandler tmp = this.asyncCompletedEventHandler_0;
        do
        {
            prevHandler = tmp;
            AsyncCompletedEventHandler handler3 = (AsyncCompletedEventHandler) Delegate.Combine(prevHandler, handler);
            tmp = Interlocked.CompareExchange<AsyncCompletedEventHandler>(ref this.asyncCompletedEventHandler_0, handler3, prevHandler);
        }
        while (tmp != prevHandler);
    }

    ///WYJ fix, original name: method_3
    public void DeleteAsyncCompletedEventHandler(AsyncCompletedEventHandler handler)
    {
        AsyncCompletedEventHandler prevHandler;
        AsyncCompletedEventHandler tmp = this.asyncCompletedEventHandler_0;
        do
        {
            prevHandler = tmp;
            AsyncCompletedEventHandler handler3 = (AsyncCompletedEventHandler) Delegate.Remove(prevHandler, handler);
            tmp = Interlocked.CompareExchange<AsyncCompletedEventHandler>(ref this.asyncCompletedEventHandler_0, handler3, prevHandler);
        }
        while (tmp != prevHandler);
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

