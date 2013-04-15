using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using WealthLab.Cryptography;
using WealthLab.Extensions;

internal class Class43
{
    private bool bool_0;
    public Class46 class46_0;
    private Delegate17 delegate17_0;
    private Delegate18 delegate18_0;
    private int int_0;
    private int int_1;
    private List<string> list_0;
    private List<string> list_1;
    private string string_0;
    private string string_1;
    private string string_2;
    private WebClient webClient_0;

    public Class43(Class46 class46_1, string[] string_3)
    {
        this.class46_0 = class46_1;
        this.list_0 = new List<string>();
        this.list_1 = new List<string>();
        this.list_0.AddRange(string_3);
        this.webClient_0 = new WebClient();
        this.webClient_0.DownloadFileCompleted += new AsyncCompletedEventHandler(this.webClient_0_DownloadFileCompleted);
        this.webClient_0.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.webClient_0_DownloadProgressChanged);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_0(Delegate17 delegate17_1)
    {
        this.delegate17_0 = (Delegate17) Delegate.Combine(this.delegate17_0, delegate17_1);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_1(Delegate17 delegate17_1)
    {
        this.delegate17_0 = (Delegate17) Delegate.Remove(this.delegate17_0, delegate17_1);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_2(Delegate18 delegate18_1)
    {
        this.delegate18_0 = (Delegate18) Delegate.Combine(this.delegate18_0, delegate18_1);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_3(Delegate18 delegate18_1)
    {
        this.delegate18_0 = (Delegate18) Delegate.Remove(this.delegate18_0, delegate18_1);
    }

    private void method_4()
    {
        if (!this.bool_0)
        {
            if (this.list_0.Count == 0)
            {
                if ((this.delegate18_0 != null) && !this.bool_0)
                {
                    this.delegate18_0(this, new EventArgs12());
                }
            }
            else
            {
                this.string_0 = this.list_0[0];
                this.string_1 = this.method_7(this.string_0);
                this.list_0.RemoveAt(0);
                this.int_1++;
                this.string_2 = Path.Combine(ExtensionManager.GetTempDir, Class44.smethod_1(this.class46_0.method_8().DisplayName)) + Path.DirectorySeparatorChar;
                if (!Directory.Exists(this.string_2))
                {
                    Directory.CreateDirectory(this.string_2);
                }
                string item = Path.Combine(this.string_2, this.string_1);
                this.list_1.Add(item);
                this.webClient_0.DownloadFileAsync(new Uri(this.string_0), item, this.string_1);
                if (this.delegate17_0 != null)
                {
                    this.delegate17_0(this, new EventArgs11(0L, 0L, 0, this.int_1, this.int_0, this.string_0, this.string_1));
                }
            }
        }
    }

    private void method_5(string string_3)
    {
        try
        {
            byte[] extensionKey = Class41.smethod_0().GetExtensionKey(this.class46_0.method_8().StrongName, this.class46_0.method_8().Version);
            byte[] extensionIV = Class41.smethod_0().GetExtensionIV(this.class46_0.method_8().StrongName, this.class46_0.method_8().Version);
            string str = Path.GetFileNameWithoutExtension(string_3) + ".tmp";
            RijndaelCryptography cryptography = new RijndaelCryptography(extensionKey, extensionIV);
            if (System.IO.File.Exists(Path.Combine(this.string_2, string_3)))
            {
                cryptography.DecryptFile(Path.Combine(this.string_2, string_3), Path.Combine(this.string_2, str));
            }
            if (System.IO.File.Exists(Path.Combine(this.string_2, string_3)))
            {
                System.IO.File.Delete(Path.Combine(this.string_2, string_3));
            }
        }
        catch (Exception exception)
        {
            this.method_9(exception.Message);
        }
    }

    public void method_6()
    {
        this.int_0 = this.list_0.Count;
        if (this.int_0 == 0)
        {
            if (this.delegate18_0 != null)
            {
                this.delegate18_0(this, new EventArgs12("Missing download link."));
            }
        }
        else
        {
            this.method_4();
        }
    }

    private string method_7(string string_3)
    {
        string[] strArray = string_3.Split(new char[] { '/' });
        if (strArray.Length > 1)
        {
            return strArray[strArray.Length - 1];
        }
        return string.Empty;
    }

    private void method_8()
    {
        if (Directory.Exists(this.string_2))
        {
            Directory.Delete(this.string_2, true);
        }
    }

    public void method_9(string string_3)
    {
        bool flag = false;
        Cursor.Current = Cursors.WaitCursor;
        try
        {
            this.bool_0 = true;
            this.webClient_0.CancelAsync();
            for (int i = 0; i < 100; i++)
            {
                if (!this.webClient_0.IsBusy)
                {
                    ///goto  Label_0041;  ///WYJ fix, simplify the flow
                    flag = true;
                    break;
                }
                Thread.Sleep(100);
            }
            if (!flag)
            {
                MessageBox.Show("Download cancel request failed", "Cancellation error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
        this.method_8();
        if (string_3 == null)
        {
            this.delegate18_0(this, new EventArgs12(true));
        }
        else
        {
            this.delegate18_0(this, new EventArgs12(string_3));
        }
    }

    private void webClient_0_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
        if (((e.Error != null) && !e.Cancelled) && (this.delegate18_0 != null))
        {
            this.delegate18_0(this, new EventArgs12(((string) e.UserState) + ", " + e.Error.Message));
            this.method_8();
        }
        else if (!this.bool_0)
        {
            this.method_5((string) e.UserState);
            this.method_4();
        }
    }

    private void webClient_0_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        if ((this.delegate17_0 != null) && !this.bool_0)
        {
            this.delegate17_0(this, new EventArgs11(e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage, this.int_1, this.int_0, this.string_0, this.string_1));
        }
    }

    public delegate void Delegate17(object sender, EventArgs11 e);

    public delegate void Delegate18(object sender, EventArgs12 e);
}

