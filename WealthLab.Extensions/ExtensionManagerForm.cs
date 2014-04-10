using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WealthLab.Extensions;
using WealthLab.Extensions.Attribute;

internal class ExtensionManagerForm : Form
{
    private bool bool_0;
    private Class40 class40_0;
    public static ExtensionManagerForm extensionManagerForm_0;
    private IContainer icontainer_0;
    private Label label1;
    private Label lblInfo;
    private LinkLabel linkMoreExtensions;
    private LinkLabel lnkOpenWleFile;
    private Class45 lstExtensions;
    private ToolStripMenuItem mniCheckUpdates;
    private OpenFileDialog openFileDialog_0;
    private Class47 pnlExtensions;
    private Panel pnlMain;
    private ToolStripComboBox tcbShow;
    private ToolStripLabel tlblShow;
    private ToolStripButton tlsbtnRestartWld;
    private ToolStrip toolbarExtensions;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton tsbtnAddins;
    private ToolStripButton tsbtnIndicators;
    private ToolStripButton tsbtnInstalls;
    private ToolStripDropDownButton tsbtnOptions;
    private ToolStripButton tsbtnProviders;
    private ToolStripButton tsbtnStrategies;
    private ToolStripButton tsbtnUpdates;
    private ToolStripSeparator tsep2;

    public ExtensionManagerForm()
    {
        base.HandleCreated += new EventHandler(this.ExtensionManagerForm_HandleCreated);
        this.InitializeComponent();
        this.method_1(ExtensionManager.HostApp);
        this.lblInfo.BackColor = Color.Transparent;
        this.lblInfo.Visible = false;
        this.method_6();
        this.method_15();
    }

