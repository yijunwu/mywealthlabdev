namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MapSeries : BaseSeriesForm
    {
        private Button BBrush;
        private Button BGradient;
        private Button BMapBrush;
        private ButtonColor buttonColor1;
        private ButtonPen ButtonPen1;
        private ButtonPen buttonPen2;
        private CheckBox CBClosed;
        private CheckBox CBGlobalBrush;
        private CheckBox CBGlobalPen;
        private CheckBox CBVisible;
        private bool ChangingText;
        private ChartListBox ChartListBox1;
        private IContainer components;
        private TextBox EditZ;
        private TextBox EText;
        private Grid3DSeries grid3DEditor;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private System.Windows.Forms.Panel panel1;
        private Map series;
        private Label ShapeIndex;
        private Splitter splitter1;
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TabPage TabFormat;
        private TabPage TabGlobal;
        private TabPage TabShapes;
        private TabPage TabValues;
        private NumericUpDown UDTransp;

        public MapSeries()
        {
            this.InitializeComponent();
            EditorUtils.Translate(this);
        }

        public MapSeries(Series s) : this()
        {
            this.series = (Map) s;
            this.ChangingText = false;
            if (this.series != null)
            {
                this.ButtonPen1.Pen = this.series.Pen;
                this.tabControl1.SelectedTab = this.TabGlobal;
                this.EnableTabs();
                this.tabControl2.SelectedTab = this.TabFormat;
                if (this.series.Shapes.Count > 0)
                {
                    this.tabControl2.SelectedTab = this.TabShapes;
                }
            }
        }

        private void BBrush_Click(object sender, EventArgs e)
        {
            if (this.SelectedShape() != null)
            {
                BrushEditor.Edit(this.SelectedShape().Brush, true);
                this.SetCustomBrush();
            }
        }

        private void BGradient_Click(object sender, EventArgs e)
        {
            if (this.SelectedShape() != null)
            {
                GradientEditor.Edit(this.SelectedShape().Gradient);
                this.SetCustomBrush();
            }
        }

        private void BMapBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush, true);
        }

        private void buttonColor1_Click(object sender, EventArgs e)
        {
            if (this.SelectedShape() != null)
            {
                this.SelectedShape().Color = this.buttonColor1.Color;
                this.SetCustomBrush();
            }
        }

        private void ButtonPen1_Click(object sender, EventArgs e)
        {
            this.series.Pen = this.ButtonPen1.Pen;
        }

        private void buttonPen2_Click(object sender, EventArgs e)
        {
            if (this.SelectedShape() != null)
            {
                this.SelectedShape().Pen = this.buttonPen2.Pen;
                this.SetCustomPen();
            }
        }

        private void CBClosed_Click(object sender, EventArgs e)
        {
            if (this.SelectedShape() != null)
            {
                this.SelectedShape().Closed = this.CBClosed.Checked;
            }
        }

        private void CBDefaultColor_Click(object sender, EventArgs e)
        {
            if (this.SelectedShape() != null)
            {
                this.SelectedShape().Color = this.series.ValueColor(this.SelectedShape().Index);
                this.buttonColor1.Invalidate();
                this.ChartListBox1.Invalidate();
            }
        }

        private void CBGlobalBrush_Click(object sender, EventArgs e)
        {
            if (this.CBGlobalBrush.Checked)
            {
                this.SetDefaultBrush();
            }
            else
            {
                this.SetCustomBrush();
            }
        }

        private void CBGlobalPen_Click(object sender, EventArgs e)
        {
            if (this.CBGlobalPen.Checked)
            {
                this.SetDefaultPen();
            }
            else
            {
                this.SetCustomPen();
            }
        }

        private void CBVisible_Click(object sender, EventArgs e)
        {
            if (this.SelectedShape() != null)
            {
                this.SelectedShape().Points.Visible = this.CBVisible.Checked;
                this.series.Marks.Items[this.ChartListBox1.SelectedIndex].Visible = this.CBVisible.Checked;
            }
        }

        private void ChartListBox1_ChangeColor(object sender, NotifySeriesEventArgs e)
        {
            this.SetCustomBrush();
        }

        private void ChartListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableTabs();
            Polygon polygon = this.SelectedShape();
            if (polygon != null)
            {
                this.ShapeIndex.Text = polygon.Index.ToString();
                this.buttonPen2.Pen = polygon.Pen;
                if (polygon.ParentBrush)
                {
                    this.buttonColor1.Color = polygon.Color;
                }
                else
                {
                    this.buttonColor1.Color = polygon.Points.Color;
                }
                this.CBGlobalPen.Checked = polygon.ParentPen;
                this.CBGlobalBrush.Checked = polygon.ParentBrush;
                this.ChangingText = true;
                try
                {
                    this.EText.Text = polygon.Text;
                    this.EditZ.Text = polygon.Z.ToString();
                }
                finally
                {
                    this.ChangingText = false;
                }
                this.CBClosed.Checked = polygon.Closed;
                this.CBVisible.Checked = polygon.Visible();
                this.UDTransp.Value = polygon.Transparency;
            }
            else
            {
                this.ShapeIndex.Text = "";
                this.buttonPen2.Pen = null;
                this.buttonColor1.Color = Color.Empty;
                this.ChangingText = true;
                this.EText.Text = "";
                this.EditZ.Text = "0";
                this.ChangingText = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EditZ_TextChanged(object sender, EventArgs e)
        {
            if (!this.ChangingText && (this.SelectedShape() != null))
            {
                double num = Utils.StringToDouble(this.EditZ.Text, 0.0);
                if (this.SelectedShape().Z != num)
                {
                    this.SelectedShape().Z = num;
                    this.series.Invalidate();
                }
            }
        }

        private void EnableTabs()
        {
            bool flag = (this.series != null) && (this.series.Shapes.Count > 0);
            foreach (Control control in this.TabFormat.Controls)
            {
                control.Enabled = flag;
            }
        }

        private void EText_TextChanged(object sender, EventArgs e)
        {
            if ((!this.ChangingText && (this.SelectedShape() != null)) && (this.SelectedShape().Text != this.EText.Text))
            {
                this.SelectedShape().Text = this.EText.Text;
                this.ChartListBox1.Invalidate();
                this.series.Invalidate();
            }
        }

        private void FillShapes()
        {
            this.ChartListBox1.ClearItems();
            this.ChartListBox1.Chart = null;
            for (int i = 0; i < this.series.Count; i++)
            {
                this.ChartListBox1.Items.Add(this.series.Shapes[i].Points);
            }
            if (this.ChartListBox1.Items.Count > 0)
            {
                this.ChartListBox1.SelectedIndex = 0;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.tabControl1 = new TabControl();
            this.TabGlobal = new TabPage();
            this.BMapBrush = new Button();
            this.ButtonPen1 = new ButtonPen();
            this.TabShapes = new TabPage();
            this.tabControl2 = new TabControl();
            this.TabValues = new TabPage();
            this.ShapeIndex = new Label();
            this.label3 = new Label();
            this.EditZ = new TextBox();
            this.EText = new TextBox();
            this.label2 = new Label();
            this.label1 = new Label();
            this.TabFormat = new TabPage();
            this.buttonColor1 = new ButtonColor();
            this.buttonPen2 = new ButtonPen();
            this.label4 = new Label();
            this.UDTransp = new NumericUpDown();
            this.CBVisible = new CheckBox();
            this.CBClosed = new CheckBox();
            this.CBGlobalBrush = new CheckBox();
            this.CBGlobalPen = new CheckBox();
            this.BGradient = new Button();
            this.BBrush = new Button();
            this.splitter1 = new Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ChartListBox1 = new ChartListBox(this.components);
            this.tabControl1.SuspendLayout();
            this.TabGlobal.SuspendLayout();
            this.TabShapes.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.TabValues.SuspendLayout();
            this.TabFormat.SuspendLayout();
            this.UDTransp.BeginInit();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.TabGlobal);
            this.tabControl1.Controls.Add(this.TabShapes);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x178, 0xcb);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.TabGlobal.Controls.Add(this.BMapBrush);
            this.TabGlobal.Controls.Add(this.ButtonPen1);
            this.TabGlobal.Location = new Point(4, 0x16);
            this.TabGlobal.Name = "TabGlobal";
            this.TabGlobal.Size = new Size(0x170, 0xb1);
            this.TabGlobal.TabIndex = 0;
            this.TabGlobal.Text = "Global";
            this.BMapBrush.FlatStyle = FlatStyle.Flat;
            this.BMapBrush.Location = new Point(12, 0x30);
            this.BMapBrush.Name = "BMapBrush";
            this.BMapBrush.TabIndex = 1;
            this.BMapBrush.Text = "B&rush...";
            this.BMapBrush.Click += new EventHandler(this.BMapBrush_Click);
            this.ButtonPen1.FlatStyle = FlatStyle.Flat;
            this.ButtonPen1.Location = new Point(12, 0x10);
            this.ButtonPen1.Name = "ButtonPen1";
            this.ButtonPen1.TabIndex = 0;
            this.ButtonPen1.Text = "&Border...";
            this.ButtonPen1.Click += new EventHandler(this.ButtonPen1_Click);
            this.TabShapes.Controls.Add(this.tabControl2);
            this.TabShapes.Controls.Add(this.splitter1);
            this.TabShapes.Controls.Add(this.panel1);
            this.TabShapes.Location = new Point(4, 0x16);
            this.TabShapes.Name = "TabShapes";
            this.TabShapes.Size = new Size(0x170, 0xb1);
            this.TabShapes.TabIndex = 1;
            this.TabShapes.Text = "Shapes";
            this.tabControl2.Controls.Add(this.TabValues);
            this.tabControl2.Controls.Add(this.TabFormat);
            this.tabControl2.Dock = DockStyle.Fill;
            this.tabControl2.HotTrack = true;
            this.tabControl2.Location = new Point(0x60, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new Size(0x110, 0xb1);
            this.tabControl2.TabIndex = 0;
            this.TabValues.Controls.Add(this.ShapeIndex);
            this.TabValues.Controls.Add(this.label3);
            this.TabValues.Controls.Add(this.EditZ);
            this.TabValues.Controls.Add(this.EText);
            this.TabValues.Controls.Add(this.label2);
            this.TabValues.Controls.Add(this.label1);
            this.TabValues.Location = new Point(4, 0x16);
            this.TabValues.Name = "TabValues";
            this.TabValues.Size = new Size(0x108, 0x97);
            this.TabValues.TabIndex = 1;
            this.TabValues.Text = "Values";
            this.ShapeIndex.Location = new Point(0x6f, 0x59);
            this.ShapeIndex.Name = "ShapeIndex";
            this.ShapeIndex.Size = new Size(0x30, 0x10);
            this.ShapeIndex.TabIndex = 15;
            this.ShapeIndex.Text = "0";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x18, 0x58);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x47, 0x10);
            this.label3.TabIndex = 14;
            this.label3.Text = "Shape Index:";
            this.EditZ.BorderStyle = BorderStyle.FixedSingle;
            this.EditZ.Location = new Point(80, 0x30);
            this.EditZ.Name = "EditZ";
            this.EditZ.Size = new Size(0x70, 20);
            this.EditZ.TabIndex = 13;
            this.EditZ.Text = "0";
            this.EditZ.TextAlign = HorizontalAlignment.Right;
            this.EditZ.TextChanged += new EventHandler(this.EditZ_TextChanged);
            this.EText.BorderStyle = BorderStyle.FixedSingle;
            this.EText.Location = new Point(80, 0x18);
            this.EText.Name = "EText";
            this.EText.Size = new Size(0x70, 20);
            this.EText.TabIndex = 11;
            this.EText.Text = "";
            this.EText.TextChanged += new EventHandler(this.EText_TextChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x3b, 50);
            this.label2.Name = "label2";
            this.label2.Size = new Size(14, 0x10);
            this.label2.TabIndex = 12;
            this.label2.Text = "&Z:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x31, 0x1b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1d, 0x10);
            this.label1.TabIndex = 10;
            this.label1.Text = "&Text:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.TabFormat.Controls.Add(this.buttonColor1);
            this.TabFormat.Controls.Add(this.buttonPen2);
            this.TabFormat.Controls.Add(this.label4);
            this.TabFormat.Controls.Add(this.UDTransp);
            this.TabFormat.Controls.Add(this.CBVisible);
            this.TabFormat.Controls.Add(this.CBClosed);
            this.TabFormat.Controls.Add(this.CBGlobalBrush);
            this.TabFormat.Controls.Add(this.CBGlobalPen);
            this.TabFormat.Controls.Add(this.BGradient);
            this.TabFormat.Controls.Add(this.BBrush);
            this.TabFormat.Location = new Point(4, 0x16);
            this.TabFormat.Name = "TabFormat";
            this.TabFormat.Size = new Size(0x108, 0x97);
            this.TabFormat.TabIndex = 0;
            this.TabFormat.Text = "Format";
            this.buttonColor1.Color = Color.Empty;
            this.buttonColor1.Location = new Point(5, 80);
            this.buttonColor1.Name = "buttonColor1";
            this.buttonColor1.TabIndex = 12;
            this.buttonColor1.Text = "Color...";
            this.buttonColor1.Click += new EventHandler(this.buttonColor1_Click);
            this.buttonPen2.FlatStyle = FlatStyle.Flat;
            this.buttonPen2.Location = new Point(5, 0x10);
            this.buttonPen2.Name = "buttonPen2";
            this.buttonPen2.TabIndex = 11;
            this.buttonPen2.Text = "Border...";
            this.buttonPen2.Click += new EventHandler(this.buttonPen2_Click);
            this.label4.Location = new Point(0x56, 0x79);
            this.label4.Name = "label4";
            this.label4.Size = new Size(80, 0x17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Transparency:";
            this.UDTransp.Location = new Point(0xa5, 0x77);
            this.UDTransp.Name = "UDTransp";
            this.UDTransp.Size = new Size(40, 20);
            this.UDTransp.TabIndex = 9;
            this.UDTransp.TextChanged += new EventHandler(this.UDTransp_ValueChanged);
            this.UDTransp.ValueChanged += new EventHandler(this.UDTransp_ValueChanged);
            this.CBVisible.FlatStyle = FlatStyle.Flat;
            this.CBVisible.Location = new Point(180, 0x54);
            this.CBVisible.Name = "CBVisible";
            this.CBVisible.Size = new Size(0x40, 0x10);
            this.CBVisible.TabIndex = 8;
            this.CBVisible.Text = "&Visible";
            this.CBVisible.Click += new EventHandler(this.CBVisible_Click);
            this.CBClosed.FlatStyle = FlatStyle.Flat;
            this.CBClosed.Location = new Point(180, 0x34);
            this.CBClosed.Name = "CBClosed";
            this.CBClosed.Size = new Size(0x40, 0x10);
            this.CBClosed.TabIndex = 7;
            this.CBClosed.Text = "Closed";
            this.CBClosed.Click += new EventHandler(this.CBClosed_Click);
            this.CBGlobalBrush.FlatStyle = FlatStyle.Flat;
            this.CBGlobalBrush.Location = new Point(0x55, 0x34);
            this.CBGlobalBrush.Name = "CBGlobalBrush";
            this.CBGlobalBrush.Size = new Size(0x40, 0x10);
            this.CBGlobalBrush.TabIndex = 3;
            this.CBGlobalBrush.Text = "Gl&obal";
            this.CBGlobalBrush.Click += new EventHandler(this.CBGlobalBrush_Click);
            this.CBGlobalPen.FlatStyle = FlatStyle.Flat;
            this.CBGlobalPen.Location = new Point(0x55, 20);
            this.CBGlobalPen.Name = "CBGlobalPen";
            this.CBGlobalPen.Size = new Size(0x40, 0x10);
            this.CBGlobalPen.TabIndex = 2;
            this.CBGlobalPen.Text = "&Global";
            this.CBGlobalPen.Click += new EventHandler(this.CBGlobalPen_Click);
            this.BGradient.FlatStyle = FlatStyle.Flat;
            this.BGradient.Location = new Point(0x95, 0x10);
            this.BGradient.Name = "BGradient";
            this.BGradient.Size = new Size(0x65, 0x17);
            this.BGradient.TabIndex = 5;
            this.BGradient.Text = "&Gradient...";
            this.BGradient.Click += new EventHandler(this.BGradient_Click);
            this.BBrush.FlatStyle = FlatStyle.Flat;
            this.BBrush.Location = new Point(5, 0x30);
            this.BBrush.Name = "BBrush";
            this.BBrush.TabIndex = 1;
            this.BBrush.Text = "Br&ush...";
            this.BBrush.Click += new EventHandler(this.BBrush_Click);
            this.splitter1.Location = new Point(0x58, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(8, 0xb1);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            this.panel1.Controls.Add(this.ChartListBox1);
            this.panel1.Dock = DockStyle.Left;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x58, 0xb1);
            this.panel1.TabIndex = 0;
            this.ChartListBox1.AllowDrop = true;
            this.ChartListBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.ChartListBox1.EnableDragSeries = false;
            this.ChartListBox1.Location = new Point(0, 0);
            this.ChartListBox1.Name = "ChartListBox1";
            this.ChartListBox1.OtherItems = null;
            this.ChartListBox1.ShowSeriesIcon = false;
            this.ChartListBox1.Size = new Size(0x58, 0xac);
            this.ChartListBox1.TabIndex = 2;
            this.ChartListBox1.ChangeColor += new ChangeColorEventHandler(this.ChartListBox1_ChangeColor);
            this.ChartListBox1.SelectedIndexChanged += new EventHandler(this.ChartListBox1_SelectedIndexChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x178, 0xcb);
            base.Controls.Add(this.tabControl1);
            base.Name = "MapSeries";
            this.Text = "Map";
            this.tabControl1.ResumeLayout(false);
            this.TabGlobal.ResumeLayout(false);
            this.TabShapes.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.TabValues.ResumeLayout(false);
            this.TabFormat.ResumeLayout(false);
            this.UDTransp.EndInit();
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void SBAdd_Click(object sender, EventArgs e)
        {
        }

        private Polygon SelectedShape()
        {
            if (this.ChartListBox1.SelectedIndex >= 0)
            {
                return this.series.Shapes[this.ChartListBox1.SelectedIndex];
            }
            return null;
        }

        private void SetCustomBrush()
        {
            if (this.SelectedShape() != null)
            {
                this.SelectedShape().ParentBrush = false;
                this.CBGlobalBrush.Checked = false;
                this.series.Invalidate();
                this.ChartListBox1.Refresh();
                this.buttonColor1.Color = this.SelectedShape().Points.Color;
            }
        }

        private void SetCustomPen()
        {
            if (this.SelectedShape() != null)
            {
                this.SelectedShape().ParentPen = false;
                this.CBGlobalPen.Checked = false;
                this.series.Invalidate();
                this.ChartListBox1.Refresh();
            }
        }

        private void SetDefaultBrush()
        {
            if (this.SelectedShape() != null)
            {
                this.SelectedShape().ParentBrush = true;
                this.SelectedShape().Points.Color = this.SelectedShape().Color;
                this.CBGlobalBrush.Checked = true;
                this.series.Invalidate();
                this.ChartListBox1.Refresh();
                this.buttonColor1.Color = this.SelectedShape().Color;
            }
        }

        private void SetDefaultPen()
        {
            if (this.SelectedShape() != null)
            {
                this.SelectedShape().ParentPen = true;
                this.CBGlobalPen.Checked = true;
                this.buttonPen2.Pen = null;
                this.buttonPen2.Pen = this.SelectedShape().ParentSeries.Pen;
                this.buttonPen2.Invalidate();
                this.series.Invalidate();
                this.ChartListBox1.Refresh();
            }
        }

        public override void SetParent(TabPage Parent)
        {
            if ((this.series != null) && (this.grid3DEditor == null))
            {
                this.grid3DEditor = new Grid3DSeries(this.series, Parent);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ChartListBox1.Items.Count != this.series.Shapes.Count)
            {
                this.FillShapes();
            }
        }

        private void UDTransp_ValueChanged(object sender, EventArgs e)
        {
            if (this.SelectedShape() != null)
            {
                this.SelectedShape().Transparency = (int) this.UDTransp.Value;
                this.SetCustomBrush();
            }
        }
    }
}

