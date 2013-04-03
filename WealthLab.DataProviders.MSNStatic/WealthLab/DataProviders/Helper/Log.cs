namespace WealthLab.DataProviders.Helper
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading;

    internal static class Log
    {
        private static readonly object _critSec = new object();
        private static bool _enable;
        private static DefaultTraceListener _listener;
        private static TextWriter _writer;

        static Log()
        {
            foreach (string str in Environment.GetCommandLineArgs())
            {
                if (((str.ToLower() == "/log") || (str.ToLower() == "log")) || (str.ToLower() == "-log"))
                {
                    _enable = true;
                }
            }
            if (_enable)
            {
                FileStream stream = new FileStream(Assembly.GetExecutingAssembly().GetName().Name + ".Log.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                _writer = new StreamWriter(stream);
                _listener = new DefaultTraceListener();
                TraceEnvironmentInfo();
            }
        }

        public static void Trace(object value)
        {
            if (_enable)
            {
                lock (_critSec)
                {
                    Write(䰾菏歓률꫸풳णᕐ.Log, value.ToString());
                }
            }
        }

        public static void Trace(params object[] args)
        {
            if (_enable)
            {
                lock (_critSec)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (object obj2 in args)
                    {
                        builder.Append(obj2.ToString());
                        builder.Append(" ");
                    }
                    Write(䰾菏歓률꫸풳णᕐ.Log, builder.ToString());
                }
            }
        }

        public static void TraceEnvironmentInfo()
        {
            if (_enable)
            {
                lock (_critSec)
                {
                    Write(䰾菏歓률꫸풳णᕐ.Info, string.Format("Command Line: {0}", Environment.CommandLine));
                    Write(䰾菏歓률꫸풳णᕐ.Info, string.Format("Assembly: {0}", Assembly.GetExecutingAssembly().FullName));
                    Write(䰾菏歓률꫸풳णᕐ.Info, string.Format("OS: {0}", Environment.OSVersion));
                    Write(䰾菏歓률꫸풳णᕐ.Info, string.Format("NET: {0}", Environment.Version));
                    Write(䰾菏歓률꫸풳णᕐ.Info, string.Format("UTC: {0}", DateTime.UtcNow.ToString("o")));
                    Write(䰾菏歓률꫸풳णᕐ.Info, "------------------------------------------------");
                }
            }
        }

        public static void TraceEvent(䰾菏歓률꫸풳णᕐ traceEvent, object value)
        {
            if (_enable)
            {
                lock (_critSec)
                {
                    Write(traceEvent, value.ToString());
                }
            }
        }

        public static void TraceEvent(䰾菏歓률꫸풳णᕐ traceEvent, params object[] args)
        {
            if (_enable)
            {
                lock (_critSec)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (object obj2 in args)
                    {
                        builder.Append(obj2.ToString());
                        builder.Append(" ");
                    }
                    lock (_critSec)
                    {
                        Write(traceEvent, builder.ToString());
                    }
                }
            }
        }

        public static void TraceMethod(params object[] args)
        {
            if (_enable)
            {
                lock (_critSec)
                {
                    MethodBase method = new StackTrace(0).GetFrame(1).GetMethod();
                    StringBuilder builder = new StringBuilder();
                    ParameterInfo[] parameters = method.GetParameters();
                    builder.AppendFormat("{0}.{1}", method.ReflectedType.FullName, method.Name);
                    for (int i = 0; i < args.Length; i++)
                    {
                        bool flag = false;
                        if ((args[i] is string) && ((args[i] as string) == "-"))
                        {
                            flag = true;
                        }
                        if (!flag)
                        {
                            if (i < parameters.Length)
                            {
                                builder.AppendFormat("\r\n{0}  =  {1}", parameters[i].Name, args[i].ToString());
                            }
                            else
                            {
                                builder.AppendFormat("\r\n... {0}", args[i].ToString());
                            }
                        }
                    }
                    Write(䰾菏歓률꫸풳णᕐ.Method, builder.ToString());
                }
            }
        }

        public static void TraceMethodInfo(params object[] args)
        {
            if (_enable)
            {
                lock (_critSec)
                {
                    MethodBase method = new StackTrace(0).GetFrame(1).GetMethod();
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat("{0}.{1}", method.ReflectedType.FullName, method.Name);
                    foreach (object obj2 in args)
                    {
                        builder.AppendFormat("\r\n{0}", obj2.ToString());
                    }
                    Write(䰾菏歓률꫸풳णᕐ.Method, builder.ToString());
                }
            }
        }

        private static void Write(䰾菏歓률꫸풳णᕐ traceEvent, string message)
        {
            string str;
            switch (traceEvent)
            {
                case 䰾菏歓률꫸풳णᕐ.Info:
                    str = "[I]";
                    break;

                case 䰾菏歓률꫸풳णᕐ.Method:
                    str = "[M]";
                    break;

                case 䰾菏歓률꫸풳णᕐ.Error:
                    str = "[E]";
                    break;

                case 䰾菏歓률꫸풳णᕐ.Warning:
                    str = "[W]";
                    break;

                case 䰾菏歓률꫸풳णᕐ.Critical:
                    str = "[C]";
                    break;

                default:
                    str = "[L]";
                    break;
            }
            if ((traceEvent == 䰾菏歓률꫸풳णᕐ.Error) || (traceEvent == 䰾菏歓률꫸풳णᕐ.Critical))
            {
                message = string.Format("{0}\r\n{1}", message, Environment.StackTrace);
            }
            string[] strArray = message.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder builder = new StringBuilder();
            if (strArray.Length > 0)
            {
                builder.AppendLine(string.Format("\t{0}", strArray[0]));
            }
            for (int i = 1; i < strArray.Length; i++)
            {
                builder.AppendLine(string.Format("\t\t\t\t{0}", strArray[i]));
            }
            string str2 = DateTime.Now.ToString("HH:mm:ss.fff");
            string str3 = string.Format("{0} {1} {2} {3}", new object[] { str2, str, Thread.CurrentThread.ManagedThreadId, builder.ToString() });
            _writer.Write(str3);
            _listener.Write(str3);
            _writer.Flush();
            _listener.Flush();
        }

        public static bool IsEnabled
        {
            get
            {
                return _enable;
            }
        }
    }
}

