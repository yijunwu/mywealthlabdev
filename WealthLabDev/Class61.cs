using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using WealthLabPro;

internal static class Class61
{
    [STAThread]
    private static void Main()
    {
        ServicePointManager.DefaultConnectionLimit = 10;
        bool flag = true;
        using (Mutex mutex = new Mutex(true, Application.ProductName, out flag))
        {
            if (!flag)
            {
                MessageBox.Show(string.Concat("Application: \"", Application.ProductName, "\" is already running."), Application.ProductName);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                string str = "";
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    str = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
                }
                else
                {
                    NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                    for (int i = 0; i < (int)allNetworkInterfaces.Length; i++)
                    {
                        NetworkInterface networkInterface = allNetworkInterfaces[i];
                        if (networkInterface.OperationalStatus == OperationalStatus.Up && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                        {
                            IPv4InterfaceStatistics pv4Statistics = networkInterface.GetIPv4Statistics();
                            if (pv4Statistics.BytesReceived > (long)0 && pv4Statistics.BytesSent > (long)0)
                            {
                                str = networkInterface.GetPhysicalAddress().ToString();
                                goto Label0;
                            }
                        }
                    }
                }
            Label0:
                Class61.smethod_0(str);
                Application.Run(new MainForm());
                Process.GetCurrentProcess().Kill();
            }
        }
    }

    public static void smethod_0(string string_0)
    {
    }
}

