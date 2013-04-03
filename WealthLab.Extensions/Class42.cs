using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using WealthLab.DataProviders.Helper;

internal static class Class42
{
    private static bool bool_0;
    private static DefaultTraceListener defaultTraceListener_0;
    private static readonly object object_0 = new object();
    private static TextWriter textWriter_0;

    static Class42()
    {
        foreach (string str in Environment.GetCommandLineArgs())
        {
            if (((str.ToLower() == "/log") || (str.ToLower() == "log")) || (str.ToLower() == "-log"))
            {
                bool_0 = true;
            }
        }
        if (bool_0)
        {
            FileStream stream = new FileStream(Assembly.GetExecutingAssembly().GetName().Name + ".Log.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            textWriter_0 = new StreamWriter(stream);
            defaultTraceListener_0 = new DefaultTraceListener();
            smethod_6();
        }
    }

    public static bool smethod_0()
    {
        return bool_0;
    }

    private static void smethod_1(WealthLab.DataProviders.Helper.TraceEventType traceEventType_0, string string_0)
    {
        string str;
        switch (traceEventType_0)
        {
            case WealthLab.DataProviders.Helper.TraceEventType.Info:
                str = "[I]";
                break;

            case WealthLab.DataProviders.Helper.TraceEventType.Method:
                str = "[M]";
                break;

            case WealthLab.DataProviders.Helper.TraceEventType.Error:
                str = "[E]";
                break;

            case WealthLab.DataProviders.Helper.TraceEventType.Warning:
                str = "[W]";
                break;

            case WealthLab.DataProviders.Helper.TraceEventType.Critical:
                str = "[C]";
                break;

            default:
                str = "[L]";
                break;
        }
        if ((traceEventType_0 == WealthLab.DataProviders.Helper.TraceEventType.Error) || (traceEventType_0 == WealthLab.DataProviders.Helper.TraceEventType.Critical))
        {
            string_0 = string.Format("{0}\r\n{1}", string_0, Environment.StackTrace);
        }
        string[] strArray2 = string_0.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        StringBuilder builder = new StringBuilder();
        if (strArray2.Length > 0)
        {
            builder.AppendLine(string.Format("\t{0}", strArray2[0]));
        }
        for (int i = 1; i < strArray2.Length; i++)
        {
            builder.AppendLine(string.Format("\t\t\t\t{0}", strArray2[i]));
        }
        string str2 = DateTime.Now.ToString("HH:mm:ss.fff");
        string str3 = string.Format("{0} {1} {2} {3}", new object[] { str2, str, Thread.CurrentThread.ManagedThreadId, builder.ToString() });
        textWriter_0.Write(str3);
        defaultTraceListener_0.Write(str3);
        textWriter_0.Flush();
        defaultTraceListener_0.Flush();
    }

    public static void smethod_2(object object_1)
    {
        if (bool_0)
        {
            lock (object_0)
            {
                smethod_1(WealthLab.DataProviders.Helper.TraceEventType.Log, object_1.ToString());
            }
        }
    }

    public static void smethod_3(params object[] object_1)
    {
        if (bool_0)
        {
            lock (object_0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (object obj2 in object_1)
                {
                    builder.Append(obj2.ToString());
                    builder.Append(" ");
                }
                smethod_1(WealthLab.DataProviders.Helper.TraceEventType.Log, builder.ToString());
            }
        }
    }

    public static void smethod_4(WealthLab.DataProviders.Helper.TraceEventType traceEventType_0, object object_1)
    {
        if (bool_0)
        {
            lock (object_0)
            {
                smethod_1(traceEventType_0, object_1.ToString());
            }
        }
    }

    public static void smethod_5(WealthLab.DataProviders.Helper.TraceEventType traceEventType_0, params object[] object_1)
    {
        if (bool_0)
        {
            lock (object_0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (object obj2 in object_1)
                {
                    builder.Append(obj2.ToString());
                    builder.Append(" ");
                }
                lock (object_0)
                {
                    smethod_1(traceEventType_0, builder.ToString());
                }
            }
        }
    }

    public static void smethod_6()
    {
        if (bool_0)
        {
            lock (object_0)
            {
                smethod_1(WealthLab.DataProviders.Helper.TraceEventType.Info, string.Format("Command Line: {0}", Environment.CommandLine));
                smethod_1(WealthLab.DataProviders.Helper.TraceEventType.Info, string.Format("Assembly: {0}", Assembly.GetExecutingAssembly().FullName));
                smethod_1(WealthLab.DataProviders.Helper.TraceEventType.Info, string.Format("OS: {0}", Environment.OSVersion));
                smethod_1(WealthLab.DataProviders.Helper.TraceEventType.Info, string.Format("NET: {0}", Environment.Version));
                smethod_1(WealthLab.DataProviders.Helper.TraceEventType.Info, string.Format("UTC: {0}", DateTime.UtcNow.ToString("o")));
                smethod_1(WealthLab.DataProviders.Helper.TraceEventType.Info, "------------------------------------------------");
            }
        }
    }

    public static void smethod_7(params object[] object_1)
    {
        if (bool_0)
        {
            lock (object_0)
            {
                MethodBase method = new StackTrace(0).GetFrame(1).GetMethod();
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("{0}.{1}", method.ReflectedType.FullName, method.Name);
                foreach (object obj2 in object_1)
                {
                    builder.AppendFormat("\r\n{0}", obj2.ToString());
                }
                smethod_1(WealthLab.DataProviders.Helper.TraceEventType.Method, builder.ToString());
            }
        }
    }

    public static void smethod_8(params object[] object_1)
    {
        if (bool_0)
        {
            lock (object_0)
            {
                MethodBase method = new StackTrace(0).GetFrame(1).GetMethod();
                StringBuilder builder = new StringBuilder();
                ParameterInfo[] parameters = method.GetParameters();
                builder.AppendFormat("{0}.{1}", method.ReflectedType.FullName, method.Name);
                for (int i = 0; i < object_1.Length; i++)
                {
                    bool flag = false;
                    if ((object_1[i] is string) && ((object_1[i] as string) == "-"))
                    {
                        flag = true;
                    }
                    if (!flag)
                    {
                        if (i < parameters.Length)
                        {
                            builder.AppendFormat("\r\n{0}  =  {1}", parameters[i].Name, object_1[i].ToString());
                        }
                        else
                        {
                            builder.AppendFormat("\r\n... {0}", object_1[i].ToString());
                        }
                    }
                }
                smethod_1(WealthLab.DataProviders.Helper.TraceEventType.Method, builder.ToString());
            }
        }
    }
}

