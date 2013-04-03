namespace WealthLab
{
    using Fidelity.Components;
    using System;

    public interface IStrategyHost : ISettingsHost
    {
        void SaveStrategy(Strategy strategy_0, string folderName);

        bool CancelDownload { get; set; }

        string FolderPath { get; }
    }
}

