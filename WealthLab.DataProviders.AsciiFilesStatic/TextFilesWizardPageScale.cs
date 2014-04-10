using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using GroupBox = System.Windows.Forms.GroupBox;

internal class TextFilesWizardPageScale : UserControl
{
    private ComboBox cmbScale;
    private GroupBox grpScale;
    private IContainer components;
    private int int_0;
    private int int_1;
    private Label lblInterval;
    private Label lblScale;
    private NumericUpDown nmrInterval;

    public TextFilesWizardPageScale()
    {
        this.InitializeComponent();
    }

    private void cmbScale_SelectedIndexChanged(object sender, EventArgs e)
    {
        int[] numArray = new int[] { 5, 4, 3, 0, 1, 2, 6, 7 };
        this.int_1 = numArray[(sender as ComboBox).SelectedIndex];
        if ((sender as ComboBox).SelectedIndex > 2)
        {
            this.nmrInterval.Value = 1M;
            this.lblInterval.Enabled = false;
            this.nmrInterval.Enabled = false;
        }
        else
        {
            this.lblInterval.Enabled = true;
            this.nmrInterval.Enabled = true;
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
        this.grpScale = new GroupBox();
        this.nmrInterval = new NumericUpDown();
        this.lblInterval = new Label();
        this.lblScale = new Label();
        this.cmbScale = new ComboBox();
        this.grpScale.SuspendLayout();
        this.nmrInterval.BeginInit();
        base.SuspendLayout();
        this.grpScale.Controls.Add(this.nmrInterval);
        this.grpScale.Controls.Add(this.lblInterval);
        this.grpScale.Controls.Add(this.lblScale);
        this.grpScale.Controls.Add(this.cmbScale);
        this.grpScale.Location = new Point(7, 7);
        this.grpScale.Name = "grpScale";
        this.grpScale.Size = new Size(0x225, 0x4f);
        this.grpScale.TabIndex = 4;
        this.grpScale.TabStop = false;
        this.grpScale.Text = "Choose Bar scale and interval";
        this.nmrInterval.Location = new Point(0x7c, 0x31);
        int[] bits = new int[4];
        bits[0] = 0x270f;
        this.nmrInterval.Maximum = new decimal(bits);
        int[] numArray2 = new int[4];
        numArray2[0] = 1;
        this.nmrInterval.Minimum = new decimal(numArray2);
        this.nmrInterval.Name = "nmrInterval";
        this.nmrInterval.Size = new Size(60, 20);
        this.nmrInterval.TabIndex = 2;
        int[] numArray3 = new int[4];
        numArray3[0] = 1;
        this.nmrInterval.Value = new decimal(numArray3);
        this.nmrInterval.ValueChanged += new EventHandler(this.nmrInterval_ValueChanged);
        this.lblInterval.AutoSize = true;
        this.lblInterval.Location = new Point(7, 0x33);
        this.lblInterval.Name = "lblInterval";
        this.lblInterval.Size = new Size(60, 13);
        this.lblInterval.TabIndex = 2;
        this.lblInterval.Text = "Bar interval";
        this.lblScale.AutoSize = true;
        this.lblScale.Location = new Point(7, 0x16);
        this.lblScale.Name = "lblScale";
        this.lblScale.Size = new Size(0x33, 13);
        this.lblScale.TabIndex = 1;
        this.lblScale.Text = "Bar scale";
        this.cmbScale.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbScale.FormattingEnabled = true;
        this.cmbScale.Items.AddRange(new object[] { "Tick", "Second", "Minute", "Daily", "Weekly", "Monthly", "Quarterly", "Yearly" });
        this.cmbScale.Location = new Point(0x7c, 0x13);
        this.cmbScale.Name = "cmbScale";
        this.cmbScale.Size = new Size(0x77, 0x15);
        this.cmbScale.TabIndex = 1;
        this.cmbScale.SelectedIndexChanged += new EventHandler(this.cmbScale_SelectedIndexChanged);
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.Controls.Add(this.grpScale);
        base.Name = "TextFilesWizardPageScale";
        base.Size = new Size(560, 0x161);
        this.grpScale.ResumeLayout(false);
        this.grpScale.PerformLayout();
        this.nmrInterval.EndInit();
        base.ResumeLayout(false);
    }

    public int method_0()
    {
        if (this.cmbScale.SelectedIndex > 2)
        {
            return 0;
        }
        return this.int_0;
    }

    public int method_1()
    {
        return this.int_1;
    }

    public void method_2()
    {
        this.cmbScale.SelectedIndex = 3;
        this.int_0 = 1;
        this.nmrInterval.Value = 1;
    }

    private void nmrInterval_ValueChanged(object sender, EventArgs e)
    {
        this.int_0 = (int) (sender as NumericUpDown).Value;
    }
}

