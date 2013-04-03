using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WealthLab.Extensions;
using WealthLab.Extensions.Attribute;

internal class ExtensionControl : UserControl
{
    private bool bool_0;
    private bool bool_1;
    private bool bool_2;
    private Button btnCancel;
    private Button btnReleaseNotes;
    private Button btnUndo;
    private Button btnUninstall;
    private Button btnUpdate;
    private Class43 class43_0;
    private Class46 class46_0;
    private EventHandler eventHandler_0;
    private EventHandler eventHandler_1;
    private IContainer icontainer_0;
    private Label lblDescription;
    private Label lblDownloadInfo;
    private Label lblLicense;
    private Label lblLicenseValue;
    private Label lblName;
    private Label lblNewVersion;
    private Label lblNotInstalled;
    private Label lblPublisher;
    private LinkLabel lblPublisherValue;
    private PictureBox picLogo;
    private ProgressBar prgBar;

    public ExtensionControl()
    {
        this.InitializeComponent();
        this.lblName.ForeColor = Color.FromArgb(0x31, 0x6a, 0xc5);
        foreach (Control control in base.Controls)
        {
            if (!(control is Button))
            {
                control.Click += new EventHandler(this.method_7);
            }
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        if ((this.class43_0 != null) && this.bool_0)
        {
            this.class43_0.method_9(null);
            if (this.eventHandler_0 != null)
            {
                this.eventHandler_0(this, EventArgs.Empty);
            }
            if (this.method_0().method_7() != null)
            {
                this.method_10();
            }
            if (ExtensionManager.HostApp == Enum8.const_0)
            {
                this.method_10();
            }
            base.Update();
        }
    }

    private void btnReleaseNotes_Click(object sender, EventArgs e)
    {
        Cursor.Current = Cursors.WaitCursor;
        try
        {
            ExtensionManager.NavigateToThirdPartySite(Class41.smethod_0().GetChangeLogUrl(this.class46_0.method_9().StrongName));
        }
        catch (Exception exception)
        {
            MessageBox.Show("Error viewing Release Notes (" + this.class46_0.method_9().StrongName + ")\r\nError: " + exception.Message, "Release Notes Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    private void btnUndo_Click(object sender, EventArgs e)
    {
        if (this.bool_1)
        {
            string path = Path.Combine(ExtensionManager.GetTempDir, Class44.smethod_1(this.method_0().method_8().DisplayName)) + Path.DirectorySeparatorChar;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            if (this.method_0().method_7() == null)
            {
                if (this.eventHandler_1 != null)
                {
                    this.eventHandler_1(this, EventArgs.Empty);
                }
            }
            else
            {
                this.bool_1 = false;
                this.method_10();
                base.Update();
            }
            if (ExtensionManager.HostApp == Enum8.const_0)
            {
                this.bool_1 = false;
                this.method_10();
                base.Update();
            }
        }
        if (this.bool_2)
        {
            ExtensionManager.smethod_1(this.method_0());
            this.bool_2 = false;
            this.method_10();
            base.Update();
        }
    }

    private void btnUninstall_Click(object sender, EventArgs e)
    {
        ExtensionManager.smethod_0(this.method_0());
        this.bool_2 = true;
        this.method_10();
        base.Update();
    }

    private void btnUpdate_Click(object sender, EventArgs e)
    {
        this.method_11(this.class46_0.method_8());
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_0 != null))
        {
            this.icontainer_0.Dispose();
        }
        base.Dispose(disposing);
    }

    private void ExtensionControl_Paint(object sender, PaintEventArgs e)
    {
        Rectangle clientRectangle = (sender as Control).ClientRectangle;
        Class45 parent = base.Parent as Class45;
        if ((sender as Control) == parent.method_4())
        {
            LinearGradientBrush brush = new LinearGradientBrush(clientRectangle, Color.FromArgb(230, 0xeb, 0xf7), Color.LightSteelBlue, 90f);
            e.Graphics.FillRectangle(brush, clientRectangle);
        }
        Pen pen = new Pen(Color.Gray, 1f) {
            DashStyle = DashStyle.Dot
        };
        e.Graphics.DrawLine(pen, clientRectangle.X, clientRectangle.Bottom - 1, clientRectangle.X + clientRectangle.Width, clientRectangle.Bottom - 1);
    }

    private void InitializeComponent()
    {
        this.lblName = new Label();
        this.lblLicense = new Label();
        this.lblLicenseValue = new Label();
        this.lblPublisher = new Label();
        this.lblDescription = new Label();
        this.btnUpdate = new Button();
        this.btnReleaseNotes = new Button();
        this.btnUninstall = new Button();
        this.lblPublisherValue = new LinkLabel();
        this.picLogo = new PictureBox();
        this.lblNewVersion = new Label();
        this.prgBar = new ProgressBar();
        this.btnCancel = new Button();
        this.lblDownloadInfo = new Label();
        this.btnUndo = new Button();
        this.lblNotInstalled = new Label();
        ((ISupportInitialize) this.picLogo).BeginInit();
        base.SuspendLayout();
        this.lblName.AutoSize = true;
        this.lblName.BackColor = Color.Transparent;
        this.lblName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblName.ForeColor = Color.RoyalBlue;
        this.lblName.Location = new Point(40, 7);
        this.lblName.Name = "lblName";
        this.lblName.Size = new Size(0x34, 13);
        this.lblName.TabIndex = 0;
        this.lblName.Text = "lblName";
        this.lblName.UseMnemonic = false;
        this.lblLicense.AutoSize = true;
        this.lblLicense.BackColor = Color.Transparent;
        this.lblLicense.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblLicense.Location = new Point(40, 0x1a);
        this.lblLicense.Name = "lblLicense";
        this.lblLicense.Size = new Size(0x37, 13);
        this.lblLicense.TabIndex = 1;
        this.lblLicense.Text = "License:";
        this.lblLicenseValue.AutoSize = true;
        this.lblLicenseValue.BackColor = Color.Transparent;
        this.lblLicenseValue.Location = new Point(0x5d, 0x1a);
        this.lblLicenseValue.Name = "lblLicenseValue";
        this.lblLicenseValue.Size = new Size(0x51, 13);
        this.lblLicenseValue.TabIndex = 2;
        this.lblLicenseValue.Text = "lblLicenseValue";
        this.lblLicenseValue.UseMnemonic = false;
        this.lblPublisher.AutoSize = true;
        this.lblPublisher.BackColor = Color.Transparent;
        this.lblPublisher.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblPublisher.Location = new Point(0xc6, 0x1a);
        this.lblPublisher.Name = "lblPublisher";
        this.lblPublisher.Size = new Size(0x3f, 13);
        this.lblPublisher.TabIndex = 3;
        this.lblPublisher.Text = "Publisher:";
        this.lblDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
        this.lblDescription.AutoEllipsis = true;
        this.lblDescription.BackColor = Color.Transparent;
        this.lblDescription.Location = new Point(40, 0x2c);
        this.lblDescription.Name = "lblDescription";
        this.lblDescription.Size = new Size(0x1d3, 13);
        this.lblDescription.TabIndex = 5;
        this.lblDescription.Text = "lblDescription";
        this.lblDescription.UseMnemonic = false;
        this.btnUpdate.BackColor = SystemColors.Control;
        this.btnUpdate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.btnUpdate.ForeColor = SystemColors.ControlText;
        this.btnUpdate.Location = new Point(0x2b, 0x40);
        this.btnUpdate.Name = "btnUpdate";
        this.btnUpdate.Size = new Size(0x5f, 0x17);
        this.btnUpdate.TabIndex = 6;
        this.btnUpdate.Text = "Update Now";
        this.btnUpdate.UseVisualStyleBackColor = true;
        this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
        this.btnReleaseNotes.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.btnReleaseNotes.BackColor = SystemColors.Control;
        this.btnReleaseNotes.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.btnReleaseNotes.Location = new Point(0x137, 0x5c);
        this.btnReleaseNotes.Name = "btnReleaseNotes";
        this.btnReleaseNotes.Size = new Size(0x5f, 0x17);
        this.btnReleaseNotes.TabIndex = 7;
        this.btnReleaseNotes.Text = "Change Log";
        this.btnReleaseNotes.UseVisualStyleBackColor = true;
        this.btnReleaseNotes.Click += new EventHandler(this.btnReleaseNotes_Click);
        this.btnUninstall.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.btnUninstall.BackColor = SystemColors.Control;
        this.btnUninstall.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.btnUninstall.Location = new Point(0x19c, 0x5c);
        this.btnUninstall.Name = "btnUninstall";
        this.btnUninstall.Size = new Size(0x5f, 0x17);
        this.btnUninstall.TabIndex = 8;
        this.btnUninstall.Text = "Uninstall";
        this.btnUninstall.UseVisualStyleBackColor = true;
        this.btnUninstall.Click += new EventHandler(this.btnUninstall_Click);
        this.lblPublisherValue.AutoSize = true;
        this.lblPublisherValue.BackColor = Color.Transparent;
        this.lblPublisherValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.lblPublisherValue.LinkBehavior = LinkBehavior.HoverUnderline;
        this.lblPublisherValue.LinkColor = SystemColors.ControlText;
        this.lblPublisherValue.Location = new Point(0x103, 0x1a);
        this.lblPublisherValue.Name = "lblPublisherValue";
        this.lblPublisherValue.Size = new Size(0x57, 13);
        this.lblPublisherValue.TabIndex = 9;
        this.lblPublisherValue.TabStop = true;
        this.lblPublisherValue.Text = "lblPublisherValue";
        this.lblPublisherValue.UseMnemonic = false;
        this.lblPublisherValue.VisitedLinkColor = SystemColors.ControlText;
        this.lblPublisherValue.DoubleClick += new EventHandler(this.lblPublisherValue_Click);
        this.lblPublisherValue.Click += new EventHandler(this.lblPublisherValue_Click);
        this.picLogo.BackColor = Color.Transparent;
        this.picLogo.Location = new Point(13, 9);
        this.picLogo.Name = "picLogo";
        this.picLogo.Size = new Size(0x10, 0x10);
        this.picLogo.TabIndex = 10;
        this.picLogo.TabStop = false;
        this.lblNewVersion.AutoSize = true;
        this.lblNewVersion.BackColor = Color.Transparent;
        this.lblNewVersion.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblNewVersion.ForeColor = Color.Red;
        this.lblNewVersion.Location = new Point(0xae, 7);
        this.lblNewVersion.Name = "lblNewVersion";
        this.lblNewVersion.Size = new Size(0x57, 13);
        this.lblNewVersion.TabIndex = 11;
        this.lblNewVersion.Text = "lblNewVersion";
        this.prgBar.Location = new Point(0x2b, 0x5d);
        this.prgBar.Name = "prgBar";
        this.prgBar.Size = new Size(0xb6, 0x15);
        this.prgBar.TabIndex = 12;
        this.prgBar.Visible = false;
        this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.btnCancel.BackColor = SystemColors.Control;
        this.btnCancel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.btnCancel.Location = new Point(0x19c, 0x3f);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(0x5f, 0x17);
        this.btnCancel.TabIndex = 13;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
        this.lblDownloadInfo.AutoSize = true;
        this.lblDownloadInfo.BackColor = Color.Transparent;
        this.lblDownloadInfo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblDownloadInfo.ForeColor = Color.Green;
        this.lblDownloadInfo.Location = new Point(0xe8, 0x44);
        this.lblDownloadInfo.Name = "lblDownloadInfo";
        this.lblDownloadInfo.Size = new Size(0, 13);
        this.lblDownloadInfo.TabIndex = 14;
        this.btnUndo.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.btnUndo.BackColor = SystemColors.Control;
        this.btnUndo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.btnUndo.Location = new Point(0x19c, 0x22);
        this.btnUndo.Name = "btnUndo";
        this.btnUndo.Size = new Size(0x5f, 0x17);
        this.btnUndo.TabIndex = 15;
        this.btnUndo.Text = "Undo";
        this.btnUndo.UseVisualStyleBackColor = true;
        this.btnUndo.Click += new EventHandler(this.btnUndo_Click);
        this.lblNotInstalled.AutoSize = true;
        this.lblNotInstalled.BackColor = Color.Transparent;
        this.lblNotInstalled.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblNotInstalled.ForeColor = Color.Brown;
        this.lblNotInstalled.Location = new Point(0x14f, 7);
        this.lblNotInstalled.Name = "lblNotInstalled";
        this.lblNotInstalled.Size = new Size(0x4f, 13);
        this.lblNotInstalled.TabIndex = 0x10;
        this.lblNotInstalled.Text = "Not Installed";
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        this.BackColor = Color.White;
        base.Controls.Add(this.lblNotInstalled);
        base.Controls.Add(this.btnUndo);
        base.Controls.Add(this.lblDownloadInfo);
        base.Controls.Add(this.btnCancel);
        base.Controls.Add(this.prgBar);
        base.Controls.Add(this.lblNewVersion);
        base.Controls.Add(this.picLogo);
        base.Controls.Add(this.lblPublisherValue);
        base.Controls.Add(this.btnUninstall);
        base.Controls.Add(this.btnReleaseNotes);
        base.Controls.Add(this.btnUpdate);
        base.Controls.Add(this.lblDescription);
        base.Controls.Add(this.lblPublisher);
        base.Controls.Add(this.lblLicenseValue);
        base.Controls.Add(this.lblLicense);
        base.Controls.Add(this.lblName);
        base.Name = "ExtensionControl";
        base.Size = new Size(530, 120);
        base.Paint += new PaintEventHandler(this.ExtensionControl_Paint);
        ((ISupportInitialize) this.picLogo).EndInit();
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    private void lblPublisherValue_Click(object sender, EventArgs e)
    {
        if (this.class46_0.method_9().PublisherUrl != null)
        {
            try
            {
                ExtensionManager.NavigateToThirdPartySite(this.class46_0.method_9().PublisherUrl);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error viewing Publisher Site (" + this.class46_0.method_9().StrongName + ")\r\nError: " + exception.Message, "Publisher Site Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
    }

    public Class46 method_0()
    {
        return this.class46_0;
    }

    public void method_1(Class46 class46_1)
    {
        this.class46_0 = class46_1;
        this.method_8();
    }

    public void method_10()
    {
        try
        {
            if (base.Parent != null)
            {
                if (this.bool_0)
                {
                    base.Height = 0x5c;
                    this.btnReleaseNotes.Visible = false;
                    this.btnUndo.Visible = false;
                    this.btnUninstall.Visible = false;
                    this.btnUpdate.Visible = false;
                    this.lblLicense.Visible = true;
                    this.lblLicenseValue.Visible = true;
                    this.lblPublisher.Visible = true;
                    this.lblPublisherValue.Visible = true;
                    this.lblDescription.Location = new Point(40, 0x2c);
                    this.lblDownloadInfo.Location = new Point(0xe8, 0x44);
                    this.lblDownloadInfo.Visible = true;
                    this.prgBar.Location = this.btnUpdate.Location;
                    this.prgBar.Visible = true;
                    if (this == (base.Parent as Class45).method_4())
                    {
                        this.btnCancel.Location = this.btnUninstall.Location;
                        this.btnCancel.Visible = true;
                    }
                    else
                    {
                        this.btnCancel.Visible = false;
                    }
                }
                else
                {
                    this.prgBar.Visible = false;
                    this.btnCancel.Visible = false;
                    if (!this.bool_1 && !this.bool_2)
                    {
                        this.lblDownloadInfo.Visible = false;
                        this.btnUndo.Visible = false;
                        if (this != (base.Parent as Class45).method_4())
                        {
                            base.Height = 0x2d;
                            this.btnReleaseNotes.Visible = false;
                            this.btnUninstall.Visible = false;
                            this.btnUpdate.Visible = false;
                            this.lblLicense.Visible = false;
                            this.lblLicenseValue.Visible = false;
                            this.lblPublisher.Visible = false;
                            this.lblPublisherValue.Visible = false;
                            this.lblDescription.Location = new Point(40, 0x1a);
                        }
                        else
                        {
                            base.Height = 0x5c;
                            this.btnReleaseNotes.Visible = true;
                            if (this.class46_0.method_6() == Enum11.flag_1)
                            {
                                this.btnUninstall.Visible = false;
                            }
                            else
                            {
                                this.btnUninstall.Visible = true;
                            }
                            this.btnUpdate.Visible = true;
                            this.lblLicense.Visible = true;
                            this.lblLicenseValue.Visible = true;
                            this.lblPublisher.Visible = true;
                            this.lblPublisherValue.Visible = true;
                            this.lblDescription.Location = new Point(40, 0x2c);
                        }
                    }
                    else
                    {
                        base.Height = 0x5c;
                        this.btnReleaseNotes.Visible = false;
                        this.btnUninstall.Visible = false;
                        this.btnUpdate.Visible = false;
                        this.lblLicense.Visible = true;
                        this.lblLicenseValue.Visible = true;
                        this.lblPublisher.Visible = true;
                        this.lblPublisherValue.Visible = true;
                        this.lblDescription.Location = new Point(40, 0x2c);
                        if (this.bool_1)
                        {
                            this.lblDownloadInfo.Text = "Wealth-Lab restart required to finish installation.";
                        }
                        else
                        {
                            this.lblDownloadInfo.Text = "Wealth-Lab restart required to finish uninstall.";
                        }
                        this.lblDownloadInfo.Location = new Point(this.btnUpdate.Location.X - 3, this.lblDownloadInfo.Location.Y);
                        this.lblDownloadInfo.Visible = true;
                        if (this == (base.Parent as Class45).method_4())
                        {
                            this.btnUndo.Location = this.btnUninstall.Location;
                            this.btnUndo.Visible = true;
                        }
                        else
                        {
                            this.btnUndo.Visible = false;
                        }
                    }
                }
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Extension Manager Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
    }

    public void method_11(ExtensionInfoAttribute extensionInfoAttribute_0)
    {
        string[] downloadUrls = new string[0];
        if (Class40.smethod_1(extensionInfoAttribute_0))
        {
            try
            {
                downloadUrls = Class41.smethod_0().GetDownloadUrls(extensionInfoAttribute_0.StrongName, extensionInfoAttribute_0.Version);
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("Error when identifying Extension download URIs. Strong Name: {0}, Version: {1}, Error {2}", extensionInfoAttribute_0.StrongName, extensionInfoAttribute_0.Version, exception.Message), "URI Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            if (downloadUrls.Length == 0)
            {
                MessageBox.Show(string.Format("The server didn't return a download URI for the Extension. Strong Name: {0}, Version: {1}", extensionInfoAttribute_0.StrongName, extensionInfoAttribute_0.Version), "URI Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                this.class43_0 = new Class43(this.class46_0, downloadUrls);
                this.class43_0.method_2(new Class43.Delegate18(this.method_13));
                this.class43_0.method_0(new Class43.Delegate17(this.method_12));
                this.bool_0 = true;
                if ((this.method_0().method_7() != null) && (this.method_0().method_2().Parent != null))
                {
                    this.method_10();
                }
                if ((ExtensionManager.HostApp == Enum8.const_0) && (this.method_0().method_2().Parent != null))
                {
                    this.method_10();
                }
                base.Update();
                this.class43_0.method_6();
            }
        }
    }

    private void method_12(object sender, EventArgs11 e)
    {
        this.prgBar.Value = e.int_0;
        this.lblDownloadInfo.Text = string.Format("Downloading file {0} of {1}, ({2}/{3} KB)", new object[] { e.int_1, e.int_2, e.long_0 / 0x400L, e.long_1 / 0x400L });
    }

    private void method_13(object sender, EventArgs12 e)
    {
        this.bool_0 = false;
        if (!e.bool_0 && !e.bool_1)
        {
            this.bool_1 = true;
            this.method_10();
            base.Update();
        }
        else
        {
            if (e.bool_0)
            {
                MessageBox.Show(string.Format("Error downloading Extension. Strong Name: {0}, Version: {1}\r\n{2}", this.class46_0.method_8().StrongName, this.class46_0.method_8().Version, (e.string_0 == null) ? "Unknown" : e.string_0), "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            if (this.eventHandler_0 != null)
            {
                this.eventHandler_0(this, EventArgs.Empty);
            }
            if (this.method_0().method_7() != null)
            {
                this.method_10();
            }
            base.Update();
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_2(EventHandler eventHandler_2)
    {
        this.eventHandler_0 = (EventHandler) Delegate.Combine(this.eventHandler_0, eventHandler_2);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_3(EventHandler eventHandler_2)
    {
        this.eventHandler_0 = (EventHandler) Delegate.Remove(this.eventHandler_0, eventHandler_2);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_4(EventHandler eventHandler_2)
    {
        this.eventHandler_1 = (EventHandler) Delegate.Combine(this.eventHandler_1, eventHandler_2);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_5(EventHandler eventHandler_2)
    {
        this.eventHandler_1 = (EventHandler) Delegate.Remove(this.eventHandler_1, eventHandler_2);
    }

    public bool method_6()
    {
        return this.bool_0;
    }

    private void method_7(object sender, EventArgs e)
    {
        this.OnClick(e);
    }

    private void method_8()
    {
        this.lblName.Text = this.class46_0.method_9().DisplayName;
        if ((this.class46_0.method_6() & Enum11.flag_0) != 0)
        {
            this.lblName.Text = this.lblName.Text + "  " + this.class46_0.method_9().Version;
        }
        else
        {
            this.lblName.Text = this.lblName.Text + "  " + this.class46_0.method_8().Version;
        }
        if (this.class46_0.method_6() == Enum11.flag_0)
        {
            this.lblName.Text = this.lblName.Text + "   (Local)";
            this.lblName.ForeColor = Color.Gray;
        }
        if (this.class46_0.method_6() == Enum11.flag_1)
        {
            this.lblNotInstalled.Location = this.lblNewVersion.Location = new Point((this.lblName.Width + this.lblName.Location.X) + 40, 7);
        }
        else
        {
            this.lblNotInstalled.Visible = false;
        }
        switch (this.class46_0.method_9().Licence)
        {
            case ExtensionLicence.Freeware:
                this.lblLicenseValue.Text = "Freeware";
                break;

            case ExtensionLicence.Commercial:
                this.lblLicenseValue.Text = "Commercial, " + this.class46_0.method_9().LicencePrice;
                break;

            default:
                this.lblLicenseValue.Text = "Private";
                break;
        }
        this.lblPublisherValue.Text = this.class46_0.method_9().Publisher;
        this.lblPublisher.Location = new Point((this.lblLicenseValue.Width + this.lblLicenseValue.Location.X) + 40, 0x1a);
        this.lblPublisherValue.Location = new Point((this.lblPublisher.Width + this.lblPublisher.Location.X) - 3, 0x1a);
        this.lblDescription.Text = this.class46_0.method_9().Description;
        try
        {
            Bitmap bitmap = new Bitmap(ExtensionInfoAttribute.ConvertBase64ToImage(this.class46_0.method_9().Glyph16x16Base64));
            bitmap.MakeTransparent(Color.Fuchsia);
            this.picLogo.Image = bitmap;
        }
        catch
        {
        }
        this.btnUpdate.Enabled = false;
        this.lblNewVersion.Visible = false;
        if ((this.class46_0.method_6() == (Enum11.flag_1 | Enum11.flag_0)) && (this.class46_0.method_5() == Enum12.const_1))
        {
            this.method_9();
        }
        if (this.class46_0.method_6() == Enum11.flag_1)
        {
            this.btnUpdate.Enabled = true;
            this.btnUpdate.Text = "Install Now";
            this.btnUninstall.Visible = false;
        }
        this.btnReleaseNotes.Enabled = (this.class46_0.method_6() & Enum11.flag_1) != 0;
    }

    private void method_9()
    {
        if (new Version(this.class46_0.method_7().Version) < new Version(this.class46_0.method_8().Version))
        {
            this.btnUpdate.Enabled = true;
            this.lblNewVersion.Visible = true;
            this.lblNewVersion.Location = new Point((this.lblName.Width + this.lblName.Location.X) + 40, 7);
            this.lblNewVersion.Text = "New Version " + this.class46_0.method_8().Version;
        }
    }
}

