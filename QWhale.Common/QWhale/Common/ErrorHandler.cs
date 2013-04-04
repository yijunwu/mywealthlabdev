namespace QWhale.Common
{
    using System;
    using System.Windows.Forms;

    public sealed class ErrorHandler
    {
        private static QWhale.Common.ErrorBehavior errorBehavior;

        private ErrorHandler()
        {
        }

        public static void Error(Exception exception)
        {
            if (exception != null)
            {
                switch (errorBehavior)
                {
                    case QWhale.Common.ErrorBehavior.Message:
                        MessageBox.Show(exception.Message, StringConsts.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;

                    case QWhale.Common.ErrorBehavior.Exception:
                        throw exception;
                }
            }
        }

        public static QWhale.Common.ErrorBehavior ErrorBehavior
        {
            get
            {
                return errorBehavior;
            }
            set
            {
                errorBehavior = value;
            }
        }
    }
}

