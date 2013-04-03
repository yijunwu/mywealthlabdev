namespace WealthLab.ChartDrawingObjects
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;

    public class CDOCallout : CDOPointBased
    {
        protected ChartDrawingObjectHandle _pointerHandle;
        private bool bool_2;
        private CalloutSettings calloutSettings_0;
        private Color color_0;
        private Color color_1;
        private static DrawingObjectHelper drawingObjectHelper_0 = new CalloutHelper();
        private Font font_1;
        private int int_0;
        private int int_1;
        private Point point_0;
        private Point point_1;
        private Point point_2;
        private Point point_3;
        private Rectangle rectangle_0;
        private string string_2;
        private static StringFormat stringFormat_0;

        public CDOCallout()
        {
            this.int_1 = 0x18;
        }

        public CDOCallout(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
            this.int_1 = 0x18;
            this._pointerHandle = base.CreateHandle();
            this._pointerHandle.Date = dateTime_0;
            this._pointerHandle.Value = value;
        }

        public override void ChangeSettings(UserControl userControl_0)
        {
            CalloutSettings settings = userControl_0 as CalloutSettings;
            this.Text = settings.Text;
            this.TextColor = settings.TextColor;
            this.TextFont = settings.TextFont;
            this.BackColor = settings.BackColor;
            this.Transparency = settings.Transparency;
        }

        public override UserControl GetSettingsUI()
        {
            if (this.calloutSettings_0 == null)
            {
                this.calloutSettings_0 = new CalloutSettings();
            }
            this.calloutSettings_0.Text = this.Text;
            this.calloutSettings_0.TextColor = this.TextColor;
            this.calloutSettings_0.TextFont = this.TextFont;
            this.calloutSettings_0.BackColor = this.BackColor;
            this.calloutSettings_0.Transparency = this.Transparency;
            return this.calloutSettings_0;
        }

        protected override bool IsMouseOver(int int_2, int int_3)
        {
            return (this.rectangle_0.Contains(int_2, int_3) || (((int_2 > (this.point_0.X - Chart.PixelSensitivity)) && (int_2 < (this.point_0.X + Chart.PixelSensitivity))) && ((int_3 > (this.point_0.Y - Chart.PixelSensitivity)) && (int_3 < (this.point_0.Y + Chart.PixelSensitivity)))));
        }

        private Point[] method_0()
        {
            if (this._pointerHandle.X < this.CenterPoint.X)
            {
                this.point_2.X = this.rectangle_0.X + Convert.ToInt32((float) (this.int_1 * 0.5f));
                this.point_3.X = this.point_2.X + 20;
            }
            else
            {
                this.point_2.X = (this.rectangle_0.X + this.rectangle_0.Width) - Convert.ToInt32((float) (this.int_1 * 0.5f));
                this.point_3.X = this.point_2.X - 20;
            }
            if (this._pointerHandle.Y < this.CenterPoint.Y)
            {
                this.point_2.Y = this.rectangle_0.Y;
                this.point_3.Y = this.rectangle_0.Y;
            }
            else
            {
                this.point_2.Y = this.rectangle_0.Y + this.rectangle_0.Height;
                this.point_3.Y = this.rectangle_0.Y + this.rectangle_0.Height;
            }
            if (this._pointerHandle.X < (this.rectangle_0.X - 60))
            {
                this.point_2.X = this.rectangle_0.X;
                this.point_3.X = this.rectangle_0.X;
                this.point_2.Y = this.CenterPoint.Y - 10;
                this.point_3.Y = this.CenterPoint.Y + 10;
            }
            if (this._pointerHandle.X > ((this.rectangle_0.X + this.rectangle_0.Width) + 60))
            {
                this.point_2.X = this.rectangle_0.X + this.rectangle_0.Width;
                this.point_3.X = this.rectangle_0.X + this.rectangle_0.Width;
                this.point_2.Y = this.CenterPoint.Y - 10;
                this.point_3.Y = this.CenterPoint.Y + 10;
            }
            return new Point[] { this.point_3, this.point_0, this.point_2 };
        }

        protected override void OnSelected(int int_2, int int_3)
        {
            if (this.rectangle_0.Contains(this.point_0))
            {
                this.bool_2 = false;
            }
            if (((this.point_0.X == 0) && (this.point_0.Y == 0)) || !this.bool_2)
            {
                this.point_0.X = this.CenterPoint.X;
                this.point_0.Y = (this.rectangle_0.Y + this.rectangle_0.Height) + 5;
            }
            this._pointerHandle.Bar = base.ConvertXToBar(this.point_0.X);
            this._pointerHandle.Value = base.Pane.ConvertYToValue(this.point_0.Y);
            this.bool_2 = !this.rectangle_0.Contains(this.point_0);
        }

        public bool PromptUserForSettings(ISettingsHost host)
        {
            UserControl settingsUI = this.GetSettingsUI();
            DrawingObjectProperties properties = new DrawingObjectProperties {
                Text = drawingObjectHelper_0.FriendlyName + " Properties"
            };
            properties.AddUserControl(settingsUI);
            if (properties.ShowDialog() == DialogResult.OK)
            {
                this.ChangeSettings(settingsUI);
                this.WriteSettings(host);
                return true;
            }
            return false;
        }

        protected override void Read(BinaryReader binaryReader_0)
        {
            base.Read(binaryReader_0);
            this.Text = binaryReader_0.ReadString();
            int argb = binaryReader_0.ReadInt32();
            this.TextColor = Color.FromArgb(argb);
            string familyName = binaryReader_0.ReadString();
            double num2 = binaryReader_0.ReadDouble();
            FontStyle style = (FontStyle) binaryReader_0.ReadInt32();
            this.TextFont = new Font(familyName, (float) num2, style);
            argb = binaryReader_0.ReadInt32();
            this.BackColor = Color.FromArgb(argb);
            this.Transparency = binaryReader_0.ReadInt32();
            base._mover = base.Handles[0];
            this._pointerHandle = base.Handles[1];
            this.bool_2 = true;
        }

        public override void ReadSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            this.TextColor = host.Get(str + "TextColor", Color.Black);
            this.TextFont = host.Get(str + "TextFont", new Font("Arial", 10f));
            this.BackColor = host.Get(str + "BackColor", Color.Yellow);
            this.Transparency = host.Get(str + "Transparency", 0);
            if ((this.Text == null) || (this.Text == ""))
            {
                this.PromptUserForSettings(host);
                if ((this.Text == "") || (this.Text == null))
                {
                    throw new Exception("CDOCallout.ReadSettings  No Text Entered.");
                }
            }
        }

        protected override void Render(Graphics graphics_0)
        {
            if (base._mover.Bar != -1)
            {
                SmoothingMode smoothingMode = graphics_0.SmoothingMode;
                graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
                this.rectangle_0.X = base._mover.X;
                this.rectangle_0.Y = base._mover.Y;
                this.rectangle_0.Width = 180;
                if (stringFormat_0 == null)
                {
                    stringFormat_0 = new StringFormat();
                    stringFormat_0.Alignment = StringAlignment.Center;
                    stringFormat_0.Trimming = StringTrimming.None;
                    stringFormat_0.LineAlignment = StringAlignment.Center;
                }
                SizeF ef = graphics_0.MeasureString(this.Text, this.TextFont, this.rectangle_0.Width, stringFormat_0);
                this.rectangle_0.Height = Convert.ToInt32((double) (Math.Ceiling((double) ef.Height) + this.int_1));
                this.rectangle_0.Width = Convert.ToInt32((double) (Math.Ceiling((double) ef.Width) + this.int_1));
                RoundedRectangle rectangle = new RoundedRectangle(this.rectangle_0, this.int_1);
                GraphicsPath path = rectangle.Path;
                Pen pen = new Pen(Color.Black);
                graphics_0.DrawPath(pen, path);
                SolidBrush brush = new SolidBrush(base.ColorWithTransparency(this.BackColor, this.Transparency));
                graphics_0.FillPath(brush, path);
                if ((this.bool_2 && (this._pointerHandle.X != 0)) && (this._pointerHandle.Y != 0))
                {
                    this.point_0.X = this._pointerHandle.X;
                    this.point_0.Y = this._pointerHandle.Y;
                    Point[] points = this.method_0();
                    graphics_0.DrawPolygon(pen, points);
                    graphics_0.FillPolygon(brush, points);
                }
                SolidBrush brush2 = new SolidBrush(this.TextColor);
                graphics_0.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics_0.DrawString(this.Text, this.TextFont, brush2, rectangle.InnerRectangle, stringFormat_0);
                brush2.Dispose();
                brush.Dispose();
                this._origin.X = base._mover.X;
                this._origin.Y = base._mover.Y;
                graphics_0.SmoothingMode = smoothingMode;
            }
        }

        protected override void Write(BinaryWriter binaryWriter_0)
        {
            base.Write(binaryWriter_0);
            binaryWriter_0.Write(this.Text);
            binaryWriter_0.Write(this.TextColor.ToArgb());
            binaryWriter_0.Write(this.TextFont.Name);
            binaryWriter_0.Write((double) this.TextFont.Size);
            binaryWriter_0.Write((int) this.TextFont.Style);
            binaryWriter_0.Write(this.BackColor.ToArgb());
            binaryWriter_0.Write(this.Transparency);
        }

        public override void WriteSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            host.Set(str + "TextColor", this.TextColor);
            host.Set(str + "TextFont", this.TextFont);
            host.Set(str + "BackColor", this.BackColor);
            host.Set(str + "Transparency", this.Transparency);
        }

        public Color BackColor
        {
            get
            {
                return this.color_1;
            }
            set
            {
                this.color_1 = value;
            }
        }

        public Point CenterPoint
        {
            get
            {
                this.point_1.X = Convert.ToInt32((double) (this.rectangle_0.X + (this.rectangle_0.Width * 0.5)));
                this.point_1.Y = Convert.ToInt32((double) (this.rectangle_0.Y + (this.rectangle_0.Height * 0.5)));
                return this.point_1;
            }
        }

        protected override DrawingObjectHelper Helper
        {
            get
            {
                return drawingObjectHelper_0;
            }
        }

        public string Text
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        public Color TextColor
        {
            get
            {
                return this.color_0;
            }
            set
            {
                this.color_0 = value;
            }
        }

        public Font TextFont
        {
            get
            {
                return this.font_1;
            }
            set
            {
                this.font_1 = value;
            }
        }

        public int Transparency
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }
    }
}

