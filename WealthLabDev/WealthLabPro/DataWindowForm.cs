namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DataWindowForm : Form
    {
        private ChartForm chartForm_0;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private IContainer icontainer_0;
        public static DataWindowForm Instance;
        private int int_0 = 6;
        private ListView lvDataWindow;

        public DataWindowForm()
        {
            this.InitializeComponent();
        }

        private void DataWindowForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainModule.Instance.Settings.Set(this, "DataWindowForm");
            MainModule.Instance.Settings.Set("DataWindowForm.Column0.Width", this.lvDataWindow.Columns[0].Width);
            MainModule.Instance.Settings.Set("DataWindowForm.Column1.Width", this.lvDataWindow.Columns[1].Width);
            MainModule.Instance.Settings.SaveSettings();
            Instance = null;
            smethod_0();
        }

        private void DataWindowForm_Load(object sender, EventArgs e)
        {
            Instance = this;
            MainModule.Instance.Settings.Get(this, "DataWindowForm");
            this.lvDataWindow.Columns[0].Width = MainModule.Instance.Settings.Get("DataWindowForm.Column0.Width", this.lvDataWindow.Columns[0].Width);
            this.lvDataWindow.Columns[1].Width = MainModule.Instance.Settings.Get("DataWindowForm.Column1.Width", this.lvDataWindow.Columns[1].Width);
            smethod_0();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ListViewItem item = new ListViewItem(new string[] { "Date", "" }, -1);
            ListViewItem item2 = new ListViewItem(new string[] { "Open", "" }, -1);
            ListViewItem item3 = new ListViewItem(new string[] { "High", "" }, -1);
            ListViewItem item4 = new ListViewItem(new string[] { "Low", "" }, -1);
            ListViewItem item5 = new ListViewItem(new string[] { "Close", "" }, -1);
            ListViewItem item6 = new ListViewItem(new string[] { "Volume", "" }, -1);
            this.lvDataWindow = new ListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            base.SuspendLayout();
            this.lvDataWindow.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_1 });
            this.lvDataWindow.Dock = DockStyle.Fill;
            this.lvDataWindow.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lvDataWindow.Items.AddRange(new ListViewItem[] { item, item2, item3, item4, item5, item6 });
            this.lvDataWindow.Location = new Point(0, 0);
            this.lvDataWindow.Name = "lvDataWindow";
            this.lvDataWindow.Size = new Size(0x11c, 0x106);
            this.lvDataWindow.TabIndex = 0;
            this.lvDataWindow.UseCompatibleStateImageBehavior = false;
            this.lvDataWindow.View = View.Details;
            this.columnHeader_0.Text = "Item";
            this.columnHeader_0.Width = 0x62;
            this.columnHeader_1.Text = "Value";
            this.columnHeader_1.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_1.Width = 0x71;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x11c, 0x106);
            base.Controls.Add(this.lvDataWindow);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DataWindowForm";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            this.Text = "Data Window";
            base.TopMost = true;
            base.Load += new EventHandler(this.DataWindowForm_Load);
            base.FormClosed += new FormClosedEventHandler(this.DataWindowForm_FormClosed);
            base.ResumeLayout(false);
        }

        internal void method_0(ChartForm chartForm_1)
        {
            this.lvDataWindow.BeginUpdate();
            this.method_4();
            if (this.chartForm_0 != chartForm_1)
            {
                this.chartForm_0 = chartForm_1;
            }
        }

        internal void method_1()
        {
            this.lvDataWindow.EndUpdate();
        }

        internal void method_2(DataWindowType dataWindowType_0, string string_0)
        {
            this.lvDataWindow.Items[(int) dataWindowType_0].SubItems[1].Text = string_0;
        }

        internal void method_3(string string_0, string string_1)
        {
            for (int i = this.int_0; i < this.lvDataWindow.Items.Count; i++)
            {
                if (this.lvDataWindow.Items[i].SubItems[0].Text == string_0)
                {
                    this.lvDataWindow.Items[i].SubItems[1].Text = string_1;
                    return;
                }
            }
            this.lvDataWindow.Items.Add(string_0).SubItems.Add(string_1);
        }

        internal void method_4()
        {
            while (this.lvDataWindow.Items.Count > this.int_0)
            {
                this.lvDataWindow.Items.RemoveAt(this.int_0);
            }
        }

        private static void smethod_0()
        {
            MainModule.Instance.FirstMainForm.method_47();
        }
    }
}

