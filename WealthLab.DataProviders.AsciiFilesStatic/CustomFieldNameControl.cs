using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Button = System.Windows.Forms.Button;

internal class CustomFieldNameControl : UserControl
{
    private Button btnFieldName;
    private IContainer components;
    private Label lblFieldName;
    private TextBox txtFieldName;

    public CustomFieldNameControl()
    {
        this.InitializeComponent();
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
        this.btnFieldName = new Button();
        this.txtFieldName = new TextBox();
        this.lblFieldName = new Label();
        base.SuspendLayout();
        this.btnFieldName.Location = new Point(0x114, 0);
        this.btnFieldName.Name = "btnFieldName";
        this.btnFieldName.Size = new Size(0x24, 0x15);
        this.btnFieldName.TabIndex = 5;
        this.btnFieldName.Text = "OK";
        this.btnFieldName.UseVisualStyleBackColor = true;
        this.txtFieldName.Location = new Point(0x51, 0);
        this.txtFieldName.Name = "txtFieldName";
        this.txtFieldName.Size = new Size(0xbd, 20);
        this.txtFieldName.TabIndex = 4;
        this.lblFieldName.AutoSize = true;
        this.lblFieldName.Location = new Point(12, 3);
        this.lblFieldName.Name = "lblFieldName";
        this.lblFieldName.Size = new Size(0x3f, 13);
        this.lblFieldName.TabIndex = 3;
        this.lblFieldName.Text = "Field Name:";
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = SystemColors.Menu;
        base.Controls.Add(this.btnFieldName);
        base.Controls.Add(this.txtFieldName);
        base.Controls.Add(this.lblFieldName);
        base.Name = "CustomFieldNameControl";
        base.Size = new Size(320, 20);
        base.ResumeLayout(false);
        base.PerformLayout();
    }
}

