namespace WealthLab.StrategyProviders.WLWebServices
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DesignerCategory("code"), DebuggerStepThrough, GeneratedCode("System.Web.Services", "4.0.30319.1")]
    public class GetPublicStrategiesCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal GetPublicStrategiesCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public string[] Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string[]) this.results[0];
            }
        }
    }
}

