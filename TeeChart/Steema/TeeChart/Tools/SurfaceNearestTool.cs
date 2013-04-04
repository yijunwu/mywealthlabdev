namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [Description("Displays a visual indication of selecting a Surface cell when the user moves the mouse over a surface series."), ToolboxBitmap(typeof(SurfaceNearestTool), "ToolsIcons.SurfaceNearestTool.bmp")]
    public class SurfaceNearestTool : ToolSeries
    {
        private Color FCell;
        private Color FColumn;
        private bool FIsSolidColor;
        private Color FRow;
        private Color FSolidColor;
        public int SelectedCell;

        public event SelectEventHandler Select;

        public SurfaceNearestTool() : this((Chart) null)
        {
        }

        public SurfaceNearestTool(Chart c) : base(c)
        {
            this.FCell = Color.Red;
            this.FColumn = Color.Green;
            this.FRow = Color.Blue;
            this.SelectedCell = -1;
        }

        public SurfaceNearestTool(Series s) : this(s.chart)
        {
            base.Series = s;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            SurfaceNearestTool tool = t as SurfaceNearestTool;
            tool.CellColor = this.CellColor;
            tool.ColumnColor = this.ColumnColor;
            tool.RowColor = this.RowColor;
        }

        public void GetRowCol(out double Row, out double Col)
        {
            Surface series = base.Series as Surface;
            if (this.SelectedCell == -1)
            {
                Row = -1.0;
                Col = -1.0;
            }
            else
            {
                CellsOrientation orientation = series.CellsOrientation();
                Row = series.XValues[this.SelectedCell];
                if (orientation.IncX == 1)
                {
                    Row--;
                }
                Col = series.ZValues[this.SelectedCell];
                if (orientation.IncZ == -1)
                {
                    Col++;
                }
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if ((kind == MouseEventKinds.Move) && (base.Series != null))
            {
                if (base.Series.Count > 0)
                {
                    this.FSolidColor = base.Series.ValueColor(0);
                }
                this.FIsSolidColor = this.FSolidColor == base.Series.ValueColor(1);
                int num = base.Series.Clicked(e.X, e.Y);
                if (num != this.SelectedCell)
                {
                    int num2;
                    this.SelectedCell = num;
                    base.Series.Chart.AutoRepaint = false;
                    Custom3DGrid series = base.Series as Custom3DGrid;
                    if (this.SelectedCell == -1)
                    {
                        for (num2 = 0; num2 < series.Count; num2++)
                        {
                            if (this.FIsSolidColor)
                            {
                                series[num2].Color = this.FSolidColor;
                            }
                            else
                            {
                                series[num2].Color = series.OriginalValueColor(num2);
                            }
                        }
                    }
                    else
                    {
                        double num3;
                        double num4;
                        this.GetRowCol(out num3, out num4);
                        for (num2 = 0; num2 < series.Count; num2++)
                        {
                            if ((series.XValues[num2] == num3) && (series.ZValues[num2] == num4))
                            {
                                series[num2].Color = this.CellColor;
                            }
                            else if (series.XValues[num2] == num3)
                            {
                                series[num2].Color = this.RowColor;
                            }
                            else if (series.ZValues.Value[num2] == num4)
                            {
                                series[num2].Color = this.ColumnColor;
                            }
                            else if (this.FIsSolidColor)
                            {
                                series[num2].Color = this.FSolidColor;
                            }
                            else
                            {
                                series[num2].Color = series.OriginalValueColor(num2);
                            }
                        }
                    }
                    base.Series.Chart.AutoRepaint = true;
                    base.Series.Chart.Invalidate();
                    if (this.Select != null)
                    {
                        this.Select(this, EventArgs.Empty);
                    }
                }
            }
        }

        [Description("Gets or sets the color used to fill the surface cell under the mouse cursor."), DefaultValue(typeof(Color), "Color.Red")]
        public Color CellColor
        {
            get
            {
                return this.FCell;
            }
            set
            {
                base.SetColorProperty(ref this.FCell, value);
            }
        }

        [Description("Gets or sets the color used to fill all the surface cells that belong to the grid column under the mouse cursor."), DefaultValue(typeof(Color), "Color.Green")]
        public Color ColumnColor
        {
            get
            {
                return this.FColumn;
            }
            set
            {
                base.SetColorProperty(ref this.FColumn, value);
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.SurfaceNearestTool;
            }
        }

        [Description("Gets or sets the color used to fill all the surface cells that belong to the grid row under the mouse cursor."), DefaultValue(typeof(Color), "Color.Blue")]
        public Color RowColor
        {
            get
            {
                return this.FRow;
            }
            set
            {
                base.SetColorProperty(ref this.FRow, value);
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.SurfaceNearestToolSummary;
            }
        }
    }
}

