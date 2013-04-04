namespace log4net.Util
{
    using System;
    using System.Diagnostics;

    public sealed class LogLog
    {
        private const string ERR_PREFIX = "log4net:ERROR ";
        private const string PREFIX = "log4net: ";
        private static bool s_debugEnabled = false;
        private static bool s_quietMode = false;
        private const string WARN_PREFIX = "log4net:WARN ";

        static LogLog()
        {
            try
            {
                InternalDebugging = OptionConverter.ToBoolean(SystemInfo.GetAppSetting("log4net.Internal.Debug"), false);
                QuietMode = OptionConverter.ToBoolean(SystemInfo.GetAppSetting("log4net.Internal.Quiet"), false);
            }
            catch (Exception exception)
            {
                Error("LogLog: Exception while reading ConfigurationSettings. Check your .config file is well formed XML.", exception);
            }
        }

        private LogLog()
        {
        }

        public static void Debug(string message)
        {
            if (IsDebugEnabled)
            {
                EmitOutLine("log4net: " + message);
            }
        }

        public static void Debug(string message, Exception exception)
        {
            if (IsDebugEnabled)
            {
                EmitOutLine("log4net: " + message);
                if (exception != null)
                {
                    EmitOutLine(exception.ToString());
                }
            }
        }

        private static void EmitErrorLine(string message)
        {
            try
            {
                Console.Error.WriteLine(message);
                Trace.WriteLine(message);
            }
            catch
            {
            }
        }

        private static void EmitOutLine(string message)
        {
            try
            {
                Console.Out.WriteLine(message);
                Trace.WriteLine(message);
            }
            catch
            {
            }
        }

        public static void Error(string message)
        {
            if (IsErrorEnabled)
            {
                EmitErrorLine("log4net:ERROR " + message);
            }
        }

        public static void Error(string message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                EmitErrorLine("log4net:ERROR " + message);
                if (exception != null)
                {
                    EmitErrorLine(exception.ToString());
                }
            }
        }

        public static void Warn(string message)
        {
            if (IsWarnEnabled)
            {
                EmitErrorLine("log4net:WARN " + message);
            }
        }

        public static void Warn(string message, Exception exception)
        {
            if (IsWarnEnabled)
            {
                EmitErrorLine("log4net:WARN " + message);
                if (exception != null)
                {
                    EmitErrorLine(exception.ToString());
                }
            }
        }

        public static bool InternalDebugging
        {
            get
            {
                return s_debugEnabled;
            }
            set
            {
                s_debugEnabled = value;
            }
        }

        public static bool IsDebugEnabled
        {
            get
            {
                return (s_debugEnabled && !s_quietMode);
            }
        }

        public static bool IsErrorEnabled
        {
            get
            {
                return !s_quietMode;
            }
        }

        public static bool IsWarnEnabled
        {
            get
            {
                return !s_quietMode;
            }
        }

        public static bool QuietMode
        {
            get
            {
                return s_quietMode;
            }
            set
            {
                s_quietMode = value;
            }
        }
    }
}

