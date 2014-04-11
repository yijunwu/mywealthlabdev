namespace WealthLabPro
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;

    public class RuleIndicatorsForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private GroupBox grpParameters;
        private IContainer icontainer_0;
        private WealthLab.IndicatorHelper indicatorHelper;
        private IndicatorTreeView indicators;
        [CompilerGenerated]
        private List<WealthLab.IndicatorHelper> indicatorHelpers;
        private Panel pnlParams;
        private static string string_0 = "";
        private string string_1;

        public RuleIndicatorsForm()
        {
            this.string_1 = "";
            this.InitializeComponent();
        }

        public RuleIndicatorsForm(string indicatorString)
        {
            this.string_1 = "";
            this.string_1 = indicatorString;
            this.InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string_0 = this.IndicatorString;
            base.DialogResult = DialogResult.OK;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void indicators_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
            {
                this.btnOK.Enabled = false;
            }
            else
            {
                this.btnOK.Enabled = e.Node.Level == 1;
            }
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            this.grpParameters = new GroupBox();
            this.pnlParams = new Panel();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.indicators = new IndicatorTreeView();
            this.grpParameters.SuspendLayout();
            base.SuspendLayout();
            this.grpParameters.Controls.Add(this.pnlParams);
            this.grpParameters.Location = new Point(8, 0x12e);
            this.grpParameters.Name = "grpParameters";
            this.grpParameters.Size = new Size(0x11f, 0x88);
            this.grpParameters.TabIndex = 6;
            this.grpParameters.TabStop = false;
            this.grpParameters.Text = "Indicator Parameters";
            this.pnlParams.AutoScroll = true;
            this.pnlParams.Location = new Point(6, 0x13);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new Size(0x113, 0x6f);
            this.pnlParams.TabIndex = 0;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xdb, 0x1bd);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new Point(0x8a, 0x1bd);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.indicators.HideSelection = false;
            this.indicators.ImageIndex = 0;
            this.indicators.Location = new Point(13, 13);
            this.indicators.Name = "indicators";
            this.indicators.SelectedImageIndex = 0;
            this.indicators.Size = new Size(0x119, 0x11b);
            this.indicators.StandardNodeName = "Wealth-Lab Standard Indicators";
            this.indicators.TabIndex = 9;
            this.indicators.IndicatorSelected += new EventHandler<IndicatorEventArgs>(this.method_0);
            this.indicators.AfterSelect += new TreeViewEventHandler(this.indicators_AfterSelect);
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x133, 0x1dc);
            base.Controls.Add(this.indicators);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.grpParameters);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "RuleIndicatorsForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Select an Indicator";
            base.Load += new EventHandler(this.RuleIndicatorsForm_Load);
            this.grpParameters.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void method_0(object sender, IndicatorEventArgs e)
        {
            foreach (WealthLab.IndicatorHelper helper in this.IndicatorHelpers)
            {
                if (helper.ParameterDisplayNames.Count == 0)
                {
                    foreach (string str in helper.ParameterDescriptions)
                    {
                        helper.ParameterDisplayNames.Add(str);
                    }
                }
            }
            this.indicatorHelper = e.Helper;
            IndicatorDragDropManager.CreateIndicatorParameterUI(this.indicatorHelper, this.pnlParams, null, "Bars", true);
        }

        private void RuleIndicatorsForm_Load(object sender, EventArgs e)
        {
            this.indicators.RecallExpandState();
            this.indicators.SynchIndicatorHelpers(this.IndicatorHelpers);
            if (this.string_1 != "")
            {
                string[] strArray = this.string_1.Split(new char[] { '.' });
                TreeNode node = this.indicators.FindIndicatorNode(strArray[0]);
                this.indicators.SelectedNode = node;
                if (node != null)
                {
                    WealthLab.IndicatorHelper tag = node.Tag as WealthLab.IndicatorHelper;
                    IndicatorDragDropManager.SetIndicatorUIValues(tag, this.string_1, this.pnlParams);
                }
            }
            if ((this.indicators.SelectedNode == null) && (string_0 != ""))
            {
                string[] strArray2 = string_0.Split(new char[] { '.' });
                TreeNode node2 = this.indicators.FindIndicatorNode(strArray2[0]);
                this.indicators.SelectedNode = node2;
                if (node2 != null)
                {
                    WealthLab.IndicatorHelper helper = node2.Tag as WealthLab.IndicatorHelper;
                    IndicatorDragDropManager.SetIndicatorUIValues(helper, string_0, this.pnlParams);
                }
            }
        }

        public WealthLab.IndicatorHelper IndicatorHelper
        {
            get
            {
                return this.indicatorHelper;
            }
        }

        public List<WealthLab.IndicatorHelper> IndicatorHelpers
        {
            [CompilerGenerated]
            get
            {
                return this.indicatorHelpers;
            }
            [CompilerGenerated]
            set
            {
                this.indicatorHelpers = value;
            }
        }

        public string IndicatorString
        {
            get
            {
                WealthLab.IndicatorHelper selectedIndicator = this.indicators.SelectedIndicator;
                if (selectedIndicator == null)
                {
                    return "";
                }
                return IndicatorDragDropManager.GetIndicatorString(selectedIndicator, this.pnlParams);
            }
        }
    }
}