    private void ExtensionManagerForm_DragDrop(object sender, DragEventArgs e)
    {
        string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop, true);
        this.class40_0.method_17(data[0]);
    }

    private void ExtensionManagerForm_DragEnter(object sender, DragEventArgs e)
    {
        if ((this.class40_0.method_12() == Enum10.const_2) && e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop, true);
            if (Path.GetExtension(data[0]).ToLower() == ".wle")
            {
                base.Activate();
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }
    }

    private void ExtensionManagerForm_FormClosed(object sender, FormClosedEventArgs e)
    {
    }

    private void ExtensionManagerForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            base.Hide();
        }
    }

    private void ExtensionManagerForm_HandleCreated(object sender, EventArgs e)
    {
        extensionManagerForm_0 = this;
    }

    private void ExtensionManagerForm_Resize(object sender, EventArgs e)
    {
        this.method_6();
    }

    private void ExtensionManagerForm_Shown(object sender, EventArgs e)
    {
        this.mniCheckUpdates.Checked = ExtensionManager.Config.CheckUpdates.Value;
        this.method_1(ExtensionManager.HostApp);
        Application.DoEvents();
    }

    private void InitializeComponent()
    {
        ComponentResourceManager resources = new ComponentResourceManager(typeof(ExtensionManagerForm));
        this.toolbarExtensions = new ToolStrip();
        this.tsbtnAddins = new ToolStripButton();
        this.tsbtnProviders = new ToolStripButton();
        this.tsbtnStrategies = new ToolStripButton();
        this.tsbtnIndicators = new ToolStripButton();
        this.tsbtnOptions = new ToolStripDropDownButton();
        this.mniCheckUpdates = new ToolStripMenuItem();
        this.tlsbtnRestartWld = new ToolStripButton();
        this.toolStripSeparator1 = new ToolStripSeparator();
        this.tsbtnUpdates = new ToolStripButton();
        this.tsbtnInstalls = new ToolStripButton();
        this.tsep2 = new ToolStripSeparator();
        this.tcbShow = new ToolStripComboBox();
        this.tlblShow = new ToolStripLabel();
        this.pnlMain = new Panel();
        this.label1 = new Label();
        this.lnkOpenWleFile = new LinkLabel();
        this.linkMoreExtensions = new LinkLabel();
        this.openFileDialog_0 = new OpenFileDialog();
        this.pnlExtensions = new Class47();
        this.lstExtensions = new Class45();
        this.lblInfo = new Label();
        this.toolbarExtensions.SuspendLayout();
        this.pnlMain.SuspendLayout();
        this.pnlExtensions.SuspendLayout();
        this.lstExtensions.SuspendLayout();
        base.SuspendLayout();
        this.toolbarExtensions.GripStyle = ToolStripGripStyle.Hidden;
        this.toolbarExtensions.Items.AddRange(new ToolStripItem[] { this.tsbtnAddins, this.tsbtnProviders, this.tsbtnStrategies, this.tsbtnIndicators, this.tsbtnOptions, this.tlsbtnRestartWld, this.toolStripSeparator1, this.tsbtnUpdates, this.tsbtnInstalls, this.tsep2, this.tcbShow, this.tlblShow });
        this.toolbarExtensions.Location = new Point(0, 0);
        this.toolbarExtensions.Name = "toolbarExtensions";
        this.toolbarExtensions.Size = new Size(0x2ff, 0x19);
        this.toolbarExtensions.TabIndex = 0;
        this.toolbarExtensions.Text = "toolbarExtensions";
        this.tsbtnAddins.Image = (Image) resources.GetObject("tsbtnAddins.Image");
        this.tsbtnAddins.ImageTransparentColor = Color.Magenta;
        this.tsbtnAddins.Name = "tsbtnAddins";
        this.tsbtnAddins.Size = new Size(0x3b, 0x16);
        this.tsbtnAddins.Tag = "Addin";
        this.tsbtnAddins.Text = "Addins";
        this.tsbtnAddins.Click += new EventHandler(this.tsbtnInstalls_Click);
        this.tsbtnProviders.Image = (Image) resources.GetObject("tsbtnProviders.Image");
        this.tsbtnProviders.ImageTransparentColor = Color.Magenta;
        this.tsbtnProviders.Name = "tsbtnProviders";
        this.tsbtnProviders.Size = new Size(0x48, 0x16);
        this.tsbtnProviders.Tag = "Provider";
        this.tsbtnProviders.Text = "Providers";
        this.tsbtnProviders.Click += new EventHandler(this.tsbtnInstalls_Click);
        this.tsbtnStrategies.Image = (Image) resources.GetObject("tsbtnStrategies.Image");
        this.tsbtnStrategies.ImageTransparentColor = Color.Magenta;
        this.tsbtnStrategies.Name = "tsbtnStrategies";
        this.tsbtnStrategies.Size = new Size(0x4c, 0x16);
        this.tsbtnStrategies.Tag = "Strategy";
        this.tsbtnStrategies.Text = "Strategies";
        this.tsbtnStrategies.Click += new EventHandler(this.tsbtnInstalls_Click);
        this.tsbtnIndicators.Image = (Image) resources.GetObject("tsbtnIndicators.Image");
        this.tsbtnIndicators.ImageTransparentColor = Color.Magenta;
        this.tsbtnIndicators.Name = "tsbtnIndicators";
        this.tsbtnIndicators.Size = new Size(0x4b, 0x16);
        this.tsbtnIndicators.Tag = "Indicator";
        this.tsbtnIndicators.Text = "Indicators";
        this.tsbtnIndicators.Click += new EventHandler(this.tsbtnInstalls_Click);
        this.tsbtnOptions.Alignment = ToolStripItemAlignment.Right;
        this.tsbtnOptions.DisplayStyle = ToolStripItemDisplayStyle.Image;
        this.tsbtnOptions.DropDownItems.AddRange(new ToolStripItem[] { this.mniCheckUpdates });
        this.tsbtnOptions.Image = (Image) resources.GetObject("tsbtnOptions.Image");
        this.tsbtnOptions.ImageTransparentColor = Color.Magenta;
        this.tsbtnOptions.Name = "tsbtnOptions";
        this.tsbtnOptions.Size = new Size(0x1d, 0x16);
        this.tsbtnOptions.Text = "Options";
        this.mniCheckUpdates.CheckOnClick = true;
        this.mniCheckUpdates.Name = "mniCheckUpdates";
        this.mniCheckUpdates.Size = new Size(0x101, 0x16);
        this.mniCheckUpdates.Text = "Check for updates on program start";
        this.mniCheckUpdates.Click += new EventHandler(this.mniCheckUpdates_Click);
        this.tlsbtnRestartWld.Alignment = ToolStripItemAlignment.Right;
        this.tlsbtnRestartWld.DisplayStyle = ToolStripItemDisplayStyle.Image;
        this.tlsbtnRestartWld.Image = (Image) resources.GetObject("tlsbtnRestartWld.Image");
        this.tlsbtnRestartWld.ImageTransparentColor = Color.Magenta;
        this.tlsbtnRestartWld.Name = "tlsbtnRestartWld";
        this.tlsbtnRestartWld.Size = new Size(0x17, 0x16);
        this.tlsbtnRestartWld.Text = "Restart Wealth-Lab";
        this.tlsbtnRestartWld.Click += new EventHandler(this.tlsbtnRestartWld_Click);
        this.toolStripSeparator1.Alignment = ToolStripItemAlignment.Right;
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new Size(6, 0x19);
        this.tsbtnUpdates.Alignment = ToolStripItemAlignment.Right;
        this.tsbtnUpdates.DisplayStyle = ToolStripItemDisplayStyle.Text;
        this.tsbtnUpdates.Image = (Image) resources.GetObject("tsbtnUpdates.Image");
        this.tsbtnUpdates.ImageTransparentColor = Color.Magenta;
        this.tsbtnUpdates.Name = "tsbtnUpdates";
        this.tsbtnUpdates.Size = new Size(0x33, 0x16);
        this.tsbtnUpdates.Tag = "NeedUpdate";
        this.tsbtnUpdates.Text = "Updates";
        this.tsbtnUpdates.ToolTipText = "Show Updates Only";
        this.tsbtnUpdates.Click += new EventHandler(this.tsbtnInstalls_Click);
        this.tsbtnInstalls.Alignment = ToolStripItemAlignment.Right;
        this.tsbtnInstalls.DisplayStyle = ToolStripItemDisplayStyle.Text;
        this.tsbtnInstalls.Image = (Image) resources.GetObject("tsbtnInstalls.Image");
        this.tsbtnInstalls.ImageTransparentColor = Color.Magenta;
        this.tsbtnInstalls.Name = "tsbtnInstalls";
        this.tsbtnInstalls.Size = new Size(0x2d, 0x16);
        this.tsbtnInstalls.Tag = "NeedInstall";
        this.tsbtnInstalls.Text = "Installs";
        this.tsbtnInstalls.ToolTipText = "Show Installs Only";
        this.tsbtnInstalls.Visible = false;
        this.tsbtnInstalls.Click += new EventHandler(this.tsbtnInstalls_Click);
        this.tsep2.Alignment = ToolStripItemAlignment.Right;
        this.tsep2.Name = "tsep2";
        this.tsep2.Size = new Size(6, 0x19);
        this.tsep2.Visible = false;
        this.tcbShow.Alignment = ToolStripItemAlignment.Right;
        this.tcbShow.DropDownStyle = ComboBoxStyle.DropDownList;
        this.tcbShow.Items.AddRange(new object[] { "Fidelity Supported Extensions", "Other Extensions" });
        this.tcbShow.Name = "tcbShow";
        this.tcbShow.Size = new Size(180, 0x19);
        this.tcbShow.Visible = false;
        this.tcbShow.SelectedIndexChanged += new EventHandler(this.tcbShow_SelectedIndexChanged);
        this.tlblShow.Alignment = ToolStripItemAlignment.Right;
        this.tlblShow.Name = "tlblShow";
        this.tlblShow.Size = new Size(0x25, 0x16);
        this.tlblShow.Text = "Show:";
        this.tlblShow.Visible = false;
        this.pnlMain.Controls.Add(this.label1);
        this.pnlMain.Controls.Add(this.lnkOpenWleFile);
        this.pnlMain.Controls.Add(this.linkMoreExtensions);
        this.pnlMain.Controls.Add(this.pnlExtensions);
        this.pnlMain.Dock = DockStyle.Fill;
        this.pnlMain.Location = new Point(0, 0x19);
        this.pnlMain.Name = "pnlMain";
        this.pnlMain.Size = new Size(0x2ff, 0x1d2);
        this.pnlMain.TabIndex = 1;
        this.pnlMain.Resize += new EventHandler(this.pnlMain_Resize);
        this.label1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
        this.label1.AutoSize = true;
        this.label1.Location = new Point(130, 0x1bd);
        this.label1.Name = "label1";
        this.label1.Size = new Size(0xb0, 13);
        this.label1.TabIndex = 1;
        this.label1.Text = "(or Drag and Drop into this Window)";
        this.lnkOpenWleFile.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
        this.lnkOpenWleFile.AutoSize = true;
        this.lnkOpenWleFile.LinkBehavior = LinkBehavior.AlwaysUnderline;
        this.lnkOpenWleFile.Location = new Point(2, 0x1bd);
        this.lnkOpenWleFile.Name = "lnkOpenWleFile";
        this.lnkOpenWleFile.Size = new Size(0x83, 13);
        this.lnkOpenWleFile.TabIndex = 0;
        this.lnkOpenWleFile.TabStop = true;
        this.lnkOpenWleFile.Text = "Open Extension's *.wle file";
        this.lnkOpenWleFile.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkOpenWleFile_LinkClicked);
        this.linkMoreExtensions.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.linkMoreExtensions.AutoSize = true;
        this.linkMoreExtensions.LinkBehavior = LinkBehavior.AlwaysUnderline;
        this.linkMoreExtensions.Location = new Point(0x24a, 0x1bd);
        this.linkMoreExtensions.Name = "linkMoreExtensions";
        this.linkMoreExtensions.Size = new Size(0xb1, 13);
        this.linkMoreExtensions.TabIndex = 2;
        this.linkMoreExtensions.TabStop = true;
        this.linkMoreExtensions.Text = "More Extensions on Wealth-Lab site";
        this.linkMoreExtensions.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkMoreExtensions_LinkClicked);
        this.openFileDialog_0.DefaultExt = "*.wle";
        this.openFileDialog_0.Filter = "Wealth-Lab Extension files (*.wle)|*.wle";
        this.pnlExtensions.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
        this.pnlExtensions.BackColor = SystemColors.Window;
        this.pnlExtensions.method_1(Color.FromArgb(0x7f, 0x9d, 0xb9));
        this.pnlExtensions.Controls.Add(this.lstExtensions);
        this.pnlExtensions.Location = new Point(4, 4);
        this.pnlExtensions.Name = "pnlExtensions";
        this.pnlExtensions.Size = new Size(0x2f7, 0x1b4);
        this.pnlExtensions.TabIndex = 3;
        this.lstExtensions.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
        this.lstExtensions.AutoScroll = true;
        this.lstExtensions.BackColor = Color.White;
        this.lstExtensions.Controls.Add(this.lblInfo);
        this.lstExtensions.Location = new Point(1, 1);
        this.lstExtensions.Name = "lstExtensions";
        this.lstExtensions.method_3(-1);
        this.lstExtensions.Size = new Size(0x2f5, 0x1b2);
        this.lstExtensions.TabIndex = 0;
        this.lstExtensions.method_0(new EventHandler(this.method_20));
        this.lblInfo.Anchor = AnchorStyles.Right | AnchorStyles.Left;
        this.lblInfo.BackColor = Color.Transparent;
        this.lblInfo.Font = new Font("Microsoft Sans Serif", 15f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.lblInfo.Location = new Point(7, 0x9d);
        this.lblInfo.Name = "lblInfo";
        this.lblInfo.Size = new Size(0x2f5, 0x26);
        this.lblInfo.TabIndex = 0;
        this.lblInfo.Text = "Please Wait...";
        this.lblInfo.TextAlign = ContentAlignment.MiddleCenter;
        this.AllowDrop = true;
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.ClientSize = new Size(0x2ff, 0x1eb);
        base.Controls.Add(this.pnlMain);
        base.Controls.Add(this.toolbarExtensions);
        base.Icon = (Icon) resources.GetObject("$this.Icon");
        this.MinimumSize = new Size(500, 300);
        base.Name = "ExtensionManagerForm";
        base.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Extension Manager";
        base.Shown += new EventHandler(this.ExtensionManagerForm_Shown);
        base.DragDrop += new DragEventHandler(this.ExtensionManagerForm_DragDrop);
        base.FormClosed += new FormClosedEventHandler(this.ExtensionManagerForm_FormClosed);
        base.DragEnter += new DragEventHandler(this.ExtensionManagerForm_DragEnter);
        base.FormClosing += new FormClosingEventHandler(this.ExtensionManagerForm_FormClosing);
        base.Resize += new EventHandler(this.ExtensionManagerForm_Resize);
        this.toolbarExtensions.ResumeLayout(false);
        this.toolbarExtensions.PerformLayout();
        this.pnlMain.ResumeLayout(false);
        this.pnlMain.PerformLayout();
        this.pnlExtensions.ResumeLayout(false);
        this.lstExtensions.ResumeLayout(false);
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    private void linkMoreExtensions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        ExtensionManager.NavigateToThirdPartySite(Class41.smethod_0().GetMoreExtensionsUrl());
    }

    private void lnkOpenWleFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        if ((this.openFileDialog_0.ShowDialog() == DialogResult.OK) && !string.IsNullOrEmpty(this.openFileDialog_0.FileName))
        {
            this.class40_0.method_17(this.openFileDialog_0.FileName);
        }
    }

    public bool method_0()
    {
        return this.bool_0;
    }

    private void method_1(Enum8 enum8_0)
    {
        if (enum8_0 == Enum8.const_0)
        {
            this.tsbtnInstalls.Visible = true;
            this.tsep2.Visible = true;
            this.tcbShow.Visible = true;
            this.tlblShow.Visible = true;
            this.tcbShow.SelectedIndex = 0;
            this.linkMoreExtensions.Visible = false;
        }
    }

    private void method_10()
    {
        try
        {
            this.class40_0.method_26();
            this.lblInfo.Visible = false;
            this.method_11();
            if (!string.IsNullOrEmpty(ExtensionManager.WleFileFromStart))
            {
                this.class40_0.method_17(ExtensionManager.WleFileFromStart);
                ExtensionManager.WleFileFromStart = string.Empty;
            }
            else if (!string.IsNullOrEmpty(ExtensionManager.WleFileFromOsClick))
            {
                this.class40_0.method_17(ExtensionManager.WleFileFromOsClick);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    public void method_11()
    {
        this.tsbtnProviders.PerformClick();
        this.lstExtensions.Update();
    }

    public void method_12()
    {
        this.tsbtnUpdates.PerformClick();
        this.lstExtensions.Update();
    }

    private void method_13(object sender, EventArgs9 e)
    {
        Class42.smethod_8(new object[] { e.method_0() });
        if (e.method_0() == (Enum9.flag_1 | Enum9.flag_0))
        {
            if (base.InvokeRequired)
            {
                Delegate8 method = new Delegate8(this.method_10);
                base.Invoke(method);
            }
            else
            {
                this.method_10();
            }
        }
    }

    private void method_14()
    {
        this.class40_0.method_15();
    }

    private void method_15()
    {
        this.lblInfo.Location = this.lstExtensions.Location;
    }

    private void method_16()
    {
        string empty = string.Empty;
        IEnumerator enumerator = this.toolbarExtensions.Items.GetEnumerator();
        try
        {
            while (true)
            {
                if (enumerator.MoveNext())
                {
                    ToolStripItem current = (ToolStripItem)enumerator.Current;
                    if (current is ToolStripButton && (current as ToolStripButton).Checked)
                    {
                        empty = (string)(current as ToolStripButton).Tag;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        string str = empty;
        string str1 = str;
        if (str != null)
        {
            if (str1 == "NeedUpdate")
            {
                this.method_17(Enum12.const_1);
                goto Label0;
            }
            else
            {
                if (str1 != "NotImplemented")
                {
                    if (str1 != "NeedInstall")
                    {
                        goto Label1;
                    }
                    this.method_17(Enum12.const_2);
                    goto Label0;
                }
                else
                {
                    goto Label0;
                }
            }
        }
    Label1:
        if (!string.IsNullOrEmpty(empty))
        {
            this.method_19((ExtensionType)Enum.Parse(typeof(ExtensionType), empty));
        }
        else
        {
            return;
        }
    Label0:
        if (this.lstExtensions.Controls.Count <= 0)
        {
            this.lblInfo.Text = "No items to display\r\n";
            this.lblInfo.Parent = this.lstExtensions;
            this.lblInfo.Visible = true;
            return;
        }
        else
        {
            this.lstExtensions.method_8(this.lstExtensions.Controls[0]);
            return;
        }
    }

    public void method_17(Enum12 enum12_0)
    {
        this.lstExtensions.method_11();
        foreach (Class46 class2 in this.class40_0.list_0)
        {
            if (class2.method_5() == enum12_0)
            {
                this.method_18(class2.method_2());
            }
        }
    }

    private void method_18(ExtensionControl extensionControl_0)
    {
        if (ExtensionManager.HostApp == Enum8.const_0)
        {
            if (this.tcbShow.SelectedIndex == 0)
            {
                if (extensionControl_0.method_0().method_4())
                {
                    this.lstExtensions.method_7(extensionControl_0);
                }
            }
            else if (!extensionControl_0.method_0().method_4())
            {
                this.lstExtensions.method_7(extensionControl_0);
            }
        }
        else
        {
            this.lstExtensions.method_7(extensionControl_0);
        }
    }

    public void method_19(ExtensionType extensionType_0)
    {
        this.lstExtensions.method_11();
        foreach (Class46 class2 in this.class40_0.list_0)
        {
            if (class2.method_9().Type == extensionType_0)
            {
                this.method_18(class2.method_2());
            }
        }
    }

    public void method_2(Class40 class40_1)
    {
        this.bool_0 = true;
        this.class40_0 = class40_1;
        this.class40_0.method_4(new Class40.Delegate14(this.method_13));
        this.class40_0.method_0(new Class40.Delegate12(this.method_9));
        this.class40_0.method_2(new Class40.Delegate13(this.method_7));
        this.class40_0.method_8(new Class40.Delegate15(this.method_5));
        this.class40_0.method_6(new Class40.Delegate15(this.method_4));
        this.class40_0.method_10(new Class40.Delegate15(this.method_3));
    }

    private void method_20(object sender, EventArgs e)
    {
        foreach (UserControl control in this.lstExtensions.Controls)
        {
            (control as ExtensionControl).method_10();
        }
    }

    private void method_3(object sender, EventArgs10 e)
    {
        this.method_16();
    }

    private void method_4(object sender, EventArgs10 e)
    {
        if (ExtensionManager.HostApp == Enum8.const_0)
        {
            if (e.class46_0.method_4())
            {
                if (this.tcbShow.SelectedIndex != 0)
                {
                    this.tcbShow.SelectedIndex = 0;
                }
            }
            else if (this.tcbShow.SelectedIndex != 1)
            {
                this.tcbShow.SelectedIndex = 1;
            }
        }
        switch (e.class46_0.method_8().Type)
        {
            case ExtensionType.Provider:
                this.tsbtnProviders.PerformClick();
                break;

            case ExtensionType.Addin:
                this.tsbtnAddins.PerformClick();
                break;

            case ExtensionType.Strategy:
                this.tsbtnStrategies.PerformClick();
                break;

            case ExtensionType.Indicator:
                this.tsbtnIndicators.PerformClick();
                break;
        }
        this.lstExtensions.method_8(e.class46_0.method_2());
    }

    private void method_5(object sender, EventArgs10 e)
    {
    }

    private void method_6()
    {
        this.lblInfo.Location = this.lstExtensions.Location;
        this.lblInfo.Size = this.lstExtensions.Size;
    }

    private void method_7(object sender, EventArgs8 e)
    {
    }

    public void method_8(EventArgs7 eventArgs7_0)
    {
        this.lblInfo.Visible = true;
        this.lblInfo.Text = "Please Wait...\r\n";
        if (eventArgs7_0.method_0() == Enum9.flag_0)
        {
            this.lblInfo.Text = this.lblInfo.Text + "Searching for local Extensions";
        }
        if (eventArgs7_0.method_0() == Enum9.flag_1)
        {
            this.lblInfo.Text = this.lblInfo.Text + "Requesting Extension data from the server";
        }
        this.lblInfo.Text = this.lblInfo.Text + "\r\n";
        Application.DoEvents();
    }

    private void method_9(object sender, EventArgs7 e)
    {
        if (base.InvokeRequired)
        {
            Delegate9 method = new Delegate9(this.method_8);
            base.Invoke(method, new object[] { e });
        }
        else
        {
            this.method_8(e);
        }
    }

    private void mniCheckUpdates_Click(object sender, EventArgs e)
    {
        ExtensionManager.Config.CheckUpdates = new bool?(this.mniCheckUpdates.Checked);
        ExtensionManager.Config.Serialize();
    }

    private void pnlMain_Resize(object sender, EventArgs e)
    {
        this.method_15();
    }

    public static void smethod_0()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, "WealthLab.Extensions.Agent.exe"), "/restart");
        Process.Start(startInfo);
        Application.DoEvents();
        Application.Exit();
    }

    //void Form.Dispose(bool disposing)
    void Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_0 != null))
        {
            this.icontainer_0.Dispose();
        }
        base.Dispose(disposing);
    }

    private void tcbShow_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.method_16();
        this.linkMoreExtensions.Visible = this.tcbShow.SelectedIndex != 0;
        this.tsbtnInstalls.Enabled = this.tcbShow.SelectedIndex == 0;
        if (this.tsbtnInstalls.Checked && (this.tcbShow.SelectedIndex == 1))
        {
            this.tsbtnProviders.PerformClick();
        }
    }

    private void tlsbtnRestartWld_Click(object sender, EventArgs e)
    {
        this.tlsbtnRestartWld.Enabled = false;
        smethod_0();
    }

    private void tsbtnInstalls_Click(object sender, EventArgs e)
    {
        foreach (ToolStripItem item in this.toolbarExtensions.Items)
        {
            if (item is ToolStripButton)
            {
                (item as ToolStripButton).Checked = false;
            }
        }
        (sender as ToolStripButton).Checked = true;
        this.method_16();
    }

    private delegate void Delegate10();

    private delegate void Delegate8();

    private delegate void Delegate9(EventArgs7 eventArgs7_0);
}

