namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.TextSource;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Runtime.InteropServices;

    [ToolboxItem(false), DesignTimeVisible(false)]
    public class EditorPrintDocument : PrintDocument
    {
        private int page;
        private int pageCount;
        private int pageIndex;
        private int pages;
        private IPrinting printing;
        private int startLine;
        private bool startPrinting;
        private ISyntaxEdit syntaxEdit;

        public EditorPrintDocument(IPrinting printing, PrinterSettings settings)
        {
            this.printing = printing;
            base.PrinterSettings = settings;
        }

        private int GetPagesCount(ISyntaxEdit edit)
        {
            int num;
            PageSettings defaultPageSettings = base.PrinterSettings.DefaultPageSettings;
            Rectangle rect = new Rectangle(0, 0, (defaultPageSettings.PaperSize.Width - defaultPageSettings.Margins.Left) - defaultPageSettings.Margins.Right, (defaultPageSettings.PaperSize.Height - defaultPageSettings.Margins.Top) - defaultPageSettings.Margins.Bottom);
            return this.GetPagesCount(edit, rect, out num);
        }

        private int GetPagesCount(ISyntaxEdit edit, Rectangle rect, out int linesPerPage)
        {
            edit.Bounds = rect;
            edit.DisplayLines.Loaded = true;
            int num = edit.DisplayLines.DisplayCount - 1;
            linesPerPage = Math.Max(edit.LinesInHeight, 1);
            this.pages = num / linesPerPage;
            if ((num % linesPerPage) != 0)
            {
                this.pages++;
            }
            return this.pages;
        }

        public void Init(ISyntaxEdit edit)
        {
            this.Init(edit, false);
        }

        public void Init(ISyntaxEdit edit, bool calcPages)
        {
            base.PrinterSettings.MinimumPage = 1;
            base.PrinterSettings.FromPage = Math.Max(base.PrinterSettings.FromPage, 1);
            base.PrinterSettings.ToPage = Math.Max(base.PrinterSettings.ToPage, 1);
            this.syntaxEdit = this.printing.OnCreatePrintEdit();
            if (((this.printing.Options & PrintOptions.PrintSelection) != PrintOptions.None) && !edit.Selection.IsEmpty)
            {
                this.syntaxEdit.Lexer = edit.Lexer;
                this.syntaxEdit.Lines.SetTextAndData(edit.Selection.SelectedText, edit.Source.NeedParse() ? edit.Selection.SelectedColorData : null);
            }
            else
            {
                this.syntaxEdit.Source = edit.Source;
            }
            this.syntaxEdit.BorderStyle = EditBorderStyle.None;
            this.syntaxEdit.Font = edit.Font;
            this.syntaxEdit.WordWrap = (this.printing.Options & PrintOptions.WordWrap) != PrintOptions.None;
            this.syntaxEdit.Outlining.Assign(edit.Outlining);
            IList<IRange> ranges = new List<IRange>();
            edit.DisplayLines.GetOutlineRanges(ranges);
            this.syntaxEdit.DisplayLines.SetOutlineRanges(ranges);
            this.syntaxEdit.Gutter.Assign(edit.Gutter);
            this.syntaxEdit.Gutter.Visible = false;
            this.syntaxEdit.Gutter.Options = ((this.printing.Options & PrintOptions.LineNumbers) != PrintOptions.None) ? GutterOptions.PaintLineNumbers : GutterOptions.None;
            this.syntaxEdit.Gutter.LineNumbersBackColor = this.syntaxEdit.BackColor;
            if (((this.printing.Options & PrintOptions.PrintSelection) != PrintOptions.None) && !edit.Selection.IsEmpty)
            {
                IGutter gutter = this.syntaxEdit.Gutter;
                gutter.LineNumbersStart += edit.Selection.SelectionRect.Top;
            }
            this.syntaxEdit.EditMargin.Visible = false;
            this.syntaxEdit.Braces.BracesOptions = BracesOptions.None;
            if ((this.printing.Options & PrintOptions.UseSyntax) == PrintOptions.None)
            {
                this.syntaxEdit.SyntaxPaint.DisableSyntaxPaint = true;
            }
            if ((this.printing.Options & PrintOptions.UseColors) == PrintOptions.None)
            {
                this.syntaxEdit.SyntaxPaint.DisableColorPaint = true;
                this.syntaxEdit.Gutter.LineNumbersForeColor = this.syntaxEdit.ForeColor;
            }
            this.syntaxEdit.Scrolling.WindowOriginX = 0;
            this.syntaxEdit.Scrolling.WindowOriginY = 0;
            if (calcPages)
            {
                base.PrinterSettings.ToPage = Math.Max(this.GetPagesCount(this.syntaxEdit), 1);
            }
            this.printing.OnInitialized();
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);
            this.startPrinting = true;
            this.page = 0;
            this.pageIndex = 0;
            this.startLine = 0;
        }

        protected override void OnEndPrint(PrintEventArgs e)
        {
            base.OnEndPrint(e);
        }

        protected override void OnPrintPage(PrintPageEventArgs ev)
        {
            base.OnPrintPage(ev);
            Rectangle marginBounds = ev.MarginBounds;
            if (this.startPrinting)
            {
                int linesPerPage = 0;
                this.pages = this.GetPagesCount(this.syntaxEdit, ev.MarginBounds, out linesPerPage);
                this.pageCount = Math.Max(this.pages, 1);
                if (base.PrinterSettings.PrintRange == PrintRange.SomePages)
                {
                    this.pages = Math.Min(this.pages, (base.PrinterSettings.ToPage - base.PrinterSettings.FromPage) + 1);
                    this.pageIndex = base.PrinterSettings.FromPage - 1;
                    this.startLine += linesPerPage * this.pageIndex;
                }
                this.pages = Math.Max(this.pages, 1);
                this.startPrinting = false;
            }
            Rectangle rectangle2 = marginBounds;
            int fontHeight = this.syntaxEdit.Painter.FontHeight;
            if (fontHeight != 0)
            {
                rectangle2.Height = (rectangle2.Height / fontHeight) * fontHeight;
            }
            float scaleX = ev.Graphics.DpiX / 100f;
            float scaleY = ev.Graphics.DpiY / 100f;
            IPainter painter = this.syntaxEdit.Painter;
            painter.BeginPaint(ev.Graphics);
            try
            {
                painter.Transform(0, 0, scaleX, scaleY);
                try
                {
                    if ((this.printing.Options & PrintOptions.UseHeader) != PrintOptions.None)
                    {
                        Rectangle rect = new Rectangle(marginBounds.Left - this.printing.Header.Offset.X, marginBounds.Top - (this.printing.Header.Font.Height + this.printing.Header.Offset.Y), marginBounds.Width + (this.printing.Header.Offset.X * 2), this.printing.Header.Font.Height);
                        this.printing.Header.Paint(painter, rect, this.pageIndex, this.pageCount, false);
                    }
                    if (((this.printing.Options & PrintOptions.UseFooter) != PrintOptions.None) || ((this.printing.Options & PrintOptions.PageNumbers) != PrintOptions.None))
                    {
                        Rectangle rectangle4 = new Rectangle(marginBounds.Left - this.printing.Footer.Offset.X, marginBounds.Bottom + this.printing.Footer.Offset.Y, marginBounds.Width + (this.printing.Footer.Offset.X * 2), this.printing.Footer.Font.Height);
                        this.printing.Footer.Paint(painter, rectangle4, this.pageIndex, this.pageCount, (this.printing.Options & PrintOptions.PageNumbers) != PrintOptions.None);
                    }
                }
                finally
                {
                    painter.EndTransform();
                }
                this.syntaxEdit.SyntaxPaint.PaintWindow(painter, this.startLine, new Rectangle(new Point(0, 0), rectangle2.Size), marginBounds.Location, scaleX, scaleY, true, true);
                this.startLine += Math.Max(this.syntaxEdit.LinesInHeight, 1);
                this.page++;
                this.pageIndex++;
                ev.HasMorePages = this.page < this.pages;
            }
            finally
            {
                painter.EndPaint();
            }
        }
    }
}

