using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

internal class FormatName : Form
{
    private Button btnCancel;
    private Button btnOK;
    private IContainer icontainer_0;
    private Label lblFormatName;
    private List<string> list_0;
    private TextBox txtFormatName;

    public FormatName()
    {
        this.InitializeComponent();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        if (!this.list_0.Contains(this.txtFormatName.Text) || (MessageBox.Show("A format named " + this.txtFormatName.Text.ToString() + " already exists, overwrite?", "Format already exists.", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.Cancel))
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }
    }

    private void FormatName_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            this.btnOK.PerformClick();
        }
        if (e.KeyCode == Keys.Escape)
        {
            this.btnCancel.PerformClick();
        }
    }

    private void InitializeComponent()
    {
        this.lblFormatName = new Label();
        this.txtFormatName = new TextBox();
        this.btnCancel = new Button();
        this.btnOK = new Button();
        base.SuspendLayout();
        this.lblFormatName.AutoSize = true;
        this.lblFormatName.Location = new Point(12, 9);
        this.lblFormatName.Name = "lblFormatName";
        this.lblFormatName.Size = new Size(0x75, 13);
        this.lblFormatName.TabIndex = 0;
        this.lblFormatName.Text = "Common Format Name:";
        this.txtFormatName.Location = new Point(15, 0x19);
        this.txtFormatName.Name = "txtFormatName";
        this.txtFormatName.Size = new Size(260, 20);
        this.txtFormatName.TabIndex = 0;
        this.btnCancel.DialogResult = DialogResult.Cancel;
        this.btnCancel.Location = new Point(180, 0x36);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(0x5f, 0x19);
        this.btnCancel.TabIndex = 2;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnOK.Location = new Point(0x4f, 0x36);
        this.btnOK.Name = "btnOK";
        this.btnOK.Size = new Size(0x5f, 0x19);
        this.btnOK.TabIndex = 1;
        this.btnOK.Text = "OK";
        this.btnOK.UseVisualStyleBackColor = true;
        this.btnOK.Click += new EventHandler(this.btnOK_Click);
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.CancelButton = this.btnCancel;
        base.ClientSize = new Size(290, 0x59);
        base.Controls.Add(this.btnOK);
        base.Controls.Add(this.btnCancel);
        base.Controls.Add(this.txtFormatName);
        base.Controls.Add(this.lblFormatName);
        base.FormBorderStyle = FormBorderStyle.FixedDialog;
        base.KeyPreview = true;
        base.MaximizeBox = false;
        base.MinimizeBox = false;
        base.Name = "FormatName";
        base.ShowInTaskbar = false;
        base.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Save Common Format";
        base.KeyDown += new KeyEventHandler(this.FormatName_KeyDown);
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    public string method_0()
    {
        return this.txtFormatName.Text;
    }

    public void method_1(List<string> list_1)
    {
        this.list_0 = list_1;
    }

    /// <summary>
    /// ///WYJ fix
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_0 != null))
        {
            this.icontainer_0.Dispose();
        }
        base.Dispose(disposing);
    }
}

