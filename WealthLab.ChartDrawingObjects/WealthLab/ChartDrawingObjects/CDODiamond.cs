namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using WealthLab;
    using WealthLab.ChartControl;

    public class CDODiamond : CDOPolygonBased
    {
        protected ChartDrawingObjectHandle _bHandle;
        protected ChartDrawingObjectHandle _cHandle;
        private static DrawingObjectHelper drawingObjectHelper_0 = new DiamondHelper();

        public CDODiamond()
        {
        }

        public CDODiamond(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
            this._bHandle = base.CreateHandle();
            this._bHandle.Date = dateTime_0;
            this._bHandle.Value = value;
            this._cHandle = base.CreateHandle();
            this._cHandle.Date = dateTime_0;
            this._cHandle.Value = value;
        }

        protected override bool IsMouseOver(int int_2, int int_3)
        {
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            flag = ChartDrawingObject.MouseOverLine(this.AHandle, this.BHandle, int_2, int_3, false, false);
            flag2 = ChartDrawingObject.MouseOverLine(this.BHandle, this.CHandle, int_2, int_3, false, false);
            flag3 = ChartDrawingObject.MouseOverLine(this.CHandle, this.DHandle, int_2, int_3, false, false);
            flag4 = ChartDrawingObject.MouseOverLine(this.DHandle, this.AHandle, int_2, int_3, false, false);
            if ((!flag && !flag2) && !flag3)
            {
                return flag4;
            }
            return true;
        }

        protected override void OnSelected(int int_2, int int_3)
        {
            int num = 0;
            if (ChartDrawingObject.MouseOverLine(this.AHandle, this.BHandle, int_2, int_3, false, false))
            {
                num = ChartDrawingObject.CalculateYIntercept(this.AHandle.X, this.AHandle.Y, this.BHandle.X, this.BHandle.Y, int_2);
            }
            else if (ChartDrawingObject.MouseOverLine(this.BHandle, this.CHandle, int_2, int_3, false, false))
            {
                num = ChartDrawingObject.CalculateYIntercept(this.BHandle.X, this.BHandle.Y, this.CHandle.X, this.CHandle.Y, int_2);
            }
            else if (ChartDrawingObject.MouseOverLine(this.CHandle, this.DHandle, int_2, int_3, false, false))
            {
                num = ChartDrawingObject.CalculateYIntercept(this.CHandle.X, this.CHandle.Y, this.DHandle.X, this.DHandle.Y, int_2);
            }
            else if (ChartDrawingObject.MouseOverLine(this.DHandle, this.AHandle, int_2, int_3, false, false))
            {
                num = ChartDrawingObject.CalculateYIntercept(this.DHandle.X, this.DHandle.Y, this.AHandle.X, this.AHandle.Y, int_2);
            }
            base._mover.Bar = base.ConvertXToBar(int_2);
            base._mover.Value = base.Pane.ConvertYToValue(num);
        }

        protected override void Read(BinaryReader binaryReader_0)
        {
            base.Read(binaryReader_0);
            this._bHandle = base.Handles[3];
            this._cHandle = base.Handles[4];
        }

        protected override void Render(Graphics graphics_0)
        {
            if ((base.LeftHandle.Bar != -1) && (base.RightHandle.Bar != -1))
            {
                SmoothingMode smoothingMode = graphics_0.SmoothingMode;
                graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
                Point[] points = new Point[] { new Point(0, 0), new Point(0, 0), new Point(0, 0), new Point(0, 0) };
                Pen pen = new Pen(base.Color, (float) base.Width);
                SolidBrush brush = new SolidBrush(base.ColorWithTransparency(base.Color, base.FillTransparency));
                ChartRenderer.SetPenStyle(pen, base.Style);
                pen.Width = base.Width;
                points[0].X = this.AHandle.X;
                points[0].Y = this.AHandle.Y;
                points[1].X = this.BHandle.X;
                points[1].Y = this.BHandle.Y;
                points[2].X = this.CHandle.X;
                points[2].Y = this.CHandle.Y;
                points[3].X = this.DHandle.X;
                points[3].Y = this.DHandle.Y;
                graphics_0.DrawPolygon(pen, points);
                graphics_0.FillPolygon(brush, points);
                pen.Dispose();
                brush.Dispose();
                graphics_0.SmoothingMode = smoothingMode;
            }
        }

        public ChartDrawingObjectHandle AHandle
        {
            get
            {
                return base._endHandle;
            }
            set
            {
                base._endHandle = value;
            }
        }

        public ChartDrawingObjectHandle BHandle
        {
            get
            {
                if ((this._bHandle.X == base._endHandle.X) && (this._bHandle.Y == base._endHandle.Y))
                {
                    this._bHandle.X = base._endHandle.X + 50;
                    this._bHandle.Y = base._endHandle.Y - 50;
                }
                return this._bHandle;
            }
            set
            {
                this._bHandle = value;
            }
        }

        public ChartDrawingObjectHandle CHandle
        {
            get
            {
                if ((this._cHandle.X == base._endHandle.X) && (this._cHandle.Y == base._endHandle.Y))
                {
                    this._cHandle.X = base._endHandle.X + 100;
                    this._cHandle.Y = base._endHandle.Y;
                }
                return this._cHandle;
            }
            set
            {
                this._cHandle = value;
            }
        }

        public ChartDrawingObjectHandle DHandle
        {
            get
            {
                return base._startHandle;
            }
            set
            {
                base._startHandle = value;
            }
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

