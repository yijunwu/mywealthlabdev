namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public class CodeSnippetRange : Range, ICodeSnippetRange, IRange, ICloneable
    {
        private string id;
        private bool isEditable;
        private string tooltip;

        public CodeSnippetRange(Point start, Point end) : base(start, end)
        {
        }

        public CodeSnippetRange(Point start, Point end, string id, string tooltip, bool isEditable) : base(start, end)
        {
            this.id = id;
            this.tooltip = tooltip;
            this.isEditable = isEditable;
        }

        public override object Clone()
        {
            return new CodeSnippetRange(this.StartPoint, this.EndPoint, this.id, this.tooltip, this.isEditable);
        }

        protected virtual void OnIDChanged()
        {
        }

        protected virtual void OnIsEditableChanged()
        {
        }

        protected virtual void OnTooltipChanged()
        {
        }

        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    this.OnIDChanged();
                }
            }
        }

        public bool IsEditable
        {
            get
            {
                return this.isEditable;
            }
            set
            {
                if (this.isEditable != value)
                {
                    this.isEditable = value;
                    this.OnIsEditableChanged();
                }
            }
        }

        public string Tooltip
        {
            get
            {
                return this.tooltip;
            }
            set
            {
                if (this.tooltip != value)
                {
                    this.tooltip = value;
                    this.OnTooltipChanged();
                }
            }
        }
    }
}

