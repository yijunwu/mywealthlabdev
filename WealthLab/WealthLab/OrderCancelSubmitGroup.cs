namespace WealthLab
{
    using System;
    using System.Collections.Generic;

    public class OrderCancelSubmitGroup
    {
        private List<Order> listCancelPending = new List<Order>();
        private List<Order> listSubmitPending = new List<Order>();

        public List<Order> CancelPending
        {
            get
            {
                return this.listCancelPending;
            }
        }

        public List<Order> SubmitPending
        {
            get
            {
                return this.listSubmitPending;
            }
        }
    }
}

