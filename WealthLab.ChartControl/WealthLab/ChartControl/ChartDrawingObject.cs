namespace WealthLab.ChartControl
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using WealthLab;

    public abstract class ChartDrawingObject
    {
        private bool bool_0;
        private bool bool_1;
        private ChartPane chartPane_0;
        private ChartRenderer chartRenderer_0;
        private DrawingObjectManager drawingObjectManager_0;
        private Font font_0;
        private List<ChartDrawingObjectHandle> list_0;
        private string string_0;
        private string string_1;

        public ChartDrawingObject()
        {
            this.list_0 = new List<ChartDrawingObjectHandle>();
            this.bool_1 = true;
        }

        public ChartDrawingObject(ChartPane pane, DateTime dateTime_0, double value)
        {
            this.list_0 = new List<ChartDrawingObjectHandle>();
            this.bool_1 = true;
            this.chartPane_0 = pane;
            this.string_0 = pane.Description;
        }

        protected virtual void AddDrawingObject(ChartDrawingObject chartDrawingObject_0)
        {
            this.drawingObjectManager_0.method_2(chartDrawingObject_0);
            this.drawingObjectManager_0.Chart.DoInvalidate();
        }

        public static double CalculateYIntercept(double double_0, double double_1, double double_2, double double_3, double double_4)
        {
            double num = double_3 - double_1;
            double num2 = double_2 - double_0;
            double num3 = num / num2;
            double num4 = double_1 - (num3 * double_0);
            return ((num3 * double_4) + num4);
        }

        public static int CalculateYIntercept(int int_0, int int_1, int int_2, int int_3, int int_4)
        {
            double num = int_3 - int_1;
            double num2 = int_2 - int_0;
            double num3 = num / num2;
            double num4 = int_1 - (num3 * int_0);
            return (int) Math.Round((double) ((num3 * int_4) + num4));
        }

        protected Color ColorWithTransparency(Color color_0, int transLevel)
        {
            int alpha = (int) ((100 - transLevel) * 2.55M);
            return Color.FromArgb(alpha, color_0.R, color_0.G, color_0.B);
        }

        public int ConvertBarToX(int int_0)
        {
            return this.chartRenderer_0.ConvertBarToX(int_0);
        }

        public int ConvertXToBar(int int_0)
        {
            return this.chartRenderer_0.ConvertXToBar(int_0);
        }

        protected ChartDrawingObjectHandle CreateHandle()
        {
            ChartDrawingObjectHandle item = new ChartDrawingObjectHandle(this);
            this.list_0.Add(item);
            return item;
        }

        protected internal abstract bool IsMouseOver(int int_0, int int_1);
        public static bool MouseOverLine(ChartDrawingObjectHandle handleStart, ChartDrawingObjectHandle handleEnd, int int_0, int int_1, bool extendLeft, bool extendRight)
        {
            bool flag;
            if ((handleStart.Bar == -1) || (handleEnd.Bar == -1))
            {
                return false;
            }
            if (Math.Abs((int) (handleStart.X - handleEnd.X)) <= Chart.PixelSensitivity)
            {
                if (Math.Abs((int) (int_0 - handleStart.X)) > Chart.PixelSensitivity)
                {
                    return (Math.Abs((int) (int_0 - handleEnd.X)) <= Chart.PixelSensitivity);
                }
                return true;
            }
            int num2 = CalculateYIntercept(handleStart.X, handleStart.Y, handleEnd.X, handleEnd.Y, int_0);
            if (flag = Math.Abs((int) (int_1 - num2)) <= Chart.PixelSensitivity)
            {
                if (!extendLeft)
                {
                    int num3 = (handleStart.X < handleEnd.X) ? handleStart.X : handleEnd.X;
                    if ((int_0 + Chart.PixelSensitivity) < num3)
                    {
                        return false;
                    }
                }
                if (!extendRight)
                {
                    int num = (handleStart.X > handleEnd.X) ? handleStart.X : handleEnd.X;
                    if ((int_0 - Chart.PixelSensitivity) > num)
                    {
                        return false;
                    }
                }
            }
            return flag;
        }

        protected internal virtual void OnBeginDrag(ChartDrawingObjectHandle handle)
        {
            if (handle.HandleType == ChartDrawingObjectHandleType.Mover)
            {
                foreach (ChartDrawingObjectHandle handle3 in this.Handles)
                {
                    handle3.method_2();
                }
            }
            else
            {
                foreach (ChartDrawingObjectHandle handle2 in this.Handles)
                {
                    if (handle2.HandleType == ChartDrawingObjectHandleType.Mover)
                    {
                        handle2.Visible = false;
                    }
                }
            }
        }

        protected internal virtual void OnDrag(ChartDrawingObjectHandle handle)
        {
            if (handle.HandleType == ChartDrawingObjectHandleType.Mover)
            {
                Point point = handle.method_3();
                foreach (ChartDrawingObjectHandle handle3 in this.Handles)
                {
                    if ((handle3 != handle) && !handle3.method_4(point))
                    {
                        return;
                    }
                }
                foreach (ChartDrawingObjectHandle handle2 in this.Handles)
                {
                    if (handle2 != handle)
                    {
                        handle2.method_5(point);
                    }
                }
            }
        }

        protected internal virtual void OnEndDrag(ChartDrawingObjectHandle handle)
        {
            foreach (ChartDrawingObjectHandle handle2 in this.Handles)
            {
                if (handle2.HandleType == ChartDrawingObjectHandleType.Mover)
                {
                    handle2.Visible = true;
                }
            }
        }

        protected internal virtual void OnSelected(int int_0, int int_1)
        {
        }

        protected internal virtual void Read(BinaryReader binaryReader_0)
        {
            for (int i = binaryReader_0.ReadInt32(); i > 0; i--)
            {
                ChartDrawingObjectHandle handle = this.CreateHandle();
                handle.Date = new DateTime(binaryReader_0.ReadInt64());
                handle.Value = binaryReader_0.ReadDouble();
                handle.SnapToValue = binaryReader_0.ReadBoolean();
                handle.HandleType = (ChartDrawingObjectHandleType) binaryReader_0.ReadInt32();
                handle.Visible = binaryReader_0.ReadBoolean();
            }
            this.Name = binaryReader_0.ReadString();
            this.PaneDescription = binaryReader_0.ReadString();
        }

        public virtual void RegisterExtendedBehaviors(ICDOBehavior behaviors)
        {
        }

        protected internal abstract void Render(Graphics graphics_0);
        protected internal virtual void RenderHandles(Graphics graphics_0)
        {
            if (this.Selected)
            {
                foreach (ChartDrawingObjectHandle handle in this.Handles)
                {
                    if (handle.Visible)
                    {
                        handle.method_1(graphics_0);
                    }
                }
            }
        }

        protected internal virtual void RenderNotClipped(Graphics graphics_0)
        {
        }

        public virtual bool TriggerAlert(WealthLab.Bars bars, ref TradeType alertType, ref string signalName)
        {
            return false;
        }

        protected internal virtual void Write(BinaryWriter binaryWriter_0)
        {
            binaryWriter_0.Write(this.Handles.Count);
            foreach (ChartDrawingObjectHandle handle in this.Handles)
            {
                binaryWriter_0.Write(handle.Date.Ticks);
                binaryWriter_0.Write(handle.Value);
                binaryWriter_0.Write(handle.SnapToValue);
                binaryWriter_0.Write((int) handle.HandleType);
                binaryWriter_0.Write(handle.Visible);
            }
            binaryWriter_0.Write(this.Name);
            binaryWriter_0.Write(this.PaneDescription);
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.chartRenderer_0.Bars;
            }
        }

        public virtual bool CanTriggerAlerts
        {
            get
            {
                return false;
            }
        }

        public int ChartHeight
        {
            get
            {
                return this.chartRenderer_0.Height;
            }
        }

        public int ChartWidth
        {
            get
            {
                return this.chartRenderer_0.Width;
            }
        }

        public bool ClipToPane
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public virtual bool ConfineToPricePane
        {
            get
            {
                return false;
            }
        }

        public Font HandleFont
        {
            get
            {
                return this.font_0;
            }
            internal set
            {
                this.font_0 = value;
            }
        }

        public List<ChartDrawingObjectHandle> Handles
        {
            get
            {
                return this.list_0;
            }
        }

        protected internal abstract DrawingObjectHelper Helper { get; }

        internal DrawingObjectManager Manager
        {
            get
            {
                return this.drawingObjectManager_0;
            }
            set
            {
                this.drawingObjectManager_0 = value;
            }
        }

        public string Name
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
            }
        }

        public ChartPane Pane
        {
            get
            {
                return this.chartPane_0;
            }
            set
            {
                this.chartPane_0 = value;
            }
        }

        public string PaneDescription
        {
            get
            {
                return this.string_0;
            }
            private set
            {
                this.string_0 = value;
            }
        }

        public ChartRenderer Renderer
        {
            get
            {
                return this.chartRenderer_0;
            }
            set
            {
                this.chartRenderer_0 = value;
            }
        }

        public bool Selected
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public virtual bool ShowToolTip
        {
            get
            {
                return false;
            }
        }

        public virtual string ToolTipText
        {
            get
            {
                return "";
            }
        }
    }
}

