namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class XmlMarginInfo : ISerializationInfo
    {
        private bool allowDrag;
        private string columnPenColor;
        private int[] columnPositions;
        private bool columnsVisible;
        private IMargin owner;
        private string penColor;
        private float penWidth;
        private int position;
        private bool showHints;
        private bool visible;

        public XmlMarginInfo()
        {
            this.position = EditConsts.DefaultMarginPosition;
            this.penColor = XmlColorInfo.SerializeColor(EditConsts.DefaultMarginForeColor);
            this.penWidth = 1f;
            this.visible = true;
            this.columnPositions = new int[] { EditConsts.DefaultColumnMarginPosition };
            this.columnPenColor = XmlColorInfo.SerializeColor(EditConsts.DefaultMarginColumnForeColor);
        }

        public XmlMarginInfo(IMargin owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IMargin) owner;
            this.Position = this.position;
            this.PenColor = this.penColor;
            this.PenWidth = this.penWidth;
            this.Visible = this.visible;
            this.AllowDrag = this.allowDrag;
            this.ShowHints = this.showHints;
            this.ColumnPositions = this.columnPositions;
            this.ColumnPenColor = this.columnPenColor;
            this.ColumnsVisible = this.columnsVisible;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.position = this.Position;
                this.penColor = this.PenColor;
                this.penWidth = this.PenWidth;
                this.visible = this.Visible;
                this.allowDrag = this.AllowDrag;
                this.showHints = this.ShowHints;
                this.columnPositions = this.ColumnPositions;
                this.columnPenColor = this.ColumnPenColor;
                this.columnsVisible = this.ColumnsVisible;
            }
        }

        public bool ShouldSerializeColumnPenColor()
        {
            return (this.ColumnPenColor != XmlColorInfo.SerializeColor(EditConsts.DefaultMarginColumnForeColor));
        }

        public bool ShouldSerializeColumnPositions()
        {
            if (this.ColumnPositions.Length == 1)
            {
                return (this.ColumnPositions[0] != EditConsts.DefaultColumnMarginPosition);
            }
            return true;
        }

        public bool ShouldSerializePenColor()
        {
            return (this.PenColor != XmlColorInfo.SerializeColor(EditConsts.DefaultMarginForeColor));
        }

        public bool ShouldSerializePosition()
        {
            return (this.Position != EditConsts.DefaultMarginPosition);
        }

        [DefaultValue(false)]
        public bool AllowDrag
        {
            get
            {
                if (this.owner == null)
                {
                    return this.allowDrag;
                }
                return this.owner.AllowDrag;
            }
            set
            {
                this.allowDrag = value;
                if (this.owner != null)
                {
                    this.owner.AllowDrag = value;
                }
            }
        }

        public string ColumnPenColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.columnPenColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.ColumnPen.Color);
            }
            set
            {
                this.columnPenColor = value;
                if (this.owner != null)
                {
                    this.owner.ColumnPen.Color = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public int[] ColumnPositions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.columnPositions;
                }
                return this.owner.ColumnPositions;
            }
            set
            {
                this.columnPositions = value;
                if (this.owner != null)
                {
                    this.owner.ColumnPositions = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool ColumnsVisible
        {
            get
            {
                if (this.owner == null)
                {
                    return this.columnsVisible;
                }
                return this.owner.ColumnsVisible;
            }
            set
            {
                this.columnsVisible = value;
                if (this.owner != null)
                {
                    this.owner.ColumnsVisible = value;
                }
            }
        }

        public string PenColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.penColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.Pen.Color);
            }
            set
            {
                this.penColor = value;
                if (this.owner != null)
                {
                    this.owner.Pen.Color = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        [DefaultValue(1)]
        public float PenWidth
        {
            get
            {
                if (this.owner == null)
                {
                    return this.penWidth;
                }
                return this.owner.Pen.Width;
            }
            set
            {
                this.penWidth = value;
                if (this.owner != null)
                {
                    this.owner.Pen.Width = value;
                }
            }
        }

        public int Position
        {
            get
            {
                if (this.owner == null)
                {
                    return this.position;
                }
                return this.owner.Position;
            }
            set
            {
                this.position = value;
                if (this.owner != null)
                {
                    this.owner.Position = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool ShowHints
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showHints;
                }
                return this.owner.ShowHints;
            }
            set
            {
                this.showHints = value;
                if (this.owner != null)
                {
                    this.owner.ShowHints = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool Visible
        {
            get
            {
                if (this.owner == null)
                {
                    return this.visible;
                }
                return this.owner.Visible;
            }
            set
            {
                this.visible = value;
                if (this.owner != null)
                {
                    this.owner.Visible = value;
                }
            }
        }
    }
}

