namespace log4net.Layout
{
    using log4net.Core;
    using System;
    using System.IO;

    public abstract class LayoutSkeleton : ILayout, IOptionHandler
    {
        private string m_footer = null;
        private string m_header = null;
        private bool m_ignoresException = true;

        protected LayoutSkeleton()
        {
        }

        public abstract void ActivateOptions();
        public abstract void Format(TextWriter writer, LoggingEvent loggingEvent);

        public virtual string ContentType
        {
            get
            {
                return "text/plain";
            }
        }

        public virtual string Footer
        {
            get
            {
                return this.m_footer;
            }
            set
            {
                this.m_footer = value;
            }
        }

        public virtual string Header
        {
            get
            {
                return this.m_header;
            }
            set
            {
                this.m_header = value;
            }
        }

        public virtual bool IgnoresException
        {
            get
            {
                return this.m_ignoresException;
            }
            set
            {
                this.m_ignoresException = value;
            }
        }
    }
}

