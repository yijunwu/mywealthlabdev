namespace Steema.TeeChart.Drawing
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Point3D
    {
        public int X;
        public int Y;
        public int Z;
        public static Point3D Empty;
        static Point3D()
        {
            Empty = new Point3D();
        }

        public Point3D(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3D(Point point, int z)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Z = z;
        }
    }
}

