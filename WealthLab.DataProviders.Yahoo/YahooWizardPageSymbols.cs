using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

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
            string s = (this.cmbPrefix.Items[e.Index] as Class24).string_0;
            string str2 = (this.cmbPrefix.Items[e.Index] as Class24).string_1;
            if ((e.State & DrawItemState.Selected) != DrawItemState.None)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.Highlight)), bounds);
                graphics.DrawString(s, font2, new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText)), bounds);
                graphics.DrawString("\t" + str2, e.Font, new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText)), bounds);
                e.DrawFocusRectangle();
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(e.BackColor), bounds);
                graphics.DrawString(s, font2, new SolidBrush(e.ForeColor), bounds);
                graphics.DrawString("\t" + str2, e.Font, new SolidBrush(e.ForeColor), bounds);
            }
        }
        graphics.Dispose();
    }

    private void cmbPrefix_SelectedIndexChanged(object sender, EventArgs e)
    {
        SymbolList class2 = new SymbolList(this.txtSymbols.Text, Enum0.const_1) {
            sortNeeded = false,
            delimiterForDisplay = ' '
        };
        class2.AddSurfixToSymbolNames((this.cmbPrefix.SelectedItem as Class24).string_0, '.');
        this.txtSymbols.Text = class2.ToString();
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
        this.grpSymbols.RightToLeft = RightToLeft.No;
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
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.grpSymbols);
        base.Name = "YahooWizardPageSymbols";
        this.RightToLeft = RightToLeft.Yes;
        base.Size = new Size(560, 0x161);
        this.grpSymbols.ResumeLayout(false);
        this.grpSymbols.PerformLayout();
        base.ResumeLayout(false);
    }

    public SymbolList method_0()
    {
        return new SymbolList(this.txtSymbols.Text, Enum0.const_1);
    }

    public void method_1()
    {
        if (this.cmbPrefix.Items.Count == 0)
        {
            this.cmbPrefix.Items.Add(new Class24("None", ""));
            this.cmbPrefix.Items.Add(new Class24(".CBT", "USA\t\tChicago Board of Trade"));
            this.cmbPrefix.Items.Add(new Class24(".CME", "USA\t\tChicago Mercantile Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".NYB", "USA\t\tNew York Board of Trade"));
            this.cmbPrefix.Items.Add(new Class24(".CMX", "USA\t\tNew York Commodities Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".NYM", "USA\t\tNew York Mercantile Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".OB", "USA\t\tOTC Bulletin Board Market"));
            this.cmbPrefix.Items.Add(new Class24(".PK", "USA\t\tPink Sheets"));
            this.cmbPrefix.Items.Add(new Class24(".BA", "Argentina\tBuenos Aires Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".VI", "Austria\t\tVienna Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".AX", "Australia\tAustralian Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".SA", "Brazil\t\tSao Paolo Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".TO", "Canada\t\tToronto Stock Exchangee"));
            this.cmbPrefix.Items.Add(new Class24(".V", "Canada\t\tTSX Venture Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".SS", "China\t\tShanghai Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".SZ", "China\t\tShenzhen Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".CO", "Denmark\tCopenhagen Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".PA", "France\t\tParis Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".BE", "Germany\tBerlin Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".BM", "Germany\tBremen Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".DU", "Germany\tDusseldorf Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".F", "Germany\tFrankfurt Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".HM", "Germany\tHamburg Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".HA", "Germany\tHanover Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".MU", "Germany\tMunich Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".SG", "Germany\tStuttgart Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".DE", "Germany\tXETRA Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".HK", "Hong Kong\tHong Kong Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".BO", "India\t\tBombay Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".NS", "India\t\tNational Stock Exchange of India"));
            this.cmbPrefix.Items.Add(new Class24(".JK", "Indonesia\tJakarta Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".TA", "Israel\t\tTel Aviv Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".MI", "Italy\t\tMilan Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".KS", "South Korea\tKorea Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".KQ", "South Korea\tKOSDAQ"));
            this.cmbPrefix.Items.Add(new Class24(".MX", "Mexico\t\tMexico Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".AS", "Netherlands\tAmsterdam Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".NZ", "New Zealand\tNew Zealand Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".OL", "Norway\t\tOslo Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".SI", "Singapore\tSingapore Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".BC", "Spain\t\tBarcelona Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".BI", "Spain\t\tBilbao Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".MF", "Spain\t\tMadrid Fixed Income Market"));
            this.cmbPrefix.Items.Add(new Class24(".MC", "Spain\t\tMadrid SE C.A.T.S."));
            this.cmbPrefix.Items.Add(new Class24(".MA", "Spain\t\tMadrid Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".ST", "Sweden\t\tStockholm Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".SW", "Switzerland\tSwiss Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".TWO", "Taiwan\t\tTaiwan OTC Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".TW", "Taiwan\t\tTaiwan Stock Exchange"));
            this.cmbPrefix.Items.Add(new Class24(".L", "United Kingdom\tLondon Stock Exchange"));
        }
        this.txtSymbols.Clear();
        this.cmbPrefix.SelectedIndex = 0;
    }

    internal class Class24   ///WYJ note, surfix
    {
        public string string_0;
        public string string_1;

        public Class24(string string_2, string string_3)
        {
            this.string_0 = string_2;
            this.string_1 = string_3;
        }
    }
}

