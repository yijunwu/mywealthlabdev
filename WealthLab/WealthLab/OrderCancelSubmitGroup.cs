namespace WealthLab
{
    using System;
    using System.Collections.Generic;

    public class OrderCancelSubmitGroup
    {
        private List<Order> list_0 = new List<Order>();
        private List<Order> list_1 = new List<Order>();

        public List<Order> CancelPending
        {
            get
            {
                return this.list_0;
            }
        }

        public List<Order> SubmitPending
        {
            get
            {
                return this.list_1;
            }
        }
    }
}

