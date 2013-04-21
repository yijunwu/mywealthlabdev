using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using WealthLab.DataProviders.Helper;
internal class Class8
{
	private WebClient webClient_0 = Delegate48.smethod_0();
	private ManualResetEvent manualResetEvent_0 = Delegate37.smethod_0(false);
	private string string_0;
	private string string_1;
	private Exception exception_0;
	private string string_2;
	private DownloadProgressChangedEventHandler downloadProgressChangedEventHandler_0;
	private AsyncCompletedEventHandler asyncCompletedEventHandler_0;
	public event DownloadProgressChangedEventHandler Event_0
	{
		add
		{
			DownloadProgressChangedEventHandler downloadProgressChangedEventHandler = this.downloadProgressChangedEventHandler_0;
			DownloadProgressChangedEventHandler downloadProgressChangedEventHandler2;
			do
			{
				downloadProgressChangedEventHandler2 = downloadProgressChangedEventHandler;
				DownloadProgressChangedEventHandler value2 = (DownloadProgressChangedEventHandler)Delegate223.smethod_0(downloadProgressChangedEventHandler2, value);
				downloadProgressChangedEventHandler = Interlocked.CompareExchange<DownloadProgressChangedEventHandler>(ref this.downloadProgressChangedEventHandler_0, value2, downloadProgressChangedEventHandler2);
			}
			while (downloadProgressChangedEventHandler != downloadProgressChangedEventHandler2);
		}
		remove
		{
			DownloadProgressChangedEventHandler downloadProgressChangedEventHandler = this.downloadProgressChangedEventHandler_0;
			DownloadProgressChangedEventHandler downloadProgressChangedEventHandler2;
			do
			{
				downloadProgressChangedEventHandler2 = downloadProgressChangedEventHandler;
				DownloadProgressChangedEventHandler value2 = (DownloadProgressChangedEventHandler)Delegate223.smethod_1(downloadProgressChangedEventHandler2, value);
				downloadProgressChangedEventHandler = Interlocked.CompareExchange<DownloadProgressChangedEventHandler>(ref this.downloadProgressChangedEventHandler_0, value2, downloadProgressChangedEventHandler2);
			}
			while (downloadProgressChangedEventHandler != downloadProgressChangedEventHandler2);
		}
	}
	public event AsyncCompletedEventHandler Event_1
	{
		add
		{
			AsyncCompletedEventHandler asyncCompletedEventHandler = this.asyncCompletedEventHandler_0;
			AsyncCompletedEventHandler asyncCompletedEventHandler2;
			do
			{
				asyncCompletedEventHandler2 = asyncCompletedEventHandler;
				AsyncCompletedEventHandler value2 = (AsyncCompletedEventHandler)Delegate223.smethod_0(asyncCompletedEventHandler2, value);
				asyncCompletedEventHandler = Interlocked.CompareExchange<AsyncCompletedEventHandler>(ref this.asyncCompletedEventHandler_0, value2, asyncCompletedEventHandler2);
			}
			while (asyncCompletedEventHandler != asyncCompletedEventHandler2);
		}
		remove
		{
			AsyncCompletedEventHandler asyncCompletedEventHandler = this.asyncCompletedEventHandler_0;
			AsyncCompletedEventHandler asyncCompletedEventHandler2;
			do
			{
				asyncCompletedEventHandler2 = asyncCompletedEventHandler;
				AsyncCompletedEventHandler value2 = (AsyncCompletedEventHandler)Delegate223.smethod_1(asyncCompletedEventHandler2, value);
				asyncCompletedEventHandler = Interlocked.CompareExchange<AsyncCompletedEventHandler>(ref this.asyncCompletedEventHandler_0, value2, asyncCompletedEventHandler2);
			}
			while (asyncCompletedEventHandler != asyncCompletedEventHandler2);
		}
	}
	public Exception Exception_0
	{
		get
		{
			return this.exception_0;
		}
	}
	public bool Boolean_0
	{
		get
		{
			return Delegate161.smethod_1(this.string_0);
		}
	}
	public ClassificationGroup ClassificationGroup_0
	{
		get
		{
			return ClassificationGroup.Deserealize(this.string_0);
		}
	}
	public Class8(string fileName, string url)
	{
		this.string_0 = fileName;
		this.string_1 = Delegate114.smethod_1(fileName, ".tmp");
		this.string_2 = url;
		Delegate297.smethod_0(Delegate296.smethod_0(this.webClient_0), HttpRequestHeader.UserAgent, Delegate199.smethod_1(fileName));
		Delegate298.smethod_0(this.webClient_0, new AsyncCompletedEventHandler(this.method_1));
		Delegate299.smethod_0(this.webClient_0, new DownloadProgressChangedEventHandler(this.method_0));
	}
	private void method_0(object sender, DownloadProgressChangedEventArgs e)
	{
		if (this.downloadProgressChangedEventHandler_0 != null)
		{
			Delegate300.smethod_0(this.downloadProgressChangedEventHandler_0, this, e);
		}
	}
	private void method_1(object sender, AsyncCompletedEventArgs e)
	{
		this.exception_0 = Delegate135.smethod_0(e);
		if (this.exception_0 == null)
		{
			if (Delegate161.smethod_1(this.string_0))
			{
				Delegate166.smethod_1(this.string_0);
			}
			Delegate301.smethod_0(this.string_1, this.string_0);
			Delegate166.smethod_1(this.string_1);
		}
		if (this.asyncCompletedEventHandler_0 != null)
		{
			Delegate302.smethod_0(this.asyncCompletedEventHandler_0, this, e);
		}
		Delegate245.smethod_1(this.manualResetEvent_0);
	}
	public void method_2(bool bool_0)
	{
		this.exception_0 = null;
		Delegate303.smethod_0(this.webClient_0, Delegate27.smethod_0(this.string_2), this.string_1);
		Delegate245.smethod_0(this.manualResetEvent_0);
		if (!bool_0)
		{
			Delegate224.smethod_0(this.manualResetEvent_0);
		}
	}
	public bool method_3(int int_0)
	{
		return Delegate200.smethod_0().ToUniversalTime().Subtract(Delegate304.smethod_0(this.string_0)).Days >= int_0;
	}
}
