namespace Steema.TeeChart
{
    using System;
    using System.Drawing;

    public class Footer : Title
    {
        public Footer() : this(null)
        {
        }

        public Footer(Chart c) : base(c)
        {
            base.Font.Color = Color.Red;
            base.Font.Brush.defaultColor = Color.Red;
            base.onTop = false;
        }

        public Footer(Chart c, bool startVisible) : this(c)
        {
            base.defaultVisible = startVisible;
            base.Visible = base.defaultVisible;
        }
    }
}

