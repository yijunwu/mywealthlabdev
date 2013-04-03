namespace WealthLab
{
    using System;

    public abstract class Commission
    {
        protected Commission()
        {
        }

        public abstract double Calculate(TradeType tradeType, OrderType orderType, double orderPrice, double shares, Bars bars);
        public override string ToString()
        {
            return this.FriendlyName;
        }

        public abstract string Description { get; }

        public abstract string FriendlyName { get; }
    }
}

