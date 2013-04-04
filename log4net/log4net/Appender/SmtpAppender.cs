namespace log4net.Appender
{
    using log4net.Core;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Mail;

    public class SmtpAppender : BufferingAppenderSkeleton
    {
        private SmtpAuthentication m_authentication = SmtpAuthentication.None;
        private string m_from;
        private MailPriority m_mailPriority = MailPriority.Normal;
        private string m_password;
        private int m_port = 0x19;
        private string m_smtpHost;
        private string m_subject;
        private string m_to;
        private string m_username;

        protected override void SendBuffer(LoggingEvent[] events)
        {
            try
            {
                StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
                string header = this.Layout.Header;
                if (header != null)
                {
                    writer.Write(header);
                }
                for (int i = 0; i < events.Length; i++)
                {
                    base.RenderLoggingEvent(writer, events[i]);
                }
                header = this.Layout.Footer;
                if (header != null)
                {
                    writer.Write(header);
                }
                this.SendEmail(writer.ToString());
            }
            catch (Exception exception)
            {
                this.ErrorHandler.Error("Error occurred while sending e-mail notification.", exception);
            }
        }

        protected virtual void SendEmail(string messageBody)
        {
            SmtpClient client = new SmtpClient();
            if ((this.m_smtpHost != null) && (this.m_smtpHost.Length > 0))
            {
                client.Host = this.m_smtpHost;
            }
            client.Port = this.m_port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            if (this.m_authentication == SmtpAuthentication.Basic)
            {
                client.Credentials = new NetworkCredential(this.m_username, this.m_password);
            }
            else if (this.m_authentication == SmtpAuthentication.Ntlm)
            {
                client.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            MailMessage message = new MailMessage {
                Body = messageBody,
                From = new MailAddress(this.m_from)
            };
            message.To.Add(this.m_to);
            message.Subject = this.m_subject;
            message.Priority = this.m_mailPriority;
            client.Send(message);
        }

        public SmtpAuthentication Authentication
        {
            get
            {
                return this.m_authentication;
            }
            set
            {
                this.m_authentication = value;
            }
        }

        public string From
        {
            get
            {
                return this.m_from;
            }
            set
            {
                this.m_from = value;
            }
        }

        [Obsolete("Use the BufferingAppenderSkeleton Fix methods")]
        public bool LocationInfo
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public string Password
        {
            get
            {
                return this.m_password;
            }
            set
            {
                this.m_password = value;
            }
        }

        public int Port
        {
            get
            {
                return this.m_port;
            }
            set
            {
                this.m_port = value;
            }
        }

        public MailPriority Priority
        {
            get
            {
                return this.m_mailPriority;
            }
            set
            {
                this.m_mailPriority = value;
            }
        }

        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }

        public string SmtpHost
        {
            get
            {
                return this.m_smtpHost;
            }
            set
            {
                this.m_smtpHost = value;
            }
        }

        public string Subject
        {
            get
            {
                return this.m_subject;
            }
            set
            {
                this.m_subject = value;
            }
        }

        public string To
        {
            get
            {
                return this.m_to;
            }
            set
            {
                this.m_to = value;
            }
        }

        public string Username
        {
            get
            {
                return this.m_username;
            }
            set
            {
                this.m_username = value;
            }
        }

        public enum SmtpAuthentication
        {
            None,
            Basic,
            Ntlm
        }
    }
}

