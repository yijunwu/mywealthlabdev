namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;

    public class AggregateIndicatorIndexParameter : UserControl
    {
        private WealthLab.IndicatorHelper _helper;
        private string _indicatorString = "";
        private IContainer components;
        private GroupBox grpIndicators;
        private GroupBox grpParameters;
        private IndicatorTreeView indicators;
        private Panel pnlParams;
        private StrategyBuilder sb;

        public AggregateIndicatorIndexParameter()
        {
            this.InitializeComponent();
        }

        private void AggregateIndicatorIndexParameter_Load(object sender, EventArgs e)
        {
            this.IndicatorHelpers = this.sb.IndicatorHelpers;
            this.indicators.RecallExpandState();
            this.indicators.SynchIndicatorHelpers(this.IndicatorHelpers);
            if (this._indicatorString != "")
            {
                string[] strArray = this._indicatorString.Split(new char[] { '.' });
                TreeNode node = this.indicators.FindIndicatorNode(strArray[0]);
                this.indicators.SelectedNode = node;
                if (node != null)
                {
                    WealthLab.IndicatorHelper tag = node.Tag as WealthLab.IndicatorHelper;
                    IndicatorDragDropManager.SetIndicatorUIValues(tag, this._indicatorString, this.pnlParams);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void indicators_IndicatorSelected(object sender, IndicatorEventArgs e)
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
            this._helper = e.Helper;
            IndicatorDragDropManager.CreateIndicatorParameterUI(this._helper, this.pnlParams, null, "Bars", true, 10);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.pnlParams = new Panel();
            this.grpParameters = new GroupBox();
            this.grpIndicators = new GroupBox();
            this.indicators = new IndicatorTreeView();
            this.sb = new StrategyBuilder(this.components);
            this.grpParameters.SuspendLayout();
            this.grpIndicators.SuspendLayout();
            base.SuspendLayout();
            this.pnlParams.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnlParams.AutoScroll = true;
            this.pnlParams.Location = new Point(3, 0x10);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new Size(0x146, 0x8e);
            this.pnlParams.TabIndex = 0;
            this.grpParameters.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.grpParameters.Controls.Add(this.pnlParams);
            this.grpParameters.Location = new Point(0, 0xd0);
            this.grpParameters.Name = "grpParameters";
            this.grpParameters.Size = new Size(0x14c, 0xa1);
            this.grpParameters.TabIndex = 10;
            this.grpParameters.TabStop = false;
            this.grpParameters.Text = "Indicator Parameters";
            this.grpIndicators.Controls.Add(this.indicators);
            this.grpIndicators.Dock = DockStyle.Top;
            this.grpIndicators.Location = new Point(0, 0);
            this.grpIndicators.Name = "grpIndicators";
            this.grpIndicators.Size = new Size(0x14c, 0xcf);
            this.grpIndicators.TabIndex = 11;
            this.grpIndicators.TabStop = false;
            this.grpIndicators.Text = "Select Indicators";
            this.indicators.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.indicators.HideSelection = false;
            this.indicators.ImageIndex = 0;
            this.indicators.Location = new Point(3, 0x10);
            this.indicators.Name = "indicators";
            this.indicators.SelectedImageIndex = 0;
            this.indicators.Size = new Size(0x146, 0xbc);
            this.indicators.StandardNodeName = "Wealth-Lab Standard Indicators";
            this.indicators.TabIndex = 10;
            this.indicators.IndicatorSelected += new EventHandler<IndicatorEventArgs>(this.indicators_IndicatorSelected);
            this.sb.RootPath = "";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.grpIndicators);
            base.Controls.Add(this.grpParameters);
            base.Name = "AggregateIndicatorIndexParameter";
            base.Size = new Size(0x14c, 0x171);
            base.Load += new EventHandler(this.AggregateIndicatorIndexParameter_Load);
            this.grpParameters.ResumeLayout(false);
            this.grpIndicators.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public WealthLab.IndicatorHelper IndicatorHelper
        {
            get
            {
                return this._helper;
            }
        }

        private List<WealthLab.IndicatorHelper> IndicatorHelpers { get; set; }

        public string IndicatorString
        {
            get
            {
                if (this.SelectedIndicator == null)
                {
                    return "";
                }
                WealthLab.IndicatorHelper selectedIndicator = this.indicators.SelectedIndicator;
                if (selectedIndicator == null)
                {
                    return "";
                }
                return IndicatorDragDropManager.GetIndicatorString(selectedIndicator, this.pnlParams);
            }
        }

        public WealthLab.IndicatorHelper SelectedIndicator
        {
            get
            {
                return this.indicators.SelectedIndicator;
            }
        }
    }
}

