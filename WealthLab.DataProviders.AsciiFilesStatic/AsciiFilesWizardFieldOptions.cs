using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WealthLab.DataProviders.AsciiFilesStatic;
using WealthLab.DataProviders.AsciiFilesStatic.Properties;
using WealthLab.DataProviders.Helper;

internal class AsciiFilesWizardFieldOptions : UserControl
{
    public AsciiFilesDataSet asciiFilesDataSet_0;
    private AsciiFilesDataSetList asciiFilesDataSetList_0;
    private Button btnCommanFormats;
    private Button btnConformity;
    private Button btnFormatsHelp;
    private Button btnItemAdd;
    private Button btnItemDelete;
    private Button btnMoveItemDown;
    private Button btnMoveItemUp;
    private Button btnViewDataFile;
    private ComboBox cmbDateFormat;
    private ComboBox cmbDecimalSeparator;
    private ComboBox cmbFieldSeparator;
    private ComboBox cmbThousandsSeparator;
    private ComboBox cmbTimeFormat;
    private ComboBox cmbVolumeMultiple;
    private ContextMenuStrip cmnuAddField;
    private ContextMenuStrip cmnuCommanFormats;
    private CustomFieldNameControl customFieldNameControl_0 = new CustomFieldNameControl();
    private GroupBox grpFieldOrder;
    private GroupBox grpFormatOptions;
    private IContainer icontainer_0;
    private ImageList imageList_0;
    private Label lblDateFormat;
    private Label lblDecimalSeparator;
    private Label lblFieldSeparator;
    private Label lblIgnoreFirstLines;
    private Label lblIgnoreLastLines;
    private Label lblImpliedDecimals;
    private Label lblThousandsSeparator;
    private Label lblTimeFormat;
    private Label lblVolumeMultiple;
    private ListBox lstFieldOrder;
    private ToolStripMenuItem mnuClose;
    private ToolStripMenuItem mnuCustomField;
    private ToolStripMenuItem mnuDate;
    private ToolStripMenuItem mnuFiller;
    private ToolStripMenuItem mnuHigh;
    private ToolStripMenuItem mnuLow;
    private ToolStripMenuItem mnuOpen;
    private ToolStripMenuItem mnuOpenInterest;
    private ToolStripMenuItem mnuSecurityName;
    private ToolStripMenuItem mnuTime;
    private ToolStripMenuItem mnuVolume;
    private NumericUpDown numFirstLines;
    private NumericUpDown numImpliedDecimals;
    private NumericUpDown numLastLines;
    private ToolStripMenuItem testToolStripMenuItem;
    private TextBox textBox_0;

    public AsciiFilesWizardFieldOptions()
    {
        this.InitializeComponent();
        Button button = (Button) this.customFieldNameControl_0.Controls[this.customFieldNameControl_0.Controls.IndexOfKey("btnFieldName")];
        this.textBox_0 = (TextBox) this.customFieldNameControl_0.Controls[this.customFieldNameControl_0.Controls.IndexOfKey("txtFieldName")];
        button.Click += new EventHandler(this.method_0);
        this.textBox_0.KeyDown += new KeyEventHandler(this.textBox_0_KeyDown);
        ToolStripControlHost host = new ToolStripControlHost(this.customFieldNameControl_0) {
            AutoSize = false,
            Height = this.customFieldNameControl_0.Height + 1
        };
        this.mnuCustomField.DropDownItems.Add(host);
    }

    private void btnCommanFormats_Click(object sender, EventArgs e)
    {
        Point screenLocation = base.PointToScreen((sender as Button).Location);
        screenLocation.Y += (sender as Button).Height + 5;
        screenLocation.X += 0x12a;
        this.cmnuCommanFormats.Show(screenLocation);
    }

    private void btnConformity_Click(object sender, EventArgs e)
    {
    }

    private void btnFormatsHelp_Click(object sender, EventArgs e)
    {
        Help.ShowHelp(this, Application.StartupPath + @"\WLNetUserGuide.chm", "ascii.htm");
    }

    private void btnItemAdd_Click(object sender, EventArgs e)
    {
        Point screenLocation = base.PointToScreen((sender as Button).Location);
        screenLocation.Y += (sender as Button).Height + 5;
        screenLocation.X += 8;
        this.cmnuAddField.Show(screenLocation);
    }

    private void btnItemDelete_Click(object sender, EventArgs e)
    {
        int selectedIndex = this.lstFieldOrder.SelectedIndex;
        if (selectedIndex != -1)
        {
            Field selectedItem = this.lstFieldOrder.SelectedItem as Field;
            if ((selectedItem.Type == FieldType.Date) || (selectedItem.Type == FieldType.Close))
            {
                return;
            }
            this.lstFieldOrder.Items.RemoveAt(selectedIndex);
        }
        this.method_1();
    }

    private void btnMoveItemDown_Click(object sender, EventArgs e)
    {
        int selectedIndex = this.lstFieldOrder.SelectedIndex;
        if ((selectedIndex != -1) && (selectedIndex < (this.lstFieldOrder.Items.Count - 1)))
        {
            object obj2 = this.lstFieldOrder.Items[selectedIndex + 1];
            this.lstFieldOrder.Items[selectedIndex + 1] = this.lstFieldOrder.Items[selectedIndex];
            this.lstFieldOrder.Items[selectedIndex] = obj2;
            this.lstFieldOrder.SelectedIndex = selectedIndex + 1;
        }
    }

