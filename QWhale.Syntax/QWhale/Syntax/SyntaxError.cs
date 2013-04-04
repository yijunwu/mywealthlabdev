namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public class SyntaxError : ISyntaxError, ICloneable
    {
        private string description;
        private string name;
        private IRange range;

        public SyntaxError()
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.range = new QWhale.Common.Range(0, 0, 0, 0);
        }

        public SyntaxError(Point position)
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.range = new QWhale.Common.Range(0, 0, 0, 0);
            this.range.StartPoint = position;
            this.range.EndPoint = position;
        }

        public SyntaxError(Point position, string name, string description) : this(position)
        {
            this.name = name;
            this.description = description;
            this.range.EndPoint = new Point(position.X + name.Length, position.Y);
        }

        public virtual object Clone()
        {
            return new SyntaxError { Name = this.name, Description = this.description, Range = (IRange) this.range.Clone() };
        }

        protected virtual void OnDescriptionChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnPositionChanged()
        {
        }

        protected virtual void OnRangeChanged()
        {
        }

        protected virtual void OnSizeChanged()
        {
        }

        public override string ToString()
        {
            if (!(this.name != string.Empty))
            {
                return base.ToString();
            }
            return this.name;
        }

        public virtual string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    this.OnDescriptionChanged();
                }
            }
        }

        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.OnNameChanged();
                }
            }
        }

        public virtual Point Position
        {
            get
            {
                return this.range.StartPoint;
            }
            set
            {
                if (this.range.StartPoint != value)
                {
                    this.range.StartPoint = value;
                    this.range.EndPoint = new Point(value.X + this.name.Length, value.Y);
                    this.OnPositionChanged();
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

        public virtual System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size(this.range.EndPoint.X - this.range.StartPoint.X, this.range.EndPoint.Y - this.range.StartPoint.Y);
            }
            set
            {
                this.range.EndPoint = new Point(this.range.StartPoint.X + value.Width, this.range.StartPoint.Y + value.Height);
                this.OnSizeChanged();
            }
        }
    }
}

