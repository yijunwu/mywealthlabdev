namespace Steema.TeeChart.Styles
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct GaugePointerStyles
    {
        public int Value;
        public static GaugePointerStyles Hand;
        public static GaugePointerStyles Center;
        public static GaugePointerStyles Tick;
        public static GaugePointerStyles MinorTick;
        public static GaugePointerStyles ColorLine;
        public GaugePointerStyles(int value)
        {
            this.Value = value;
        }

        public static implicit operator GaugePointerStyles(PointerStyles PointerStyle)
        {
            return new GaugePointerStyles((int) PointerStyle);
        }

        public static implicit operator GaugePointerStyles(int PointerStyle)
        {
            return new GaugePointerStyles(PointerStyle);
        }

        public static implicit operator int(GaugePointerStyles PointerStyle)
        {
            GaugePointerStyles styles = new GaugePointerStyles(PointerStyle.Value);
            return styles.Value;
        }

        public static implicit operator PointerStyles(GaugePointerStyles PointerStyle)
        {
            if (PointerStyle.Value < 14)
            {
                return (PointerStyles) PointerStyle.Value;
            }
            return PointerStyles.Nothing;
        }

        public override bool Equals(object obj)
        {
            GaugePointerStyles styles = (GaugePointerStyles) obj;
            if (styles == 0)
            {
                return false;
            }
            return (base.Equals(obj) && (this.Value == styles.Value));
        }

        public bool Equals(GaugePointerStyles p)
        {
            return (base.Equals(p) && (this.Value == p.Value));
        }

        public static bool operator ==(GaugePointerStyles a, GaugePointerStyles b)
        {
            return (object.ReferenceEquals(a, b) || (((a != 0) && (b != 0)) && (a.Value == b.Value)));
        }

        public static bool operator !=(GaugePointerStyles a, GaugePointerStyles b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() ^ this.Value);
        }

        static GaugePointerStyles()
        {
            Hand = new GaugePointerStyles(15);
            Center = new GaugePointerStyles(0x10);
            Tick = new GaugePointerStyles(0x11);
            MinorTick = new GaugePointerStyles(0x12);
            ColorLine = new GaugePointerStyles(0x13);
        }
    }
}

