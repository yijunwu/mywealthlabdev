namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PieSeries : BaseSeriesForm
    {
        private Button BPen;
        private Button BShadowColor;
        private CheckBox CBAutoMarkPosition;
        private CheckBox CBDark3d;
        private ComboBox CBEdgeStyle;
        private ComboBox CBOther;
        private CheckBox CBPatterns;
        private CheckBox cbShadowVisible;
        private CircledSeries circledEditor;
        private Container components;
        private TextBox EOtherLabel;
        private TextBox EOtherValue;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label lEdgeStyle;
        private Pie pie;
        private NumericUpDown UDAngleSize;
        private NumericUpDown UDBevelPercentage;
        private NumericUpDown UDExpBig;
        private NumericUpDown UDShadowHoriz;
        private NumericUpDown UDShadowVert;
        private NumericUpDown UDTransparency;

        public PieSeries()
        {
            this.InitializeComponent();
        }

        public PieSeries(Series s) : this()
        {
            this.pie = (Pie) s;
        }

        public PieSeries(Pie s, Control parent, CircledSeries cEditor) : this()
        {
            this.pie = s;
            this.circledEditor = cEditor;
            this.circledEditor.BBack.Visible = false;
            this.circledEditor.BBGrad.Visible = false;
            EditorUtils.InsertForm(this, parent);
        }

        private void BPen_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.pie.Pen);
        }

        private void BShadowColor_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.pie.Shadow.Brush);
        }

        private void CBAutoMarkPosition_CheckedChanged(object sender, EventArgs e)
        {
            this.pie.AutoMarkPosition = this.CBAutoMarkPosition.Checked;
        }

        private void CBDark3d_CheckedChanged(object sender, EventArgs e)
        {
            this.pie.Dark3D = this.CBDark3d.Checked;
        }

        private void CBEdgeStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.CBEdgeStyle.SelectedIndex)
            {
                case 0:
                    this.pie.EdgeStyle = EdgeStyles.Curved;
                    return;

                case 1:
                    this.pie.EdgeStyle = EdgeStyles.Flat;
                    return;

                case 2:
                    this.pie.EdgeStyle = EdgeStyles.None;
                    return;
            }
        }

        private void CBOther_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.CBOther.SelectedIndex)
            {
                case 0:
                    this.pie.OtherSlice.Style = PieOtherStyles.None;
                    return;

                case 1:
                    this.pie.OtherSlice.Style = PieOtherStyles.BelowPercent;
                    return;

                case 2:
                    this.pie.OtherSlice.Style = PieOtherStyles.BelowValue;
                    return;
            }
        }

        private void CBPatterns_CheckedChanged(object sender, EventArgs e)
        {
            this.pie.UsePatterns = this.CBPatterns.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.pie != null)
            {
                this.pie.Shadow.Visible = this.cbShadowVisible.Checked;
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

        private void EOtherLabel_TextChanged(object sender, EventArgs e)
        {
            if (this.pie != null)
            {
                this.pie.OtherSlice.Text = this.EOtherLabel.Text;
            }
        }

        private void EOtherValue_TextChanged(object sender, EventArgs e)
        {
            if (this.EOtherValue.Text.Length != 0)
            {
                this.pie.OtherSlice.Value = Utils.StringToDouble(this.EOtherValue.Text, 0.0);
            }
        }

        private void InitializeComponent()
        {
            this.CBDark3d = new CheckBox();
            this.CBPatterns = new CheckBox();
            this.BPen = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.UDExpBig = new NumericUpDown();
            this.UDAngleSize = new NumericUpDown();
            this.groupBox1 = new GroupBox();
            this.EOtherLabel = new TextBox();
            this.EOtherValue = new TextBox();
            this.CBOther = new ComboBox();
            this.label5 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.groupBox2 = new GroupBox();
            this.cbShadowVisible = new CheckBox();
            this.UDShadowVert = new NumericUpDown();
            this.UDShadowHoriz = new NumericUpDown();
            this.label7 = new Label();
            this.label6 = new Label();
            this.BShadowColor = new Button();
            this.CBAutoMarkPosition = new CheckBox();
            this.UDTransparency = new NumericUpDown();
            this.label8 = new Label();
            this.label9 = new Label();
            this.UDBevelPercentage = new NumericUpDown();
            this.CBEdgeStyle = new ComboBox();
            this.lEdgeStyle = new Label();
            this.UDExpBig.BeginInit();
            this.UDAngleSize.BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.UDShadowVert.BeginInit();
            this.UDShadowHoriz.BeginInit();
            this.UDTransparency.BeginInit();
            this.UDBevelPercentage.BeginInit();
            base.SuspendLayout();
            this.CBDark3d.FlatStyle = FlatStyle.Flat;
            this.CBDark3d.Location = new Point(0xbb, 0x21);
            this.CBDark3d.Name = "CBDark3d";
            this.CBDark3d.Size = new Size(0x7a, 20);
            this.CBDark3d.TabIndex = 5;
            this.CBDark3d.Text = "&Dark 3D";
            this.CBDark3d.CheckedChanged += new EventHandler(this.CBDark3d_CheckedChanged);
            this.CBPatterns.FlatStyle = FlatStyle.Flat;
            this.CBPatterns.Location = new Point(0x11a, 5);
            this.CBPatterns.Name = "CBPatterns";
            this.CBPatterns.Size = new Size(0x4b, 20);
            this.CBPatterns.TabIndex = 6;
            this.CBPatterns.Text = "Pa&tterns";
            this.CBPatterns.CheckedChanged += new EventHandler(this.CBPatterns_CheckedChanged);
            this.BPen.FlatStyle = FlatStyle.Flat;
            this.BPen.Location = new Point(0xbb, 4);
            this.BPen.Name = "BPen";
            this.BPen.Size = new Size(0x4b, 0x17);
            this.BPen.TabIndex = 4;
            this.BPen.Text = "&Border...";
            this.BPen.Click += new EventHandler(this.BPen_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(14, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Explode biggest:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(14, 0x23);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x3f, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Total angle:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.UDExpBig.BorderStyle = BorderStyle.FixedSingle;
            int[] bits = new int[4];
            bits[0] = 5;
            this.UDExpBig.Increment = new decimal(bits);
            this.UDExpBig.Location = new Point(0x73, 7);
            this.UDExpBig.Name = "UDExpBig";
            this.UDExpBig.Size = new Size(0x30, 20);
            this.UDExpBig.TabIndex = 1;
            this.UDExpBig.TextAlign = HorizontalAlignment.Right;
            this.UDExpBig.ValueChanged += new EventHandler(this.UDExpBig_ValueChanged);
            this.UDExpBig.TextChanged += new EventHandler(this.UDExpBig_TextChanged);
            this.UDAngleSize.BorderStyle = BorderStyle.FixedSingle;
            int[] numArray2 = new int[4];
            numArray2[0] = 5;
            this.UDAngleSize.Increment = new decimal(numArray2);
            this.UDAngleSize.Location = new Point(0x73, 0x21);
            int[] numArray3 = new int[4];
            numArray3[0] = 360;
            this.UDAngleSize.Maximum = new decimal(numArray3);
            int[] numArray4 = new int[4];
            numArray4[0] = 1;
            this.UDAngleSize.Minimum = new decimal(numArray4);
            this.UDAngleSize.Name = "UDAngleSize";
            this.UDAngleSize.Size = new Size(0x30, 20);
            this.UDAngleSize.TabIndex = 3;
            this.UDAngleSize.TextAlign = HorizontalAlignment.Right;
            int[] numArray5 = new int[4];
            numArray5[0] = 360;
            this.UDAngleSize.Value = new decimal(numArray5);
            this.UDAngleSize.ValueChanged += new EventHandler(this.UDAngleSize_ValueChanged);
            this.UDAngleSize.TextChanged += new EventHandler(this.UDAngleSize_ValueChanged);
            this.groupBox1.Controls.Add(this.EOtherLabel);
            this.groupBox1.Controls.Add(this.EOtherValue);
            this.groupBox1.Controls.Add(this.CBOther);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new Point(0x11, 0x6f);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xa4, 0x63);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Group slices:";
            this.EOtherLabel.BorderStyle = BorderStyle.FixedSingle;
            this.EOtherLabel.Location = new Point(0x40, 0x45);
            this.EOtherLabel.Name = "EOtherLabel";
            this.EOtherLabel.Size = new Size(0x60, 20);
            this.EOtherLabel.TabIndex = 5;
            this.EOtherLabel.Text = "Other";
            this.EOtherLabel.TextChanged += new EventHandler(this.EOtherLabel_TextChanged);
            this.EOtherValue.BorderStyle = BorderStyle.FixedSingle;
            this.EOtherValue.Location = new Point(0x40, 0x2d);
            this.EOtherValue.Name = "EOtherValue";
            this.EOtherValue.Size = new Size(0x38, 20);
            this.EOtherValue.TabIndex = 3;
            this.EOtherValue.Text = "0";
            this.EOtherValue.TextAlign = HorizontalAlignment.Right;
            this.EOtherValue.TextChanged += new EventHandler(this.EOtherValue_TextChanged);
            this.CBOther.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBOther.Items.AddRange(new object[] { "None", "Below %", "Below Value" });
            this.CBOther.Location = new Point(0x40, 0x15);
            this.CBOther.Name = "CBOther";
            this.CBOther.Size = new Size(0x60, 0x15);
            this.CBOther.TabIndex = 1;
            this.CBOther.SelectedIndexChanged += new EventHandler(this.CBOther_SelectedIndexChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x1c, 0x47);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x24, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "&Label:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x1c, 0x2f);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x25, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "&Value:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x1f, 0x17);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x21, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "&Style:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.groupBox2.Controls.Add(this.cbShadowVisible);
            this.groupBox2.Controls.Add(this.UDShadowVert);
            this.groupBox2.Controls.Add(this.UDShadowHoriz);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.BShadowColor);
            this.groupBox2.Location = new Point(0xbb, 0x6f);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xbc, 0x63);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Shadow:";
            this.cbShadowVisible.FlatStyle = FlatStyle.Flat;
            this.cbShadowVisible.Location = new Point(10, 0x15);
            this.cbShadowVisible.Name = "cbShadowVisible";
            this.cbShadowVisible.Size = new Size(0x41, 0x10);
            this.cbShadowVisible.TabIndex = 0;
            this.cbShadowVisible.Text = "&Visible";
            this.cbShadowVisible.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.UDShadowVert.BorderStyle = BorderStyle.FixedSingle;
            this.UDShadowVert.Location = new Point(0x7b, 70);
            int[] numArray6 = new int[4];
            numArray6[0] = 0x3e8;
            this.UDShadowVert.Maximum = new decimal(numArray6);
            int[] numArray7 = new int[4];
            numArray7[0] = 0x3e8;
            numArray7[3] = -2147483648;
            this.UDShadowVert.Minimum = new decimal(numArray7);
            this.UDShadowVert.Name = "UDShadowVert";
            this.UDShadowVert.Size = new Size(0x38, 20);
            this.UDShadowVert.TabIndex = 5;
            this.UDShadowVert.TextAlign = HorizontalAlignment.Right;
            this.UDShadowVert.ValueChanged += new EventHandler(this.UDShadowVert_ValueChanged);
            this.UDShadowVert.TextChanged += new EventHandler(this.UDShadowVert_TextChanged);
            this.UDShadowHoriz.BorderStyle = BorderStyle.FixedSingle;
            this.UDShadowHoriz.Location = new Point(0x7b, 0x2e);
            int[] numArray8 = new int[4];
            numArray8[0] = 0x3e8;
            this.UDShadowHoriz.Maximum = new decimal(numArray8);
            int[] numArray9 = new int[4];
            numArray9[0] = 0x3e8;
            numArray9[3] = -2147483648;
            this.UDShadowHoriz.Minimum = new decimal(numArray9);
            this.UDShadowHoriz.Name = "UDShadowHoriz";
            this.UDShadowHoriz.Size = new Size(0x38, 20);
            this.UDShadowHoriz.TabIndex = 3;
            this.UDShadowHoriz.TextAlign = HorizontalAlignment.Right;
            this.UDShadowHoriz.ValueChanged += new EventHandler(this.UDShadowHoriz_ValueChanged);
            this.UDShadowHoriz.TextChanged += new EventHandler(this.UDShadowHoriz_TextChanged);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x37, 0x30);
            this.label7.Name = "label7";
            this.label7.Size = new Size(60, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "&Horiz. Size:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x3d, 0x48);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x37, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "&Vert. Size:";
            this.label6.TextAlign = ContentAlignment.TopRight;
            this.BShadowColor.FlatStyle = FlatStyle.Flat;
            this.BShadowColor.Location = new Point(0x6f, 0x10);
            this.BShadowColor.Name = "BShadowColor";
            this.BShadowColor.Size = new Size(0x44, 0x17);
            this.BShadowColor.TabIndex = 1;
            this.BShadowColor.Text = "&Color...";
            this.BShadowColor.Click += new EventHandler(this.BShadowColor_Click);
            this.CBAutoMarkPosition.FlatStyle = FlatStyle.Flat;
            this.CBAutoMarkPosition.Location = new Point(0xbb, 0x36);
            this.CBAutoMarkPosition.Name = "CBAutoMarkPosition";
            this.CBAutoMarkPosition.Size = new Size(0xba, 20);
            this.CBAutoMarkPosition.TabIndex = 7;
            this.CBAutoMarkPosition.Text = "&Auto Mark Position";
            this.CBAutoMarkPosition.CheckedChanged += new EventHandler(this.CBAutoMarkPosition_CheckedChanged);
            this.UDTransparency.BorderStyle = BorderStyle.FixedSingle;
            int[] numArray10 = new int[4];
            numArray10[0] = 5;
            this.UDTransparency.Increment = new decimal(numArray10);
            this.UDTransparency.Location = new Point(0x73, 0x3b);
            this.UDTransparency.Name = "UDTransparency";
            this.UDTransparency.Size = new Size(0x30, 20);
            this.UDTransparency.TabIndex = 10;
            this.UDTransparency.TextAlign = HorizontalAlignment.Right;
            this.UDTransparency.ValueChanged += new EventHandler(this.UDTransparency_ValueChanged);
            this.label8.AutoSize = true;
            this.label8.Location = new Point(14, 0x3d);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x4b, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Transparency:";
            this.label8.TextAlign = ContentAlignment.TopRight;
            this.label9.AutoSize = true;
            this.label9.Location = new Point(14, 0x57);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x5f, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Bevel Percentage:";
            this.label9.TextAlign = ContentAlignment.TopRight;
            this.UDBevelPercentage.BorderStyle = BorderStyle.FixedSingle;
            int[] numArray11 = new int[4];
            numArray11[0] = 5;
            this.UDBevelPercentage.Increment = new decimal(numArray11);
            this.UDBevelPercentage.Location = new Point(0x73, 0x55);
            this.UDBevelPercentage.Name = "UDBevelPercentage";
            this.UDBevelPercentage.Size = new Size(0x30, 20);
            this.UDBevelPercentage.TabIndex = 12;
            this.UDBevelPercentage.TextAlign = HorizontalAlignment.Right;
            this.UDBevelPercentage.ValueChanged += new EventHandler(this.UDBevelPercentage_ValueChanged);
            this.CBEdgeStyle.FormattingEnabled = true;
            this.CBEdgeStyle.Location = new Point(0xec, 0x54);
            this.CBEdgeStyle.Name = "CBEdgeStyle";
            this.CBEdgeStyle.Size = new Size(0x79, 0x15);
            this.CBEdgeStyle.TabIndex = 14;
            this.CBEdgeStyle.SelectedIndexChanged += new EventHandler(this.CBEdgeStyle_SelectedIndexChanged);
            this.lEdgeStyle.AutoSize = true;
            this.lEdgeStyle.Location = new Point(0xa9, 0x57);
            this.lEdgeStyle.Name = "lEdgeStyle";
            this.lEdgeStyle.Size = new Size(0x3d, 13);
            this.lEdgeStyle.TabIndex = 15;
            this.lEdgeStyle.Text = "Edge Style:";
            this.lEdgeStyle.TextAlign = ContentAlignment.TopRight;
            base.ClientSize = new Size(0x17a, 0xd5);
            base.Controls.Add(this.lEdgeStyle);
            base.Controls.Add(this.CBEdgeStyle);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.UDBevelPercentage);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.UDTransparency);
            base.Controls.Add(this.CBAutoMarkPosition);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.UDAngleSize);
            base.Controls.Add(this.UDExpBig);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.BPen);
            base.Controls.Add(this.CBPatterns);
            base.Controls.Add(this.CBDark3d);
            base.Name = "PieSeries";
            this.UDExpBig.EndInit();
            this.UDAngleSize.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.UDShadowVert.EndInit();
            this.UDShadowHoriz.EndInit();
            this.UDTransparency.EndInit();
            this.UDBevelPercentage.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.pie != null)
            {
                if (this.circledEditor == null)
                {
                    this.circledEditor = CircledSeries.InsertForm(Parent, this.pie);
                    this.circledEditor.BBack.Visible = false;
                    this.circledEditor.BBGrad.Visible = false;
                }
                this.CBDark3d.Checked = this.pie.Dark3D;
                this.CBPatterns.Checked = this.pie.UsePatterns;
                this.UDExpBig.Value = this.pie.ExplodeBiggest;
                this.UDAngleSize.Value = this.pie.AngleSize;
                this.cbShadowVisible.Checked = this.pie.Shadow.Visible;
                this.CBAutoMarkPosition.Checked = this.pie.AutoMarkPosition;
                this.UDTransparency.Value = this.pie.Transparency;
                this.UDBevelPercentage.Value = this.pie.BevelPercent;
                this.CBEdgeStyle.Items.Add(Enum.GetName(typeof(EdgeStyles), EdgeStyles.Curved));
                this.CBEdgeStyle.Items.Add(Enum.GetName(typeof(EdgeStyles), EdgeStyles.Flat));
                this.CBEdgeStyle.Items.Add(Enum.GetName(typeof(EdgeStyles), EdgeStyles.None));
                if (this.pie.BevelPercent > 0)
                {
                    this.CBEdgeStyle.SelectedItem = Enum.GetName(typeof(EdgeStyles), this.pie.EdgeStyle);
                }
                else
                {
                    this.CBEdgeStyle.SelectedIndex = 2;
                    this.CBEdgeStyle.Enabled = false;
                    this.lEdgeStyle.Enabled = false;
                }
                if (this.pie.OtherSlice.Style == PieOtherStyles.None)
                {
                    this.CBOther.SelectedIndex = 0;
                }
                else if (this.pie.OtherSlice.Style == PieOtherStyles.BelowPercent)
                {
                    this.CBOther.SelectedIndex = 1;
                }
                else if (this.pie.OtherSlice.Style == PieOtherStyles.BelowValue)
                {
                    this.CBOther.SelectedIndex = 2;
                }
                this.EOtherValue.Text = this.pie.OtherSlice.Value.ToString();
                this.EOtherLabel.Text = this.pie.OtherSlice.Text;
                this.UDShadowHoriz.Value = this.pie.Shadow.Width;
                this.UDShadowVert.Value = this.pie.Shadow.Height;
            }
        }

        private void UDAngleSize_ValueChanged(object sender, EventArgs e)
        {
            if (this.pie != null)
            {
                this.pie.AngleSize = (int) this.UDAngleSize.Value;
            }
        }

        private void UDBevelPercentage_ValueChanged(object sender, EventArgs e)
        {
            this.pie.BevelPercent = (int) this.UDBevelPercentage.Value;
            if (this.pie.BevelPercent > 0)
            {
                this.CBEdgeStyle.SelectedItem = this.pie.EdgeStyle;
                this.CBEdgeStyle.Enabled = true;
                this.lEdgeStyle.Enabled = true;
            }
            else
            {
                this.CBEdgeStyle.SelectedIndex = 2;
                this.CBEdgeStyle.Enabled = false;
                this.lEdgeStyle.Enabled = false;
            }
        }

        private void UDExpBig_TextChanged(object sender, EventArgs e)
        {
            this.UDExpBig_ValueChanged(sender, e);
        }

        private void UDExpBig_ValueChanged(object sender, EventArgs e)
        {
            if (this.pie != null)
            {
                this.pie.ExplodeBiggest = (int) this.UDExpBig.Value;
            }
        }

        private void UDShadowHoriz_TextChanged(object sender, EventArgs e)
        {
            this.UDShadowHoriz_ValueChanged(sender, e);
        }

        private void UDShadowHoriz_ValueChanged(object sender, EventArgs e)
        {
            if (this.pie != null)
            {
                this.pie.Shadow.Width = (int) this.UDShadowHoriz.Value;
            }
        }

        private void UDShadowVert_TextChanged(object sender, EventArgs e)
        {
            this.UDShadowVert_ValueChanged(sender, e);
        }

        private void UDShadowVert_ValueChanged(object sender, EventArgs e)
        {
            if (this.pie != null)
            {
                this.pie.Shadow.Height = (int) this.UDShadowVert.Value;
            }
        }

        private void UDTransparency_ValueChanged(object sender, EventArgs e)
        {
            this.pie.Transparency = (int) this.UDTransparency.Value;
        }
    }
}

