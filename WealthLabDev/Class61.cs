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
        bool createdNew = true;
        using (new Mutex(true, Application.ProductName, out createdNew))
        {
            if (!createdNew)
            {
                goto Label_00D4;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string str = "";
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                goto Label_00A6;
            }
            foreach (NetworkInterface interface2 in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (((interface2.OperationalStatus == OperationalStatus.Up) && (interface2.NetworkInterfaceType != NetworkInterfaceType.Tunnel)) && (interface2.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                {
                    IPv4InterfaceStatistics statistics = interface2.GetIPv4Statistics();
                    if ((statistics.BytesReceived > 0L) && (statistics.BytesSent > 0L))
                    {
                        goto Label_0097;
                    }
                }
            }
            goto Label_00B8;
        Label_0097:
            str = interface2.GetPhysicalAddress().ToString();
            goto Label_00B8;
        Label_00A6:
            str = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
        Label_00B8:
            smethod_0(str);
            Application.Run(new MainForm());
            Process.GetCurrentProcess().Kill();
            return;
        Label_00D4:
            MessageBox.Show("Application: \"" + Application.ProductName + "\" is already running.", Application.ProductName);
        }
    }

    public static void smethod_0(string string_0)
    {
    }
}

