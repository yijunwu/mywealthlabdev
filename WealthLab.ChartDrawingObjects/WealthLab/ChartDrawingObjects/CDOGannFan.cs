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

    public class CDOGannFan : CDOLineBased
    {
        private bool bool_5;
        private bool bool_6;
        private double double_0;
        private static DrawingObjectHelper drawingObjectHelper_0 = new GannFanHelper();
        private GannFanSettings gannFanSettings_0;
        private ISettingsHost isettingsHost_0;
        private string string_2;

        public CDOGannFan()
        {
        }

        public CDOGannFan(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
        }

        public override void ChangeSettings(UserControl userControl_0)
        {
            GannFanSettings settings = userControl_0 as GannFanSettings;
            base.Color = settings.Color;
            base.Style = settings.Style;
            base.Width = settings.DrawingObjectWidth;
            base.SnapToValue = settings.SnapToValue;
            this.FanDown = settings.FanDown;
            this.FanUp = settings.FanUp;
            this.PriceUnit = Convert.ToDouble(settings.PriceUnit);
            this.Ratios = settings.Ratios;
        }

        public override UserControl GetSettingsUI()
        {
            if (this.gannFanSettings_0 == null)
            {
                this.gannFanSettings_0 = new GannFanSettings();
            }
            this.gannFanSettings_0.Color = base.Color;
            this.gannFanSettings_0.DrawingObjectWidth = base.Width;
            this.gannFanSettings_0.Style = base.Style;
            this.gannFanSettings_0.SnapToValue = base.SnapToValue;
            this.gannFanSettings_0.FanUp = this.FanUp;
            this.gannFanSettings_0.FanDown = this.FanDown;
            this.gannFanSettings_0.PriceUnit = this.PriceUnit.ToString();
            this.gannFanSettings_0.Ratios = this.Ratios;
            return this.gannFanSettings_0;
        }

        protected override void OnEndDrag(ChartDrawingObjectHandle handle)
        {
            base.OnEndDrag(handle);
            if ((this.isettingsHost_0 != null) && (this.gannFanSettings_0 == null))
            {
                base.PromptUserForSettings(this.isettingsHost_0, this.Helper);
            }
        }

        protected override void Read(BinaryReader binaryReader_0)
        {
            base.Read(binaryReader_0);
            int argb = binaryReader_0.ReadInt32();
            base.Color = Color.FromArgb(argb);
            base.Width = binaryReader_0.ReadInt32();
            base.Style = (LineStyle) binaryReader_0.ReadInt32();
            base.ExtendLeft = binaryReader_0.ReadBoolean();
            base.ExtendRight = binaryReader_0.ReadBoolean();
            base.SnapToValue = binaryReader_0.ReadBoolean();
            this.FanUp = binaryReader_0.ReadBoolean();
            this.FanDown = binaryReader_0.ReadBoolean();
            this.PriceUnit = binaryReader_0.ReadDouble();
            this.Ratios = binaryReader_0.ReadString();
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
            this.FanUp = host.Get(str + "FanUp", true);
            this.FanDown = host.Get(str + "FanDown", true);
            this.PriceUnit = host.Get(str + "PriceUnit", (double) 1.0);
            this.Ratios = host.Get(str + "Ratios", "1x8|1x4|1x3|1x2|1x1|2x1|3x1|4x1|8x1");
            this.isettingsHost_0 = host;
        }

        protected override void Render(Graphics graphics_0)
        {
            if ((base.LeftHandle.Bar != -1) && (base.RightHandle.Bar != -1))
            {
                SmoothingMode smoothingMode = graphics_0.SmoothingMode;
                graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    int bar = base._startHandle.Bar;
                    if (bar < base._startHandle.Bars.Count)
                    {
                        Pen pen = new Pen(base.Color, (float) base.Width);
                        ChartRenderer.SetPenStyle(pen, base.Style);
                        pen.Width = base.Width;
                        int x = base._startHandle.X;
                        int y = base._startHandle.Y;
                        foreach (string str in this.Ratios.Split(new char[] { '|' }))
                        {
                            string[] strArray3 = str.Split(new char[] { 'x', 'X' });
                            int num9 = int.Parse(strArray3[0]);
                            int num6 = int.Parse(strArray3[1]);
                            if (num9.Equals(1) && num6.Equals(1))
                            {
                                pen.Width = base.Width;
                            }
                            else
                            {
                                pen.Width = 1f;
                            }
                            int num5 = 100;
                            int num10 = (base.Bars.Count - bar) - 1;
                            if ((100 * num9) > num10)
                            {
                                num5 = num10 / num9;
                            }
                            if (num5 > 0)
                            {
                                int num8 = base.ConvertBarToX(bar + (num5 * num9));
                                if (this.FanUp)
                                {
                                    int num11 = base.Pane.ConvertValueToY(base._startHandle.Value + ((num5 * num6) * this.PriceUnit));
                                    num11 = ChartDrawingObject.CalculateYIntercept(x, y, num8, num11, base.ChartWidth);
                                    graphics_0.DrawLine(pen, x, y, base.ChartWidth, num11);
                                }
                                if (this.FanDown)
                                {
                                    int num7 = base.Pane.ConvertValueToY(base._startHandle.Value - ((num5 * num6) * this.PriceUnit));
                                    num7 = ChartDrawingObject.CalculateYIntercept(x, y, num8, num7, base.ChartWidth);
                                    graphics_0.DrawLine(pen, x, y, base.ChartWidth, num7);
                                }
                            }
                            base._endHandle.Visible = false;
                        }
                        pen.Dispose();
                        graphics_0.SmoothingMode = smoothingMode;
                    }
                }
                catch (Exception exception)
                {
                    new Exception("Error calculating Gann fan value: ", exception);
                }
            }
        }

        protected override void Write(BinaryWriter binaryWriter_0)
        {
            base.Write(binaryWriter_0);
            binaryWriter_0.Write(base.Color.ToArgb());
            binaryWriter_0.Write(base.Width);
            binaryWriter_0.Write((int) base.Style);
            binaryWriter_0.Write(base.ExtendLeft);
            binaryWriter_0.Write(base.ExtendRight);
            binaryWriter_0.Write(base.SnapToValue);
            binaryWriter_0.Write(this.FanUp);
            binaryWriter_0.Write(this.FanDown);
            binaryWriter_0.Write(this.PriceUnit);
            binaryWriter_0.Write(this.Ratios);
        }

        public override void WriteSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            host.Set(str + "Color", base.Color);
            host.Set(str + "Style", base.Style.ToString());
            host.Set(str + "Width", base.Width);
            host.Set(str + "SnapToValue", base.SnapToValue);
            host.Set(str + "FanUp", this.FanUp);
            host.Set(str + "FanDown", this.FanDown);
            host.Set(str + "PriceUnit", this.PriceUnit);
            host.Set(str + "Ratios", this.Ratios);
        }

        public bool FanDown
        {
            get
            {
                return this.bool_6;
            }
            set
            {
                this.bool_6 = value;
            }
        }

        public bool FanUp
        {
            get
            {
                return this.bool_5;
            }
            set
            {
                this.bool_5 = value;
            }
        }

        protected override DrawingObjectHelper Helper
        {
            get
            {
                return drawingObjectHelper_0;
            }
        }

        public double PriceUnit
        {
            get
            {
                return this.double_0;
            }
            set
            {
                this.double_0 = value;
            }
        }

        public string Ratios
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
    }
}

