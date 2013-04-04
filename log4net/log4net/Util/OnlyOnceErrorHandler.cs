namespace log4net.Util
{
    using log4net.Core;
    using System;

    public class OnlyOnceErrorHandler : IErrorHandler
    {
        private bool m_firstTime;
        private readonly string m_prefix;

        public OnlyOnceErrorHandler()
        {
            this.m_firstTime = true;
            this.m_prefix = "";
        }

        public OnlyOnceErrorHandler(string prefix)
        {
            this.m_firstTime = true;
            this.m_prefix = prefix;
        }

        public void Error(string message)
        {
            if (this.IsEnabled)
            {
                LogLog.Error("[" + this.m_prefix + "] " + message);
            }
        }

        public void Error(string message, Exception e)
        {
            if (this.IsEnabled)
            {
                LogLog.Error("[" + this.m_prefix + "] " + message, e);
            }
        }

        public void Error(string message, Exception e, ErrorCode errorCode)
        {
            if (this.IsEnabled)
            {
                LogLog.Error("[" + this.m_prefix + "] " + message, e);
            }
        }

        private bool IsEnabled
        {
            get
            {
                if (this.m_firstTime)
                {
                    this.m_firstTime = false;
                    return true;
                }
                return !(!LogLog.InternalDebugging || LogLog.QuietMode);
            }
        }
    }
}

