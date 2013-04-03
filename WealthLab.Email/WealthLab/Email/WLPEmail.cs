namespace WealthLab.Email
{
    using log4net;
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Net.Mail;

    public class WLPEmail
    {
        private string address;
        private string credPass;
        private string credUser;
        private string host;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string messageText;
        private int port;
        private bool ssl;
        private string subject;

        public WLPEmail(string Host, int Port, bool SSL, string CredUser, string CredPass, string Address, string Subject, string Message)
        {
            this.host = Host;
            this.port = Port;
            this.ssl = SSL;
            this.credUser = CredUser;
            this.credPass = CredPass;
            this.address = Address;
            this.subject = Subject;
            this.messageText = Message;
        }

        private string formatEmailAddresses(string value)
        {
            value = value.Replace("\r\n", ";");
            value = value.Replace(";", ",");
            value = value.TrimEnd(new char[] { ',' });
            value = value.Replace(" ", "");
            return value;
        }

        public void Notify(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.UserState is WLPEmail)
                {
                    WLPEmail userState = (WLPEmail) e.UserState;
                    if (e.Error == null)
                    {
                        logger.Info("Sent Mail" + ((userState != null) ? (" - Message text: " + userState.messageText + " to " + userState.address) : ""));
                    }
                    else
                    {
                        logger.Error("Unable to send Mail" + ((userState != null) ? (" - Message text: " + userState.messageText + " to " + userState.address) : ""));
                        if (e.Error.InnerException != null)
                        {
                            logger.Error(e.Error.InnerException.Message);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                logger.Error("Error occured while notifying the email alert" + exception.Message);
            }
        }

        public void Send(bool synchroneous, string product)
        {
            SmtpClient client = new SmtpClient();
            MailMessage message = new MailMessage();
            try
            {
                client = new SmtpClient(this.host, this.port) {
                    EnableSsl = this.ssl,
                    Credentials = new NetworkCredential(this.credUser, this.credPass)
                };
                string[] strArray = this.host.Split(new char[] { '.' });
                string credUser = string.Empty;
                if (!this.credUser.Contains("@"))
                {
                    if (strArray.Length > 1)
                    {
                        credUser = this.credUser + "@" + strArray[strArray.Length - 2] + "." + strArray[strArray.Length - 1];
                    }
                    else
                    {
                        credUser = this.credUser + "@" + this.host;
                    }
                }
                else
                {
                    credUser = this.credUser;
                }
                string displayName = string.Empty;
                if (product == "WealthLabPro")
                {
                    displayName = "Wealth-Lab Pro\x00ae";
                }
                else
                {
                    displayName = "Wealth-Lab Dev\x00ae";
                }
                MailAddress address = new MailAddress(credUser, displayName);
                message.From = address;
                message.To.Add(this.formatEmailAddresses(this.address));
                message.Subject = this.subject;
                message.Body = this.messageText;
            }
            catch
            {
            }
            if (!synchroneous)
            {
                try
                {
                    client.SendCompleted += new SendCompletedEventHandler(this.Notify);
                    client.SendAsync(message, this);
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    client.Send(message);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }
    }
}

