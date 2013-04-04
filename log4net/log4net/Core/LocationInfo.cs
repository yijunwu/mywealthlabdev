namespace log4net.Core
{
    using log4net.Util;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Security;

    [Serializable]
    public class LocationInfo
    {
        private readonly string m_className;
        private readonly string m_fileName;
        private readonly string m_fullInfo;
        private readonly string m_lineNumber;
        private readonly string m_methodName;
        private const string NA = "?";

        public LocationInfo(Type callerStackBoundaryDeclaringType)
        {
            this.m_className = "?";
            this.m_fileName = "?";
            this.m_lineNumber = "?";
            this.m_methodName = "?";
            this.m_fullInfo = "?";
            if (callerStackBoundaryDeclaringType != null)
            {
                try
                {
                    System.Diagnostics.StackFrame frame;
                    StackTrace trace = new StackTrace(true);
                    int index = 0;
                    while (index < trace.FrameCount)
                    {
                        frame = trace.GetFrame(index);
                        if ((frame != null) && (frame.GetMethod().DeclaringType == callerStackBoundaryDeclaringType))
                        {
                            break;
                        }
                        index++;
                    }
                    while (index < trace.FrameCount)
                    {
                        frame = trace.GetFrame(index);
                        if ((frame != null) && (frame.GetMethod().DeclaringType != callerStackBoundaryDeclaringType))
                        {
                            break;
                        }
                        index++;
                    }
                    if (index < trace.FrameCount)
                    {
                        System.Diagnostics.StackFrame frame2 = trace.GetFrame(index);
                        if (frame2 != null)
                        {
                            MethodBase method = frame2.GetMethod();
                            if (method != null)
                            {
                                this.m_methodName = method.Name;
                                if (method.DeclaringType != null)
                                {
                                    this.m_className = method.DeclaringType.FullName;
                                }
                            }
                            this.m_fileName = frame2.GetFileName();
                            this.m_lineNumber = frame2.GetFileLineNumber().ToString(NumberFormatInfo.InvariantInfo);
                            this.m_fullInfo = string.Concat(new object[] { this.m_className, '.', this.m_methodName, '(', this.m_fileName, ':', this.m_lineNumber, ')' });
                        }
                    }
                }
                catch (SecurityException)
                {
                    LogLog.Debug("LocationInfo: Security exception while trying to get caller stack frame. Error Ignored. Location Information Not Available.");
                }
            }
        }

        public LocationInfo(string className, string methodName, string fileName, string lineNumber)
        {
            this.m_className = className;
            this.m_fileName = fileName;
            this.m_lineNumber = lineNumber;
            this.m_methodName = methodName;
            this.m_fullInfo = string.Concat(new object[] { this.m_className, '.', this.m_methodName, '(', this.m_fileName, ':', this.m_lineNumber, ')' });
        }

        public string ClassName
        {
            get
            {
                return this.m_className;
            }
        }

        public string FileName
        {
            get
            {
                return this.m_fileName;
            }
        }

        public string FullInfo
        {
            get
            {
                return this.m_fullInfo;
            }
        }

        public string LineNumber
        {
            get
            {
                return this.m_lineNumber;
            }
        }

        public string MethodName
        {
            get
            {
                return this.m_methodName;
            }
        }
    }
}

