namespace WealthLab
{
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net;
    using System.Net.Mail;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class WLPEmail
    {
        private bool bool_0;
        private static readonly ILog ilog_0 = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int int_0;
        private static Stack<WLPEmail> stack_0 = new Stack<WLPEmail>();
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;
        private string string_4;
        private string string_5;
        [CompilerGenerated]
        private string string_6;
        private static Thread thread_0 = new Thread(new ThreadStart(WLPEmail.smethod_0));

        static WLPEmail()
        {
            thread_0.IsBackground = true;
            thread_0.Start();
        }

        public WLPEmail(string Host, int Port, bool SSL, string CredUser, string CredPass, string Address, string Subject, string Message)
        {
            this.string_0 = Host;
            this.int_0 = Port;
            this.bool_0 = SSL;
            this.string_1 = CredUser;
            this.string_2 = CredPass;
            this.string_3 = Address;
            this.string_4 = Subject;
            this.string_5 = Message;
        }

        public void Enqueue(string product)
        {
            this.Product = product;
            lock (stack_0)
            {
                stack_0.Push(this);
            }
        }

        private void method_0(bool bool_1)
        {
            SmtpClient client = new SmtpClient();
            MailMessage message = new MailMessage();
            try
            {
                client = new SmtpClient(this.string_0, this.int_0) {
                    EnableSsl = this.bool_0,
                    Credentials = new NetworkCredential(this.string_1, this.string_2)
                };
                string[] strArray = this.string_0.Split(new char[] { '.' });
                string str = string.Empty;
                if (!this.string_1.Contains("@"))
                {
                    if (strArray.Length > 1)
                    {
                        str = this.string_1 + "@" + strArray[strArray.Length - 2] + "." + strArray[strArray.Length - 1];
                    }
                    else
                    {
                        str = this.string_1 + "@" + this.string_0;
                    }
                }
                else
                {
                    str = this.string_1;
                }
                string displayName = string.Empty;
                if (this.Product == "WealthLabPro")
                {
                    displayName = "Wealth-Lab Pro\x00ae";
                }
                else
                {
                    displayName = "Wealth-Lab Dev\x00ae";
                }
                MailAddress address = new MailAddress(str, displayName);
                message.From = address;
                message.To.Add(this.method_1(this.string_3));
                message.Subject = this.string_4;
                message.Body = this.string_5;
            }
            catch (Exception exception2)
            {
                ilog_0.Error("Error creating EMail Alert: " + exception2.Message);
            }
            if (!bool_1)
            {
                try
                {
                    client.SendCompleted += new SendCompletedEventHandler(this.Notify);
                    client.SendAsync(message, this);
                    return;
                }
                catch (Exception exception)
                {
                    ilog_0.Error("Error sending EMail Alert: " + exception.Message);
                    return;
                }
            }
            try
            {
                client.Send(message);
            }
            catch (Exception exception3)
            {
                ilog_0.Error("Error sending EMail Alert: " + exception3.Message);
                throw exception3;
            }
        }

        private string method_1(string string_7)
        {
            string_7 = string_7.Replace("\r\n", ";");
            string_7 = string_7.Replace(";", ",");
            string_7 = string_7.TrimEnd(new char[] { ',' });
            string_7 = string_7.Replace(" ", "");
            return string_7;
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
                        ilog_0.Info("Sent Mail" + ((userState != null) ? (" - Message text: " + userState.string_5 + " to " + userState.string_3) : ""));
                    }
                    else
                    {
                        ilog_0.Error("Unable to send Mail" + ((userState != null) ? (" - Message text: " + userState.string_5 + " to " + userState.string_3) : ""));
                        if (e.Error.InnerException != null)
                        {
                            ilog_0.Error(e.Error.InnerException.Message);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ilog_0.Error("Error occured while notifying the email alert" + exception.Message);
            }
        }

        private static void smethod_0()
        {
            WLPEmail email;
        Label_0000:
            email = null;
            lock (stack_0)
            {
                if (stack_0.Count > 0)
                {
                    email = stack_0.Pop();
                }
                if (email == null)
                {
                    goto Label_003D;
                }
            }
            email.method_0(true);
        Label_003D:
            Thread.Sleep(20);
            goto Label_0000;
        }

        public string Product
        {
            [CompilerGenerated]
            get
            {
                return this.string_6;
            }
            [CompilerGenerated]
            set
            {
                this.string_6 = value;
            }
        }
    }
}

