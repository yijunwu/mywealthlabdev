using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

internal class YahooWizardPageStart : UserControl
{
    private CheckBox cbUpdateGroup;
    private DateTimePicker dtStartingDate;
    private GroupBox grpOptions;
    private IContainer icontainer_0;
    private Label lblOptions;
    private Label lblStartingDate;
    private Label lblStartingDateDesc;
    private Label lblVersion;
    private RadioButton rbClassification;
    private RadioButton rbManual;

    public YahooWizardPageStart()
    {
        this.InitializeComponent();
        this.lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }

    private void cbUpdateGroup_CheckedChanged(object sender, EventArgs e)
    {
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
        this.grpOptions = new GroupBox();
        this.lblStartingDateDesc = new Label();
        this.dtStartingDate = new DateTimePicker();
        this.lblStartingDate = new Label();
        this.cbUpdateGroup = new CheckBox();
        this.rbClassification = new RadioButton();
        this.rbManual = new RadioButton();
        this.lblOptions = new Label();
        this.lblVersion = new Label();
        this.grpOptions.SuspendLayout();
        base.SuspendLayout();
        this.grpOptions.Controls.Add(this.lblStartingDateDesc);
        this.grpOptions.Controls.Add(this.dtStartingDate);
        this.grpOptions.Controls.Add(this.lblStartingDate);
        this.grpOptions.Controls.Add(this.cbUpdateGroup);
        this.grpOptions.Controls.Add(this.rbClassification);
        this.grpOptions.Controls.Add(this.rbManual);
        this.grpOptions.Controls.Add(this.lblOptions);
        this.grpOptions.Location = new Point(7, 7);
        this.grpOptions.Name = "grpOptions";
        this.grpOptions.RightToLeft = RightToLeft.No;
        this.grpOptions.Size = new Size(550, 0x14c);
        this.grpOptions.TabIndex = 0;
        this.grpOptions.TabStop = false;
        this.grpOptions.Text = "Yahoo Dataset Options";
        this.lblStartingDateDesc.AutoSize = true;
        this.lblStartingDateDesc.Location = new Point(6, 0x6a);
        this.lblStartingDateDesc.Name = "lblStartingDateDesc";
        this.lblStartingDateDesc.Size = new Size(0x147, 13);
        this.lblStartingDateDesc.TabIndex = 8;
        this.lblStartingDateDesc.Text = "Which date would you like to use as starting date of collected data?";
        this.dtStartingDate.Format = DateTimePickerFormat.Short;
        this.dtStartingDate.Location = new Point(0x52, 0x7f);
        this.dtStartingDate.Name = "dtStartingDate";
        this.dtStartingDate.Size = new Size(0x70, 20);
        this.dtStartingDate.TabIndex = 7;
        this.dtStartingDate.Value = new DateTime(0x7d0, 1, 1, 0, 0, 0, 0);
        this.lblStartingDate.AutoSize = true;
        this.lblStartingDate.Location = new Point(6, 0x83);
        this.lblStartingDate.Name = "lblStartingDate";
        this.lblStartingDate.Size = new Size(70, 13);
        this.lblStartingDate.TabIndex = 6;
        this.lblStartingDate.Text = "Starting date:";
        this.cbUpdateGroup.AutoSize = true;
        this.cbUpdateGroup.Location = new Point(0x1f, 0x56);
        this.cbUpdateGroup.Name = "cbUpdateGroup";
        this.cbUpdateGroup.Size = new Size(0x149, 0x11);
        this.cbUpdateGroup.TabIndex = 2;
        this.cbUpdateGroup.Text = "Update DataSet composition when Classification group changes";
        this.cbUpdateGroup.UseVisualStyleBackColor = true;
        this.cbUpdateGroup.Visible = false;
        this.cbUpdateGroup.CheckedChanged += new EventHandler(this.cbUpdateGroup_CheckedChanged);
        this.rbClassification.AutoSize = true;
        this.rbClassification.Location = new Point(11, 0x3f);
        this.rbClassification.Name = "rbClassification";
        this.rbClassification.Size = new Size(290, 0x11);
        this.rbClassification.TabIndex = 1;
        this.rbClassification.TabStop = true;
        this.rbClassification.Text = "Select the Symbols from predefined Classification groups";
        this.rbClassification.UseVisualStyleBackColor = true;
        this.rbClassification.CheckedChanged += new EventHandler(this.rbClassification_CheckedChanged);
        this.rbManual.AutoSize = true;
        this.rbManual.Location = new Point(11, 40);
        this.rbManual.Name = "rbManual";
        this.rbManual.Size = new Size(0x121, 0x11);
        this.rbManual.TabIndex = 0;
        this.rbManual.TabStop = true;
        this.rbManual.Text = "Enter symbols manually or paste them from the Clipboard";
        this.rbManual.UseVisualStyleBackColor = true;
        this.lblOptions.AutoSize = true;
        this.lblOptions.Location = new Point(6, 0x13);
        this.lblOptions.Name = "lblOptions";
        this.lblOptions.Size = new Size(0x15a, 13);
        this.lblOptions.TabIndex = 0;
        this.lblOptions.Text = "How do you want to select the Symbols that will make up your DataSet?";
        this.lblVersion.AutoSize = true;
        this.lblVersion.Font = new Font("Microsoft Sans Serif", 6.5f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.lblVersion.Location = new Point(0x20d, 0x156);
        this.lblVersion.Name = "lblVersion";
        this.lblVersion.RightToLeft = RightToLeft.Yes;
        this.lblVersion.Size = new Size(0x22, 12);
        this.lblVersion.TabIndex = 9;
        this.lblVersion.Text = "1.0.2.3";
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.lblVersion);
        base.Controls.Add(this.grpOptions);
        base.Name = "YahooWizardPageStart";
        this.RightToLeft = RightToLeft.Yes;
        base.Size = new Size(560, 0x161);
        this.grpOptions.ResumeLayout(false);
        this.grpOptions.PerformLayout();
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    public void InitStates()
    {
        this.rbManual.Checked = true;
        this.cbUpdateGroup.Checked = false;
        this.cbUpdateGroup.Enabled = false;
    }

    ///WYJ fix, original name: method_1
    public DateTime optStartDate()
    {
        return this.dtStartingDate.Value;
    }

    public bool optChooseFromClassification()
    {
        return this.rbClassification.Checked;
    }

    public bool optUpdateGroup()
    {
        return this.cbUpdateGroup.Checked;  ///the check box is unchecked and invisible
    }

    ///WYJ fix, original name: method_4
    private void showDialog(object sender, EventArgs e)
    {
        new ProviderSettingsForm().ShowDialog(this);
    }

    private void rbClassification_CheckedChanged(object sender, EventArgs e)
    {
        this.cbUpdateGroup.Enabled = (sender as RadioButton).Checked;
    }
}