    private void btnMoveItemUp_Click(object sender, EventArgs e)
    {
        int selectedIndex = this.lstFieldOrder.SelectedIndex;
        if (selectedIndex > 0)
        {
            object obj2 = this.lstFieldOrder.Items[selectedIndex - 1];
            this.lstFieldOrder.Items[selectedIndex - 1] = this.lstFieldOrder.Items[selectedIndex];
            this.lstFieldOrder.Items[selectedIndex] = obj2;
            this.lstFieldOrder.SelectedIndex = selectedIndex - 1;
        }
    }

    private void btnViewDataFile_Click(object sender, EventArgs e)
    {
        List<string> list = AsciiFilesDataSet.GetFilesFromDir(this.asciiFilesDataSet_0.Folder, this.asciiFilesDataSet_0.Extension, true, true);
        if (list.Count > 0)
        {
            Process.Start("notepad.exe", list[0]);
        }
        else
        {
            MessageBox.Show("Files not found.", "Files not found", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
    }

    private void cmnuAddField_Opening(object sender, CancelEventArgs e)
    {
        this.textBox_0.Clear();
        this.method_4(this.mnuClose);
        this.method_4(this.mnuDate);
        this.method_4(this.mnuHigh);
        this.method_4(this.mnuLow);
        this.method_4(this.mnuOpen);
        this.method_4(this.mnuOpenInterest);
        this.method_4(this.mnuSecurityName);
        this.method_4(this.mnuTime);
        this.method_4(this.mnuVolume);
    }

    private void cmnuCommanFormats_Opening(object sender, CancelEventArgs e)
    {
        ToolStripMenuItem item;
        ToolStripMenuItem item2;
        this.cmnuCommanFormats.Items.Clear();
        foreach (object obj3 in this.asciiFilesDataSetList_0.Items)
        {
            AsciiFilesDataSet set2 = (AsciiFilesDataSet) obj3;
            item = new ToolStripMenuItem {
                Text = set2.Name,
                Tag = set2
            };
            item.Click += new EventHandler(this.method_9);
            this.cmnuCommanFormats.Items.Add(item);
        }
        this.cmnuCommanFormats.Items.Add("-");
        item = new ToolStripMenuItem {
            Text = "Copy Format from a DataSet",
            Tag = "Copy"
        };
        item.Click += new EventHandler(this.method_9);
        this.cmnuCommanFormats.Items.Add(item);
        string str = Directory.GetParent(AsciiFilesStaticProvider.RootDataPath).Parent.FullName + @"\DataSets";
        AsciiFilesDataSetList formatFromDataSets = this.asciiFilesDataSetList_0.GetFormatFromDataSets(str);
        if (formatFromDataSets.Items.Count == 0)
        {
            item.Enabled = false;
        }
        else
        {
            foreach (AsciiFilesDataSet set3 in formatFromDataSets.Items)
            {
                item2 = new ToolStripMenuItem {
                    Text = set3.Name,
                    Tag = set3
                };
                item2.Click += new EventHandler(this.method_9);
                item.DropDownItems.Add(item2);
            }
        }
        this.cmnuCommanFormats.Items.Add("-");
        item = new ToolStripMenuItem {
            Text = "Save these Settings as a Common Format...",
            Tag = "Save"
        };
        item.Click += new EventHandler(this.method_9);
        this.cmnuCommanFormats.Items.Add(item);
        item = new ToolStripMenuItem {
            Text = "Delete Common Format",
            Tag = "Delete"
        };
        this.cmnuCommanFormats.Items.Add(item);
        if (this.asciiFilesDataSetList_0.Items.Count == 0)
        {
            item.Enabled = false;
        }
        else
        {
            foreach (object obj2 in this.asciiFilesDataSetList_0.Items)
            {
                AsciiFilesDataSet set = (AsciiFilesDataSet) obj2;
                item2 = new ToolStripMenuItem {
                    Text = set.Name,
                    Tag = "Delete"
                };
                item2.Click += new EventHandler(this.method_9);
                item.DropDownItems.Add(item2);
            }
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
        this.icontainer_0 = new Container();
        ComponentResourceManager manager = new ComponentResourceManager(typeof(AsciiFilesWizardFieldOptions));
        this.imageList_0 = new ImageList(this.icontainer_0);
        this.grpFieldOrder = new GroupBox();
        this.btnConformity = new Button();
        this.btnItemDelete = new Button();
        this.btnItemAdd = new Button();
        this.btnMoveItemDown = new Button();
        this.btnMoveItemUp = new Button();
        this.lstFieldOrder = new ListBox();
        this.grpFormatOptions = new GroupBox();
        this.btnFormatsHelp = new Button();
        this.lblThousandsSeparator = new Label();
        this.cmbThousandsSeparator = new ComboBox();
        this.numImpliedDecimals = new NumericUpDown();
        this.numLastLines = new NumericUpDown();
        this.numFirstLines = new NumericUpDown();
        this.lblImpliedDecimals = new Label();
        this.lblVolumeMultiple = new Label();
        this.lblIgnoreLastLines = new Label();
        this.lblIgnoreFirstLines = new Label();
        this.cmbVolumeMultiple = new ComboBox();
        this.cmbDecimalSeparator = new ComboBox();
        this.cmbFieldSeparator = new ComboBox();
        this.cmbTimeFormat = new ComboBox();
        this.btnCommanFormats = new Button();
        this.cmbDateFormat = new ComboBox();
        this.lblDecimalSeparator = new Label();
        this.lblFieldSeparator = new Label();
        this.lblTimeFormat = new Label();
        this.lblDateFormat = new Label();
        this.cmnuAddField = new ContextMenuStrip(this.icontainer_0);
        this.mnuDate = new ToolStripMenuItem();
        this.mnuTime = new ToolStripMenuItem();
        this.mnuOpen = new ToolStripMenuItem();
        this.mnuHigh = new ToolStripMenuItem();
        this.mnuLow = new ToolStripMenuItem();
        this.mnuClose = new ToolStripMenuItem();
        this.mnuVolume = new ToolStripMenuItem();
        this.mnuSecurityName = new ToolStripMenuItem();
        this.mnuOpenInterest = new ToolStripMenuItem();
        this.mnuFiller = new ToolStripMenuItem();
        this.mnuCustomField = new ToolStripMenuItem();
        this.cmnuCommanFormats = new ContextMenuStrip(this.icontainer_0);
        this.testToolStripMenuItem = new ToolStripMenuItem();
        this.btnViewDataFile = new Button();
        this.grpFieldOrder.SuspendLayout();
        this.grpFormatOptions.SuspendLayout();
        this.numImpliedDecimals.BeginInit();
        this.numLastLines.BeginInit();
        this.numFirstLines.BeginInit();
        this.cmnuAddField.SuspendLayout();
        this.cmnuCommanFormats.SuspendLayout();
        base.SuspendLayout();
        this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("imlMain.ImageStream");
        this.imageList_0.TransparentColor = Color.Transparent;
        this.imageList_0.Images.SetKeyName(0, "add.png");
        this.imageList_0.Images.SetKeyName(1, "delete.png");
        this.imageList_0.Images.SetKeyName(2, "disk.png");
        this.imageList_0.Images.SetKeyName(3, "down.ico");
        this.imageList_0.Images.SetKeyName(4, "up.ico");
        this.grpFieldOrder.Controls.Add(this.btnConformity);
        this.grpFieldOrder.Controls.Add(this.btnItemDelete);
        this.grpFieldOrder.Controls.Add(this.btnItemAdd);
        this.grpFieldOrder.Controls.Add(this.btnMoveItemDown);
        this.grpFieldOrder.Controls.Add(this.btnMoveItemUp);
        this.grpFieldOrder.Controls.Add(this.lstFieldOrder);
        this.grpFieldOrder.Location = new Point(7, 7);
        this.grpFieldOrder.Name = "grpFieldOrder";
        this.grpFieldOrder.Size = new Size(0x11c, 0x124);
        this.grpFieldOrder.TabIndex = 1;
        this.grpFieldOrder.TabStop = false;
        this.grpFieldOrder.Text = "Field Order";
        this.btnConformity.Image = (Image) manager.GetObject("btnConformity.Image");
        this.btnConformity.ImageAlign = ContentAlignment.MiddleLeft;
        this.btnConformity.Location = new Point(0xb6, 0xaf);
        this.btnConformity.Name = "btnConformity";
        this.btnConformity.Size = new Size(0x60, 0x18);
        this.btnConformity.TabIndex = 5;
        this.btnConformity.Text = "Conformitys...";
        this.btnConformity.TextAlign = ContentAlignment.MiddleRight;
        this.btnConformity.UseVisualStyleBackColor = true;
        this.btnConformity.Visible = false;
        this.btnConformity.Click += new EventHandler(this.btnConformity_Click);
        this.btnItemDelete.ImageAlign = ContentAlignment.MiddleLeft;
        this.btnItemDelete.ImageIndex = 1;
        this.btnItemDelete.ImageList = this.imageList_0;
        this.btnItemDelete.Location = new Point(0xb6, 0x7f);
        this.btnItemDelete.Name = "btnItemDelete";
        this.btnItemDelete.Size = new Size(0x60, 0x18);
        this.btnItemDelete.TabIndex = 4;
        this.btnItemDelete.Text = "Delete Field";
        this.btnItemDelete.TextAlign = ContentAlignment.MiddleRight;
        this.btnItemDelete.UseVisualStyleBackColor = true;
        this.btnItemDelete.Click += new EventHandler(this.btnItemDelete_Click);
        this.btnItemAdd.ImageAlign = ContentAlignment.MiddleLeft;
        this.btnItemAdd.ImageIndex = 0;
        this.btnItemAdd.ImageList = this.imageList_0;
        this.btnItemAdd.Location = new Point(0xb6, 0x60);
        this.btnItemAdd.Name = "btnItemAdd";
        this.btnItemAdd.Size = new Size(0x60, 0x18);
        this.btnItemAdd.TabIndex = 3;
        this.btnItemAdd.Text = "Add Field...";
        this.btnItemAdd.TextAlign = ContentAlignment.MiddleRight;
        this.btnItemAdd.UseVisualStyleBackColor = true;
        this.btnItemAdd.Click += new EventHandler(this.btnItemAdd_Click);
        this.btnMoveItemDown.ImageAlign = ContentAlignment.MiddleLeft;
        this.btnMoveItemDown.ImageIndex = 3;
        this.btnMoveItemDown.ImageList = this.imageList_0;
        this.btnMoveItemDown.Location = new Point(0xb6, 50);
        this.btnMoveItemDown.Name = "btnMoveItemDown";
        this.btnMoveItemDown.Size = new Size(0x60, 0x18);
        this.btnMoveItemDown.TabIndex = 2;
        this.btnMoveItemDown.Text = "Move down";
        this.btnMoveItemDown.TextAlign = ContentAlignment.MiddleRight;
        this.btnMoveItemDown.UseVisualStyleBackColor = true;
        this.btnMoveItemDown.Click += new EventHandler(this.btnMoveItemDown_Click);
        this.btnMoveItemUp.ImageAlign = ContentAlignment.MiddleLeft;
        this.btnMoveItemUp.ImageIndex = 4;
        this.btnMoveItemUp.ImageList = this.imageList_0;
        this.btnMoveItemUp.Location = new Point(0xb6, 0x13);
        this.btnMoveItemUp.Name = "btnMoveItemUp";
        this.btnMoveItemUp.Size = new Size(0x60, 0x18);
        this.btnMoveItemUp.TabIndex = 1;
        this.btnMoveItemUp.Text = "Move up";
        this.btnMoveItemUp.TextAlign = ContentAlignment.MiddleRight;
        this.btnMoveItemUp.UseVisualStyleBackColor = true;
        this.btnMoveItemUp.Click += new EventHandler(this.btnMoveItemUp_Click);
        this.lstFieldOrder.FormattingEnabled = true;
        this.lstFieldOrder.Location = new Point(6, 0x13);
        this.lstFieldOrder.Name = "lstFieldOrder";
        this.lstFieldOrder.Size = new Size(170, 0x108);
        this.lstFieldOrder.TabIndex = 0;
        this.lstFieldOrder.SelectedIndexChanged += new EventHandler(this.lstFieldOrder_SelectedIndexChanged);
        this.grpFormatOptions.Controls.Add(this.btnFormatsHelp);
        this.grpFormatOptions.Controls.Add(this.lblThousandsSeparator);
        this.grpFormatOptions.Controls.Add(this.cmbThousandsSeparator);
        this.grpFormatOptions.Controls.Add(this.numImpliedDecimals);
        this.grpFormatOptions.Controls.Add(this.numLastLines);
        this.grpFormatOptions.Controls.Add(this.numFirstLines);
        this.grpFormatOptions.Controls.Add(this.lblImpliedDecimals);
        this.grpFormatOptions.Controls.Add(this.lblVolumeMultiple);
        this.grpFormatOptions.Controls.Add(this.lblIgnoreLastLines);
        this.grpFormatOptions.Controls.Add(this.lblIgnoreFirstLines);
        this.grpFormatOptions.Controls.Add(this.cmbVolumeMultiple);
        this.grpFormatOptions.Controls.Add(this.cmbDecimalSeparator);
        this.grpFormatOptions.Controls.Add(this.cmbFieldSeparator);
        this.grpFormatOptions.Controls.Add(this.cmbTimeFormat);
        this.grpFormatOptions.Controls.Add(this.btnCommanFormats);
        this.grpFormatOptions.Controls.Add(this.cmbDateFormat);
        this.grpFormatOptions.Controls.Add(this.lblDecimalSeparator);
        this.grpFormatOptions.Controls.Add(this.lblFieldSeparator);
        this.grpFormatOptions.Controls.Add(this.lblTimeFormat);
        this.grpFormatOptions.Controls.Add(this.lblDateFormat);
        this.grpFormatOptions.Location = new Point(0x129, 7);
        this.grpFormatOptions.Name = "grpFormatOptions";
        this.grpFormatOptions.Size = new Size(0x103, 0x124);
        this.grpFormatOptions.TabIndex = 4;
        this.grpFormatOptions.TabStop = false;
        this.grpFormatOptions.Text = "Format Options";
        this.btnFormatsHelp.Image = Resources.help;
        this.btnFormatsHelp.ImageAlign = ContentAlignment.MiddleLeft;
        this.btnFormatsHelp.Location = new Point(0x83, 0x13);
        this.btnFormatsHelp.Name = "btnFormatsHelp";
        this.btnFormatsHelp.Size = new Size(0x7a, 0x18);
        this.btnFormatsHelp.TabIndex = 0x16;
        this.btnFormatsHelp.Text = "Formats Help...";
        this.btnFormatsHelp.TextAlign = ContentAlignment.MiddleRight;
        this.btnFormatsHelp.UseVisualStyleBackColor = true;
        this.btnFormatsHelp.Click += new EventHandler(this.btnFormatsHelp_Click);
        this.lblThousandsSeparator.AutoSize = true;
        this.lblThousandsSeparator.Location = new Point(6, 0xa1);
        this.lblThousandsSeparator.Name = "lblThousandsSeparator";
        this.lblThousandsSeparator.Size = new Size(0x70, 13);
        this.lblThousandsSeparator.TabIndex = 0x15;
        this.lblThousandsSeparator.Text = "Thousands Separator:";
        this.cmbThousandsSeparator.FormattingEnabled = true;
        this.cmbThousandsSeparator.Items.AddRange(new object[] { "None", "Space", "Period", "Comma" });
        this.cmbThousandsSeparator.Location = new Point(0x77, 0x9e);
        this.cmbThousandsSeparator.Name = "cmbThousandsSeparator";
        this.cmbThousandsSeparator.Size = new Size(0x86, 0x15);
        this.cmbThousandsSeparator.TabIndex = 20;
        this.numImpliedDecimals.Location = new Point(0x9e, 0x109);
        this.numImpliedDecimals.Name = "numImpliedDecimals";
        this.numImpliedDecimals.Size = new Size(0x5f, 20);
        this.numImpliedDecimals.TabIndex = 0x13;
        this.numLastLines.Location = new Point(0x9e, 0xd3);
        this.numLastLines.Name = "numLastLines";
        this.numLastLines.Size = new Size(0x5f, 20);
        this.numLastLines.TabIndex = 0x12;
        this.numFirstLines.Location = new Point(0x9e, 0xb9);
        this.numFirstLines.Name = "numFirstLines";
        this.numFirstLines.Size = new Size(0x5f, 20);
        this.numFirstLines.TabIndex = 0x11;
        this.lblImpliedDecimals.AutoSize = true;
        this.lblImpliedDecimals.Location = new Point(6, 0x10c);
        this.lblImpliedDecimals.Name = "lblImpliedDecimals";
        this.lblImpliedDecimals.Size = new Size(0x56, 13);
        this.lblImpliedDecimals.TabIndex = 0x10;
        this.lblImpliedDecimals.Text = "Implied Decimals";
        this.lblVolumeMultiple.AutoSize = true;
        this.lblVolumeMultiple.Location = new Point(6, 0xf1);
        this.lblVolumeMultiple.Name = "lblVolumeMultiple";
        this.lblVolumeMultiple.Size = new Size(0x51, 13);
        this.lblVolumeMultiple.TabIndex = 15;
        this.lblVolumeMultiple.Text = "Volume Multiple";
        this.lblIgnoreLastLines.AutoSize = true;
        this.lblIgnoreLastLines.Location = new Point(6, 0xd6);
        this.lblIgnoreLastLines.Name = "lblIgnoreLastLines";
        this.lblIgnoreLastLines.Size = new Size(0x76, 13);
        this.lblIgnoreLastLines.TabIndex = 14;
        this.lblIgnoreLastLines.Text = "Ignore Last Lines in File";
        this.lblIgnoreFirstLines.AutoSize = true;
        this.lblIgnoreFirstLines.Location = new Point(6, 0xbb);
        this.lblIgnoreFirstLines.Name = "lblIgnoreFirstLines";
        this.lblIgnoreFirstLines.Size = new Size(0x75, 13);
        this.lblIgnoreFirstLines.TabIndex = 13;
        this.lblIgnoreFirstLines.Text = "Ignore First Lines in File";
        this.cmbVolumeMultiple.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbVolumeMultiple.FormattingEnabled = true;
        this.cmbVolumeMultiple.Items.AddRange(new object[] { "1", "10", "100", "1000", "10000" });
        this.cmbVolumeMultiple.Location = new Point(0x9e, 0xee);
        this.cmbVolumeMultiple.Name = "cmbVolumeMultiple";
        this.cmbVolumeMultiple.Size = new Size(0x5f, 0x15);
        this.cmbVolumeMultiple.TabIndex = 11;
        this.cmbDecimalSeparator.FormattingEnabled = true;
        this.cmbDecimalSeparator.Items.AddRange(new object[] { "Period", "Comma" });
        this.cmbDecimalSeparator.Location = new Point(0x77, 0x83);
        this.cmbDecimalSeparator.Name = "cmbDecimalSeparator";
        this.cmbDecimalSeparator.Size = new Size(0x86, 0x15);
        this.cmbDecimalSeparator.TabIndex = 8;
        this.cmbFieldSeparator.FormattingEnabled = true;
        this.cmbFieldSeparator.Items.AddRange(new object[] { "Comma", "Space", "Tab" });
        this.cmbFieldSeparator.Location = new Point(0x77, 0x68);
        this.cmbFieldSeparator.Name = "cmbFieldSeparator";
        this.cmbFieldSeparator.Size = new Size(0x86, 0x15);
        this.cmbFieldSeparator.TabIndex = 7;
        this.cmbTimeFormat.FormattingEnabled = true;
        this.cmbTimeFormat.Items.AddRange(new object[] { "H.mm", "H.mm.ss", "H:mm", "H:mm:ss", "Hmm", "Hmmss", "hh.mm tt", "hh.mm.ss tt", "hh:mm tt", "hh:mm:ss tt", "hhmm tt", "hhmmss tt" });
        this.cmbTimeFormat.Location = new Point(0x53, 0x4d);
        this.cmbTimeFormat.Name = "cmbTimeFormat";
        this.cmbTimeFormat.Size = new Size(170, 0x15);
        this.cmbTimeFormat.TabIndex = 6;
        this.btnCommanFormats.Image = Resources.folder_page_white;
        this.btnCommanFormats.ImageAlign = ContentAlignment.MiddleLeft;
        this.btnCommanFormats.Location = new Point(6, 0x13);
        this.btnCommanFormats.Name = "btnCommanFormats";
        this.btnCommanFormats.Size = new Size(0x7a, 0x18);
        this.btnCommanFormats.TabIndex = 5;
        this.btnCommanFormats.Text = "Common Formats...";
        this.btnCommanFormats.TextAlign = ContentAlignment.MiddleRight;
        this.btnCommanFormats.UseVisualStyleBackColor = true;
        this.btnCommanFormats.Click += new EventHandler(this.btnCommanFormats_Click);
        this.cmbDateFormat.FormattingEnabled = true;
        this.cmbDateFormat.Items.AddRange(new object[] { 
            "d/M/yyyy", "d/M/yyyy H:mm", "d/M/yyyy hh:mm tt", "ddMMyyyy", "d-MMM-yyyy", "d-M-yyyy", "d.M.yy", "d.M.yy H:mm", "dd.MM.yyyy", "dd.MM.yyyy H:mm", "M/d/yyyy", "M/d/yyyy H:mm", "M/d/yyyy hh:mm tt", "M-d-yyyy", "MMddyy", "MMddyyyy", 
            "MMM d, yyyy H:mm", "yyddMM", "yyMMdd", "yyyy/MM/dd", "yyyyddMM", "yyyyddMM H:mm", "yyyyMMdd", "yyyy-MM-dd", "yyyyMMdd H:mm", "yyyy-MM-dd H:mm"
         });
        this.cmbDateFormat.Location = new Point(0x53, 50);
        this.cmbDateFormat.Name = "cmbDateFormat";
        this.cmbDateFormat.Size = new Size(170, 0x15);
        this.cmbDateFormat.TabIndex = 4;
        this.lblDecimalSeparator.AutoSize = true;
        this.lblDecimalSeparator.Location = new Point(6, 0x86);
        this.lblDecimalSeparator.Name = "lblDecimalSeparator";
        this.lblDecimalSeparator.Size = new Size(0x61, 13);
        this.lblDecimalSeparator.TabIndex = 3;
        this.lblDecimalSeparator.Text = "Decimal Separator:";
        this.lblFieldSeparator.AutoSize = true;
        this.lblFieldSeparator.Location = new Point(6, 0x6b);
        this.lblFieldSeparator.Name = "lblFieldSeparator";
        this.lblFieldSeparator.Size = new Size(0x51, 13);
        this.lblFieldSeparator.TabIndex = 2;
        this.lblFieldSeparator.Text = "Field Separator:";
        this.lblTimeFormat.AutoSize = true;
        this.lblTimeFormat.Location = new Point(6, 80);
        this.lblTimeFormat.Name = "lblTimeFormat";
        this.lblTimeFormat.Size = new Size(0x44, 13);
        this.lblTimeFormat.TabIndex = 1;
        this.lblTimeFormat.Text = "Time Format:";
        this.lblDateFormat.AutoSize = true;
        this.lblDateFormat.Location = new Point(6, 0x35);
        this.lblDateFormat.Name = "lblDateFormat";
        this.lblDateFormat.Size = new Size(0x44, 13);
        this.lblDateFormat.TabIndex = 0;
        this.lblDateFormat.Text = "Date Format:";
        this.cmnuAddField.Items.AddRange(new ToolStripItem[] { this.mnuDate, this.mnuTime, this.mnuOpen, this.mnuHigh, this.mnuLow, this.mnuClose, this.mnuVolume, this.mnuSecurityName, this.mnuOpenInterest, this.mnuFiller, this.mnuCustomField });
        this.cmnuAddField.Name = "cmnuAddField";
        this.cmnuAddField.Size = new Size(0xcc, 0xf6);
        this.cmnuAddField.Opening += new CancelEventHandler(this.cmnuAddField_Opening);
        this.mnuDate.Name = "mnuDate";
        this.mnuDate.Size = new Size(0xcb, 0x16);
        this.mnuDate.Tag = "Date";
        this.mnuDate.Text = "Date (or Date with Time)";
        this.mnuDate.Click += new EventHandler(this.mnuFiller_Click);
        this.mnuTime.Name = "mnuTime";
        this.mnuTime.Size = new Size(0xcb, 0x16);
        this.mnuTime.Tag = "Time";
        this.mnuTime.Text = "Time";
        this.mnuTime.Click += new EventHandler(this.mnuFiller_Click);
        this.mnuOpen.Name = "mnuOpen";
        this.mnuOpen.Size = new Size(0xcb, 0x16);
        this.mnuOpen.Tag = "Open";
        this.mnuOpen.Text = "Open";
        this.mnuOpen.Click += new EventHandler(this.mnuFiller_Click);
        this.mnuHigh.Name = "mnuHigh";
        this.mnuHigh.Size = new Size(0xcb, 0x16);
        this.mnuHigh.Tag = "High";
        this.mnuHigh.Text = "High";
        this.mnuHigh.Click += new EventHandler(this.mnuFiller_Click);
        this.mnuLow.Name = "mnuLow";
        this.mnuLow.Size = new Size(0xcb, 0x16);
        this.mnuLow.Tag = "Low";
        this.mnuLow.Text = "Low";
        this.mnuLow.Click += new EventHandler(this.mnuFiller_Click);
        this.mnuClose.Name = "mnuClose";
        this.mnuClose.Size = new Size(0xcb, 0x16);
        this.mnuClose.Tag = "Close";
        this.mnuClose.Text = "Close";
        this.mnuClose.Click += new EventHandler(this.mnuFiller_Click);
        this.mnuVolume.Name = "mnuVolume";
        this.mnuVolume.Size = new Size(0xcb, 0x16);
        this.mnuVolume.Tag = "Volume";
        this.mnuVolume.Text = "Volume";
        this.mnuVolume.Click += new EventHandler(this.mnuFiller_Click);
        this.mnuSecurityName.Name = "mnuSecurityName";
        this.mnuSecurityName.Size = new Size(0xcb, 0x16);
        this.mnuSecurityName.Tag = "SecurityName";
        this.mnuSecurityName.Text = "Security Name";
        this.mnuSecurityName.Click += new EventHandler(this.mnuFiller_Click);
        this.mnuOpenInterest.Name = "mnuOpenInterest";
        this.mnuOpenInterest.Size = new Size(0xcb, 0x16);
        this.mnuOpenInterest.Tag = "OpenInterest";
        this.mnuOpenInterest.Text = "Open Interest";
        this.mnuOpenInterest.Click += new EventHandler(this.mnuFiller_Click);
        this.mnuFiller.Name = "mnuFiller";
        this.mnuFiller.Size = new Size(0xcb, 0x16);
        this.mnuFiller.Tag = "Filler";
        this.mnuFiller.Text = "Filler";
        this.mnuFiller.Click += new EventHandler(this.mnuFiller_Click);
        this.mnuCustomField.Name = "mnuCustomField";
        this.mnuCustomField.Size = new Size(0xcb, 0x16);
        this.mnuCustomField.Tag = "Custom";
        this.mnuCustomField.Text = "Custom Field";
        this.cmnuCommanFormats.Items.AddRange(new ToolStripItem[] { this.testToolStripMenuItem });
        this.cmnuCommanFormats.Name = "cmnuCommanFormats";
        this.cmnuCommanFormats.Size = new Size(0x69, 0x1a);
        this.cmnuCommanFormats.Opening += new CancelEventHandler(this.cmnuCommanFormats_Opening);
        this.testToolStripMenuItem.Name = "testToolStripMenuItem";
        this.testToolStripMenuItem.Size = new Size(0x68, 0x16);
        this.testToolStripMenuItem.Text = "test";
        this.btnViewDataFile.Image = Resources.notepad;
        this.btnViewDataFile.ImageAlign = ContentAlignment.MiddleLeft;
        this.btnViewDataFile.Location = new Point(380, 0x131);
        this.btnViewDataFile.Name = "btnViewDataFile";
        this.btnViewDataFile.Size = new Size(0xb0, 0x18);
        this.btnViewDataFile.TabIndex = 0x17;
        this.btnViewDataFile.Text = "View Data File in Notepad...";
        this.btnViewDataFile.TextAlign = ContentAlignment.MiddleRight;
        this.btnViewDataFile.UseVisualStyleBackColor = true;
        this.btnViewDataFile.Click += new EventHandler(this.btnViewDataFile_Click);
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.btnViewDataFile);
        base.Controls.Add(this.grpFormatOptions);
        base.Controls.Add(this.grpFieldOrder);
        base.Name = "AsciiFilesWizardFieldOptions";
        base.Size = new Size(560, 0x161);
        this.grpFieldOrder.ResumeLayout(false);
        this.grpFormatOptions.ResumeLayout(false);
        this.grpFormatOptions.PerformLayout();
        this.numImpliedDecimals.EndInit();
        this.numLastLines.EndInit();
        this.numFirstLines.EndInit();
        this.cmnuAddField.ResumeLayout(false);
        this.cmnuCommanFormats.ResumeLayout(false);
        base.ResumeLayout(false);
    }

    private void lstFieldOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.lstFieldOrder.SelectedIndex != -1)
        {
            Field selectedItem = this.lstFieldOrder.SelectedItem as Field;
            if ((selectedItem.Type == FieldType.Date) || (selectedItem.Type == FieldType.Close))
            {
                this.btnItemDelete.Enabled = false;
            }
            else
            {
                this.btnItemDelete.Enabled = true;
            }
        }
    }

