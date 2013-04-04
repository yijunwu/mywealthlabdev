namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Editor(typeof(Walls.WallsEditor), typeof(UITypeEditor)), Description("Chart walls.")]
    public class Walls : TeeBase
    {
        private BackWall back;
        private BottomWall bottom;
        private LeftWall left;
        private RightWall right;
        private bool view3D;
        private bool visible;

        public Walls(Chart c) : base(c)
        {
            this.visible = true;
            this.view3D = true;
            this.left = new LeftWall(c, this);
            this.right = new RightWall(c, this);
            this.bottom = new BottomWall(c, this);
            this.back = new BackWall(c, this);
        }

        public int CalcWallSize(Axis a)
        {
            if (base.chart.aspect.view3D && this.visible)
            {
                if (a == base.chart.axes.Left)
                {
                    return this.Left.Size;
                }
                if (a == base.chart.axes.Bottom)
                {
                    return this.Bottom.Size;
                }
            }
            return 0;
        }

        public void Paint(Graphics3D g, Rectangle r)
        {
            if (this.back.Visible && !base.chart.DrawBackWallAfter(base.chart.aspect.Width3D))
            {
                this.back.Paint(g, r);
            }
            if (base.chart.aspect.view3D && this.visible)
            {
                if (this.left.Visible && !base.chart.DrawLeftWallFirst())
                {
                    this.left.Paint(g, r);
                }
                if (this.right.Visible && !base.chart.DrawRightWallAfter())
                {
                    this.right.Paint(g, r);
                }
                if (this.bottom.Visible && !base.chart.DrawBottomWallFirst())
                {
                    this.bottom.Paint(g, r);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Back wall.")]
        public BackWall Back
        {
            get
            {
                return this.back;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Bottom wall.")]
        public BottomWall Bottom
        {
            get
            {
                return this.bottom;
            }
        }

        [Description("Left wall."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public LeftWall Left
        {
            get
            {
                return this.left;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Right wall.")]
        public RightWall Right
        {
            get
            {
                return this.right;
            }
        }

        [Description("Sets the Chart Walls thickness."), DefaultValue(0)]
        public int Size
        {
            set
            {
                this.Left.Size = value;
                this.Right.Size = value;
                this.Bottom.Size = value;
                this.Back.Size = value;
            }
        }

        [Description("Shows all Chart walls in 3D."), DefaultValue(true)]
        public bool View3D
        {
            get
            {
                return this.view3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.view3D, value);
            }
        }

        [DefaultValue(true), Description("Shows / Hides all Chart walls.")]
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

        public class BackWall : Wall
        {
            public BackWall(Chart c, Walls w) : base(c, w)
            {
                base.Color = Color.Silver;
                base.Brush.defaultColor = Color.Silver;
                base.Brush.defaultVisible = true;
                base.Brush.Visible = base.Brush.defaultVisible;
                base.defaultTransparent = false;
                base.Transparent = base.defaultTransparent;
                base.Brush.Gradient.defaultVisible = true;
                base.Brush.Gradient.Visible = base.Brush.Gradient.defaultVisible;
                base.Brush.Gradient.defaultStartColor = Utils.FromArgb(0xea, 0xea, 0xea);
                base.Brush.Gradient.StartColor = base.Brush.Gradient.defaultStartColor;
                base.Brush.Gradient.defaultEndColor = Utils.FromArgb(0xff, 0xff, 0xff);
                base.Brush.Gradient.EndColor = base.Brush.Gradient.defaultEndColor;
                base.Brush.Gradient.defaultMiddleColor = Utils.EmptyColor;
                base.Brush.Gradient.MiddleColor = base.Brush.Gradient.defaultMiddleColor;
            }

            public override void Paint(Graphics3D g, Rectangle rect)
            {
                base.PrepareGraphics(g);
                if (base.Brush.Color.A < 0xff)
                {
                    g.StartBlending(100.0 - ((((double) base.Brush.Color.A) / 255.0) * 100.0));
                }
                if (base.chart.aspect.view3D)
                {
                    int num = base.chart.aspect.Width3D;
                    if (base.iSize > 0)
                    {
                        Rectangle r = rect;
                        int num2 = base.walls.CalcWallSize(base.chart.axes.Left);
                        r.X -= num2;
                        r.Width += num2;
                        r.Height += base.walls.CalcWallSize(base.chart.Axes.Bottom);
                        g.Cube(r, num, num + base.iSize, base.ShouldDark);
                    }
                    else
                    {
                        g.Rectangle(rect, num);
                    }
                }
                else
                {
                    g.Rectangle(Utils.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom + 1));
                }
                g.EndBlending();
            }
        }

        public class BottomWall : Wall
        {
            public BottomWall(Chart c, Walls w) : base(c, w)
            {
                base.Color = Color.White;
                base.Brush.defaultVisible = true;
            }

            public override void Paint(Graphics3D g, Rectangle rect)
            {
                base.PrepareGraphics(g);
                if (base.Brush.Color.A < 0xff)
                {
                    g.StartBlending(100.0 - ((((double) base.Brush.Color.A) / 255.0) * 100.0));
                }
                int num = base.chart.Aspect.Width3D;
                if (base.iSize > 0)
                {
                    Rectangle r = rect;
                    r.Y = r.Bottom;
                    r.Height = base.iSize;
                    g.Cube(r, 0, num, base.ShouldDark);
                }
                else
                {
                    g.RectangleY(rect.X, rect.Bottom, rect.Right, 0, num);
                }
                g.EndBlending();
            }
        }

        public class LeftWall : Wall
        {
            public LeftWall(Chart c, Walls w) : base(c, w)
            {
                this.Color = System.Drawing.Color.LightYellow;
                base.Brush.defaultColor = System.Drawing.Color.LightYellow;
                base.Brush.defaultVisible = true;
            }

            public override void Paint(Graphics3D g, Rectangle rect)
            {
                base.PrepareGraphics(g);
                if (base.Brush.Color.A < 0xff)
                {
                    g.StartBlending(100.0 - ((((double) base.Brush.Color.A) / 255.0) * 100.0));
                }
                int bottom = rect.Bottom + base.walls.CalcWallSize(base.chart.axes.Bottom);
                int num2 = base.chart.aspect.Width3D;
                if (base.iSize > 0)
                {
                    g.Cube(rect.X - base.iSize, rect.Y, rect.X, bottom, 0, num2, base.ShouldDark);
                }
                else
                {
                    g.RectangleZ(rect.X, rect.Y, bottom, 0, num2);
                }
                g.EndBlending();
            }

            [Description("Specifies the color used to fill the LeftWall background."), DefaultValue(typeof(System.Drawing.Color), "LightYellow")]
            public System.Drawing.Color Color
            {
                get
                {
                    return base.Color;
                }
                set
                {
                    base.Color = value;
                }
            }
        }

        public class RightWall : Wall
        {
            public RightWall(Chart c, Walls w) : base(c, w)
            {
                base.visible = false;
                base.Color = Color.LightYellow;
                base.Brush.defaultColor = Color.LightYellow;
                base.Brush.defaultVisible = true;
            }

            public override void Paint(Graphics3D g, Rectangle rect)
            {
                base.PrepareGraphics(g);
                if (base.Brush.Color.A < 0xff)
                {
                    g.StartBlending(100.0 - ((((double) base.Brush.Color.A) / 255.0) * 100.0));
                }
                int bottom = rect.Bottom + base.walls.CalcWallSize(base.chart.axes.Bottom);
                int num2 = base.chart.aspect.Width3D;
                if (base.iSize > 0)
                {
                    g.Cube(rect.Right, rect.Y, rect.Right + base.iSize, bottom, 0, num2 + base.chart.Walls.Back.Size, base.ShouldDark);
                }
                else
                {
                    g.RectangleZ(rect.Right, rect.Y, bottom, 0, num2 + 1);
                }
                g.EndBlending();
            }

            [DefaultValue(false), Description("Shows/Hides Right Wall.")]
            public bool Visible
            {
                get
                {
                    return base.Visible;
                }
                set
                {
                    base.Visible = value;
                }
            }
        }

        internal class WallsEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Walls walls = (Walls) value;
                bool flag = ChartEditor.ShowModal(walls.chart, ChartEditorTabs.Walls);
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return flag;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }
    }
}

