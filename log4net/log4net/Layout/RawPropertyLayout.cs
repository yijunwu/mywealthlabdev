namespace log4net.Layout
{
    using log4net.Core;
    using System;

    public class RawPropertyLayout : IRawLayout
    {
        private string m_key;

        public virtual object Format(LoggingEvent loggingEvent)
        {
            return loggingEvent.LookupProperty(this.m_key);
        }

        public string Key
        {
            get
            {
                return this.m_key;
            }
            set
            {
                this.m_key = value;
            }
        }
    }
}

