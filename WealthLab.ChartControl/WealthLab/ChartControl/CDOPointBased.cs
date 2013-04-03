namespace WealthLab.ChartControl
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;

    public abstract class CDOPointBased : ChartDrawingObject, ICustomSettings
    {
        protected ChartDrawingObjectHandle _mover;
        protected Point _origin;
        private PointBasedObjectSettings pointBasedObjectSettings_0;

        public CDOPointBased()
        {
        }

        public CDOPointBased(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
            this._mover = base.CreateHandle();
            this._mover.Date = dateTime_0;
            this._mover.Value = value;
            this._mover.HandleType = ChartDrawingObjectHandleType.Mover;
        }

        public virtual void ChangeSettings(UserControl userControl_0)
        {
            PointBasedObjectSettings settings = userControl_0 as PointBasedObjectSettings;
            this._origin = settings.Origin;
        }

        public virtual UserControl GetSettingsUI()
        {
            if (this.pointBasedObjectSettings_0 == null)
            {
                this.pointBasedObjectSettings_0 = new PointBasedObjectSettings();
            }
            this.pointBasedObjectSettings_0.Origin = this._origin;
            return this.pointBasedObjectSettings_0;
        }

        protected internal override bool IsMouseOver(int int_0, int int_1)
        {
            return false;
        }

        protected internal override void OnSelected(int int_0, int int_1)
        {
            int num = base.ConvertXToBar(int_0);
            double num2 = base.Pane.ConvertYToValue(int_1);
            this._mover.Bar = num;
            this._mover.Value = num2;
        }

        protected internal override void Read(BinaryReader binaryReader_0)
        {
            base.Read(binaryReader_0);
            this.X = binaryReader_0.ReadInt32();
            this.Y = binaryReader_0.ReadInt32();
            this._mover = base.Handles[0];
        }

        public virtual void ReadSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            this.X = host.Get(str + "X", 10);
            this.Y = host.Get(str + "Y", 10);
        }

        protected internal override void Render(Graphics graphics_0)
        {
            int bar = this._mover.Bar;
        }

        protected internal override void Write(BinaryWriter binaryWriter_0)
        {
            base.Write(binaryWriter_0);
            binaryWriter_0.Write(this.X);
            binaryWriter_0.Write(this.Y);
        }

        public virtual void WriteSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            host.Set(str + "X", this.X);
            host.Set(str + "Y", this.Y);
        }

        public int X
        {
            get
            {
                return this._origin.X;
            }
            set
            {
                this._origin.Y = value;
            }
        }

        public int Y
        {
            get
            {
                return this._origin.Y;
            }
            set
            {
                this._origin.Y = value;
            }
        }
    }
}

