namespace WealthLab.DataProviders.AsciiFilesStatic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.DataProviders.Helper;

    public class AsciiCacheControl : DataBehaviorUserControl
    {
        private BorderLabel borderLabel1;
        private Button btnClear;
        private CheckBox cbEnableCache;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private GroupBox gpBars;
        private GroupBox gpSize;
        private IContainer icontainer_1;
        private Label label1;
        private ListView lstBars;
        public static readonly int MaxSize = 300;
        private NumericUpDown numSize;
        private ProgressBar pbSize;

        public AsciiCacheControl()
        {
            this.InitializeComponent_1();
            Class6.smethod_0(new Class6.Delegate0(this.method_3));
            Class6.smethod_2(new Class6.Delegate0(this.method_0));
            Class6.smethod_4(new EventHandler(this.method_1));
            this.method_2();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Class6.smethod_11();
        }

        private void cbEnableCache_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.cbEnableCache.Checked)
            {
                Class6.smethod_11();
            }
            Class6.smethod_6().EnableCache = this.cbEnableCache.Checked;
            Class6.smethod_6().Serialize();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_1 != null))
            {
                this.icontainer_1.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent_1()
        {
            this.cbEnableCache = new CheckBox();
            this.gpSize = new GroupBox();
            this.label1 = new Label();
            this.numSize = new NumericUpDown();
            this.pbSize = new ProgressBar();
            this.gpBars = new GroupBox();
            this.btnClear = new Button();
            this.lstBars = new ListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.borderLabel1 = new BorderLabel();
            this.gpSize.SuspendLayout();
            this.numSize.BeginInit();
            this.gpBars.SuspendLayout();
            base.SuspendLayout();
            this.cbEnableCache.AutoSize = true;
            this.cbEnableCache.Location = new Point(3, 0x40);
            this.cbEnableCache.Name = "cbEnableCache";
            this.cbEnableCache.Size = new Size(0x71, 0x11);
            this.cbEnableCache.TabIndex = 1;
            this.cbEnableCache.Text = "Cache ASCII Data";
            this.cbEnableCache.UseVisualStyleBackColor = true;
            this.cbEnableCache.CheckedChanged += new EventHandler(this.cbEnableCache_CheckedChanged);
            this.gpSize.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpSize.Controls.Add(this.label1);
            this.gpSize.Controls.Add(this.numSize);
            this.gpSize.Controls.Add(this.pbSize);
            this.gpSize.Location = new Point(3, 0x5e);
            this.gpSize.Name = "gpSize";
            this.gpSize.Size = new Size(500, 0x35);
            this.gpSize.TabIndex = 2;
            this.gpSize.TabStop = false;
            this.gpSize.Text = "Cache Size";
            this.gpSize.Visible = false;
            this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x1d8, 0x17);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x16, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Mb";
            this.numSize.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            int[] bits = new int[4];
            bits[0] = 50;
            this.numSize.Increment = new decimal(bits);
            this.numSize.Location = new Point(0x1a5, 0x13);
            int[] numArray2 = new int[4];
            numArray2[0] = 350;
            this.numSize.Maximum = new decimal(numArray2);
            int[] numArray3 = new int[4];
            numArray3[0] = 50;
            this.numSize.Minimum = new decimal(numArray3);
            this.numSize.Name = "numSize";
            this.numSize.Size = new Size(0x2d, 20);
            this.numSize.TabIndex = 1;
            int[] numArray4 = new int[4];
            numArray4[0] = 150;
            this.numSize.Value = new decimal(numArray4);
            this.pbSize.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.pbSize.Location = new Point(6, 0x13);
            this.pbSize.Name = "pbSize";
            this.pbSize.Size = new Size(0x199, 20);
            this.pbSize.TabIndex = 0;
            this.pbSize.Value = 50;
            this.gpBars.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpBars.Controls.Add(this.btnClear);
            this.gpBars.Controls.Add(this.lstBars);
            this.gpBars.Location = new Point(3, 0x54);
            this.gpBars.Name = "gpBars";
            this.gpBars.Size = new Size(500, 0xf7);
            this.gpBars.TabIndex = 3;
            this.gpBars.TabStop = false;
            this.gpBars.Text = "Bars Cache";
            this.btnClear.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnClear.Location = new Point(6, 0xd9);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(0x54, 0x18);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            this.lstBars.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lstBars.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_1, this.columnHeader_2, this.columnHeader_3 });
            this.lstBars.FullRowSelect = true;
            this.lstBars.Location = new Point(6, 0x13);
            this.lstBars.Name = "lstBars";
            this.lstBars.Size = new Size(0x1e8, 0xc0);
            this.lstBars.TabIndex = 0;
            this.lstBars.UseCompatibleStateImageBehavior = false;
            this.lstBars.View = View.Details;
            this.columnHeader_0.Text = "Symbol";
            this.columnHeader_0.Width = 120;
            this.columnHeader_1.Text = "Bars";
            this.columnHeader_1.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_2.Text = "Date";
            this.columnHeader_2.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_2.Width = 120;
            this.columnHeader_3.Text = "DataSet";
            this.columnHeader_3.Width = 180;
            this.borderLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.borderLabel1.AutoEllipsis = true;
            this.borderLabel1.BackColor = Color.FromArgb(0xff, 0xff, 0xe1);
            this.borderLabel1.BorderColor = Color.FromArgb(0xfc, 0xf2, 0xad);
            this.borderLabel1.BorderWidth = 1;
            this.borderLabel1.ImageAlign = ContentAlignment.MiddleLeft;
            this.borderLabel1.Location = new Point(3, 3);
            this.borderLabel1.Name = "borderLabel1";
            this.borderLabel1.Size = new Size(500, 0x35);
            this.borderLabel1.TabIndex = 0;
            this.borderLabel1.Text = "Caching ASCII data speeds up working with them by reducing file access time. \r\nEnable caching if you work with large ASCII files.";
            this.borderLabel1.TextAlign = ContentAlignment.MiddleCenter;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.gpBars);
            base.Controls.Add(this.gpSize);
            base.Controls.Add(this.cbEnableCache);
            base.Controls.Add(this.borderLabel1);
            base.Name = "AsciiCacheControl";
            base.Size = new Size(0x1fa, 0x14e);
            base.Tag = "ASCII Data";
            this.gpSize.ResumeLayout(false);
            this.gpSize.PerformLayout();
            this.numSize.EndInit();
            this.gpBars.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0(EventArgs0 eventArgs0_0)
        {
            for (int i = 0; i < this.lstBars.Items.Count; i++)
            {
                if (((string) this.lstBars.Items[i].Tag) == eventArgs0_0.method_0().SourceFileName)
                {
                    this.lstBars.Items.RemoveAt(i);
                    return;
                }
            }
        }

        private void method_1(object sender, EventArgs e)
        {
            this.lstBars.BeginUpdate();
            this.lstBars.Items.Clear();
            this.lstBars.EndUpdate();
        }

        private void method_2()
        {
            this.cbEnableCache.Checked = Class6.smethod_6().EnableCache;
            this.lstBars.BeginUpdate();
            foreach (AsciiCache cache in Class6.smethod_6().AsciiCacheList)
            {
                this.method_4(cache);
            }
            this.lstBars.EndUpdate();
        }

        private void method_3(EventArgs0 eventArgs0_0)
        {
            this.method_4(eventArgs0_0.method_0());
        }

        private void method_4(AsciiCache asciiCache_0)
        {
            ListViewItem item = new ListViewItem {
                Tag = asciiCache_0.SourceFileName,
                Text = asciiCache_0.Symbol
            };
            item.SubItems.AddRange(new string[] { asciiCache_0.BarsCount.ToString(), asciiCache_0.DateTime.ToString(), asciiCache_0.DataSet });
            this.lstBars.Items.Add(item);
        }
    }
}

