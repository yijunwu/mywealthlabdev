namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Reflection;

    [Description("Axes properties."), Editor(typeof(Axes.AxesComponentEditor), typeof(UITypeEditor))]
    public class Axes : TeeBase
    {
        protected internal AxisCalcPosLabelsEventHandler AxisCalcPosLabels;
        private Axis bottom;
        internal CustomAxes custom;
        internal DepthAxis depth;
        internal DepthAxis depthTop;
        private bool drawBehind;
        private Axis left;
        public int NumFixedAxes;
        private Axis right;
        private Axis top;
        private bool visible;

        public Axes(Chart c) : base(c)
        {
            this.custom = new CustomAxes();
            this.visible = true;
            this.drawBehind = true;
            this.NumFixedAxes = 6;
            this.custom.Chart = base.chart;
            this.left = new Axis(base.chart);
            this.left.Title.SetInitialAngle(90);
            this.right = new Axis(false, true, base.chart);
            this.right.ZPosition = 100.0;
            this.right.Title.SetInitialAngle(270);
            this.top = new Axis(true, true, base.chart);
            this.top.ZPosition = 100.0;
            this.bottom = new Axis(true, false, base.chart);
            this.depth = new DepthAxis(false, true, base.chart);
            this.depthTop = new DepthAxis(false, false, base.chart);
        }

        internal void AdjustMaxMin()
        {
            this.left.AdjustMaxMin();
            this.top.AdjustMaxMin();
            this.right.AdjustMaxMin();
            this.bottom.AdjustMaxMin();
            this.depth.AdjustMaxMin();
            this.depthTop.AdjustMaxMin();
            foreach (Axis axis in this.custom)
            {
                axis.AdjustMaxMin();
            }
        }

        internal void CheckAxis(ref Axis a)
        {
            if (a == null)
            {
                a = new Axis(base.chart);
            }
        }

        public static Axis CreateNewAxis(Chart chart)
        {
            Axis axis = (Axis) Activator.CreateInstance(typeof(Axis));
            axis.Chart = chart;
            return axis;
        }

        internal void DoZoom(int x0, int y0, int x1, int y1)
        {
            this.Left.CalcPosPoint(y1);
            this.Right.CalcPosPoint(y0);
            if (((Math.Abs(this.Left.CalcPosValue(this.Left.CalcIncrement)) < 2126008810.53) && (Math.Abs(this.Bottom.CalcPosValue(this.Bottom.CalcIncrement)) < 2126008810.53)) && ((Math.Abs(this.Bottom.CalcPosValue(this.Right.CalcIncrement)) < 2126008810.53) && (Math.Abs(this.Bottom.CalcPosValue(this.Top.CalcIncrement)) < 2126008810.53)))
            {
                base.chart.DoZoom(this.Top.CalcPosPoint(x0), this.Top.CalcPosPoint(x1), this.Bottom.CalcPosPoint(x0), this.Bottom.CalcPosPoint(x1), this.Left.CalcPosPoint(y1), this.Left.CalcPosPoint(y0), this.Right.CalcPosPoint(y1), this.Right.CalcPosPoint(y0));
            }
        }

        public void Draw()
        {
            this.Draw(base.chart.graphics3D);
        }

        public void Draw(Graphics3D g)
        {
            if (base.chart.parent != null)
            {
                base.chart.parent.DoBeforeDrawAxes();
            }
            if (base.chart.IsAxisVisible(this.left))
            {
                this.left.Draw(true);
            }
            if (base.chart.IsAxisVisible(this.right))
            {
                this.right.Draw(true);
            }
            if (base.chart.IsAxisVisible(this.top))
            {
                this.top.Draw(true);
            }
            if (base.chart.IsAxisVisible(this.bottom))
            {
                this.bottom.Draw(true);
            }
            if (base.chart.IsAxisVisible(this.depth))
            {
                this.depth.Draw(true);
            }
            if (base.chart.IsAxisVisible(this.depthTop))
            {
                this.depthTop.Draw(true);
            }
            foreach (Axis axis in this.custom)
            {
                if (axis.Visible)
                {
                    axis.Draw(true);
                }
            }
        }

        public int IndexOf(Axis a)
        {
            if (a == this.left)
            {
                return 0;
            }
            if (a == this.top)
            {
                return 1;
            }
            if (a == this.right)
            {
                return 2;
            }
            if (a == this.bottom)
            {
                return 3;
            }
            if (a == this.depth)
            {
                return 4;
            }
            if (a == this.depthTop)
            {
                return 5;
            }
            return this.custom.IndexOf(a);
        }

        internal void InternalCalcPositions()
        {
            this.left.InternalCalcPositions();
            this.top.InternalCalcPositions();
            this.right.InternalCalcPositions();
            this.bottom.InternalCalcPositions();
            this.depth.InternalCalcPositions();
            this.depthTop.InternalCalcPositions();
            foreach (Axis axis in this.custom)
            {
                axis.InternalCalcPositions();
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            this.custom.Chart = c;
        }

        protected virtual bool ShouldSerializeCustom()
        {
            return (this.custom.Count > 0);
        }

        public string[] StringItems()
        {
            string[] strArray = new string[4 + this.custom.Count];
            strArray[0] = Texts.LeftAxis;
            strArray[1] = Texts.TopAxis;
            strArray[2] = Texts.RightAxis;
            strArray[3] = Texts.BottomAxis;
            for (int i = 0; i < this.custom.Count; i++)
            {
                strArray[4 + i] = "Custom " + i.ToString();
            }
            return strArray;
        }

        [Category("Axes"), Description("Bottom Axis."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Axis Bottom
        {
            get
            {
                this.CheckAxis(ref this.bottom);
                return this.bottom;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Returns the number of custom axes.")]
        public int Count
        {
            get
            {
                return (this.custom.Count + this.NumFixedAxes);
            }
        }

        [Description("Custom defined axes collection."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CustomAxes Custom
        {
            get
            {
                return this.custom;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Depth Axis."), Category("Axes")]
        public Axis Depth
        {
            get
            {
                if (this.depth == null)
                {
                    this.depth = new DepthAxis(false, true, base.chart);
                }
                return this.depth;
            }
        }

        [Category("Axes"), Description("DepthTop Axis."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Axis DepthTop
        {
            get
            {
                if (this.depthTop == null)
                {
                    this.depthTop = new DepthAxis(false, false, base.chart);
                }
                return this.depthTop;
            }
        }

        [DefaultValue(true), Description("Draw axes behind or in front of Series.")]
        public bool DrawBehind
        {
            get
            {
                return this.drawBehind;
            }
            set
            {
                base.SetBooleanProperty(ref this.drawBehind, value);
            }
        }

        public Axis this[int index]
        {
            get
            {
                if (index < (this.custom.Count + this.NumFixedAxes))
                {
                    if (index >= this.NumFixedAxes)
                    {
                        return this.custom[index - this.NumFixedAxes];
                    }
                    switch (index)
                    {
                        case 0:
                            return this.left;

                        case 1:
                            return this.top;

                        case 2:
                            return this.right;

                        case 3:
                            return this.bottom;

                        case 4:
                            return this.depth;

                        case 5:
                            return this.depthTop;
                    }
                }
                return null;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Axes"), Description("Left Axis.")]
        public Axis Left
        {
            get
            {
                this.CheckAxis(ref this.left);
                return this.left;
            }
        }

        [Description("Right Axis."), Category("Axes"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Axis Right
        {
            get
            {
                this.CheckAxis(ref this.right);
                return this.right;
            }
        }

        [Description("Top Axis."), Category("Axes"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Axis Top
        {
            get
            {
                this.CheckAxis(ref this.top);
                return this.top;
            }
        }

        [Category("Axes"), Description("Shows / Hides all Chart axes."), DefaultValue(true)]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                base.SetBooleanProperty(ref this.visible, value);
            }
        }

        internal class AxesComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Axes axes = (Axes) value;
                bool flag = EditorUtils.ShowFormModal(new AxesEditor(axes.Chart, null));
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }
    }
}

