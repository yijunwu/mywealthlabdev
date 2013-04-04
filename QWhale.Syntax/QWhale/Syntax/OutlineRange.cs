namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class OutlineRange : Range, IOutlineRange, IRange, ICloneable
    {
        private int level;
        private string text;
        private bool visible;

        public OutlineRange()
        {
        }

        public OutlineRange(Point start, Point end, int level, string text) : base(start, end)
        {
            this.text = text;
            this.level = level;
            this.visible = true;
        }

        public OutlineRange(Point start, Point end, int level, string text, bool visible) : base(start, end)
        {
            this.text = text;
            this.level = level;
            this.visible = visible;
        }

        public override object Clone()
        {
            return new OutlineRange(this.StartPoint, this.EndPoint, this.level, this.text) { Visible = this.visible };
        }

        protected virtual void OnLevelChanged()
        {
        }

        protected virtual void OnVisibleChanged()
        {
        }

        [Description("Represents text substituting collapsed outline section if outline buttons are displayed.")]
        public virtual string DisplayText
        {
            get
            {
                return this.text;
            }
        }

        [Description("Represents level of outline nesting for the outline section.")]
        public virtual int Level
        {
            get
            {
                return this.level;
            }
            set
            {
                if (this.level != value)
                {
                    this.level = value;
                    this.OnLevelChanged();
                }
            }
        }

        [Description("Represents text substituting collapsed outline section.")]
        public virtual string Text
        {
            get
            {
                return this.text;
            }
        }

        [Description("Gets or sets a value indicating whether outline section is visible (expanded).")]
        public virtual bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    this.OnVisibleChanged();
                }
            }
        }
    }
}

