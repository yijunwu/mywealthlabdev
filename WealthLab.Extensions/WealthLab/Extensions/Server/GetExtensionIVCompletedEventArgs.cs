namespace WealthLab.Extensions.Server
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DebuggerStepThrough, DesignerCategory("code"), GeneratedCode("System.Web.Services", "2.0.50727.3053")]
    public class GetExtensionIVCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] object_0;

        internal GetExtensionIVCompletedEventArgs(object[] object_1, Exception exception_0, bool bool_0, object object_2) : base(exception_0, bool_0, object_2)
        {
            this.object_0 = object_1;
        }

        public byte[] Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (byte[]) this.object_0[0];
            }
        }
    }
}

