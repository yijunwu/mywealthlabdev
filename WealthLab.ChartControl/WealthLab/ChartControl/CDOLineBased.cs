namespace WealthLab.ChartControl
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;

    public abstract class CDOLineBased : ChartDrawingObject, ICustomSettings
    {
        protected ChartDrawingObjectHandle _endHandle;
        protected ChartDrawingObjectHandle _mover;
        protected ChartDrawingObjectHandle _startHandle;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private System.Drawing.Color color_0;
        private int int_0;
        private LineBasedObjectSettings lineBasedObjectSettings_0;
        private LineStyle lineStyle_0;

        public CDOLineBased()
        {
            this.color_0 = System.Drawing.Color.Black;
            this.int_0 = 2;
        }

        public CDOLineBased(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
            this.color_0 = System.Drawing.Color.Black;
            this.int_0 = 2;
            this._startHandle = base.CreateHandle();
            this._startHandle.Date = dateTime_0;
            this._startHandle.Value = value;
            this._endHandle = base.CreateHandle();
            this._endHandle.Date = dateTime_0;
            this._endHandle.Value = value;
            this._mover = base.CreateHandle();
            this._mover.Date = dateTime_0;
            this._mover.Value = value;
            this._mover.HandleType = ChartDrawingObjectHandleType.Mover;
        }

        public virtual void ChangeSettings(UserControl userControl_0)
        {
            LineBasedObjectSettings settings = userControl_0 as LineBasedObjectSettings;
            base.Name = settings.DrawingObjectName;
            this.Color = settings.Color;
            this.Style = settings.Style;
            this.Width = settings.DrawingObjectWidth;
            this.ExtendLeft = settings.ExtendLeft;
            this.ExtendRight = settings.ExtendRight;
            this.SnapToValue = settings.SnapToValue;
        }

        public virtual UserControl GetSettingsUI()
        {
            if (this.lineBasedObjectSettings_0 == null)
            {
                this.lineBasedObjectSettings_0 = new LineBasedObjectSettings();
            }
            this.lineBasedObjectSettings_0.DrawingObjectName = base.Name;
            this.lineBasedObjectSettings_0.Color = this.Color;
            this.lineBasedObjectSettings_0.DrawingObjectWidth = this.Width;
            this.lineBasedObjectSettings_0.Style = this.Style;
            this.lineBasedObjectSettings_0.ExtendLeft = this.ExtendLeft;
            this.lineBasedObjectSettings_0.ExtendRight = this.ExtendRight;
            this.lineBasedObjectSettings_0.SnapToValue = this.SnapToValue;
            return this.lineBasedObjectSettings_0;
        }

        protected internal override bool IsMouseOver(int int_1, int int_2)
        {
            return ChartDrawingObject.MouseOverLine(this._startHandle, this._endHandle, int_1, int_2, this.ExtendLeft, this.ExtendRight);
        }

        protected internal override void OnSelected(int int_1, int int_2)
        {
            int num = base.ConvertXToBar(int_1);
            int num2 = ChartDrawingObject.CalculateYIntercept(this._startHandle.X, this._startHandle.Y, this._endHandle.X, this._endHandle.Y, int_1);
            double num3 = base.Pane.ConvertYToValue(num2);
            this._mover.Bar = num;
            this._mover.Value = num3;
        }

        public bool PromptUserForSettings(ISettingsHost host, DrawingObjectHelper _helper)
        {
            UserControl settingsUI = this.GetSettingsUI();
            DrawingObjectProperties properties = new DrawingObjectProperties {
                Text = _helper.FriendlyName + " Properties"
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

        protected internal override void Read(BinaryReader binaryReader_0)
        {
            base.Read(binaryReader_0);
            int argb = binaryReader_0.ReadInt32();
            this.Color = System.Drawing.Color.FromArgb(argb);
            this.Width = binaryReader_0.ReadInt32();
            this.Style = (LineStyle) binaryReader_0.ReadInt32();
            this.ExtendLeft = binaryReader_0.ReadBoolean();
            this.ExtendRight = binaryReader_0.ReadBoolean();
            this.SnapToValue = binaryReader_0.ReadBoolean();
            this._startHandle = base.Handles[0];
            this._endHandle = base.Handles[1];
            this._mover = base.Handles[2];
        }

        public virtual void ReadSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            this.Color = host.Get(str + "Color", System.Drawing.Color.Red);
            this.Style = (LineStyle) Enum.Parse(typeof(LineStyle), host.Get(str + "Style", "Solid"));
            this.Width = host.Get(str + "Width", 1);
            this.ExtendLeft = host.Get(str + "ExtendLeft", false);
            this.ExtendRight = host.Get(str + "ExtendRight", false);
            this.SnapToValue = host.Get(str + "SnapToValue", false);
        }

        protected internal override void Render(Graphics graphics_0)
        {
            if ((this.LeftHandle.Bar != -1) && (this.RightHandle.Bar != -1))
            {
                Pen pen = new Pen(this.Color, (float) this.Width);
                ChartRenderer.SetPenStyle(pen, this.Style);
                using (pen)
                {
                    pen.Width = this.Width;
                    int x = this.LeftHandle.X;
                    int y = this.LeftHandle.Y;
                    int chartWidth = this.RightHandle.X;
                    int num4 = this.RightHandle.Y;
                    if (this.ExtendLeft)
                    {
                        y = ChartDrawingObject.CalculateYIntercept(x, y, chartWidth, num4, 0);
                        x = 0;
                    }
                    if (this.ExtendRight)
                    {
                        num4 = ChartDrawingObject.CalculateYIntercept(x, y, chartWidth, num4, base.ChartWidth);
                        chartWidth = base.ChartWidth;
                    }
                    if ((y != -2147483648) && (num4 != -2147483648))
                    {
                        graphics_0.DrawLine(pen, x, y, chartWidth, num4);
                    }
                    else
                    {
                        x = this.LeftHandle.X;
                        y = this.LeftHandle.Y;
                        chartWidth = this.RightHandle.X;
                        num4 = this.RightHandle.Y;
                        graphics_0.DrawLine(pen, x, y, chartWidth, num4);
                    }
                }
            }
        }

        protected internal override void Write(BinaryWriter binaryWriter_0)
        {
            base.Write(binaryWriter_0);
            binaryWriter_0.Write(this.Color.ToArgb());
            binaryWriter_0.Write(this.Width);
            binaryWriter_0.Write((int) this.Style);
            binaryWriter_0.Write(this.ExtendLeft);
            binaryWriter_0.Write(this.ExtendRight);
            binaryWriter_0.Write(this.SnapToValue);
        }

        public virtual void WriteSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            host.Set(str + "Color", this.Color);
            host.Set(str + "Style", this.Style.ToString());
            host.Set(str + "Width", this.Width);
            host.Set(str + "ExtendLeft", this.ExtendLeft);
            host.Set(str + "ExtendRight", this.ExtendRight);
            host.Set(str + "SnapToValue", this.SnapToValue);
        }

        public System.Drawing.Color Color
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

        public bool ExtendLeft
        {
            get
            {
                return this.bool_3;
            }
            set
            {
                this.bool_3 = value;
            }
        }

        public bool ExtendRight
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
            }
        }

        public ChartDrawingObjectHandle LeftHandle
        {
            get
            {
                if (this._startHandle.Date >= this._endHandle.Date)
                {
                    return this._endHandle;
                }
                return this._startHandle;
            }
        }

        public ChartDrawingObjectHandle RightHandle
        {
            get
            {
                if (this._startHandle.Date < this._endHandle.Date)
                {
                    return this._endHandle;
                }
                return this._startHandle;
            }
        }

        public bool SnapToValue
        {
            get
            {
                return this.bool_4;
            }
            set
            {
                this.bool_4 = value;
                foreach (ChartDrawingObjectHandle handle in base.Handles)
                {
                    handle.SnapToValue = this.bool_4;
                }
            }
        }

        public LineStyle Style
        {
            get
            {
                return this.lineStyle_0;
            }
            set
            {
                this.lineStyle_0 = value;
            }
        }

        public int Width
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

