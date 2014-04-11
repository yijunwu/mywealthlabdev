namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class RuleFundamentalsForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private IContainer icontainer_0;
        private static string string_0 = "";
        private string itemName;
        private FundamentalTreeView treeFundamentals;
        private TextBox txtDescription;

        public RuleFundamentalsForm()
        {
            this.InitializeComponent();
        }

        public RuleFundamentalsForm(string itemName)
        {
            this.InitializeComponent();
            this.itemName = itemName;
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
            this.treeFundamentals = new FundamentalTreeView();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.txtDescription = new TextBox();
            base.SuspendLayout();
            this.treeFundamentals.ignoreDragandDrop = false;
            this.treeFundamentals.ImageIndex = 0;
            this.treeFundamentals.ItemHeight = 0x10;
            this.treeFundamentals.Location = new Point(13, 13);
            this.treeFundamentals.Name = "treeFundamentals";
            this.treeFundamentals.SelectedImageIndex = 0;
            this.treeFundamentals.Size = new Size(280, 0x17a);
            this.treeFundamentals.StandardNodeName = "";
            this.treeFundamentals.TabIndex = 0;
            this.treeFundamentals.AfterSelect += new TreeViewEventHandler(this.treeFundamentals_AfterSelect);
            this.treeFundamentals.FundamentalSelected += new EventHandler<FundamentalSelectedEventArgs>(this.method_0);
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new Point(0x89, 0x1bd);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xda, 0x1bd);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.txtDescription.BackColor = SystemColors.Info;
            this.txtDescription.Location = new Point(13, 0x18d);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = ScrollBars.Vertical;
            this.txtDescription.Size = new Size(280, 0x2a);
            this.txtDescription.TabIndex = 11;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x131, 0x1da);
            base.Controls.Add(this.txtDescription);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.treeFundamentals);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "RuleFundamentalsForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Select a Fundamental Item";
            base.Load += new EventHandler(this.RuleFundamentalsForm_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0(object sender, FundamentalSelectedEventArgs e)
        {
            this.itemName = e.ItemName;
        }

        private void RuleFundamentalsForm_Load(object sender, EventArgs e)
        {
            this.treeFundamentals.DataHost = MainModule.Instance.DataSources;
            this.treeFundamentals.ignoreDragandDrop = true;
            this.treeFundamentals.LoadNodes();
            this.treeFundamentals.SelectedNode = this.treeFundamentals.FindItem(this.itemName);
            if (this.treeFundamentals.SelectedNode == null)
            {
                this.treeFundamentals.SelectedNode = this.treeFundamentals.FindItem(string_0);
            }
        }

        private void treeFundamentals_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
            {
                this.btnOK.Enabled = false;
            }
            else
            {
                this.btnOK.Enabled = e.Node.Level == 1;
                if (e.Node.Level == 1)
                {
                    this.txtDescription.Text = ((FundamentalDataProvider) e.Node.Tag).ItemDescription(e.Node.Text);
                    string_0 = e.Node.Text;
                }
                else
                {
                    this.txtDescription.Text = "";
                }
            }
        }

        public string ItemName
        {
            get
            {
                return this.itemName;
            }
        }
    }
}

