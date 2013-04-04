namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct PointDouble
    {
        private double x;
        private double y;
        public static readonly PointDouble Empty;
        public PointDouble(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }
        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }
        static PointDouble()
        {
            Empty = new PointDouble();
        }

        public static Point Round(PointDouble value)
        {
            return new Point(Utils.Round(value.X), Utils.Round(value.Y));
        }

        public static Point[] Round(PointDouble[] value)
        {
            Point[] pointArray = new Point[value.Length];
            for (int i = 0; i < pointArray.Length; i++)
            {
                pointArray[i] = Round(value[i]);
            }
            return pointArray;
        }

        public static PointF RoundF(PointDouble value)
        {
            return new PointF((float) value.X, (float) value.Y);
        }

        public static PointF[] RoundF(PointDouble[] value)
        {
            PointF[] tfArray = new PointF[value.Length];
            for (int i = 0; i < tfArray.Length; i++)
            {
                tfArray[i] = RoundF(value[i]);
            }
            return tfArray;
        }

        public bool IsEmpty
        {
            get
            {
                return ((this.x == 0.0) && (this.y == 0.0));
            }
        }
        public static PointDouble[] FromPoint(Point[] point)
        {
            PointDouble[] numArray = new PointDouble[point.Length];
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = FromPoint(point[i]);
            }
            return numArray;
        }

        public static PointDouble FromPoint(Point point)
        {
            return new PointDouble((double) point.X, (double) point.Y);
        }
    }
}

