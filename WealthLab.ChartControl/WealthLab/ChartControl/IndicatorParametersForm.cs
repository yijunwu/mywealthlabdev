namespace WealthLab.ChartControl
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using WealthLab;

    public class IndicatorParametersForm : Form
    {
        private bool bool_0;
        [CompilerGenerated]
        private bool useIndicatorPane;
        private Button btnCancel;
        private Button btnOK;
        private CheckBox cbDrawOscillator;
        private CheckBox cbFillBands;
        private CheckBox cbPlotBands;
        private CheckBox cbUseIndicatorPane;
        private ChartPane chartPane_0;
        private ComboBox cmbStyle;
        private float float_0 = 1f;
        private float float_1 = 1f;
        private GroupBox grpBand;
        private GroupBox grpDrawing;
        private GroupBox grpOscillator;
        private GroupBox grpParameters;
        private IContainer icontainer_0;
        private IndicatorHelper indicatorHelper;
        private int int_0 = 450;
        private Label label1;
        private Label lblColor;
        private Label lblFillColor;
        private Label lblOscTransparency;
        private Label lblOverboughtColor;
        private Label lblOverboughtLevel;
        private Label lblOversoldLevel;
        private Label lblStyle;
        private Label lblTransparency;
        private Label lblWidth;
        private NumericUpDown numOscTransparency;
        private NumericUpDown numOverboughtLevel;
        private NumericUpDown numOversoldLevel;
        private NumericUpDown numTransparency;
        private NumericUpDown numWidth;
        private ColorPickerPanel pnlBandColor;
        private ColorPickerPanel pnlColor;
        private ColorPickerPanel pnlOverboughtColor;
        private ColorPickerPanel pnlOversoldColor;
        private string string_0;

        public IndicatorParametersForm()
        {
            this.InitializeComponent();
            if (base.AutoScaleMode == AutoScaleMode.Dpi)
            {
                this.float_1 = base.AutoScaleDimensions.Height / 96f;
                this.float_0 = base.AutoScaleDimensions.Width / 96f;
                this.int_0 = (int) (this.int_0 * this.float_0);
            }
        }

        private void cbDrawOscillator_CheckedChanged(object sender, EventArgs e)
        {
            bool flag = this.cbDrawOscillator.Checked;
            this.numOverboughtLevel.Enabled = flag;
            this.pnlOverboughtColor.Enabled = flag;
            this.numOversoldLevel.Enabled = flag;
            this.pnlOversoldColor.Enabled = flag;
            this.numOscTransparency.Enabled = flag;
        }

        private void cbFillBands_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlBandColor.Enabled = this.cbFillBands.Checked;
            this.numTransparency.Enabled = this.cbFillBands.Checked;
        }

        private void cbPlotBands_CheckedChanged(object sender, EventArgs e)
        {
            this.cbFillBands.Enabled = this.cbPlotBands.Checked;
            this.pnlBandColor.Enabled = this.cbPlotBands.Checked && this.cbFillBands.Checked;
            this.numTransparency.Enabled = this.cbPlotBands.Checked && this.cbFillBands.Checked;
        }

        private void cbUseIndicatorPane_CheckedChanged(object sender, EventArgs e)
        {
            this.UseIndicatorPane = this.cbUseIndicatorPane.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public IndicatorDescriptor GetFundamentalDescriptor()
        {
            return new IndicatorDescriptor(0) { Color = this.pnlColor.BackColor, Style = (LineStyle) Enum.Parse(typeof(LineStyle), this.cmbStyle.Text), Width = (int) this.numWidth.Value, FundamentalItemName = this.string_0 };
        }

        public IndicatorDescriptor GetIndicatorDescriptor()
        {
            IndicatorDescriptor descriptor = new IndicatorDescriptor(this.indicatorHelper.ParameterDefaultValues.Count) {
                Color = this.pnlColor.BackColor,
                Style = (LineStyle) Enum.Parse(typeof(LineStyle), this.cmbStyle.Text),
                Width = (int) this.numWidth.Value,
                IndicatorType = this.indicatorHelper.IndicatorType,
                PlotBandPairIndicator = (this.indicatorHelper.PartnerBandIndicatorType != null) && this.cbPlotBands.Checked,
                FillBand = this.cbFillBands.Checked,
                BandFillColor = this.method_0(this.pnlBandColor.BackColor, this.numTransparency),
                BandPairIndicatorType = this.indicatorHelper.PartnerBandIndicatorType,
                PlotOscillator = this.PlotAsOscillator,
                OverboughtLevel = this.OscillatorOverboughtLevel,
                OverboughtColor = this.method_0(this.OscillatorOverboughtColor, this.numOscTransparency),
                OversoldLevel = this.OscillatorOversoldLevel,
                OversoldColor = this.method_0(this.OscillatorOversoldColor, this.numOscTransparency),
                PaneDescription = this.chartPane_0.Description
            };
            for (int i = 0; i < descriptor.Parameters.Length; i++)
            {
                Control control = this.grpParameters.Controls[(i * 2) + 1];
                System.Type type = this.indicatorHelper.ParameterDefaultValues[i].GetType();
                if (type == typeof(CoreDataSeries))
                {
                    ComboBox box3 = control as ComboBox;
                    string text = box3.Text;
                    if (Enum.IsDefined(typeof(CoreDataSeries), text) && (text != "Volume"))
                    {
                        CoreDataSeries selectedItem = (CoreDataSeries) box3.SelectedItem;
                        descriptor.Parameters[i] = selectedItem;
                    }
                    else
                    {
                        IndicatorDescriptionString str3 = new IndicatorDescriptionString(box3.Text);
                        descriptor.Parameters[i] = str3;
                    }
                }
                else if (type == typeof(BarDataType))
                {
                    ComboBox box = control as ComboBox;
                    BarsDescriptorString str = new BarsDescriptorString(box.Text);
                    descriptor.Parameters[i] = str;
                }
                else if (this.indicatorHelper.ParameterDefaultValues[i] is Enum)
                {
                    ComboBox box4 = control as ComboBox;
                    descriptor.Parameters[i] = Enum.Parse(this.indicatorHelper.ParameterDefaultValues[i].GetType(), box4.Text);
                }
                else if ((type != typeof(int)) && (type != typeof(RangeBoundInt32)))
                {
                    if ((type != typeof(double)) && (type != typeof(RangeBoundDouble)))
                    {
                        if (type == typeof(string))
                        {
                            TextBox box5 = control as TextBox;
                            descriptor.Parameters[i] = box5.Text;
                        }
                        else if (type == typeof(bool))
                        {
                            CheckBox box2 = control as CheckBox;
                            descriptor.Parameters[i] = box2.Checked;
                        }
                        else if (type == typeof(DateTime))
                        {
                            DateTimePicker picker = control as DateTimePicker;
                            descriptor.Parameters[i] = picker.Value;
                        }
                    }
                    else
                    {
                        NumericUpDown down = control as NumericUpDown;
                        double num2 = (double) down.Value;
                        descriptor.Parameters[i] = num2;
                    }
                }
                else
                {
                    NumericUpDown down2 = control as NumericUpDown;
                    int num3 = (int) down2.Value;
                    descriptor.Parameters[i] = num3;
                }
            }
            return descriptor;
        }

        public string GetIndicatorParameterValue(int index)
        {
            if (this.grpParameters.Controls.Count <= (index * 2))
            {
                return "";
            }
            Control control = this.grpParameters.Controls[(index * 2) + 1];
            if (control is CheckBox)
            {
                CheckBox box2 = control as CheckBox;
                if (box2.Checked)
                {
                    return "true";
                }
                return "false";
            }
            if (control is NumericUpDown)
            {
                NumericUpDown down = control as NumericUpDown;
                return down.Value.ToString();
            }
            if (control is DateTimePicker)
            {
                DateTimePicker picker = control as DateTimePicker;
                if (picker.Format == DateTimePickerFormat.Time)
                {
                    return picker.Value.ToShortTimeString();
                }
                return picker.Value.ToShortDateString();
            }
            if (!(control is ComboBox))
            {
                return control.Text;
            }
            ComboBox box = control as ComboBox;
            if (((box.Items.Count > 0) && (box.Items[0] is CoreDataSeries)) && (box.SelectedIndex > 3))
            {
                return "";
            }
            return box.Text;
        }

        public void Initialize(string fundamentalItemName)
        {
            this.string_0 = fundamentalItemName;
            this.Text = fundamentalItemName + " Properties";
            this.grpParameters.Visible = false;
            base.Height = (int) (185f * this.float_1);
            this.cmbStyle.Items.Clear();
            this.cmbStyle.Items.Add("Solid");
            this.cmbStyle.Items.Add("Dotted");
            this.cmbStyle.Items.Add("Dashed");
            this.cmbStyle.SelectedIndex = 0;
        }

        public bool Initialize(IndicatorHelper indHelper, Chart chart, ChartPane pane)
        {
            this.indicatorHelper = indHelper;
            this.chartPane_0 = pane;
            System.Type indicatorType = indHelper.IndicatorType;
            this.Text = indicatorType.Name + " Properties";
            this.cmbStyle.Items.Clear();
            foreach (LineStyle style in Enum.GetValues(typeof(LineStyle)))
            {
                this.cmbStyle.Items.Add(style.ToString());
            }
            this.pnlColor.BackColor = indHelper.DefaultColor;
            this.cmbStyle.Text = indHelper.DefaultStyle.ToString();
            this.numWidth.Value = indHelper.DefaultWidth;
            if (indHelper.TargetPane.StartsWith("?"))
            {
                this.bool_0 = true;
                this.cbUseIndicatorPane.Visible = true;
                this.cbUseIndicatorPane.Text = this.cbUseIndicatorPane.Text.Replace("<indi>", indHelper.TargetPane.Substring(1));
            }
            if (indHelper.PartnerBandIndicatorType != null)
            {
                base.Width = this.int_0;
                this.grpBand.Visible = true;
                this.pnlBandColor.BackColor = indHelper.DefaultBandColor;
            }
            else if (indHelper.IsOscillator)
            {
                base.Width = this.int_0;
                this.grpOscillator.Visible = true;
                this.numOverboughtLevel.Value = (decimal) indHelper.OscillatorOverboughtValue;
                this.pnlOverboughtColor.BackColor = indHelper.OscillatorOverboughtColor;
                this.numOversoldLevel.Value = (decimal) indHelper.OscillatorOversoldValue;
                this.pnlOversoldColor.BackColor = indHelper.OscillatorOversoldColor;
            }
            IList<object> parameterDefaultValues = indHelper.ParameterDefaultValues;
            if (parameterDefaultValues == null)
            {
                this.grpParameters.Visible = false;
                base.Height = (int) (190f * this.float_1);
            }
            else
            {
                this.grpParameters.Visible = true;
                this.grpParameters.Height = (int) ((0x36 + ((parameterDefaultValues.Count - 1) * 0x18)) * this.float_1);
                base.Height = (int) ((250 + ((parameterDefaultValues.Count - 1) * 0x18)) * this.float_1);
                this.method_2();
                if (!IndicatorDragDropManager.CreateIndicatorParameterUI(indHelper, this.grpParameters, pane, chart.Bars.Symbol, false, this.float_0))
                {
                    return false;
                }
            }
            base.Height += (int) (this._cbUseIndicatorPaneReSize * this.float_1);
            return true;
        }

        public void Initialize(PlottedIndicator plottedIndicator_0, IndicatorDescriptor indDesc, IndicatorHelper helper, Chart chart, ChartPane pane)
        {
            this.Initialize(helper, chart, pane);
            this.pnlColor.BackColor = plottedIndicator_0.Color;
            this.cmbStyle.Text = plottedIndicator_0.Style.ToString();
            this.numWidth.Value = plottedIndicator_0.Width;
            this.cbPlotBands.Checked = indDesc.PlotBandPairIndicator;
            this.cbFillBands.Checked = indDesc.FillBand;
            this.method_1(this.pnlBandColor, this.numTransparency, indDesc.BandFillColor);
            this.cbDrawOscillator.Checked = indDesc.PlotOscillator;
            this.numOverboughtLevel.Value = (decimal) indDesc.OverboughtLevel;
            this.method_1(this.pnlOverboughtColor, this.numOscTransparency, indDesc.OverboughtColor);
            this.numOversoldLevel.Value = (decimal) indDesc.OversoldLevel;
            this.method_1(this.pnlOversoldColor, this.numOscTransparency, indDesc.OversoldColor);
            for (int i = 0; i < indDesc.Parameters.Length; i++)
            {
                object obj2 = indDesc.Parameters[i];
                Control control = this.grpParameters.Controls[(i * 2) + 1];
                if (!(control is TextBox) && !(control is ComboBox))
                {
                    if (control is CheckBox)
                    {
                        CheckBox box = control as CheckBox;
                        box.Checked = (bool) obj2;
                    }
                    else if (control is DateTimePicker)
                    {
                        DateTimePicker picker = control as DateTimePicker;
                        picker.Value = (DateTime) obj2;
                    }
                    else
                    {
                        if (!(control is NumericUpDown))
                        {
                            throw new InvalidOperationException("Unknown parameter type in Initialize: " + obj2.GetType().Name);
                        }
                        NumericUpDown down = control as NumericUpDown;
                        if (obj2 is int)
                        {
                            down.Value = (int) obj2;
                        }
                        else
                        {
                            down.Value = (decimal) ((double) obj2);
                        }
                    }
                }
                else
                {
                    if ((obj2 is CoreDataSeries) || (obj2 is IndicatorDescriptionString))
                    {
                        obj2.ToString();
                        ComboBox box2 = control as ComboBox;
                        string description = plottedIndicator_0.Series.Description;
                        if (box2.Items.Contains(description))
                        {
                            box2.Items.Remove(description);
                        }
                        if (indDesc.PlottedPartner != null)
                        {
                            description = indDesc.PlottedPartner.Series.Description;
                            if (box2.Items.Contains(description))
                            {
                                box2.Items.Remove(description);
                            }
                        }
                        if (obj2 is CoreDataSeries)
                        {
                            box2.Items.Clear();
                            box2.Items.Add(CoreDataSeries.Close);
                            box2.Items.Add(CoreDataSeries.High);
                            box2.Items.Add(CoreDataSeries.Low);
                            box2.Items.Add(CoreDataSeries.Open);
                        }
                    }
                    control.Text = obj2.ToString();
                }
            }
        }

        private void InitializeComponent()
        {
            this.grpDrawing = new GroupBox();
            this.cbUseIndicatorPane = new CheckBox();
            this.pnlColor = new ColorPickerPanel();
            this.numWidth = new NumericUpDown();
            this.lblWidth = new Label();
            this.cmbStyle = new ComboBox();
            this.lblStyle = new Label();
            this.lblColor = new Label();
            this.grpParameters = new GroupBox();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.grpBand = new GroupBox();
            this.numTransparency = new NumericUpDown();
            this.lblTransparency = new Label();
            this.pnlBandColor = new ColorPickerPanel();
            this.lblFillColor = new Label();
            this.cbFillBands = new CheckBox();
            this.cbPlotBands = new CheckBox();
            this.grpOscillator = new GroupBox();
            this.numOscTransparency = new NumericUpDown();
            this.lblOscTransparency = new Label();
            this.pnlOversoldColor = new ColorPickerPanel();
            this.label1 = new Label();
            this.numOversoldLevel = new NumericUpDown();
            this.lblOversoldLevel = new Label();
            this.pnlOverboughtColor = new ColorPickerPanel();
            this.lblOverboughtColor = new Label();
            this.numOverboughtLevel = new NumericUpDown();
            this.lblOverboughtLevel = new Label();
            this.cbDrawOscillator = new CheckBox();
            this.grpDrawing.SuspendLayout();
            this.numWidth.BeginInit();
            this.grpBand.SuspendLayout();
            this.numTransparency.BeginInit();
            this.grpOscillator.SuspendLayout();
            this.numOscTransparency.BeginInit();
            this.numOversoldLevel.BeginInit();
            this.numOverboughtLevel.BeginInit();
            base.SuspendLayout();
            this.grpDrawing.Controls.Add(this.pnlColor);
            this.grpDrawing.Controls.Add(this.numWidth);
            this.grpDrawing.Controls.Add(this.lblWidth);
            this.grpDrawing.Controls.Add(this.cmbStyle);
            this.grpDrawing.Controls.Add(this.lblStyle);
            this.grpDrawing.Controls.Add(this.lblColor);
            this.grpDrawing.Location = new Point(14, 13);
            this.grpDrawing.Name = "grpDrawing";
            this.grpDrawing.Size = new Size(0x106, 0x66);
            this.grpDrawing.TabIndex = 0;
            this.grpDrawing.TabStop = false;
            this.grpDrawing.Text = "Drawing Style";
            this.cbUseIndicatorPane.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbUseIndicatorPane.AutoSize = true;
            this.cbUseIndicatorPane.Location = new Point(14, 0x5f);
            this.cbUseIndicatorPane.Margin = new Padding(2, 2, 2, 2);
            this.cbUseIndicatorPane.Name = "cbUseIndicatorPane";
            this.cbUseIndicatorPane.Size = new Size(0x67, 0x11);
            this.cbUseIndicatorPane.TabIndex = 7;
            this.cbUseIndicatorPane.Text = "Use <indi> pane";
            this.cbUseIndicatorPane.UseVisualStyleBackColor = true;
            this.cbUseIndicatorPane.Visible = false;
            this.cbUseIndicatorPane.CheckedChanged += new EventHandler(this.cbUseIndicatorPane_CheckedChanged);
            this.pnlColor.BackColor = Color.Red;
            this.pnlColor.Cursor = Cursors.Hand;
            this.pnlColor.DrawOutline = false;
            this.pnlColor.Location = new Point(0x5e, 0x11);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.OutlineColor = Color.Black;
            this.pnlColor.Size = new Size(0xa1, 0x16);
            this.pnlColor.TabIndex = 6;
            this.pnlColor.Text = "colorPickerPanel1";
            this.numWidth.Location = new Point(0x5e, 0x49);
            int[] bits = new int[4];
            bits[0] = 1;
            this.numWidth.Minimum = new decimal(bits);
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new Size(0x35, 20);
            this.numWidth.TabIndex = 5;
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.numWidth.Value = new decimal(numArray2);
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new Point(7, 0x49);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new Size(0x3d, 13);
            this.lblWidth.TabIndex = 4;
            this.lblWidth.Text = "Line Width:";
            this.cmbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStyle.FormattingEnabled = true;
            this.cmbStyle.Location = new Point(0x5e, 0x2e);
            this.cmbStyle.Name = "cmbStyle";
            this.cmbStyle.Size = new Size(0xa1, 0x15);
            this.cmbStyle.TabIndex = 3;
            this.lblStyle.AutoSize = true;
            this.lblStyle.Location = new Point(6, 0x2e);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new Size(0x4b, 13);
            this.lblStyle.TabIndex = 2;
            this.lblStyle.Text = "Drawing Style:";
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new Point(7, 20);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new Size(0x22, 13);
            this.lblColor.TabIndex = 0;
            this.lblColor.Text = "Color:";
            this.grpParameters.Location = new Point(14, 0x75);
            this.grpParameters.Name = "grpParameters";
            this.grpParameters.Size = new Size(0x106, 0x4a);
            this.grpParameters.TabIndex = 1;
            this.grpParameters.TabStop = false;
            this.grpParameters.Text = "Parameters";
            this.grpParameters.Visible = false;
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xc6, 0x75);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 0x16);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x6f, 0x75);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(80, 0x16);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.grpBand.Controls.Add(this.numTransparency);
            this.grpBand.Controls.Add(this.lblTransparency);
            this.grpBand.Controls.Add(this.pnlBandColor);
            this.grpBand.Controls.Add(this.lblFillColor);
            this.grpBand.Controls.Add(this.cbFillBands);
            this.grpBand.Controls.Add(this.cbPlotBands);
            this.grpBand.Location = new Point(0x11c, 13);
            this.grpBand.Name = "grpBand";
            this.grpBand.Size = new Size(0xb7, 0xb2);
            this.grpBand.TabIndex = 6;
            this.grpBand.TabStop = false;
            this.grpBand.Text = "Band Properties";
            this.grpBand.Visible = false;
            this.numTransparency.Enabled = false;
            int[] numArray3 = new int[4];
            numArray3[0] = 5;
            this.numTransparency.Increment = new decimal(numArray3);
            this.numTransparency.Location = new Point(0x76, 0x75);
            this.numTransparency.Name = "numTransparency";
            this.numTransparency.Size = new Size(0x35, 20);
            this.numTransparency.TabIndex = 10;
            int[] numArray4 = new int[4];
            numArray4[0] = 0x4b;
            this.numTransparency.Value = new decimal(numArray4);
            this.lblTransparency.AutoSize = true;
            this.lblTransparency.Location = new Point(10, 0x76);
            this.lblTransparency.Name = "lblTransparency";
            this.lblTransparency.Size = new Size(90, 13);
            this.lblTransparency.TabIndex = 9;
            this.lblTransparency.Text = "Fill Transparency:";
            this.pnlBandColor.BackColor = Color.Red;
            this.pnlBandColor.Cursor = Cursors.Hand;
            this.pnlBandColor.DrawOutline = false;
            this.pnlBandColor.Enabled = false;
            this.pnlBandColor.Location = new Point(10, 0x59);
            this.pnlBandColor.Name = "pnlBandColor";
            this.pnlBandColor.OutlineColor = Color.Black;
            this.pnlBandColor.Size = new Size(0xa1, 0x16);
            this.pnlBandColor.TabIndex = 8;
            this.pnlBandColor.Text = "colorPickerPanel1";
            this.lblFillColor.AutoSize = true;
            this.lblFillColor.Location = new Point(6, 0x49);
            this.lblFillColor.Name = "lblFillColor";
            this.lblFillColor.Size = new Size(0x4d, 13);
            this.lblFillColor.TabIndex = 7;
            this.lblFillColor.Text = "Band Fill Color:";
            this.cbFillBands.AutoSize = true;
            this.cbFillBands.Location = new Point(7, 40);
            this.cbFillBands.Name = "cbFillBands";
            this.cbFillBands.Size = new Size(0x59, 0x11);
            this.cbFillBands.TabIndex = 1;
            this.cbFillBands.Text = "Fill the Bands";
            this.cbFillBands.UseVisualStyleBackColor = true;
            this.cbFillBands.CheckedChanged += new EventHandler(this.cbFillBands_CheckedChanged);
            this.cbPlotBands.AutoSize = true;
            this.cbPlotBands.Checked = true;
            this.cbPlotBands.CheckState = CheckState.Checked;
            this.cbPlotBands.Location = new Point(7, 20);
            this.cbPlotBands.Name = "cbPlotBands";
            this.cbPlotBands.Size = new Size(0x91, 0x11);
            this.cbPlotBands.TabIndex = 0;
            this.cbPlotBands.Text = "Plot both Indicator Bands";
            this.cbPlotBands.UseVisualStyleBackColor = true;
            this.cbPlotBands.CheckedChanged += new EventHandler(this.cbPlotBands_CheckedChanged);
            this.grpOscillator.Controls.Add(this.numOscTransparency);
            this.grpOscillator.Controls.Add(this.lblOscTransparency);
            this.grpOscillator.Controls.Add(this.pnlOversoldColor);
            this.grpOscillator.Controls.Add(this.label1);
            this.grpOscillator.Controls.Add(this.numOversoldLevel);
            this.grpOscillator.Controls.Add(this.lblOversoldLevel);
            this.grpOscillator.Controls.Add(this.pnlOverboughtColor);
            this.grpOscillator.Controls.Add(this.lblOverboughtColor);
            this.grpOscillator.Controls.Add(this.numOverboughtLevel);
            this.grpOscillator.Controls.Add(this.lblOverboughtLevel);
            this.grpOscillator.Controls.Add(this.cbDrawOscillator);
            this.grpOscillator.Location = new Point(0x11c, 13);
            this.grpOscillator.Name = "grpOscillator";
            this.grpOscillator.Size = new Size(0xb7, 0xb3);
            this.grpOscillator.TabIndex = 7;
            this.grpOscillator.TabStop = false;
            this.grpOscillator.Text = "Oscillator Properties";
            this.grpOscillator.Visible = false;
            this.numOscTransparency.Enabled = false;
            int[] numArray5 = new int[4];
            numArray5[0] = 5;
            this.numOscTransparency.Increment = new decimal(numArray5);
            this.numOscTransparency.Location = new Point(0x71, 150);
            this.numOscTransparency.Name = "numOscTransparency";
            this.numOscTransparency.Size = new Size(0x3a, 20);
            this.numOscTransparency.TabIndex = 14;
            int[] numArray6 = new int[4];
            numArray6[0] = 0x4b;
            this.numOscTransparency.Value = new decimal(numArray6);
            this.lblOscTransparency.AutoSize = true;
            this.lblOscTransparency.Location = new Point(7, 0x99);
            this.lblOscTransparency.Name = "lblOscTransparency";
            this.lblOscTransparency.Size = new Size(0x4b, 13);
            this.lblOscTransparency.TabIndex = 13;
            this.lblOscTransparency.Text = "Transparency:";
            this.pnlOversoldColor.BackColor = Color.Red;
            this.pnlOversoldColor.Cursor = Cursors.Hand;
            this.pnlOversoldColor.DrawOutline = false;
            this.pnlOversoldColor.Enabled = false;
            this.pnlOversoldColor.Location = new Point(0x72, 0x7c);
            this.pnlOversoldColor.Name = "pnlOversoldColor";
            this.pnlOversoldColor.OutlineColor = Color.Black;
            this.pnlOversoldColor.Size = new Size(0x3a, 0x16);
            this.pnlOversoldColor.TabIndex = 12;
            this.pnlOversoldColor.Text = "colorPickerPanel2";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(7, 0x7c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4f, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Oversold Color:";
            this.numOversoldLevel.Enabled = false;
            this.numOversoldLevel.Location = new Point(0x72, 0x67);
            int[] numArray7 = new int[4];
            numArray7[0] = 0x5f5e100;
            this.numOversoldLevel.Maximum = new decimal(numArray7);
            int[] numArray8 = new int[4];
            numArray8[0] = 0x5f5e100;
            numArray8[3] = -2147483648;
            this.numOversoldLevel.Minimum = new decimal(numArray8);
            this.numOversoldLevel.Name = "numOversoldLevel";
            this.numOversoldLevel.Size = new Size(0x3a, 20);
            this.numOversoldLevel.TabIndex = 10;
            this.lblOversoldLevel.AutoSize = true;
            this.lblOversoldLevel.Location = new Point(7, 0x6a);
            this.lblOversoldLevel.Name = "lblOversoldLevel";
            this.lblOversoldLevel.Size = new Size(0x51, 13);
            this.lblOversoldLevel.TabIndex = 9;
            this.lblOversoldLevel.Text = "Oversold Level:";
            this.pnlOverboughtColor.BackColor = Color.Red;
            this.pnlOverboughtColor.Cursor = Cursors.Hand;
            this.pnlOverboughtColor.DrawOutline = false;
            this.pnlOverboughtColor.Enabled = false;
            this.pnlOverboughtColor.Location = new Point(0x73, 80);
            this.pnlOverboughtColor.Name = "pnlOverboughtColor";
            this.pnlOverboughtColor.OutlineColor = Color.Black;
            this.pnlOverboughtColor.Size = new Size(0x3a, 0x16);
            this.pnlOverboughtColor.TabIndex = 8;
            this.pnlOverboughtColor.Text = "colorPickerPanel1";
            this.lblOverboughtColor.AutoSize = true;
            this.lblOverboughtColor.Location = new Point(7, 80);
            this.lblOverboughtColor.Name = "lblOverboughtColor";
            this.lblOverboughtColor.Size = new Size(0x5d, 13);
            this.lblOverboughtColor.TabIndex = 7;
            this.lblOverboughtColor.Text = "Overbought Color:";
            this.numOverboughtLevel.Enabled = false;
            this.numOverboughtLevel.Location = new Point(0x73, 0x3a);
            int[] numArray9 = new int[4];
            numArray9[0] = 0x5f5e100;
            this.numOverboughtLevel.Maximum = new decimal(numArray9);
            int[] numArray10 = new int[4];
            numArray10[0] = 0x5f5e100;
            numArray10[3] = -2147483648;
            this.numOverboughtLevel.Minimum = new decimal(numArray10);
            this.numOverboughtLevel.Name = "numOverboughtLevel";
            this.numOverboughtLevel.Size = new Size(0x3a, 20);
            this.numOverboughtLevel.TabIndex = 6;
            this.lblOverboughtLevel.AutoSize = true;
            this.lblOverboughtLevel.Location = new Point(7, 60);
            this.lblOverboughtLevel.Name = "lblOverboughtLevel";
            this.lblOverboughtLevel.Size = new Size(0x5f, 13);
            this.lblOverboughtLevel.TabIndex = 1;
            this.lblOverboughtLevel.Text = "Overbought Level:";
            this.cbDrawOscillator.CheckAlign = ContentAlignment.TopLeft;
            this.cbDrawOscillator.Location = new Point(7, 14);
            this.cbDrawOscillator.Name = "cbDrawOscillator";
            this.cbDrawOscillator.Size = new Size(170, 0x2b);
            this.cbDrawOscillator.TabIndex = 0;
            this.cbDrawOscillator.Text = "Draw as an Oscillator with colored Overbought and Oversold areas";
            this.cbDrawOscillator.UseVisualStyleBackColor = true;
            this.cbDrawOscillator.CheckedChanged += new EventHandler(this.cbDrawOscillator_CheckedChanged);
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoSize = true;
            base.ClientSize = new Size(0x11f, 0x94);
            base.Controls.Add(this.cbUseIndicatorPane);
            base.Controls.Add(this.grpOscillator);
            base.Controls.Add(this.grpBand);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.grpParameters);
            base.Controls.Add(this.grpDrawing);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "IndicatorParametersForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Indicator Settings - SMA";
            this.grpDrawing.ResumeLayout(false);
            this.grpDrawing.PerformLayout();
            this.numWidth.EndInit();
            this.grpBand.ResumeLayout(false);
            this.grpBand.PerformLayout();
            this.numTransparency.EndInit();
            this.grpOscillator.ResumeLayout(false);
            this.grpOscillator.PerformLayout();
            this.numOscTransparency.EndInit();
            this.numOversoldLevel.EndInit();
            this.numOverboughtLevel.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private Color method_0(Color color_0, NumericUpDown numericUpDown_0)
        {
            int num = (int) numericUpDown_0.Value;
            int alpha = (int) ((100 - num) * 2.55M);
            return Color.FromArgb(alpha, color_0.R, color_0.G, color_0.B);
        }

        private void method_1(ColorPickerPanel colorPickerPanel_0, NumericUpDown numericUpDown_0, Color color_0)
        {
            numericUpDown_0.Value = (int) (((double) (0xff - color_0.A)) / 2.55);
            colorPickerPanel_0.BackColor = Color.FromArgb(color_0.R, color_0.G, color_0.B);
        }

        private void method_2()
        {
            int num = this.grpParameters.Location.Y + this.grpParameters.Height;
            int num2 = this.grpOscillator.Location.Y + this.grpOscillator.Height;
            if ((base.Width > (365f * this.float_0)) && (num < num2))
            {
                int num4 = num2 - num;
                this.grpParameters.Height += num4;
                base.Height += num4;
            }
            else if (num2 < num)
            {
                int num3 = num - num2;
                this.grpOscillator.Height += num3;
            }
        }

        public void SetIndicatorParameterValue(int index, string value)
        {
            if (this.grpParameters.Controls.Count > (index * 2))
            {
                Control control = this.grpParameters.Controls[(index * 2) + 1];
                if (control is CheckBox)
                {
                    CheckBox box = control as CheckBox;
                    box.Checked = value == "true";
                }
                else if (control is NumericUpDown)
                {
                    NumericUpDown down = control as NumericUpDown;
                    down.Value = decimal.Parse(value);
                }
                else if (control is DateTimePicker)
                {
                    DateTimePicker picker = control as DateTimePicker;
                    picker.Value = DateTime.Parse(value);
                }
                else if (control is ComboBox)
                {
                    ComboBox box2 = control as ComboBox;
                    if ((box2.Tag == null) && (value != ""))
                    {
                        box2.Text = value;
                    }
                }
                else
                {
                    control.Text = value;
                }
            }
        }

        private int _cbUseIndicatorPaneReSize
        {
            get
            {
                if (!this.bool_0)
                {
                    return 0;
                }
                return this.cbUseIndicatorPane.Height;
            }
        }

        public Color BandFillColor
        {
            get
            {
                return this.pnlBandColor.BackColor;
            }
            set
            {
                this.pnlBandColor.BackColor = value;
            }
        }

        public int BandFillTransparency
        {
            get
            {
                return (int) this.numTransparency.Value;
            }
            set
            {
                this.numTransparency.Value = value;
            }
        }

        public bool FillBand
        {
            get
            {
                return this.cbFillBands.Checked;
            }
            set
            {
                this.cbFillBands.Checked = value;
            }
        }

        public Color IndicatorColor
        {
            get
            {
                return this.pnlColor.BackColor;
            }
            set
            {
                this.pnlColor.BackColor = value;
            }
        }

        public int IndicatorParameterCount
        {
            get
            {
                return (this.grpParameters.Controls.Count / 2);
            }
        }

        public LineStyle IndicatorStyle
        {
            get
            {
                return (LineStyle) Enum.Parse(typeof(LineStyle), this.cmbStyle.Text);
            }
            set
            {
                this.cmbStyle.Text = value.ToString();
            }
        }

        public int IndicatorWidth
        {
            get
            {
                return (int) this.numWidth.Value;
            }
            set
            {
                this.numWidth.Value = value;
            }
        }

        public int OscillatorFillTransparency
        {
            get
            {
                return (int) this.numOscTransparency.Value;
            }
            set
            {
                this.numOscTransparency.Value = value;
            }
        }

        public Color OscillatorOverboughtColor
        {
            get
            {
                return this.pnlOverboughtColor.BackColor;
            }
            set
            {
                this.pnlOverboughtColor.BackColor = value;
            }
        }

        public double OscillatorOverboughtLevel
        {
            get
            {
                return (double) this.numOverboughtLevel.Value;
            }
            set
            {
                this.numOverboughtLevel.Value = (decimal) value;
            }
        }

        public Color OscillatorOversoldColor
        {
            get
            {
                return this.pnlOversoldColor.BackColor;
            }
            set
            {
                this.pnlOversoldColor.BackColor = value;
            }
        }

        public double OscillatorOversoldLevel
        {
            get
            {
                return (double) this.numOversoldLevel.Value;
            }
            set
            {
                this.numOversoldLevel.Value = (decimal) value;
            }
        }

        public bool PlotAsOscillator
        {
            get
            {
                return (((this.indicatorHelper != null) && this.indicatorHelper.IsOscillator) && this.cbDrawOscillator.Checked);
            }
            set
            {
                this.cbDrawOscillator.Checked = value;
            }
        }

        public bool PlotBandPairIndicator
        {
            get
            {
                if (this.indicatorHelper == null)
                {
                    return false;
                }
                return ((this.indicatorHelper.PartnerBandIndicatorType != null) && this.cbPlotBands.Checked);
            }
            set
            {
                this.cbPlotBands.Checked = value;
            }
        }

        public bool UseIndicatorPane
        {
            [CompilerGenerated]
            get
            {
                return this.useIndicatorPane;
            }
            [CompilerGenerated]
            internal set
            {
                this.useIndicatorPane = value;
            }
        }
    }
}

