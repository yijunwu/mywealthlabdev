namespace WealthLabPro
{
    using CtrlLib;
    using Fidelity.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using WealthLab;
    using Label = System.Windows.Forms.Label;

    public class PositionSizeSelecterForm : Form
    {
        private bool combinationStrategyChildMode;
        private Button btnCancel;
        private Button btnConfigure;
        private Button btnOK;
        private ComboBox cmbPosSizers;
        private decimal maxEquityMarginFactor;
        private decimal equityMarginFactor;
        private IContainer icontainer_0;
        private Label lblMarginFactor;
        private Label lblMarginTo1;
        private Label lblPortSim;
        private Label lblRawProfit;
        private Label lblStartingCapital;
        private NumericUpDown numCapital;
        private NumericUpDown numDollar;
        private NumEdit numMarginFactor;
        private NumericUpDown numMaxRisk;
        private NumericUpDown numPctEquity;
        private NumericUpDown numRawDollar;
        private NumericUpDown numRawShare;
        private NumericUpDown numShare;
        private Panel panel1;
        private Panel panel2;
        private Panel pnlPortSim;
        private RadioButton rbDollar;
        private RadioButton rbFixedDollar;
        private RadioButton rbFixedShare;
        private RadioButton rbMaxRisk;
        private RadioButton rbPctEquity;
        private RadioButton rbPosSizer;
        private RadioButton rbScriptOverride;
        private RadioButton rbShare;
        private string string_0 = "";

        public PositionSizeSelecterForm(List<PosSizer> posSizers)
        {
            this.InitializeComponent();
            this.method_1();
            base.ActiveControl = this.btnOK;
            MainModule.Instance.HelpProvider.SetHelpNavigator(this, HelpNavigator.Topic);
            MainModule.Instance.HelpProvider.SetHelpKeyword(this, "Position_Size_Control.htm");
            foreach (PosSizer sizer in posSizers)
            {
                this.cmbPosSizers.Items.Add((PosSizer) Activator.CreateInstance(sizer.GetType()));
            }
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            ICustomSettings selectedPosSizer = this.SelectedPosSizer as ICustomSettings;
            if (selectedPosSizer != null)
            {
                UserControl settingsUI = selectedPosSizer.GetSettingsUI();
                ChartSettingsForm form = new ChartSettingsForm {
                    Text = "PosSizer Settings"
                };
                form.AddSettingsUI(settingsUI);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    selectedPosSizer.ChangeSettings(settingsUI);
                    selectedPosSizer.WriteSettings(this.SettingsHost);
                    this.string_0 = "*" + this.SelectedPosSizer.FriendlyName + "^" + this.SelectedPosSizer.GetConfigString();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.rbPosSizer.Checked)
            {
                if (this.cmbPosSizers.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a PosSizer to use.");
                }
                else
                {
                    base.DialogResult = DialogResult.OK;
                }
            }
            else if (this.rbFixedDollar.Checked && (this.numRawDollar.Value <= 0M))
            {
                MessageBox.Show("Raw profit Dollar position size must be greater than zero.");
            }
            else if (this.rbDollar.Checked && (this.numDollar.Value <= 0M))
            {
                MessageBox.Show("Dollar position size must be greater than zero.");
            }
            else if (this.rbPctEquity.Checked && (this.numPctEquity.Value <= 0M))
            {
                MessageBox.Show("Percent of Equity position size must be greater than zero.");
            }
            else if (this.rbMaxRisk.Checked && (this.numMaxRisk.Value <= 0M))
            {
                MessageBox.Show("Max Risk position size must be greater than zero.");
            }
            else if (this.rbPosSizer.Checked && (this.cmbPosSizers.SelectedIndex == -1))
            {
                MessageBox.Show("PosSizer must be selected.");
            }
            else if (this.RawProfitMode && (this.numCapital.Value <= 0M))
            {
                MessageBox.Show("Starting Capital must be greater than zero.");
            }
            else if (this.numMarginFactor.Value <= 0M)
            {
                MessageBox.Show("Margin Factor must be greater than zero.");
            }
            else
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        private void cmbPosSizers_Enter(object sender, EventArgs e)
        {
            this.rbPosSizer.Checked = true;
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
            this.lblRawProfit = new Label();
            this.rbFixedDollar = new RadioButton();
            this.numRawDollar = new NumericUpDown();
            this.rbFixedShare = new RadioButton();
            this.numRawShare = new NumericUpDown();
            this.panel1 = new Panel();
            this.lblPortSim = new Label();
            this.rbDollar = new RadioButton();
            this.numDollar = new NumericUpDown();
            this.rbShare = new RadioButton();
            this.numShare = new NumericUpDown();
            this.rbPctEquity = new RadioButton();
            this.numPctEquity = new NumericUpDown();
            this.rbMaxRisk = new RadioButton();
            this.numMaxRisk = new NumericUpDown();
            this.rbPosSizer = new RadioButton();
            this.cmbPosSizers = new ComboBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.lblStartingCapital = new Label();
            this.numCapital = new NumericUpDown();
            this.lblMarginFactor = new Label();
            this.numMarginFactor = new NumEdit();
            this.lblMarginTo1 = new Label();
            this.rbScriptOverride = new RadioButton();
            this.panel2 = new Panel();
            this.btnConfigure = new Button();
            this.pnlPortSim = new Panel();
            this.numRawDollar.BeginInit();
            this.numRawShare.BeginInit();
            this.numDollar.BeginInit();
            this.numShare.BeginInit();
            this.numPctEquity.BeginInit();
            this.numMaxRisk.BeginInit();
            this.numCapital.BeginInit();
            this.pnlPortSim.SuspendLayout();
            base.SuspendLayout();
            this.lblRawProfit.AutoSize = true;
            this.lblRawProfit.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblRawProfit.Location = new Point(4, 4);
            this.lblRawProfit.Name = "lblRawProfit";
            this.lblRawProfit.Size = new Size(0x65, 13);
            this.lblRawProfit.TabIndex = 0;
            this.lblRawProfit.Text = "Raw Profit Mode";
            this.rbFixedDollar.AutoSize = true;
            this.rbFixedDollar.Location = new Point(7, 0x15);
            this.rbFixedDollar.Name = "rbFixedDollar";
            this.rbFixedDollar.Size = new Size(80, 0x11);
            this.rbFixedDollar.TabIndex = 1;
            this.rbFixedDollar.TabStop = true;
            this.rbFixedDollar.Tag = "R";
            this.rbFixedDollar.Text = "Fixed Dollar";
            this.rbFixedDollar.UseVisualStyleBackColor = true;
            this.rbFixedDollar.CheckedChanged += new EventHandler(this.rbScriptOverride_CheckedChanged);
            this.numRawDollar.ForeColor = SystemColors.GrayText;
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.numRawDollar.Increment = new decimal(bits);
            this.numRawDollar.Location = new Point(0x8b, 0x15);
            int[] numArray2 = new int[4];
            numArray2[0] = 0xf4240;
            this.numRawDollar.Maximum = new decimal(numArray2);
            this.numRawDollar.Name = "numRawDollar";
            this.numRawDollar.Size = new Size(0x43, 20);
            this.numRawDollar.TabIndex = 2;
            int[] numArray3 = new int[4];
            numArray3[0] = 0x1388;
            this.numRawDollar.Value = new decimal(numArray3);
            this.numRawDollar.Enter += new EventHandler(this.numRawDollar_Enter);
            this.rbFixedShare.AutoSize = true;
            this.rbFixedShare.Location = new Point(7, 0x29);
            this.rbFixedShare.Name = "rbFixedShare";
            this.rbFixedShare.Size = new Size(0x6c, 0x11);
            this.rbFixedShare.TabIndex = 3;
            this.rbFixedShare.TabStop = true;
            this.rbFixedShare.Tag = "R";
            this.rbFixedShare.Text = "Shares/Contracts";
            this.rbFixedShare.UseVisualStyleBackColor = true;
            this.rbFixedShare.CheckedChanged += new EventHandler(this.rbScriptOverride_CheckedChanged);
            this.numRawShare.ForeColor = SystemColors.GrayText;
            int[] numArray4 = new int[4];
            numArray4[0] = 100;
            this.numRawShare.Increment = new decimal(numArray4);
            this.numRawShare.Location = new Point(0x8b, 0x29);
            int[] numArray5 = new int[4];
            numArray5[0] = 0xf4240;
            this.numRawShare.Maximum = new decimal(numArray5);
            int[] numArray6 = new int[4];
            numArray6[0] = 1;
            this.numRawShare.Minimum = new decimal(numArray6);
            this.numRawShare.Name = "numRawShare";
            this.numRawShare.Size = new Size(0x43, 20);
            this.numRawShare.TabIndex = 4;
            int[] numArray7 = new int[4];
            numArray7[0] = 100;
            this.numRawShare.Value = new decimal(numArray7);
            this.numRawShare.Enter += new EventHandler(this.numRawShare_Enter);
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Location = new Point(12, 0x43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0xc2, 1);
            this.panel1.TabIndex = 6;
            this.lblPortSim.AutoSize = true;
            this.lblPortSim.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblPortSim.Location = new Point(3, 0);
            this.lblPortSim.Name = "lblPortSim";
            this.lblPortSim.Size = new Size(0x97, 13);
            this.lblPortSim.TabIndex = 5;
            this.lblPortSim.Text = "Portfolio Simulation Mode";
            this.rbDollar.AutoSize = true;
            this.rbDollar.Location = new Point(6, 40);
            this.rbDollar.Name = "rbDollar";
            this.rbDollar.Size = new Size(80, 0x11);
            this.rbDollar.TabIndex = 9;
            this.rbDollar.TabStop = true;
            this.rbDollar.Tag = "P";
            this.rbDollar.Text = "Fixed Dollar";
            this.rbDollar.UseVisualStyleBackColor = true;
            this.rbDollar.CheckedChanged += new EventHandler(this.rbScriptOverride_CheckedChanged);
            this.numDollar.ForeColor = SystemColors.GrayText;
            int[] numArray8 = new int[4];
            numArray8[0] = 0x3e8;
            this.numDollar.Increment = new decimal(numArray8);
            this.numDollar.Location = new Point(0x87, 40);
            int[] numArray9 = new int[4];
            numArray9[0] = 0xf4240;
            this.numDollar.Maximum = new decimal(numArray9);
            this.numDollar.Name = "numDollar";
            this.numDollar.Size = new Size(0x43, 20);
            this.numDollar.TabIndex = 10;
            int[] numArray10 = new int[4];
            numArray10[0] = 0x1388;
            this.numDollar.Value = new decimal(numArray10);
            this.numDollar.Enter += new EventHandler(this.numDollar_Enter);
            this.rbShare.AutoSize = true;
            this.rbShare.Location = new Point(6, 60);
            this.rbShare.Name = "rbShare";
            this.rbShare.Size = new Size(0x6c, 0x11);
            this.rbShare.TabIndex = 11;
            this.rbShare.TabStop = true;
            this.rbShare.Tag = "P";
            this.rbShare.Text = "Shares/Contracts";
            this.rbShare.UseVisualStyleBackColor = true;
            this.rbShare.CheckedChanged += new EventHandler(this.rbScriptOverride_CheckedChanged);
            this.numShare.ForeColor = SystemColors.GrayText;
            int[] numArray11 = new int[4];
            numArray11[0] = 100;
            this.numShare.Increment = new decimal(numArray11);
            this.numShare.Location = new Point(0x87, 60);
            int[] numArray12 = new int[4];
            numArray12[0] = 0xf4240;
            this.numShare.Maximum = new decimal(numArray12);
            int[] numArray13 = new int[4];
            numArray13[0] = 1;
            this.numShare.Minimum = new decimal(numArray13);
            this.numShare.Name = "numShare";
            this.numShare.Size = new Size(0x43, 20);
            this.numShare.TabIndex = 12;
            int[] numArray14 = new int[4];
            numArray14[0] = 100;
            this.numShare.Value = new decimal(numArray14);
            this.numShare.Enter += new EventHandler(this.numShare_Enter);
            this.rbPctEquity.AutoSize = true;
            this.rbPctEquity.Location = new Point(6, 0x4f);
            this.rbPctEquity.Name = "rbPctEquity";
            this.rbPctEquity.Size = new Size(0x6a, 0x11);
            this.rbPctEquity.TabIndex = 13;
            this.rbPctEquity.TabStop = true;
            this.rbPctEquity.Tag = "P";
            this.rbPctEquity.Text = "Percent of Equity";
            this.rbPctEquity.UseVisualStyleBackColor = true;
            this.rbPctEquity.CheckedChanged += new EventHandler(this.rbScriptOverride_CheckedChanged);
            this.numPctEquity.DecimalPlaces = 2;
            this.numPctEquity.ForeColor = SystemColors.GrayText;
            this.numPctEquity.Location = new Point(0x87, 80);
            this.numPctEquity.Name = "numPctEquity";
            this.numPctEquity.Size = new Size(0x43, 20);
            this.numPctEquity.TabIndex = 14;
            int[] numArray15 = new int[4];
            numArray15[0] = 5;
            this.numPctEquity.Value = new decimal(numArray15);
            this.numPctEquity.Enter += new EventHandler(this.numPctEquity_Enter);
            this.rbMaxRisk.AutoSize = true;
            this.rbMaxRisk.Location = new Point(6, 100);
            this.rbMaxRisk.Name = "rbMaxRisk";
            this.rbMaxRisk.Size = new Size(0x6d, 0x11);
            this.rbMaxRisk.TabIndex = 15;
            this.rbMaxRisk.TabStop = true;
            this.rbMaxRisk.Tag = "P";
            this.rbMaxRisk.Text = "Max Percent Risk";
            this.rbMaxRisk.UseVisualStyleBackColor = true;
            this.rbMaxRisk.CheckedChanged += new EventHandler(this.rbScriptOverride_CheckedChanged);
            this.numMaxRisk.DecimalPlaces = 2;
            this.numMaxRisk.ForeColor = SystemColors.GrayText;
            this.numMaxRisk.Location = new Point(0x87, 100);
            this.numMaxRisk.Name = "numMaxRisk";
            this.numMaxRisk.Size = new Size(0x43, 20);
            this.numMaxRisk.TabIndex = 0x10;
            int[] numArray16 = new int[4];
            numArray16[0] = 5;
            this.numMaxRisk.Value = new decimal(numArray16);
            this.numMaxRisk.Enter += new EventHandler(this.numMaxRisk_Enter);
            this.rbPosSizer.AutoSize = true;
            this.rbPosSizer.Location = new Point(6, 0x92);
            this.rbPosSizer.Name = "rbPosSizer";
            this.rbPosSizer.Size = new Size(0x45, 0x11);
            this.rbPosSizer.TabIndex = 0x11;
            this.rbPosSizer.TabStop = true;
            this.rbPosSizer.Tag = "P";
            this.rbPosSizer.Text = "PosSizer:";
            this.rbPosSizer.UseVisualStyleBackColor = true;
            this.rbPosSizer.CheckedChanged += new EventHandler(this.rbScriptOverride_CheckedChanged);
            this.cmbPosSizers.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPosSizers.DropDownWidth = 0xc4;
            this.cmbPosSizers.ForeColor = SystemColors.GrayText;
            this.cmbPosSizers.FormattingEnabled = true;
            this.cmbPosSizers.Location = new Point(0x51, 0x91);
            this.cmbPosSizers.Name = "cmbPosSizers";
            this.cmbPosSizers.Size = new Size(0x75, 0x15);
            this.cmbPosSizers.TabIndex = 0x12;
            this.cmbPosSizers.SelectedIndexChanged += new EventHandler(this.rbScriptOverride_CheckedChanged);
            this.cmbPosSizers.Enter += new EventHandler(this.cmbPosSizers_Enter);
            this.btnOK.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnOK.Location = new Point(50, 0x131);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x83, 0x131);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 0x13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.lblStartingCapital.AutoSize = true;
            this.lblStartingCapital.Location = new Point(6, 0x11);
            this.lblStartingCapital.Name = "lblStartingCapital";
            this.lblStartingCapital.Size = new Size(0x4e, 13);
            this.lblStartingCapital.TabIndex = 7;
            this.lblStartingCapital.Text = "Starting Capital";
            this.numCapital.ForeColor = SystemColors.GrayText;
            int[] numArray17 = new int[4];
            numArray17[0] = 0x2710;
            this.numCapital.Increment = new decimal(numArray17);
            this.numCapital.Location = new Point(0x7f, 14);
            int[] numArray18 = new int[4];
            numArray18[0] = 0x540be400;
            numArray18[1] = 2;
            this.numCapital.Maximum = new decimal(numArray18);
            int[] numArray19 = new int[4];
            numArray19[0] = 100;
            this.numCapital.Minimum = new decimal(numArray19);
            this.numCapital.Name = "numCapital";
            this.numCapital.Size = new Size(0x4b, 20);
            this.numCapital.TabIndex = 8;
            int[] numArray20 = new int[4];
            numArray20[0] = 0x186a0;
            this.numCapital.Value = new decimal(numArray20);
            this.lblMarginFactor.AutoSize = true;
            this.lblMarginFactor.Location = new Point(7, 0x119);
            this.lblMarginFactor.Name = "lblMarginFactor";
            this.lblMarginFactor.Size = new Size(0x4b, 13);
            this.lblMarginFactor.TabIndex = 0x15;
            this.lblMarginFactor.Text = "Margin Factor:";
            this.numMarginFactor.InputType = NumEdit.NumEditType.Double;
            this.numMarginFactor.Location = new Point(0x55, 0x116);
            this.numMarginFactor.Name = "numMarginFactor";
            this.numMarginFactor.Size = new Size(0x1d, 20);
            this.numMarginFactor.TabIndex = 0x16;
            this.numMarginFactor.Text = "1";
            this.numMarginFactor.TextChanged += new EventHandler(this.numMarginFactor_TextChanged);
            this.numMarginFactor.KeyPress += new KeyPressEventHandler(this.numMarginFactor_KeyPress);
            this.lblMarginTo1.AutoSize = true;
            this.lblMarginTo1.Location = new Point(120, 0x119);
            this.lblMarginTo1.Name = "lblMarginTo1";
            this.lblMarginTo1.Size = new Size(0x19, 13);
            this.lblMarginTo1.TabIndex = 0x17;
            this.lblMarginTo1.Text = "to 1";
            this.rbScriptOverride.AutoSize = true;
            this.rbScriptOverride.Location = new Point(6, 0x7b);
            this.rbScriptOverride.Name = "rbScriptOverride";
            this.rbScriptOverride.Size = new Size(0xca, 0x11);
            this.rbScriptOverride.TabIndex = 0x18;
            this.rbScriptOverride.TabStop = true;
            this.rbScriptOverride.Tag = "P";
            this.rbScriptOverride.Text = "WealthScript Override (SetShareSize)";
            this.rbScriptOverride.UseVisualStyleBackColor = true;
            this.rbScriptOverride.CheckedChanged += new EventHandler(this.rbScriptOverride_CheckedChanged);
            this.panel2.BorderStyle = BorderStyle.FixedSingle;
            this.panel2.Location = new Point(12, 0x110);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0xc2, 1);
            this.panel2.TabIndex = 0x19;
            this.btnConfigure.Location = new Point(0x51, 0xa9);
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Size = new Size(0x75, 0x17);
            this.btnConfigure.TabIndex = 0x1a;
            this.btnConfigure.Text = "Configure ...";
            this.btnConfigure.UseVisualStyleBackColor = true;
            this.btnConfigure.Visible = false;
            this.btnConfigure.Click += new EventHandler(this.btnConfigure_Click);
            this.pnlPortSim.Controls.Add(this.lblPortSim);
            this.pnlPortSim.Controls.Add(this.btnConfigure);
            this.pnlPortSim.Controls.Add(this.rbDollar);
            this.pnlPortSim.Controls.Add(this.numDollar);
            this.pnlPortSim.Controls.Add(this.rbScriptOverride);
            this.pnlPortSim.Controls.Add(this.rbShare);
            this.pnlPortSim.Controls.Add(this.numShare);
            this.pnlPortSim.Controls.Add(this.rbPctEquity);
            this.pnlPortSim.Controls.Add(this.numPctEquity);
            this.pnlPortSim.Controls.Add(this.numCapital);
            this.pnlPortSim.Controls.Add(this.rbMaxRisk);
            this.pnlPortSim.Controls.Add(this.lblStartingCapital);
            this.pnlPortSim.Controls.Add(this.numMaxRisk);
            this.pnlPortSim.Controls.Add(this.rbPosSizer);
            this.pnlPortSim.Controls.Add(this.cmbPosSizers);
            this.pnlPortSim.Location = new Point(3, 70);
            this.pnlPortSim.Name = "pnlPortSim";
            this.pnlPortSim.Size = new Size(0xce, 0xc4);
            this.pnlPortSim.TabIndex = 0x1b;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.Linen;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0xd9, 0x152);
            base.ControlBox = false;
            base.Controls.Add(this.pnlPortSim);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.lblMarginTo1);
            base.Controls.Add(this.numMarginFactor);
            base.Controls.Add(this.lblMarginFactor);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.numRawShare);
            base.Controls.Add(this.rbFixedShare);
            base.Controls.Add(this.numRawDollar);
            base.Controls.Add(this.rbFixedDollar);
            base.Controls.Add(this.lblRawProfit);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "PositionSizeSelecterForm";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "PositionSizeSelecterForm";
            base.Paint += new PaintEventHandler(this.PositionSizeSelecterForm_Paint);
            this.numRawDollar.EndInit();
            this.numRawShare.EndInit();
            this.numDollar.EndInit();
            this.numShare.EndInit();
            this.numPctEquity.EndInit();
            this.numMaxRisk.EndInit();
            this.numCapital.EndInit();
            this.pnlPortSim.ResumeLayout(false);
            this.pnlPortSim.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0()
        {
            if (this.RawProfitMode)
            {
                this.BackColor = Color.Linen;
            }
            else
            {
                this.BackColor = Color.Honeydew;
            }
        }

        private void method_1()
        {
            try
            {
                this.maxEquityMarginFactor = decimal.Parse(ConfigurationManager.AppSettings["MaxEquityMarginFactor"]);
            }
            catch (Exception)
            {
                this.maxEquityMarginFactor = 10M;
            }
        }

        private void numDollar_Enter(object sender, EventArgs e)
        {
            this.rbDollar.Checked = true;
        }

        private void numMarginFactor_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (e.KeyChar == '\b')
            {
                e.Handled = false;
                if (this.numMarginFactor.Text.Length > 1)
                {
                    this.EquityMarginFactor = decimal.Parse(this.numMarginFactor.Text.Substring(0, this.numMarginFactor.Text.Length - 1));
                }
            }
            else if ((this.numMarginFactor.SelectionLength == this.numMarginFactor.Text.Length) && char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
                this.EquityMarginFactor = decimal.Parse(e.KeyChar.ToString());
            }
            else if (char.IsDigit(e.KeyChar) && (decimal.Parse(this.numMarginFactor.Text + e.KeyChar) <= this.MaxEquityMarginFactor))
            {
                e.Handled = false;
                this.EquityMarginFactor = decimal.Parse(this.numMarginFactor.Text + e.KeyChar);
            }
            else if (CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator.Equals(e.KeyChar.ToString()))
            {
                e.Handled = false;
                this.EquityMarginFactor = decimal.Parse(this.numMarginFactor.Text);
            }
            base.OnKeyPress(e);
        }

        private void numMarginFactor_TextChanged(object sender, EventArgs e)
        {
            this.numPctEquity.Maximum = 100M * this.EquityMarginFactor;
            if (this.numPctEquity.Value > this.numPctEquity.Maximum)
            {
                this.numPctEquity.Value = this.numPctEquity.Maximum;
            }
        }

        private void numMaxRisk_Enter(object sender, EventArgs e)
        {
            this.rbMaxRisk.Checked = true;
        }

        private void numPctEquity_Enter(object sender, EventArgs e)
        {
            this.rbPctEquity.Checked = true;
        }

        private void numRawDollar_Enter(object sender, EventArgs e)
        {
            this.rbFixedDollar.Checked = true;
        }

        private void numRawShare_Enter(object sender, EventArgs e)
        {
            this.rbFixedShare.Checked = true;
        }

        private void numShare_Enter(object sender, EventArgs e)
        {
            this.rbShare.Checked = true;
        }

        private void PositionSizeSelecterForm_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.SaddleBrown, 3f);
            if (!this.RawProfitMode)
            {
                pen.Color = Color.DarkGreen;
            }
            using (pen)
            {
                e.Graphics.DrawRectangle(pen, 0, 0, base.Width, base.Height);
            }
        }

        private void rbScriptOverride_CheckedChanged(object sender, EventArgs e)
        {
            this.method_0();
            if (sender is RadioButton)
            {
                RadioButton button = sender as RadioButton;
                if (button.Checked)
                {
                    if ((button.Tag as string) == "R")
                    {
                        this.rbDollar.Checked = false;
                        this.rbPctEquity.Checked = false;
                        this.rbMaxRisk.Checked = false;
                        this.rbShare.Checked = false;
                        this.rbScriptOverride.Checked = false;
                        this.rbPosSizer.Checked = false;
                    }
                    else
                    {
                        this.rbFixedDollar.Checked = false;
                        this.rbFixedShare.Checked = false;
                    }
                }
            }
            Color color = Color.FromKnownColor(KnownColor.ControlText);
            Color color2 = Color.FromKnownColor(KnownColor.GrayText);
            this.numRawDollar.ForeColor = this.rbFixedDollar.Checked ? color : color2;
            this.numRawShare.ForeColor = this.rbFixedShare.Checked ? color : color2;
            this.numCapital.ForeColor = (this.rbFixedShare.Checked || this.rbFixedDollar.Checked) ? color2 : color;
            this.numDollar.ForeColor = this.rbDollar.Checked ? color : color2;
            this.numShare.ForeColor = this.rbShare.Checked ? color : color2;
            this.numPctEquity.ForeColor = this.rbPctEquity.Checked ? color : color2;
            this.numMaxRisk.ForeColor = this.rbMaxRisk.Checked ? color : color2;
            this.cmbPosSizers.ForeColor = this.rbPosSizer.Checked ? color : color2;
            this.btnConfigure.Visible = (this.rbPosSizer.Checked && (this.cmbPosSizers.SelectedIndex >= 0)) && (this.SelectedPosSizer is ICustomSettings);
        }

        public bool CombinationStrategyChildMode
        {
            get
            {
                return this.combinationStrategyChildMode;
            }
            set
            {
                this.combinationStrategyChildMode = value;
                if (value)
                {
                    this.pnlPortSim.Top = 3;
                    base.Height = 240;
                    this.numCapital.Enabled = false;
                    int[] bits = new int[4];
                    bits[0] = 1;
                    this.numCapital.Minimum = new decimal(bits);
                }
            }
        }

        public decimal EquityMarginFactor
        {
            get
            {
                return this.equityMarginFactor;
            }
            set
            {
                this.equityMarginFactor = value;
            }
        }

        public decimal MaxEquityMarginFactor
        {
            get
            {
                return this.maxEquityMarginFactor;
            }
        }

        public WealthLab.PositionSize PositionSize
        {
            get
            {
                WealthLab.PositionSize size = new WealthLab.PositionSize();
                if (this.rbFixedDollar.Checked)
                {
                    size.Mode = PosSizeMode.RawProfitDollar;
                }
                else if (this.rbFixedShare.Checked)
                {
                    size.Mode = PosSizeMode.RawProfitShare;
                }
                else if (this.rbDollar.Checked)
                {
                    size.Mode = PosSizeMode.Dollar;
                }
                else if (this.rbShare.Checked)
                {
                    size.Mode = PosSizeMode.Share;
                }
                else if (this.rbPctEquity.Checked)
                {
                    size.Mode = PosSizeMode.PctEquity;
                }
                else if (this.rbMaxRisk.Checked)
                {
                    size.Mode = PosSizeMode.MaxRisk;
                }
                else if (this.rbPosSizer.Checked)
                {
                    size.Mode = PosSizeMode.SimuScript;
                }
                else if (this.rbScriptOverride.Checked)
                {
                    size.Mode = PosSizeMode.ScriptOverride;
                }
                size.RawProfitDollarSize = (double) this.numRawDollar.Value;
                size.RawProfitShareSize = (double) this.numRawShare.Value;
                size.DollarSize = (double) this.numDollar.Value;
                size.ShareSize = (double) this.numShare.Value;
                size.PctSize = (double) this.numPctEquity.Value;
                size.RiskSize = (double) this.numMaxRisk.Value;
                size.SimuScriptName = this.cmbPosSizers.Text;
                size.StartingCapital = (double) this.numCapital.Value;
                size.MarginFactor = (double) this.numMarginFactor.Value;
                size.PosSizerConfig = this.string_0;
                return size;
            }
            set
            {
                PosSizeMode mode = value.Mode;
                switch (mode)
                {
                    case PosSizeMode.RawProfitDollar:
                        {
                            this.rbFixedDollar.Checked = true;
                            break;
                        }
                    case PosSizeMode.RawProfitShare:
                        {
                            this.rbFixedShare.Checked = true;
                            break;
                        }
                    case PosSizeMode.Dollar:
                        {
                            this.rbDollar.Checked = true;
                            break;
                        }
                    case PosSizeMode.Share:
                        {
                            this.rbShare.Checked = true;
                            break;
                        }
                    case PosSizeMode.PctEquity:
                        {
                            this.rbPctEquity.Checked = true;
                            break;
                        }
                    case PosSizeMode.MaxRisk:
                        {
                            this.rbMaxRisk.Checked = true;
                            break;
                        }
                    case PosSizeMode.SimuScript:
                        {
                            this.rbPosSizer.Checked = true;
                            break;
                        }
                    case PosSizeMode.ScriptOverride:
                        {
                            this.rbScriptOverride.Checked = true;
                            break;
                        }
                }
                this.numRawDollar.Value = (decimal)((double)value.RawProfitDollarSize);
                this.numRawShare.Value = (decimal)((double)value.RawProfitShareSize);
                this.numDollar.Value = (decimal)((double)value.DollarSize);
                this.numShare.Value = (decimal)((double)value.ShareSize);
                this.EquityMarginFactor = Convert.ToDecimal(value.MarginFactor);
                this.numPctEquity.Maximum = new decimal(100) * this.EquityMarginFactor;
                if ((decimal)((double)value.PctSize) <= this.numPctEquity.Maximum)
                {
                    this.numPctEquity.Value = (decimal)((double)value.PctSize);
                }
                else
                {
                    this.numPctEquity.Value = new decimal(100);
                }
                this.numMaxRisk.Value = (decimal)((double)value.RiskSize);
                PosSizer posSizer = null;
                IEnumerator enumerator = this.cmbPosSizers.Items.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            PosSizer current = (PosSizer)enumerator.Current;
                            if (current.FriendlyName == value.SimuScriptName)
                            {
                                posSizer = current;
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
                if (posSizer != null)
                {
                    this.cmbPosSizers.SelectedIndex = this.cmbPosSizers.Items.IndexOf(posSizer);
                    this.string_0 = value.PosSizerConfig;
                    if (this.string_0 != "")
                    {
                        try
                        {
                            posSizer.ApplyConfigString(PosSizer.ParseConfigString(this.string_0));
                        }
                        catch
                        {
                        }
                    }
                }
                this.numCapital.Value = (decimal)((double)value.StartingCapital);
                double marginFactor = value.MarginFactor;
                this.numMarginFactor.Text = marginFactor.ToString();
                this.method_0();
            }
        }

        public bool RawProfitMode
        {
            get
            {
                if (!this.rbFixedDollar.Checked)
                {
                    return this.rbFixedShare.Checked;
                }
                return true;
            }
        }

        private PosSizer SelectedPosSizer
        {
            get
            {
                return (this.cmbPosSizers.SelectedItem as PosSizer);
            }
        }

        private ISettingsHost SettingsHost
        {
            get
            {
                return MainModule.Instance.Settings;
            }
        }
    }
}

