namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;

    public interface IBrokerHost
    {
        void AccountBalanceUpdate(Account account);
        void AccountPositionsUpdate(Account account);
        void DisplayStatusBarMessage(string message);
        List<Order> GetOrders(Account account);
        void OrderStatusUpdate(string orderID, OrderStatus status, DateTime timestamp, double fillPrice, double fillQty, int code, string message);
        void OrderStatusUpdate(string orderID, OrderStatus status, DateTime timestamp, double fillPrice, double fillQty, int code, string message, int stateIndex);
        void QuoteUpdate(Quote quote);

        ISettingsHost Settings { get; }
    }
}

