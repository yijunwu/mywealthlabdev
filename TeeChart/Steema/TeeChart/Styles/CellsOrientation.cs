namespace Steema.TeeChart.Styles
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct CellsOrientation
    {
        public int InitX;
        public int EndX;
        public int IncX;
        public int InitZ;
        public int EndZ;
        public int IncZ;
    }
}

