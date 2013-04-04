namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IEditRuler : IControl
    {
        event EventHandler Change;

        void Assign(IEditRuler source);
        void CancelDragging();
        void ResetIndentBackColor();
        void ResetOptions();
        void ResetUnits();
        void SendToBack();

        AnchorStyles Anchor { get; set; }

        Color IndentBackColor { get; set; }

        bool IsDragging { get; }

        int MarkWidth { get; set; }

        RulerOptions Options { get; set; }

        int PageStart { get; set; }

        int PageWidth { get; set; }

        int RulerStart { get; set; }

        int RulerWidth { get; set; }

        RulerUnits Units { get; set; }

        bool Vertical { get; set; }
    }
}

