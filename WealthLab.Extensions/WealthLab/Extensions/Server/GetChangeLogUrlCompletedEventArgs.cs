namespace WealthLab.Extensions.Server
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [GeneratedCode("System.Web.Services", "2.0.50727.3053"), DebuggerStepThrough, DesignerCategory("code")]
    public class GetChangeLogUrlCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] object_0;

        internal GetChangeLogUrlCompletedEventArgs(object[] object_1, Exception exception_0, bool bool_0, object object_2) : base(exception_0, bool_0, object_2)
        {
            this.object_0 = object_1;
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

