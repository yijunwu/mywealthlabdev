namespace WealthLab.ChartDrawingObjects
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;

    public class CDOFibArcs : CDOLineBased
    {
        private bool bool_5;
        private static DrawingObjectHelper drawingObjectHelper_0 = new FibArcsHelper();
        private FibSettings fibSettings_0;
        private string string_2;

        public CDOFibArcs()
        {
        }

        public CDOFibArcs(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
        }

        public override void ChangeSettings(UserControl userControl_0)
        {
            FibSettings settings = userControl_0 as FibSettings;
            base.Color = settings.Color;
            base.Style = settings.Style;
            base.Width = settings.DrawingObjectWidth;
            base.SnapToValue = settings.SnapToValue;
            this.string_2 = settings.RetracementLevels;
            this.bool_5 = settings.ShowLevels;
        }

        public override UserControl GetSettingsUI()
        {
            if (this.fibSettings_0 == null)
            {
                this.fibSettings_0 = new FibSettings();
            }
            this.fibSettings_0.Color = base.Color;
            this.fibSettings_0.DrawingObjectWidth = base.Width;
            this.fibSettings_0.Style = base.Style;
            this.fibSettings_0.SnapToValue = base.SnapToValue;
            this.fibSettings_0.RetracementLevels = this.string_2;
            this.fibSettings_0.ShowLevels = this.bool_5;
            return this.fibSettings_0;
        }

        protected override void Read(BinaryReader binaryReader_0)
        {
            base.Read(binaryReader_0);
            int argb = binaryReader_0.ReadInt32();
            base.Color = Color.FromArgb(argb);
            base.Width = binaryReader_0.ReadInt32();
            base.Style = (LineStyle) binaryReader_0.ReadInt32();
            base.SnapToValue = binaryReader_0.ReadBoolean();
            this.string_2 = binaryReader_0.ReadString();
            this.bool_5 = binaryReader_0.ReadBoolean();
            base._startHandle = base.Handles[0];
            base._endHandle = base.Handles[1];
            base._mover = base.Handles[2];
        }

        public override void ReadSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            base.Color = host.Get(str + "Color", Color.Red);
            base.Style = (LineStyle) Enum.Parse(typeof(LineStyle), host.Get(str + "Style", "Solid"));
            base.Width = host.Get(str + "Width", 1);
            base.SnapToValue = host.Get(str + "SnapToValue", false);
            this.string_2 = host.Get(str + "RetracementLevels", Class3.smethod_0());
            this.bool_5 = host.Get(str + "ShowLevels", true);
        }

        protected override void Render(Graphics graphics_0)
        {
            if ((base.LeftHandle.Bar != -1) && (base.RightHandle.Bar != -1))
            {
                SmoothingMode smoothingMode = graphics_0.SmoothingMode;
                graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    Pen pen = new Pen(base.Color, (float) base.Width);
                    ChartRenderer.SetPenStyle(pen, base.Style);
                    pen.Width = base.Width;
                    int x = base.LeftHandle.X;
                    int y = base.LeftHandle.Y;
                    int num3 = base.RightHandle.X;
                    int num4 = base.RightHandle.Y;
                    if ((y != -2147483648) && (num4 != -2147483648))
                    {
                        graphics_0.DrawLine(pen, x, y, num3, num4);
                    }
                    else
                    {
                        x = base.LeftHandle.X;
                        y = base.LeftHandle.Y;
                        num3 = base.RightHandle.X;
                        num4 = base.RightHandle.Y;
                        graphics_0.DrawLine(pen, x, y, num3, num4);
                    }
                    pen.Width = 1f;
                    string[] strArray = this.string_2.Split(new char[] { '|' });
                    int length = strArray.Length;
                    int num1 = base.LeftHandle.X;
                    float num17 = base.LeftHandle.Y;
                    float num19 = base.RightHandle.X;
                    float num16 = base.RightHandle.Y;
                    for (int i = 0; i < length; i++)
                    {
                        float num18;
                        float num12 = Class3.smethod_1(strArray[i]);
                        float num15 = num12 / 100f;
                        if (num16 < num17)
                        {
                            num18 = num17 - num16;
                            num15 = num18 * num15;
                            num15 = num16 + num15;
                        }
                        else
                        {
                            num18 = num16 - num17;
                            num15 = num18 * num15;
                            num15 = num16 - num15;
                        }
                        x = base.LeftHandle.X;
                        y = base.LeftHandle.Y;
                        num3 = base.RightHandle.X;
                        num4 = base.RightHandle.Y;
                        int num10 = Math.Abs((int) (x - num3));
                        int num11 = Math.Abs((int) (y - num4));
                        int num13 = (int) Math.Round((double) (Math.Sqrt((double) ((num10 * num10) + (num11 * num11))) * (num12 / 100f)));
                        int num5 = num3 - num13;
                        int num6 = num4 - num13;
                        int width = (num3 + num13) - num5;
                        int height = (num4 + num13) - num6;
                        if (base.LeftHandle.Y < base.RightHandle.Y)
                        {
                            graphics_0.DrawArc(pen, num5, num6, width, height, 180, 180);
                        }
                        else
                        {
                            graphics_0.DrawArc(pen, num5, num6, width, height, 0, 180);
                        }
                        if (this.bool_5)
                        {
                            string s = string.Concat(new object[] { ' ', num12.ToString("N1"), "% ", base.Pane.ConvertYToValue(Convert.ToInt32(num15)).ToString("N2") });
                            graphics_0.DrawString(s, base.HandleFont, new SolidBrush(base.Color), num19, num15);
                        }
                    }
                    pen.Dispose();
                    graphics_0.SmoothingMode = smoothingMode;
                }
                catch (Exception)
                {
                }
            }
        }

        protected override void Write(BinaryWriter binaryWriter_0)
        {
            base.Write(binaryWriter_0);
            binaryWriter_0.Write(base.Color.ToArgb());
            binaryWriter_0.Write(base.Width);
            binaryWriter_0.Write((int) base.Style);
            binaryWriter_0.Write(base.SnapToValue);
            binaryWriter_0.Write(this.string_2);
            binaryWriter_0.Write(this.bool_5);
        }

        public override void WriteSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            host.Set(str + "Color", base.Color);
            host.Set(str + "Style", base.Style.ToString());
            host.Set(str + "Width", base.Width);
            host.Set(str + "SnapToValue", base.SnapToValue);
            host.Set(str + "RetracementLevels", this.string_2);
            host.Set(str + "ShowLevels", this.bool_5);
        }

        protected override DrawingObjectHelper Helper
        {
            get
            {
                return drawingObjectHelper_0;
            }
        }
    }
}

