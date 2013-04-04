namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public class SyntaxAttribute : ISyntaxAttribute, ICloneable
    {
        private object _value;
        private string name;
        private Point position;

        public SyntaxAttribute()
        {
            this.name = string.Empty;
            this.position = Point.Empty;
        }

        public SyntaxAttribute(Point position, string name, object value)
        {
            this.name = string.Empty;
            this.position = Point.Empty;
            this.position = position;
            this.name = name;
            this._value = value;
        }

        public virtual object Clone()
        {
            return new SyntaxAttribute { Name = this.name, Value = this.Value };
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnPositionChanged()
        {
        }

        protected virtual void OnValueChanged()
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

        public virtual Point EndPosition
        {
            get
            {
                return new Point(this.position.X + ((this._value != null) ? this._value.ToString().Length : 0), this.position.Y);
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

        public virtual IRange Range
        {
            get
            {
                return new QWhale.Common.Range(this.position, this.EndPosition);
            }
        }

        public virtual object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if (this._value != value)
                {
                    this._value = value;
                    this.OnValueChanged();
                }
            }
        }
    }
}