    private void method_0(object sender, EventArgs e)
    {
        if (this.textBox_0.Text != "")
        {
            this.mnuFiller_Click(this.mnuCustomField, null);
            this.cmnuAddField.Close();
        }
    }

    private void method_1()
    {
        this.cmbTimeFormat.Enabled = this.lblTimeFormat.Enabled = this.method_3("Time");
    }

    public void method_2()
    {
        string path = AsciiFilesStaticProvider.RootDataPath + "CommonFormats.xml";
        if (File.Exists(path))
        {
            this.asciiFilesDataSetList_0 = (AsciiFilesDataSetList) DataSetSettings.DeserializeFromFile(path);
            AsciiFilesDataSetList list = new AsciiFilesDataSetList();
            if (this.asciiFilesDataSetList_0.Version < list.Version)
            {
                List<string> names = this.asciiFilesDataSetList_0.GetNames();
                list.CreateDefaultFormats(this.asciiFilesDataSetList_0.Version);
                foreach (object obj2 in list.Items)
                {
                    AsciiFilesDataSet set = (AsciiFilesDataSet) obj2;
                    if (!names.Contains(set.Name))
                    {
                        this.asciiFilesDataSetList_0.Items.Add(set);
                    }
                }
                this.asciiFilesDataSetList_0.Version = list.Version;
                this.asciiFilesDataSetList_0.SerializeToFile(AsciiFilesStaticProvider.RootDataPath + "CommonFormats.xml");
            }
        }
        else
        {
            this.asciiFilesDataSetList_0 = new AsciiFilesDataSetList();
            this.asciiFilesDataSetList_0.CreateDefaultFormats(0);
        }
        this.asciiFilesDataSet_0 = this.asciiFilesDataSetList_0.Search("Default");
        if (this.asciiFilesDataSet_0 == null)
        {
            this.asciiFilesDataSet_0 = new AsciiFilesDataSet();
        }
        this.method_6(this.asciiFilesDataSet_0);
    }

