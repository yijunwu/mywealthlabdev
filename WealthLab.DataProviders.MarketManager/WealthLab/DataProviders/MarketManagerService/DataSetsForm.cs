namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class DataSetsForm : Form
    {
        private Button btnCancel;
        private Button btnOk;
        private DataSourceManager dataSourceManager_0;
        private IContainer icontainer_0;
        private ImageList imageList_0;
        private ListView lvSymbols;

        public DataSetsForm()
        {
            this.InitializeComponent();
        }

        private void DataSetsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                base.DialogResult = DialogResult.Cancel;
            }
            if (e.KeyCode == Keys.Enter)
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public void Initialize()
        {
            this.dataSourceManager_0 = new DataSourceManager();
            this.dataSourceManager_0.RootPath = Application.UserAppDataPath + @"\Data\";
            this.dataSourceManager_0.OnDemandUpdatesEnabled = false;
            this.method_0();
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            this.lvSymbols = new ListView();
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.btnCancel = new Button();
            this.btnOk = new Button();
            base.SuspendLayout();
            this.lvSymbols.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvSymbols.FullRowSelect = true;
            this.lvSymbols.HideSelection = false;
            this.lvSymbols.Location = new Point(12, 12);
            this.lvSymbols.MultiSelect = false;
            this.lvSymbols.Name = "lvSymbols";
            this.lvSymbols.ShowItemToolTips = true;
            this.lvSymbols.Size = new Size(0x144, 280);
            this.lvSymbols.SmallImageList = this.imageList_0;
            this.lvSymbols.TabIndex = 3;
            this.lvSymbols.UseCompatibleStateImageBehavior = false;
            this.lvSymbols.View = View.List;
            this.lvSymbols.DoubleClick += new EventHandler(this.lvSymbols_DoubleClick);
            this.imageList_0.ColorDepth = ColorDepth.Depth32Bit;
            this.imageList_0.ImageSize = new Size(0x10, 0x10);
            this.imageList_0.TransparentColor = Color.Magenta;
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x105, 0x12a);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Location = new Point(180, 0x12a);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x4b, 0x17);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x15c, 0x147);
            base.Controls.Add(this.btnOk);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.lvSymbols);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DataSetsForm";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "DataSets";
            base.KeyDown += new KeyEventHandler(this.DataSetsForm_KeyDown);
            base.ResumeLayout(false);
        }

        private void lvSymbols_DoubleClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void method_0()
        {
            foreach (DataSource source in this.dataSourceManager_0.DataSources)
            {
                ListViewItem item = new ListViewItem(source.Name);
                this.imageList_0.Images.Add(source.Provider.Glyph);
                item.ImageIndex = this.imageList_0.Images.Count - 1;
                item.Tag = source;
                this.lvSymbols.Items.Add(item);
            }
            if (this.lvSymbols.Items.Count > 0)
            {
                this.lvSymbols.Items[0].Selected = true;
            }
        }

        public DataSource SelectedDataSet
        {
            get
            {
                if (this.lvSymbols.SelectedItems.Count > 0)
                {
                    return (DataSource) this.lvSymbols.SelectedItems[0].Tag;
                }
                return null;
            }
        }

        public List<string> SelectedSymbols
        {
            get
            {
                if (this.SelectedDataSet != null)
                {
                    return this.SelectedDataSet.Symbols;
                }
                return null;
            }
        }
    }
}

