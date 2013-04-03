using System;
using System.Threading;
using System.Windows.Forms;

internal static class Class18
{
    private static ManualResetEvent manualResetEvent_0 = new ManualResetEvent(true);
    private static string string_0 = null;
    private static string string_1 = null;
    private static string string_2;
    private static string string_3;

    public static string smethod_0()
    {
        return string_0;
    }

    public static string smethod_1()
    {
        return string_1;
    }

    public static string smethod_2()
    {
        return string_2;
    }

    public static string smethod_3()
    {
        return string_3;
    }

    public static void smethod_4()
    {
        Class21.smethod_8(new object[0]);
        string_0 = null;
    }

    private static void smethod_5()
    {
        try
        {
            WebBrowser browser = new WebBrowser();
            browser.Navigate(string.Format("https://login.yahoo.com/config/login?.done=http://finance.yahoo.com%2f&.src=quote&.intl=us&login={0}&passwd={1}", string_2, string_3));
            while (!browser.IsBusy)
            {
                Thread.Sleep(50);
                Application.DoEvents();
            }
            string_0 = browser.Document.Cookie;
            Class21.smethod_2("Cookie: " + string_0);
        }
        catch (Exception exception)
        {
            Class21.smethod_4(Enum2.const_3, exception.Message);
            string_1 = exception.Message;
        }
        finally
        {
            manualResetEvent_0.Set();
        }
    }

    public static void smethod_6(string string_4, string string_5)
    {
        Class21.smethod_8(new object[] { string_4, string_5 });
        manualResetEvent_0.Reset();
        string_0 = string.Empty;
        string_1 = null;
        string_2 = string_4;
        string_3 = string_5.Trim();
        string_3 = string_3.Trim(new char[1]);
        Thread thread = new Thread(new ThreadStart(Class18.smethod_5));
        thread.SetApartmentState(ApartmentState.STA);
        thread.IsBackground = true;
        thread.Start();
        if (!manualResetEvent_0.WaitOne(0x2710, false))
        {
            string_1 = "Time out";
        }
    }
}

