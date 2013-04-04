namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(Marker), "ToolsIcons.Marker.bmp"), Description("Displays custom text at any location inside Chart.")]
    public class Marker : Annotation
    {
        private readonly double barfontSpace;
        private bool centered;
        private readonly double dotfontSpace;
        public Rectangle IRectangle;
        private readonly double textWidth;
        private bool usePalette;

        public Marker() : this(null, "", 11, AnnotationPositions.LeftBottom, StringAlignment.Center, Color.Black, Color.White)
        {
            this.usePalette = false;
        }

        public Marker(Chart c) : this(c, "", 11, AnnotationPositions.LeftBottom, StringAlignment.Center, Color.Black, Color.White)
        {
            this.usePalette = false;
        }

        public Marker(string text, int fontsize, AnnotationPositions pos, StringAlignment align, Color color1, Color color2) : this(null, text, fontsize, pos, align, color1, color2)
        {
        }

        public Marker(Chart c, string text, int fontsize, AnnotationPositions pos, StringAlignment align, Color color1, Color color2) : base(c)
        {
            this.barfontSpace = 0.4;
            this.dotfontSpace = 0.33333333333333331;
            this.textWidth = 0.8;
            this.usePalette = true;
            this.centered = false;
            base.Shape.Font.UsePrivateFont = true;
            base.Shape.Font.Name = "DS-Digital";
            base.TextAlign = align;
            base.Text = text;
            base.Shape.Font.Size = fontsize;
            base.Shape.Pen.Visible = false;
            base.Shape.Font.Color = color1;
            base.Shape.Color = color2;
            base.Shape.Shadow.Visible = false;
            base.Position = pos;
            if (base.Position == ~AnnotationPositions.LeftTop)
            {
                this.centered = true;
            }
        }

        protected override void CalcTempPosition(out int x, out int y, int tmpW, int tmpH, int horizMargin, int vertMargin)
        {
            int num = (this.IRectangle.Right - tmpW) - (2 * horizMargin);
            int num2 = (this.IRectangle.Bottom - tmpH) - (4 * vertMargin);
            if (this.Centered)
            {
                base.Height = tmpH;
                base.Width = this.IRectangle.Width - (2 * horizMargin);
                x = this.IRectangle.Left + (2 * horizMargin);
                int num3 = Utils.Round((float) ((this.IRectangle.Height - tmpH) / 2));
                y = this.IRectangle.Top + num3;
            }
            else
            {
                switch (base.Position)
                {
                    case AnnotationPositions.LeftTop:
                        x = this.IRectangle.Left + (2 * horizMargin);
                        y = this.IRectangle.Top + (4 * vertMargin);
                        return;

                    case AnnotationPositions.LeftBottom:
                        x = this.IRectangle.Left + (2 * horizMargin);
                        y = num2;
                        return;

                    case AnnotationPositions.RightTop:
                        x = num;
                        y = this.IRectangle.Top + (4 * vertMargin);
                        return;
                }
                x = num;
                y = num2;
            }
        }

        protected override int CalcTempWidth(string tmp, out int NumLines)
        {
            if (base.Shape.Font.UsePrivateFont)
            {
                return this.MultiLineTextWidth(tmp, out NumLines);
            }
            return base.CalcTempWidth(tmp, out NumLines);
        }

        protected override void DrawString(Graphics3D g, int x, int y, int t, int tmpHeight, string[] s)
        {
            if (!base.Shape.Font.UsePrivateFont)
            {
                base.DrawString(g, x, y, t, tmpHeight, s);
            }
            else
            {
                int num = Utils.Round((double) (g.TextWidth("W") * this.textWidth));
                int num2 = Utils.Round((double) (g.FontHeight * 0.5));
                int num3 = 0;
                int num4 = 0;
                int num5 = 0;
                string name = g.Font.Name;
                if (name != null)
                {
                    if (!(name == "DS-Digital"))
                    {
                        if (name == "Elektra")
                        {
                            num3 = Utils.Round((double) (g.TextWidth(",") * this.dotfontSpace));
                            num4 = Utils.Round((double) (g.TextWidth("'") * this.dotfontSpace));
                        }
                    }
                    else
                    {
                        num3 = Utils.Round((double) (g.TextWidth(",") * this.barfontSpace));
                    }
                }
                int num6 = x - num3;
                foreach (char ch in s[t - 1])
                {
                    string str4;
                    if (ch == '.')
                    {
                        num6 -= num3;
                    }
                    else
                    {
                        if (ch != ',')
                        {
                            goto Label_0148;
                        }
                        string str3 = g.Font.Name;
                        if (str3 != null)
                        {
                            if (!(str3 == "DS-Digital"))
                            {
                                if (str3 == "Elektra")
                                {
                                    goto Label_0135;
                                }
                            }
                            else
                            {
                                num6 -= num3;
                            }
                        }
                    }
                    goto Label_017F;
                Label_0135:
                    ch = '\'';
                    num6 -= num4;
                    num5 = (y + num2) + 1;
                    goto Label_017F;
                Label_0148:
                    if (((ch == '1') && ((str4 = g.Font.Name) != null)) && (str4 == "DS-Digital"))
                    {
                        num6 += Utils.Round((double) (num * this.barfontSpace));
                    }
                Label_017F:
                    if (ch == '\'')
                    {
                        g.TextOut(num6, num5, ch.ToString());
                    }
                    else
                    {
                        g.TextOut(num6, y, ch.ToString());
                    }
                    if (ch != '.')
                    {
                        goto Label_0204;
                    }
                    string str5 = g.Font.Name;
                    if (str5 != null)
                    {
                        if (!(str5 == "DS-Digital"))
                        {
                            if (str5 == "Elektra")
                            {
                                goto Label_01F3;
                            }
                        }
                        else
                        {
                            num6 += Utils.Round((float) num3);
                        }
                    }
                    continue;
                Label_01F3:
                    num6 += Utils.Round((float) num3);
                    continue;
                Label_0204:
                    switch (ch)
                    {
                        case ',':
                        {
                            string str6;
                            if (((str6 = g.Font.Name) != null) && (str6 == "DS-Digital"))
                            {
                                num6 += Utils.Round((float) num3);
                            }
                            continue;
                        }
                        case '\'':
                        {
                            string str7;
                            if (((str7 = g.Font.Name) != null) && (str7 == "Elektra"))
                            {
                                num6 += Utils.Round((float) num4);
                            }
                            continue;
                        }
                        default:
                        {
                            if (ch != '1')
                            {
                                goto Label_02C4;
                            }
                            string str8 = g.Font.Name;
                            if (str8 != null)
                            {
                                if (!(str8 == "DS-Digital"))
                                {
                                    if (str8 == "Elektra")
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    num6 = (num6 + num) - Utils.Round((double) (num * this.barfontSpace));
                                }
                            }
                            continue;
                        }
                    }
                    num6 += num;
                    continue;
                Label_02C4:
                    num6 += num;
                }
            }
        }

        private int MultiLineTextWidth(string s, out int numLines)
        {
            int index = s.IndexOf('\n');
            int num2 = 0;
            numLines = 0;
            while (index > 0)
            {
                num2 = Utils.Round((float) Math.Max(num2, this.TextWidth(s.Substring(1, index - 1))));
                numLines++;
                s = s.Remove(1, index);
                index = s.IndexOf('\n');
            }
            if (s != "")
            {
                num2 = Utils.Round((float) Math.Max(num2, this.TextWidth(s)));
                numLines++;
            }
            return num2;
        }

        public int TextWidth()
        {
            int num;
            if (base.Shape.Font.UsePrivateFont)
            {
                return this.MultiLineTextWidth(base.Text, out num);
            }
            return base.chart.MultiLineTextWidth(base.Text, out num);
        }

        private int TextWidth(string text)
        {
            Graphics3D graphicsd = base.Chart.Graphics3D;
            int num = 0;
            int num2 = Utils.Round((double) (graphicsd.TextWidth("W") * this.textWidth));
            int num3 = 0;
            int num4 = 0;
            string name = graphicsd.Font.Name;
            if (name != null)
            {
                if (!(name == "DS-Digital"))
                {
                    if (name == "Elektra")
                    {
                        num3 = Utils.Round((double) (graphicsd.TextWidth(",") * this.dotfontSpace));
                        num4 = Utils.Round((double) (graphicsd.TextWidth("'") * this.dotfontSpace));
                    }
                }
                else
                {
                    num3 = Utils.Round((double) (graphicsd.TextWidth(",") * this.barfontSpace));
                }
            }
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (ch != '.')
                {
                    goto Label_010A;
                }
                string str2 = graphicsd.Font.Name;
                if (str2 != null)
                {
                    if (!(str2 == "DS-Digital"))
                    {
                        if (str2 == "Elektra")
                        {
                            goto Label_00F7;
                        }
                    }
                    else
                    {
                        num += num3;
                    }
                }
                continue;
            Label_00F7:
                num += Utils.Round((double) (((double) num3) / this.barfontSpace));
                continue;
            Label_010A:
                if (ch != ',')
                {
                    goto Label_0158;
                }
                string str3 = graphicsd.Font.Name;
                if (str3 != null)
                {
                    if (!(str3 == "DS-Digital"))
                    {
                        if (str3 == "Elektra")
                        {
                            goto Label_0144;
                        }
                    }
                    else
                    {
                        num += num3;
                    }
                }
                continue;
            Label_0144:
                num += Utils.Round((double) (((double) num4) / this.barfontSpace));
                continue;
            Label_0158:
                num += num2;
            }
            return num;
        }

        public bool Centered
        {
            get
            {
                return this.centered;
            }
            set
            {
                base.Shape.CustomPosition = false;
                this.centered = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.MarkerTool;
            }
        }

        public bool UsePalette
        {
            get
            {
                return this.usePalette;
            }
            set
            {
                this.usePalette = value;
            }
        }
    }
}

