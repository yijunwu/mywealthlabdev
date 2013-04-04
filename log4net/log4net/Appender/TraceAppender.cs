namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using System;
    using System.Diagnostics;

    public class TraceAppender : AppenderSkeleton
    {
        private bool m_immediateFlush;

        public TraceAppender()
        {
            this.m_immediateFlush = true;
        }

        [Obsolete("Instead use the default constructor and set the Layout property")]
        public TraceAppender(ILayout layout)
        {
            this.m_immediateFlush = true;
            this.Layout = layout;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            Trace.Write(base.RenderLoggingEvent(loggingEvent), loggingEvent.LoggerName);
            if (this.m_immediateFlush)
            {
                Trace.Flush();
            }
        }

        public bool ImmediateFlush
        {
            get
            {
                return this.m_immediateFlush;
            }
            set
            {
                this.m_immediateFlush = value;
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

