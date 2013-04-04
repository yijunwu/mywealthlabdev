namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MarkSymbolEditor : CustomShapeEditor
    {
        private CheckBox cBVisible;
        private IContainer components;

        public MarkSymbolEditor()
        {
            this.InitializeComponent();
        }

        public MarkSymbolEditor(SeriesMarks.Symbols s) : this()
        {
            base.bShape = s;
            base.numericUpDown1.Value = base.bShape.Transparency;
            base.CBTransparent.Checked = base.bShape.Transparent;
            base.tabFormat.Controls.Add(this.cBVisible);
            this.cBVisible.Checked = base.bShape.Visible;
            base.BBackColor.Enabled = false;
            base.BBackColor.Visible = false;
            base.CBRound.Enabled = false;
            base.CBRound.Visible = false;
            base.Button4.Pen = base.bShape.Pen;
            TabControl control = new TabControl();
            base.tabText.Parent = control;
            base.tabBevels.Parent = control;
            base.PC1.SelectedTab = base.tabFormat;
            base.Translate();
        }

        public static MarkSymbolEditor Add(TabControl tabControl, SeriesMarks.Symbols s)
        {
            MarkSymbolEditor editor = new MarkSymbolEditor(s) {
                Dock = DockStyle.Fill
            };
            tabControl.TabPages.Add(editor.tabFormat);
            tabControl.TabPages.Add(editor.tabGradient);
            tabControl.TabPages.Add(editor.tabShadow);
            editor.gradientEditor = new GradientEditor(s.Gradient, editor.tabGradient);
            EditorUtils.Translate(editor.gradientEditor);
            editor.shadowEditor = new ShadowEditor(s.Shadow, editor.tabShadow);
            EditorUtils.Translate(editor.shadowEditor);
            return editor;
        }

        private void cBVisible_Click(object sender, EventArgs e)
        {
            base.bShape.Visible = this.cBVisible.Checked;
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
            this.cBVisible = new CheckBox();
            base.numericUpDown1.BeginInit();
            base.BBackColor.Location = new Point(0xb8, 0x80);
            base.BBackColor.Name = "BBackColor";
            base.CBTransparent.Name = "CBTransparent";
            base.Button4.Name = "Button4";
            base.numericUpDown1.Name = "numericUpDown1";
            this.cBVisible.FlatStyle = FlatStyle.Flat;
            this.cBVisible.Location = new Point(0x10, 0x10);
            this.cBVisible.Name = "cBVisible";
            this.cBVisible.TabIndex = 7;
            this.cBVisible.Text = "Visible";
            this.cBVisible.Click += new EventHandler(this.cBVisible_Click);
            base.ClientSize = new Size(0x130, 0xc5);
            base.Name = "MarkSymbolEditor";
            base.numericUpDown1.EndInit();
        }
    }
}

