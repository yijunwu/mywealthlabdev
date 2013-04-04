namespace Steema.TeeChart
{
    using Steema.TeeChart.Styles;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ChartClickedPart
    {
        public ChartClickedPartStyle Part;
        public int PointIndex;
        public Series ASeries;
        public Axis AAxis;
    }
}

