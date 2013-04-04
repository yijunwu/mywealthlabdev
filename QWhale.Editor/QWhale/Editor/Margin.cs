namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class Margin : IMargin, IUpdate
    {
        private bool allowDrag;
        private System.Drawing.Pen columnPen;
        private int[] columnPositions;
        private bool columnsVisible;
        private int drawX;
        private int drawY;
        private bool isDragging;
        private ISyntaxEdit owner;
        private System.Drawing.Pen pen;
        private int position;
        private bool showHints;
        private int updateCount;
        private bool visible;

        public Margin()
        {
            this.position = EditConsts.DefaultMarginPosition;
            this.columnPositions = new int[] { EditConsts.DefaultColumnMarginPosition };
            this.drawX = -1;
            this.drawY = -1;
            this.pen = new System.Drawing.Pen(EditConsts.DefaultMarginForeColor, 1f);
            this.columnPen = new System.Drawing.Pen(EditConsts.DefaultMarginColumnForeColor, 1f);
            this.columnPen.DashStyle = DashStyle.Dot;
        }

        public Margin(ISyntaxEdit owner) : this()
        {
            this.owner = owner;
        }

        public virtual void Assign(IMargin source)
        {
            this.BeginUpdate();
            try
            {
                this.Pen.Width = source.Pen.Width;
                this.Pen.Color = source.Pen.Color;
                this.ColumnPen.Width = source.ColumnPen.Width;
                this.ColumnPen.Color = source.ColumnPen.Color;
                this.Position = source.Position;
                this.ColumnPositions = source.ColumnPositions;
                this.Visible = source.Visible;
                this.ColumnsVisible = source.ColumnsVisible;
                this.AllowDrag = source.AllowDrag;
                this.ShowHints = source.ShowHints;
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

        public virtual void CancelDragging()
        {
            this.IsDragging = false;
        }

        public virtual bool Contains(int x, int y)
        {
            if (this.owner == null)
            {
                return false;
            }
            if (this.owner.Pages.PageType == PageType.PageLayout)
            {
                IEditPage pageAtPoint = this.owner.Pages.GetPageAtPoint(x, y);
                if ((pageAtPoint == null) || !pageAtPoint.ClientRect.Contains(x, y))
                {
                    return false;
                }
            }
            Point point = this.owner.ScreenToDisplay(x, y);
            int num = this.owner.DisplayToScreen(this.Position, point.Y, true).X;
            return ((x > (num - EditConsts.DefaultRulerHitWidth)) && (x <= (num + EditConsts.DefaultRulerHitWidth)));
        }

        public virtual int DisableUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public virtual void DragTo(int x, int y)
        {
            if (this.drawX >= 0)
            {
                this.DrawLine(this.drawX, this.drawY, true);
            }
            this.DrawLine(x, y, false);
        }

        protected void DrawLine(int x, int y, bool erase)
        {
            this.drawX = x;
            this.drawY = y;
            if (this.owner != null)
            {
                Rectangle clientRect;
                ISyntaxEdit owner = this.owner;
                if (owner.Pages.PageType == PageType.PageLayout)
                {
                    IEditPage pageAtPoint = owner.Pages.GetPageAtPoint(x, y);
                    if (pageAtPoint == null)
                    {
                        return;
                    }
                    clientRect = pageAtPoint.ClientRect;
                }
                else
                {
                    clientRect = owner.ClientRect;
                }
                if (erase)
                {
                    owner.Invalidate(new Rectangle(x, clientRect.Top, 1, clientRect.Height));
                }
                else
                {
                    using (Graphics graphics = owner.CreateGraphics())
                    {
                        graphics.DrawLine(this.pen, x, clientRect.Top, x, clientRect.Bottom - 1);
                    }
                }
            }
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

        ~Margin()
        {
            this.pen.Dispose();
            this.columnPen.Dispose();
        }

        protected int GetColumnPosition(int pos)
        {
            int length = this.columnPositions.Length;
            if (length == 0)
            {
                return EditConsts.DefaultColumnMarginPosition;
            }
            int num2 = pos - this.columnPositions[length - 1];
            int index = 0;
            if (num2 >= 0)
            {
                if (length == 1)
                {
                    index = this.columnPositions[0];
                }
                else
                {
                    index = this.columnPositions[length - 1] - this.columnPositions[length - 2];
                }
                return (this.columnPositions[length - 1] + (((num2 / index) + 1) * index));
            }
            index = 0;
            while (pos >= this.columnPositions[index])
            {
                index++;
            }
            return this.columnPositions[index];
        }

        protected virtual void OnAllowDrawChanged()
        {
        }

        protected virtual void OnColumnPenChanged()
        {
            this.Update();
        }

        protected virtual void OnColumnPenColorChanged()
        {
        }

        protected virtual void OnColumnPositionsChanged()
        {
        }

        protected virtual void OnColumnsVisibleChanged()
        {
            this.Update();
        }

        protected virtual void OnIsDragingChanged()
        {
            if (this.drawX >= 0)
            {
                this.DrawLine(this.drawX, this.drawY, true);
            }
            this.owner.Invalidate();
            this.drawX = -1;
            this.drawY = -1;
        }

        protected virtual void OnPenChanged()
        {
            this.Update();
        }

        protected virtual void OnPenColorChanged()
        {
        }

        protected virtual void OnPositionChanged()
        {
            this.Update(true);
        }

        protected virtual void OnShowHintsChanged()
        {
        }

        protected virtual void OnVisibleChanged()
        {
            this.Update();
        }

        public virtual void Paint(IPainter painter, Rectangle rect)
        {
            if (!this.isDragging)
            {
                painter.DrawLine(rect.Left, 0, rect.Left, rect.Bottom, this.pen.Color, (int) this.pen.Width, this.pen.DashStyle);
            }
        }

        public virtual void PaintColumn(IPainter painter, Rectangle rect)
        {
            if (this.columnPen.DashStyle == DashStyle.Dot)
            {
                painter.DrawDotLine(rect.Left, 0, rect.Left, rect.Bottom, this.columnPen.Color, Color.Empty);
            }
            else
            {
                painter.DrawLine(rect.Left, 0, rect.Left, rect.Bottom, this.columnPen.Color, (int) this.columnPen.Width, this.columnPen.DashStyle);
            }
        }

        public virtual void ResetAllowDrag()
        {
            this.AllowDrag = false;
        }

        public virtual void ResetColumnPositions()
        {
            this.ColumnPositions = new int[] { EditConsts.DefaultColumnMarginPosition };
        }

        public virtual void ResetColumnsPenColor()
        {
            this.PenColor = EditConsts.DefaultMarginColumnForeColor;
        }

        public virtual void ResetColumnsVisible()
        {
            this.ColumnsVisible = false;
        }

        public virtual void ResetPenColor()
        {
            this.PenColor = EditConsts.DefaultMarginForeColor;
        }

        public virtual void ResetPosition()
        {
            this.Position = EditConsts.DefaultMarginPosition;
        }

        public virtual void ResetShowHints()
        {
            this.ShowHints = false;
        }

        public virtual void ResetVisible()
        {
            this.Visible = false;
        }

        public bool ShouldSerializeColumnPenColor()
        {
            return (this.ColumnPenColor != EditConsts.DefaultMarginColumnForeColor);
        }

        public bool ShouldSerializeColumnPositions()
        {
            if (this.columnPositions.Length == 1)
            {
                return (this.columnPositions[0] != EditConsts.DefaultColumnMarginPosition);
            }
            return true;
        }

        public bool ShouldSerializePenColor()
        {
            return (this.PenColor != EditConsts.DefaultMarginForeColor);
        }

        public bool ShouldSerializePosition()
        {
            return (this.position != EditConsts.DefaultMarginPosition);
        }

        public void Update()
        {
            if ((this.updateCount == 0) && (this.owner != null))
            {
                this.owner.Invalidate();
            }
        }

        public void Update(bool needUpdate)
        {
            if ((this.updateCount == 0) && (this.owner != null))
            {
                this.owner.Invalidate();
                if ((needUpdate && this.owner.WordWrap) && this.owner.WrapAtMargin)
                {
                    this.owner.UpdateWordWrap();
                }
            }
        }

        [DefaultValue(false), Description("Indicates whether drag operation can performed to \"Margin\".")]
        public virtual bool AllowDrag
        {
            get
            {
                return this.allowDrag;
            }
            set
            {
                if (this.allowDrag != value)
                {
                    this.allowDrag = value;
                    this.OnAllowDrawChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual System.Drawing.Pen ColumnPen
        {
            get
            {
                return this.columnPen;
            }
            set
            {
                if (this.columnPen != value)
                {
                    this.columnPen = value;
                    this.OnColumnPenChanged();
                }
            }
        }

        [Description("Gets or sets a color of the column margin line.")]
        public virtual Color ColumnPenColor
        {
            get
            {
                return this.columnPen.Color;
            }
            set
            {
                if (this.columnPen.Color != value)
                {
                    this.columnPen.Color = value;
                    this.OnColumnPenColorChanged();
                }
            }
        }

        [Description("Gets or sets the character columns where additional column margin will be drawn.")]
        public virtual int[] ColumnPositions
        {
            get
            {
                return this.columnPositions;
            }
            set
            {
                if (this.columnPositions != value)
                {
                    this.columnPositions = new int[value.Length];
                    Array.Copy(value, this.columnPositions, value.Length);
                    int num = 0;
                    foreach (int num2 in this.columnPositions)
                    {
                        if (num2 <= num)
                        {
                            ErrorHandler.Error(new Exception(string.Format(StringConsts.InvalidMarginColumn, value.ToString())));
                        }
                        num = num2;
                    }
                    this.OnColumnPositionsChanged();
                }
            }
        }

        [DefaultValue(false), Description("Gets or sets a value indicating whether column margins should be painted.")]
        public virtual bool ColumnsVisible
        {
            get
            {
                return this.columnsVisible;
            }
            set
            {
                if (this.columnsVisible != value)
                {
                    this.columnsVisible = value;
                    this.OnColumnsVisibleChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool IsDragging
        {
            get
            {
                return this.isDragging;
            }
            set
            {
                if (this.isDragging != value)
                {
                    this.isDragging = value;
                    this.OnIsDragingChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual System.Drawing.Pen Pen
        {
            get
            {
                return this.pen;
            }
            set
            {
                if (this.pen != value)
                {
                    this.pen = value;
                    this.OnPenChanged();
                }
            }
        }

        [Description("Gets or sets a color of the margin line.")]
        public virtual Color PenColor
        {
            get
            {
                return this.pen.Color;
            }
            set
            {
                if (this.pen.Color != value)
                {
                    this.pen.Color = value;
                    this.OnPenColorChanged();
                }
            }
        }

        [Description("Gets or sets value indicating position, in characters, of the vertical line within the text portion of the control.")]
        public virtual int Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    this.OnPositionChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlMarginInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [DefaultValue(false), Description("Gets or sets a value indicating whether \"Margin\" should display some hint when mouse pointer is over the margin area.")]
        public virtual bool ShowHints
        {
            get
            {
                return this.showHints;
            }
            set
            {
                if (this.showHints != value)
                {
                    this.showHints = value;
                    this.OnShowHintsChanged();
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

        [Description("Gets or sets a value indicating whether vertical line should be painted."), DefaultValue(false)]
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

