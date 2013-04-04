namespace Steema.TeeChart.Drawing
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Triangle2D
    {
        public Point p0;
        public Point p1;
        public Point p2;
    }
}

