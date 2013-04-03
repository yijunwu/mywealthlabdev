namespace WealthLab
{
    using System;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public interface IVisualizerHost
    {
        string ApplicationName();
        void CopyListViewToClipboard(ListView listView_0);
        TradingSystemExecutor GetExecutor();
        void GetPageSettings(ref PageSettings pageSettings);
        string LookupStrategyName(string strategyID);
        void PrintAll();
        void SelectPosition(Position position);
        void SelectPV(string string_0);
        void SelectSymbol(string symbol);
        bool ShowPrintDialog();
        bool ShowPrintPreview();
        void StrategySummary(ref DataObject printObject);
    }
}

