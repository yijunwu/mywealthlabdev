namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Themes;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ThemeEditor : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private Chart chart;
        private CheckBox checkBox3D;
        private CheckBox checkBoxScale;
        private ComboBox comboBoxPalette;
        private Container components;
        private bool Is3DChart;
        private bool IsScaledChart;
        private Label label1;
        private ListBox listBoxThemes;
        private System.Windows.Forms.Panel panel1;
        private TPanel panelPreview;
        private Control parent;
        private Splitter splitter1;
        private TabControl tabControlPreview;
        private TabControl tabControlThemes;
        private TabPage tabPagePreview;
        private TabPage tabPageThemes;
        private Chart themePainter;

        public ThemeEditor()
        {
            this.InitializeComponent();
        }

        public ThemeEditor(Chart c, Control p) : this()
        {
            object[] objArray;
            this.chart = c;
            this.parent = p;
            this.InitializeThemePainter();
            if (this.parent != null)
            {
                this.ThemeShowEditor();
                EditorUtils.InsertForm(this, this.parent);
            }
            this.Is3DChart = this.chart.Aspect.View3D;
            this.checkBox3D.Checked = this.Is3DChart;
            this.buttonOK.Enabled = false;
            this.IsScaledChart = true;
            this.AddThemes(out objArray);
            this.listBoxThemes.Items.Clear();
            this.listBoxThemes.Items.Add(Texts.Current);
            this.listBoxThemes.Items.AddRange(objArray);
            this.comboBoxPalette.Items.Clear();
            this.comboBoxPalette.Items.Add(Texts.Default);
            foreach (string str in Theme.ColorPalettes)
            {
                this.comboBoxPalette.Items.Add(str);
            }
            if (this.chart.Aspect.ColorPaletteIndex > -1)
            {
                this.comboBoxPalette.SelectedIndex = this.chart.Aspect.ColorPaletteIndex + 1;
            }
            else
            {
                this.comboBoxPalette.SelectedIndex = 0;
            }
            this.listBoxThemes.SelectedIndex = 0;
        }

        public void AddChartThemes(out object[] ObjectCollection)
        {
            ObjectCollection = new object[Theme.ChartThemes.Count];
            for (int i = 0; i < Theme.ChartThemes.Count; i++)
            {
                ObjectCollection[i] = this.ReturnThemeInstance(Theme.ChartThemes[i]);
            }
        }

        private void AddCustomThemes(ref object[] ObjectCollection)
        {
            int length = ObjectCollection.Length;
            if (Directory.Exists(Utils.ThemeFolder()))
            {
                FileInfo[] files = new DirectoryInfo(Utils.ThemeFolder()).GetFiles("*.xml");
                if (files.Length > 0)
                {
                    ObjectCollection = (object[]) Utils.SetLength(ObjectCollection, length + files.Length, typeof(object));
                    foreach (FileInfo info2 in files)
                    {
                        info2.Attributes = FileAttributes.Normal;
                        ObjectCollection[length] = new CustomThemeObject(info2);
                        length++;
                    }
                }
            }
        }

        private void AddThemes(out object[] ObjectCollection)
        {
            this.AddChartThemes(out ObjectCollection);
            this.AddCustomThemes(ref ObjectCollection);
        }

        private void ApplyThemeAndPalette(object ATheme, int Index, Chart AChart)
        {
            if (ATheme is Theme)
            {
                Theme.ApplyChartTheme(ATheme as Theme, AChart, Index - 1);
            }
            else if (ATheme is Stream)
            {
                Theme.ApplyChartTheme(ATheme as Stream, AChart, Index - 1);
            }
            else if (ATheme == null)
            {
                ColorPalettes.ApplyPalette(AChart, (int) (Index - 1));
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.chart.Aspect.View3D = this.checkBox3D.Checked;
            object aTheme = this.SelectedTheme();
            int selectedIndex = this.comboBoxPalette.SelectedIndex;
            this.ApplyThemeAndPalette(aTheme, selectedIndex, this.chart);
            this.chart.Aspect.ColorPaletteIndex = selectedIndex - 1;
            this.buttonOK.Enabled = false;
        }

        private void checkBox3D_Click(object sender, EventArgs e)
        {
            this.Is3DChart = this.checkBox3D.Checked;
            this.panelPreview.Draw();
            this.buttonOK.Enabled = true;
        }

        private void checkBoxScale_Click(object sender, EventArgs e)
        {
            this.IsScaledChart = this.checkBoxScale.Checked;
            this.panelPreview.Draw();
        }

        private void comboBoxPalette_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panelPreview.Draw();
            this.buttonOK.Enabled = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (object obj2 in this.listBoxThemes.Items)
                {
                    if (obj2 is CustomThemeObject)
                    {
                        (obj2 as CustomThemeObject).Close();
                    }
                }
                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void DrawAChartToPanel(PaintEventArgs g, Chart AChart)
        {
            bool flag = AChart.Aspect.View3D;
            AChart.Aspect.View3D = this.Is3DChart;
            if (this.IsScaledChart)
            {
                int width = AChart.Width;
                int height = AChart.Height;
                AChart.Width = Convert.ToInt32((double) (this.panelPreview.Width * 1.4));
                AChart.Height = Convert.ToInt32((double) (this.panelPreview.Height * 1.4));
                Bitmap image = AChart.Bitmap();
                g.Graphics.DrawImage(image, g.ClipRectangle);
                AChart.Width = width;
                AChart.Height = height;
            }
            else
            {
                AChart.Draw(g.Graphics, g.ClipRectangle);
            }
            AChart.Aspect.View3D = flag;
        }

        internal void DrawChart(PaintEventArgs g)
        {
            object aTheme = this.SelectedTheme();
            int selectedIndex = this.comboBoxPalette.SelectedIndex;
            if ((aTheme == null) && ((selectedIndex - 1) == -1))
            {
                this.DrawAChartToPanel(g, this.chart);
            }
            else
            {
                this.ApplyThemeAndPalette(aTheme, selectedIndex, this.themePainter);
                this.DrawAChartToPanel(g, this.themePainter);
            }
        }

        private void InitializeComponent()
        {
            this.tabControlThemes = new TabControl();
            this.tabPageThemes = new TabPage();
            this.label1 = new Label();
            this.comboBoxPalette = new ComboBox();
            this.listBoxThemes = new ListBox();
            this.splitter1 = new Splitter();
            this.tabControlPreview = new TabControl();
            this.tabPagePreview = new TabPage();
            this.panelPreview = new TPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxScale = new CheckBox();
            this.checkBox3D = new CheckBox();
            this.buttonCancel = new Button();
            this.buttonOK = new Button();
            this.tabControlThemes.SuspendLayout();
            this.tabPageThemes.SuspendLayout();
            this.tabControlPreview.SuspendLayout();
            this.tabPagePreview.SuspendLayout();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.tabControlThemes.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabControlThemes.Controls.Add(this.tabPageThemes);
            this.tabControlThemes.Location = new Point(0, 0);
            this.tabControlThemes.Name = "tabControlThemes";
            this.tabControlThemes.SelectedIndex = 0;
            this.tabControlThemes.Size = new Size(0x70, 200);
            this.tabControlThemes.TabIndex = 0;
            this.tabPageThemes.Controls.Add(this.label1);
            this.tabPageThemes.Controls.Add(this.comboBoxPalette);
            this.tabPageThemes.Controls.Add(this.listBoxThemes);
            this.tabPageThemes.Location = new Point(4, 0x16);
            this.tabPageThemes.Name = "tabPageThemes";
            this.tabPageThemes.Size = new Size(0x68, 0xae);
            this.tabPageThemes.TabIndex = 0;
            this.tabPageThemes.Text = "Themes";
            this.label1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.label1.Location = new Point(1, 0x87);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x59, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Color Palette:";
            this.comboBoxPalette.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.comboBoxPalette.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxPalette.Location = new Point(0, 0x99);
            this.comboBoxPalette.Name = "comboBoxPalette";
            this.comboBoxPalette.Size = new Size(0x69, 0x15);
            this.comboBoxPalette.TabIndex = 1;
            this.comboBoxPalette.SelectedIndexChanged += new EventHandler(this.comboBoxPalette_SelectedIndexChanged);
            this.listBoxThemes.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxThemes.Location = new Point(0, 0);
            this.listBoxThemes.Name = "listBoxThemes";
            this.listBoxThemes.Size = new Size(0x68, 0x86);
            this.listBoxThemes.TabIndex = 0;
            this.listBoxThemes.SelectedIndexChanged += new EventHandler(this.listBoxThemes_SelectedIndexChanged);
            this.splitter1.Dock = DockStyle.Right;
            this.splitter1.Location = new Point(0x70, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(0x108, 0xed);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            this.tabControlPreview.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabControlPreview.Controls.Add(this.tabPagePreview);
            this.tabControlPreview.Location = new Point(0x70, 0);
            this.tabControlPreview.Name = "tabControlPreview";
            this.tabControlPreview.SelectedIndex = 0;
            this.tabControlPreview.Size = new Size(0x108, 200);
            this.tabControlPreview.TabIndex = 2;
            this.tabPagePreview.Controls.Add(this.panelPreview);
            this.tabPagePreview.Location = new Point(4, 0x16);
            this.tabPagePreview.Name = "tabPagePreview";
            this.tabPagePreview.Size = new Size(0x100, 0xae);
            this.tabPagePreview.TabIndex = 0;
            this.tabPagePreview.Text = "Preview";
            this.panelPreview.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panelPreview.Location = new Point(0, 0);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Size = new Size(0x100, 0xae);
            this.panelPreview.TabIndex = 0;
            this.panel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.panel1.Controls.Add(this.checkBoxScale);
            this.panel1.Controls.Add(this.checkBox3D);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Location = new Point(0, 200);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x178, 40);
            this.panel1.TabIndex = 3;
            this.checkBoxScale.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.checkBoxScale.Checked = true;
            this.checkBoxScale.CheckState = CheckState.Checked;
            this.checkBoxScale.FlatStyle = FlatStyle.Flat;
            this.checkBoxScale.Location = new Point(0x4b, 9);
            this.checkBoxScale.Name = "checkBoxScale";
            this.checkBoxScale.Size = new Size(0x43, 0x18);
            this.checkBoxScale.TabIndex = 3;
            this.checkBoxScale.Text = "Scale";
            this.checkBoxScale.Click += new EventHandler(this.checkBoxScale_Click);
            this.checkBox3D.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.checkBox3D.FlatStyle = FlatStyle.Flat;
            this.checkBox3D.Location = new Point(5, 8);
            this.checkBox3D.Name = "checkBox3D";
            this.checkBox3D.Size = new Size(0x43, 0x18);
            this.checkBox3D.TabIndex = 2;
            this.checkBox3D.Text = "View 3D";
            this.checkBox3D.Click += new EventHandler(this.checkBox3D_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.FlatStyle = FlatStyle.Flat;
            this.buttonCancel.Location = new Point(0x128, 9);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.FlatStyle = FlatStyle.Flat;
            this.buttonOK.Location = new Point(0xd8, 9);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            base.ClientSize = new Size(0x178, 0xed);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.tabControlPreview);
            base.Controls.Add(this.splitter1);
            base.Controls.Add(this.tabControlThemes);
            base.Name = "ThemeEditor";
            base.SizeGripStyle = SizeGripStyle.Hide;
            this.Text = "Chart Theme Selector";
            base.Closing += new CancelEventHandler(this.ThemeEditor_Closing);
            this.tabControlThemes.ResumeLayout(false);
            this.tabPageThemes.ResumeLayout(false);
            this.tabControlPreview.ResumeLayout(false);
            this.tabPagePreview.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        internal void InitializeThemePainter()
        {
            this.themePainter = this.chart.Clone() as Chart;
            this.themePainter.Graphics3D.UseBuffer = false;
            this.panelPreview.ThemeEditor = this;
            this.panelPreview.Draw();
        }

        private void listBoxThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxThemes.SelectedIndex > 0)
            {
                this.buttonOK.Enabled = true;
                this.panelPreview.Draw();
            }
            else
            {
                this.buttonOK.Enabled = false;
            }
        }

        private Theme ReturnThemeInstance(System.Type ThemeType)
        {
            return (Theme) Activator.CreateInstance(ThemeType, new object[] { this.chart });
        }

        private object SelectedTheme()
        {
            if (this.listBoxThemes.SelectedIndex <= 0)
            {
                return null;
            }
            return this.listBoxThemes.SelectedItem;
        }

        public static void ShowModal(Chart c)
        {
            ThemeEditor editor = new ThemeEditor(c, null);
            EditorUtils.Translate(editor);
            editor.ShowDialog();
        }

        private void ThemeEditor_Closing(object sender, CancelEventArgs e)
        {
            if (this.buttonOK.Enabled && (this.parent == null))
            {
                switch (MessageBox.Show(Texts.SureToApply, "Confirm"))
                {
                    case DialogResult.Yes:
                        this.buttonOK_Click(null, null);
                        e.Cancel = this.buttonOK.Enabled;
                        return;

                    case DialogResult.No:
                        this.buttonOK.Enabled = false;
                        e.Cancel = false;
                        return;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;

                    default:
                        return;
                }
            }
        }

        private void ThemeShowEditor()
        {
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Visible = false;
            this.buttonOK.Left = this.buttonCancel.Left;
            this.buttonOK.Text = "Apply";
        }

        internal sealed class CustomThemeObject : FileStream
        {
            private FileInfo file;

            public CustomThemeObject(FileInfo f) : base(f.FullName, FileMode.Open)
            {
                this.file = f;
            }

            public override string ToString()
            {
                return this.file.Name.Remove(this.file.Name.Length - 4, 4);
            }
        }

        [ToolboxItem(false)]
        private class TPanel : System.Windows.Forms.Panel
        {
            private Steema.TeeChart.Editors.ThemeEditor themeEditor;

            public TPanel()
            {
                base.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            }

            public void Draw()
            {
                this.Refresh();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                if (this.ThemeEditor != null)
                {
                    this.themeEditor.DrawChart(e);
                }
            }

            public Steema.TeeChart.Editors.ThemeEditor ThemeEditor
            {
                get
                {
                    return this.themeEditor;
                }
                set
                {
                    this.themeEditor = value;
                }
            }
        }
    }
}

