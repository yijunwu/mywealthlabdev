namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using QWhale.Editor.TextSource.Serialization;
    using System;
    using System.ComponentModel;

    public class LineStyle : BookMark, ILineStyle, IBookMark
    {
        private int priority;
        private IRange range;

        public LineStyle()
        {
        }

        public LineStyle(int line) : base(line)
        {
        }

        public LineStyle(int line, int ch, int index) : base(line, ch, index)
        {
        }

        public LineStyle(int line, int ch, int index, int priority, IRange range) : this(line, ch, index)
        {
            this.priority = priority;
            this.range = range;
        }

        public virtual void Assign(ILineStyle source)
        {
            base.Assign(source);
            this.priority = source.Priority;
            this.range = source.Range;
        }

        protected virtual void OnPriorityChanged()
        {
        }

        protected virtual void OnRangeChanged()
        {
        }

        public virtual int Priority
        {
            get
            {
                return this.priority;
            }
            set
            {
                if (this.priority != value)
                {
                    this.priority = value;
                    this.OnPriorityChanged();
                }
            }
        }

        public virtual IRange Range
        {
            get
            {
                return this.range;
            }
            set
            {
                if (this.range != value)
                {
                    this.range = value;
                    this.OnRangeChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlLineStyleInfo(this);
            }
        }
    }
}

