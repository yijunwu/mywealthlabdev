namespace WealthLab.Optimizers
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using WealthLab;

    public class OptResultsGraph1D : UserControl
    {
        private OptimizationResultList _results;
        private WealthScript _ws;
        private Bar bar1;
        private ComboBox cmbMetric;
        private ComboBox cmbParameters;
        private ComboBox cmbSymbol;
        private IContainer components;
        private TChart graph;
        private Label lblMetric;
        private Label lblParameter;
        private Label lblSymbol;
        private ToolStripMenuItem mniCopyToClipboard;
        private ToolStripMenuItem mniPrint;
        private System.Windows.Forms.Panel pnlTop;
        private ContextMenuStrip popupGraph1D;

        public OptResultsGraph1D()
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
            if ((((this._results != null) && (this.cmbParameters.SelectedIndex != -1)) && (this.cmbMetric.SelectedIndex != -1)) && (this.cmbSymbol.SelectedIndex != -1))
            {
                if (this.graph.Zoom.Zoomed)
                {
                    this.graph.Zoom.Undo();
                }
                StrategyParameter selectedItem = this.cmbParameters.SelectedItem as StrategyParameter;
                string metric = this.cmbMetric.SelectedItem as string;
                this.bar1.Clear();
                this.graph.Axes.Bottom.Title.Text = selectedItem.Name;
                this.graph.Axes.Left.Title.Text = metric;
                double start = selectedItem.Start;
                for (int i = 0; i <= selectedItem.NumberOfRuns; i++)
                {
                    double d = this._results.FindMetric(this.cmbSymbol.Text, metric, this._ws, selectedItem, start);
                    if (!double.IsNaN(d))
                    {
                        if (double.IsInfinity(d))
                        {
                            d = 0.0;
                        }
                        this.bar1.Add(start, d);
                    }
                    start += selectedItem.Step;
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            new ComponentResourceManager(typeof(OptResultsGraph1D));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.cmbSymbol = new ComboBox();
            this.lblSymbol = new Label();
            this.lblMetric = new Label();
            this.cmbMetric = new ComboBox();
            this.cmbParameters = new ComboBox();
            this.lblParameter = new Label();
            this.graph = new TChart();
            this.popupGraph1D = new ContextMenuStrip(this.components);
            this.mniCopyToClipboard = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.bar1 = new Bar();
            this.pnlTop.SuspendLayout();
            this.popupGraph1D.SuspendLayout();
            base.SuspendLayout();
            this.pnlTop.Controls.Add(this.cmbSymbol);
            this.pnlTop.Controls.Add(this.lblSymbol);
            this.pnlTop.Controls.Add(this.lblMetric);
            this.pnlTop.Controls.Add(this.cmbMetric);
            this.pnlTop.Controls.Add(this.cmbParameters);
            this.pnlTop.Controls.Add(this.lblParameter);
            this.pnlTop.Dock = DockStyle.Top;
            this.pnlTop.Location = new Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new Size(0x24d, 0x1f);
            this.pnlTop.TabIndex = 0;
            this.cmbSymbol.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSymbol.FormattingEnabled = true;
            this.cmbSymbol.Location = new Point(0x35, 4);
            this.cmbSymbol.Name = "cmbSymbol";
            this.cmbSymbol.Size = new Size(0x4b, 0x15);
            this.cmbSymbol.TabIndex = 5;
            this.cmbSymbol.SelectedIndexChanged += new EventHandler(this.GenerateGraph);
            this.lblSymbol.AutoSize = true;
            this.lblSymbol.Location = new Point(3, 7);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new Size(0x2c, 13);
            this.lblSymbol.TabIndex = 4;
            this.lblSymbol.Text = "Symbol:";
            this.lblMetric.AutoSize = true;
            this.lblMetric.Location = new Point(0x145, 7);
            this.lblMetric.Name = "lblMetric";
            this.lblMetric.Size = new Size(0x27, 13);
            this.lblMetric.TabIndex = 3;
            this.lblMetric.Text = "Metric:";
            this.cmbMetric.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMetric.FormattingEnabled = true;
            this.cmbMetric.Location = new Point(370, 4);
            this.cmbMetric.Name = "cmbMetric";
            this.cmbMetric.Size = new Size(0x66, 0x15);
            this.cmbMetric.TabIndex = 2;
            this.cmbMetric.SelectedIndexChanged += new EventHandler(this.GenerateGraph);
            this.cmbParameters.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbParameters.FormattingEnabled = true;
            this.cmbParameters.Location = new Point(0xc6, 4);
            this.cmbParameters.Name = "cmbParameters";
            this.cmbParameters.Size = new Size(0x79, 0x15);
            this.cmbParameters.TabIndex = 1;
            this.cmbParameters.SelectedIndexChanged += new EventHandler(this.GenerateGraph);
            this.lblParameter.AutoSize = true;
            this.lblParameter.Location = new Point(0x86, 7);
            this.lblParameter.Name = "lblParameter";
            this.lblParameter.Size = new Size(0x3a, 13);
            this.lblParameter.TabIndex = 0;
            this.lblParameter.Text = "Parameter:";
            this.graph.Aspect.ColorPaletteIndex = 0;
            this.graph.Aspect.ZOffset = 0.0;
            this.graph.Axes.Bottom.MaximumOffset = 50;
            this.graph.Axes.Bottom.MinimumOffset = 50;
            this.graph.Axes.Bottom.Title.Caption = "Parameter Value";
            this.graph.Axes.Bottom.Title.Lines = new string[] { "Parameter Value" };
            this.graph.Axes.Left.MaximumOffset = 0x25;
            this.graph.Axes.Left.Title.Caption = "Net Profit";
            this.graph.Axes.Left.Title.Lines = new string[] { "Net Profit" };
            this.graph.ContextMenuStrip = this.popupGraph1D;
            this.graph.Dock = DockStyle.Fill;
            this.graph.Header.Lines = new string[] { "" };
            this.graph.Legend.Visible = false;
            this.graph.Location = new Point(0, 0x1f);
            this.graph.Name = "graph";
            this.graph.Panning.Allow = ScrollModes.None;
            this.graph.Series.Add(this.bar1);
            this.graph.Size = new Size(0x24d, 0x167);
            this.graph.TabIndex = 1;
            this.popupGraph1D.Items.AddRange(new ToolStripItem[] { this.mniCopyToClipboard, this.mniPrint });
            this.popupGraph1D.Name = "popupGraph1D";
            this.popupGraph1D.Size = new Size(0xac, 0x30);
            this.mniCopyToClipboard.Name = "mniCopyToClipboard";
            this.mniCopyToClipboard.Size = new Size(0xab, 0x16);
            this.mniCopyToClipboard.Text = "Copy to Clipboard";
            this.mniCopyToClipboard.Click += new EventHandler(this.mniCopyToClipboard_Click);
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0xab, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.bar1.Brush.Color = Color.Red;
            this.bar1.Color = Color.Red;
            this.bar1.ColorEach = false;
            this.bar1.Marks.Callout.ArrowHead = ArrowHeadStyles.None;
            this.bar1.Marks.Callout.ArrowHeadSize = 8;
            this.bar1.Marks.Callout.Brush.Color = Color.Black;
            this.bar1.Marks.Callout.Distance = 0;
            this.bar1.Marks.Callout.Draw3D = false;
            this.bar1.Marks.Callout.Length = 20;
            this.bar1.Marks.Callout.Style = PointerStyles.Rectangle;
            this.bar1.Marks.Callout.Visible = false;
            this.bar1.Pen.Color = Color.FromArgb(0x99, 0, 0);
            this.bar1.Title = "bar1";
            this.bar1.XValues.DataMember = "X";
            this.bar1.XValues.Order = ValueListOrder.Ascending;
            this.bar1.YValues.DataMember = "Bar";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.graph);
            base.Controls.Add(this.pnlTop);
            base.Name = "OptResultsGraph1D";
            base.Size = new Size(0x24d, 390);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.popupGraph1D.ResumeLayout(false);
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
                this.PrintHost.Print("Optimization 1 Parameter Graph", this.graph.Bitmap, true);
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
            this.cmbParameters.Items.Clear();
            foreach (StrategyParameter parameter in ws.Parameters)
            {
                this.cmbParameters.Items.Add(parameter);
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
            if (this.cmbParameters.Items.Count > 0)
            {
                this.cmbParameters.SelectedIndex = 0;
            }
            if (this.cmbMetric.Items.Count > 0)
            {
                this.cmbMetric.SelectedIndex = 0;
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

