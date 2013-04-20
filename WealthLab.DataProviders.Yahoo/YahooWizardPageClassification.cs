using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using WealthLab.DataProviders.Helper;
using WealthLab.DataProviders.Yahoo;

internal class YahooWizardPageClassification : UserControl
{
    private bool onWait;
    private Button btnAddGroup;
    private Button btnRemoveGroup;
    private Button btnUpdateClassification;
    private ClassificationGroup classificationGroup_0;
    private ColumnHeader columnHeader_0;
    private Font font;
    private Font boldFont;
    private GroupBox grpClassification;
    private IContainer icontainer_0;
    private int symbolCount;
    private Label lblAvailableGroups;
    private Label lblClassification;
    private Label lblSelectedGroups;
    private Label lblUpdateClassification;
    private ListView lvSelected;
    private string string_0;
    private TreeView treeClassification;

    public YahooWizardPageClassification()
    {
        this.InitializeComponent();
    }

    private void btnAddGroup_Click(object sender, EventArgs e)
    {
        this.addGroupToSelected();
    }

    private void btnRemoveGroup_Click(object sender, EventArgs e)
    {
        this.removeGroupFromSelected();
    }

    private void btnUpdateClassification_Click(object sender, EventArgs e)
    {
        this.updateGroupSpec();
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
        this.grpClassification = new GroupBox();
        this.lblAvailableGroups = new Label();
        this.lblSelectedGroups = new Label();
        this.btnRemoveGroup = new Button();
        this.btnAddGroup = new Button();
        this.lvSelected = new ListView();
        this.columnHeader_0 = new ColumnHeader();
        this.lblUpdateClassification = new Label();
        this.btnUpdateClassification = new Button();
        this.lblClassification = new Label();
        this.treeClassification = new TreeView();
        this.grpClassification.SuspendLayout();
        base.SuspendLayout();
        this.grpClassification.Anchor = AnchorStyles.None;
        this.grpClassification.Controls.Add(this.lblAvailableGroups);
        this.grpClassification.Controls.Add(this.lblSelectedGroups);
        this.grpClassification.Controls.Add(this.btnRemoveGroup);
        this.grpClassification.Controls.Add(this.btnAddGroup);
        this.grpClassification.Controls.Add(this.lvSelected);
        this.grpClassification.Controls.Add(this.lblUpdateClassification);
        this.grpClassification.Controls.Add(this.btnUpdateClassification);
        this.grpClassification.Controls.Add(this.lblClassification);
        this.grpClassification.Controls.Add(this.treeClassification);
        this.grpClassification.Location = new Point(7, 7);
        this.grpClassification.Name = "grpClassification";
        this.grpClassification.RightToLeft = RightToLeft.No;
        this.grpClassification.Size = new Size(550, 0x157);
        this.grpClassification.TabIndex = 0;
        this.grpClassification.TabStop = false;
        this.grpClassification.Text = "Classification groups";
        this.lblAvailableGroups.AutoSize = true;
        this.lblAvailableGroups.ForeColor = SystemColors.ControlText;
        this.lblAvailableGroups.Location = new Point(6, 0x3b);
        this.lblAvailableGroups.Name = "lblAvailableGroups";
        this.lblAvailableGroups.Size = new Size(0x55, 13);
        this.lblAvailableGroups.TabIndex = 8;
        this.lblAvailableGroups.Text = "Available groups";
        this.lblSelectedGroups.AutoSize = true;
        this.lblSelectedGroups.ForeColor = SystemColors.ControlText;
        this.lblSelectedGroups.Location = new Point(0x120, 0x3b);
        this.lblSelectedGroups.Name = "lblSelectedGroups";
        this.lblSelectedGroups.Size = new Size(0x90, 13);
        this.lblSelectedGroups.TabIndex = 7;
        this.lblSelectedGroups.Text = "Selected groups (0 Symbols) ";
        this.btnRemoveGroup.Location = new Point(260, 0xc0);
        this.btnRemoveGroup.Name = "btnRemoveGroup";
        this.btnRemoveGroup.Size = new Size(0x19, 0x19);
        this.btnRemoveGroup.TabIndex = 6;
        this.btnRemoveGroup.Text = "<";
        this.btnRemoveGroup.UseVisualStyleBackColor = true;
        this.btnRemoveGroup.Click += new EventHandler(this.btnRemoveGroup_Click);
        this.btnAddGroup.Location = new Point(260, 0xa1);
        this.btnAddGroup.Name = "btnAddGroup";
        this.btnAddGroup.Size = new Size(0x19, 0x19);
        this.btnAddGroup.TabIndex = 5;
        this.btnAddGroup.Text = ">";
        this.btnAddGroup.UseVisualStyleBackColor = true;
        this.btnAddGroup.Click += new EventHandler(this.btnAddGroup_Click);
        this.lvSelected.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0 });
        this.lvSelected.HeaderStyle = ColumnHeaderStyle.None;
        this.lvSelected.HideSelection = false;
        this.lvSelected.Location = new Point(0x123, 0x4b);
        this.lvSelected.Name = "lvSelected";
        this.lvSelected.Size = new Size(0xfd, 230);
        this.lvSelected.TabIndex = 4;
        this.lvSelected.UseCompatibleStateImageBehavior = false;
        this.lvSelected.View = View.Details;
        this.lvSelected.SelectedIndexChanged += new EventHandler(this.lvSelected_SelectedIndexChanged);
        this.lvSelected.DoubleClick += new EventHandler(this.lvSelected_DoubleClick);
        this.columnHeader_0.Text = "Group";
        this.columnHeader_0.Width = 0xdd;
        this.lblUpdateClassification.AutoSize = true;
        this.lblUpdateClassification.Location = new Point(170, 0x13e);
        this.lblUpdateClassification.Name = "lblUpdateClassification";
        this.lblUpdateClassification.Size = new Size(0x48, 13);
        this.lblUpdateClassification.TabIndex = 3;
        this.lblUpdateClassification.Text = "Last updated:";
        this.lblUpdateClassification.Paint += new PaintEventHandler(this.lblUpdateClassification_Paint);
        this.btnUpdateClassification.Location = new Point(6, 0x139);
        this.btnUpdateClassification.Name = "btnUpdateClassification";
        this.btnUpdateClassification.Size = new Size(0x9e, 0x18);
        this.btnUpdateClassification.TabIndex = 2;
        this.btnUpdateClassification.Text = "Refresh classification";
        this.btnUpdateClassification.UseVisualStyleBackColor = true;
        this.btnUpdateClassification.Click += new EventHandler(this.btnUpdateClassification_Click);
        this.lblClassification.AutoSize = true;
        this.lblClassification.Location = new Point(6, 0x13);
        this.lblClassification.MaximumSize = new Size(550, 40);
        this.lblClassification.Name = "lblClassification";
        this.lblClassification.Size = new Size(0x211, 0x1a);
        this.lblClassification.TabIndex = 1;
        this.lblClassification.Text = "Select one or more Classification Groups below and add them to the Selected list to the right. Your DataSet will contain all of the Symbols in the selected Groups.";
        this.treeClassification.HideSelection = false;
        this.treeClassification.Location = new Point(6, 0x4b);
        this.treeClassification.Name = "treeClassification";
        this.treeClassification.Size = new Size(0xf8, 230);
        this.treeClassification.TabIndex = 0;
        this.treeClassification.DoubleClick += new EventHandler(this.treeClassification_DoubleClick);
        this.treeClassification.AfterSelect += new TreeViewEventHandler(this.treeClassification_AfterSelect);
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.grpClassification);
        base.Name = "YahooWizardPageClassification";
        this.RightToLeft = RightToLeft.Yes;
        base.Size = new Size(560, 0x161);
        base.Load += new EventHandler(this.YahooWizardPageClassification_Load);
        this.grpClassification.ResumeLayout(false);
        this.grpClassification.PerformLayout();
        base.ResumeLayout(false);
    }

    private void lblUpdateClassification_Paint(object sender, PaintEventArgs e)
    {
        this.lblUpdateClassification.Font = this.onWait ? this.boldFont : this.font;
    }

    private void lvSelected_DoubleClick(object sender, EventArgs e)
    {
        this.removeGroupFromSelected();
    }

    private void lvSelected_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.btnRemoveGroup.Enabled = true;
    }

    ///WYJ fix, original name: method_0
    public SymbolList generateSymbolList()
    {
        SymbolList sList = new SymbolList();
        foreach (ListViewItem item in this.lvSelected.Items)
        {
            sList.AddSymbols((item.Tag as ClassificationGroup).Symbols, DelimeterSetEnum.ForGuiInput);
        }
        return sList;
    }

    ///WYJ fix, original name: method_1
    public string SuggestedDataSourceName()
    {
        StringBuilder builder = new StringBuilder();
        foreach (ListViewItem item in this.lvSelected.Items)
        {
            builder.Append((item.Tag as ClassificationGroup).Name);
            builder.Append(", ");
        }
        string str = builder.ToString();
        if (str.Length > 0x67)
        {
            str = str.Remove(100) + "...";
        }
        return str.Trim(new char[] { ' ', ',' });
    }

    ///original name: method_10
    private void onDownloadCompleted(object sender, AsyncCompletedEventArgs e)
    {
        Application.DoEvents();
        if (e.Error != null)
        {
            MessageBox.Show(this, "Error while downloading Classification file: " + e.Error.Message, "File download error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        this.processClassificationFile();
    }

    ///original name: method_11
    private void countSymbols()
    {
        this.symbolCount = 0;
        foreach (ListViewItem item in this.lvSelected.Items)
        {
            this.symbolCount += Convert.ToInt32((item.Tag as ClassificationGroup).SymbolsCount);
        }
        this.lblSelectedGroups.Text = string.Format("Selected groups ({0} Symbols)", this.symbolCount);
    }

    ///original name: method_12
    private void addGroupToSelected()
    {
        TreeNode selectedNode = this.treeClassification.SelectedNode;
        if (selectedNode != null)
        {
            ClassificationGroup tag = (ClassificationGroup) selectedNode.Tag;
            if (tag.Type == "Symbols")
            {
                foreach (ListViewItem item in this.lvSelected.Items)
                {
                    if (item.Tag == tag)
                    {
                        return;
                    }
                }
                this.lvSelected.Items.Add(string.Format("{0} ({1})", tag.Name, tag.SymbolsCount)).Tag = tag;
                this.lvSelected.Items[this.lvSelected.Items.Count - 1].EnsureVisible();
                this.countSymbols();
            }
        }
    }

    ///original name: method_13
    private void removeGroupFromSelected()
    {
        if (this.lvSelected.SelectedItems != null)
        {
            foreach (ListViewItem item in this.lvSelected.SelectedItems)
            {
                item.Remove();
            }
        }
        this.btnRemoveGroup.Enabled = false;
        this.countSymbols();
    }

    ///WYJ fix, original name method_2
    public string getGroupListString()
    {
        StringBuilder builder = new StringBuilder();
        foreach (ListViewItem item in this.lvSelected.Items)
        {
            builder.Append((item.Tag as ClassificationGroup).ID);
            builder.Append(",");
        }
        return builder.ToString();
    }

    ///WYJ fix, original name method_3
    private void updateGroupSpec()
    {
        this.showWaitStatus();
        this.lblUpdateClassification.Text = "Please wait, updating Classification...";
        Application.DoEvents();
        YahooStaticProvider.ClassificationFile.UpdateClassificationGroupsFile(true);
    }

    ///original name: method_4
    private void updateGroupTreeWithData(ClassificationGroup classificationGroup_1, TreeNodeCollection treeNodeCollection_0)
    {
        for (int i = 0; i < classificationGroup_1.Groups.Count; i++)
        {
            TreeNode node = treeNodeCollection_0.Add(classificationGroup_1.Groups[i].Name);
            node.Tag = classificationGroup_1.Groups[i];
            if (classificationGroup_1.Groups[i].Type == "Symbols")
            {
                node.Text = node.Text + " (" + classificationGroup_1.Groups[i].SymbolsCount + ")";
            }
            this.updateGroupTreeWithData(classificationGroup_1.Groups[i], node.Nodes);
        }
    }

    ///WYJ fix, original name method_5
    private void processClassificationFile()
    {
        if (!YahooStaticProvider.ClassificationFile.FileExists())
        {
            this.lblUpdateClassification.Text = "Classification created:";
            this.showNormalStatus();
        }
        else
        {
            this.showWaitStatus();
            try
            {
                this.lblUpdateClassification.Text = "Please wait, processing the Classification data...";
                Application.DoEvents();
                this.treeClassification.Nodes.Clear();
                this.lvSelected.Items.Clear();
                this.classificationGroup_0 = YahooStaticProvider.ClassificationFile.ReadClassificationGroupFromFile();
                if (this.classificationGroup_0 != null)
                {
                    this.updateGroupTreeWithData(this.classificationGroup_0, this.treeClassification.Nodes);
                    if (this.classificationGroup_0 != null)
                    {
                        this.lblAvailableGroups.Text = string.Format("Available groups ({0} Symbols)", this.classificationGroup_0.SymbolsCount);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error processing Classification data", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                this.showNormalStatus();
            }
            this.lblUpdateClassification.Text = string.Format("Classification created: {0}", this.classificationGroup_0.Update.ToLocalTime());
            Application.DoEvents();
        }
    }

    private void showWaitStatus()
    {
        this.onWait = true;
        this.Cursor = Cursors.WaitCursor;
    }

    private void showNormalStatus()
    {
        this.onWait = false;
        this.Cursor = Cursors.Default;
    }

    ///WYJ fix, original name method_8
    public void InitStates(string string_1)
    {
        YahooStaticProvider.ClassificationFile.AddAsyncCompletedEventHandler(new AsyncCompletedEventHandler(this.onDownloadCompleted));
        YahooStaticProvider.ClassificationFile.AddDownloadProgressChangedEventHandler(new DownloadProgressChangedEventHandler(this.onDownloadProgress));
        this.string_0 = string_1;
        this.font = this.lblUpdateClassification.Font;
        this.boldFont = new Font(this.lblUpdateClassification.Font, FontStyle.Bold);
        this.lvSelected.Items.Clear();
        this.treeClassification.CollapseAll();
        this.btnAddGroup.Enabled = false;
        this.btnRemoveGroup.Enabled = false;
        this.countSymbols();
    }

    ///original name: method_9
    private void onDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
    {
        this.lblUpdateClassification.Text = string.Format("Download progress {0}%", e.ProgressPercentage);
        Application.DoEvents();
    }

    private void treeClassification_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if ((e.Node.Tag as ClassificationGroup).Type == "Symbols")
        {
            this.btnAddGroup.Enabled = true;
        }
        else
        {
            this.btnAddGroup.Enabled = false;
        }
    }

    private void treeClassification_DoubleClick(object sender, EventArgs e)
    {
        this.addGroupToSelected();
    }

    private void YahooWizardPageClassification_Load(object sender, EventArgs e)
    {
        Application.DoEvents();
        if (!YahooStaticProvider.ClassificationFile.FileExists())
        {
            if (MessageBox.Show("Classification data file was not found.\n\rDownload it now?", "Classification not found", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.updateGroupSpec();
            }
        }
        else
        {
            this.processClassificationFile();
        }
    }
}

