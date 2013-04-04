namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct TempLevel
    {
        public int Count;
        public int Allocated;
        public LevelLine[] Line;
        public double UpToValue;
        public System.Drawing.Color Color;
        public System.Drawing.Color LineColor;
        public ContourLevel Level;
        public ChartPen Pen;
    }
}

