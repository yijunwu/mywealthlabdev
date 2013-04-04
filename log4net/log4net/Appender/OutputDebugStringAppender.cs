namespace log4net.Appender
{
    using log4net.Core;
    using System;
    using System.Runtime.InteropServices;

    public class OutputDebugStringAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            OutputDebugString(base.RenderLoggingEvent(loggingEvent));
        }

        [DllImport("Kernel32.dll")]
        protected static extern void OutputDebugString(string message);

        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }
    }
}

