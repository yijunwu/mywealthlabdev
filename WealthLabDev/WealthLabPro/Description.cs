namespace WealthLabPro
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxItem(false)]
    public class Description : UserControl
    {
        private bool bool_0 = true;
        private WebBrowser browserDesc;
        private IContainer components;
        private ImageList imageList_0;
        private const int int_0 = 0;
        private const int int_1 = 1;
        private const int int_2 = 2;
        private int int_3;
        private int int_4;
        private int int_5;
        private Label lblActivation;
        private Label lblAlerts;
        private Label lblAuthor;
        private Label lblAuthorValue;
        private Label lblBH;
        private Label lblCreated;
        private Label lblCreatedValue;
        private Label lblData;
        private Label lblDataValue;
        private Label lblDescription;
        private Label lblGenerating;
        private Label lblMode;
        private Label lblModeValue;
        private Label lblModified;
        private Label lblModifiedValue;
        private Label lblName;
        private Label lblNetProfit;
        private Label lblParams;
        private Label lblParamsValue;
        private Label lblPerformance;
        private Label lblPosSize;
        private Label lblPosSizeValue;
        private Label lblRange;
        private Label lblRangeTested;
        private Label lblRangeTestedValue;
        private Label lblRangeValue;
        private Label lblScale;
        private Label lblScaleValue;
        private Label lblStartCapValue;
        private Label lblStartingCap;
        private Label lblStrategiesIncluded;
        private Label lblStrategyActive;
        private Label lblStrategyMonitor;
        private Label lblStrategyName;
        private LinkLabel linkActivate;
        private LinkLabel linkAlerts;
        private LinkLabel linkBuyHold;
        private LinkLabel linkEdit;
        private LinkLabel linkGoBack;
        private LinkLabel linkMoreInfo;
        private LinkLabel linkNetProfit;
        private ToolStripMenuItem mniCopy;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrintAll;
        private PictureBox picActive;
        private PictureBox picGenerating;
        private ContextMenuStrip popup;
        private PrintPreview printPreview_0;
        private PrintReport printReport_0 = new PrintReport();
        private RichTextBox richTextBox1;
        private WealthLab.Strategy strategy;

        public Description()
        {
            this.InitializeComponent();
        }

        private void browserDesc_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string str = e.Url.ToString().ToUpper();
            if (str.StartsWith("HTTP"))
            {
                if (this.bool_0)
                {
                    e.Cancel = !MainModule.Instance.NavigateToThirdPartySite(str);
                }
                if (!e.Cancel)
                {
                    this.bool_0 = false;
                }
            }
            else
            {
                this.bool_0 = true;
            }
        }

        public void CopyToClipboard()
        {
            MainModule.Instance.CopyListViewToClipboard(this.method_4());
        }

        public void descriptionReport_BeginPrint(object sender, PrintEventArgs e)
        {
            if ((this.printPreview_0 != null) && this.printPreview_0.PrintSomePages)
            {
                this.int_3 = this.printPreview_0.FromPage;
                this.int_4 = this.printPreview_0.ToPage;
            }
            this.printReport_0.intPageCounter = 1;
            this.printReport_0.BasePrintTitle = MainModule.Instance.AuthProvider.ApplicationName;
            this.printReport_0.printTitle = "Strategy Summary";
            this.printReport_0.UseDefaultDisclosure();
            this.printReport_0.printStrategy = this.lblStrategyName.Text;
        }

        public void descriptionReport_EndPrint(object sender, PrintEventArgs e)
        {
            if (this.printPreview_0 != null)
            {
                this.printPreview_0.FromPage = 1;
                this.printPreview_0.ToPage = this.int_4 = --this.printReport_0.intPageCounter;
            }
        }

        public void descriptionReport_PrintPage(object sender, PrintPageEventArgs e)
        {
            bool flag = true;
            Rectangle destRect = new Rectangle(e.MarginBounds.X, e.MarginBounds.Y, e.MarginBounds.Width, e.MarginBounds.Height);
            this.int_5 = ((int) e.Graphics.MeasureString("Test", this.printReport_0.printFontBody).Height) + 1;
            if (this.printReport_0.printSomePages && (this.printReport_0.intPageCounter < this.int_3))
            {
                flag = false;
            }
            else
            {
                this.printReport_0.PrintTitle(e, ref destRect);
                this.printReport_0.PrintHeader(e, ref destRect);
            }
            if (this.printReport_0.intPageCounter == 1)
            {
                this.method_3(e, ref destRect, flag);
            }
            SizeF disclosureRect = this.printReport_0.GetDisclosureRect(e, ref destRect);
            if (this.printReport_0.fPrintText)
            {
                e.Graphics.DrawString(this.lblDescription.Text, this.lblDescription.Font, Brushes.Black, (float) destRect.X, (float) destRect.Y);
                destRect.Y += this.int_5;
                destRect.Height -= this.int_5;
                this.printReport_0.PrintText(e, ref destRect, flag);
            }
            this.printReport_0.PrintFooter(e, flag, ref destRect, disclosureRect);
            e.HasMorePages = this.printReport_0.fPrintText;
            if ((++this.printReport_0.intPageCounter > this.int_4) && this.printReport_0.printSomePages)
            {
                e.HasMorePages = false;
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

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Description));
            this.browserDesc = new WebBrowser();
            this.lblName = new Label();
            this.lblStrategyName = new Label();
            this.lblAuthor = new Label();
            this.lblCreated = new Label();
            this.lblModified = new Label();
            this.lblAuthorValue = new Label();
            this.lblCreatedValue = new Label();
            this.lblModifiedValue = new Label();
            this.lblData = new Label();
            this.lblScale = new Label();
            this.lblRange = new Label();
            this.lblParams = new Label();
            this.lblDescription = new Label();
            this.linkGoBack = new LinkLabel();
            this.linkEdit = new LinkLabel();
            this.linkMoreInfo = new LinkLabel();
            this.lblDataValue = new Label();
            this.lblScaleValue = new Label();
            this.lblRangeValue = new Label();
            this.lblParamsValue = new Label();
            this.lblPerformance = new Label();
            this.lblMode = new Label();
            this.lblStartingCap = new Label();
            this.lblPosSize = new Label();
            this.lblNetProfit = new Label();
            this.lblBH = new Label();
            this.lblModeValue = new Label();
            this.lblStartCapValue = new Label();
            this.lblPosSizeValue = new Label();
            this.linkNetProfit = new LinkLabel();
            this.linkBuyHold = new LinkLabel();
            this.lblRangeTested = new Label();
            this.lblRangeTestedValue = new Label();
            this.lblActivation = new Label();
            this.picActive = new PictureBox();
            this.imageList_0 = new ImageList(this.components);
            this.lblStrategyActive = new Label();
            this.lblAlerts = new Label();
            this.linkAlerts = new LinkLabel();
            this.picGenerating = new PictureBox();
            this.lblGenerating = new Label();
            this.linkActivate = new LinkLabel();
            this.lblStrategyMonitor = new Label();
            this.popup = new ContextMenuStrip(this.components);
            this.mniCopy = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.mniPrintAll = new ToolStripMenuItem();
            this.lblStrategiesIncluded = new Label();
            this.richTextBox1 = new RichTextBox();
            ((ISupportInitialize) this.picActive).BeginInit();
            ((ISupportInitialize) this.picGenerating).BeginInit();
            this.popup.SuspendLayout();
            base.SuspendLayout();
            this.browserDesc.AllowWebBrowserDrop = false;
            this.browserDesc.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.browserDesc.Location = new Point(14, 0xc3);
            this.browserDesc.MinimumSize = new Size(20, 20);
            this.browserDesc.Name = "browserDesc";
            this.browserDesc.Size = new Size(0x284, 0xec);
            this.browserDesc.TabIndex = 2;
            this.browserDesc.Navigating += new WebBrowserNavigatingEventHandler(this.browserDesc_Navigating);
            this.lblName.AutoSize = true;
            this.lblName.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblName.Location = new Point(4, 4);
            this.lblName.Name = "lblName";
            this.lblName.Size = new Size(0x49, 20);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Strategy:";
            this.lblStrategyName.AutoSize = true;
            this.lblStrategyName.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblStrategyName.ForeColor = SystemColors.ActiveCaption;
            this.lblStrategyName.Location = new Point(0x4a, 4);
            this.lblStrategyName.Name = "lblStrategyName";
            this.lblStrategyName.Size = new Size(0xe0, 20);
            this.lblStrategyName.TabIndex = 4;
            this.lblStrategyName.Text = "CMO Signals with Profit Target";
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Location = new Point(6, 0x20);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new Size(0x29, 13);
            this.lblAuthor.TabIndex = 5;
            this.lblAuthor.Text = "Author:";
            this.lblCreated.AutoSize = true;
            this.lblCreated.Location = new Point(6, 0x31);
            this.lblCreated.Name = "lblCreated";
            this.lblCreated.Size = new Size(0x2f, 13);
            this.lblCreated.TabIndex = 6;
            this.lblCreated.Text = "Created:";
            this.lblModified.AutoSize = true;
            this.lblModified.Location = new Point(6, 0x42);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new Size(0x49, 13);
            this.lblModified.TabIndex = 7;
            this.lblModified.Text = "Last Modified:";
            this.lblAuthorValue.AutoSize = true;
            this.lblAuthorValue.ForeColor = SystemColors.ControlText;
            this.lblAuthorValue.Location = new Point(0x4d, 0x20);
            this.lblAuthorValue.Name = "lblAuthorValue";
            this.lblAuthorValue.Size = new Size(0x21, 13);
            this.lblAuthorValue.TabIndex = 8;
            this.lblAuthorValue.Text = "Local";
            this.lblCreatedValue.AutoSize = true;
            this.lblCreatedValue.ForeColor = SystemColors.ControlText;
            this.lblCreatedValue.Location = new Point(0x4d, 0x31);
            this.lblCreatedValue.Name = "lblCreatedValue";
            this.lblCreatedValue.Size = new Size(0x35, 13);
            this.lblCreatedValue.TabIndex = 9;
            this.lblCreatedValue.Text = "1/1/2007";
            this.lblModifiedValue.AutoSize = true;
            this.lblModifiedValue.ForeColor = SystemColors.ControlText;
            this.lblModifiedValue.Location = new Point(0x4d, 0x42);
            this.lblModifiedValue.Name = "lblModifiedValue";
            this.lblModifiedValue.Size = new Size(0x3b, 13);
            this.lblModifiedValue.TabIndex = 10;
            this.lblModifiedValue.Text = "3/15/2007";
            this.lblData.AutoSize = true;
            this.lblData.Location = new Point(6, 0x86);
            this.lblData.Name = "lblData";
            this.lblData.Size = new Size(0x47, 13);
            this.lblData.TabIndex = 11;
            this.lblData.Text = "Data Applied:";
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new Point(6, 0x53);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new Size(0x38, 13);
            this.lblScale.TabIndex = 12;
            this.lblScale.Text = "Bar Scale:";
            this.lblRange.AutoSize = true;
            this.lblRange.Location = new Point(7, 100);
            this.lblRange.Name = "lblRange";
            this.lblRange.Size = new Size(0x44, 13);
            this.lblRange.TabIndex = 13;
            this.lblRange.Text = "Data Range:";
            this.lblParams.AutoSize = true;
            this.lblParams.Location = new Point(7, 0x75);
            this.lblParams.Name = "lblParams";
            this.lblParams.Size = new Size(0x3f, 13);
            this.lblParams.TabIndex = 14;
            this.lblParams.Text = "Parameters:";
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblDescription.Location = new Point(12, 0xb3);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new Size(0x7a, 13);
            this.lblDescription.TabIndex = 15;
            this.lblDescription.Text = "Strategy Description";
            this.linkGoBack.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.linkGoBack.AutoSize = true;
            this.linkGoBack.LinkColor = SystemColors.ActiveCaption;
            this.linkGoBack.Location = new Point(0x260, 0xb1);
            this.linkGoBack.Name = "linkGoBack";
            this.linkGoBack.Size = new Size(0x31, 13);
            this.linkGoBack.TabIndex = 0x10;
            this.linkGoBack.TabStop = true;
            this.linkGoBack.Text = "Go Back";
            this.linkGoBack.Visible = false;
            this.linkGoBack.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkGoBack_LinkClicked);
            this.linkEdit.AutoSize = true;
            this.linkEdit.LinkColor = SystemColors.ActiveCaption;
            this.linkEdit.Location = new Point(140, 0xb3);
            this.linkEdit.Name = "linkEdit";
            this.linkEdit.Size = new Size(0x84, 13);
            this.linkEdit.TabIndex = 0x11;
            this.linkEdit.TabStop = true;
            this.linkEdit.Text = "Edit Description (HTML) ...";
            this.linkEdit.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkEdit_LinkClicked);
            this.linkMoreInfo.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.linkMoreInfo.AutoSize = true;
            this.linkMoreInfo.LinkColor = SystemColors.ActiveCaption;
            this.linkMoreInfo.Location = new Point(0x207, 9);
            this.linkMoreInfo.Name = "linkMoreInfo";
            this.linkMoreInfo.Size = new Size(0x8b, 13);
            this.linkMoreInfo.TabIndex = 0x12;
            this.linkMoreInfo.TabStop = true;
            this.linkMoreInfo.Text = "More info on this Strategy ...";
            this.lblDataValue.ForeColor = SystemColors.ControlText;
            this.lblDataValue.Location = new Point(0x4d, 0x86);
            this.lblDataValue.Name = "lblDataValue";
            this.lblDataValue.Size = new Size(130, 0x2d);
            this.lblDataValue.TabIndex = 0x13;
            this.lblDataValue.Text = "Symbol AA";
            this.lblScaleValue.AutoSize = true;
            this.lblScaleValue.ForeColor = SystemColors.ControlText;
            this.lblScaleValue.Location = new Point(0x4d, 0x53);
            this.lblScaleValue.Name = "lblScaleValue";
            this.lblScaleValue.Size = new Size(30, 13);
            this.lblScaleValue.TabIndex = 20;
            this.lblScaleValue.Text = "Daily";
            this.lblRangeValue.AutoSize = true;
            this.lblRangeValue.ForeColor = SystemColors.ControlText;
            this.lblRangeValue.Location = new Point(0x4d, 100);
            this.lblRangeValue.Name = "lblRangeValue";
            this.lblRangeValue.Size = new Size(0x8a, 13);
            this.lblRangeValue.TabIndex = 0x15;
            this.lblRangeValue.Text = "10/10/1999 to 10/10/2000";
            this.lblParamsValue.AutoSize = true;
            this.lblParamsValue.ForeColor = SystemColors.ControlText;
            this.lblParamsValue.Location = new Point(0x4d, 0x75);
            this.lblParamsValue.Name = "lblParamsValue";
            this.lblParamsValue.Size = new Size(40, 13);
            this.lblParamsValue.TabIndex = 0x16;
            this.lblParamsValue.Text = "(20,30)";
            this.lblPerformance.AutoSize = true;
            this.lblPerformance.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblPerformance.Location = new Point(0x123, 0x20);
            this.lblPerformance.Name = "lblPerformance";
            this.lblPerformance.Size = new Size(0x84, 13);
            this.lblPerformance.TabIndex = 0x17;
            this.lblPerformance.Text = "Backtest Performance";
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new Point(0x123, 0x31);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new Size(0x4b, 13);
            this.lblMode.TabIndex = 0x18;
            this.lblMode.Text = "Testing Mode:";
            this.lblStartingCap.AutoSize = true;
            this.lblStartingCap.Location = new Point(0x123, 0x42);
            this.lblStartingCap.Name = "lblStartingCap";
            this.lblStartingCap.Size = new Size(0x51, 13);
            this.lblStartingCap.TabIndex = 0x19;
            this.lblStartingCap.Text = "Starting Capital:";
            this.lblPosSize.AutoSize = true;
            this.lblPosSize.Location = new Point(0x123, 0x53);
            this.lblPosSize.Name = "lblPosSize";
            this.lblPosSize.Size = new Size(70, 13);
            this.lblPosSize.TabIndex = 0x1a;
            this.lblPosSize.Text = "Position Size:";
            this.lblNetProfit.AutoSize = true;
            this.lblNetProfit.Location = new Point(0x123, 100);
            this.lblNetProfit.Name = "lblNetProfit";
            this.lblNetProfit.Size = new Size(0x60, 13);
            this.lblNetProfit.TabIndex = 0x1b;
            this.lblNetProfit.Text = "Strategy Net Profit:";
            this.lblBH.AutoSize = true;
            this.lblBH.Location = new Point(0x123, 0x75);
            this.lblBH.Name = "lblBH";
            this.lblBH.Size = new Size(0x4a, 13);
            this.lblBH.TabIndex = 0x1c;
            this.lblBH.Text = "Buy and Hold:";
            this.lblModeValue.AutoSize = true;
            this.lblModeValue.ForeColor = SystemColors.ControlText;
            this.lblModeValue.Location = new Point(0x182, 0x31);
            this.lblModeValue.Name = "lblModeValue";
            this.lblModeValue.Size = new Size(0x60, 13);
            this.lblModeValue.TabIndex = 0x1d;
            this.lblModeValue.Text = "Portfolio Simulation";
            this.lblStartCapValue.AutoSize = true;
            this.lblStartCapValue.ForeColor = SystemColors.ControlText;
            this.lblStartCapValue.Location = new Point(0x182, 0x42);
            this.lblStartCapValue.Name = "lblStartCapValue";
            this.lblStartCapValue.Size = new Size(0x3d, 13);
            this.lblStartCapValue.TabIndex = 30;
            this.lblStartCapValue.Text = "$1,000,000";
            this.lblPosSizeValue.AutoSize = true;
            this.lblPosSizeValue.ForeColor = SystemColors.ControlText;
            this.lblPosSizeValue.Location = new Point(0x182, 0x53);
            this.lblPosSizeValue.Name = "lblPosSizeValue";
            this.lblPosSizeValue.Size = new Size(40, 13);
            this.lblPosSizeValue.TabIndex = 0x1f;
            this.lblPosSizeValue.Text = "$5,000";
            this.linkNetProfit.AutoSize = true;
            this.linkNetProfit.DisabledLinkColor = SystemColors.ControlText;
            this.linkNetProfit.ForeColor = SystemColors.ControlText;
            this.linkNetProfit.LinkColor = SystemColors.ActiveCaption;
            this.linkNetProfit.Location = new Point(0x182, 100);
            this.linkNetProfit.Name = "linkNetProfit";
            this.linkNetProfit.Size = new Size(0x43, 13);
            this.linkNetProfit.TabIndex = 0x20;
            this.linkNetProfit.TabStop = true;
            this.linkNetProfit.Text = "$123,456.78";
            this.linkNetProfit.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkBuyHold_LinkClicked);
            this.linkBuyHold.AutoSize = true;
            this.linkBuyHold.DisabledLinkColor = SystemColors.ControlText;
            this.linkBuyHold.ForeColor = SystemColors.ControlText;
            this.linkBuyHold.LinkColor = SystemColors.ActiveCaption;
            this.linkBuyHold.Location = new Point(0x182, 0x75);
            this.linkBuyHold.Name = "linkBuyHold";
            this.linkBuyHold.Size = new Size(0x43, 13);
            this.linkBuyHold.TabIndex = 0x21;
            this.linkBuyHold.TabStop = true;
            this.linkBuyHold.Text = "$765,432,10";
            this.linkBuyHold.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkBuyHold_LinkClicked);
            this.lblRangeTested.AutoSize = true;
            this.lblRangeTested.Location = new Point(0x123, 0x86);
            this.lblRangeTested.Name = "lblRangeTested";
            this.lblRangeTested.Size = new Size(0x63, 13);
            this.lblRangeTested.TabIndex = 0x22;
            this.lblRangeTested.Text = "Range Backtested:";
            this.lblRangeTestedValue.AutoSize = true;
            this.lblRangeTestedValue.Location = new Point(0x182, 0x86);
            this.lblRangeTestedValue.Name = "lblRangeTestedValue";
            this.lblRangeTestedValue.Size = new Size(0x8a, 13);
            this.lblRangeTestedValue.TabIndex = 0x23;
            this.lblRangeTestedValue.Text = "10/10/1999 to 10/10/2000";
            this.lblActivation.AutoSize = true;
            this.lblActivation.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblActivation.Location = new Point(0x1f0, 0x20);
            this.lblActivation.Name = "lblActivation";
            this.lblActivation.Size = new Size(0x73, 13);
            this.lblActivation.TabIndex = 0x24;
            this.lblActivation.Text = "Strategy Activation";
            this.picActive.Location = new Point(0x1f3, 0x2e);
            this.picActive.Name = "picActive";
            this.picActive.Size = new Size(0x10, 0x10);
            this.picActive.TabIndex = 0x25;
            this.picActive.TabStop = false;
            this.imageList_0.ImageStream = (ImageListStreamer) resources.GetObject("imagesStreaming.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "Streaming.bmp");
            this.imageList_0.Images.SetKeyName(1, "StreamingDisconnect.bmp");
            this.imageList_0.Images.SetKeyName(2, "SendToOrderMgr.bmp");
            this.lblStrategyActive.Location = new Point(0x209, 0x31);
            this.lblStrategyActive.Name = "lblStrategyActive";
            this.lblStrategyActive.Size = new Size(0x76, 0x22);
            this.lblStrategyActive.TabIndex = 0x26;
            this.lblStrategyActive.Text = "Chart is Static (Strategy is not Active)";
            this.lblAlerts.AutoSize = true;
            this.lblAlerts.Location = new Point(0x209, 0x53);
            this.lblAlerts.Name = "lblAlerts";
            this.lblAlerts.Size = new Size(0x68, 13);
            this.lblAlerts.TabIndex = 0x27;
            this.lblAlerts.Text = "Current Trade Alerts:";
            this.linkAlerts.AutoSize = true;
            this.linkAlerts.ForeColor = SystemColors.ActiveCaption;
            this.linkAlerts.LinkColor = SystemColors.ActiveCaption;
            this.linkAlerts.Location = new Point(0x277, 0x53);
            this.linkAlerts.Name = "linkAlerts";
            this.linkAlerts.Size = new Size(13, 13);
            this.linkAlerts.TabIndex = 40;
            this.linkAlerts.TabStop = true;
            this.linkAlerts.Text = "0";
            this.linkAlerts.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkAlerts_LinkClicked);
            this.picGenerating.Location = new Point(500, 0x61);
            this.picGenerating.Name = "picGenerating";
            this.picGenerating.Size = new Size(0x10, 0x10);
            this.picGenerating.TabIndex = 0x29;
            this.picGenerating.TabStop = false;
            this.lblGenerating.AutoSize = true;
            this.lblGenerating.Location = new Point(0x209, 100);
            this.lblGenerating.Name = "lblGenerating";
            this.lblGenerating.Size = new Size(0x91, 13);
            this.lblGenerating.TabIndex = 0x2a;
            this.lblGenerating.Text = "Generating Orders from Alerts";
            this.linkActivate.AutoSize = true;
            this.linkActivate.ForeColor = SystemColors.ActiveCaption;
            this.linkActivate.LinkColor = SystemColors.ActiveCaption;
            this.linkActivate.Location = new Point(0x209, 0x75);
            this.linkActivate.Name = "linkActivate";
            this.linkActivate.Size = new Size(0x76, 13);
            this.linkActivate.TabIndex = 0x2b;
            this.linkActivate.TabStop = true;
            this.linkActivate.Text = "Add to Strategy Monitor";
            this.linkActivate.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkActivate_LinkClicked);
            this.lblStrategyMonitor.Location = new Point(0x209, 0x86);
            this.lblStrategyMonitor.Name = "lblStrategyMonitor";
            this.lblStrategyMonitor.Size = new Size(0x90, 0x2d);
            this.lblStrategyMonitor.TabIndex = 0x2c;
            this.lblStrategyMonitor.Text = "(Strategies can also be Activated and Monitored in the Strategy Monitor window)";
            this.popup.Items.AddRange(new ToolStripItem[] { this.mniCopy, this.mniPrint, this.mniPrintAll });
            this.popup.Name = "popup";
            this.popup.Size = new Size(0xdb, 70);
            this.mniCopy.Image = (Image) resources.GetObject("mniCopy.Image");
            this.mniCopy.ImageTransparentColor = Color.Fuchsia;
            this.mniCopy.Name = "mniCopy";
            this.mniCopy.Size = new Size(0xda, 0x16);
            this.mniCopy.Text = "Copy Summary to Clipboard";
            this.mniCopy.Click += new EventHandler(this.mniCopy_Click);
            this.mniPrint.Image = (Image) resources.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0xda, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.mniPrintAll.Name = "mniPrintAll";
            this.mniPrintAll.Size = new Size(0xda, 0x16);
            this.mniPrintAll.Text = "Print All";
            this.mniPrintAll.ToolTipText = "Print content from all tabs";
            this.mniPrintAll.Click += new EventHandler(this.mniPrintAll_Click);
            this.lblStrategiesIncluded.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblStrategiesIncluded.Location = new Point(0x8b, 0x20);
            this.lblStrategiesIncluded.Name = "lblStrategiesIncluded";
            this.lblStrategiesIncluded.Size = new Size(0x76, 0x17);
            this.lblStrategiesIncluded.TabIndex = 0x2d;
            this.lblStrategiesIncluded.Text = "Strategies included";
            this.richTextBox1.BackColor = SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new Point(0x8e, 0x30);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new Size(0x92, 0x7c);
            this.richTextBox1.TabIndex = 0x2f;
            this.richTextBox1.Text = "";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.popup;
            base.Controls.Add(this.richTextBox1);
            base.Controls.Add(this.lblStrategiesIncluded);
            base.Controls.Add(this.lblStrategyMonitor);
            base.Controls.Add(this.linkActivate);
            base.Controls.Add(this.lblGenerating);
            base.Controls.Add(this.picGenerating);
            base.Controls.Add(this.linkAlerts);
            base.Controls.Add(this.lblAlerts);
            base.Controls.Add(this.lblStrategyActive);
            base.Controls.Add(this.picActive);
            base.Controls.Add(this.lblActivation);
            base.Controls.Add(this.lblRangeTestedValue);
            base.Controls.Add(this.lblRangeTested);
            base.Controls.Add(this.linkBuyHold);
            base.Controls.Add(this.linkNetProfit);
            base.Controls.Add(this.lblPosSizeValue);
            base.Controls.Add(this.lblStartCapValue);
            base.Controls.Add(this.lblModeValue);
            base.Controls.Add(this.lblBH);
            base.Controls.Add(this.lblNetProfit);
            base.Controls.Add(this.lblPosSize);
            base.Controls.Add(this.lblStartingCap);
            base.Controls.Add(this.lblMode);
            base.Controls.Add(this.lblPerformance);
            base.Controls.Add(this.lblParamsValue);
            base.Controls.Add(this.lblRangeValue);
            base.Controls.Add(this.lblScaleValue);
            base.Controls.Add(this.lblDataValue);
            base.Controls.Add(this.linkMoreInfo);
            base.Controls.Add(this.linkEdit);
            base.Controls.Add(this.linkGoBack);
            base.Controls.Add(this.lblDescription);
            base.Controls.Add(this.lblParams);
            base.Controls.Add(this.lblRange);
            base.Controls.Add(this.lblScale);
            base.Controls.Add(this.lblData);
            base.Controls.Add(this.lblModifiedValue);
            base.Controls.Add(this.lblCreatedValue);
            base.Controls.Add(this.lblAuthorValue);
            base.Controls.Add(this.lblModified);
            base.Controls.Add(this.lblCreated);
            base.Controls.Add(this.lblAuthor);
            base.Controls.Add(this.lblStrategyName);
            base.Controls.Add(this.lblName);
            base.Controls.Add(this.browserDesc);
            base.Name = "Description";
            base.Size = new Size(670, 0x1b0);
            ((ISupportInitialize) this.picActive).EndInit();
            ((ISupportInitialize) this.picGenerating).EndInit();
            this.popup.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void linkActivate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChartForm parentForm = (ChartForm) base.ParentForm;
            if (parentForm.Strategy.Name == string.Empty)
            {
                parentForm.SaveStrategyAs();
                if (parentForm.Strategy.Name == string.Empty)
                {
                    MessageBox.Show("An untitled strategy cannot be added to the Strategy Monitor.", "Please Save Strategy", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
            parentForm.AddToStrategyMonitor();
        }

        private void linkAlerts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChartForm parentForm = (ChartForm) base.ParentForm;
            if (this.linkAlerts.Text == "1")
            {
                parentForm.SelectTab("1 Alert");
            }
            else
            {
                parentForm.SelectTab(this.linkAlerts.Text + " Alerts");
            }
        }

        private void linkBuyHold_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ((ChartForm) base.ParentForm).SelectTab("Performance");
        }

        private void linkEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EditDescriptionForm form = new EditDescriptionForm {
                Description = this.strategy.Description
            };
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.strategy.Description = form.Description;
                this.method_1();
            }
        }

        private void linkGoBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.browserDesc.GoBack();
        }

        private void method_0(bool bool_1)
        {
            this.richTextBox1.Visible = bool_1;
            this.lblStrategiesIncluded.Visible = bool_1;
            this.lblData.Visible = !bool_1;
            this.lblDataValue.Visible = !bool_1;
            this.lblParams.Visible = !bool_1;
            this.lblParamsValue.Visible = !bool_1;
            this.lblRange.Visible = !bool_1;
            this.lblRangeValue.Visible = !bool_1;
            this.lblScale.Visible = !bool_1;
            this.lblScaleValue.Visible = !bool_1;
            this.picActive.Visible = !bool_1;
            this.lblGenerating.Visible = !bool_1;
            this.linkActivate.Visible = !bool_1;
            this.lblStrategyMonitor.Visible = !bool_1;
        }

        private void method_1()
        {
            File.WriteAllText(MainModule.Instance.DataPath + @"\temp.html", this.strategy.Description);
            this.browserDesc.Navigate(MainModule.Instance.DataPath + @"\temp.html");
            this.linkGoBack.Visible = false;
            this.linkMoreInfo.Visible = (this.strategy.URL != null) && (this.strategy.URL != "");
        }

        private bool method_2()
        {
            using (IEnumerator<IPerformanceVisualizer> enumerator = MainModule.Instance.VisualizersChecked.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    IPerformanceVisualizer current = enumerator.Current;
                    if (current.TabText.Equals("Performance"))
                    {
                        ///goto  Label_0036;  ///WYJ fix, simplify the flow
                        return true;
                    }
                }
                return false;
            }
        }

        private void method_3(PrintPageEventArgs printPageEventArgs_0, ref Rectangle rectangle_0, bool bool_1)
        {
            Rectangle destRect = new Rectangle(rectangle_0.X, rectangle_0.Y, 270, rectangle_0.Height);
            ListView view = new ListView();
            view.Columns.Add("", 0x84, HorizontalAlignment.Left);
            view.Columns.Add("", 0x8a, HorizontalAlignment.Left);
            view.Items.Add(this.lblAuthor.Text).SubItems.Add(this.lblAuthorValue.Text);
            view.Items.Add(this.lblCreated.Text).SubItems.Add(this.lblCreatedValue.Text);
            view.Items.Add(this.lblModified.Text).SubItems.Add(this.lblModifiedValue.Text);
            view.Items.Add(this.lblScale.Text).SubItems.Add(this.lblScaleValue.Text);
            view.Items.Add(this.lblRange.Text).SubItems.Add(this.lblRangeValue.Text);
            view.Items.Add(this.lblParams.Text).SubItems.Add(this.lblParamsValue.Text);
            view.Items.Add(this.lblData.Text).SubItems.Add(this.lblDataValue.Text);
            this.printReport_0.printListView = view;
            this.printReport_0.PrintListView(printPageEventArgs_0, ref destRect, bool_1);
            int y = destRect.Y;
            int height = destRect.Height;
            destRect.X += 270;
            destRect.Y = rectangle_0.Y;
            destRect.Height = rectangle_0.Height;
            view.Dispose();
            view = new ListView();
            view.Columns.Add("", 0x84, HorizontalAlignment.Left);
            view.Columns.Add("", 0x8a, HorizontalAlignment.Left);
            view.Items.Add(this.lblPerformance.Text).Font = this.lblPerformance.Font;
            view.Items.Add(this.lblMode.Text).SubItems.Add(this.lblModeValue.Text);
            view.Items.Add(this.lblStartingCap.Text).SubItems.Add(this.lblStartCapValue.Text);
            view.Items.Add(this.lblPosSize.Text).SubItems.Add(this.lblPosSizeValue.Text);
            view.Items.Add(this.lblNetProfit.Text).SubItems.Add(this.linkNetProfit.Text);
            view.Items.Add(this.lblBH.Text).SubItems.Add(this.linkBuyHold.Text);
            view.Items.Add(this.lblRangeTested.Text).SubItems.Add(this.lblRangeTestedValue.Text);
            this.printReport_0.printListView = view;
            this.printReport_0.PrintListView(printPageEventArgs_0, ref destRect, bool_1);
            if (destRect.Y > y)
            {
                y = destRect.Y;
                height = destRect.Height;
            }
            destRect.X += 270;
            destRect.Width = printPageEventArgs_0.MarginBounds.Width - destRect.X;
            if (destRect.Width < 0x84)
            {
                destRect.X = rectangle_0.X;
                destRect.Width = 270;
                destRect.Y = y;
                destRect.Height = height;
                y = 0;
                height = 0;
            }
            else
            {
                destRect.Y = rectangle_0.Y;
                destRect.Height = rectangle_0.Height;
            }
            view.Dispose();
            view = new ListView();
            view.Columns.Add("", destRect.Width - 12, HorizontalAlignment.Left);
            view.Columns.Add("", 12, HorizontalAlignment.Left);
            view.Items.Add(this.lblActivation.Text).Font = this.lblActivation.Font;
            ListViewItem item = view.Items.Add(this.lblStrategyActive.Text);
            view.Items.Add(this.lblAlerts.Text).SubItems.Add(this.linkAlerts.Text);
            if (this.lblGenerating.Visible)
            {
                item = view.Items.Add(this.lblGenerating.Text);
            }
            this.printReport_0.printListView = view;
            this.printReport_0.PrintListView(printPageEventArgs_0, ref destRect, bool_1);
            if (destRect.Y > y)
            {
                y = destRect.Y;
                height = destRect.Height;
            }
            rectangle_0.Y = y;
            rectangle_0.Height = height;
            rectangle_0.Y += this.int_5;
            rectangle_0.Height -= this.int_5;
            rectangle_0.X = printPageEventArgs_0.MarginBounds.X;
        }

        private ListView method_4()
        {
            ListView view = new ListView();
            view.Columns.Add("", 0x84, HorizontalAlignment.Left);
            view.Columns.Add("", 0x8a, HorizontalAlignment.Left);
            view.Items.Add(this.lblName.Text).SubItems.Add(this.lblStrategyName.Text);
            view.Items.Add(this.lblAuthor.Text).SubItems.Add(this.lblAuthorValue.Text);
            view.Items.Add(this.lblCreated.Text).SubItems.Add(this.lblCreatedValue.Text);
            view.Items.Add(this.lblModified.Text).SubItems.Add(this.lblModifiedValue.Text);
            view.Items.Add(this.lblScale.Text).SubItems.Add(this.lblScaleValue.Text);
            view.Items.Add(this.lblRange.Text).SubItems.Add(this.lblRangeValue.Text);
            view.Items.Add(this.lblParams.Text).SubItems.Add(this.lblParamsValue.Text);
            view.Items.Add(this.lblData.Text).SubItems.Add(this.lblDataValue.Text);
            view.Items.Add(this.lblPerformance.Text).Font = this.lblPerformance.Font;
            view.Items.Add(this.lblMode.Text).SubItems.Add(this.lblModeValue.Text);
            view.Items.Add(this.lblStartingCap.Text).SubItems.Add(this.lblStartCapValue.Text);
            view.Items.Add(this.lblPosSize.Text).SubItems.Add(this.lblPosSizeValue.Text);
            view.Items.Add(this.lblNetProfit.Text).SubItems.Add(this.linkNetProfit.Text);
            view.Items.Add(this.lblBH.Text).SubItems.Add(this.linkBuyHold.Text);
            view.Items.Add(this.lblRangeTested.Text).SubItems.Add(this.lblRangeTestedValue.Text);
            view.Items.Add(this.lblActivation.Text).Font = this.lblActivation.Font;
            ListViewItem item = view.Items.Add(this.lblStrategyActive.Text);
            view.Items.Add(this.lblAlerts.Text).SubItems.Add(this.linkAlerts.Text);
            if (this.lblGenerating.Visible)
            {
                item = view.Items.Add(this.lblGenerating.Text);
            }
            return view;
        }

        private void mniCopy_Click(object sender, EventArgs e)
        {
            this.CopyToClipboard();
        }

        private void mniPrint_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        private void mniPrintAll_Click(object sender, EventArgs e)
        {
            ((ChartForm) base.ParentForm).PrintAll();
        }

        public void PopulateActivationSection()
        {
            ChartForm parentForm = (ChartForm) base.ParentForm;
            if (!parentForm.IsStreaming)
            {
                this.picActive.Image = this.imageList_0.Images[1];
                this.lblStrategyActive.Text = "Chart is Static (Strategy is not Active)";
                this.picGenerating.Visible = false;
                this.lblGenerating.Visible = false;
            }
            else
            {
                this.picActive.Image = this.imageList_0.Images[0];
                this.lblStrategyActive.Text = "Chart is Streaming (Strategy is Active)";
                bool autoStage = parentForm.AutoStage;
                this.picGenerating.Image = this.imageList_0.Images[2];
                this.picGenerating.Visible = autoStage;
                this.lblGenerating.Visible = autoStage;
            }
        }

        public void Print()
        {
            PrintDocument prtdoc = new PrintDocument();
            prtdoc.BeginPrint += new PrintEventHandler(this.descriptionReport_BeginPrint);
            prtdoc.PrintPage += new PrintPageEventHandler(this.descriptionReport_PrintPage);
            prtdoc.EndPrint += new PrintEventHandler(this.descriptionReport_EndPrint);
            ChartForm parentForm = (ChartForm) base.ParentForm;
            PageSettings pageSettings = new PageSettings();
            parentForm.GetPageSettings(ref pageSettings);
            prtdoc.DefaultPageSettings = pageSettings;
            this.printReport_0.ShowPrintDialog = parentForm.ShowPrintDialog();
            this.printReport_0.ShowPrintPreview = parentForm.ShowPrintPreview();
            if (parentForm.ShowPrintPreview())
            {
                this.printPreview_0 = new PrintPreview(prtdoc);
                this.printPreview_0.ShowPrintDialog = parentForm.ShowPrintDialog();
                this.printPreview_0.ShowDialog();
                this.printPreview_0.Dispose();
            }
            else
            {
                PrintDialog dialog = new PrintDialog {
                    Document = prtdoc
                };
                if (parentForm.ShowPrintDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        dialog.Document.Print();
                    }
                }
                else
                {
                    dialog.Document.Print();
                }
                dialog.Dispose();
            }
            prtdoc.Dispose();
        }

        public void SetDescriptiveFields(Bars bars, BarDataScale dataScale, BarDataRange range, PositionSize posSize, DataSource dataSet, string symbol, WealthScript wealthScript_0, SystemResults results, SystemResults buyHold, bool pvUsed)
        {
            if (symbol != "")
            {
                this.lblDataValue.Text = "Symbol " + symbol;
            }
            else if (dataSet != null)
            {
                this.lblDataValue.Text = "{" + dataSet.Name + "}";
            }
            else
            {
                this.lblDataValue.Text = "";
            }
            this.lblScaleValue.Text = dataScale.ToString();
            this.lblRangeValue.Text = range.Text;
            if (wealthScript_0 == null)
            {
                this.lblParamsValue.Text = "";
            }
            else if (pvUsed && (symbol == ""))
            {
                this.lblParamsValue.Text = "(Preferred Values Used)";
            }
            else
            {
                this.lblParamsValue.Text = wealthScript_0.ParameterString;
            }
            if (posSize.RawProfitMode)
            {
                this.lblModeValue.Text = "Raw Profit";
                this.lblStartCapValue.Text = "(N/A in RP mode)";
            }
            else
            {
                this.lblModeValue.Text = "Portfolio Simulation";
                this.lblStartCapValue.Text = posSize.StartingCapital.ToString("C");
            }
            this.lblPosSizeValue.Text = posSize.Text;
            if (results != null)
            {
                this.linkNetProfit.Text = results.NetProfit.ToString("C");
            }
            else
            {
                this.linkNetProfit.Text = "";
            }
            if (buyHold != null)
            {
                this.linkBuyHold.Text = buyHold.NetProfit.ToString("C");
            }
            else
            {
                this.linkBuyHold.Text = "";
            }
            if (!this.method_2())
            {
                this.linkNetProfit.Links.Clear();
                this.linkBuyHold.Links.Clear();
            }
            if (bars == null)
            {
                this.lblRangeTestedValue.Text = "";
            }
            else if (bars.Count == 0)
            {
                this.lblRangeTestedValue.Text = "No Data";
            }
            else
            {
                this.lblRangeTestedValue.Text = bars.Date[0].ToShortDateString() + " to " + bars.Date[bars.Count - 1].ToShortDateString();
            }
            this.linkAlerts.Text = results.Alerts.Count.ToString();
            this.PopulateActivationSection();
            if (this.Strategy.StrategyType == StrategyType.CombinedStrategy)
            {
                this.method_0(true);
                this.richTextBox1.Text = "";
                foreach (CombinedStrategyInfo info in this.Strategy.CombinedStrategyChildren)
                {
                    if (this.richTextBox1.Text.Equals(string.Empty))
                    {
                        this.richTextBox1.Text = info.Name;
                    }
                    else
                    {
                        this.richTextBox1.Text = this.richTextBox1.Text + "\n" + info.Name;
                    }
                }
            }
            else
            {
                this.method_0(false);
            }
        }

        public WealthLab.Strategy Strategy
        {
            get
            {
                return this.strategy;
            }
            set
            {
                this.strategy = value;
                if (this.strategy != null)
                {
                    this.lblStrategyName.Text = this.strategy.Name;
                    this.method_1();
                    this.lblAuthorValue.Text = this.strategy.Author;
                    this.lblCreatedValue.Text = this.strategy.CreationDate.ToShortDateString();
                    this.lblModifiedValue.Text = this.strategy.LastModified.ToShortDateString();
                }
            }
        }
    }
}

