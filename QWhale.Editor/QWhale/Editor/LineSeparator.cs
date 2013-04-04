namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    public class LineSeparator : ILineSeparator, IUpdate
    {
        private Color contentDividerColor;
        private Color highlightBackColor;
        private Color highlightForeColor;
        private Color lineColor;
        private SeparatorOptions options;
        private ISyntaxEdit owner;
        private int tempLine;
        private int updateCount;

        public LineSeparator()
        {
            this.highlightBackColor = EditConsts.DefaultLineSeparatorColor;
            this.highlightForeColor = Color.Empty;
            this.lineColor = EditConsts.DefaultLineSeparatorLineColor;
            this.contentDividerColor = EditConsts.DefaultContentDividerColor;
            this.tempLine = -1;
        }

        public LineSeparator(ISyntaxEdit owner) : this()
        {
            this.owner = owner;
        }

        public virtual void Assign(ILineSeparator source)
        {
            this.BeginUpdate();
            try
            {
                this.Options = source.Options;
                this.HighlightForeColor = source.HighlightForeColor;
                this.HighlightBackColor = source.HighlightBackColor;
                this.LineColor = source.LineColor;
                this.ContentDividerColor = source.ContentDividerColor;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public virtual int BeginUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public virtual int DisableUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public virtual int EnableUpdate()
        {
            this.updateCount--;
            return this.updateCount;
        }

        public virtual int EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update();
            }
            return this.updateCount;
        }

        protected void InvalidateLine()
        {
            if ((this.owner != null) && (this.tempLine >= 0))
            {
                Region rectRegion = this.owner.SyntaxPaint.GetRectRegion(new Rectangle(0, this.tempLine, 0x7fffffff, this.tempLine));
                if (rectRegion != null)
                {
                    this.owner.Invalidate(rectRegion, false);
                    rectRegion.Dispose();
                }
            }
        }

        public virtual bool NeedHide()
        {
            return (((this.options & SeparatorOptions.HighlightCurrentLine) != SeparatorOptions.None) && ((this.options & SeparatorOptions.HideHighlighting) != SeparatorOptions.None));
        }

        public virtual bool NeedHighlight()
        {
            if ((this.owner == null) || ((this.options & SeparatorOptions.HighlightCurrentLine) == SeparatorOptions.None))
            {
                return false;
            }
            if ((this.options & SeparatorOptions.HideHighlighting) != SeparatorOptions.None)
            {
                return this.owner.Focused;
            }
            return true;
        }

        public virtual bool NeedHighlightDisplayLine(int index)
        {
            if (((this.owner == null) || (((this.options & SeparatorOptions.HighlightCurrentLine) == SeparatorOptions.None) && (this.tempLine < 0))) || (this.owner.DisplayLines.PointToDisplayPoint((this.tempLine >= 0) ? new Point(0, this.tempLine) : this.owner.Position).Y != index))
            {
                return false;
            }
            if ((this.options & SeparatorOptions.HideHighlighting) != SeparatorOptions.None)
            {
                return this.owner.Focused;
            }
            return true;
        }

        public virtual bool NeedHighlightLine(int index)
        {
            if (((this.owner == null) || (((this.options & SeparatorOptions.HighlightCurrentLine) == SeparatorOptions.None) && (this.tempLine < 0))) || (((this.tempLine >= 0) ? this.tempLine : this.owner.Position.Y) != index))
            {
                return false;
            }
            if ((this.options & SeparatorOptions.HideHighlighting) != SeparatorOptions.None)
            {
                return this.owner.Focused;
            }
            return true;
        }

        protected virtual void OnContentDividerColorChanged()
        {
            this.Update();
        }

        protected virtual void OnHighlightBackColorChanged()
        {
            this.Update();
        }

        protected virtual void OnHighlightForeColorChanged()
        {
            this.Update();
        }

        protected virtual void OnLineColorChanged()
        {
            this.Update();
        }

        protected virtual void OnOptionsChanged()
        {
            this.Update();
        }

        public virtual void ResetContentDividerColor()
        {
            this.ContentDividerColor = EditConsts.DefaultContentDividerColor;
        }

        public virtual void ResetHighlightBackColor()
        {
            this.HighlightBackColor = EditConsts.DefaultLineSeparatorColor;
        }

        public virtual void ResetHighlightForeColor()
        {
            this.HighlightForeColor = Color.Empty;
        }

        public virtual void ResetLineColor()
        {
            this.LineColor = EditConsts.DefaultLineSeparatorLineColor;
        }

        public virtual void ResetOptions()
        {
            this.Options = SeparatorOptions.None;
        }

        public bool ShouldSerializeContentDividerColor()
        {
            return (this.contentDividerColor != EditConsts.DefaultContentDividerColor);
        }

        public bool ShouldSerializeHighlightBackColor()
        {
            return (this.highlightBackColor != EditConsts.DefaultLineSeparatorColor);
        }

        public bool ShouldSerializeHighlightForeColor()
        {
            return (this.highlightForeColor != Color.Empty);
        }

        public bool ShouldSerializeLineColor()
        {
            return (this.lineColor != EditConsts.DefaultLineSeparatorLineColor);
        }

        public virtual void TempHighlightLine(int index)
        {
            this.TempLine = index;
        }

        public virtual void TempUnhighlightLine()
        {
            this.TempLine = -1;
        }

        public void Update()
        {
            if ((this.updateCount == 0) && (this.owner != null))
            {
                int num = ((this.options & SeparatorOptions.SeparateLines) != SeparatorOptions.None) ? 1 : 0;
                if (num != this.owner.Painter.LineSpace)
                {
                    this.owner.Painter.LineSpace = num;
                    this.owner.UpdateView();
                }
                this.owner.Invalidate();
            }
        }

        [Description("Gets or sets color of horizontal lines between particular sections(for example, methods) in the Edit control.")]
        public virtual Color ContentDividerColor
        {
            get
            {
                return this.contentDividerColor;
            }
            set
            {
                if (this.contentDividerColor != value)
                {
                    this.contentDividerColor = value;
                    this.OnContentDividerColorChanged();
                }
            }
        }

        [Description("Gets or sets a background color of highlighted line.")]
        public virtual Color HighlightBackColor
        {
            get
            {
                return this.highlightBackColor;
            }
            set
            {
                if (this.highlightBackColor != value)
                {
                    this.highlightBackColor = value;
                    this.OnHighlightBackColorChanged();
                }
            }
        }

        [Description("Gets or sets a foreground color of highlighted line.")]
        public virtual Color HighlightForeColor
        {
            get
            {
                return this.highlightForeColor;
            }
            set
            {
                if (this.highlightForeColor != value)
                {
                    this.highlightForeColor = value;
                    this.OnHighlightForeColorChanged();
                }
            }
        }

        [Description("Gets or sets color of horizontal lines between particular lines in the Edit control.")]
        public virtual Color LineColor
        {
            get
            {
                return this.lineColor;
            }
            set
            {
                if (this.lineColor != value)
                {
                    this.lineColor = value;
                    this.OnLineColorChanged();
                }
            }
        }

        [Description("Gets or sets a set of flags customizing appearance and behaviour of the \"LineSeparator\"."), DefaultValue(0), Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
        public virtual SeparatorOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    this.OnOptionsChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlLineSeparatorInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        protected int TempLine
        {
            get
            {
                return this.tempLine;
            }
            set
            {
                if (this.tempLine != value)
                {
                    this.InvalidateLine();
                    this.tempLine = value;
                    this.InvalidateLine();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }
    }
}

