namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public interface IOptimizationHost
    {
        void CreateTab(string text, UserControl userControl_0);

        IList<string> MetricNames { get; }
    }
}

