namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;

    public class HomeForm : Form, IWorkspace
    {
        private static bool bool_0;
        private bool bool_1;
        private WebBrowser browserWhatsNew;
        private CheckBox cbDontShow;
        private ColumnHeader columnHeader_0;
        private IContainer components;
        private ImageList imageList_0;
        public static HomeForm Instance;
        private Label label1;
        private Label lblFooter1;
        private Label lblFooter2;
        private Label lblFooter3;
        private Label lblFooter4;
        private Label lblFooter5;
        private Label lblHeader1;
        private Label lblHeader2;
        private Label lblHeader3;
        private Label lblHeader4;
        private Label lblHeader5;
        private Label lblTitle;
        private Label lblWhatsNew;
        private LinkLabel linkBack;
        private ListView lvRecent;
        private PictureBox picFlow1;
        private PictureBox picFlow2;
        private PictureBox picFlow3;
        private PictureBox picFlow4;
        private PictureBox picFlow5;
        private Panel pnlBanner;
        private string string_0 = (MainModule.Instance.DataPath + @"\WhatsNew.htm");

        public HomeForm()
        {
            this.InitializeComponent();
        }

        private void browserWhatsNew_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            this.linkBack.Visible = true;
        }

        private void browserWhatsNew_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string iD = e.Url.ToString();
            if (iD.StartsWith("strategy://"))
            {
                e.Cancel = true;
                iD = iD.Substring(11);
                iD = iD.Substring(0, iD.Length - 1);
                Strategy strategy = MainModule.Instance.Strategies.LookupID(iD);
                if (strategy != null)
                {
                    this.MyMainForm.OpenStrategyWindow(strategy);
                }
                else
                {
                    MessageBox.Show("The Strategy you selected is not installed.");
                }
            }
            else if (iD.StartsWith("http://"))
            {
                e.Cancel = true;
                if (MainModule.Instance.NavigateToThirdPartySite(iD))
                {
                    Process.Start(iD);
                }
            }
        }

        private void cbDontShow_CheckedChanged(object sender, EventArgs e)
        {
            MainModule.Instance.Settings.Set("ShowHomePage", !this.cbDontShow.Checked);
            MainModule.Instance.SaveSettings();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void HomeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Instance = null;
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            Instance = this;
            this.cbDontShow.Checked = !MainModule.Instance.Settings.Get("ShowHomePage", true);
            this.lblTitle.Text = MainModule.Instance.AuthProvider.ApplicationName;
            if (System.IO.File.Exists(this.string_0))
            {
                this.browserWhatsNew.Navigate(this.string_0);
            }
            else
            {
                this.browserWhatsNew.Navigate(Path.GetDirectoryName(Application.ExecutablePath) + @"\WhatsNew.htm");
            }
            if (!bool_0)
            {
                bool_0 = true;
                new Thread(new ThreadStart(this.method_0)) { IsBackground = true }.Start();
            }
            this.LoadStrategyMRU();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(HomeForm));
            this.pnlBanner = new Panel();
            this.lblFooter5 = new Label();
            this.lblHeader5 = new Label();
            this.picFlow5 = new PictureBox();
            this.lblFooter4 = new Label();
            this.lblHeader4 = new Label();
            this.picFlow4 = new PictureBox();
            this.lblFooter3 = new Label();
            this.lblHeader3 = new Label();
            this.picFlow3 = new PictureBox();
            this.lblFooter2 = new Label();
            this.lblHeader2 = new Label();
            this.picFlow2 = new PictureBox();
            this.lblFooter1 = new Label();
            this.lblHeader1 = new Label();
            this.picFlow1 = new PictureBox();
            this.lblTitle = new Label();
            this.label1 = new Label();
            this.lvRecent = new ListView();
            this.columnHeader_0 = new ColumnHeader();
            this.imageList_0 = new ImageList(this.components);
            this.lblWhatsNew = new Label();
            this.browserWhatsNew = new WebBrowser();
            this.linkBack = new LinkLabel();
            this.cbDontShow = new CheckBox();
            this.pnlBanner.SuspendLayout();
            ((ISupportInitialize) this.picFlow5).BeginInit();
            ((ISupportInitialize) this.picFlow4).BeginInit();
            ((ISupportInitialize) this.picFlow3).BeginInit();
            ((ISupportInitialize) this.picFlow2).BeginInit();
            ((ISupportInitialize) this.picFlow1).BeginInit();
            base.SuspendLayout();
            this.pnlBanner.BackColor = Color.FromArgb(0xf7, 0xff, 0xf7);
            this.pnlBanner.Controls.Add(this.lblFooter5);
            this.pnlBanner.Controls.Add(this.lblHeader5);
            this.pnlBanner.Controls.Add(this.picFlow5);
            this.pnlBanner.Controls.Add(this.lblFooter4);
            this.pnlBanner.Controls.Add(this.lblHeader4);
            this.pnlBanner.Controls.Add(this.picFlow4);
            this.pnlBanner.Controls.Add(this.lblFooter3);
            this.pnlBanner.Controls.Add(this.lblHeader3);
            this.pnlBanner.Controls.Add(this.picFlow3);
            this.pnlBanner.Controls.Add(this.lblFooter2);
            this.pnlBanner.Controls.Add(this.lblHeader2);
            this.pnlBanner.Controls.Add(this.picFlow2);
            this.pnlBanner.Controls.Add(this.lblFooter1);
            this.pnlBanner.Controls.Add(this.lblHeader1);
            this.pnlBanner.Controls.Add(this.picFlow1);
            this.pnlBanner.Controls.Add(this.lblTitle);
            this.pnlBanner.Dock = DockStyle.Top;
            this.pnlBanner.Location = new Point(0, 0);
            this.pnlBanner.Name = "pnlBanner";
            this.pnlBanner.Size = new Size(0x2e0, 0xbd);
            this.pnlBanner.TabIndex = 0;
            this.lblFooter5.ForeColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.lblFooter5.Location = new Point(0x270, 0x8f);
            this.lblFooter5.Name = "lblFooter5";
            this.lblFooter5.Size = new Size(100, 0x2c);
            this.lblFooter5.TabIndex = 0x15;
            this.lblFooter5.Text = "View, Place or Cancel your Trade Orders";
            this.lblHeader5.AutoSize = true;
            this.lblHeader5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblHeader5.Location = new Point(0x26d, 0x20);
            this.lblHeader5.Name = "lblHeader5";
            this.lblHeader5.Size = new Size(0x5d, 13);
            this.lblHeader5.TabIndex = 20;
            this.lblHeader5.Text = "Manage Orders";
            this.picFlow5.Cursor = Cursors.Hand;
            this.picFlow5.Image = (Image) resources.GetObject("picFlow5.Image");
            this.picFlow5.Location = new Point(0x270, 0x30);
            this.picFlow5.Name = "picFlow5";
            this.picFlow5.Size = new Size(0x5c, 0x5c);
            this.picFlow5.SizeMode = PictureBoxSizeMode.AutoSize;
            this.picFlow5.TabIndex = 0x13;
            this.picFlow5.TabStop = false;
            this.picFlow5.Click += new EventHandler(this.picFlow5_Click);
            this.lblFooter4.ForeColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.lblFooter4.Location = new Point(0x1d5, 0x8f);
            this.lblFooter4.Name = "lblFooter4";
            this.lblFooter4.Size = new Size(0x73, 0x2c);
            this.lblFooter4.TabIndex = 0x11;
            this.lblFooter4.Text = "Activate Strategies in the Strategy Monitor to get Trade Alerts";
            this.lblHeader4.AutoSize = true;
            this.lblHeader4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblHeader4.Location = new Point(0x1d5, 0x20);
            this.lblHeader4.Name = "lblHeader4";
            this.lblHeader4.Size = new Size(0x73, 13);
            this.lblHeader4.TabIndex = 0x10;
            this.lblHeader4.Text = "Activate Strategies";
            this.picFlow4.Cursor = Cursors.Hand;
            this.picFlow4.Image = (Image) resources.GetObject("picFlow4.Image");
            this.picFlow4.Location = new Point(0x1d8, 0x30);
            this.picFlow4.Name = "picFlow4";
            this.picFlow4.Size = new Size(0x5c, 0x5c);
            this.picFlow4.SizeMode = PictureBoxSizeMode.AutoSize;
            this.picFlow4.TabIndex = 15;
            this.picFlow4.TabStop = false;
            this.picFlow4.Click += new EventHandler(this.picFlow4_Click);
            this.lblFooter3.ForeColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.lblFooter3.Location = new Point(0x13d, 0x8f);
            this.lblFooter3.Name = "lblFooter3";
            this.lblFooter3.Size = new Size(0x6a, 0x2c);
            this.lblFooter3.TabIndex = 13;
            this.lblFooter3.Text = "Use Drag && Drop to Create and Backtest your Strategies";
            this.lblHeader3.AutoSize = true;
            this.lblHeader3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblHeader3.Location = new Point(0x13d, 0x20);
            this.lblHeader3.Name = "lblHeader3";
            this.lblHeader3.Size = new Size(100, 13);
            this.lblHeader3.TabIndex = 12;
            this.lblHeader3.Text = "Build && Backtest";
            this.picFlow3.Cursor = Cursors.Hand;
            this.picFlow3.Image = (Image) resources.GetObject("picFlow3.Image");
            this.picFlow3.Location = new Point(320, 0x30);
            this.picFlow3.Name = "picFlow3";
            this.picFlow3.Size = new Size(0x5c, 0x5c);
            this.picFlow3.SizeMode = PictureBoxSizeMode.AutoSize;
            this.picFlow3.TabIndex = 11;
            this.picFlow3.TabStop = false;
            this.picFlow3.Click += new EventHandler(this.picFlow3_Click);
            this.lblFooter2.ForeColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.lblFooter2.Location = new Point(0xa5, 0x8f);
            this.lblFooter2.Name = "lblFooter2";
            this.lblFooter2.Size = new Size(100, 0x2c);
            this.lblFooter2.TabIndex = 9;
            this.lblFooter2.Text = "Explore pre-made Strategies to use out-of-the-box";
            this.lblHeader2.AutoSize = true;
            this.lblHeader2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblHeader2.Location = new Point(0xa5, 0x20);
            this.lblHeader2.Name = "lblHeader2";
            this.lblHeader2.Size = new Size(0x72, 13);
            this.lblHeader2.TabIndex = 8;
            this.lblHeader2.Text = "Explore && Backtest";
            this.picFlow2.Cursor = Cursors.Hand;
            this.picFlow2.Image = (Image) resources.GetObject("picFlow2.Image");
            this.picFlow2.Location = new Point(0xa8, 0x30);
            this.picFlow2.Name = "picFlow2";
            this.picFlow2.Size = new Size(0x5c, 0x5c);
            this.picFlow2.SizeMode = PictureBoxSizeMode.AutoSize;
            this.picFlow2.TabIndex = 7;
            this.picFlow2.TabStop = false;
            this.picFlow2.Click += new EventHandler(this.picFlow2_Click);
            this.lblFooter1.ForeColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.lblFooter1.Location = new Point(12, 0x8f);
            this.lblFooter1.Name = "lblFooter1";
            this.lblFooter1.Size = new Size(0x80, 0x2c);
            this.lblFooter1.TabIndex = 5;
            this.lblFooter1.Text = "Create a new Chart with Technical and Fundamental Indicators";
            this.lblHeader1.AutoSize = true;
            this.lblHeader1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblHeader1.Location = new Point(9, 0x20);
            this.lblHeader1.Name = "lblHeader1";
            this.lblHeader1.Size = new Size(0x42, 13);
            this.lblHeader1.TabIndex = 4;
            this.lblHeader1.Text = "New Chart";
            this.picFlow1.Cursor = Cursors.Hand;
            this.picFlow1.Image = (Image) resources.GetObject("picFlow1.Image");
            this.picFlow1.Location = new Point(12, 0x30);
            this.picFlow1.Name = "picFlow1";
            this.picFlow1.Size = new Size(0x5c, 0x5c);
            this.picFlow1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.picFlow1.TabIndex = 3;
            this.picFlow1.TabStop = false;
            this.picFlow1.Click += new EventHandler(this.picFlow1_Click);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblTitle.ForeColor = Color.Green;
            this.lblTitle.Location = new Point(9, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(0x5c, 15);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "WEALTH-LAB";
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(9, 0xc0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x89, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Recently Viewed Strategies";
            this.lvRecent.Activation = ItemActivation.OneClick;
            this.lvRecent.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvRecent.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0 });
            this.lvRecent.Cursor = Cursors.Hand;
            this.lvRecent.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Underline, GraphicsUnit.Point, 0);
            this.lvRecent.ForeColor = Color.Blue;
            this.lvRecent.FullRowSelect = true;
            this.lvRecent.HeaderStyle = ColumnHeaderStyle.None;
            this.lvRecent.HideSelection = false;
            this.lvRecent.Location = new Point(12, 0xd0);
            this.lvRecent.MultiSelect = false;
            this.lvRecent.Name = "lvRecent";
            this.lvRecent.Size = new Size(0xd0, 0xc3);
            this.lvRecent.SmallImageList = this.imageList_0;
            this.lvRecent.TabIndex = 3;
            this.lvRecent.UseCompatibleStateImageBehavior = false;
            this.lvRecent.View = View.Details;
            this.lvRecent.Click += new EventHandler(this.lvRecent_Click);
            this.columnHeader_0.Text = "Recently Viewed Strategies";
            this.columnHeader_0.Width = 200;
            this.imageList_0.ImageStream = (ImageListStreamer) resources.GetObject("images.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "precompiled.bmp");
            this.imageList_0.Images.SetKeyName(1, "Wizard.bmp");
            this.imageList_0.Images.SetKeyName(2, "Editor.bmp");
            this.lblWhatsNew.AutoSize = true;
            this.lblWhatsNew.Location = new Point(0xea, 0xc0);
            this.lblWhatsNew.Name = "lblWhatsNew";
            this.lblWhatsNew.Size = new Size(0x47, 13);
            this.lblWhatsNew.TabIndex = 4;
            this.lblWhatsNew.Text = "What's New?";
            this.browserWhatsNew.AllowWebBrowserDrop = false;
            this.browserWhatsNew.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.browserWhatsNew.Location = new Point(0xed, 0xd0);
            this.browserWhatsNew.MinimumSize = new Size(20, 20);
            this.browserWhatsNew.Name = "browserWhatsNew";
            this.browserWhatsNew.Size = new Size(0x1e7, 0xda);
            this.browserWhatsNew.TabIndex = 5;
            this.browserWhatsNew.Navigated += new WebBrowserNavigatedEventHandler(this.browserWhatsNew_Navigated);
            this.browserWhatsNew.Navigating += new WebBrowserNavigatingEventHandler(this.browserWhatsNew_Navigating);
            this.linkBack.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.linkBack.AutoSize = true;
            this.linkBack.Location = new Point(0x2a3, 0xc0);
            this.linkBack.Name = "linkBack";
            this.linkBack.Size = new Size(0x31, 13);
            this.linkBack.TabIndex = 6;
            this.linkBack.TabStop = true;
            this.linkBack.Text = "Go Back";
            this.linkBack.Visible = false;
            this.linkBack.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkBack_LinkClicked);
            this.cbDontShow.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbDontShow.AutoSize = true;
            this.cbDontShow.Location = new Point(12, 0x199);
            this.cbDontShow.Name = "cbDontShow";
            this.cbDontShow.Size = new Size(190, 0x11);
            this.cbDontShow.TabIndex = 7;
            this.cbDontShow.Text = "Don't show Home Page on Startup";
            this.cbDontShow.UseVisualStyleBackColor = true;
            this.cbDontShow.CheckedChanged += new EventHandler(this.cbDontShow_CheckedChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x2e0, 0x1b6);
            base.Controls.Add(this.cbDontShow);
            base.Controls.Add(this.linkBack);
            base.Controls.Add(this.browserWhatsNew);
            base.Controls.Add(this.lblWhatsNew);
            base.Controls.Add(this.lvRecent);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.pnlBanner);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "HomeForm";
            this.Text = "Home";
            base.FormClosed += new FormClosedEventHandler(this.HomeForm_FormClosed);
            base.Load += new EventHandler(this.HomeForm_Load);
            this.pnlBanner.ResumeLayout(false);
            this.pnlBanner.PerformLayout();
            ((ISupportInitialize) this.picFlow5).EndInit();
            ((ISupportInitialize) this.picFlow4).EndInit();
            ((ISupportInitialize) this.picFlow3).EndInit();
            ((ISupportInitialize) this.picFlow2).EndInit();
            ((ISupportInitialize) this.picFlow1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void linkBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.browserWhatsNew.GoBack();
        }

        public void LoadStrategyMRU()
        {
            this.lvRecent.Items.Clear();
            foreach (Strategy strategy in MainModule.Instance.StrategyMRU)
            {
                ListViewItem item = this.lvRecent.Items.Add(strategy.Name);
                item.Tag = strategy;
                item.ImageIndex = 0;
            }
        }

        public void LoadWorkspaceItems(IList<string> items, int version)
        {
        }

        private void lvRecent_Click(object sender, EventArgs e)
        {
            if (!this.bool_1 && (this.lvRecent.SelectedItems.Count == 1))
            {
                this.bool_1 = true;
                try
                {
                    Strategy tag = (Strategy) this.lvRecent.SelectedItems[0].Tag;
                    this.MyMainForm.OpenStrategyWindow(tag);
                }
                finally
                {
                    this.bool_1 = false;
                }
            }
        }

        private void method_0()
        {
            string whatsNewLink = MainModule.Instance.AuthProvider.WhatsNewLink;
            if (whatsNewLink.ToUpper().StartsWith("FILE://"))
            {
                this.string_0 = whatsNewLink;
                base.Invoke(new Delegate64(this.method_1));
            }
            else
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(whatsNewLink);
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    string contents = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    FileNameValidator.ValidateFileName(this.string_0);
                    System.IO.File.WriteAllText(this.string_0, contents);
                    response.Close();
                    base.Invoke(new Delegate64(this.method_1));
                }
                catch
                {
                }
            }
        }

        private void method_1()
        {
            this.browserWhatsNew.Navigate(this.string_0);
        }

        private void picFlow1_Click(object sender, EventArgs e)
        {
            this.MyMainForm.CreateChartWindow(true);
        }

        private void picFlow2_Click(object sender, EventArgs e)
        {
            this.MyMainForm.OpenStrategyExplorer();
            this.MyMainForm.SelectNode();
        }

        private void picFlow3_Click(object sender, EventArgs e)
        {
            this.MyMainForm.CreateNewStrategyWindow(false).Show();
        }

        private void picFlow4_Click(object sender, EventArgs e)
        {
            this.MyMainForm.OpenStrategyCenter();
        }

        private void picFlow5_Click(object sender, EventArgs e)
        {
            this.MyMainForm.OpenOrderManager();
        }

        public int SaveWorkspaceItems(IList<string> items)
        {
            return 1;
        }

        public void SetShowOnStartup(bool bool_2)
        {
            this.cbDontShow.Checked = !bool_2;
        }

        public MainForm MyMainForm
        {
            get
            {
                return (base.MdiParent as MainForm);
            }
        }

        private delegate void Delegate64();
    }
}

