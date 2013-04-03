namespace WealthLab.ChartControl
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;

    public abstract class CDOPolygonBased : ChartDrawingObject, ICustomSettings
    {
        protected ChartDrawingObjectHandle _endHandle;
        protected ChartDrawingObjectHandle _mover;
        protected ChartDrawingObjectHandle _startHandle;
        private bool bool_2;
        private System.Drawing.Color color_0;
        private int int_0;
        private int int_1;
        private LineStyle lineStyle_0;
        private PolygonBasedObjectSettings polygonBasedObjectSettings_0;

        public CDOPolygonBased()
        {
            this.color_0 = System.Drawing.Color.Black;
            this.int_0 = 2;
        }

        public CDOPolygonBased(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
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
            PolygonBasedObjectSettings settings = userControl_0 as PolygonBasedObjectSettings;
            this.Color = settings.Color;
            this.Style = settings.Style;
            this.Width = settings.DrawingObjectWidth;
            this.FillTransparency = settings.FillTransparency;
            this.SnapToValue = settings.SnapToValue;
        }

        public virtual UserControl GetSettingsUI()
        {
            if (this.polygonBasedObjectSettings_0 == null)
            {
                this.polygonBasedObjectSettings_0 = new PolygonBasedObjectSettings();
            }
            this.polygonBasedObjectSettings_0.Color = this.Color;
            this.polygonBasedObjectSettings_0.DrawingObjectWidth = this.Width;
            this.polygonBasedObjectSettings_0.Style = this.Style;
            this.polygonBasedObjectSettings_0.FillTransparency = this.FillTransparency;
            this.polygonBasedObjectSettings_0.SnapToValue = this.SnapToValue;
            return this.polygonBasedObjectSettings_0;
        }

        protected internal override bool IsMouseOver(int int_2, int int_3)
        {
            return ChartDrawingObject.MouseOverLine(this._startHandle, this._endHandle, int_2, int_3, false, false);
        }

        protected internal override void OnSelected(int int_2, int int_3)
        {
            int num = base.ConvertXToBar(int_2);
            int num2 = ChartDrawingObject.CalculateYIntercept(this._startHandle.X, this._startHandle.Y, this._endHandle.X, this._endHandle.Y, int_2);
            double num3 = base.Pane.ConvertYToValue(num2);
            this._mover.Bar = num;
            this._mover.Value = num3;
        }

        protected internal override void Read(BinaryReader binaryReader_0)
        {
            base.Read(binaryReader_0);
            int argb = binaryReader_0.ReadInt32();
            this.Color = System.Drawing.Color.FromArgb(argb);
            this.Width = binaryReader_0.ReadInt32();
            this.Style = (LineStyle) binaryReader_0.ReadInt32();
            this.FillTransparency = binaryReader_0.ReadInt32();
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
            this.FillTransparency = host.Get(str + "FillTransparency", 80);
            this.SnapToValue = host.Get(str + "SnapToValue", false);
        }

        protected internal override void Render(Graphics graphics_0)
        {
            if (this.LeftHandle.Bar != -1)
            {
                int bar = this.RightHandle.Bar;
            }
        }

        protected internal override void Write(BinaryWriter binaryWriter_0)
        {
            base.Write(binaryWriter_0);
            binaryWriter_0.Write(this.Color.ToArgb());
            binaryWriter_0.Write(this.Width);
            binaryWriter_0.Write((int) this.Style);
            binaryWriter_0.Write(this.FillTransparency);
            binaryWriter_0.Write(this.SnapToValue);
        }

        public virtual void WriteSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            host.Set(str + "Color", this.Color);
            host.Set(str + "Style", this.Style.ToString());
            host.Set(str + "Width", this.Width);
            host.Set(str + "FillTransparency", this.FillTransparency);
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

        public int FillTransparency
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
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
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
                foreach (ChartDrawingObjectHandle handle in base.Handles)
                {
                    handle.SnapToValue = this.bool_2;
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

