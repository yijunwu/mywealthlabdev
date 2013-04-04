namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using System;
    using System.Runtime.CompilerServices;

    public interface ICodeCompletionBox : ICodeCompletionWindow, IControl
    {
        event EventHandler SelectionChanged;

        ICodeCompletionColumn AddColumn();
        void ClearColumns();
        ICodeCompletionColumn InsertColumn(int index);
        void RemoveColumnAt(int index);
        void ResetDropDownCount();

        ICodeCompletionColumn[] Columns { get; }

        int DropDownCount { get; set; }

        string Filter { get; set; }

        bool Filtered { get; set; }

        bool ShowTabs { get; set; }

        bool Sorted { get; set; }
    }
}

