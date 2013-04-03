namespace WealthLab
{
    using System;

    public interface IPerformanceVisualizer
    {
        void CopyToClipboard();
        void CreateVisualization(SystemPerformance performance, IVisualizerHost visHost);
        void EnableControls(bool enable);
        void Print();

        VisualizerAppliesTo AppliesTo { get; }

        string Description { get; }

        bool SupportClipboardCopy { get; }

        bool SupportsPrint { get; }

        string TabText { get; }
    }
}

