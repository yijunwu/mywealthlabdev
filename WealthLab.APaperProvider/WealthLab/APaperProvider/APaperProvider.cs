namespace WealthLab.APaperProvider
{
    using System;
    using WealthLab;
    using WealthLab.PaperProvider;

    [ProductionBrokerProvider]
    public class APaperProvider : PaperBrokerProvider
    {
        private Class1 class1_0;

        public override Quote GetQuote(string symbol)
        {
            return this.class1_0.method_2(symbol);
        }

        public override void Initialize(IBrokerHost brokerHost, AuthenticationProvider authProvider)
        {
            base.Initialize(brokerHost, authProvider);
            this.class1_0 = new Class1(authProvider.DataHost);
        }

        public override Bars RequestHistoricalData(string symbol, DateTime startDate, DateTime endDate)
        {
            return this.class1_0.method_3(symbol, startDate, endDate);
        }
    }
}