    private bool method_3(string string_0)
    {
        bool flag;
        using (IEnumerator enumerator = this.lstFieldOrder.Items.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                if ((current as Field).Name == string_0)
                {
                    goto Label_0038;
                }
            }
            return false;
        Label_0038:
            flag = true;
        }
        return flag;
    }

    private void method_4(ToolStripMenuItem toolStripMenuItem_0)
    {
        toolStripMenuItem_0.Enabled = !this.method_3(toolStripMenuItem_0.Text);
    }

    public void method_5(AsciiFilesDataSet asciiFilesDataSet_1)
    {
        asciiFilesDataSet_1.Options.DateFormat = this.cmbDateFormat.Text.Trim();
        asciiFilesDataSet_1.Options.TimeFormat = this.cmbTimeFormat.Text.Trim();
        asciiFilesDataSet_1.Options.FieldSeparator = this.method_7(this.cmbFieldSeparator.Text.Trim());
        asciiFilesDataSet_1.Options.DecimalSeparator = this.method_7(this.cmbDecimalSeparator.Text.Trim());
        asciiFilesDataSet_1.Options.ThousandsSeparator = this.method_7(this.cmbThousandsSeparator.Text.Trim());
        asciiFilesDataSet_1.Options.IgnoreFirstLines = Convert.ToInt32(this.numFirstLines.Value);
        asciiFilesDataSet_1.Options.IgnoreLastLines = Convert.ToInt32(this.numLastLines.Value);
        asciiFilesDataSet_1.Options.VolumeMultiple = Convert.ToInt32(this.cmbVolumeMultiple.Text);
        asciiFilesDataSet_1.Options.ImpliedDecimals = Convert.ToInt32(this.numImpliedDecimals.Value);
        asciiFilesDataSet_1.Fields.Items.Clear();
        foreach (object obj2 in this.lstFieldOrder.Items)
        {
            asciiFilesDataSet_1.Fields.Items.Add((Field) obj2);
        }
    }

    private void method_6(AsciiFilesDataSet asciiFilesDataSet_1)
    {
        this.cmbDateFormat.Text = asciiFilesDataSet_1.Options.DateFormat;
        this.cmbTimeFormat.Text = asciiFilesDataSet_1.Options.TimeFormat;
        this.cmbFieldSeparator.Text = this.method_8(asciiFilesDataSet_1.Options.FieldSeparator);
        this.cmbDecimalSeparator.Text = this.method_8(asciiFilesDataSet_1.Options.DecimalSeparator);
        this.cmbThousandsSeparator.Text = this.method_8(asciiFilesDataSet_1.Options.ThousandsSeparator);
        this.numFirstLines.Value = Convert.ToDecimal(asciiFilesDataSet_1.Options.IgnoreFirstLines);
        this.numLastLines.Value = Convert.ToDecimal(asciiFilesDataSet_1.Options.IgnoreLastLines);
        this.cmbVolumeMultiple.Text = asciiFilesDataSet_1.Options.VolumeMultiple.ToString();
        this.numImpliedDecimals.Value = Convert.ToDecimal(asciiFilesDataSet_1.Options.ImpliedDecimals);
        this.lstFieldOrder.Items.Clear();
        foreach (object obj2 in asciiFilesDataSet_1.Fields.Items)
        {
            Field item = (Field) obj2;
            this.lstFieldOrder.Items.Add(item);
        }
        this.method_1();
    }

    private string method_7(string string_0)
    {
        switch (string_0.ToLower())
        {
            case "comma":
                return ",";

            case "period":
                return ".";

            case "space":
                return " ";

            case "space*":
                return " *";

            case "none":
                return "";

            case "tab":
                return "\t";
        }
        return string_0;
    }

    private string method_8(string string_0)
    {
        if (string_0 == ",")
        {
            return "Comma";
        }
        if (string_0 == " ")
        {
            return "Space";
        }
        if (string_0 == ".")
        {
            return "Period";
        }
        if (string_0 == "\t")
        {
            return "Tab";
        }
        if (string_0 == "")
        {
            return "None";
        }
        return string_0;
    }

    private void method_9(object sender, EventArgs e)
    {
        if ((sender as ToolStripMenuItem).Tag is AsciiFilesDataSet)
        {
            this.method_6((AsciiFilesDataSet) (sender as ToolStripMenuItem).Tag);
        }
        if ((sender as ToolStripMenuItem).Tag is string)
        {
            string tag = (sender as ToolStripMenuItem).Tag as string;
            if (tag == "Save")
            {
                FormatName name = new FormatName();
                List<string> names = this.asciiFilesDataSetList_0.GetNames();
                name.method_1(names);
                if (name.ShowDialog(this) == DialogResult.OK)
                {
                    AsciiFilesDataSet set = new AsciiFilesDataSet();
                    if (names.Contains(name.method_0()))
                    {
                        this.asciiFilesDataSetList_0.Delete(name.method_0());
                    }
                    this.method_5(set);
                    set.Name = name.method_0();
                    this.asciiFilesDataSetList_0.Items.Add(set);
                }
                name.Dispose();
            }
            switch (tag)
            {
                case "Load":
                    return;

                case "Delete":
                    this.asciiFilesDataSetList_0.Delete((sender as ToolStripMenuItem).Text);
                    break;
            }
        }
        this.asciiFilesDataSetList_0.SerializeToFile(AsciiFilesStaticProvider.RootDataPath + "CommonFormats.xml");
    }

    private void mnuFiller_Click(object sender, EventArgs e)
    {
        Field item = new Field {
            Type = (FieldType) Enum.Parse(typeof(FieldType), (sender as ToolStripMenuItem).Tag.ToString())
        };
        if (item.Type == FieldType.Custom)
        {
            item.Name = this.textBox_0.Text;
            if (this.method_3(item.Name))
            {
                this.cmnuAddField.Close();
                MessageBox.Show(string.Format("Custom Field \"{0}\" already present in DataSet.", item.Name), "ASCII Files DataSet", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
        else
        {
            item.Name = (sender as ToolStripMenuItem).Text;
        }
        this.lstFieldOrder.Items.Add(item);
        this.method_1();
    }

    private void textBox_0_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            this.method_0(null, null);
        }
    }
}

