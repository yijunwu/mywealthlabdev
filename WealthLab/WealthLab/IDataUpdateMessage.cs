namespace WealthLab
{
    using System;

    public interface IDataUpdateMessage
    {
        void DisplayUpdateMessage(string message);
        void ReportUpdateProgress(int progressPercent);
    }
}

