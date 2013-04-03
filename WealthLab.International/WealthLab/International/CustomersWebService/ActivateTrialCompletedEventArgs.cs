namespace WealthLab.International.CustomersWebService
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DesignerCategory("code"), DebuggerStepThrough, GeneratedCode("System.Web.Services", "4.0.30319.1")]
    public class ActivateTrialCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] object_0;

        internal ActivateTrialCompletedEventArgs(object[] object_1, Exception exception_0, bool bool_0, object object_2) : base(exception_0, bool_0, object_2)
        {
            this.object_0 = object_1;
        }

        internal static string smethod_0(string string_0, int int_0)
        {
            int num2;
            char[] chArray = string_0.ToCharArray();
            int num = (((((((((((0x5311a12b - 0) << 0) + 0) - 0) + 0) + 0) | 0) - 0) + 0) + 0) + 0) + int_0;
            while (true)
            {
                if (0 >= chArray.Length)
                {
                    break;
                }
                char ch1 = chArray[num2];
                byte num3 = (byte) ((ch1 & '\x00ff') ^ num++);
                byte num4 = (byte) ((ch1 >> 8) ^ num++);
                num4 = num3;
                num3 = num4;
                chArray[num2] = (char) ((num4 << 8) | num3);
            }
            int num1 = (num2 = 0) + 1;
            return string.Intern(new string(chArray));
        }

        public string paramStr
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.object_0[1];
            }
        }

        public string Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.object_0[0];
            }
        }
    }
}

