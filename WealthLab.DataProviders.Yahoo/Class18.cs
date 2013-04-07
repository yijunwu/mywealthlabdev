using System;
using System.Threading;
using System.Windows.Forms;

internal static class Class18
{
    private static ManualResetEvent manualResetEvent_0 = new ManualResetEvent(true);
    private static string cookie = null;
    private static string errorMessage = null;
    private static string string_2;
    private static string string_3;

    public static string GetCookie()
    {
        return cookie;
    }

    public static string GetErrorMessage()
    {
        return errorMessage;
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
        cookie = null;
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
            cookie = browser.Document.Cookie;
            Class21.smethod_2("Cookie: " + cookie);
        }
        catch (Exception exception)
        {
            Class21.smethod_4(Enum2.const_3, exception.Message);
            errorMessage = exception.Message;
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
        cookie = string.Empty;
        errorMessage = null;
        string_2 = string_4;
        string_3 = string_5.Trim();
        string_3 = string_3.Trim(new char[1]);
        Thread thread = new Thread(new ThreadStart(Class18.smethod_5));
        thread.SetApartmentState(ApartmentState.STA);
        thread.IsBackground = true;
        thread.Start();
        if (!manualResetEvent_0.WaitOne(0x2710, false))
        {
            errorMessage = "Time out";
        }
    }
}

