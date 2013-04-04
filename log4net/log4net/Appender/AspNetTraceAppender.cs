namespace log4net.Appender
{
    using log4net.Core;
    using System;
    using System.Web;

    public class AspNetTraceAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            if ((HttpContext.Current != null) && HttpContext.Current.Trace.IsEnabled)
            {
                if (loggingEvent.Level >= Level.Warn)
                {
                    HttpContext.Current.Trace.Warn(loggingEvent.LoggerName, base.RenderLoggingEvent(loggingEvent));
                }
                else
                {
                    HttpContext.Current.Trace.Write(loggingEvent.LoggerName, base.RenderLoggingEvent(loggingEvent));
                }
            }
        }

        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }
    }
}

