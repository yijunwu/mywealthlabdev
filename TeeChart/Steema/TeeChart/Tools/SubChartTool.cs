namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [Serializable, ToolboxBitmap(typeof(SubChartTool), "ToolsIcons.SubChartTool.bmp")]
    public class SubChartTool : Steema.TeeChart.Tools.Tool
    {
        private ChartCollection charts;

        public SubChartTool() : this(null)
        {
        }

        public SubChartTool(Chart c) : base(c)
        {
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (base.Active && (e is AfterDrawEventArgs))
            {
                for (int i = 0; i < this.Charts.Count; i++)
                {
                    this.Charts[i].Chart.Chart.Draw(base.Chart.Graphics3D.g, this.Charts[i].Bounds());
                }
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (base.Active)
            {
                for (int i = 0; i < this.Charts.Count; i++)
                {
                    if (!this.Charts[i].Bounds().Contains(e.X, e.Y))
                    {
                        continue;
                    }
                    if (this.Charts[i].Chart.Graphics3D.g == null)
                    {
                        this.Charts[i].Chart.Graphics3D.g = this.Charts[i].Chart.CreateGraphics();
                    }
                    switch (kind)
                    {
                        case MouseEventKinds.Down:
                            this.Charts[i].Chart.Chart.DoMouseDown(false, e, Control.ModifierKeys);
                            break;

                        case MouseEventKinds.Move:
                            this.Charts[i].Chart.Chart.DoMouseMove(e.X, e.Y, ref c);
                            break;

                        case MouseEventKinds.Up:
                            this.Charts[i].Chart.Chart.DoMouseUp(e, Control.ModifierKeys);
                            break;
                    }
                    base.Chart.CancelMouse = (this.Charts[i].Chart.Chart.CancelMouse || this.Charts[i].Chart.Zoom.Active) || this.Charts[i].Chart.Panning.Active;
                    if (base.Chart.CancelMouse)
                    {
                        return;
                    }
                }
            }
            else
            {
                base.MouseEvent(kind, e, ref c);
            }
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            if (value != null)
            {
                for (int i = 0; i < this.Charts.Count; i++)
                {
                    this.Charts[i].Chart.Chart.Parent = value.Parent;
                }
            }
            this.Invalidate();
        }

        [Description("The collection of charts within the subchart tool."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartCollection Charts
        {
            get
            {
                if (this.charts == null)
                {
                    this.charts = new ChartCollection(this);
                }
                return this.charts;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.SubChartTool;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.SubChartToolSummary;
            }
        }
    }
}

