namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;

    internal abstract class NamedPatternConverter : PatternLayoutConverter, IOptionHandler
    {
        protected int m_precision = 0;

        protected NamedPatternConverter()
        {
        }

        public void ActivateOptions()
        {
            this.m_precision = 0;
            if (this.Option != null)
            {
                string s = this.Option.Trim();
                if (s.Length > 0)
                {
                    int num;
                    if (SystemInfo.TryParse(s, out num))
                    {
                        if (num <= 0)
                        {
                            LogLog.Error("NamedPatternConverter: Precision option (" + s + ") isn't a positive integer.");
                        }
                        else
                        {
                            this.m_precision = num;
                        }
                    }
                    else
                    {
                        LogLog.Error("NamedPatternConverter: Precision option \"" + s + "\" not a decimal integer.");
                    }
                }
            }
        }

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            string fullyQualifiedName = this.GetFullyQualifiedName(loggingEvent);
            if (this.m_precision <= 0)
            {
                writer.Write(fullyQualifiedName);
            }
            else
            {
                int length = fullyQualifiedName.Length;
                int num2 = length - 1;
                for (int i = this.m_precision; i > 0; i--)
                {
                    num2 = fullyQualifiedName.LastIndexOf('.', num2 - 1);
                    if (num2 == -1)
                    {
                        writer.Write(fullyQualifiedName);
                        return;
                    }
                }
                writer.Write(fullyQualifiedName.Substring(num2 + 1, (length - num2) - 1));
            }
        }

        protected abstract string GetFullyQualifiedName(LoggingEvent loggingEvent);
    }
}

