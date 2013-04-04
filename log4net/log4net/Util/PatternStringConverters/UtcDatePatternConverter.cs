namespace log4net.Util.PatternStringConverters
{
    using log4net.Util;
    using System;
    using System.IO;

    internal class UtcDatePatternConverter : DatePatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            try
            {
                base.m_dateFormatter.FormatDate(DateTime.UtcNow, writer);
            }
            catch (Exception exception)
            {
                LogLog.Error("UtcDatePatternConverter: Error occurred while converting date.", exception);
            }
        }
    }
}

