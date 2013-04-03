namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using WealthLab;

    public class DataManagerForm : Form, IItemTracker<DataSource>, IDataUpdateMessage, IWorkspace, IExtendedBehaviorHost
    {
        private BackgroundWorker backgroundWorker_0;
        private BackgroundWorker backgroundWorker_1;
        private BackgroundWorker backgroundWorker_2;
        private BarsLoader barsLoader_0;
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private bool bool_3 = true;
        private bool bool_4;
        private bool bool_5 = true;
        private Button btnCancelSymbolDetails;
        private ToolStripButton btnCancelUpdate;
        private ToolStripButton btnClearLog;
        private Button btnCloseSymbolDetails;
        private ToolStripButton btnDetails;
        private ToolStripButton btnHelp;
        private Button btnModifySymbols;
        private ToolStripButton btnNewDS;
        private ToolStripButton btnUpdate;
        private ToolStripButton btnUpdatePricing;
        private Button btnUpdateProviders;
        private CheckBox cbAutomatedUpdate;
        private CheckBox cbCleanup;
        private CheckBox cbOnDemand;
        private ComboBox cmbHours;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private DataSource dataSource_0;
        private DateTime dateTime_0;
        private FundamentalsLoader fundamentalsLoader_0;
        private GroupBox grpDataSets;
        private GroupBox grpOnDemand;
        private GroupBox grpProviders;
        private GroupBox grpScheduled;
        private GroupBox grpSymbolDetails;
        private GroupBox grpSymbols;
        private GroupBox grpUpdateOptions;
        private HistoricalProvider historicalProvider_0;
        private IContainer icontainer_0;
        private ImageList imageList_0;
        public static DataManagerForm Instance;
        private ToolStripStatusLabel lblDataSets;
        private Label lblEST;
        private Label lblHours;
        private Label lblProvider;
        private Label lblProviderDesc;
        private Label lblProviderName;
        private Label lblSelectProviders;
        private Label lblUpdateHour;
        private ToolStripLabel lblUpdateProgress;
        private LinkLabel linkProvider;
        private List<HistoricalProvider> list_0 = new List<HistoricalProvider>();
        private List<string> list_1 = new List<string>();
        private DataSourceListView lvDataSets;
        private ListView lvProviders;
        private SortableListView lvSymbolDetails;
        private ToolStripMenuItem mniAddSymbols;
        private ToolStripMenuItem mniReloadSymbolData;
        private ToolStripMenuItem mniRemoveSymbols;
        private ToolStripMenuItem mniStockSplit;
        private TabPage pageDS;
        private TabPage pageUpdateLog;
        private PictureBox picDataSet;
        private ContextMenuStrip popupSymDetails;
        private ProgressBar progSymbolDetails;
        private ToolStripProgressBar progUpdate;
        private RadioButton rbUpdateAllData;
        private RadioButton rbUpdateDataSets;
        private ToolStripSeparator sepUpdate;
        private SplitContainer splitDataSets;
        private ToolStripStatusLabel statusLastUpdateLog;
        private StatusStrip statusStrip1;
        private string string_0 = (MainModule.Instance.DataPath + @"\LastUpdateLog.txt");
        private TabControl tabDSM;
        private TabPage tabUpdate;
        private ToolStrip toolbarUpdate;
        private ToolStrip toolStrip1;
        private TextBox txtSymbols;
        private TextBox txtUpdateLog;

        public DataManagerForm()
        {
            this.InitializeComponent();
        }

        private void backgroundWorker_1_DoWork(object sender, DoWorkEventArgs e)
        {
            StaticDataProvider providerInstance = MainModule.Instance.DataSources.GetProviderInstance(this.dataSource_0.Provider.GetType());
            this.historicalProvider_0 = providerInstance;
            providerInstance.UpdateDataSource(this.dataSource_0, this);
            if (!this.bool_2)
            {
                foreach (HistoricalProvider provider2 in this.list_0)
                {
                    if (this.bool_1)
                    {
                        break;
                    }
                    if ((provider2 is FundamentalDataProvider) && provider2.SupportsDataSourceUpdate)
                    {
                        this.historicalProvider_0 = provider2;
                        this.backgroundWorker_1.ReportProgress(0, provider2);
                        provider2.UpdateDataSource(this.dataSource_0, this);
                        this.backgroundWorker_1.ReportProgress(-1, provider2);
                    }
                }
            }
        }

        private void backgroundWorker_2_DoWork(object sender, DoWorkEventArgs e)
        {
            this.dateTime_0 = DateTime.Now;
            foreach (HistoricalProvider provider in this.list_0)
            {
                if (this.bool_1)
                {
                    break;
                }
                this.historicalProvider_0 = provider;
                this.backgroundWorker_2.ReportProgress(0, provider);
                List<DataSource> dataSources = new List<DataSource>();
                foreach (DataSource source in MainModule.Instance.DataSources.DataSources)
                {
                    if ((source.ProviderName == provider.GetType().Name) || (provider is FundamentalDataProvider))
                    {
                        dataSources.Add(source);
                    }
                }
                provider.UpdateProvider(this, dataSources, this.rbUpdateAllData.Checked, this.cbCleanup.Checked);
                if (!this.bool_1)
                {
                    this.backgroundWorker_2.ReportProgress(-1, provider);
                }
            }
        }

        private void backgroundWorker_2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                if (e.UserState is HistoricalProvider)
                {
                    HistoricalProvider userState = e.UserState as HistoricalProvider;
                    StringBuilder builder = new StringBuilder();
                    if (e.ProgressPercentage == 0)
                    {
                        builder.Append("Updating Provider ");
                        builder.Append(userState.FriendlyName);
                        builder.Append(" ...");
                        builder.Append(Environment.NewLine);
                    }
                    else
                    {
                        builder.Append("Provider update complete for ");
                        builder.Append(userState.FriendlyName);
                        builder.Append(Environment.NewLine);
                        builder.Append("----------------");
                        builder.Append(Environment.NewLine);
                        builder.Append(Environment.NewLine);
                    }
                    this.txtUpdateLog.Text = this.txtUpdateLog.Text + builder.ToString();
                    this.list_1.Add(builder.ToString());
                }
                else if (e.UserState is string)
                {
                    if (this.txtUpdateLog.Text.Length > 0x7d00)
                    {
                        this.txtUpdateLog.Text = "Log window cleared.  After update completes, click the status bar link below to view the full Update Log." + Environment.NewLine;
                    }
                    this.txtUpdateLog.Text = this.txtUpdateLog.Text + ((string) e.UserState) + Environment.NewLine;
                    this.txtUpdateLog.SelectionStart = this.txtUpdateLog.Text.Length;
                    this.txtUpdateLog.ScrollToCaret();
                }
            }
            else if (((e != null) && (Instance != null)) && (this.progUpdate != null))
            {
                try
                {
                    this.progUpdate.Value = e.ProgressPercentage;
                }
                catch (NullReferenceException)
                {
                }
            }
        }

        private void backgroundWorker_2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.bool_2 = false;
            TimeSpan span = (TimeSpan) (DateTime.Now - this.dateTime_0);
            StringBuilder builder = new StringBuilder();
            if (this.bool_1)
            {
                builder.Append("Update cancelled (");
            }
            else
            {
                builder.Append("Update completed (");
            }
            if (span.Hours > 0)
            {
                builder.Append(span.Hours);
                builder.Append(" hrs ");
                span = span.Subtract(new TimeSpan(span.Hours, 0, 0));
            }
            if (span.Minutes > 0)
            {
                builder.Append(span.Minutes);
                builder.Append(" min ");
                span = span.Subtract(new TimeSpan(0, span.Minutes, 0));
            }
            builder.Append(span.Seconds);
            builder.Append(".");
            builder.Append(span.Milliseconds);
            builder.Append(" sec)");
            builder.Append(Environment.NewLine);
            builder.Append("----------------");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            this.txtUpdateLog.Text = this.txtUpdateLog.Text + builder.ToString();
            this.list_1.Add(builder.ToString());
            this.Cursor = Cursors.Default;
            this.txtUpdateLog.Cursor = Cursors.Default;
            this.txtUpdateLog.SelectionStart = this.txtUpdateLog.Text.Length;
            this.txtUpdateLog.ScrollToCaret();
            this.btnCancelUpdate.Enabled = false;
            this.progUpdate.Value = 0;
            string[] contents = new string[this.list_1.Count];
            int num = 0;
            foreach (string str in this.list_1)
            {
                contents[num++] = str;
            }
            FileNameValidator.ValidateFileName(this.string_0);
            File.WriteAllLines(this.string_0, contents);
            this.list_1.Clear();
            this.method_6();
            this.bool_3 = true;
        }

        private void btnCancelSymbolDetails_Click(object sender, EventArgs e)
        {
            this.bool_0 = true;
        }

        private void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Environment.NewLine);
            builder.Append("Cancelling update, please wait ...");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            this.txtUpdateLog.Text = this.txtUpdateLog.Text + builder.ToString();
            this.txtUpdateLog.Cursor = Cursors.WaitCursor;
            this.bool_1 = true;
            this.historicalProvider_0.CancelUpdate();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            this.txtUpdateLog.Clear();
        }

        private void btnCloseSymbolDetails_Click(object sender, EventArgs e)
        {
            this.grpSymbolDetails.Visible = false;
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            if (this.dataSource_0 != null)
            {
                bool onDemandUpdatesEnabled = MainModule.Instance.DataSources.OnDemandUpdatesEnabled;
                MainModule.Instance.DataSources.OnDemandUpdatesEnabled = false;
                this.barsLoader_0.Scale = this.dataSource_0.Scale;
                this.barsLoader_0.BarInterval = this.dataSource_0.BarInterval;
                try
                {
                    this.grpSymbolDetails.BringToFront();
                    this.grpSymbolDetails.Visible = true;
                    this.method_2(true);
                    this.progSymbolDetails.Value = 0;
                    this.progSymbolDetails.Maximum = this.dataSource_0.Symbols.Count;
                    this.progSymbolDetails.Visible = true;
                    this.bool_0 = false;
                    this.Cursor = Cursors.WaitCursor;
                    this.txtUpdateLog.Cursor = Cursors.WaitCursor;
                    if (this.dataSource_0.IsIntraday)
                    {
                        this.columnHeader_4.Tag = "DT";
                    }
                    else
                    {
                        this.columnHeader_4.Tag = "D";
                    }
                    this.lvSymbolDetails.BeginUpdate();
                    this.lvSymbolDetails.Items.Clear();
                    foreach (string str in this.dataSource_0.Symbols)
                    {
                        Bars data = this.barsLoader_0.GetData(this.dataSource_0, str);
                        ListViewItem item = this.lvSymbolDetails.Items.Add(data.Symbol);
                        item.SubItems.Add(data.SecurityName);
                        item.SubItems.Add(data.Count.ToString("N0"));
                        if (data.Count > 0)
                        {
                            DateTime time = data.Date[data.Count - 1];
                            string text = time.ToShortDateString();
                            if (data.IsIntraday)
                            {
                                text = text + " " + time.ToShortTimeString();
                            }
                            item.SubItems.Add(text);
                        }
                        else
                        {
                            item.SubItems.Add("");
                        }
                        this.progSymbolDetails.Value++;
                        Application.DoEvents();
                        if (this.bool_0)
                        {
                            return;
                        }
                    }
                }
                finally
                {
                    this.method_2(false);
                    this.lvSymbolDetails.EndUpdate();
                    MainModule.Instance.DataSources.OnDemandUpdatesEnabled = onDemandUpdatesEnabled;
                    this.Cursor = Cursors.Default;
                    this.txtUpdateLog.Cursor = Cursors.Default;
                }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MainModule.Instance.ContextSensitiveHelp("dataManager.htm");
        }

        private void btnModifySymbols_Click(object sender, EventArgs e)
        {
            SymbolParser parser = new SymbolParser {
                Text = this.txtSymbols.Text
            };
            MainModule.Instance.DataSources.ModifyDataSourceSymbols(this.dataSource_0, parser.Symbols);
        }

        private void btnNewDS_Click(object sender, EventArgs e)
        {
            MainModule.Instance.CreateNewDataSource();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.list_1.Clear();
            this.tabDSM.SelectedTab = this.pageUpdateLog;
            this.btnCancelUpdate.Enabled = true;
            this.progUpdate.Value = 0;
            this.bool_1 = false;
            this.dateTime_0 = DateTime.Now;
            this.list_0.Clear();
            foreach (ListViewItem item in this.lvProviders.Items)
            {
                if (item.Checked)
                {
                    this.list_0.Add(item.Tag as HistoricalProvider);
                }
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("Updating DataSet ");
            builder.Append(this.dataSource_0.Name);
            builder.Append(" ...");
            builder.Append(Environment.NewLine);
            this.txtUpdateLog.Text = this.txtUpdateLog.Text + builder.ToString();
            this.list_1.Add(builder.ToString());
            this.Cursor = Cursors.WaitCursor;
            this.txtUpdateLog.Cursor = Cursors.WaitCursor;
            this.bool_3 = false;
            this.backgroundWorker_0 = this.backgroundWorker_1;
            this.backgroundWorker_1.RunWorkerAsync();
        }

        private void btnUpdatePricing_Click(object sender, EventArgs e)
        {
            this.bool_2 = true;
            this.btnUpdate_Click(sender, e);
        }

        private void btnUpdateProviders_Click(object sender, EventArgs e)
        {
            this.UpdateProviders();
        }

        private void cbAutomatedUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.bool_5)
            {
                this.method_0();
            }
        }

        private void cbOnDemand_CheckedChanged(object sender, EventArgs e)
        {
            MainModule.Instance.SetOnDemand(this.cbOnDemand.Checked);
        }

        private void cmbHours_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.bool_5)
            {
                this.method_0();
            }
        }

        public void CreateNewDataSet()
        {
            MainModule.Instance.CreateNewDataSource();
        }

        private void DataManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainModule.Instance.Settings.Set(this, "DataManagerForm");
            MainModule.Instance.DataSources.UnregisterObserver(this.lvDataSets);
            MainModule.Instance.DataSources.UnregisterObserver(this);
            Instance = null;
            ISettingsHost settings = MainModule.Instance.Settings;
            settings.Set("UpdateOnlySymbolsInDataSets", this.rbUpdateDataSets.Checked);
            settings.Set("CleanupSymbolsOnUpdate", this.cbCleanup.Checked);
            this.method_0();
            string str = "";
            foreach (ListViewItem item in this.lvProviders.Items)
            {
                if (item.Checked)
                {
                    str = str + item.Text + ";";
                }
            }
            settings.Set("ProvidersToUpdate", str);
            foreach (StaticDataProvider provider in MainModule.Instance.DataSources.Providers)
            {
                foreach (DataBehaviorUserControl control in provider.ExtendedBehaviors)
                {
                    control.RegisterObserver(this);
                }
            }
            MainModule.Instance.Settings.SaveSettings();
        }

        private void DataManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.bool_4)
            {
                e.Cancel = true;
            }
            else
            {
                this.bool_4 = true;
                if (this.btnCancelUpdate.Enabled)
                {
                    this.btnCancelUpdate_Click(this, e);
                    while (!this.bool_3)
                    {
                        Application.DoEvents();
                    }
                }
            }
        }

        private void DataManagerForm_Load(object sender, EventArgs e)
        {
            Instance = this;
            ISettingsHost settings = MainModule.Instance.Settings;
            settings.Get(this, "DataManagerForm");
            this.toolbarUpdate.Visible = true;
            this.lvDataSets.Populate(MainModule.Instance.DataSources);
            this.lblDataSets.Text = this.lvDataSets.Items.Count + " DataSets";
            MainModule.Instance.DataSources.RegisterObserver(this.lvDataSets);
            MainModule.Instance.DataSources.RegisterObserver(this);
            ListViewGroup group = new ListViewGroup("Historical Data Providers");
            this.lvProviders.Groups.Add(group);
            foreach (StaticDataProvider provider2 in MainModule.Instance.DataSources.Providers)
            {
                if (provider2.SupportsProviderUpdate)
                {
                    ListViewItem item = this.lvProviders.Items.Add(provider2.FriendlyName);
                    this.imageList_0.Images.Add(provider2.Glyph);
                    item.ImageIndex = this.imageList_0.Images.Count - 1;
                    item.Group = group;
                    item.Tag = provider2;
                }
            }
            group = new ListViewGroup("Fundamental Data Providers");
            this.lvProviders.Groups.Add(group);
            this.fundamentalsLoader_0.DataHost = MainModule.Instance.DataSources;
            foreach (FundamentalDataProvider provider3 in this.fundamentalsLoader_0.Providers)
            {
                if (provider3.SupportsProviderUpdate)
                {
                    ListViewItem item3 = this.lvProviders.Items.Add(provider3.FriendlyName);
                    this.imageList_0.Images.Add(provider3.Glyph);
                    item3.ImageIndex = this.imageList_0.Images.Count - 1;
                    item3.Group = group;
                    item3.Tag = provider3;
                }
            }
            this.cbOnDemand.Checked = MainModule.Instance.DataSources.OnDemandUpdatesEnabled;
            this.rbUpdateDataSets.Checked = settings.Get("UpdateOnlySymbolsInDataSets", true);
            this.rbUpdateAllData.Checked = !this.rbUpdateDataSets.Checked;
            this.cbCleanup.Checked = settings.Get("CleanupSymbolsOnUpdate", false);
            this.cbAutomatedUpdate.Checked = settings.Get("ScheduledDataUpdates", false);
            string str = settings.Get("ScheduledUpdateTime_Local", "07:00");
            this.cmbHours.SelectedIndex = this.cmbHours.Items.IndexOf(str);
            string str2 = settings.Get("ProvidersToUpdate", "");
            foreach (ListViewItem item2 in this.lvProviders.Items)
            {
                if (str2.Contains(item2.Text + ";"))
                {
                    item2.Checked = true;
                }
            }
            this.method_6();
            foreach (StaticDataProvider provider in MainModule.Instance.DataSources.Providers)
            {
                foreach (DataBehaviorUserControl control in provider.ExtendedBehaviors)
                {
                    control.Initialize(MainModule.Instance.DataSources);
                    control.RegisterObserver(this);
                    TabPage page = new TabPage {
                        Text = (string) control.Tag
                    };
                    page.Controls.Add(control);
                    control.Dock = DockStyle.Fill;
                    this.tabDSM.TabPages.Add(page);
                }
            }
            this.bool_5 = false;
        }

        public void DisplayUpdateMessage(string message)
        {
            this.backgroundWorker_0.ReportProgress(-1, message);
            this.list_1.Add(message);
            Application.DoEvents();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DataManagerForm));
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.popupSymDetails = new ContextMenuStrip(this.icontainer_0);
            this.mniAddSymbols = new ToolStripMenuItem();
            this.mniRemoveSymbols = new ToolStripMenuItem();
            this.mniReloadSymbolData = new ToolStripMenuItem();
            this.mniStockSplit = new ToolStripMenuItem();
            this.barsLoader_0 = new BarsLoader(this.icontainer_0);
            this.backgroundWorker_1 = new BackgroundWorker();
            this.fundamentalsLoader_0 = new FundamentalsLoader(this.icontainer_0);
            this.backgroundWorker_2 = new BackgroundWorker();
            this.statusStrip1 = new StatusStrip();
            this.lblDataSets = new ToolStripStatusLabel();
            this.statusLastUpdateLog = new ToolStripStatusLabel();
            this.toolStrip1 = new ToolStrip();
            this.btnNewDS = new ToolStripButton();
            this.btnUpdate = new ToolStripButton();
            this.btnUpdatePricing = new ToolStripButton();
            this.btnDetails = new ToolStripButton();
            this.btnHelp = new ToolStripButton();
            this.tabDSM = new TabControl();
            this.pageDS = new TabPage();
            this.splitDataSets = new SplitContainer();
            this.grpDataSets = new GroupBox();
            this.picDataSet = new PictureBox();
            this.linkProvider = new LinkLabel();
            this.lblProviderDesc = new Label();
            this.lblProviderName = new Label();
            this.lblProvider = new Label();
            this.lvDataSets = new DataSourceListView();
            this.grpSymbolDetails = new GroupBox();
            this.btnCancelSymbolDetails = new Button();
            this.progSymbolDetails = new ProgressBar();
            this.lvSymbolDetails = new SortableListView();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.btnCloseSymbolDetails = new Button();
            this.grpSymbols = new GroupBox();
            this.btnModifySymbols = new Button();
            this.txtSymbols = new TextBox();
            this.tabUpdate = new TabPage();
            this.grpOnDemand = new GroupBox();
            this.cbOnDemand = new CheckBox();
            this.grpUpdateOptions = new GroupBox();
            this.cbCleanup = new CheckBox();
            this.rbUpdateAllData = new RadioButton();
            this.rbUpdateDataSets = new RadioButton();
            this.grpScheduled = new GroupBox();
            this.lblHours = new Label();
            this.lblEST = new Label();
            this.cmbHours = new ComboBox();
            this.lblUpdateHour = new Label();
            this.cbAutomatedUpdate = new CheckBox();
            this.grpProviders = new GroupBox();
            this.lvProviders = new ListView();
            this.columnHeader_0 = new ColumnHeader();
            this.lblSelectProviders = new Label();
            this.btnUpdateProviders = new Button();
            this.pageUpdateLog = new TabPage();
            this.txtUpdateLog = new TextBox();
            this.toolbarUpdate = new ToolStrip();
            this.lblUpdateProgress = new ToolStripLabel();
            this.progUpdate = new ToolStripProgressBar();
            this.btnCancelUpdate = new ToolStripButton();
            this.sepUpdate = new ToolStripSeparator();
            this.btnClearLog = new ToolStripButton();
            this.popupSymDetails.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabDSM.SuspendLayout();
            this.pageDS.SuspendLayout();
            this.splitDataSets.Panel1.SuspendLayout();
            this.splitDataSets.Panel2.SuspendLayout();
            this.splitDataSets.SuspendLayout();
            this.grpDataSets.SuspendLayout();
            ((ISupportInitialize) this.picDataSet).BeginInit();
            this.grpSymbolDetails.SuspendLayout();
            this.grpSymbols.SuspendLayout();
            this.tabUpdate.SuspendLayout();
            this.grpOnDemand.SuspendLayout();
            this.grpUpdateOptions.SuspendLayout();
            this.grpScheduled.SuspendLayout();
            this.grpProviders.SuspendLayout();
            this.pageUpdateLog.SuspendLayout();
            this.toolbarUpdate.SuspendLayout();
            base.SuspendLayout();
            this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("images.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "NewDS.bmp");
            this.popupSymDetails.Items.AddRange(new ToolStripItem[] { this.mniAddSymbols, this.mniRemoveSymbols, this.mniReloadSymbolData, this.mniStockSplit });
            this.popupSymDetails.Name = "popupSymDetails";
            this.popupSymDetails.Size = new Size(0xdb, 0x5c);
            this.mniAddSymbols.Enabled = false;
            this.mniAddSymbols.Image = (Image) manager.GetObject("mniAddSymbols.Image");
            this.mniAddSymbols.ImageTransparentColor = Color.Fuchsia;
            this.mniAddSymbols.Name = "mniAddSymbols";
            this.mniAddSymbols.Size = new Size(0xda, 0x16);
            this.mniAddSymbols.Text = "Add Symbols ...";
            this.mniAddSymbols.Click += new EventHandler(this.mniAddSymbols_Click);
            this.mniRemoveSymbols.Enabled = false;
            this.mniRemoveSymbols.Image = (Image) manager.GetObject("mniRemoveSymbols.Image");
            this.mniRemoveSymbols.ImageTransparentColor = Color.Fuchsia;
            this.mniRemoveSymbols.Name = "mniRemoveSymbols";
            this.mniRemoveSymbols.Size = new Size(0xda, 0x16);
            this.mniRemoveSymbols.Text = "Remove Selected Symbol(s)";
            this.mniRemoveSymbols.Click += new EventHandler(this.mniRemoveSymbols_Click);
            this.mniReloadSymbolData.Name = "mniReloadSymbolData";
            this.mniReloadSymbolData.Size = new Size(0xda, 0x16);
            this.mniReloadSymbolData.Text = "Reload Symbol Data File";
            this.mniReloadSymbolData.Click += new EventHandler(this.mniReloadSymbolData_Click);
            this.mniStockSplit.Name = "mniStockSplit";
            this.mniStockSplit.Size = new Size(0xda, 0x16);
            this.mniStockSplit.Text = "Enter a Stock Split";
            this.mniStockSplit.Click += new EventHandler(this.mniStockSplit_Click);
            this.barsLoader_0.AutoConvertScale = true;
            this.barsLoader_0.AutoCreateProvider = true;
            this.barsLoader_0.BarInterval = 0;
            this.barsLoader_0.EndDate = new DateTime(0x270f, 12, 0x1f, 0x17, 0x3b, 0x3b, 0x3e7);
            this.barsLoader_0.IncludePartialBar = false;
            this.barsLoader_0.MaxBars = 0;
            this.barsLoader_0.OverrideOnDemand = false;
            this.barsLoader_0.OverrideOnDemandValue = false;
            this.barsLoader_0.Scale = BarScale.Daily;
            this.barsLoader_0.StartDate = new DateTime(0L);
            this.backgroundWorker_1.WorkerReportsProgress = true;
            this.backgroundWorker_1.WorkerSupportsCancellation = true;
            this.backgroundWorker_1.DoWork += new DoWorkEventHandler(this.backgroundWorker_1_DoWork);
            this.backgroundWorker_1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_2_RunWorkerCompleted);
            this.backgroundWorker_1.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker_2_ProgressChanged);
            this.backgroundWorker_2.WorkerReportsProgress = true;
            this.backgroundWorker_2.WorkerSupportsCancellation = true;
            this.backgroundWorker_2.DoWork += new DoWorkEventHandler(this.backgroundWorker_2_DoWork);
            this.backgroundWorker_2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_2_RunWorkerCompleted);
            this.backgroundWorker_2.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker_2_ProgressChanged);
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.lblDataSets, this.statusLastUpdateLog });
            this.statusStrip1.Location = new Point(0, 0x19c);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x266, 0x16);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            this.lblDataSets.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.lblDataSets.Name = "lblDataSets";
            this.lblDataSets.Size = new Size(0x40, 0x11);
            this.lblDataSets.Text = "0 DataSets";
            this.statusLastUpdateLog.Name = "statusLastUpdateLog";
            this.statusLastUpdateLog.Size = new Size(0xaf, 0x11);
            this.statusLastUpdateLog.Text = "Last Update Log does not yet exist";
            this.statusLastUpdateLog.Click += new EventHandler(this.statusLastUpdateLog_Click);
            this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.btnNewDS, this.btnUpdate, this.btnUpdatePricing, this.btnDetails, this.btnHelp });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x266, 0x19);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.btnNewDS.Image = (Image) manager.GetObject("btnNewDS.Image");
            this.btnNewDS.ImageTransparentColor = Color.Magenta;
            this.btnNewDS.Name = "btnNewDS";
            this.btnNewDS.Size = new Size(0x86, 0x16);
            this.btnNewDS.Text = "Create a new DataSet";
            this.btnNewDS.Click += new EventHandler(this.btnNewDS_Click);
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Image = (Image) manager.GetObject("btnUpdate.Image");
            this.btnUpdate.ImageTransparentColor = Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new Size(0x68, 0x16);
            this.btnUpdate.Text = "Update DataSet";
            this.btnUpdate.ToolTipText = "Update pricing and fundamentals data for the selected DataSet";
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            this.btnUpdatePricing.Enabled = false;
            this.btnUpdatePricing.Image = (Image) manager.GetObject("btnUpdatePricing.Image");
            this.btnUpdatePricing.ImageTransparentColor = Color.Magenta;
            this.btnUpdatePricing.Name = "btnUpdatePricing";
            this.btnUpdatePricing.Size = new Size(0x92, 0x16);
            this.btnUpdatePricing.Text = "Update DataSet (Pricing)";
            this.btnUpdatePricing.ToolTipText = "Update pricing data for the selected DataSet";
            this.btnUpdatePricing.Click += new EventHandler(this.btnUpdatePricing_Click);
            this.btnDetails.Enabled = false;
            this.btnDetails.Image = (Image) manager.GetObject("btnDetails.Image");
            this.btnDetails.ImageTransparentColor = Color.Magenta;
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new Size(0x60, 0x16);
            this.btnDetails.Text = "Symbol Details";
            this.btnDetails.ToolTipText = "Get Symbol details for the selected DataSet";
            this.btnDetails.Click += new EventHandler(this.btnDetails_Click);
            this.btnHelp.Image = (Image) manager.GetObject("btnHelp.Image");
            this.btnHelp.ImageTransparentColor = Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x30, 0x16);
            this.btnHelp.Text = "Help";
            this.btnHelp.ToolTipText = "Help on Data Manager";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.tabDSM.Controls.Add(this.pageDS);
            this.tabDSM.Controls.Add(this.tabUpdate);
            this.tabDSM.Controls.Add(this.pageUpdateLog);
            this.tabDSM.Dock = DockStyle.Fill;
            this.tabDSM.Location = new Point(0, 0x19);
            this.tabDSM.Name = "tabDSM";
            this.tabDSM.SelectedIndex = 0;
            this.tabDSM.Size = new Size(0x266, 0x183);
            this.tabDSM.TabIndex = 5;
            this.tabDSM.Selecting += new TabControlCancelEventHandler(this.tabDSM_Selecting);
            this.tabDSM.SelectedIndexChanged += new EventHandler(this.tabDSM_SelectedIndexChanged);
            this.pageDS.BackColor = Color.Transparent;
            this.pageDS.Controls.Add(this.splitDataSets);
            this.pageDS.Location = new Point(4, 0x16);
            this.pageDS.Name = "pageDS";
            this.pageDS.Padding = new Padding(3);
            this.pageDS.Size = new Size(0x25e, 0x169);
            this.pageDS.TabIndex = 0;
            this.pageDS.Text = "Data Sets";
            this.splitDataSets.BackColor = SystemColors.ControlDark;
            this.splitDataSets.Dock = DockStyle.Fill;
            this.splitDataSets.Location = new Point(3, 3);
            this.splitDataSets.Name = "splitDataSets";
            this.splitDataSets.Panel1.BackColor = SystemColors.Control;
            this.splitDataSets.Panel1.Controls.Add(this.grpDataSets);
            this.splitDataSets.Panel2.BackColor = SystemColors.Control;
            this.splitDataSets.Panel2.Controls.Add(this.grpSymbolDetails);
            this.splitDataSets.Panel2.Controls.Add(this.grpSymbols);
            this.splitDataSets.Size = new Size(600, 0x163);
            this.splitDataSets.SplitterDistance = 0x133;
            this.splitDataSets.TabIndex = 4;
            this.grpDataSets.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.grpDataSets.Controls.Add(this.picDataSet);
            this.grpDataSets.Controls.Add(this.linkProvider);
            this.grpDataSets.Controls.Add(this.lblProviderDesc);
            this.grpDataSets.Controls.Add(this.lblProviderName);
            this.grpDataSets.Controls.Add(this.lblProvider);
            this.grpDataSets.Controls.Add(this.lvDataSets);
            this.grpDataSets.Location = new Point(5, 3);
            this.grpDataSets.Name = "grpDataSets";
            this.grpDataSets.Size = new Size(0x12b, 0x156);
            this.grpDataSets.TabIndex = 2;
            this.grpDataSets.TabStop = false;
            this.grpDataSets.Text = "DataSets";
            this.picDataSet.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.picDataSet.BackColor = Color.Transparent;
            this.picDataSet.Location = new Point(7, 0x10d);
            this.picDataSet.Name = "picDataSet";
            this.picDataSet.Size = new Size(0x10, 0x10);
            this.picDataSet.TabIndex = 6;
            this.picDataSet.TabStop = false;
            this.linkProvider.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.linkProvider.AutoSize = true;
            this.linkProvider.Location = new Point(9, 0x143);
            this.linkProvider.Name = "linkProvider";
            this.linkProvider.Size = new Size(0x2b, 13);
            this.linkProvider.TabIndex = 5;
            this.linkProvider.TabStop = true;
            this.linkProvider.Text = "More ...";
            this.linkProvider.Visible = false;
            this.linkProvider.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkProvider_LinkClicked);
            this.lblProviderDesc.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.lblProviderDesc.Location = new Point(0x21, 0x11d);
            this.lblProviderDesc.Name = "lblProviderDesc";
            this.lblProviderDesc.Size = new Size(260, 0x33);
            this.lblProviderDesc.TabIndex = 4;
            this.lblProviderName.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lblProviderName.AutoSize = true;
            this.lblProviderName.ForeColor = SystemColors.Highlight;
            this.lblProviderName.Location = new Point(0x74, 0x10d);
            this.lblProviderName.Name = "lblProviderName";
            this.lblProviderName.Size = new Size(0, 13);
            this.lblProviderName.TabIndex = 2;
            this.lblProvider.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lblProvider.AutoSize = true;
            this.lblProvider.Location = new Point(0x1d, 0x10d);
            this.lblProvider.Name = "lblProvider";
            this.lblProvider.Size = new Size(0x54, 13);
            this.lblProvider.TabIndex = 1;
            this.lblProvider.Text = "Provider Details:";
            this.lvDataSets.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvDataSets.FullRowSelect = true;
            this.lvDataSets.HideSelection = false;
            this.lvDataSets.Location = new Point(7, 20);
            this.lvDataSets.MultiSelect = false;
            this.lvDataSets.Name = "lvDataSets";
            this.lvDataSets.Size = new Size(0x11e, 0xf6);
            this.lvDataSets.TabIndex = 0;
            this.lvDataSets.Text = "dataSourceListView1";
            this.lvDataSets.UseCompatibleStateImageBehavior = false;
            this.lvDataSets.View = View.Details;
            this.lvDataSets.DataSourceSelected += new EventHandler<DataSourceEventArgs>(this.method_1);
            this.lvDataSets.NewDataSourceClicked += new EventHandler<EventArgs>(this.method_3);
            this.grpSymbolDetails.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.grpSymbolDetails.Controls.Add(this.btnCancelSymbolDetails);
            this.grpSymbolDetails.Controls.Add(this.progSymbolDetails);
            this.grpSymbolDetails.Controls.Add(this.lvSymbolDetails);
            this.grpSymbolDetails.Controls.Add(this.btnCloseSymbolDetails);
            this.grpSymbolDetails.Location = new Point(3, 3);
            this.grpSymbolDetails.Name = "grpSymbolDetails";
            this.grpSymbolDetails.Size = new Size(0x11b, 0x156);
            this.grpSymbolDetails.TabIndex = 4;
            this.grpSymbolDetails.TabStop = false;
            this.grpSymbolDetails.Text = "Symbol Details";
            this.grpSymbolDetails.Visible = false;
            this.btnCancelSymbolDetails.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancelSymbolDetails.Location = new Point(0xd5, 0x138);
            this.btnCancelSymbolDetails.Name = "btnCancelSymbolDetails";
            this.btnCancelSymbolDetails.Size = new Size(0x40, 0x17);
            this.btnCancelSymbolDetails.TabIndex = 3;
            this.btnCancelSymbolDetails.Text = "Cancel";
            this.btnCancelSymbolDetails.UseVisualStyleBackColor = true;
            this.btnCancelSymbolDetails.Click += new EventHandler(this.btnCancelSymbolDetails_Click);
            this.progSymbolDetails.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.progSymbolDetails.Location = new Point(7, 0x138);
            this.progSymbolDetails.Name = "progSymbolDetails";
            this.progSymbolDetails.Size = new Size(200, 0x17);
            this.progSymbolDetails.TabIndex = 2;
            this.lvSymbolDetails.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvSymbolDetails.Columns.AddRange(new ColumnHeader[] { this.columnHeader_1, this.columnHeader_2, this.columnHeader_3, this.columnHeader_4 });
            this.lvSymbolDetails.ContextMenuStrip = this.popupSymDetails;
            this.lvSymbolDetails.FullRowSelect = true;
            this.lvSymbolDetails.HideSelection = false;
            this.lvSymbolDetails.Location = new Point(6, 0x13);
            this.lvSymbolDetails.Name = "lvSymbolDetails";
            this.lvSymbolDetails.Size = new Size(0x10f, 0x11e);
            this.lvSymbolDetails.TabIndex = 1;
            this.lvSymbolDetails.UseCompatibleStateImageBehavior = false;
            this.lvSymbolDetails.View = View.Details;
            this.lvSymbolDetails.SelectedIndexChanged += new EventHandler(this.lvSymbolDetails_SelectedIndexChanged);
            this.columnHeader_1.Text = "Symbol";
            this.columnHeader_1.Width = 50;
            this.columnHeader_2.Text = "Security";
            this.columnHeader_2.Width = 80;
            this.columnHeader_3.Tag = "N";
            this.columnHeader_3.Text = "Bars";
            this.columnHeader_3.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_3.Width = 50;
            this.columnHeader_4.Tag = "D";
            this.columnHeader_4.Text = "Updated";
            this.columnHeader_4.Width = 80;
            this.btnCloseSymbolDetails.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCloseSymbolDetails.Location = new Point(0xd5, 0x138);
            this.btnCloseSymbolDetails.Name = "btnCloseSymbolDetails";
            this.btnCloseSymbolDetails.Size = new Size(0x40, 0x17);
            this.btnCloseSymbolDetails.TabIndex = 0;
            this.btnCloseSymbolDetails.Text = "Close";
            this.btnCloseSymbolDetails.UseVisualStyleBackColor = true;
            this.btnCloseSymbolDetails.Click += new EventHandler(this.btnCloseSymbolDetails_Click);
            this.grpSymbols.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.grpSymbols.Controls.Add(this.btnModifySymbols);
            this.grpSymbols.Controls.Add(this.txtSymbols);
            this.grpSymbols.Location = new Point(3, 3);
            this.grpSymbols.Name = "grpSymbols";
            this.grpSymbols.Size = new Size(0x11b, 0x156);
            this.grpSymbols.TabIndex = 3;
            this.grpSymbols.TabStop = false;
            this.grpSymbols.Text = "Symbols";
            this.btnModifySymbols.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnModifySymbols.Enabled = false;
            this.btnModifySymbols.Location = new Point(6, 0x139);
            this.btnModifySymbols.Name = "btnModifySymbols";
            this.btnModifySymbols.Size = new Size(0x10f, 0x17);
            this.btnModifySymbols.TabIndex = 1;
            this.btnModifySymbols.Text = "Apply Symbol Changes made above to DataSet";
            this.btnModifySymbols.UseVisualStyleBackColor = true;
            this.btnModifySymbols.Click += new EventHandler(this.btnModifySymbols_Click);
            this.txtSymbols.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.txtSymbols.CharacterCasing = CharacterCasing.Upper;
            this.txtSymbols.Location = new Point(7, 20);
            this.txtSymbols.Multiline = true;
            this.txtSymbols.Name = "txtSymbols";
            this.txtSymbols.ScrollBars = ScrollBars.Vertical;
            this.txtSymbols.Size = new Size(270, 0x11f);
            this.txtSymbols.TabIndex = 0;
            this.txtSymbols.TextChanged += new EventHandler(this.txtSymbols_TextChanged);
            this.tabUpdate.BackColor = SystemColors.Control;
            this.tabUpdate.Controls.Add(this.grpOnDemand);
            this.tabUpdate.Controls.Add(this.grpUpdateOptions);
            this.tabUpdate.Controls.Add(this.grpScheduled);
            this.tabUpdate.Controls.Add(this.grpProviders);
            this.tabUpdate.Location = new Point(4, 0x16);
            this.tabUpdate.Name = "tabUpdate";
            this.tabUpdate.Padding = new Padding(3);
            this.tabUpdate.Size = new Size(0x25e, 0x169);
            this.tabUpdate.TabIndex = 1;
            this.tabUpdate.Text = "Update Data";
            this.grpOnDemand.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.grpOnDemand.Controls.Add(this.cbOnDemand);
            this.grpOnDemand.Location = new Point(0x127, 0x129);
            this.grpOnDemand.Name = "grpOnDemand";
            this.grpOnDemand.Size = new Size(0x12f, 0x3a);
            this.grpOnDemand.TabIndex = 3;
            this.grpOnDemand.TabStop = false;
            this.grpOnDemand.Text = "On Demand Data Updates";
            this.cbOnDemand.Checked = true;
            this.cbOnDemand.CheckState = CheckState.Checked;
            this.cbOnDemand.Location = new Point(10, 15);
            this.cbOnDemand.Name = "cbOnDemand";
            this.cbOnDemand.Size = new Size(0x11f, 0x25);
            this.cbOnDemand.TabIndex = 0;
            this.cbOnDemand.Text = "Automatically update data for symbols on-demand when they are charted or accessed";
            this.cbOnDemand.UseVisualStyleBackColor = true;
            this.cbOnDemand.CheckedChanged += new EventHandler(this.cbOnDemand_CheckedChanged);
            this.grpUpdateOptions.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.grpUpdateOptions.Controls.Add(this.cbCleanup);
            this.grpUpdateOptions.Controls.Add(this.rbUpdateAllData);
            this.grpUpdateOptions.Controls.Add(this.rbUpdateDataSets);
            this.grpUpdateOptions.Location = new Point(0x127, 7);
            this.grpUpdateOptions.Name = "grpUpdateOptions";
            this.grpUpdateOptions.Size = new Size(0x12f, 0x7e);
            this.grpUpdateOptions.TabIndex = 1;
            this.grpUpdateOptions.TabStop = false;
            this.grpUpdateOptions.Text = "Data Provider Update Options";
            this.cbCleanup.Location = new Point(10, 0x52);
            this.cbCleanup.Name = "cbCleanup";
            this.cbCleanup.Size = new Size(0x10f, 0x26);
            this.cbCleanup.TabIndex = 2;
            this.cbCleanup.Text = "Delete data for Symbols that are not contained in DataSets";
            this.cbCleanup.UseVisualStyleBackColor = true;
            this.rbUpdateAllData.Location = new Point(10, 0x2b);
            this.rbUpdateAllData.Name = "rbUpdateAllData";
            this.rbUpdateAllData.Size = new Size(0x10f, 0x20);
            this.rbUpdateAllData.TabIndex = 1;
            this.rbUpdateAllData.Text = "Also Update Symbols that you accessed, but are not contained in DataSets";
            this.rbUpdateAllData.UseVisualStyleBackColor = true;
            this.rbUpdateDataSets.AutoSize = true;
            this.rbUpdateDataSets.Checked = true;
            this.rbUpdateDataSets.Location = new Point(10, 20);
            this.rbUpdateDataSets.Name = "rbUpdateDataSets";
            this.rbUpdateDataSets.Size = new Size(0x10f, 0x11);
            this.rbUpdateDataSets.TabIndex = 0;
            this.rbUpdateDataSets.TabStop = true;
            this.rbUpdateDataSets.Text = "Update only Symbols that are contained in DataSets";
            this.rbUpdateDataSets.UseVisualStyleBackColor = true;
            this.rbUpdateDataSets.CheckedChanged += new EventHandler(this.rbUpdateDataSets_CheckedChanged);
            this.grpScheduled.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            this.grpScheduled.Controls.Add(this.lblHours);
            this.grpScheduled.Controls.Add(this.lblEST);
            this.grpScheduled.Controls.Add(this.cmbHours);
            this.grpScheduled.Controls.Add(this.lblUpdateHour);
            this.grpScheduled.Controls.Add(this.cbAutomatedUpdate);
            this.grpScheduled.Location = new Point(0x127, 0x88);
            this.grpScheduled.Name = "grpScheduled";
            this.grpScheduled.Size = new Size(0x12f, 0x9c);
            this.grpScheduled.TabIndex = 2;
            this.grpScheduled.TabStop = false;
            this.grpScheduled.Text = "Automated Data Updates";
            this.lblHours.Location = new Point(7, 0x6c);
            this.lblHours.Name = "lblHours";
            this.lblHours.Size = new Size(290, 0x2c);
            this.lblHours.TabIndex = 4;
            this.lblHours.Text = "Note: The later the Hour you select for an Automated Data Update, the better chance the data Provider will have had to apply the most recent corrections to historical data.";
            this.lblEST.AutoSize = true;
            this.lblEST.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Italic, GraphicsUnit.Point, 0);
            this.lblEST.Location = new Point(7, 0x55);
            this.lblEST.Name = "lblEST";
            this.lblEST.Size = new Size(0xe8, 13);
            this.lblEST.TabIndex = 3;
            this.lblEST.Text = "(Hours above are expressed in your Local Time)";
            this.cmbHours.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbHours.FormattingEnabled = true;
            this.cmbHours.Items.AddRange(new object[] { 
                "00:00", "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", 
                "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00"
             });
            this.cmbHours.Location = new Point(0x60, 0x3a);
            this.cmbHours.Name = "cmbHours";
            this.cmbHours.Size = new Size(0x47, 0x15);
            this.cmbHours.TabIndex = 2;
            this.cmbHours.SelectedIndexChanged += new EventHandler(this.cmbHours_SelectedIndexChanged);
            this.lblUpdateHour.AutoSize = true;
            this.lblUpdateHour.Location = new Point(7, 0x3d);
            this.lblUpdateHour.Name = "lblUpdateHour";
            this.lblUpdateHour.Size = new Size(0x53, 13);
            this.lblUpdateHour.TabIndex = 1;
            this.lblUpdateHour.Text = "Hour of Update:";
            this.cbAutomatedUpdate.AutoSize = true;
            this.cbAutomatedUpdate.Location = new Point(7, 20);
            this.cbAutomatedUpdate.Name = "cbAutomatedUpdate";
            this.cbAutomatedUpdate.Size = new Size(0xcc, 0x11);
            this.cbAutomatedUpdate.TabIndex = 0;
            this.cbAutomatedUpdate.Text = "Schedule an Automated Daily Update";
            this.cbAutomatedUpdate.UseVisualStyleBackColor = true;
            this.cbAutomatedUpdate.CheckedChanged += new EventHandler(this.cbAutomatedUpdate_CheckedChanged);
            this.grpProviders.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.grpProviders.Controls.Add(this.lvProviders);
            this.grpProviders.Controls.Add(this.lblSelectProviders);
            this.grpProviders.Controls.Add(this.btnUpdateProviders);
            this.grpProviders.Location = new Point(9, 7);
            this.grpProviders.Name = "grpProviders";
            this.grpProviders.Size = new Size(280, 0x15c);
            this.grpProviders.TabIndex = 0;
            this.grpProviders.TabStop = false;
            this.grpProviders.Text = "Data Providers to Update";
            this.lvProviders.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvProviders.CheckBoxes = true;
            this.lvProviders.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0 });
            this.lvProviders.HeaderStyle = ColumnHeaderStyle.None;
            this.lvProviders.Location = new Point(10, 0x4f);
            this.lvProviders.MultiSelect = false;
            this.lvProviders.Name = "lvProviders";
            this.lvProviders.Size = new Size(0x108, 0xea);
            this.lvProviders.SmallImageList = this.imageList_0;
            this.lvProviders.TabIndex = 1;
            this.lvProviders.UseCompatibleStateImageBehavior = false;
            this.lvProviders.View = View.Details;
            this.lvProviders.Resize += new EventHandler(this.lvProviders_Resize);
            this.columnHeader_0.Width = 0xe1;
            this.lblSelectProviders.Location = new Point(7, 20);
            this.lblSelectProviders.Name = "lblSelectProviders";
            this.lblSelectProviders.Size = new Size(0x10b, 0x37);
            this.lblSelectProviders.TabIndex = 0;
            this.lblSelectProviders.Text = "Select the Data Providers that you want to include in the data updates below.  You can update data for the Providers now, or schedule an Automated Data Update.";
            this.btnUpdateProviders.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnUpdateProviders.Location = new Point(7, 0x13f);
            this.btnUpdateProviders.Name = "btnUpdateProviders";
            this.btnUpdateProviders.Size = new Size(0x10b, 0x17);
            this.btnUpdateProviders.TabIndex = 2;
            this.btnUpdateProviders.Text = "Update all data for selected Providers now";
            this.btnUpdateProviders.UseVisualStyleBackColor = true;
            this.btnUpdateProviders.Click += new EventHandler(this.btnUpdateProviders_Click);
            this.pageUpdateLog.BackColor = SystemColors.Control;
            this.pageUpdateLog.Controls.Add(this.txtUpdateLog);
            this.pageUpdateLog.Controls.Add(this.toolbarUpdate);
            this.pageUpdateLog.Location = new Point(4, 0x16);
            this.pageUpdateLog.Name = "pageUpdateLog";
            this.pageUpdateLog.Padding = new Padding(3);
            this.pageUpdateLog.Size = new Size(0x25e, 0x169);
            this.pageUpdateLog.TabIndex = 2;
            this.pageUpdateLog.Text = "Data Update Log";
            this.txtUpdateLog.Dock = DockStyle.Fill;
            this.txtUpdateLog.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtUpdateLog.Location = new Point(3, 0x1c);
            this.txtUpdateLog.Multiline = true;
            this.txtUpdateLog.Name = "txtUpdateLog";
            this.txtUpdateLog.ScrollBars = ScrollBars.Vertical;
            this.txtUpdateLog.Size = new Size(600, 330);
            this.txtUpdateLog.TabIndex = 2;
            this.toolbarUpdate.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbarUpdate.Items.AddRange(new ToolStripItem[] { this.lblUpdateProgress, this.progUpdate, this.btnCancelUpdate, this.sepUpdate, this.btnClearLog });
            this.toolbarUpdate.Location = new Point(3, 3);
            this.toolbarUpdate.Name = "toolbarUpdate";
            this.toolbarUpdate.Size = new Size(600, 0x19);
            this.toolbarUpdate.TabIndex = 1;
            this.toolbarUpdate.Text = "toolStrip1";
            this.lblUpdateProgress.Name = "lblUpdateProgress";
            this.lblUpdateProgress.Size = new Size(0x5b, 0x16);
            this.lblUpdateProgress.Text = "Update Progress:";
            this.progUpdate.Name = "progUpdate";
            this.progUpdate.Size = new Size(200, 0x16);
            this.btnCancelUpdate.Enabled = false;
            this.btnCancelUpdate.Image = (Image) manager.GetObject("btnCancelUpdate.Image");
            this.btnCancelUpdate.ImageTransparentColor = Color.White;
            this.btnCancelUpdate.Name = "btnCancelUpdate";
            this.btnCancelUpdate.Size = new Size(0x61, 0x16);
            this.btnCancelUpdate.Text = "Cancel Update";
            this.btnCancelUpdate.Click += new EventHandler(this.btnCancelUpdate_Click);
            this.sepUpdate.Name = "sepUpdate";
            this.sepUpdate.Size = new Size(6, 0x19);
            this.btnClearLog.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnClearLog.Image = (Image) manager.GetObject("btnClearLog.Image");
            this.btnClearLog.ImageTransparentColor = Color.Magenta;
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new Size(0x38, 0x16);
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.Click += new EventHandler(this.btnClearLog_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x266, 0x1b2);
            base.Controls.Add(this.tabDSM);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.statusStrip1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "DataManagerForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Data Manager";
            base.Load += new EventHandler(this.DataManagerForm_Load);
            base.FormClosed += new FormClosedEventHandler(this.DataManagerForm_FormClosed);
            base.FormClosing += new FormClosingEventHandler(this.DataManagerForm_FormClosing);
            this.popupSymDetails.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabDSM.ResumeLayout(false);
            this.pageDS.ResumeLayout(false);
            this.splitDataSets.Panel1.ResumeLayout(false);
            this.splitDataSets.Panel2.ResumeLayout(false);
            this.splitDataSets.ResumeLayout(false);
            this.grpDataSets.ResumeLayout(false);
            this.grpDataSets.PerformLayout();
            ((ISupportInitialize) this.picDataSet).EndInit();
            this.grpSymbolDetails.ResumeLayout(false);
            this.grpSymbols.ResumeLayout(false);
            this.grpSymbols.PerformLayout();
            this.tabUpdate.ResumeLayout(false);
            this.grpOnDemand.ResumeLayout(false);
            this.grpUpdateOptions.ResumeLayout(false);
            this.grpUpdateOptions.PerformLayout();
            this.grpScheduled.ResumeLayout(false);
            this.grpScheduled.PerformLayout();
            this.grpProviders.ResumeLayout(false);
            this.pageUpdateLog.ResumeLayout(false);
            this.pageUpdateLog.PerformLayout();
            this.toolbarUpdate.ResumeLayout(false);
            this.toolbarUpdate.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void ItemAdded(DataSource item)
        {
            this.lblDataSets.Text = this.lvDataSets.Items.Count + " DataSets";
        }

        public void ItemChanged(DataSource item)
        {
            if (item == this.dataSource_0)
            {
                this.method_1(this, new DataSourceEventArgs(this.dataSource_0));
            }
        }

        public void ItemRemoved(DataSource item)
        {
            if (item == this.dataSource_0)
            {
                this.btnCloseSymbolDetails.PerformClick();
                this.btnModifySymbols.Enabled = false;
                this.btnDetails.Enabled = false;
                this.btnUpdate.Enabled = false;
                this.txtSymbols.Clear();
                this.btnModifySymbols.Enabled = false;
                this.btnUpdatePricing.Enabled = false;
            }
            this.lblDataSets.Text = this.lvDataSets.Items.Count + " DataSets";
        }

        private void linkProvider_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MainModule.Instance.NavigateToThirdPartySite(this.dataSource_0.Provider.URL))
            {
                Process.Start(this.dataSource_0.Provider.URL);
            }
        }

        public void LoadWorkspaceItems(IList<string> items, int version)
        {
            this.tabDSM.SelectedIndex = int.Parse(items[0]);
        }

        private void lvProviders_Resize(object sender, EventArgs e)
        {
            this.columnHeader_0.Width = this.lvProviders.Width - 0x18;
        }

        private void lvSymbolDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.btnCancelSymbolDetails.Visible)
            {
                this.method_2(false);
            }
        }

        private void method_0()
        {
            SettingsManager settings = MainModule.Instance.Settings;
            settings.Set("ScheduledDataUpdates", this.cbAutomatedUpdate.Checked);
            settings.Set("ScheduledUpdateTime_Local", this.cmbHours.Text);
            settings.SaveSettings();
        }

        private void method_1(object sender, DataSourceEventArgs e)
        {
            this.dataSource_0 = e.DataSource;
            this.btnCloseSymbolDetails.PerformClick();
            this.btnDetails.Enabled = true;
            this.btnUpdate.Enabled = this.dataSource_0.Provider.SupportsDataSourceUpdate;
            this.btnUpdatePricing.Enabled = this.btnUpdate.Enabled;
            StringBuilder builder = new StringBuilder();
            foreach (string str in this.dataSource_0.Symbols)
            {
                builder.Append(str);
                builder.Append(" ");
            }
            this.txtSymbols.Text = builder.ToString();
            Bitmap glyph = this.dataSource_0.Provider.Glyph;
            glyph.MakeTransparent(Color.Fuchsia);
            this.picDataSet.Image = glyph;
            this.btnModifySymbols.Enabled = false;
            this.lblProviderName.Text = this.dataSource_0.Provider.FriendlyName;
            this.linkProvider.Visible = this.dataSource_0.Provider.URL != "";
            this.lblProviderDesc.Text = this.dataSource_0.Provider.Description;
        }

        private void method_2(bool bool_6)
        {
            this.btnCloseSymbolDetails.Visible = !bool_6;
            this.btnCancelSymbolDetails.Visible = bool_6;
            this.progSymbolDetails.Visible = bool_6;
            if ((this.dataSource_0 != null) && (this.dataSource_0.Provider != null))
            {
                this.mniReloadSymbolData.Enabled = (!bool_6 && this.dataSource_0.Provider.CanDeleteSymbolDataFile) && (this.lvSymbolDetails.SelectedItems.Count > 0);
                this.mniStockSplit.Enabled = (!bool_6 && this.dataSource_0.Provider.CanEditSymbolDataFile) && (this.lvSymbolDetails.SelectedItems.Count == 1);
            }
            else
            {
                this.mniReloadSymbolData.Enabled = false;
                this.mniStockSplit.Enabled = false;
            }
        }

        private void method_3(object sender, EventArgs e)
        {
            MainModule.Instance.CreateNewDataSource();
        }

        private void method_4(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.mniAddSymbols.Enabled = this.dataSource_0.Provider.CanModifySymbols;
            this.mniRemoveSymbols.Enabled = this.dataSource_0.Provider.CanModifySymbols && (this.lvSymbolDetails.SelectedItems.Count > 0);
        }

        private void method_5(object sender, EventArgs e)
        {
            base.Close();
        }

        private void method_6()
        {
            if (File.Exists(this.string_0))
            {
                DateTime lastWriteTime = File.GetLastWriteTime(this.string_0);
                this.statusLastUpdateLog.Text = "Last Update Log saved " + lastWriteTime.ToShortDateString() + " " + lastWriteTime.ToShortTimeString() + " (click here to view the log)";
                this.statusLastUpdateLog.IsLink = true;
            }
        }

        private void mniAddSymbols_Click(object sender, EventArgs e)
        {
            MainModule.Instance.DataSources.AddSymbols(this.dataSource_0);
        }

        private void mniReloadSymbolData_Click(object sender, EventArgs e)
        {
            if ((this.dataSource_0 != null) && (this.dataSource_0.Provider != null))
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    foreach (ListViewItem item in this.lvSymbolDetails.Items)
                    {
                        if (item.Selected)
                        {
                            string text = item.Text;
                            this.dataSource_0.Provider.DeleteSymbolDataFile(this.dataSource_0, text);
                            Bars bars = this.dataSource_0.Provider.RequestData(this.dataSource_0, text, DateTime.MinValue, DateTime.MaxValue, 0, false);
                            item.SubItems[2].Text = bars.Count.ToString("N0");
                            if (bars.Count > 0)
                            {
                                DateTime time = bars.Date[bars.Count - 1];
                                string str = time.ToShortDateString();
                                if (bars.IsIntraday)
                                {
                                    str = str + " " + time.ToShortTimeString();
                                }
                                item.SubItems[3].Text = str;
                            }
                            else
                            {
                                item.SubItems[3].Text = "";
                            }
                        }
                    }
                    MessageBox.Show(this.lvSymbolDetails.SelectedItems.Count + " Symbols Reloaded");
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void mniRemoveSymbols_Click(object sender, EventArgs e)
        {
            if (this.lvSymbolDetails.SelectedItems.Count > 0)
            {
                List<string> symbols = new List<string>();
                foreach (ListViewItem item in this.lvSymbolDetails.SelectedItems)
                {
                    symbols.Add(item.Text);
                }
                MainModule.Instance.DataSources.RemoveSymbols(this.dataSource_0, symbols);
            }
        }

        private void mniStockSplit_Click(object sender, EventArgs e)
        {
            if (((this.dataSource_0 != null) && (this.dataSource_0.Provider != null)) && (this.lvSymbolDetails.SelectedItems.Count == 1))
            {
                string text = this.lvSymbolDetails.SelectedItems[0].Text;
                StockSplitForm form = new StockSplitForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    MainModule.Instance.DataSources.ProcessStockSplit(this.dataSource_0, text, form.SplitDate, form.Factor);
                    MessageBox.Show("Stock Split processed");
                }
            }
        }

        private void rbUpdateDataSets_CheckedChanged(object sender, EventArgs e)
        {
            this.cbCleanup.Enabled = this.rbUpdateDataSets.Checked;
            if (!this.cbCleanup.Enabled)
            {
                this.cbCleanup.Checked = false;
            }
        }

        public void ReportUpdateProgress(int progressPercent)
        {
            this.backgroundWorker_0.ReportProgress(progressPercent);
            Application.DoEvents();
        }

        public int SaveWorkspaceItems(IList<string> items)
        {
            items.Add(this.tabDSM.SelectedIndex.ToString());
            return 1;
        }

        public void SetFirstTab()
        {
            this.tabDSM.SelectedTab = this.pageDS;
        }

        public void SetOnDemand(bool onDemandOn)
        {
            this.cbOnDemand.Checked = onDemandOn;
        }

        private void statusLastUpdateLog_Click(object sender, EventArgs e)
        {
            if (File.Exists(this.string_0))
            {
                Process.Start(this.string_0);
            }
        }

        private void tabDSM_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool flag = false;
            if (this.tabDSM.SelectedIndex == 0)
            {
                flag = this.lvDataSets.SelectedItems.Count == 1;
            }
            this.btnUpdate.Enabled = flag;
            if (flag)
            {
                DataSource tag = (DataSource) this.lvDataSets.SelectedItems[0].Tag;
                this.btnUpdate.Enabled = tag.Provider.SupportsDataSourceUpdate;
            }
            this.btnUpdatePricing.Enabled = this.btnUpdate.Enabled;
            this.btnDetails.Enabled = flag;
        }

        private void tabDSM_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (this.btnCancelUpdate.Enabled)
            {
                e.Cancel = true;
            }
        }

        private void txtSymbols_TextChanged(object sender, EventArgs e)
        {
            if ((this.dataSource_0 != null) && !this.dataSource_0.IsIndexLabDataset)
            {
                this.btnModifySymbols.Enabled = this.dataSource_0.Provider.CanModifySymbols;
            }
        }

        public void UpdateProviders()
        {
            this.list_1.Clear();
            this.progUpdate.Value = 0;
            this.bool_1 = false;
            this.tabDSM.SelectedTab = this.pageUpdateLog;
            this.btnCancelUpdate.Enabled = true;
            this.list_0.Clear();
            foreach (ListViewItem item in this.lvProviders.Items)
            {
                if (item.Checked)
                {
                    this.list_0.Add(item.Tag as HistoricalProvider);
                }
            }
            this.Cursor = Cursors.WaitCursor;
            this.txtUpdateLog.Cursor = Cursors.WaitCursor;
            this.bool_3 = false;
            this.backgroundWorker_0 = this.backgroundWorker_2;
            this.backgroundWorker_2.RunWorkerAsync();
        }
    }
}

