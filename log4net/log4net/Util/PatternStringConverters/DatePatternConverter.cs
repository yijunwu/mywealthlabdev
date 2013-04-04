namespace log4net.Util.PatternStringConverters
{
    using log4net.Core;
    using log4net.DateFormatter;
    using log4net.Util;
    using System;
    using System.Globalization;
    using System.IO;

    internal class DatePatternConverter : PatternConverter, IOptionHandler
    {
        protected IDateFormatter m_dateFormatter;

        public void ActivateOptions()
        {
            string option = this.Option;
            if (option == null)
            {
                option = "ISO8601";
            }
            if (string.Compare(option, "ISO8601", true, CultureInfo.InvariantCulture) == 0)
            {
                this.m_dateFormatter = new Iso8601DateFormatter();
            }
            else if (string.Compare(option, "ABSOLUTE", true, CultureInfo.InvariantCulture) == 0)
            {
                this.m_dateFormatter = new AbsoluteTimeDateFormatter();
            }
            else if (string.Compare(option, "DATE", true, CultureInfo.InvariantCulture) == 0)
            {
                this.m_dateFormatter = new DateTimeDateFormatter();
            }
            else
            {
                try
                {
                    this.m_dateFormatter = new SimpleDateFormatter(option);
                }
                catch (Exception exception)
                {
                    LogLog.Error("DatePatternConverter: Could not instantiate SimpleDateFormatter with [" + option + "]", exception);
                    this.m_dateFormatter = new Iso8601DateFormatter();
                }
            }
        }

        protected override void Convert(TextWriter writer, object state)
        {
            try
            {
                this.m_dateFormatter.FormatDate(DateTime.Now, writer);
            }
            catch (Exception exception)
            {
                LogLog.Error("DatePatternConverter: Error occurred while converting date.", exception);
            }
        }
    }
}

