namespace Steema.TeeChart
{
    using System;
    using System.Drawing;

    public class Header : Title
    {
        public Header() : this(null)
        {
        }

        public Header(Chart c) : base(c)
        {
            base.Color = Utils.FromArgb(0xc0, 0xc0, 0xc0);
            base.Font.Brush.Color = Utils.FromArgb(0, 0, 0x80);
            base.Font.Brush.defaultColor = Utils.FromArgb(0, 0, 0x80);
            base.Pen.Visible = true;
            base.Pen.Color = Utils.FromArgb(0, 0, 0);
            base.Transparency = 0;
            base.Shadow.Size = new Size(3, 3);
            base.Shadow.Transparency = 0;
        }

        public Header(Chart c, bool startVisible) : this(c)
        {
            base.defaultVisible = startVisible;
            base.Visible = base.defaultVisible;
        }
    }
}

