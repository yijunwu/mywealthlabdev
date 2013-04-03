namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using WealthLabPro.Properties;

    public class ReferencesForm : Form
    {
        private Button btnAdd;
        private Button btnCancel;
        private Button btnOK;
        private Button btnRemove;
        private IContainer icontainer_0;
        private CheckedListBox lbFramework;
        private Label lblNote;
        private ListBox lbOther;
        private OpenFileDialog openFileDialog_0;
        private TabPage pageBrowse;
        private TabPage pageFramework;
        private string string_0;
        private TabControl tabReferences;

        public ReferencesForm(string references)
        {
            this.InitializeComponent();
            this.string_0 = references;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_0.ShowDialog() == DialogResult.OK)
            {
                string fileName = this.openFileDialog_0.FileName;
                if (!this.lbOther.Items.Contains(fileName))
                {
                    try
                    {
                        Assembly.LoadFile(fileName);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Could not add this Reference: " + exception.Message);
                        return;
                    }
                    this.lbOther.Items.Add(fileName);
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.lbOther.SelectedIndex >= 0)
            {
                this.lbOther.Items.RemoveAt(this.lbOther.SelectedIndex);
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
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.tabReferences = new TabControl();
            this.pageFramework = new TabPage();
            this.lbFramework = new CheckedListBox();
            this.pageBrowse = new TabPage();
            this.btnAdd = new Button();
            this.btnRemove = new Button();
            this.lbOther = new ListBox();
            this.lblNote = new Label();
            this.openFileDialog_0 = new OpenFileDialog();
            this.tabReferences.SuspendLayout();
            this.pageFramework.SuspendLayout();
            this.pageBrowse.SuspendLayout();
            base.SuspendLayout();
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(450, 0x145);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x171, 0x145);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.tabReferences.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabReferences.Controls.Add(this.pageFramework);
            this.tabReferences.Controls.Add(this.pageBrowse);
            this.tabReferences.Location = new Point(3, 1);
            this.tabReferences.Name = "tabReferences";
            this.tabReferences.SelectedIndex = 0;
            this.tabReferences.Size = new Size(0x213, 0x13e);
            this.tabReferences.TabIndex = 2;
            this.pageFramework.Controls.Add(this.lbFramework);
            this.pageFramework.Location = new Point(4, 0x16);
            this.pageFramework.Name = "pageFramework";
            this.pageFramework.Padding = new Padding(3);
            this.pageFramework.Size = new Size(0x20b, 0x124);
            this.pageFramework.TabIndex = 0;
            this.pageFramework.Text = ".NET Framework";
            this.pageFramework.UseVisualStyleBackColor = true;
            this.lbFramework.Dock = DockStyle.Fill;
            this.lbFramework.FormattingEnabled = true;
            this.lbFramework.Location = new Point(3, 3);
            this.lbFramework.Name = "lbFramework";
            this.lbFramework.Size = new Size(0x205, 0x112);
            this.lbFramework.TabIndex = 0;
            this.pageBrowse.Controls.Add(this.btnAdd);
            this.pageBrowse.Controls.Add(this.btnRemove);
            this.pageBrowse.Controls.Add(this.lbOther);
            this.pageBrowse.Location = new Point(4, 0x16);
            this.pageBrowse.Name = "pageBrowse";
            this.pageBrowse.Padding = new Padding(3);
            this.pageBrowse.Size = new Size(0x20b, 0x124);
            this.pageBrowse.TabIndex = 1;
            this.pageBrowse.Text = "Other Assemblies to Reference";
            this.pageBrowse.UseVisualStyleBackColor = true;
            this.btnAdd.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnAdd.Location = new Point(0xad, 0x108);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(0x7e, 0x17);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add a Reference ...";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.btnRemove.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnRemove.Location = new Point(7, 0x108);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new Size(160, 0x17);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "Remove Selected Reference";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
            this.lbOther.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lbOther.FormattingEnabled = true;
            this.lbOther.Location = new Point(6, 6);
            this.lbOther.Name = "lbOther";
            this.lbOther.Size = new Size(0x1ff, 0xfb);
            this.lbOther.TabIndex = 0;
            this.lblNote.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.lblNote.Location = new Point(3, 0x142);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new Size(360, 0x1a);
            this.lblNote.TabIndex = 3;
            this.lblNote.Text = "Note: System, System.Drawing, and System.Windows.Forms are always referenced automatically.";
            this.openFileDialog_0.DefaultExt = "dll";
            this.openFileDialog_0.Filter = ".NET Assemblies|*.dll";
            this.openFileDialog_0.Title = "Locate the .NET Assembly to Reference";
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x216, 0x165);
            base.Controls.Add(this.lblNote);
            base.Controls.Add(this.tabReferences);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ReferencesForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = ".NET References";
            base.Load += new EventHandler(this.ReferencesForm_Load);
            this.tabReferences.ResumeLayout(false);
            this.pageFramework.ResumeLayout(false);
            this.pageBrowse.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void method_0(string string_1)
        {
            int index = this.lbFramework.Items.IndexOf(string_1);
            if (index >= 0)
            {
                this.lbFramework.SetItemChecked(index, true);
            }
        }

        private void ReferencesForm_Load(object sender, EventArgs e)
        {
            foreach (string str3 in Resources.DotNetReferences.Split(new char[] { '\r', '\n' }))
            {
                if (str3.Trim() != "")
                {
                    this.lbFramework.Items.Add(str3);
                }
            }
            this.method_0("System");
            this.method_0("System.Drawing");
            this.method_0("System.Windows.Forms");
            string[] strArray5 = this.string_0.Split(new char[] { ';' });
            foreach (string str4 in strArray5)
            {
                if (str4.Trim() != "")
                {
                    this.method_0(str4);
                }
            }
            foreach (string str2 in strArray5)
            {
                if ((str2.Trim() != "") && (this.lbFramework.Items.IndexOf(str2) == -1))
                {
                    this.lbOther.Items.Add(str2);
                }
            }
        }

        public string References
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (int num in this.lbFramework.CheckedIndices)
                {
                    string str2 = (string) this.lbFramework.Items[num];
                    if ((!(str2 == "System") && !(str2 == "System.Drawing")) && !(str2 == "System.Windows.Forms"))
                    {
                        builder.Append(str2);
                        builder.Append(";");
                    }
                }
                foreach (string str in this.lbOther.Items)
                {
                    builder.Append(str);
                    builder.Append(";");
                }
                return builder.ToString();
            }
        }
    }
}

