namespace WealthLab
{
    using System;

    public interface IConnectionStatus
    {
        void Connect();
        void Connect(bool reconnect);
        void Disconnect();
        void StatusUpdate(ConnStatus status, int StatusCode, string Message);
    }
}

