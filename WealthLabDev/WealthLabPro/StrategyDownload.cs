namespace WealthLabPro
{
    using Fidelity.Components;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web.Services.Protocols;
    using System.Windows.Forms;
    using WealthLab;

    public class StrategyDownload : Form, IStrategyHost
    {
        private AssemblyLoader assemblyLoader_0;
        private bool bool_0;
        private bool bool_1;
        [CompilerGenerated]
        private bool bool_2;
        private Button btnClose;
        private Button btnDownload;
        private CheckBox cbGetPrivate;
        private CheckBox cbGetPublic;
        private CheckBox cbPublishedSince;
        private ComboBox cmbStrategyProviders;
        private DateTimePicker dtPickerSince;
        private GroupBox grpResults;
        private GroupBox grpWarning;
        private IContainer icontainer_0;
        private IList<string> ilist_0 = MainModule.Instance.Strategies.FolderNames;
        private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ImageList imageList_0;
        private int int_0;
        private int int_1;
        private ISettingsHost isettingsHost_0 = MainModule.Instance.Settings;
        private Label lblDownloadFrom;
        private Label lblWarning;
        private Panel pnDownload;
        private Panel pnlButton;
        private StatusStrip status;
        private ToolStripStatusLabel statuslbl;
        private StrategyProvider strategyProvider_0;
        private string string_0 = (MainModule.Instance.Strategies.RootPath + @"\Strategies\");
        private string string_1 = "";
        private Thread thread_0;
        private TreeView treeDownloadedStrategies;

        public StrategyDownload()
        {
            this.InitializeComponent();
            this.CancelDownload = false;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            this.strategyProvider_0 = (StrategyProvider) this.cmbStrategyProviders.SelectedItem;
            if (!this.bool_1)
            {
                this.grpResults.Visible = true;
                this.treeDownloadedStrategies.Enabled = true;
                this.btnDownload.Text = "C&ancel";
                this.statuslbl.Text = "Starting Download";
                this.int_0 = 0;
                this.int_1 = 0;
                this.bool_1 = true;
                this.method_3(false);
                this.thread_0 = new Thread(new ThreadStart(this.method_4));
                this.thread_0.IsBackground = true;
                this.thread_0.Start();
            }
            else
            {
                this.statuslbl.Text = "Canceling download";
                this.strategyProvider_0.CancelDownload();
                this.CancelDownload = true;
            }
        }

        private void cbGetPrivate_CheckedChanged(object sender, EventArgs e)
        {
            this.method_8();
        }

        private void cbGetPublic_CheckedChanged(object sender, EventArgs e)
        {
            this.method_8();
        }

        private void cbPublishedSince_CheckedChanged(object sender, EventArgs e)
        {
            this.dtPickerSince.Enabled = this.cbPublishedSince.Checked;
        }

        private void cmbStrategyProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.method_6();
        }

        public bool ContainsKey(string string_2)
        {
            return this.isettingsHost_0.ContainsKey(this.method_1(string_2));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Get(string string_2, bool defaultValue)
        {
            return this.isettingsHost_0.Get(this.method_1(string_2), defaultValue);
        }

        public DateTime Get(string string_2, DateTime defaultValue)
        {
            return this.isettingsHost_0.Get(this.method_1(string_2), defaultValue);
        }

        public double Get(string string_2, double defaultValue)
        {
            return this.isettingsHost_0.Get(this.method_1(string_2), defaultValue);
        }

        public Color Get(string string_2, Color defaultValue)
        {
            return this.isettingsHost_0.Get(this.method_1(string_2), defaultValue);
        }

        public Font Get(string string_2, Font defaultFont)
        {
            return this.isettingsHost_0.Get(this.method_1(string_2), defaultFont);
        }

        public int Get(string string_2, int defaultValue)
        {
            return this.isettingsHost_0.Get(this.method_1(string_2), defaultValue);
        }

        public string Get(string string_2, string defaultValue)
        {
            return this.isettingsHost_0.Get(this.method_1(string_2), defaultValue);
        }

        public bool Get(Form form_0, string string_2)
        {
            return this.isettingsHost_0.Get(form_0, this.method_1(string_2));
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(StrategyDownload));
            this.btnDownload = new Button();
            this.btnClose = new Button();
            this.cbGetPrivate = new CheckBox();
            this.grpWarning = new GroupBox();
            this.lblWarning = new Label();
            this.treeDownloadedStrategies = new TreeView();
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.dtPickerSince = new DateTimePicker();
            this.cbGetPublic = new CheckBox();
            this.cbPublishedSince = new CheckBox();
            this.cmbStrategyProviders = new ComboBox();
            this.assemblyLoader_0 = new AssemblyLoader(this.icontainer_0);
            this.lblDownloadFrom = new Label();
            this.pnlButton = new Panel();
            this.pnDownload = new Panel();
            this.grpResults = new GroupBox();
            this.status = new StatusStrip();
            this.statuslbl = new ToolStripStatusLabel();
            this.grpWarning.SuspendLayout();
            this.pnlButton.SuspendLayout();
            this.pnDownload.SuspendLayout();
            this.grpResults.SuspendLayout();
            this.status.SuspendLayout();
            base.SuspendLayout();
            this.btnDownload.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnDownload.Enabled = false;
            this.btnDownload.Location = new Point(0xf2, 4);
            this.btnDownload.Margin = new Padding(4);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new Size(0x71, 0x1b);
            this.btnDownload.TabIndex = 7;
            this.btnDownload.Text = "Begin &Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new EventHandler(this.btnDownload_Click);
            this.btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnClose.DialogResult = DialogResult.Cancel;
            this.btnClose.Location = new Point(0x16b, 4);
            this.btnClose.Margin = new Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x5b, 0x1b);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "C&lose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.cbGetPrivate.AutoSize = true;
            this.cbGetPrivate.Location = new Point(0xf7, 0x2f);
            this.cbGetPrivate.Name = "cbGetPrivate";
            this.cbGetPrivate.Size = new Size(0xc7, 0x13);
            this.cbGetPrivate.TabIndex = 1;
            this.cbGetPrivate.Text = "Download My Private Strategies";
            this.cbGetPrivate.UseVisualStyleBackColor = true;
            this.cbGetPrivate.CheckedChanged += new EventHandler(this.cbGetPrivate_CheckedChanged);
            this.grpWarning.Controls.Add(this.lblWarning);
            this.grpWarning.Dock = DockStyle.Bottom;
            this.grpWarning.Enabled = false;
            this.grpWarning.Location = new Point(0, 0x65);
            this.grpWarning.Name = "grpWarning";
            this.grpWarning.Size = new Size(0x1d3, 0x48);
            this.grpWarning.TabIndex = 12;
            this.grpWarning.TabStop = false;
            this.grpWarning.Text = "Please note:";
            this.lblWarning.Dock = DockStyle.Fill;
            this.lblWarning.Enabled = false;
            this.lblWarning.Location = new Point(3, 0x11);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new Size(0x1cd, 0x34);
            this.lblWarning.TabIndex = 0;
            this.lblWarning.Text = manager.GetString("lblWarning.Text");
            this.treeDownloadedStrategies.AllowDrop = true;
            this.treeDownloadedStrategies.Dock = DockStyle.Fill;
            this.treeDownloadedStrategies.ImageIndex = 0;
            this.treeDownloadedStrategies.ImageList = this.imageList_0;
            this.treeDownloadedStrategies.Location = new Point(3, 0x11);
            this.treeDownloadedStrategies.Name = "treeDownloadedStrategies";
            this.treeDownloadedStrategies.SelectedImageIndex = 0;
            this.treeDownloadedStrategies.Size = new Size(0x1cd, 0xa7);
            this.treeDownloadedStrategies.TabIndex = 9;
            this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("images.ImageStream");
            this.imageList_0.TransparentColor = Color.Transparent;
            this.imageList_0.Images.SetKeyName(0, "WLD5.ico");
            this.imageList_0.Images.SetKeyName(1, "Check.bmp");
            this.imageList_0.Images.SetKeyName(2, "Canceled.bmp");
            this.imageList_0.Images.SetKeyName(3, "Blank.bmp");
            this.dtPickerSince.Format = DateTimePickerFormat.Short;
            this.dtPickerSince.Location = new Point(0x86, 0x48);
            this.dtPickerSince.Name = "dtPickerSince";
            this.dtPickerSince.Size = new Size(0x65, 0x15);
            this.dtPickerSince.TabIndex = 3;
            this.cbGetPublic.AutoSize = true;
            this.cbGetPublic.Checked = true;
            this.cbGetPublic.CheckState = CheckState.Checked;
            this.cbGetPublic.Location = new Point(12, 0x2f);
            this.cbGetPublic.Name = "cbGetPublic";
            this.cbGetPublic.Size = new Size(0xb1, 0x13);
            this.cbGetPublic.TabIndex = 0;
            this.cbGetPublic.Text = "Download Public Strategies";
            this.cbGetPublic.UseVisualStyleBackColor = true;
            this.cbGetPublic.CheckedChanged += new EventHandler(this.cbGetPublic_CheckedChanged);
            this.cbPublishedSince.AutoSize = true;
            this.cbPublishedSince.Location = new Point(12, 0x48);
            this.cbPublishedSince.Name = "cbPublishedSince";
            this.cbPublishedSince.Size = new Size(0x74, 0x13);
            this.cbPublishedSince.TabIndex = 2;
            this.cbPublishedSince.Text = "Published since:";
            this.cbPublishedSince.UseVisualStyleBackColor = true;
            this.cbPublishedSince.CheckedChanged += new EventHandler(this.cbPublishedSince_CheckedChanged);
            this.cmbStrategyProviders.FormattingEnabled = true;
            this.cmbStrategyProviders.Location = new Point(12, 0x12);
            this.cmbStrategyProviders.Name = "cmbStrategyProviders";
            this.cmbStrategyProviders.Size = new Size(0x1ba, 0x17);
            this.cmbStrategyProviders.TabIndex = 0x12;
            this.cmbStrategyProviders.SelectedIndexChanged += new EventHandler(this.cmbStrategyProviders_SelectedIndexChanged);
            this.assemblyLoader_0.BaseClass = "StrategyProvider";
            this.assemblyLoader_0.DLLNameFilter = "";
            this.assemblyLoader_0.Interface = "";
            this.assemblyLoader_0.Path = null;
            this.assemblyLoader_0.PathMask = "*.dll";
            this.lblDownloadFrom.AutoSize = true;
            this.lblDownloadFrom.Location = new Point(3, 0);
            this.lblDownloadFrom.Name = "lblDownloadFrom";
            this.lblDownloadFrom.Size = new Size(150, 15);
            this.lblDownloadFrom.TabIndex = 20;
            this.lblDownloadFrom.Text = "Download strategies from:";
            this.pnlButton.Controls.Add(this.btnClose);
            this.pnlButton.Controls.Add(this.btnDownload);
            this.pnlButton.Dock = DockStyle.Bottom;
            this.pnlButton.Location = new Point(0, 360);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.Size = new Size(0x1d3, 0x23);
            this.pnlButton.TabIndex = 0x15;
            this.pnDownload.Controls.Add(this.grpWarning);
            this.pnDownload.Controls.Add(this.lblDownloadFrom);
            this.pnDownload.Controls.Add(this.dtPickerSince);
            this.pnDownload.Controls.Add(this.cmbStrategyProviders);
            this.pnDownload.Controls.Add(this.cbPublishedSince);
            this.pnDownload.Controls.Add(this.cbGetPublic);
            this.pnDownload.Controls.Add(this.cbGetPrivate);
            this.pnDownload.Dock = DockStyle.Top;
            this.pnDownload.Location = new Point(0, 0);
            this.pnDownload.Name = "pnDownload";
            this.pnDownload.Size = new Size(0x1d3, 0xad);
            this.pnDownload.TabIndex = 0x16;
            this.grpResults.Controls.Add(this.treeDownloadedStrategies);
            this.grpResults.Dock = DockStyle.Fill;
            this.grpResults.Location = new Point(0, 0xad);
            this.grpResults.Name = "grpResults";
            this.grpResults.Size = new Size(0x1d3, 0xbb);
            this.grpResults.TabIndex = 0x17;
            this.grpResults.TabStop = false;
            this.grpResults.Text = "The following strategies were downloaded:";
            this.status.Items.AddRange(new ToolStripItem[] { this.statuslbl });
            this.status.Location = new Point(0, 0x18b);
            this.status.Name = "status";
            this.status.Size = new Size(0x1d3, 0x16);
            this.status.SizingGrip = false;
            this.status.TabIndex = 0x18;
            this.statuslbl.Name = "statuslbl";
            this.statuslbl.Size = new Size(0, 0x11);
            base.AcceptButton = this.btnClose;
            base.AutoScaleDimensions = new SizeF(7f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            base.CancelButton = this.btnClose;
            base.ClientSize = new Size(0x1d3, 0x1a1);
            base.Controls.Add(this.grpResults);
            base.Controls.Add(this.pnDownload);
            base.Controls.Add(this.pnlButton);
            base.Controls.Add(this.status);
            this.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Margin = new Padding(4);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "StrategyDownload";
            base.ShowIcon = false;
            base.SizeGripStyle = SizeGripStyle.Show;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Download Strategies";
            base.Load += new EventHandler(this.StrategyDownload_Load);
            base.Shown += new EventHandler(this.StrategyDownload_Shown);
            base.FormClosing += new FormClosingEventHandler(this.StrategyDownload_FormClosing);
            this.grpWarning.ResumeLayout(false);
            this.pnlButton.ResumeLayout(false);
            this.pnDownload.ResumeLayout(false);
            this.pnDownload.PerformLayout();
            this.grpResults.ResumeLayout(false);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0(Strategy strategy_0, string string_2)
        {
            bool flag = false;
            this.int_1++;
            strategy_0.Origin = this.strategyProvider_0.FriendlyName;
            string reason = "";
            if (MainModule.Instance.Strategies.AddStrategy(strategy_0, string_2, false, ref reason))
            {
                flag = true;
                this.bool_0 = true;
                this.int_0++;
            }
            int num = 0;
            if (this.treeDownloadedStrategies.Nodes.ContainsKey(string_2))
            {
                num = this.treeDownloadedStrategies.Nodes.IndexOfKey(string_2);
            }
            else
            {
                this.treeDownloadedStrategies.Nodes.Add(string_2, string_2);
                num = this.treeDownloadedStrategies.Nodes.Count - 1;
            }
            TreeNode node = this.treeDownloadedStrategies.Nodes[num];
            string name = strategy_0.Name;
            if (!flag)
            {
                name = name + " [" + reason + "]";
            }
            node.Nodes.Add(name);
            node.Nodes[node.Nodes.Count - 1].ImageIndex = flag ? 1 : 2;
            node.Nodes[node.Nodes.Count - 1].SelectedImageIndex = flag ? 1 : 2;
            this.method_2();
            this.treeDownloadedStrategies.Visible = true;
        }

        private string method_1(string string_2)
        {
            return (this.strategyProvider_0.FriendlyName + "." + string_2);
        }

        private void method_2()
        {
            this.statuslbl.Text = this.int_0.ToString() + " of " + this.int_1.ToString() + " downloaded strategies added";
        }

        private void method_3(bool bool_3)
        {
            this.cmbStrategyProviders.Enabled = (this.cmbStrategyProviders.Items.Count > 1) && bool_3;
            this.cbGetPrivate.Enabled = bool_3;
            this.cbGetPublic.Enabled = bool_3;
            this.cbPublishedSince.Enabled = bool_3;
            this.btnClose.Enabled = bool_3;
        }

        private void method_4()
        {
            StrategyPubType @private;
            if (this.cbGetPrivate.Checked)
            {
                if (this.cbGetPublic.Checked)
                {
                    @private = StrategyPubType.Public | StrategyPubType.Private;
                }
                else
                {
                    @private = StrategyPubType.Private;
                }
            }
            else
            {
                @private = StrategyPubType.Public;
            }
            DateTime minValue = DateTime.MinValue;
            if (this.cbPublishedSince.Checked)
            {
                minValue = this.dtPickerSince.Value;
            }
            Exception exception = null;
            try
            {
                this.strategyProvider_0.DownloadStrategies(@private, minValue);
            }
            catch (Exception exception2)
            {
                exception = exception2;
            }
            base.Invoke(new Delegate23(this.method_5), new object[] { exception });
        }

        private void method_5(Exception exception_0)
        {
            if (exception_0 != null)
            {
                if ((exception_0 is SoapException) && exception_0.Message.Contains("WrongUsernameOrPasswordException"))
                {
                    string str = "Unable to connect using the supplied user name and password. Please register at\n" + this.strategyProvider_0.StrategyProviderURL;
                    if (this.strategyProvider_0.StrategyProviderURL.Contains("wealth-lab.com"))
                    {
                        str = str + " (see link under the Help menu)";
                    }
                    if ((MessageBox.Show(str + " to download strategies", "Authentication Failed", MessageBoxButtons.OKCancel) == DialogResult.OK) && MainModule.Instance.NavigateToThirdPartySite(this.strategyProvider_0.StrategyProviderURL))
                    {
                        Process.Start(this.strategyProvider_0.StrategyProviderURL);
                    }
                }
                else
                {
                    MessageBox.Show(exception_0.Message, Application.ProductName + " Strategy Download");
                }
            }
            else if ((this.int_1 == 0) && !this.CancelDownload)
            {
                MessageBox.Show("No strategies found", Application.ProductName + " Strategy Download");
            }
            this.method_2();
            this.bool_1 = false;
            this.method_3(true);
            this.treeDownloadedStrategies.ExpandAll();
            this.btnDownload.Text = "Begin &Download";
        }

        private void method_6()
        {
            this.strategyProvider_0 = (StrategyProvider) this.cmbStrategyProviders.SelectedItem;
            this.strategyProvider_0.StrategyHost = this;
            this.strategyProvider_0.SettingsHost = this;
            this.dtPickerSince.Value = this.strategyProvider_0.LastDownload;
            if ((this.strategyProvider_0.StrategyTypesProvided & StrategyPubType.Public) > 0)
            {
                this.cbGetPublic.Enabled = true;
            }
            else
            {
                this.cbGetPublic.Enabled = false;
            }
            if ((this.strategyProvider_0.StrategyTypesProvided & StrategyPubType.Private) > 0)
            {
                this.cbGetPrivate.Enabled = true;
            }
            else
            {
                this.cbGetPrivate.Enabled = false;
            }
        }

        private void method_7()
        {
            MessageBox.Show("Cannot download strategies because no StrategyProviders were found.", "No StrategyProviders Found");
        }

        private void method_8()
        {
            this.btnDownload.Enabled = this.cbGetPublic.Checked || this.cbGetPrivate.Checked;
            this.grpWarning.Enabled = this.btnDownload.Enabled;
            this.lblWarning.Enabled = this.btnDownload.Enabled;
        }

        public void SaveStrategy(Strategy strategy_0, string folderName)
        {
            base.Invoke(new Delegate22(this.method_0), new object[] { strategy_0, folderName });
        }

        public void Set(string string_2, bool value)
        {
            this.isettingsHost_0.Set(this.method_1(string_2), value);
        }

        public void Set(string string_2, DateTime value)
        {
            this.isettingsHost_0.Set(this.method_1(string_2), value);
        }

        public void Set(string string_2, double value)
        {
            this.isettingsHost_0.Set(this.method_1(string_2), value);
        }

        public void Set(string string_2, Color color)
        {
            this.isettingsHost_0.Set(this.method_1(string_2), color);
        }

        public void Set(string string_2, Font value)
        {
            this.isettingsHost_0.Set(this.method_1(string_2), value);
        }

        public void Set(string string_2, int value)
        {
            this.isettingsHost_0.Set(this.method_1(string_2), value);
        }

        public void Set(string string_2, string value)
        {
            this.isettingsHost_0.Set(this.method_1(string_2), value);
        }

        public void Set(Form form_0, string string_2)
        {
            this.isettingsHost_0.Set(form_0, this.method_1(string_2));
        }

        private void StrategyDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void StrategyDownload_Load(object sender, EventArgs e)
        {
            this.string_1 = MainModule.Instance.Settings.Get("StrategyProvider", "");
            this.assemblyLoader_0.Path = Path.GetDirectoryName(Application.ExecutablePath);
            foreach (System.Type type in this.assemblyLoader_0.Types)
            {
                StrategyProvider item = (StrategyProvider) this.assemblyLoader_0.CreateInstance(type);
                this.cmbStrategyProviders.Items.Add(item);
                if (!string.IsNullOrEmpty(this.string_1) && (this.string_1 == item.FriendlyName))
                {
                    this.cmbStrategyProviders.SelectedIndex = this.cmbStrategyProviders.Items.Count - 1;
                }
            }
            if (this.cmbStrategyProviders.Items.Count == 0)
            {
                this.method_7();
                base.Close();
            }
            else
            {
                if (this.cmbStrategyProviders.Items.Count == 1)
                {
                    this.cmbStrategyProviders.Enabled = false;
                    this.cmbStrategyProviders.SelectedIndex = 0;
                }
                this.method_6();
                this.dtPickerSince.Enabled = this.cbPublishedSince.Checked;
                base.Activate();
            }
        }

        private void StrategyDownload_Shown(object sender, EventArgs e)
        {
            this.method_8();
        }

        public bool CancelDownload
        {
            [CompilerGenerated]
            get
            {
                return this.bool_2;
            }
            [CompilerGenerated]
            set
            {
                this.bool_2 = value;
            }
        }

        public string FolderPath
        {
            get
            {
                return this.string_0;
            }
        }

        public bool StrategiesAdded
        {
            get
            {
                return this.bool_0;
            }
        }

        private delegate void Delegate22(Strategy strategy_0, string string_0);

        private delegate void Delegate23(Exception exception_0);
    }
}

