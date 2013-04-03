using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

internal class AboutBox1 : Form
{
    private IContainer icontainer_0;
    private Label labelCompanyName;
    private Label labelCopyright;
    private Label labelProductName;
    private Label labelVersion;
    private PictureBox logoPictureBox;
    private Button okButton;
    private TableLayoutPanel tableLayoutPanel;
    private TextBox textBoxDescription;

    public AboutBox1()
    {
        this.InitializeComponent();
        this.Text = string.Format("About {0}", this.method_0());
        this.labelProductName.Text = this.method_3();
        this.labelVersion.Text = "Version " + this.method_1();
        this.labelCopyright.Text = this.method_4();
        this.labelCompanyName.Text = this.method_5();
        this.textBoxDescription.Text = this.method_2();
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
        ComponentResourceManager manager = new ComponentResourceManager(typeof(AboutBox1));
        this.tableLayoutPanel = new TableLayoutPanel();
        this.logoPictureBox = new PictureBox();
        this.labelProductName = new Label();
        this.labelVersion = new Label();
        this.labelCopyright = new Label();
        this.labelCompanyName = new Label();
        this.textBoxDescription = new TextBox();
        this.okButton = new Button();
        this.tableLayoutPanel.SuspendLayout();
        ((ISupportInitialize) this.logoPictureBox).BeginInit();
        base.SuspendLayout();
        this.tableLayoutPanel.ColumnCount = 2;
        this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
        this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67f));
        this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
        this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
        this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
        this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 2);
        this.tableLayoutPanel.Controls.Add(this.labelCompanyName, 1, 3);
        this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 4);
        this.tableLayoutPanel.Controls.Add(this.okButton, 1, 5);
        this.tableLayoutPanel.Dock = DockStyle.Fill;
        this.tableLayoutPanel.Location = new Point(9, 9);
        this.tableLayoutPanel.Name = "tableLayoutPanel";
        this.tableLayoutPanel.RowCount = 6;
        this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
        this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
        this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
        this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
        this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
        this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
        this.tableLayoutPanel.Size = new Size(0x1a1, 0x109);
        this.tableLayoutPanel.TabIndex = 0;
        this.logoPictureBox.Dock = DockStyle.Fill;
        this.logoPictureBox.Image = (Image) manager.GetObject("logoPictureBox.Image");
        this.logoPictureBox.Location = new Point(3, 3);
        this.logoPictureBox.Name = "logoPictureBox";
        this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 6);
        this.logoPictureBox.Size = new Size(0x83, 0x103);
        this.logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        this.logoPictureBox.TabIndex = 12;
        this.logoPictureBox.TabStop = false;
        this.labelProductName.Dock = DockStyle.Fill;
        this.labelProductName.Location = new Point(0x8f, 0);
        this.labelProductName.Margin = new Padding(6, 0, 3, 0);
        this.labelProductName.MaximumSize = new Size(0, 0x11);
        this.labelProductName.Name = "labelProductName";
        this.labelProductName.Size = new Size(0x10f, 0x11);
        this.labelProductName.TabIndex = 0x13;
        this.labelProductName.Text = "Product Name";
        this.labelProductName.TextAlign = ContentAlignment.MiddleLeft;
        this.labelVersion.Dock = DockStyle.Fill;
        this.labelVersion.Location = new Point(0x8f, 0x1a);
        this.labelVersion.Margin = new Padding(6, 0, 3, 0);
        this.labelVersion.MaximumSize = new Size(0, 0x11);
        this.labelVersion.Name = "labelVersion";
        this.labelVersion.Size = new Size(0x10f, 0x11);
        this.labelVersion.TabIndex = 0;
        this.labelVersion.Text = "Version";
        this.labelVersion.TextAlign = ContentAlignment.MiddleLeft;
        this.labelCopyright.Dock = DockStyle.Fill;
        this.labelCopyright.Location = new Point(0x8f, 0x34);
        this.labelCopyright.Margin = new Padding(6, 0, 3, 0);
        this.labelCopyright.MaximumSize = new Size(0, 0x11);
        this.labelCopyright.Name = "labelCopyright";
        this.labelCopyright.Size = new Size(0x10f, 0x11);
        this.labelCopyright.TabIndex = 0x15;
        this.labelCopyright.Text = "Copyright";
        this.labelCopyright.TextAlign = ContentAlignment.MiddleLeft;
        this.labelCompanyName.Dock = DockStyle.Fill;
        this.labelCompanyName.Location = new Point(0x8f, 0x4e);
        this.labelCompanyName.Margin = new Padding(6, 0, 3, 0);
        this.labelCompanyName.MaximumSize = new Size(0, 0x11);
        this.labelCompanyName.Name = "labelCompanyName";
        this.labelCompanyName.Size = new Size(0x10f, 0x11);
        this.labelCompanyName.TabIndex = 0x16;
        this.labelCompanyName.Text = "Company Name";
        this.labelCompanyName.TextAlign = ContentAlignment.MiddleLeft;
        this.textBoxDescription.Dock = DockStyle.Fill;
        this.textBoxDescription.Location = new Point(0x8f, 0x6b);
        this.textBoxDescription.Margin = new Padding(6, 3, 3, 3);
        this.textBoxDescription.Multiline = true;
        this.textBoxDescription.Name = "textBoxDescription";
        this.textBoxDescription.ReadOnly = true;
        this.textBoxDescription.ScrollBars = ScrollBars.Both;
        this.textBoxDescription.Size = new Size(0x10f, 0x7e);
        this.textBoxDescription.TabIndex = 0x17;
        this.textBoxDescription.TabStop = false;
        this.textBoxDescription.Text = "Description";
        this.okButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.okButton.DialogResult = DialogResult.Cancel;
        this.okButton.Location = new Point(0x153, 0xef);
        this.okButton.Name = "okButton";
        this.okButton.Size = new Size(0x4b, 0x17);
        this.okButton.TabIndex = 0x18;
        this.okButton.Text = "&OK";
        base.AcceptButton = this.okButton;
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.ClientSize = new Size(0x1b3, 0x11b);
        base.Controls.Add(this.tableLayoutPanel);
        base.FormBorderStyle = FormBorderStyle.FixedDialog;
        base.MaximizeBox = false;
        base.MinimizeBox = false;
        base.Name = "AboutBox1";
        base.Padding = new Padding(9);
        base.ShowIcon = false;
        base.ShowInTaskbar = false;
        base.StartPosition = FormStartPosition.CenterParent;
        this.Text = "AboutBox1";
        this.tableLayoutPanel.ResumeLayout(false);
        this.tableLayoutPanel.PerformLayout();
        ((ISupportInitialize) this.logoPictureBox).EndInit();
        base.ResumeLayout(false);
    }

    public string method_0()
    {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
        if (customAttributes.Length > 0)
        {
            AssemblyTitleAttribute attribute = (AssemblyTitleAttribute) customAttributes[0];
            if (attribute.Title != "")
            {
                return attribute.Title;
            }
        }
        return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
    }

    public string method_1()
    {
        string str = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        if (IntPtr.Size == 8)
        {
            return (str + "    64-bit Edition");
        }
        return str;
    }

    public string method_2()
    {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
        if (customAttributes.Length == 0)
        {
            return "";
        }
        return ((AssemblyDescriptionAttribute) customAttributes[0]).Description;
    }

    public string method_3()
    {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
        if (customAttributes.Length == 0)
        {
            return "";
        }
        return ((AssemblyProductAttribute) customAttributes[0]).Product;
    }

    public string method_4()
    {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
        if (customAttributes.Length == 0)
        {
            return "";
        }
        return ((AssemblyCopyrightAttribute) customAttributes[0]).Copyright;
    }

    public string method_5()
    {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
        if (customAttributes.Length == 0)
        {
            return "";
        }
        return ((AssemblyCompanyAttribute) customAttributes[0]).Company;
    }
}

