namespace WealthLab.APaperProvider
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public static class ErrorMessage
    {
        private static ErrorForm errorForm_0 = new ErrorForm();
        [CompilerGenerated]
        private static MethodInvoker methodInvoker_0;

        public static void Show(string errorText)
        {
            errorForm_0.ErrorText = errorText;
            if (ErrorForm.GetMainForm() != null)
            {
                if (methodInvoker_0 == null)
                {
                    methodInvoker_0 = new MethodInvoker(ErrorMessage.smethod_0);
                }
                ErrorForm.GetMainForm().Invoke(methodInvoker_0);
            }
            else
            {
                errorForm_0.ShowDialog();
            }
        }

        [CompilerGenerated]
        private static void smethod_0()
        {
            errorForm_0.ShowDialog(ErrorForm.GetMainForm());
        }
    }
}

