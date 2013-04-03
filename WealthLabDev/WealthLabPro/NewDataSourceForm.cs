namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class NewDataSourceForm : Form
    {
        private Button btnCancel;
        private Button btnFinished;
        private Button btnNext;
        private Button btnPrevious;
        private GroupBox grpDataSource;
        private GroupBox grpDetails;
        private GroupBox grpName;
        private GroupBox grpProviders;
        private IContainer icontainer_0;
        private ImageList imageList_0;
        private Label lblDataSource;
        private Label lblName;
        private Label lblProvider;
        private LinkLabel linkProvider;
        private ListView lvDataSources;
        private Panel panel_0;
        private PictureBox picProvider;
        private Panel pnlButtons;
        private Panel pnlContent;
        private Panel pnlFinished;
        private Panel pnlIntro;
        private StaticDataProvider staticDataProvider_0;
        private TextBox txtDataSourceName;
        private UserControl userControl_0;

        public NewDataSourceForm()
        {
            this.InitializeComponent();
            this.panel_0 = this.pnlIntro;
        }

        public void AddProvider(StaticDataProvider staticDataProvider_1)
        {
            this.imageList_0.Images.Add(staticDataProvider_1.Glyph);
            this.lvDataSources.Items.Add(staticDataProvider_1.FriendlyName, (int) (this.imageList_0.Images.Count - 1)).Tag = staticDataProvider_1;
        }

        private void btnFinished_Click(object sender, EventArgs e)
        {
            if (this.txtDataSourceName.Text == "")
            {
                MessageBox.Show("Please provide a name for the new DataSet");
                this.txtDataSourceName.Focus();
            }
            else if (MainModule.Instance.DataSources.FindDataSource(this.txtDataSourceName.Text) != null)
            {
                MessageBox.Show("There is already a DataSet defined with this name");
                this.txtDataSourceName.SelectAll();
                this.txtDataSourceName.Focus();
            }
            else
            {
                DataSource source = this.staticDataProvider_0.CreateDataSource();
                source.Name = this.txtDataSourceName.Text;
                MainModule.Instance.DataSources.Add(source);
                MessageBox.Show("DataSet \"" + source.Name + "\" was successfully created");
                base.DialogResult = DialogResult.OK;
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (this.panel_0 == this.pnlFinished)
            {
                this.panel_0 = this.pnlContent;
                this.pnlContent.BringToFront();
                this.btnFinished.Enabled = false;
                this.btnNext.Enabled = true;
            }
            else if (this.panel_0 == this.pnlContent)
            {
                UserControl control = this.staticDataProvider_0.WizardPreviousPage(this.userControl_0);
                if (control == null)
                {
                    this.panel_0 = this.pnlIntro;
                    this.pnlIntro.BringToFront();
                    this.btnPrevious.Enabled = false;
                }
                else
                {
                    this.pnlContent.Controls.Remove(this.userControl_0);
                    this.userControl_0 = control;
                    this.pnlContent.Controls.Add(this.userControl_0);
                    this.method_0(this.userControl_0);
                }
            }
        }

        public void Clear()
        {
            this.lvDataSources.Clear();
            this.imageList_0.Images.Clear();
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
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(NewDataSourceForm));
            this.pnlButtons = new Panel();
            this.btnCancel = new Button();
            this.btnPrevious = new Button();
            this.btnNext = new Button();
            this.btnFinished = new Button();
            this.pnlIntro = new Panel();
            this.grpDetails = new GroupBox();
            this.picProvider = new PictureBox();
            this.lblProvider = new Label();
            this.linkProvider = new LinkLabel();
            this.grpProviders = new GroupBox();
            this.lvDataSources = new ListView();
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.grpDataSource = new GroupBox();
            this.lblDataSource = new Label();
            this.pnlFinished = new Panel();
            this.grpName = new GroupBox();
            this.txtDataSourceName = new TextBox();
            this.lblName = new Label();
            this.pnlContent = new Panel();
            this.pnlButtons.SuspendLayout();
            this.pnlIntro.SuspendLayout();
            this.grpDetails.SuspendLayout();
            ((ISupportInitialize) this.picProvider).BeginInit();
            this.grpProviders.SuspendLayout();
            this.grpDataSource.SuspendLayout();
            this.pnlFinished.SuspendLayout();
            this.grpName.SuspendLayout();
            base.SuspendLayout();
            this.pnlButtons.BorderStyle = BorderStyle.FixedSingle;
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnPrevious);
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.btnFinished);
            this.pnlButtons.Dock = DockStyle.Bottom;
            this.pnlButtons.Location = new Point(0, 370);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new Size(0x23d, 0x20);
            this.pnlButtons.TabIndex = 0;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xf2, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnPrevious.Enabled = false;
            this.btnPrevious.Location = new Point(0x143, 3);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new Size(0x4b, 0x17);
            this.btnPrevious.TabIndex = 2;
            this.btnPrevious.Text = "<- Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new EventHandler(this.btnPrevious_Click);
            this.btnNext.Enabled = false;
            this.btnNext.Location = new Point(0x194, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new Size(0x4b, 0x17);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next ->";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new EventHandler(this.lvDataSources_DoubleClick);
            this.btnFinished.Enabled = false;
            this.btnFinished.Location = new Point(0x1e5, 3);
            this.btnFinished.Name = "btnFinished";
            this.btnFinished.Size = new Size(0x4b, 0x17);
            this.btnFinished.TabIndex = 0;
            this.btnFinished.Text = "Finished";
            this.btnFinished.UseVisualStyleBackColor = true;
            this.btnFinished.Click += new EventHandler(this.btnFinished_Click);
            this.pnlIntro.Controls.Add(this.grpDetails);
            this.pnlIntro.Controls.Add(this.grpProviders);
            this.pnlIntro.Controls.Add(this.grpDataSource);
            this.pnlIntro.Location = new Point(0, 0);
            this.pnlIntro.Name = "pnlIntro";
            this.pnlIntro.Size = new Size(0x23d, 0x170);
            this.pnlIntro.TabIndex = 1;
            this.grpDetails.Controls.Add(this.picProvider);
            this.grpDetails.Controls.Add(this.lblProvider);
            this.grpDetails.Controls.Add(this.linkProvider);
            this.grpDetails.Location = new Point(12, 0x113);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new Size(0x225, 0x53);
            this.grpDetails.TabIndex = 2;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Data Provider Details";
            this.picProvider.Location = new Point(13, 20);
            this.picProvider.Name = "picProvider";
            this.picProvider.Size = new Size(0x10, 0x10);
            this.picProvider.TabIndex = 4;
            this.picProvider.TabStop = false;
            this.lblProvider.Location = new Point(0x3b, 20);
            this.lblProvider.Name = "lblProvider";
            this.lblProvider.Size = new Size(0x1e4, 0x39);
            this.lblProvider.TabIndex = 3;
            this.linkProvider.AutoSize = true;
            this.linkProvider.Location = new Point(10, 0x40);
            this.linkProvider.Name = "linkProvider";
            this.linkProvider.Size = new Size(0x2b, 13);
            this.linkProvider.TabIndex = 2;
            this.linkProvider.TabStop = true;
            this.linkProvider.Text = "More ...";
            this.linkProvider.Visible = false;
            this.linkProvider.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkProvider_LinkClicked);
            this.grpProviders.Controls.Add(this.lvDataSources);
            this.grpProviders.Location = new Point(12, 0x4a);
            this.grpProviders.Name = "grpProviders";
            this.grpProviders.Size = new Size(0x225, 0xc3);
            this.grpProviders.TabIndex = 1;
            this.grpProviders.TabStop = false;
            this.grpProviders.Text = "Select a Data Provider";
            this.lvDataSources.HideSelection = false;
            this.lvDataSources.Location = new Point(10, 0x13);
            this.lvDataSources.MultiSelect = false;
            this.lvDataSources.Name = "lvDataSources";
            this.lvDataSources.Size = new Size(0x215, 0xa9);
            this.lvDataSources.SmallImageList = this.imageList_0;
            this.lvDataSources.TabIndex = 0;
            this.lvDataSources.UseCompatibleStateImageBehavior = false;
            this.lvDataSources.View = View.List;
            this.lvDataSources.DoubleClick += new EventHandler(this.lvDataSources_DoubleClick);
            this.lvDataSources.SelectedIndexChanged += new EventHandler(this.lvDataSources_SelectedIndexChanged);
            this.imageList_0.ColorDepth = ColorDepth.Depth8Bit;
            this.imageList_0.ImageSize = new Size(0x10, 0x10);
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.grpDataSource.Controls.Add(this.lblDataSource);
            this.grpDataSource.Location = new Point(12, 12);
            this.grpDataSource.Name = "grpDataSource";
            this.grpDataSource.Size = new Size(0x225, 0x38);
            this.grpDataSource.TabIndex = 0;
            this.grpDataSource.TabStop = false;
            this.grpDataSource.Text = "What is a DataSet?";
            this.lblDataSource.Location = new Point(10, 20);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new Size(0x215, 0x21);
            this.lblDataSource.TabIndex = 0;
            this.lblDataSource.Text = manager.GetString("lblDataSource.Text");
            this.pnlFinished.Controls.Add(this.grpName);
            this.pnlFinished.Location = new Point(0, 0);
            this.pnlFinished.Name = "pnlFinished";
            this.pnlFinished.Size = new Size(0x23d, 0x170);
            this.pnlFinished.TabIndex = 2;
            this.grpName.Controls.Add(this.txtDataSourceName);
            this.grpName.Controls.Add(this.lblName);
            this.grpName.Location = new Point(13, 13);
            this.grpName.Name = "grpName";
            this.grpName.Size = new Size(0x224, 80);
            this.grpName.TabIndex = 0;
            this.grpName.TabStop = false;
            this.grpName.Text = "DataSet Name";
            this.txtDataSourceName.Location = new Point(10, 0x30);
            this.txtDataSourceName.Name = "txtDataSourceName";
            this.txtDataSourceName.Size = new Size(0x214, 20);
            this.txtDataSourceName.TabIndex = 1;
            this.lblName.AutoSize = true;
            this.lblName.Location = new Point(7, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new Size(0x100, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Please enter a name for your newly created DataSet:";
            this.pnlContent.AutoScroll = true;
            this.pnlContent.Location = new Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new Size(570, 0x16c);
            this.pnlContent.TabIndex = 3;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x23d, 0x192);
            base.Controls.Add(this.pnlButtons);
            base.Controls.Add(this.pnlIntro);
            base.Controls.Add(this.pnlContent);
            base.Controls.Add(this.pnlFinished);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "NewDataSourceForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Create a new DataSet";
            base.TopMost = true;
            base.Load += new EventHandler(this.NewDataSourceForm_Load);
            this.pnlButtons.ResumeLayout(false);
            this.pnlIntro.ResumeLayout(false);
            this.grpDetails.ResumeLayout(false);
            this.grpDetails.PerformLayout();
            ((ISupportInitialize) this.picProvider).EndInit();
            this.grpProviders.ResumeLayout(false);
            this.grpDataSource.ResumeLayout(false);
            this.pnlFinished.ResumeLayout(false);
            this.grpName.ResumeLayout(false);
            this.grpName.PerformLayout();
            base.ResumeLayout(false);
        }

        private void linkProvider_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MainModule.Instance.NavigateToThirdPartySite(this.staticDataProvider_0.URL))
            {
                Process.Start(this.staticDataProvider_0.URL);
            }
        }

        private void lvDataSources_DoubleClick(object sender, EventArgs e)
        {
            if (this.panel_0 == this.pnlIntro)
            {
                this.panel_0 = this.pnlContent;
                this.userControl_0 = this.staticDataProvider_0.WizardFirstPage();
                this.pnlContent.Controls.Clear();
                this.pnlContent.Controls.Add(this.userControl_0);
                this.method_0(this.userControl_0);
                this.pnlContent.BringToFront();
                this.btnPrevious.Enabled = true;
            }
            else if (this.panel_0 == this.pnlContent)
            {
                try
                {
                    UserControl control = this.staticDataProvider_0.WizardNextPage(this.userControl_0);
                    if (control == null)
                    {
                        this.panel_0 = this.pnlFinished;
                        this.pnlFinished.BringToFront();
                        this.btnFinished.Enabled = true;
                        this.btnNext.Enabled = false;
                        this.txtDataSourceName.Text = this.staticDataProvider_0.SuggestedDataSourceName;
                        this.txtDataSourceName.Enabled = !this.staticDataProvider_0.DataSourceNameReadOnly;
                        this.txtDataSourceName.SelectAll();
                        this.txtDataSourceName.Focus();
                    }
                    else
                    {
                        this.pnlContent.Controls.Remove(this.userControl_0);
                        this.userControl_0 = control;
                        this.pnlContent.Controls.Add(this.userControl_0);
                        this.method_0(this.userControl_0);
                    }
                }
                catch (WizardValidationException exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void lvDataSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lvDataSources.SelectedItems.Count == 0)
            {
                this.btnNext.Enabled = false;
            }
            else
            {
                ListViewItem item = this.lvDataSources.SelectedItems[0];
                this.staticDataProvider_0 = (StaticDataProvider) item.Tag;
                this.btnNext.Enabled = true;
                this.linkProvider.Visible = this.staticDataProvider_0.URL != "";
                this.lblProvider.Text = this.staticDataProvider_0.Description;
                Bitmap glyph = this.staticDataProvider_0.Glyph;
                glyph.MakeTransparent(Color.Fuchsia);
                this.picProvider.Image = glyph;
            }
        }

        private void method_0(UserControl userControl_1)
        {
            int x = (this.pnlContent.Width - userControl_1.Width) / 2;
            if (x < 0)
            {
                x = 0;
            }
            int y = (this.pnlContent.Height - userControl_1.Height) / 2;
            if (y < 0)
            {
                y = 0;
            }
            userControl_1.Location = new Point(x, y);
        }

        private void NewDataSourceForm_Load(object sender, EventArgs e)
        {
            if ((this.lvDataSources.Items.Count > 0) && (this.lvDataSources.SelectedItems.Count == 0))
            {
                this.lvDataSources.Items[0].Selected = true;
            }
        }

        public StaticDataProvider ProviderSelected
        {
            get
            {
                return this.staticDataProvider_0;
            }
        }
    }
}

