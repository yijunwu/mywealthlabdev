namespace WealthLab
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public class PrintPreview : Form
    {
        private bool bool_0;
        private bool bool_1;
        private ToolStripButton btnClose;
        private ToolStripButton btnDown;
        private ToolStripButton btnPrint;
        private ToolStripButton btnSetup;
        private ToolStripButton btnUp;
        private ToolStripComboBox cmbZoom;
        private IContainer icontainer_0;
        private int int_0;
        private int int_1;
        private ToolStripLabel lblOf;
        private ToolStripLabel lblPage;
        private ToolStripLabel lblPages;
        private ToolStripLabel lblZoom;
        private PageSetupDialog pageSetupDialog_0;
        private PrintDialog printDialog_0;
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
            this.printDialog_0.Document = this.printPreviewControl1.Document;
            if (this.bool_1)
            {
                if (this.printDialog_0.ShowDialog() == DialogResult.OK)
                {
                    if (this.printDialog_0.AllowSomePages)
                    {
                        this.bool_0 = true;
                        this.int_0 = this.printDialog_0.PrinterSettings.FromPage;
                        this.int_1 = this.printDialog_0.PrinterSettings.ToPage;
                    }
                    this.printDialog_0.Document.Print();
                    base.Close();
                }
            }
            else
            {
                this.printDialog_0.Document.Print();
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(PrintPreview));
            this.toolStrip1 = new ToolStrip();
            this.btnPrint = new ToolStripButton();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.btnSetup = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.lblZoom = new ToolStripLabel();
            this.cmbZoom = new ToolStripComboBox();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.lblPage = new ToolStripLabel();
            this.txbxPage = new ToolStripTextBox();
            this.lblOf = new ToolStripLabel();
            this.lblPages = new ToolStripLabel();
            this.btnDown = new ToolStripButton();
            this.btnUp = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.btnClose = new ToolStripButton();
            this.printPreviewControl1 = new PrintPreviewControl();
            this.pageSetupDialog_0 = new PageSetupDialog();
            this.printDialog_0 = new PrintDialog();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.btnPrint, this.toolStripSeparator4, this.btnSetup, this.toolStripSeparator1, this.lblZoom, this.cmbZoom, this.toolStripSeparator3, this.lblPage, this.txbxPage, this.lblOf, this.lblPages, this.btnDown, this.btnUp, this.toolStripSeparator2, this.btnClose });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x2b9, 0x19);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            this.btnPrint.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnPrint.Image = (Image) resources.GetObject("btnPrint.Image");
            this.btnPrint.ImageTransparentColor = Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x2d, 0x16);
            this.btnPrint.Text = "&Print...";
            this.btnPrint.ToolTipText = "Print";
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(6, 0x19);
            this.btnSetup.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnSetup.Image = (Image) resources.GetObject("btnSetup.Image");
            this.btnSetup.ImageTransparentColor = Color.Magenta;
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new Size(0x27, 0x16);
            this.btnSetup.Text = "&Setup";
            this.btnSetup.ToolTipText = "Page Setup";
            this.btnSetup.Click += new EventHandler(this.btnSetup_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.lblZoom.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new Size(0x25, 0x16);
            this.lblZoom.Text = "Zoom:";
            this.lblZoom.TextAlign = ContentAlignment.MiddleRight;
            this.cmbZoom.Items.AddRange(new object[] { "Auto", "500%", "200%", "150%", "100%", "75%", "50%", "25%", "10%", "Two Pages", "Whole Page", "Page Width", "All Pages" });
            this.cmbZoom.MaxDropDownItems = 10;
            this.cmbZoom.Name = "cmbZoom";
            this.cmbZoom.Size = new Size(100, 0x19);
            this.cmbZoom.Text = "Auto";
            this.cmbZoom.ToolTipText = "Zoom";
            this.cmbZoom.TextChanged += new EventHandler(this.cmbZoom_TextChanged);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 0x19);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new Size(0x1f, 0x16);
            this.lblPage.Text = "Pa&ge";
            this.txbxPage.Name = "txbxPage";
            this.txbxPage.Size = new Size(0x19, 0x19);
            this.txbxPage.Text = "1";
            this.txbxPage.TextBoxTextAlign = HorizontalAlignment.Right;
            this.txbxPage.ToolTipText = "Go to page";
            this.txbxPage.TextChanged += new EventHandler(this.txbxPage_TextChanged);
            this.lblOf.Name = "lblOf";
            this.lblOf.Size = new Size(0x11, 0x16);
            this.lblOf.Text = "of";
            this.lblPages.Name = "lblPages";
            this.lblPages.Size = new Size(13, 0x16);
            this.lblPages.Text = "1";
            this.btnDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnDown.Enabled = false;
            this.btnDown.Image = (Image) resources.GetObject("btnDown.Image");
            this.btnDown.ImageTransparentColor = Color.Magenta;
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new Size(0x17, 0x16);
            this.btnDown.Text = "toolStripButton1";
            this.btnDown.ToolTipText = "Previous page";
            this.btnDown.Click += new EventHandler(this.btnDown_Click);
            this.btnUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnUp.Enabled = false;
            this.btnUp.Image = (Image) resources.GetObject("btnUp.Image");
            this.btnUp.ImageTransparentColor = Color.Magenta;
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new Size(0x17, 0x16);
            this.btnUp.Text = "toolStripButton1";
            this.btnUp.ToolTipText = "Next page";
            this.btnUp.Click += new EventHandler(this.btnUp_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.btnClose.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnClose.Image = (Image) resources.GetObject("btnClose.Image");
            this.btnClose.ImageTransparentColor = Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x25, 0x16);
            this.btnClose.Text = "&Close";
            this.btnClose.ToolTipText = "Close preview";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.printPreviewControl1.Cursor = Cursors.Default;
            this.printPreviewControl1.Dock = DockStyle.Fill;
            this.printPreviewControl1.Location = new Point(0, 0x19);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new Size(0x2b9, 0x1ce);
            this.printPreviewControl1.TabIndex = 1;
            this.printPreviewControl1.StartPageChanged += new EventHandler(this.printPreviewControl1_StartPageChanged);
            this.printDialog_0.UseEXDialog = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x2b9, 0x1e7);
            base.Controls.Add(this.printPreviewControl1);
            base.Controls.Add(this.toolStrip1);
            base.Name = "PrintPreview";
            this.Text = "Print Preview";
            base.Resize += new EventHandler(this.PrintPreview_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
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
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
                this.txbxPage.Text = this.int_0.ToString();
            }
        }

        public bool PrintSomePages
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public bool ShowPrintDialog
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public int ToPage
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
                this.lblPages.Text = this.int_1.ToString();
                if (value > 1)
                {
                    this.btnUp.Enabled = true;
                }
            }
        }
    }
}

