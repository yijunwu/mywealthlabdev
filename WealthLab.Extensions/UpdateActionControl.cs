using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using WealthLab.Extensions.Properties;
using Label = System.Windows.Forms.Label;

internal class UpdateActionControl : UserControl
{
    private bool bool_0;
    private Class39 class39_0;
    private IContainer icontainer_0;
    private Label lblAction;
    private Label lblActionDesc;
    private Label lblName;
    private Label lblResult;
    private Label lblResultDesc;
    private Pen pen_0;
    private PictureBox picResult;

    public UpdateActionControl()
    {
        this.pen_0 = new Pen(Color.Gray, 1f);
        this.InitializeComponent();
    }

    public UpdateActionControl(Class39 class39_1) : this()
    {
        this.class39_0 = class39_1;
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
        this.lblName = new Label();
        this.lblAction = new Label();
        this.lblResult = new Label();
        this.picResult = new PictureBox();
        this.lblActionDesc = new Label();
        this.lblResultDesc = new Label();
        ((ISupportInitialize) this.picResult).BeginInit();
        base.SuspendLayout();
        this.lblName.AutoSize = true;
        this.lblName.BackColor = Color.Transparent;
        this.lblName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblName.ForeColor = Color.RoyalBlue;
        this.lblName.Location = new Point(3, 7);
        this.lblName.Name = "lblName";
        this.lblName.Size = new Size(0x34, 13);
        this.lblName.TabIndex = 1;
        this.lblName.Text = "lblName";
        this.lblName.UseMnemonic = false;
        this.lblAction.AutoSize = true;
        this.lblAction.BackColor = Color.Transparent;
        this.lblAction.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblAction.Location = new Point(3, 0x1a);
        this.lblAction.Name = "lblAction";
        this.lblAction.Size = new Size(0x2f, 13);
        this.lblAction.TabIndex = 2;
        this.lblAction.Text = "Action:";
        this.lblResult.AutoSize = true;
        this.lblResult.BackColor = Color.Transparent;
        this.lblResult.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblResult.Location = new Point(3, 0x2c);
        this.lblResult.Name = "lblResult";
        this.lblResult.Size = new Size(0x2f, 13);
        this.lblResult.TabIndex = 3;
        this.lblResult.Text = "Result:";
        this.picResult.BackColor = Color.Transparent;
        this.picResult.Location = new Point(50, 0x2b);
        this.picResult.Name = "picResult";
        this.picResult.Size = new Size(0x10, 0x10);
        this.picResult.TabIndex = 11;
        this.picResult.TabStop = false;
        this.lblActionDesc.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
        this.lblActionDesc.AutoEllipsis = true;
        this.lblActionDesc.Location = new Point(0x31, 0x1a);
        this.lblActionDesc.Name = "lblActionDesc";
        this.lblActionDesc.Size = new Size(0x1de, 13);
        this.lblActionDesc.TabIndex = 12;
        this.lblActionDesc.Text = "lblActionDesc";
        this.lblResultDesc.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
        this.lblResultDesc.AutoEllipsis = true;
        this.lblResultDesc.Location = new Point(0x44, 0x2c);
        this.lblResultDesc.Name = "lblResultDesc";
        this.lblResultDesc.Size = new Size(0x1cb, 13);
        this.lblResultDesc.TabIndex = 13;
        this.lblResultDesc.Text = "lblResultDesc";
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = Color.Transparent;
        base.Controls.Add(this.lblResultDesc);
        base.Controls.Add(this.lblActionDesc);
        base.Controls.Add(this.picResult);
        base.Controls.Add(this.lblResult);
        base.Controls.Add(this.lblAction);
        base.Controls.Add(this.lblName);
        base.Name = "UpdateActionControl";
        base.Size = new Size(530, 0x42);
        base.Paint += new PaintEventHandler(this.UpdateActionControl_Paint);
        ((ISupportInitialize) this.picResult).EndInit();
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    public Class39 method_0()
    {
        return this.class39_0;
    }

    public void method_1()
    {
        this.bool_0 = true;
        this.lblName.Text = this.class39_0.method_0();
        switch (this.class39_0.method_2())
        {
            case Enum6.const_0:
                this.lblActionDesc.Text = "Delete File";
                break;

            case Enum6.const_1:
                this.lblActionDesc.Text = "Run Batch File";
                break;

            case Enum6.const_2:
                this.lblActionDesc.Text = "Install File";
                break;
        }
        this.lblActionDesc.Text = this.lblActionDesc.Text + ": " + this.class39_0.method_8();
        switch (this.class39_0.method_4())
        {
            case Enum7.const_0:
                this.picResult.Image = Resources.accept;
                this.lblResultDesc.Text = "Succeeded";
                return;

            case Enum7.const_1:
                this.picResult.Image = Resources.exclamation;
                this.lblResultDesc.ForeColor = Color.Brown;
                this.lblResultDesc.Text = "Failed";
                if (!string.IsNullOrEmpty(this.class39_0.method_6()))
                {
                    this.lblResultDesc.Text = this.lblResultDesc.Text + ": " + this.class39_0.method_6();
                }
                return;
        }
    }

    public override string ToString()
    {
        if (!this.bool_0)
        {
            return "Not Initialized";
        }
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(this.lblName.Text);
        builder.AppendLine(this.lblActionDesc.Text);
        builder.AppendLine(this.lblResultDesc.Text);
        return builder.ToString();
    }

    private void UpdateActionControl_Paint(object sender, PaintEventArgs e)
    {
        Rectangle clientRectangle = (sender as Control).ClientRectangle;
        this.pen_0.DashStyle = DashStyle.Dot;
        e.Graphics.DrawLine(this.pen_0, clientRectangle.X, clientRectangle.Bottom - 1, clientRectangle.X + clientRectangle.Width, clientRectangle.Bottom - 1);
    }
}

