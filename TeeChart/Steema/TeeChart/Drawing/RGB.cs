namespace Steema.TeeChart.Drawing
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct RGB
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;
    }
}

