namespace QWhale.Common
{
    using System;
    using System.Drawing;

    public class Range : IRange, ICloneable
    {
        private Point endPoint;
        private Point startPoint;

        public Range()
        {
        }

        public Range(Point startPoint, Point endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public Range(int x1, int y1, int x2, int y2)
        {
            this.startPoint = new Point(x1, y1);
            this.endPoint = new Point(x2, y2);
        }

        public virtual object Clone()
        {
            return new Range { StartPoint = this.startPoint, EndPoint = this.endPoint };
        }

        public static bool InsideRange(Point pt, Rectangle rect)
        {
            return InsideRange(pt, rect, false);
        }

        public static bool InsideRange(Point pt, Rectangle rect, bool checkMaxInt)
        {
            if ((pt.Y > rect.Bottom) || (pt.Y < rect.Top))
            {
                return false;
            }
            if (pt.Y == rect.Top)
            {
                if (pt.Y != rect.Bottom)
                {
                    return (pt.X >= rect.Left);
                }
                if (pt.X >= rect.Left)
                {
                    if (pt.X < rect.Right)
                    {
                        return true;
                    }
                    if (checkMaxInt && (pt.X == rect.Right))
                    {
                        return (pt.X == 0x7fffffff);
                    }
                }
                return false;
            }
            return ((pt.Y != rect.Bottom) || ((pt.X < rect.Right) || ((checkMaxInt && (pt.X == rect.Right)) && (pt.X == 0x7fffffff))));
        }

        protected virtual void OnEndPointChanged()
        {
        }

        protected virtual void OnStartPointChanged()
        {
        }

        public bool ShouldSerializeSize()
        {
            return false;
        }

        public static bool UpdatePos(int x, int y, int deltaX, int deltaY, ref Point pt, bool endPos)
        {
            int ch = pt.X;
            int ln = pt.Y;
            bool flag = UpdatePos(x, y, deltaX, deltaY, ref ch, ref ln, endPos);
            pt.X = ch;
            pt.Y = ln;
            return flag;
        }

        public static bool UpdatePos(int x, int y, int deltaX, int deltaY, ref int ch, ref int ln, bool endPos)
        {
            int num = ch;
            int num2 = ln;
            if (deltaY == 0)
            {
                bool flag = (deltaX < 0) || endPos;
                if ((ln == y) && (((ch >= x) && !flag) || ((ch > x) && flag)))
                {
                    if (ch != 0x7fffffff)
                    {
                        ch += deltaX;
                    }
                    ch = Math.Max(ch, 0);
                }
            }
            else
            {
                bool flag2 = endPos;
                if ((ln > y) || ((ln == y) && (((ch >= x) && !flag2) || ((ch > x) && flag2))))
                {
                    if (((ln == y) && (deltaY >= 0)) || (((ln + deltaY) == y) && (deltaY < 0)))
                    {
                        if (ch != 0x7fffffff)
                        {
                            ch += deltaX;
                        }
                        ch = Math.Max(ch, 0);
                    }
                    if (ln != 0x7fffffff)
                    {
                        ln += deltaY;
                    }
                    ln = Math.Max(ln, 0);
                }
            }
            if (ch == num)
            {
                return (ln != num2);
            }
            return true;
        }

        public virtual Point EndPoint
        {
            get
            {
                return this.endPoint;
            }
            set
            {
                if (this.endPoint != value)
                {
                    this.endPoint = value;
                    this.OnEndPointChanged();
                }
            }
        }

        public virtual bool IsEmpty
        {
            get
            {
                return ((this.startPoint.Y > this.endPoint.Y) || ((this.startPoint.Y == this.endPoint.Y) && (this.startPoint.X >= this.endPoint.X)));
            }
        }

        public virtual System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size(this.endPoint.X - this.startPoint.X, this.endPoint.Y - this.startPoint.Y);
            }
            set
            {
                if (this.Size != value)
                {
                    this.endPoint.X = this.startPoint.X + value.Width;
                    this.endPoint.Y = this.startPoint.Y + value.Height;
                    this.OnEndPointChanged();
                }
            }
        }

        public virtual Point StartPoint
        {
            get
            {
                return this.startPoint;
            }
            set
            {
                if (this.startPoint != value)
                {
                    this.startPoint = value;
                    this.OnStartPointChanged();
                }
            }
        }
    }
}

