using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WealthLab;

internal class PageScale : UserControl
{
    private ComboBox cmbScale;
    private GroupBox grpScale;
    private IContainer icontainer_0;
    private Label lblChooseScale;
    private Label lblInterval;
    private Label lblScale;
    private NumericUpDown numInterval;

    public PageScale()
    {
        this.InitializeComponent();
    }

    private void cmbScale_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (((this.method_3() != BarScale.Minute) && (this.method_3() != BarScale.Second)) && (this.method_3() != BarScale.Tick))
        {
            this.numInterval.Enabled = false;
            this.numInterval.Value = 1M;
        }
        else
        {
            this.numInterval.Enabled = true;
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
        this.grpScale = new GroupBox();
        this.cmbScale = new ComboBox();
        this.lblChooseScale = new Label();
        this.numInterval = new NumericUpDown();
        this.lblInterval = new Label();
        this.lblScale = new Label();
        this.grpScale.SuspendLayout();
        this.numInterval.BeginInit();
        base.SuspendLayout();
        this.grpScale.Controls.Add(this.cmbScale);
        this.grpScale.Controls.Add(this.lblChooseScale);
        this.grpScale.Controls.Add(this.numInterval);
        this.grpScale.Controls.Add(this.lblInterval);
        this.grpScale.Controls.Add(this.lblScale);
        this.grpScale.Location = new Point(7, 7);
        this.grpScale.Name = "grpScale";
        this.grpScale.RightToLeft = RightToLeft.No;
        this.grpScale.Size = new Size(0x225, 0x157);
        this.grpScale.TabIndex = 5;
        this.grpScale.TabStop = false;
        this.grpScale.Text = "Bar scale and interval";
        this.cmbScale.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbScale.FormattingEnabled = true;
        this.cmbScale.Items.AddRange(new object[] { "Tick", "Second", "Minute", "Daily", "Weekly", "Monthly" });
        this.cmbScale.Location = new Point(0x76, 0x29);
        this.cmbScale.Name = "cmbScale";
        this.cmbScale.Size = new Size(0x77, 0x15);
        this.cmbScale.TabIndex = 4;
        this.cmbScale.SelectedIndexChanged += new EventHandler(this.cmbScale_SelectedIndexChanged);
        this.lblChooseScale.AutoSize = true;
        this.lblChooseScale.Location = new Point(6, 0x13);
        this.lblChooseScale.Name = "lblChooseScale";
        this.lblChooseScale.Size = new Size(0xd6, 13);
        this.lblChooseScale.TabIndex = 3;
        this.lblChooseScale.Text = "Specify bar scalе and interval of source files";
        this.numInterval.Location = new Point(0x76, 0x44);
        int[] bits = new int[4];
        bits[0] = 0x270f;
        this.numInterval.Maximum = new decimal(bits);
        int[] numArray2 = new int[4];
        numArray2[0] = 1;
        this.numInterval.Minimum = new decimal(numArray2);
        this.numInterval.Name = "numInterval";
        this.numInterval.Size = new Size(60, 20);
        this.numInterval.TabIndex = 2;
        int[] numArray3 = new int[4];
        numArray3[0] = 1;
        this.numInterval.Value = new decimal(numArray3);
        this.lblInterval.AutoSize = true;
        this.lblInterval.Location = new Point(6, 70);
        this.lblInterval.Name = "lblInterval";
        this.lblInterval.Size = new Size(60, 13);
        this.lblInterval.TabIndex = 2;
        this.lblInterval.Text = "Bar interval";
        this.lblScale.AutoSize = true;
        this.lblScale.Location = new Point(6, 0x2c);
        this.lblScale.Name = "lblScale";
        this.lblScale.Size = new Size(0x33, 13);
        this.lblScale.TabIndex = 1;
        this.lblScale.Text = "Bar scale";
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.grpScale);
        base.Name = "PageScale";
        this.RightToLeft = RightToLeft.Yes;
        base.Size = new Size(560, 0x161);
        this.grpScale.ResumeLayout(false);
        this.grpScale.PerformLayout();
        this.numInterval.EndInit();
        base.ResumeLayout(false);
    }

    public int method_0()
    {
        switch (this.method_3())
        {
            case BarScale.Minute:
            case BarScale.Second:
            case BarScale.Tick:
                return Convert.ToInt32(this.numInterval.Value);
        }
        return 0;
    }

    public void method_1(int int_0)
    {
        this.numInterval.Value = int_0;
    }

    public void method_2()
    {
        this.cmbScale.SelectedIndex = 3;
        this.numInterval.Value = 1M;
    }

    public BarScale method_3()
    {
        return (BarScale) Enum.Parse(typeof(BarScale), this.cmbScale.Text);
    }

    public void method_4(BarScale barScale_0)
    {
        this.cmbScale.Text = Enum.GetName(typeof(BarScale), barScale_0);
    }
}

