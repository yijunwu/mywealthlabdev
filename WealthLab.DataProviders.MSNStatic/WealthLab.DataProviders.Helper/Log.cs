using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
namespace WealthLab.DataProviders.Helper
{
	internal static class Log
	{
		private static bool _enable;
		private static DefaultTraceListener _listener;
		private static TextWriter _writer;
		private static readonly object _critSec;
		public static bool IsEnabled
		{
			get
			{
				return Log._enable;
			}
		}
		static Log()
		{
			Log._critSec = new object();
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			string[] array = commandLineArgs;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text.ToLower() == "/log" || text.ToLower() == "log" || text.ToLower() == "-log")
				{
					Log._enable = true;
				}
			}
			if (Log._enable)
			{
				string path = Assembly.GetExecutingAssembly().GetName().Name + ".Log.txt";
				FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
				Log._writer = new StreamWriter(stream);
				Log._listener = new DefaultTraceListener();
				Log.TraceEnvironmentInfo();
			}
		}
		private static void Write(Enum0 traceEvent, string message)
		{
			string text;
			switch (traceEvent)
			{
			case Enum0.Info:
				text = "[I]";
				break;
			case Enum0.Method:
				text = "[M]";
				break;
			case Enum0.Error:
				text = "[E]";
				break;
			case Enum0.Warning:
				text = "[W]";
				break;
			case Enum0.Critical:
				text = "[C]";
				break;
			default:
				text = "[L]";
				break;
			}
			if (traceEvent == Enum0.Error || traceEvent == Enum0.Critical)
			{
				message = string.Format("{0}\r\n{1}", message, Environment.StackTrace);
			}
			string[] array = message.Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.RemoveEmptyEntries);
			StringBuilder stringBuilder = new StringBuilder();
			if (array.Length > 0)
			{
				stringBuilder.AppendLine(string.Format("\t{0}", array[0]));
			}
			for (int i = 1; i < array.Length; i++)
			{
				stringBuilder.AppendLine(string.Format("\t\t\t\t{0}", array[i]));
			}
			string text2 = DateTime.Now.ToString("HH:mm:ss.fff");
			string text3 = string.Format("{0} {1} {2} {3}", new object[]
			{
				text2,
				text,
				Thread.CurrentThread.ManagedThreadId,
				stringBuilder.ToString()
			});
			Log._writer.Write(text3);
			Log._listener.Write(text3);
			Log._writer.Flush();
			Log._listener.Flush();
		}
		public static void Trace(object value)
		{
			if (!Log._enable)
			{
				return;
			}
			lock (Log._critSec)
			{
				Log.Write(Enum0.Log, value.ToString());
			}
		}
		public static void Trace(params object[] args)
		{
			if (!Log._enable)
			{
				return;
			}
			lock (Log._critSec)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < args.Length; i++)
				{
					object obj = args[i];
					stringBuilder.Append(obj.ToString());
					stringBuilder.Append(" ");
				}
				Log.Write(Enum0.Log, stringBuilder.ToString());
			}
		}
		public static void TraceEvent(Enum0 traceEvent, object value)
		{
			if (!Log._enable)
			{
				return;
			}
			lock (Log._critSec)
			{
				Log.Write(traceEvent, value.ToString());
			}
		}
		public static void TraceEvent(Enum0 traceEvent, params object[] args)
		{
			if (!Log._enable)
			{
				return;
			}
			lock (Log._critSec)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < args.Length; i++)
				{
					object obj = args[i];
					stringBuilder.Append(obj.ToString());
					stringBuilder.Append(" ");
				}
				lock (Log._critSec)
				{
					Log.Write(traceEvent, stringBuilder.ToString());
				}
			}
		}
		public static void TraceEnvironmentInfo()
		{
			if (!Log._enable)
			{
				return;
			}
			lock (Log._critSec)
			{
				Log.Write(Enum0.Info, string.Format("Command Line: {0}", Environment.CommandLine));
				Log.Write(Enum0.Info, string.Format("Assembly: {0}", Assembly.GetExecutingAssembly().FullName));
				Log.Write(Enum0.Info, string.Format("OS: {0}", Environment.OSVersion));
				Log.Write(Enum0.Info, string.Format("NET: {0}", Environment.Version));
				Log.Write(Enum0.Info, string.Format("UTC: {0}", DateTime.UtcNow.ToString("o")));
				Log.Write(Enum0.Info, "------------------------------------------------");
			}
		}
		public static void TraceMethodInfo(params object[] args)
		{
			if (!Log._enable)
			{
				return;
			}
			lock (Log._critSec)
			{
				MethodBase method = new StackTrace(0).GetFrame(1).GetMethod();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("{0}.{1}", method.ReflectedType.FullName, method.Name);
				for (int i = 0; i < args.Length; i++)
				{
					object obj = args[i];
					stringBuilder.AppendFormat("\r\n{0}", obj.ToString());
				}
				Log.Write(Enum0.Method, stringBuilder.ToString());
			}
		}
		public static void TraceMethod(params object[] args)
		{
			if (!Log._enable)
			{
				return;
			}
			lock (Log._critSec)
			{
				MethodBase method = new StackTrace(0).GetFrame(1).GetMethod();
				StringBuilder stringBuilder = new StringBuilder();
				ParameterInfo[] parameters = method.GetParameters();
				stringBuilder.AppendFormat("{0}.{1}", method.ReflectedType.FullName, method.Name);
				for (int i = 0; i < args.Length; i++)
				{
					bool flag2 = false;
					if (args[i] is string && args[i] as string == "-")
					{
						flag2 = true;
					}
					if (!flag2)
					{
						if (i < parameters.Length)
						{
							stringBuilder.AppendFormat("\r\n{0}  =  {1}", parameters[i].Name, args[i].ToString());
						}
						else
						{
							stringBuilder.AppendFormat("\r\n... {0}", args[i].ToString());
						}
					}
				}
				Log.Write(Enum0.Method, stringBuilder.ToString());
			}
		}
	}
}
