namespace WealthLab
{
    using System;

    public class OrderEventArgs : EventArgs
    {
        private WealthLab.Order order;

        public OrderEventArgs(WealthLab.Order order)
        {
            this.order = order;
        }

        public WealthLab.Order Order
        {
            get
            {
                return this.order;
            }
        }
    }
}

