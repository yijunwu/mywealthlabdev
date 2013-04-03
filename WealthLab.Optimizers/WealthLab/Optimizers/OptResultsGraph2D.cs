namespace WealthLab.Optimizers
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using WealthLab;

    public class OptResultsGraph2D : UserControl
    {
        private OptimizationResultList _results;
        private WealthScript _ws;
        private ComboBox cmbMetric;
        private ComboBox cmbParameter1;
        private ComboBox cmbParameter2;
        private ComboBox cmbSymbol;
        private IContainer components;
        private TChart graph;
        private Label lblBy;
        private Label lblMetric;
        private Label lblParameter;
        private Label lblSymbol;
        private ToolStripMenuItem mniCopyToClipboard;
        private ToolStripMenuItem mniPrint;
        private System.Windows.Forms.Panel pnlTop;
        private ContextMenuStrip popupGraph2d;
        private Rotate rotate1;
        private Surface surface1;

        public OptResultsGraph2D()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GenerateGraph(object sender, EventArgs e)
        {
            if ((((((this._results != null) && (this.cmbParameter1.SelectedIndex != -1)) && (this.cmbMetric.SelectedIndex != -1)) && (this.cmbParameter2.SelectedIndex != -1)) && (this.cmbParameter1.SelectedIndex != this.cmbParameter2.SelectedIndex)) && (this.cmbSymbol.SelectedIndex != -1))
            {
                StrategyParameter selectedItem = this.cmbParameter1.SelectedItem as StrategyParameter;
                int index = this._ws.Parameters.IndexOf(selectedItem);
                StrategyParameter item = this.cmbParameter2.SelectedItem as StrategyParameter;
                int num2 = this._ws.Parameters.IndexOf(item);
                string metric = this.cmbMetric.SelectedItem as string;
                List<double> values = new List<double>();
                foreach (StrategyParameter parameter3 in this._ws.Parameters)
                {
                    values.Add(parameter3.Value);
                }
                if (this.graph.Zoom.Zoomed)
                {
                    this.graph.Zoom.Undo();
                }
                this.surface1.Clear();
                this.surface1.IrregularGrid = true;
                this.graph.Axes.Bottom.Title.Text = selectedItem.Name;
                this.graph.Axes.Left.Title.Text = metric;
                this.graph.Axes.Depth.Title.Text = item.Name;
                double start = selectedItem.Start;
                for (int i = 0; i <= selectedItem.NumberOfRuns; i++)
                {
                    values[index] = start;
                    double z = item.Start;
                    for (int j = 0; j <= item.NumberOfRuns; j++)
                    {
                        values[num2] = z;
                        double d = this._results.FindMetric(this.cmbSymbol.Text, metric, values);
                        if (!double.IsNaN(d))
                        {
                            if (double.IsInfinity(d))
                            {
                                d = 0.0;
                            }
                            this.surface1.Add(start, d, z);
                        }
                        z += item.Step;
                    }
                    start += selectedItem.Step;
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            new ComponentResourceManager(typeof(OptResultsGraph2D));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.cmbSymbol = new ComboBox();
            this.lblSymbol = new Label();
            this.lblBy = new Label();
            this.cmbParameter2 = new ComboBox();
            this.lblMetric = new Label();
            this.cmbMetric = new ComboBox();
            this.cmbParameter1 = new ComboBox();
            this.lblParameter = new Label();
            this.graph = new TChart();
            this.popupGraph2d = new ContextMenuStrip(this.components);
            this.mniCopyToClipboard = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.surface1 = new Surface();
            this.rotate1 = new Rotate();
            this.pnlTop.SuspendLayout();
            this.popupGraph2d.SuspendLayout();
            base.SuspendLayout();
            this.pnlTop.Controls.Add(this.cmbSymbol);
            this.pnlTop.Controls.Add(this.lblSymbol);
            this.pnlTop.Controls.Add(this.lblBy);
            this.pnlTop.Controls.Add(this.cmbParameter2);
            this.pnlTop.Controls.Add(this.lblMetric);
            this.pnlTop.Controls.Add(this.cmbMetric);
            this.pnlTop.Controls.Add(this.cmbParameter1);
            this.pnlTop.Controls.Add(this.lblParameter);
            this.pnlTop.Dock = DockStyle.Top;
            this.pnlTop.Location = new Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new Size(0x261, 0x3d);
            this.pnlTop.TabIndex = 1;
            this.cmbSymbol.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSymbol.FormattingEnabled = true;
            this.cmbSymbol.Location = new Point(0x39, 4);
            this.cmbSymbol.Name = "cmbSymbol";
            this.cmbSymbol.Size = new Size(0x4c, 0x15);
            this.cmbSymbol.TabIndex = 7;
            this.cmbSymbol.SelectedIndexChanged += new EventHandler(this.GenerateGraph);
            this.lblSymbol.AutoSize = true;
            this.lblSymbol.Location = new Point(7, 7);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new Size(0x2c, 13);
            this.lblSymbol.TabIndex = 6;
            this.lblSymbol.Text = "Symbol:";
            this.lblBy.AutoSize = true;
            this.lblBy.Location = new Point(0xcb, 0x25);
            this.lblBy.Name = "lblBy";
            this.lblBy.Size = new Size(0x12, 13);
            this.lblBy.TabIndex = 5;
            this.lblBy.Text = "by";
            this.cmbParameter2.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbParameter2.FormattingEnabled = true;
            this.cmbParameter2.Location = new Point(0xe3, 0x22);
            this.cmbParameter2.Name = "cmbParameter2";
            this.cmbParameter2.Size = new Size(0x79, 0x15);
            this.cmbParameter2.TabIndex = 4;
            this.cmbParameter2.SelectedIndexChanged += new EventHandler(this.GenerateGraph);
            this.lblMetric.AutoSize = true;
            this.lblMetric.Location = new Point(0x8b, 7);
            this.lblMetric.Name = "lblMetric";
            this.lblMetric.Size = new Size(0x27, 13);
            this.lblMetric.TabIndex = 3;
            this.lblMetric.Text = "Metric:";
            this.cmbMetric.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMetric.FormattingEnabled = true;
            this.cmbMetric.Location = new Point(0xb8, 4);
            this.cmbMetric.Name = "cmbMetric";
            this.cmbMetric.Size = new Size(0x6c, 0x15);
            this.cmbMetric.TabIndex = 2;
            this.cmbMetric.SelectedIndexChanged += new EventHandler(this.GenerateGraph);
            this.cmbParameter1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbParameter1.FormattingEnabled = true;
            this.cmbParameter1.Location = new Point(0x4c, 0x22);
            this.cmbParameter1.Name = "cmbParameter1";
            this.cmbParameter1.Size = new Size(0x79, 0x15);
            this.cmbParameter1.TabIndex = 1;
            this.cmbParameter1.SelectedIndexChanged += new EventHandler(this.GenerateGraph);
            this.lblParameter.AutoSize = true;
            this.lblParameter.Location = new Point(7, 0x25);
            this.lblParameter.Name = "lblParameter";
            this.lblParameter.Size = new Size(0x3f, 13);
            this.lblParameter.TabIndex = 0;
            this.lblParameter.Text = "Parameters:";
            this.graph.Aspect.Chart3DPercent = 50;
            this.graph.Aspect.ColorPaletteIndex = 0;
            this.graph.Aspect.ZOffset = 0.0;
            this.graph.Aspect.Zoom = 0x5f;
            this.graph.Aspect.ZoomFloat = 95.0;
            this.graph.Axes.Bottom.Title.Caption = "Parameter Value";
            this.graph.Axes.Bottom.Title.Lines = new string[] { "Parameter Value" };
            this.graph.Axes.Depth.Visible = true;
            this.graph.Axes.Left.Title.Caption = "Net Profit";
            this.graph.Axes.Left.Title.Lines = new string[] { "Net Profit" };
            this.graph.ContextMenuStrip = this.popupGraph2d;
            this.graph.Dock = DockStyle.Fill;
            this.graph.Header.Lines = new string[] { "" };
            this.graph.Legend.Visible = false;
            this.graph.Location = new Point(0, 0x3d);
            this.graph.Name = "graph";
            this.graph.Panning.Allow = ScrollModes.None;
            this.graph.Series.Add(this.surface1);
            this.graph.Size = new Size(0x261, 0x17b);
            this.graph.TabIndex = 2;
            this.graph.Tools.Add(this.rotate1);
            this.popupGraph2d.Items.AddRange(new ToolStripItem[] { this.mniCopyToClipboard, this.mniPrint });
            this.popupGraph2d.Name = "popupGraph2d";
            this.popupGraph2d.Size = new Size(0xaf, 0x30);
            this.mniCopyToClipboard.Name = "mniCopyToClipboard";
            this.mniCopyToClipboard.Size = new Size(0xae, 0x16);
            this.mniCopyToClipboard.Text = "Copy To Clipboard";
            this.mniCopyToClipboard.Click += new EventHandler(this.mniCopyToClipboard_Click);
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0xae, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.surface1.Brush.Color = Color.FromArgb(0x44, 0x66, 0xa3);
            this.surface1.Color = Color.FromArgb(0x44, 0x66, 0xa3);
            this.surface1.ColorEach = false;
            this.surface1.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.surface1.Marks.Callout.ArrowHeadSize = 8;
            this.surface1.Marks.Callout.Brush.Color = Color.Black;
            this.surface1.Marks.Callout.Distance = 0;
            this.surface1.Marks.Callout.Draw3D = false;
            this.surface1.Marks.Callout.Length = 10;
            this.surface1.Marks.Callout.Style = PointerStyles.Rectangle;
            this.surface1.Marks.Callout.Visible = false;
            this.surface1.PaletteMin = 0.0;
            this.surface1.PaletteStep = 0.0;
            this.surface1.PaletteStyle = PaletteStyles.Pale;
            this.surface1.Title = "surface1";
            this.surface1.XValues.DataMember = "X";
            this.surface1.YValues.DataMember = "Y";
            this.surface1.ZValues.DataMember = "Z";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.graph);
            base.Controls.Add(this.pnlTop);
            base.Name = "OptResultsGraph2D";
            base.Size = new Size(0x261, 440);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.popupGraph2d.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void mniCopyToClipboard_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetImage(this.graph.Bitmap);
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void mniPrint_Click(object sender, EventArgs e)
        {
            if (this.PrintHost != null)
            {
                Cursor cursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                this.PrintHost.Print("Optimization 2 Parameter Graph", this.graph.Bitmap, true);
                this.Cursor = cursor;
            }
            else
            {
                MessageBox.Show("Cannot print until optimization has completed");
            }
        }

        public void RefreshView()
        {
            this.GenerateGraph(this, EventArgs.Empty);
        }

        internal void UpdateResults(OptimizationResultList results, WealthScript ws)
        {
            this._results = results;
            this._ws = ws;
            this.cmbParameter1.Items.Clear();
            this.cmbParameter2.Items.Clear();
            foreach (StrategyParameter parameter in ws.Parameters)
            {
                this.cmbParameter1.Items.Add(parameter);
                this.cmbParameter2.Items.Add(parameter);
            }
            this.cmbMetric.Items.Clear();
            foreach (string str in results.Names)
            {
                this.cmbMetric.Items.Add(str);
            }
            this.cmbSymbol.Items.Clear();
            if (this._results.Symbols.Count > 1)
            {
                this.cmbSymbol.Items.Add("(Average)");
            }
            foreach (string str2 in this._results.Symbols)
            {
                this.cmbSymbol.Items.Add(str2);
            }
            if (this.cmbParameter1.Items.Count > 0)
            {
                this.cmbParameter1.SelectedIndex = 0;
            }
            if (this.cmbMetric.Items.Count > 0)
            {
                this.cmbMetric.SelectedIndex = 0;
            }
            if (this.cmbParameter2.Items.Count > 1)
            {
                this.cmbParameter2.SelectedIndex = 1;
            }
            if (this.cmbSymbol.Items.Count > 1)
            {
                this.cmbSymbol.SelectedIndex = 1;
            }
            else if (this.cmbSymbol.Items.Count > 0)
            {
                this.cmbSymbol.SelectedIndex = 0;
            }
        }

        internal IPrintHost PrintHost { get; set; }
    }
}

