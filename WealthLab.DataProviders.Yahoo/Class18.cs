using System;
using System.Threading;
using System.Windows.Forms;

///WYJ fix, original name Class18
internal static class Login  ///WYJ note, Login class
{
    private static ManualResetEvent manualResetEvent_0 = new ManualResetEvent(true);
    private static string cookie = null;
    private static string errorMessage = null;
    private static string user;
    private static string password;

    public static string GetCookie()
    {
        return cookie;
    }

    public static string GetErrorMessage()
    {
        return errorMessage;
    }

    public static string GetUser()
    {
        return user;
    }

    public static string GetPassword()
    {
        return password;
    }

    public static void Reset()
    {
        Logger.LogParameters(new object[0]);
        cookie = null;
    }

    private static void doLogin()
    {
        try
        {
            WebBrowser browser = new WebBrowser();
            browser.Navigate(string.Format("https://login.yahoo.com/config/login?.done=http://finance.yahoo.com%2f&.src=quote&.intl=us&login={0}&passwd={1}", user, password));
            while (!browser.IsBusy)
            {
                Thread.Sleep(50);
                Application.DoEvents();
            }
            cookie = browser.Document.Cookie;
            Logger.Log("Cookie: " + cookie);
        }
        catch (Exception exception)
        {
            Logger.Log(Enum2.const_3, exception.Message);
            errorMessage = exception.Message;
        }
        finally
        {
            manualResetEvent_0.Set();
        }
    }

    public static void LoginWith(string pUser, string pPassword)
    {
        Logger.LogParameters(new object[] { pUser, pPassword });
        manualResetEvent_0.Reset();
        cookie = string.Empty;
        errorMessage = null;
        user = pUser;
        password = pPassword.Trim();
        password = password.Trim(new char[1]);
        Thread thread = new Thread(new ThreadStart(Login.doLogin));
        thread.SetApartmentState(ApartmentState.STA);
        thread.IsBackground = true;
        thread.Start();
        if (!manualResetEvent_0.WaitOne(0x2710, false))
        {
            errorMessage = "Time out";
        }
    }
}

