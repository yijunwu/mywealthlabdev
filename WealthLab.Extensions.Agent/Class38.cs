using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WealthLab.Extensions.Agent;

internal static class Class38
{
    private static List<string> list_0 = new List<string>();
    private static Remoting remoting_0;
    private static readonly string string_0 = "WealthLabDev.exe";
    private static readonly string string_1 = "WealthLabPro.exe";
    private static string string_2;
    private static string string_3;

    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        if (smethod_3())
        {
            smethod_4();
        }
        else if (!smethod_2())
        {
            smethod_1();
            if (string_3 != null)
            {
                smethod_5();
                smethod_6();
                if (string_2 == null)
                {
                    MessageBox.Show("WealthLabPro.exe/WealthLab.exe not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else if (list_0.Contains(string_2))
                {
                    InterProcessCopyData.SendData("EM_CopyDataForm", string_3);
                }
                else
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo {
                        FileName = string_2,
                        Arguments = '"' + string_3 + '"'
                    };
                    Process.Start(startInfo);
                }
            }
        }
        else
        {
            remoting_0 = new Remoting(AppType.Server);
            string str = remoting_0.Initialize();
            if (str != null)
            {
                MessageBox.Show("Server initialization - " + str, "WealthLab.Extensions.Agent Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                Application.Run();
            }
        }
    }

    public static Remoting smethod_0()
    {
        return remoting_0;
    }

    private static void smethod_1()
    {
        string[] commandLineArgs = Environment.GetCommandLineArgs();
        if (commandLineArgs.Length >= 2)
        {
            string path = commandLineArgs[1];
            if (File.Exists(path) && (Path.GetExtension(path).ToLower() == ".wle"))
            {
                string_3 = path;
            }
        }
    }

    private static bool smethod_2()
    {
        string[] commandLineArgs = Environment.GetCommandLineArgs();
        if (commandLineArgs.Length < 2)
        {
            return false;
        }
        return (commandLineArgs[1] == "/rem");
    }

    private static bool smethod_3()
    {
        string[] commandLineArgs = Environment.GetCommandLineArgs();
        if (commandLineArgs.Length < 2)
        {
            return false;
        }
        return (commandLineArgs[1] == "/restart");
    }

    private static void smethod_4()
    {
        smethod_6();
        if (string_2 != null)
        {
            for (int i = 0; i < 60; i++)
            {
                Thread.Sleep(0x3e8);
                smethod_5();
                if (!list_0.Contains(string_2))
                {
                    Process.Start(string_2);
                    return;
                }
            }
        }
    }

    private static void smethod_5()
    {
        list_0.Clear();
        foreach (Process process in Process.GetProcesses())
        {
            try
            {
                list_0.Add(process.MainModule.FileName.ToUpper());
            }
            catch
            {
            }
        }
    }

    private static void smethod_6()
    {
        string path = Path.Combine(Application.StartupPath, string_1);
        string str2 = Path.Combine(Application.StartupPath, string_0);
        if (File.Exists(path))
        {
            string_2 = path;
        }
        if (File.Exists(str2))
        {
            string_2 = str2;
        }
        string_2 = string_2.ToUpper();
    }
}

