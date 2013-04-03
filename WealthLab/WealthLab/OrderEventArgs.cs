namespace WealthLab
{
    using System;

    public class OrderEventArgs : EventArgs
    {
        private WealthLab.Order order_0;

        public OrderEventArgs(WealthLab.Order order)
        {
            this.order_0 = order;
        }

        public WealthLab.Order Order
        {
            get
            {
                return this.order_0;
            }
        }
    }
}

