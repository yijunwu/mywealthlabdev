using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using GroupBox = System.Windows.Forms.GroupBox;

internal class YahooWizardPageSymbols : UserControl
{
    private ComboBox cmbPrefix;
    private GroupBox grpSymbols;
    private IContainer icontainer_0;
    private Label lblPrefix;
    private Label lblSymbols;
    private TextBox txtSymbols;

    public YahooWizardPageSymbols()
    {
        this.InitializeComponent();
    }

    private void cmbPrefix_DrawItem(object sender, DrawItemEventArgs e)
    {
        Graphics graphics = e.Graphics;
        Rectangle bounds = e.Bounds;
        Font font = new Font(e.Font.FontFamily, e.Font.SizeInPoints, FontStyle.Bold);
        if (e.Index >= 0)
        {
            Font font2 = (e.Index == 0) ? e.Font : font;
            string s = (this.cmbPrefix.Items[e.Index] as Surfix).surfix;
            string d = (this.cmbPrefix.Items[e.Index] as Surfix).description;
            if ((e.State & DrawItemState.Selected) != DrawItemState.None)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.Highlight)), bounds);
                graphics.DrawString(s, font2, new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText)), bounds);
                graphics.DrawString("\t" + d, e.Font, new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText)), bounds);
                e.DrawFocusRectangle();
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(e.BackColor), bounds);
                graphics.DrawString(s, font2, new SolidBrush(e.ForeColor), bounds);
                graphics.DrawString("\t" + d, e.Font, new SolidBrush(e.ForeColor), bounds);
            }
        }
        graphics.Dispose();
    }

    private void cmbPrefix_SelectedIndexChanged(object sender, EventArgs e)
    {
        SymbolList sList = new SymbolList(this.txtSymbols.Text, DelimeterSetEnum.ForGuiInput) {
            sortNeeded = false,
            delimiterForDisplay = ' '
        };
        sList.AddSurfixToSymbolNames((this.cmbPrefix.SelectedItem as Surfix).surfix, '.');
        this.txtSymbols.Text = sList.ToString();
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
        this.grpSymbols = new GroupBox();
        this.cmbPrefix = new ComboBox();
        this.lblPrefix = new Label();
        this.lblSymbols = new Label();
        this.txtSymbols = new TextBox();
        this.grpSymbols.SuspendLayout();
        base.SuspendLayout();
        this.grpSymbols.Controls.Add(this.cmbPrefix);
        this.grpSymbols.Controls.Add(this.lblPrefix);
        this.grpSymbols.Controls.Add(this.lblSymbols);
        this.grpSymbols.Controls.Add(this.txtSymbols);
        this.grpSymbols.Location = new Point(7, 7);
        this.grpSymbols.Name = "grpSymbols";
        this.grpSymbols.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.grpSymbols.Size = new Size(550, 0x157);
        this.grpSymbols.TabIndex = 5;
        this.grpSymbols.TabStop = false;
        this.grpSymbols.Text = "Symbols";
        this.cmbPrefix.DrawMode = DrawMode.OwnerDrawFixed;
        this.cmbPrefix.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbPrefix.FormattingEnabled = true;
        this.cmbPrefix.Location = new Point(0x42, 0x13b);
        this.cmbPrefix.MaxDropDownItems = 12;
        this.cmbPrefix.Name = "cmbPrefix";
        this.cmbPrefix.Size = new Size(330, 0x15);
        this.cmbPrefix.TabIndex = 3;
        this.cmbPrefix.DrawItem += new DrawItemEventHandler(this.cmbPrefix_DrawItem);
        this.cmbPrefix.SelectedIndexChanged += new EventHandler(this.cmbPrefix_SelectedIndexChanged);
        this.lblPrefix.AutoSize = true;
        this.lblPrefix.Location = new Point(3, 0x13e);
        this.lblPrefix.Name = "lblPrefix";
        this.lblPrefix.Size = new Size(0x38, 13);
        this.lblPrefix.TabIndex = 2;
        this.lblPrefix.Text = "Add suffix:";
        this.lblSymbols.AutoSize = true;
        this.lblSymbols.Location = new Point(6, 0x13);
        this.lblSymbols.Name = "lblSymbols";
        this.lblSymbols.Size = new Size(0x10a, 13);
        this.lblSymbols.TabIndex = 1;
        this.lblSymbols.Text = "Enter Symbols below, separated by spaces or commas.";
        this.txtSymbols.BackColor = SystemColors.Window;
        this.txtSymbols.Location = new Point(6, 40);
        this.txtSymbols.Multiline = true;
        this.txtSymbols.Name = "txtSymbols";
        this.txtSymbols.ScrollBars = ScrollBars.Both;
        this.txtSymbols.Size = new Size(0x219, 0x10b);
        this.txtSymbols.TabIndex = 0;
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.Controls.Add(this.grpSymbols);
        base.Name = "YahooWizardPageSymbols";
        this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        base.Size = new Size(560, 0x161);
        this.grpSymbols.ResumeLayout(false);
        this.grpSymbols.PerformLayout();
        base.ResumeLayout(false);
    }

    public SymbolList generateSymbolList()
    {
        return new SymbolList(this.txtSymbols.Text, DelimeterSetEnum.ForGuiInput);
    }

    ///WYJ fix, original name: method_1
    public void InitStates()
    {
        if (this.cmbPrefix.Items.Count == 0)
        {
            this.cmbPrefix.Items.Add(new Surfix("None", ""));
            this.cmbPrefix.Items.Add(new Surfix(".CBT", "USA\t\tChicago Board of Trade"));
            this.cmbPrefix.Items.Add(new Surfix(".CME", "USA\t\tChicago Mercantile Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".NYB", "USA\t\tNew York Board of Trade"));
            this.cmbPrefix.Items.Add(new Surfix(".CMX", "USA\t\tNew York Commodities Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".NYM", "USA\t\tNew York Mercantile Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".OB", "USA\t\tOTC Bulletin Board Market"));
            this.cmbPrefix.Items.Add(new Surfix(".PK", "USA\t\tPink Sheets"));
            this.cmbPrefix.Items.Add(new Surfix(".BA", "Argentina\tBuenos Aires Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".VI", "Austria\t\tVienna Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".AX", "Australia\tAustralian Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".SA", "Brazil\t\tSao Paolo Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".TO", "Canada\t\tToronto Stock Exchangee"));
            this.cmbPrefix.Items.Add(new Surfix(".V", "Canada\t\tTSX Venture Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".SS", "China\t\tShanghai Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".SZ", "China\t\tShenzhen Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".CO", "Denmark\tCopenhagen Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".PA", "France\t\tParis Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".BE", "Germany\tBerlin Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".BM", "Germany\tBremen Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".DU", "Germany\tDusseldorf Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".F", "Germany\tFrankfurt Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".HM", "Germany\tHamburg Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".HA", "Germany\tHanover Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".MU", "Germany\tMunich Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".SG", "Germany\tStuttgart Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".DE", "Germany\tXETRA Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".HK", "Hong Kong\tHong Kong Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".BO", "India\t\tBombay Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".NS", "India\t\tNational Stock Exchange of India"));
            this.cmbPrefix.Items.Add(new Surfix(".JK", "Indonesia\tJakarta Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".TA", "Israel\t\tTel Aviv Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".MI", "Italy\t\tMilan Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".KS", "South Korea\tKorea Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".KQ", "South Korea\tKOSDAQ"));
            this.cmbPrefix.Items.Add(new Surfix(".MX", "Mexico\t\tMexico Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".AS", "Netherlands\tAmsterdam Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".NZ", "New Zealand\tNew Zealand Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".OL", "Norway\t\tOslo Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".SI", "Singapore\tSingapore Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".BC", "Spain\t\tBarcelona Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".BI", "Spain\t\tBilbao Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".MF", "Spain\t\tMadrid Fixed Income Market"));
            this.cmbPrefix.Items.Add(new Surfix(".MC", "Spain\t\tMadrid SE C.A.T.S."));
            this.cmbPrefix.Items.Add(new Surfix(".MA", "Spain\t\tMadrid Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".ST", "Sweden\t\tStockholm Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".SW", "Switzerland\tSwiss Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".TWO", "Taiwan\t\tTaiwan OTC Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".TW", "Taiwan\t\tTaiwan Stock Exchange"));
            this.cmbPrefix.Items.Add(new Surfix(".L", "United Kingdom\tLondon Stock Exchange"));
        }
        this.txtSymbols.Clear();
        this.cmbPrefix.SelectedIndex = 0;
    }

    internal class Surfix   ///WYJ note, surfix
    {
        public string surfix;
        public string description;

        public Surfix(string s, string d)
        {
            this.surfix = s;
            this.description = d;
        }
    }
}

