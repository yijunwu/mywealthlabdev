using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WealthLab.Extensions;

internal class UpdateExtensionsForm : Form
{
    private Class47 brPanel;
    private Button btnStartWealthLab;
    private ExtensionManager extensionManager_0;
    private IContainer icontainer_0;
    private LinkLabel lnkCopy;
    private Class45 lstUpdateActions;

    public UpdateExtensionsForm(ExtensionManager extensionManager_1)
    {
        this.InitializeComponent();
        this.extensionManager_0 = extensionManager_1;
        this.extensionManager_0.UpdateExtensionAction += new ExtensionManager.UpdateExtensionActionEventHandler(this.extensionManager_0_UpdateExtensionAction);
        this.extensionManager_0.UpdateExtensionsCompleted += new ExtensionManager.UpdateExtensionsCompletedHandler(this.method_1);
        this.extensionManager_0.UpdateExtensionsStart += new ExtensionManager.UpdateExtensionsStartHandler(this.method_0);
    }

    private void btnStartWealthLab_Click(object sender, EventArgs e)
    {
        base.Close();
    }

    [DllImport("user32.dll")]
    private static extern IntPtr DeleteMenu(IntPtr intptr_0, int int_0, int int_1);
    private void extensionManager_0_UpdateExtensionAction(object sender, EventArgs6 e)
    {
        ListViewItem item = new ListViewItem {
            Text = e.string_1
        };
        string str = string.Empty;
        switch (e.enum6_0)
        {
            case Enum6.const_0:
                str = "Delete File";
                break;

            case Enum6.const_1:
                str = "Run Batch File";
                break;

            case Enum6.const_2:
                str = "Install File";
                break;
        }
        str = str + ": " + e.string_0;
        string str2 = string.Empty;
        switch (e.enum7_0)
        {
            case Enum7.const_0:
                str2 = "OK";
                break;

            case Enum7.const_1:
                str2 = e.string_2;
                break;
        }
        item.SubItems.AddRange(new string[] { str, str2 });
        if (e.enum7_0 == Enum7.const_1)
        {
            item.ForeColor = Color.Red;
            foreach (ListViewItem.ListViewSubItem item2 in item.SubItems)
            {
                item2.ForeColor = Color.Red;
            }
        }
        Class39 class2 = new Class39(e.string_1, e.enum6_0, e.enum7_0, e.string_0, e.string_2);
        UpdateActionControl control = new UpdateActionControl(class2);
        control.method_1();
        this.lstUpdateActions.method_7(control);
        Application.DoEvents();
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr intptr_0, bool bool_0);
    private void InitializeComponent()
    {
        ComponentResourceManager manager = new ComponentResourceManager(typeof(UpdateExtensionsForm));
        this.btnStartWealthLab = new Button();
        this.lnkCopy = new LinkLabel();
        this.brPanel = new Class47();
        this.lstUpdateActions = new Class45();
        this.brPanel.SuspendLayout();
        base.SuspendLayout();
        this.btnStartWealthLab.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.btnStartWealthLab.Enabled = false;
        this.btnStartWealthLab.Location = new Point(0x1f9, 0x14b);
        this.btnStartWealthLab.Name = "btnStartWealthLab";
        this.btnStartWealthLab.Size = new Size(120, 0x17);
        this.btnStartWealthLab.TabIndex = 5;
        this.btnStartWealthLab.Text = "Start Wealth-Lab";
        this.btnStartWealthLab.UseVisualStyleBackColor = true;
        this.btnStartWealthLab.Click += new EventHandler(this.btnStartWealthLab_Click);
        this.lnkCopy.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
        this.lnkCopy.AutoSize = true;
        this.lnkCopy.LinkBehavior = LinkBehavior.AlwaysUnderline;
        this.lnkCopy.Location = new Point(4, 0x150);
        this.lnkCopy.Name = "lnkCopy";
        this.lnkCopy.Size = new Size(90, 13);
        this.lnkCopy.TabIndex = 8;
        this.lnkCopy.TabStop = true;
        this.lnkCopy.Text = "Copy to Clipboard";
        this.lnkCopy.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkCopy_LinkClicked);
        this.brPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
        this.brPanel.BackColor = SystemColors.Window;
        this.brPanel.method_1(Color.FromArgb(0x7f, 0x9d, 0xb9));
        this.brPanel.Controls.Add(this.lstUpdateActions);
        this.brPanel.Location = new Point(6, 6);
        this.brPanel.Name = "brPanel";
        this.brPanel.Size = new Size(0x26b, 0x13d);
        this.brPanel.TabIndex = 7;
        this.lstUpdateActions.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
        this.lstUpdateActions.AutoScroll = true;
        this.lstUpdateActions.BackColor = SystemColors.Window;
        this.lstUpdateActions.Location = new Point(1, 1);
        this.lstUpdateActions.Name = "lstUpdateActions";
        this.lstUpdateActions.method_3(-1);
        this.lstUpdateActions.Size = new Size(0x269, 0x13b);
        this.lstUpdateActions.TabIndex = 6;
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.ClientSize = new Size(0x278, 0x16a);
        base.Controls.Add(this.lnkCopy);
        base.Controls.Add(this.brPanel);
        base.Controls.Add(this.btnStartWealthLab);
        base.Icon = (Icon) manager.GetObject("$this.Icon");
        base.MaximizeBox = false;
        base.MinimizeBox = false;
        this.MinimumSize = new Size(300, 200);
        base.Name = "UpdateExtensionsForm";
        base.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Scheduled Actions";
        base.FormClosing += new FormClosingEventHandler(this.UpdateExtensionsForm_FormClosing);
        this.brPanel.ResumeLayout(false);
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    private void lnkCopy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        StringBuilder builder = new StringBuilder();
        foreach (UpdateActionControl control in this.lstUpdateActions.Controls)
        {
            builder.AppendLine(control.ToString());
            builder.AppendLine("---------------------------");
        }
        try
        {
            Clipboard.SetText(builder.ToString());
        }
        catch
        {
        }
    }

    private void method_0(object object_0)
    {
    }

    private void method_1(object object_0)
    {
        this.btnStartWealthLab.Enabled = true;
    }

    void Form.Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_0 != null))
        {
            this.icontainer_0.Dispose();
        }
        base.Dispose(disposing);
    }

    private void UpdateExtensionsForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!this.btnStartWealthLab.Enabled)
        {
            e.Cancel = true;
        }
    }
}

