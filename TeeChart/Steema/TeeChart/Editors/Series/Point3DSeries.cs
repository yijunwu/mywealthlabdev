namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Point3DSeries : BaseSeriesForm
    {
        private ButtonPen BaseLineButton;
        private ButtonColor BColor;
        private ButtonPen Button1;
        private CheckBox CBColorEach;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Steema.TeeChart.Editors.SeriesPointer pointerEditor;
        private Points3D points3D;
        private NumericUpDown UDPointDepth;

        public Point3DSeries()
        {
            this.InitializeComponent();
        }

        public Point3DSeries(Series s) : this()
        {
            this.points3D = (Points3D) s;
            if (this.points3D != null)
            {
                this.CBColorEach.Checked = this.points3D.ColorEach;
                this.UDPointDepth.Value = (decimal) this.points3D.DepthSize;
                this.BColor.Color = this.points3D.Color;
                this.BColor.Enabled = !this.points3D.ColorEach;
                this.Button1.Pen = this.points3D.LinePen;
                this.BaseLineButton.Pen = this.points3D.BaseLine;
            }
        }

        private void BColor_Click(object sender, EventArgs e)
        {
            this.points3D.Color = this.BColor.Color;
            this.points3D.ColorEach = false;
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.points3D.ColorEach = this.CBColorEach.Checked;
            this.BColor.Enabled = !this.points3D.ColorEach;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.CBColorEach = new CheckBox();
            this.BColor = new ButtonColor();
            this.Button1 = new ButtonPen();
            this.UDPointDepth = new NumericUpDown();
            this.label1 = new Label();
            this.BaseLineButton = new ButtonPen();
            this.groupBox1.SuspendLayout();
            this.UDPointDepth.BeginInit();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.CBColorEach);
            this.groupBox1.Controls.Add(this.BColor);
            this.groupBox1.Location = new Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x80, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(0x10, 0x38);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x68, 0x10);
            this.CBColorEach.TabIndex = 1;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.BColor.Color = Color.Empty;
            this.BColor.Location = new Point(0x10, 0x10);
            this.BColor.Name = "BColor";
            this.BColor.TabIndex = 0;
            this.BColor.Text = "&Color...";
            this.BColor.Click += new EventHandler(this.BColor_Click);
            this.Button1.FlatStyle = FlatStyle.Flat;
            this.Button1.Location = new Point(0x98, 0x10);
            this.Button1.Name = "Button1";
            this.Button1.TabIndex = 1;
            this.Button1.Text = "&Line...";
            this.UDPointDepth.BorderStyle = BorderStyle.FixedSingle;
            this.UDPointDepth.Location = new Point(0xb3, 0x38);
            this.UDPointDepth.Name = "UDPointDepth";
            this.UDPointDepth.Size = new Size(0x30, 20);
            this.UDPointDepth.TabIndex = 3;
            this.UDPointDepth.TextAlign = HorizontalAlignment.Right;
            this.UDPointDepth.TextChanged += new EventHandler(this.UDPointDepth_ValueChanged);
            this.UDPointDepth.ValueChanged += new EventHandler(this.UDPointDepth_ValueChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x88, 60);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x26, 0x10);
            this.label1.TabIndex = 2;
            this.label1.Text = "Depth:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.BaseLineButton.FlatStyle = FlatStyle.Flat;
            this.BaseLineButton.Location = new Point(140, 0x60);
            this.BaseLineButton.Name = "BaseLineButton";
            this.BaseLineButton.Size = new Size(0x58, 0x17);
            this.BaseLineButton.TabIndex = 4;
            this.BaseLineButton.Text = "&Base Line...";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xf1, 0x8e);
            base.Controls.Add(this.BaseLineButton);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.UDPointDepth);
            base.Controls.Add(this.Button1);
            base.Controls.Add(this.groupBox1);
            base.Name = "Point3DSeries";
            this.groupBox1.ResumeLayout(false);
            this.UDPointDepth.EndInit();
            base.ResumeLayout(false);
        }

        public override void SetParent(TabPage Parent)
        {
            if ((this.points3D != null) && (this.pointerEditor == null))
            {
                this.pointerEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(Parent, this.points3D.Pointer);
            }
        }

        private void UDPointDepth_ValueChanged(object sender, EventArgs e)
        {
            this.points3D.DepthSize = (double) this.UDPointDepth.Value;
        }
    }
}

