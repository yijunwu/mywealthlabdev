namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SelectorEditor : Form, IStopComboBoxTranslate
    {
        private ButtonColor buttonColor1;
        private ButtonPen buttonPen1;
        private CheckBox cbAllowDrag;
        private ComboBox cbCursor;
        private CheckBox cbResizeChart;
        private Container components;
        private Label label1;
        private Label label2;
        private Selector selector;
        private NumericUpDown udSize;

        public SelectorEditor()
        {
            this.InitializeComponent();
        }

        public SelectorEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            if ((s != null) && (s is Selector))
            {
                this.selector = s as Selector;
                this.buttonColor1.Color = this.selector.Brush.Color;
                this.buttonPen1.Pen = this.selector.Pen;
                this.udSize.Value = this.selector.HandleSize;
                this.cbAllowDrag.Checked = this.selector.AllowDrag;
                this.cbResizeChart.Checked = this.selector.AllowResizeChart;
                EditorUtils.FillCursors(this.cbCursor, this.selector.Cursor);
            }
        }

        private void cbAllowDrag_Click(object sender, EventArgs e)
        {
            if (this.selector != null)
            {
                this.selector.AllowDrag = this.cbAllowDrag.Checked;
            }
        }

        private void cbCursor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.selector != null)
            {
                this.selector.Cursor = EditorUtils.StringToCursor(this.cbCursor.SelectedItem.ToString());
            }
        }

        private void cbResizeChart_Click(object sender, EventArgs e)
        {
            if (this.selector != null)
            {
                this.selector.AllowResizeChart = this.cbResizeChart.Checked;
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

        public ComboBox[] GetComboBoxes()
        {
            return new ComboBox[] { this.cbCursor };
        }

        private void InitializeComponent()
        {
            this.buttonColor1 = new ButtonColor();
            this.buttonPen1 = new ButtonPen();
            this.cbAllowDrag = new CheckBox();
            this.cbResizeChart = new CheckBox();
            this.udSize = new NumericUpDown();
            this.label1 = new Label();
            this.cbCursor = new ComboBox();
            this.label2 = new Label();
            this.udSize.BeginInit();
            base.SuspendLayout();
            this.buttonColor1.Color = Color.Empty;
            this.buttonColor1.Location = new Point(0x10, 40);
            this.buttonColor1.Name = "buttonColor1";
            this.buttonColor1.Size = new Size(0x60, 0x17);
            this.buttonColor1.TabIndex = 0;
            this.buttonColor1.Text = "&Color...";
            this.buttonPen1.FlatStyle = FlatStyle.Flat;
            this.buttonPen1.Location = new Point(0x10, 8);
            this.buttonPen1.Name = "buttonPen1";
            this.buttonPen1.Size = new Size(0x60, 0x17);
            this.buttonPen1.TabIndex = 1;
            this.buttonPen1.Text = "&Handles...";
            this.cbAllowDrag.Location = new Point(0x10, 80);
            this.cbAllowDrag.Name = "cbAllowDrag";
            this.cbAllowDrag.TabIndex = 2;
            this.cbAllowDrag.Text = "&Allow Drag";
            this.cbAllowDrag.Click += new EventHandler(this.cbAllowDrag_Click);
            this.cbResizeChart.Location = new Point(0x10, 0x68);
            this.cbResizeChart.Name = "cbResizeChart";
            this.cbResizeChart.Size = new Size(0x90, 0x18);
            this.cbResizeChart.TabIndex = 3;
            this.cbResizeChart.Text = "Allow &Resize Chart";
            this.cbResizeChart.Click += new EventHandler(this.cbResizeChart_Click);
            this.udSize.Location = new Point(0x88, 0x88);
            this.udSize.Name = "udSize";
            this.udSize.Size = new Size(0x30, 20);
            this.udSize.TabIndex = 4;
            int[] bits = new int[4];
            bits[0] = 3;
            this.udSize.Value = new decimal(bits);
            this.udSize.ValueChanged += new EventHandler(this.udSize_ValueChanged);
            this.label1.Location = new Point(0x5d, 0x8a);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x27, 0x10);
            this.label1.TabIndex = 5;
            this.label1.Text = "Size:";
            this.cbCursor.Location = new Point(0x10, 0xb8);
            this.cbCursor.Name = "cbCursor";
            this.cbCursor.Size = new Size(0x79, 0x15);
            this.cbCursor.TabIndex = 6;
            this.cbCursor.SelectedIndexChanged += new EventHandler(this.cbCursor_SelectedIndexChanged);
            this.label2.Location = new Point(0x10, 160);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x38, 0x10);
            this.label2.TabIndex = 7;
            this.label2.Text = "Cursor:";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xd8, 0xde);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.cbCursor);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.udSize);
            base.Controls.Add(this.cbResizeChart);
            base.Controls.Add(this.cbAllowDrag);
            base.Controls.Add(this.buttonPen1);
            base.Controls.Add(this.buttonColor1);
            base.Name = "SelectorEditor";
            this.Text = "SelectorEditor";
            this.udSize.EndInit();
            base.ResumeLayout(false);
        }

        private void udSize_ValueChanged(object sender, EventArgs e)
        {
            if (this.selector != null)
            {
                this.selector.HandleSize = Convert.ToInt32(this.udSize.Value);
            }
        }
    }
}

