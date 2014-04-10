namespace WealthLab
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public class PrintPreview : Form
    {
        private bool printSomePages;
        private bool showPrintDialog;
        private ToolStripButton btnClose;
        private ToolStripButton btnDown;
        private ToolStripButton btnPrint;
        private ToolStripButton btnSetup;
        private ToolStripButton btnUp;
        private ToolStripComboBox cmbZoom;
        private IContainer icontainer_0;
        private int fromPage;
        private int toPage;
        private ToolStripLabel lblOf;
        private ToolStripLabel lblPage;
        private ToolStripLabel lblPages;
        private ToolStripLabel lblZoom;
        private PageSetupDialog pageSetupDialog_0;
        private PrintDialog printDialog;
        private PrintPreviewControl printPreviewControl1;
        private ToolStrip toolStrip1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripTextBox txbxPage;

        public PrintPreview()
        {
            this.InitializeComponent();
        }

        public PrintPreview(PrintDocument prtdoc)
        {
            this.InitializeComponent();
            this.printPreviewControl1.Document = prtdoc;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            this.txbxPage.Text = (int.Parse(this.txbxPage.Text) - 1).ToString();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.printDialog.Document = this.printPreviewControl1.Document;
            if (this.showPrintDialog)
            {
                if (this.printDialog.ShowDialog() == DialogResult.OK)
                {
                    if (this.printDialog.AllowSomePages)
                    {
                        this.printSomePages = true;
                        this.fromPage = this.printDialog.PrinterSettings.FromPage;
                        this.toPage = this.printDialog.PrinterSettings.ToPage;
                    }
                    this.printDialog.Document.Print();
                    base.Close();
                }
            }
            else
            {
                this.printDialog.Document.Print();
                base.Close();
            }
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            this.pageSetupDialog_0.Document = this.printPreviewControl1.Document;
            if (DialogResult.OK == this.pageSetupDialog_0.ShowDialog())
            {
                this.Cursor = this.printPreviewControl1.Cursor = Cursors.WaitCursor;
                this.printPreviewControl1.InvalidatePreview();
                this.Cursor = this.printPreviewControl1.Cursor = Cursors.Default;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            this.txbxPage.Text = (int.Parse(this.txbxPage.Text) + 1).ToString();
        }

        private void cmbZoom_TextChanged(object sender, EventArgs e)
        {
            if (this.cmbZoom.Text == "500%")
            {
                this.printPreviewControl1.AutoZoom = false;
                this.printPreviewControl1.Zoom = 5.0;
            }
            else if (this.cmbZoom.Text == "200%")
            {
                this.printPreviewControl1.AutoZoom = false;
                this.printPreviewControl1.Zoom = 2.0;
            }
            else if (this.cmbZoom.Text == "150%")
            {
                this.printPreviewControl1.AutoZoom = false;
                this.printPreviewControl1.Zoom = 1.5;
            }
            else if (this.cmbZoom.Text == "100%")
            {
                this.printPreviewControl1.AutoZoom = false;
                this.printPreviewControl1.Zoom = 1.0;
            }
            else if (this.cmbZoom.Text == "75%")
            {
                this.printPreviewControl1.AutoZoom = false;
                this.printPreviewControl1.Zoom = 0.75;
            }
            else if (this.cmbZoom.Text == "50%")
            {
                this.printPreviewControl1.AutoZoom = false;
                this.printPreviewControl1.Zoom = 0.5;
            }
            else if (this.cmbZoom.Text == "25%")
            {
                this.printPreviewControl1.AutoZoom = false;
                this.printPreviewControl1.Zoom = 0.25;
            }
            else if (this.cmbZoom.Text == "10%")
            {
                this.printPreviewControl1.AutoZoom = false;
                this.printPreviewControl1.Zoom = 0.1;
            }
            else if (this.cmbZoom.Text == "Page Width")
            {
                this.printPreviewControl1.AutoZoom = false;
                this.printPreviewControl1.Columns = 1;
                this.printPreviewControl1.Rows = 1;
                double num6 = (this.printPreviewControl1.Width - this.printPreviewControl1.Margin.Left) - this.printPreviewControl1.Margin.Right;
                double width = this.printPreviewControl1.Document.DefaultPageSettings.PaperSize.Width;
                this.printPreviewControl1.Zoom = num6 / width;
            }
            else if ((this.cmbZoom.Text != "Whole Page") && (this.cmbZoom.Text != "Auto"))
            {
                if (this.cmbZoom.Text == "Two Pages")
                {
                    this.printPreviewControl1.AutoZoom = true;
                    this.printPreviewControl1.Columns = 2;
                    this.printPreviewControl1.Rows = 1;
                }
                else if (this.cmbZoom.Text == "All Pages")
                {
                    int num3 = 1;
                    int num4 = 1;
                    if (this.ToPage > 1)
                    {
                        int num2 = (int) Math.Sqrt((double) this.ToPage);
                        int num = (int) Math.Sqrt((double) this.ToPage);
                        if ((num2 * num) < this.ToPage)
                        {
                            int num5 = this.ToPage - (num2 * num);
                            num2++;
                            if (num5 > num)
                            {
                                num++;
                            }
                        }
                        if (this.printPreviewControl1.Width > this.printPreviewControl1.Height)
                        {
                            num3 = num2;
                            num4 = num;
                        }
                        else
                        {
                            num3 = num;
                            num4 = num2;
                        }
                    }
                    this.printPreviewControl1.AutoZoom = true;
                    this.printPreviewControl1.Columns = num3;
                    this.printPreviewControl1.Rows = num4;
                }
            }
            else
            {
                this.printPreviewControl1.AutoZoom = true;
                this.printPreviewControl1.Columns = 1;
                this.printPreviewControl1.Rows = 1;
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintPreview));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSetup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblZoom = new System.Windows.Forms.ToolStripLabel();
            this.cmbZoom = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.lblPage = new System.Windows.Forms.ToolStripLabel();
            this.txbxPage = new System.Windows.Forms.ToolStripTextBox();
            this.lblOf = new System.Windows.Forms.ToolStripLabel();
            this.lblPages = new System.Windows.Forms.ToolStripLabel();
            this.btnDown = new System.Windows.Forms.ToolStripButton();
            this.btnUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.printPreviewControl1 = new System.Windows.Forms.PrintPreviewControl();
            this.pageSetupDialog_0 = new System.Windows.Forms.PageSetupDialog();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPrint,
            this.toolStripSeparator4,
            this.btnSetup,
            this.toolStripSeparator1,
            this.lblZoom,
            this.cmbZoom,
            this.toolStripSeparator3,
            this.lblPage,
            this.txbxPage,
            this.lblOf,
            this.lblPages,
            this.btnDown,
            this.btnUp,
            this.toolStripSeparator2,
            this.btnClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(697, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnPrint
            // 
            this.btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(45, 22);
            this.btnPrint.Text = "&Print...";
            this.btnPrint.ToolTipText = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSetup
            // 
            this.btnSetup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSetup.Image = ((System.Drawing.Image)(resources.GetObject("btnSetup.Image")));
            this.btnSetup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(39, 22);
            this.btnSetup.Text = "&Setup";
            this.btnSetup.ToolTipText = "Page Setup";
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblZoom
            // 
            this.lblZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(37, 22);
            this.lblZoom.Text = "Zoom:";
            this.lblZoom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbZoom
            // 
            this.cmbZoom.Items.AddRange(new object[] {
            "Auto",
            "500%",
            "200%",
            "150%",
            "100%",
            "75%",
            "50%",
            "25%",
            "10%",
            "Two Pages",
            "Whole Page",
            "Page Width",
            "All Pages"});
            this.cmbZoom.MaxDropDownItems = 10;
            this.cmbZoom.Name = "cmbZoom";
            this.cmbZoom.Size = new System.Drawing.Size(100, 25);
            this.cmbZoom.Text = "Auto";
            this.cmbZoom.ToolTipText = "Zoom";
            this.cmbZoom.TextChanged += new System.EventHandler(this.cmbZoom_TextChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // lblPage
            // 
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(31, 22);
            this.lblPage.Text = "Pa&ge";
            // 
            // txbxPage
            // 
            this.txbxPage.Name = "txbxPage";
            this.txbxPage.Size = new System.Drawing.Size(25, 25);
            this.txbxPage.Text = "1";
            this.txbxPage.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txbxPage.ToolTipText = "Go to page";
            this.txbxPage.TextChanged += new System.EventHandler(this.txbxPage_TextChanged);
            // 
            // lblOf
            // 
            this.lblOf.Name = "lblOf";
            this.lblOf.Size = new System.Drawing.Size(17, 22);
            this.lblOf.Text = "of";
            // 
            // lblPages
            // 
            this.lblPages.Name = "lblPages";
            this.lblPages.Size = new System.Drawing.Size(13, 22);
            this.lblPages.Text = "1";
            // 
            // btnDown
            // 
            this.btnDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDown.Enabled = false;
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(23, 22);
            this.btnDown.Text = "toolStripButton1";
            this.btnDown.ToolTipText = "Previous page";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUp.Enabled = false;
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(23, 22);
            this.btnUp.Text = "toolStripButton1";
            this.btnUp.ToolTipText = "Next page";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClose
            // 
            this.btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(37, 22);
            this.btnClose.Text = "&Close";
            this.btnClose.ToolTipText = "Close preview";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // printPreviewControl1
            // 
            this.printPreviewControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.printPreviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printPreviewControl1.Location = new System.Drawing.Point(0, 25);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new System.Drawing.Size(697, 462);
            this.printPreviewControl1.TabIndex = 1;
            this.printPreviewControl1.StartPageChanged += new System.EventHandler(this.printPreviewControl1_StartPageChanged);
            // 
            // printDialog_0
            // 
            this.printDialog.UseEXDialog = true;
            // 
            // PrintPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 487);
            this.Controls.Add(this.printPreviewControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PrintPreview";
            this.Text = "Print Preview";
            this.Resize += new System.EventHandler(this.PrintPreview_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void PrintPreview_Resize(object sender, EventArgs e)
        {
            if (this.cmbZoom.Text == "All Pages")
            {
                this.cmbZoom_TextChanged(sender, e);
            }
        }

        private void printPreviewControl1_StartPageChanged(object sender, EventArgs e)
        {
            if ((this.printPreviewControl1.StartPage + 1) == this.FromPage)
            {
                this.btnDown.Enabled = false;
            }
            else
            {
                this.btnDown.Enabled = true;
            }
            if ((this.printPreviewControl1.StartPage + 1) == this.ToPage)
            {
                this.btnUp.Enabled = false;
            }
            else
            {
                this.btnUp.Enabled = true;
            }
        }

        private void txbxPage_TextChanged(object sender, EventArgs e)
        {
            int num;
            if (int.TryParse(this.txbxPage.Text, out num))
            {
                num--;
                if (num != this.printPreviewControl1.StartPage)
                {
                    if ((num >= 0) && (num < this.ToPage))
                    {
                        this.printPreviewControl1.StartPage = num;
                    }
                    else
                    {
                        this.txbxPage.Text = (this.printPreviewControl1.StartPage + 1).ToString();
                    }
                }
            }
            else
            {
                this.txbxPage.Text = (this.printPreviewControl1.StartPage + 1).ToString();
            }
        }

        public int FromPage
        {
            get
            {
                return this.fromPage;
            }
            set
            {
                this.fromPage = value;
                this.txbxPage.Text = this.fromPage.ToString();
            }
        }

        public bool PrintSomePages
        {
            get
            {
                return this.printSomePages;
            }
            set
            {
                this.printSomePages = value;
            }
        }

        public bool ShowPrintDialog
        {
            get
            {
                return this.showPrintDialog;
            }
            set
            {
                this.showPrintDialog = value;
            }
        }

        public int ToPage
        {
            get
            {
                return this.toPage;
            }
            set
            {
                this.toPage = value;
                this.lblPages.Text = this.toPage.ToString();
                if (value > 1)
                {
                    this.btnUp.Enabled = true;
                }
            }
        }
    }
}

