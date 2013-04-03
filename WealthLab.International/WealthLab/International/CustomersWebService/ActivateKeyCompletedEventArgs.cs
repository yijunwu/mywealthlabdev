namespace WealthLab.International.CustomersWebService
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DebuggerStepThrough, DesignerCategory("code"), GeneratedCode("System.Web.Services", "4.0.30319.1")]
    public class ActivateKeyCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] object_0;

        internal ActivateKeyCompletedEventArgs(object[] object_1, Exception exception_0, bool bool_0, object object_2) : base(exception_0, bool_0, object_2)
        {
            this.object_0 = object_1;
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

