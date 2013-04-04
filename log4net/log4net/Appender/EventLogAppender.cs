namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using log4net.Util;
    using System;
    using System.Diagnostics;
    using System.Threading;

    public class EventLogAppender : AppenderSkeleton
    {
        private string m_applicationName;
        private LevelMapping m_levelMapping;
        private string m_logName;
        private string m_machineName;
        private log4net.Core.SecurityContext m_securityContext;

        public EventLogAppender()
        {
            this.m_levelMapping = new LevelMapping();
            this.m_applicationName = Thread.GetDomain().FriendlyName;
            this.m_logName = "Application";
            this.m_machineName = ".";
        }

        [Obsolete("Instead use the default constructor and set the Layout property")]
        public EventLogAppender(ILayout layout) : this()
        {
            this.Layout = layout;
        }

        public override void ActivateOptions()
        {
            IDisposable disposable;
            base.ActivateOptions();
            if (this.m_securityContext == null)
            {
                this.m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
            }
            bool flag = false;
            string str = null;
            using (disposable = this.SecurityContext.Impersonate(this))
            {
                flag = EventLog.SourceExists(this.m_applicationName);
                if (flag)
                {
                    str = EventLog.LogNameFromSourceName(this.m_applicationName, this.m_machineName);
                }
            }
            if (flag && (str != this.m_logName))
            {
                LogLog.Debug("EventLogAppender: Changing event source [" + this.m_applicationName + "] from log [" + str + "] to log [" + this.m_logName + "]");
            }
            else if (!flag)
            {
                LogLog.Debug("EventLogAppender: Creating event source Source [" + this.m_applicationName + "] in log " + this.m_logName + "]");
            }
            string str2 = null;
            using (disposable = this.SecurityContext.Impersonate(this))
            {
                if (flag && (str != this.m_logName))
                {
                    EventLog.DeleteEventSource(this.m_applicationName, this.m_machineName);
                    CreateEventSource(this.m_applicationName, this.m_logName, this.m_machineName);
                    str2 = EventLog.LogNameFromSourceName(this.m_applicationName, this.m_machineName);
                }
                else if (!flag)
                {
                    CreateEventSource(this.m_applicationName, this.m_logName, this.m_machineName);
                    str2 = EventLog.LogNameFromSourceName(this.m_applicationName, this.m_machineName);
                }
            }
            this.m_levelMapping.ActivateOptions();
            LogLog.Debug("EventLogAppender: Source [" + this.m_applicationName + "] is registered to log [" + str2 + "]");
        }

        public void AddMapping(Level2EventLogEntryType mapping)
        {
            this.m_levelMapping.Add(mapping);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            int eventID = 0;
            object property = loggingEvent.LookupProperty("EventID");
            if (property != null)
            {
                if (property is int)
                {
                    eventID = (int) property;
                }
                else
                {
                    string s = property as string;
                    if ((s != null) && (s.Length > 0))
                    {
                        int num2;
                        if (SystemInfo.TryParse(s, out num2))
                        {
                            eventID = num2;
                        }
                        else
                        {
                            this.ErrorHandler.Error("Unable to parse event ID property [" + s + "].");
                        }
                    }
                }
            }
            try
            {
                string message = base.RenderLoggingEvent(loggingEvent);
                if (message.Length > 0x7d00)
                {
                    message = message.Substring(0, 0x7d00);
                }
                EventLogEntryType entryType = this.GetEntryType(loggingEvent.Level);
                using (this.SecurityContext.Impersonate(this))
                {
                    EventLog.WriteEntry(this.m_applicationName, message, entryType, eventID);
                }
            }
            catch (Exception exception)
            {
                this.ErrorHandler.Error("Unable to write to event log [" + this.m_logName + "] using source [" + this.m_applicationName + "]", exception);
            }
        }

        private static void CreateEventSource(string source, string logName, string machineName)
        {
            EventSourceCreationData sourceData = new EventSourceCreationData(source, logName) {
                MachineName = machineName
            };
            EventLog.CreateEventSource(sourceData);
        }

        protected virtual EventLogEntryType GetEntryType(Level level)
        {
            Level2EventLogEntryType type = this.m_levelMapping.Lookup(level) as Level2EventLogEntryType;
            if (type != null)
            {
                return type.EventLogEntryType;
            }
            if (level >= Level.Error)
            {
                return EventLogEntryType.Error;
            }
            if (level == Level.Warn)
            {
                return EventLogEntryType.Warning;
            }
            return EventLogEntryType.Information;
        }

        public string ApplicationName
        {
            get
            {
                return this.m_applicationName;
            }
            set
            {
                this.m_applicationName = value;
            }
        }

        public string LogName
        {
            get
            {
                return this.m_logName;
            }
            set
            {
                this.m_logName = value;
            }
        }

        public string MachineName
        {
            get
            {
                return this.m_machineName;
            }
            set
            {
            }
        }

        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }

        public log4net.Core.SecurityContext SecurityContext
        {
            get
            {
                return this.m_securityContext;
            }
            set
            {
                this.m_securityContext = value;
            }
        }

        public class Level2EventLogEntryType : LevelMappingEntry
        {
            private System.Diagnostics.EventLogEntryType m_entryType;

            public System.Diagnostics.EventLogEntryType EventLogEntryType
            {
                get
                {
                    return this.m_entryType;
                }
                set
                {
                    this.m_entryType = value;
                }
            }
        }
    }
}

