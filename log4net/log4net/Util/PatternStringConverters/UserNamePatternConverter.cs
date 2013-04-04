namespace log4net.Util.PatternStringConverters
{
    using log4net.Util;
    using System;
    using System.IO;
    using System.Security;
    using System.Security.Principal;

    internal sealed class UserNamePatternConverter : PatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            try
            {
                WindowsIdentity current = null;
                current = WindowsIdentity.GetCurrent();
                if ((current != null) && (current.Name != null))
                {
                    writer.Write(current.Name);
                }
            }
            catch (SecurityException)
            {
                LogLog.Debug("UserNamePatternConverter: Security exception while trying to get current windows identity. Error Ignored.");
                writer.Write(SystemInfo.NotAvailableText);
            }
        }
    }
}

