namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Functions;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FunctionEditor : Form
    {
        private bool addingPages;
        private ADXFunctionEditor adxFunction;
        private Button bApply;
        private Button BNone;
        private BollingerFunctionEditor bollingerFunction;
        private ComboBox cbFunctions;
        private ComboBox CBSingle;
        private ComboBox CBValues;
        private CCIFunctionEditor cciFunction;
        private CLVFunctionEditor clvFunction;
        private Container components;
        private CompressOHLCFunctionEditor compressOHLC;
        private CustomFunctionEditor custom;
        private Label label1;
        private Label label2;
        private Label LValues;
        private MACDFunctionEditor macdFunction;
        private MovingAverageFunctionEditor movavgFunction;
        private OBVFunctionEditor obvFunction;
        private PeriodEditor options;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel PanSingle;
        private PVOFunctionEditor pvoFunction;
        internal SelectListForm select;
        private Steema.TeeChart.Styles.Series series;
        private bool singleSource;
        private SmoothingFunctionEditor smoothingFunction;
        private TabPage tabADX;
        private TabPage tabBollinger;
        private TabPage tabCCI;
        private TabPage tabCLV;
        private TabPage tabCompress;
        private TabControl tabControl1;
        private TabPage tabCustom;
        private TabPage tabMACD;
        private TabPage tabMovAvg;
        private TabPage tabOBV;
        private TabPage tabOptions;
        private TabPage tabPage1;
        private TabPage tabPVO;
        private TabPage tabSmoothing;

        public FunctionEditor()
        {
            this.InitializeComponent();
        }

        public FunctionEditor(Steema.TeeChart.Styles.Series s)
            : this()
        {
            this.series = s;
            this.FillTeeFuntions(this.cbFunctions);
            if (this.series != null)
            {
                this.singleSource = (this.series.Function == null) || (!this.series.Function.NoSourceRequired && this.series.Function.SingleSource);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.select = new SelectListForm();
                this.select.controlToEnable = this.bApply;
                EditorUtils.InsertForm(this.select, this.tabPage1);
                EditorUtils.Translate(this.tabPage1);
                this.FillSeries(false);
                this.select.ToList.BeginUpdate();
                this.select.ToList.Items.Clear();
                ArrayList list = this.series.DataSourceArray();
                if (list != null)
                {
                    foreach (object obj2 in list)
                    {
                        if (obj2 is Steema.TeeChart.Styles.Series)
                        {
                            Steema.TeeChart.Styles.Series series = (Steema.TeeChart.Styles.Series)obj2;
                            this.select.ToList.Items.Add(series.ToString());
                            int index = this.select.FromList.Items.IndexOf(series.ToString());
                            if (index != -1)
                            {
                                this.select.FromList.Items.RemoveAt(index);
                            }
                        }
                    }
                }
                this.select.ToList.EndUpdate();
                if (this.series.Function == null)
                {
                    this.cbFunctions.SelectedIndex = 0;
                }
                else
                {
                    int num2 = this.FunctionIndexOf(this.series.Function.GetType());
                    this.cbFunctions.SelectedIndex = num2 + 1;
                }
                this.select.EnableButtons();
            }
            this.bApply.Enabled = false;
        }

        private void AddPages(bool withOptions)
        {
            this.addingPages = true;
            this.tabControl1.TabPages.Clear();
            this.addingPages = false;
            if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(Steema.TeeChart.Functions.Custom)))
            {
                if ((this.series.Function == null) || !(this.series.Function is Steema.TeeChart.Functions.Custom))
                {
                    this.series.Function = new Steema.TeeChart.Functions.Custom();
                }
                this.tabControl1.TabPages.Add(this.tabCustom);
                this.tabControl1.SelectedTab = this.tabCustom;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(CompressOHLC)))
            {
                if ((this.series.Function == null) || !(this.series.Function is CompressOHLC))
                {
                    this.series.Function = new CompressOHLC();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabCompress);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(CLVFunction)))
            {
                if ((this.series.Function == null) || !(this.series.Function is CLVFunction))
                {
                    this.series.Function = new CLVFunction();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabCLV);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(OBVFunction)))
            {
                if ((this.series.Function == null) || !(this.series.Function is OBVFunction))
                {
                    this.series.Function = new OBVFunction();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabOBV);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(CCIFunction)))
            {
                if ((this.series.Function == null) || !(this.series.Function is CCIFunction))
                {
                    this.series.Function = new CCIFunction();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabCCI);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(MovingAverage)))
            {
                if ((this.series.Function == null) || !(this.series.Function is MovingAverage))
                {
                    this.series.Function = new MovingAverage();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabMovAvg);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(PVOFunction)))
            {
                if ((this.series.Function == null) || !(this.series.Function is PVOFunction))
                {
                    this.series.Function = new PVOFunction();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabPVO);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(Bollinger)))
            {
                if ((this.series.Function == null) || !(this.series.Function is Bollinger))
                {
                    this.series.Function = new Bollinger();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabBollinger);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(MACDFunction)))
            {
                if ((this.series.Function == null) || !(this.series.Function is MACDFunction))
                {
                    this.series.Function = new MACDFunction();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabMACD);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(ADXFunction)))
            {
                if ((this.series.Function == null) || !(this.series.Function is ADXFunction))
                {
                    this.series.Function = new ADXFunction();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabADX);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(Smoothing)))
            {
                if ((this.series.Function == null) || !(this.series.Function is Smoothing))
                {
                    this.series.Function = new Smoothing();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabSmoothing);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else
            {
                if ((this.cbFunctions.SelectedIndex != 0) && ((this.series.Function == null) || (this.series.Function.GetType() != Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1])))
                {
                    this.series.Function = Function.NewInstance((System.Type) Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1]);
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                if (withOptions)
                {
                    this.tabControl1.TabPages.Add(this.tabOptions);
                }
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
        }

        private void AddSource(Steema.TeeChart.Styles.Series ASeries, bool addCurrent)
        {
            if (this.series.Chart.IsValidDataSource(this.series, ASeries))
            {
                this.CBSingle.Items.Add(ASeries);
                if (addCurrent || !this.series.HasDataSource(ASeries))
                {
                    this.select.FromList.Items.Add(ASeries.ToString());
                }
            }
        }

        private void bApply_Click(object sender, EventArgs e)
        {
            if (this.options != null)
            {
                this.options.Apply();
            }
            this.DoApply();
            this.bApply.Enabled = false;
        }

        private void BNone_Click(object sender, EventArgs e)
        {
            this.CBSingle.SelectedIndex = -1;
            this.CBSingle_SelectedIndexChanged(this, null);
        }

        private void cbFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.TryCreateNewFunction();
            this.AddPages(this.FunctionType != null);
            this.singleSource = (this.series.Function == null) || (!this.series.Function.NoSourceRequired && this.series.Function.SingleSource);
            this.PanSingle.Visible = this.singleSource;
            if (this.singleSource && (this.series.Function != null))
            {
                this.LValues.Visible = !this.series.Function.HideSourceList;
                this.CBValues.Visible = !this.series.Function.HideSourceList;
            }
        }

        private void CBSingle_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.bApply.Enabled = true;
            this.CBValues.Items.Clear();
            this.CBValues.Enabled = this.CBSingle.SelectedIndex != -1;
            this.BNone.Enabled = this.CBValues.Enabled;
            if (this.CBValues.Enabled)
            {
                Steema.TeeChart.Styles.Series selectedItem = this.CBSingle.SelectedItem as Steema.TeeChart.Styles.Series;
                for (int i = 1; i < selectedItem.ValuesLists.Count; i++)
                {
                    this.CBValues.Items.Add(selectedItem.ValuesLists[i].Name);
                }
                if (this.CBValues.Items.Count > 0)
                {
                    this.CBValues.SelectedIndex = 0;
                }
            }
        }

        private void CBValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.bApply.Enabled = true;
        }

        protected void ClearMembers()
        {
            this.series.labelMember = "";
            foreach (Steema.TeeChart.Styles.ValueList list in this.series.ValuesLists)
            {
                list.valueSource = "";
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

        private void DoApply()
        {
            this.ClearMembers();
            if (this.singleSource)
            {
                if (this.CBSingle.SelectedIndex == -1)
                {
                    this.series.DataSource = null;
                    this.series.mandatory.valueSource = "";
                }
                else
                {
                    this.series.DataSource = this.CBSingle.SelectedItem as Steema.TeeChart.Styles.Series;
                    if (this.CBValues.SelectedIndex == -1)
                    {
                        this.series.mandatory.valueSource = "";
                    }
                    else
                    {
                        this.series.mandatory.valueSource = this.CBValues.SelectedItem.ToString();
                    }
                }
            }
            else if (this.select.ToList.Items.Count == 0)
            {
                this.series.DataSource = null;
            }
            else
            {
                foreach (string str in this.select.ToList.Items)
                {
                    Steema.TeeChart.Styles.Series dest = this.series.chart.series.WithTitle(str);
                    this.series.CheckOtherSeries(dest);
                }
                if (this.select.ToList.Items.Count == 1)
                {
                    this.series.DataSource = this.series.chart.series.WithTitle(this.select.ToList.Items[0].ToString());
                }
                else
                {
                    object[] objArray = new object[this.select.ToList.Items.Count];
                    int index = 0;
                    foreach (string str2 in this.select.ToList.Items)
                    {
                        Steema.TeeChart.Styles.Series series2 = this.series.chart.series.WithTitle(str2);
                        objArray.SetValue(series2, index);
                        index++;
                    }
                    this.series.DataSource = objArray;
                }
            }
            System.Type functionType = this.FunctionType;
            if (functionType != null)
            {
                if ((this.series.Function == null) || (this.series.Function.GetType() != functionType))
                {
                    this.series.Function = Function.NewInstance(functionType);
                }
                else
                {
                    this.series.CheckDataSource();
                }
            }
            else
            {
                this.series.Function = null;
            }
        }

        private void FillSeries(bool addCurrent)
        {
            this.CBSingle.Items.Clear();
            this.select.FromList.BeginUpdate();
            this.select.FromList.Items.Clear();
            this.FillSources(addCurrent);
            this.select.FromList.EndUpdate();
            if (this.singleSource)
            {
                object dataSource = this.series.DataSource;
                if ((dataSource != null) && (dataSource is Steema.TeeChart.Styles.Series))
                {
                    int index = this.CBSingle.Items.IndexOf(dataSource);
                    if (index != this.CBSingle.SelectedIndex)
                    {
                        this.CBSingle.SelectedIndex = index;
                        this.CBSingle_SelectedIndexChanged(this, null);
                    }
                }
                else
                {
                    this.CBSingle.SelectedIndex = -1;
                }
            }
        }

        private void FillSources(bool AddCurrent)
        {
            foreach (Steema.TeeChart.Styles.Series series in this.series.chart.Series)
            {
                this.AddSource(series, AddCurrent);
            }
        }

        private void FillTeeFuntions(ComboBox ACombo)
        {
            if (this.series != null)
            {
                ACombo.Items.Add(Texts.FunctionNone);
                foreach (System.Type type in Utils.FunctionTypesOf)
                {
                    Function.NewInstance(type);
                    string str = Function.NewInstance(type).Description();
                    ACombo.Items.Add(str.Replace('\n', ' '));
                }
            }
        }

        private Function FunctionClass()
        {
            if (this.FunctionType == null)
            {
                return null;
            }
            return Function.NewInstance(this.FunctionType);
        }

        private void FunctionEditor_Load(object sender, EventArgs e)
        {
        }

        private int FunctionIndexOf(System.Type f)
        {
            for (int i = 0; i < Utils.FunctionTypesCount; i++)
            {
                if (Utils.FunctionTypesOf[i] == f)
                {
                    return i;
                }
            }
            return -1;
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.bApply = new Button();
            this.cbFunctions = new ComboBox();
            this.label1 = new Label();
            this.tabOptions = new TabPage();
            this.tabPage1 = new TabPage();
            this.PanSingle = new System.Windows.Forms.Panel();
            this.BNone = new Button();
            this.CBValues = new ComboBox();
            this.LValues = new Label();
            this.label2 = new Label();
            this.CBSingle = new ComboBox();
            this.tabControl1 = new TabControl();
            this.tabCompress = new TabPage();
            this.tabMovAvg = new TabPage();
            this.tabMACD = new TabPage();
            this.tabCLV = new TabPage();
            this.tabPVO = new TabPage();
            this.tabADX = new TabPage();
            this.tabCCI = new TabPage();
            this.tabBollinger = new TabPage();
            this.tabCustom = new TabPage();
            this.tabOBV = new TabPage();
            this.tabSmoothing = new TabPage();
            this.panel1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.PanSingle.SuspendLayout();
            this.tabControl1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.bApply);
            this.panel1.Controls.Add(this.cbFunctions);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x158, 0x19);
            this.panel1.TabIndex = 0;
            this.bApply.FlatStyle = FlatStyle.Flat;
            this.bApply.Location = new Point(0xf8, 1);
            this.bApply.Name = "bApply";
            this.bApply.TabIndex = 2;
            this.bApply.Text = "&Apply";
            this.bApply.Click += new EventHandler(this.bApply_Click);
            this.cbFunctions.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbFunctions.Location = new Point(0x5c, 1);
            this.cbFunctions.Name = "cbFunctions";
            this.cbFunctions.Size = new Size(140, 0x15);
            this.cbFunctions.TabIndex = 1;
            this.cbFunctions.SelectedIndexChanged += new EventHandler(this.cbFunctions_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x22, 3);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x39, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Functions:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.tabOptions.Location = new Point(4, 0x16);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Size = new Size(0x150, 0x9a);
            this.tabOptions.TabIndex = 1;
            this.tabOptions.Text = "Options";
            this.tabPage1.Controls.Add(this.PanSingle);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x150, 0x9a);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Source Series";
            this.PanSingle.Controls.Add(this.BNone);
            this.PanSingle.Controls.Add(this.CBValues);
            this.PanSingle.Controls.Add(this.LValues);
            this.PanSingle.Controls.Add(this.label2);
            this.PanSingle.Controls.Add(this.CBSingle);
            this.PanSingle.Dock = DockStyle.Fill;
            this.PanSingle.Location = new Point(0, 0);
            this.PanSingle.Name = "PanSingle";
            this.PanSingle.Size = new Size(0x150, 0x9a);
            this.PanSingle.TabIndex = 0;
            this.BNone.FlatStyle = FlatStyle.Flat;
            this.BNone.Location = new Point(0xe3, 0x10);
            this.BNone.Name = "BNone";
            this.BNone.TabIndex = 4;
            this.BNone.Text = "None";
            this.BNone.Click += new EventHandler(this.BNone_Click);
            this.CBValues.Location = new Point(0x55, 0x30);
            this.CBValues.Name = "CBValues";
            this.CBValues.Size = new Size(0x79, 0x15);
            this.CBValues.TabIndex = 3;
            this.CBValues.SelectedIndexChanged += new EventHandler(this.CBValues_SelectedIndexChanged);
            this.LValues.Location = new Point(0x20, 0x30);
            this.LValues.Name = "LValues";
            this.LValues.Size = new Size(0x30, 0x17);
            this.LValues.TabIndex = 2;
            this.LValues.Text = "&Values:";
            this.label2.Location = new Point(0x20, 0x10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x30, 0x17);
            this.label2.TabIndex = 1;
            this.label2.Text = "&Series:";
            this.CBSingle.Location = new Point(0x55, 0x10);
            this.CBSingle.Name = "CBSingle";
            this.CBSingle.Size = new Size(0x79, 0x15);
            this.CBSingle.TabIndex = 0;
            this.CBSingle.SelectedIndexChanged += new EventHandler(this.CBSingle_SelectedIndexChanged);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabCompress);
            this.tabControl1.Controls.Add(this.tabMovAvg);
            this.tabControl1.Controls.Add(this.tabMACD);
            this.tabControl1.Controls.Add(this.tabCLV);
            this.tabControl1.Controls.Add(this.tabPVO);
            this.tabControl1.Controls.Add(this.tabADX);
            this.tabControl1.Controls.Add(this.tabOptions);
            this.tabControl1.Controls.Add(this.tabCCI);
            this.tabControl1.Controls.Add(this.tabBollinger);
            this.tabControl1.Controls.Add(this.tabCustom);
            this.tabControl1.Controls.Add(this.tabOBV);
            this.tabControl1.Controls.Add(this.tabSmoothing);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.ItemSize = new Size(0x4e, 0x12);
            this.tabControl1.Location = new Point(0, 0x19);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x158, 180);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabCompress.Location = new Point(4, 0x16);
            this.tabCompress.Name = "tabCompress";
            this.tabCompress.Size = new Size(0x150, 0x9a);
            this.tabCompress.TabIndex = 3;
            this.tabCompress.Text = "Compress";
            this.tabMovAvg.Location = new Point(4, 0x16);
            this.tabMovAvg.Name = "tabMovAvg";
            this.tabMovAvg.Size = new Size(0x150, 0x9a);
            this.tabMovAvg.TabIndex = 7;
            this.tabMovAvg.Text = "Mov. Avg.";
            this.tabMACD.Location = new Point(4, 0x16);
            this.tabMACD.Name = "tabMACD";
            this.tabMACD.Size = new Size(0x150, 0x9a);
            this.tabMACD.TabIndex = 10;
            this.tabMACD.Text = "MACD";
            this.tabCLV.Location = new Point(4, 0x16);
            this.tabCLV.Name = "tabCLV";
            this.tabCLV.Size = new Size(0x150, 0x9a);
            this.tabCLV.TabIndex = 4;
            this.tabCLV.Text = "CLV";
            this.tabPVO.Location = new Point(4, 0x16);
            this.tabPVO.Name = "tabPVO";
            this.tabPVO.Size = new Size(0x150, 0x9a);
            this.tabPVO.TabIndex = 8;
            this.tabPVO.Text = "PVO";
            this.tabADX.Location = new Point(4, 0x16);
            this.tabADX.Name = "tabADX";
            this.tabADX.Size = new Size(0x150, 0x9a);
            this.tabADX.TabIndex = 11;
            this.tabADX.Text = "ADX";
            this.tabCCI.Location = new Point(4, 0x16);
            this.tabCCI.Name = "tabCCI";
            this.tabCCI.Size = new Size(0x150, 0x9a);
            this.tabCCI.TabIndex = 6;
            this.tabCCI.Text = "CCI";
            this.tabBollinger.Location = new Point(4, 0x16);
            this.tabBollinger.Name = "tabBollinger";
            this.tabBollinger.Size = new Size(0x150, 0x9a);
            this.tabBollinger.TabIndex = 9;
            this.tabBollinger.Text = "Bollinger";
            this.tabCustom.Location = new Point(4, 0x16);
            this.tabCustom.Name = "tabCustom";
            this.tabCustom.Size = new Size(0x150, 0x9a);
            this.tabCustom.TabIndex = 2;
            this.tabCustom.Text = "Custom";
            this.tabOBV.Location = new Point(4, 0x16);
            this.tabOBV.Name = "tabOBV";
            this.tabOBV.Size = new Size(0x150, 0x9a);
            this.tabOBV.TabIndex = 5;
            this.tabOBV.Text = "OBV";
            this.tabSmoothing.Location = new Point(4, 0x16);
            this.tabSmoothing.Name = "tabSmoothing";
            this.tabSmoothing.Size = new Size(0x150, 0x9a);
            this.tabSmoothing.TabIndex = 12;
            this.tabSmoothing.Text = "Smoothing";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x158, 0xcd);
            base.Controls.Add(this.tabControl1);
            base.Controls.Add(this.panel1);
            base.Name = "FunctionEditor";
            this.panel1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.PanSingle.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.addingPages)
            {
                if ((this.tabControl1.SelectedTab == this.tabOptions) || this.tabControl1.TabPages.Contains(this.tabOptions))
                {
                    if (this.bApply.Enabled)
                    {
                        this.bApply_Click(this.bApply, EventArgs.Empty);
                    }
                    this.bApply.Enabled = false;
                    if ((this.options == null) && (this.series.Function != null))
                    {
                        this.options = new PeriodEditor(this.series.Function);
                        EditorUtils.InsertForm(this.options, this.tabOptions);
                        EditorUtils.Translate(this.tabOptions);
                    }
                }
                else if (((this.tabControl1.SelectedTab == this.tabCustom) && (this.FunctionType == typeof(Steema.TeeChart.Functions.Custom))) || (this.tabControl1.TabPages.Contains(this.tabCustom) && (this.FunctionType == typeof(Steema.TeeChart.Functions.Custom))))
                {
                    if (this.bApply.Enabled)
                    {
                        this.bApply_Click(this.bApply, EventArgs.Empty);
                    }
                    this.bApply.Enabled = false;
                    if (this.series.Function != null)
                    {
                        this.custom = new CustomFunctionEditor(this.series.Function);
                        this.custom.controlToEnable = this.bApply;
                        EditorUtils.InsertForm(this.custom, this.tabCustom);
                        EditorUtils.Translate(this.tabCustom);
                    }
                }
                else if (((this.tabControl1.SelectedTab == this.tabCompress) && (this.FunctionType == typeof(CompressOHLC))) || (this.tabControl1.TabPages.Contains(this.tabCompress) && (this.FunctionType == typeof(CompressOHLC))))
                {
                    if (this.bApply.Enabled)
                    {
                        this.bApply_Click(this.bApply, EventArgs.Empty);
                    }
                    this.bApply.Enabled = false;
                    if (this.series.Function != null)
                    {
                        this.compressOHLC = new CompressOHLCFunctionEditor(this.series.Function);
                        this.compressOHLC.controlToEnable = this.bApply;
                        EditorUtils.InsertForm(this.compressOHLC, this.tabCompress);
                        EditorUtils.Translate(this.tabCompress);
                    }
                }
                else if (((this.tabControl1.SelectedTab == this.tabCLV) && (this.FunctionType == typeof(CLVFunction))) || (this.tabControl1.TabPages.Contains(this.tabCLV) && (this.FunctionType == typeof(CLVFunction))))
                {
                    if (this.bApply.Enabled)
                    {
                        this.bApply_Click(this.bApply, EventArgs.Empty);
                    }
                    this.bApply.Enabled = false;
                    if (this.series.Function != null)
                    {
                        this.clvFunction = new CLVFunctionEditor(this.series, this.select.FromList);
                        for (int i = 0; i < this.CBSingle.Items.Count; i++)
                        {
                            if (!(this.CBSingle.Items[i] is OHLC))
                            {
                                this.CBSingle.Items.Remove(this.CBSingle.Items[i]);
                            }
                        }
                        this.clvFunction.controlToEnable = this.bApply;
                        EditorUtils.InsertForm(this.clvFunction, this.tabCLV);
                        EditorUtils.Translate(this.tabCLV);
                    }
                }
                else if (((this.tabControl1.SelectedTab == this.tabOBV) && (this.FunctionType == typeof(OBVFunction))) || (this.tabControl1.TabPages.Contains(this.tabOBV) && (this.FunctionType == typeof(OBVFunction))))
                {
                    if (this.bApply.Enabled)
                    {
                        this.bApply_Click(this.bApply, EventArgs.Empty);
                    }
                    this.bApply.Enabled = false;
                    if (this.series.Function != null)
                    {
                        this.obvFunction = new OBVFunctionEditor(this.series, this.select.FromList);
                        for (int j = 0; j < this.CBSingle.Items.Count; j++)
                        {
                            if (!(this.CBSingle.Items[j] is OHLC))
                            {
                                this.CBSingle.Items.Remove(this.CBSingle.Items[j]);
                            }
                        }
                        this.obvFunction.controlToEnable = this.bApply;
                        EditorUtils.InsertForm(this.obvFunction, this.tabOBV);
                        EditorUtils.Translate(this.tabOBV);
                    }
                }
                else if (((this.tabControl1.SelectedTab == this.tabCCI) && (this.FunctionType == typeof(CCIFunction))) || (this.tabControl1.TabPages.Contains(this.tabCCI) && (this.FunctionType == typeof(CCIFunction))))
                {
                    if (this.bApply.Enabled)
                    {
                        this.bApply_Click(this.bApply, EventArgs.Empty);
                    }
                    this.bApply.Enabled = false;
                    if (this.series.Function != null)
                    {
                        this.cciFunction = new CCIFunctionEditor(this.series.Function);
                        this.cciFunction.controlToEnable = this.bApply;
                        EditorUtils.InsertForm(this.cciFunction, this.tabCCI);
                        EditorUtils.Translate(this.tabCCI);
                    }
                }
                else if (((this.tabControl1.SelectedTab == this.tabMovAvg) && (this.FunctionType == typeof(MovingAverage))) || (this.tabControl1.TabPages.Contains(this.tabMovAvg) && (this.FunctionType == typeof(MovingAverage))))
                {
                    if (this.bApply.Enabled)
                    {
                        this.bApply_Click(this.bApply, EventArgs.Empty);
                    }
                    this.bApply.Enabled = false;
                    if (this.series.Function != null)
                    {
                        this.movavgFunction = new MovingAverageFunctionEditor(this.series.Function);
                        this.movavgFunction.controlToEnable = this.bApply;
                        EditorUtils.InsertForm(this.movavgFunction, this.tabMovAvg);
                        EditorUtils.Translate(this.tabMovAvg);
                    }
                }
                else if (((this.tabControl1.SelectedTab == this.tabPVO) && (this.FunctionType == typeof(PVOFunction))) || (this.tabControl1.TabPages.Contains(this.tabPVO) && (this.FunctionType == typeof(PVOFunction))))
                {
                    if (this.bApply.Enabled)
                    {
                        this.bApply_Click(this.bApply, EventArgs.Empty);
                    }
                    this.bApply.Enabled = false;
                    if (this.series.Function != null)
                    {
                        this.pvoFunction = new PVOFunctionEditor(this.series.Function);
                        this.pvoFunction.controlToEnable = this.bApply;
                        EditorUtils.InsertForm(this.pvoFunction, this.tabPVO);
                        EditorUtils.Translate(this.tabPVO);
                    }
                }
                else if (((this.tabControl1.SelectedTab == this.tabBollinger) && (this.FunctionType == typeof(Bollinger))) || (this.tabControl1.TabPages.Contains(this.tabBollinger) && (this.FunctionType == typeof(Bollinger))))
                {
                    if (this.bApply.Enabled)
                    {
                        this.bApply_Click(this.bApply, EventArgs.Empty);
                    }
                    this.bApply.Enabled = false;
                    if (this.series.Function != null)
                    {
                        this.bollingerFunction = new BollingerFunctionEditor(this.series.Function);
                        this.bollingerFunction.controlToEnable = this.bApply;
                        EditorUtils.InsertForm(this.bollingerFunction, this.tabBollinger);
                        EditorUtils.Translate(this.tabBollinger);
                    }
                }
                else if (((this.tabControl1.SelectedTab == this.tabMACD) && (this.FunctionType == typeof(MACDFunction))) || (this.tabControl1.TabPages.Contains(this.tabMACD) && (this.FunctionType == typeof(MACDFunction))))
                {
                    if (this.bApply.Enabled)
                    {
                        this.bApply_Click(this.bApply, EventArgs.Empty);
                    }
                    this.bApply.Enabled = false;
                    if (this.series.Function != null)
                    {
                        this.macdFunction = new MACDFunctionEditor(this.series.Function);
                        this.macdFunction.controlToEnable = this.bApply;
                        EditorUtils.InsertForm(this.macdFunction, this.tabMACD);
                        EditorUtils.Translate(this.tabMACD);
                    }
                }
                else if (((this.tabControl1.SelectedTab == this.tabADX) && (this.FunctionType == typeof(ADXFunction))) || (this.tabControl1.TabPages.Contains(this.tabADX) && (this.FunctionType == typeof(ADXFunction))))
                {
                    if (this.bApply.Enabled)
                    {
                        this.bApply_Click(this.bApply, EventArgs.Empty);
                    }
                    this.bApply.Enabled = false;
                    if (this.series.Function != null)
                    {
                        this.adxFunction = new ADXFunctionEditor(this.series.Function);
                        this.adxFunction.controlToEnable = this.bApply;
                        EditorUtils.InsertForm(this.adxFunction, this.tabADX);
                        EditorUtils.Translate(this.tabADX);
                    }
                }
                else if (((this.tabControl1.SelectedTab == this.tabSmoothing) && (this.FunctionType == typeof(Smoothing))) || (this.tabControl1.TabPages.Contains(this.tabSmoothing) && (this.FunctionType == typeof(Smoothing))))
                {
                    if (this.bApply.Enabled)
                    {
                        this.bApply_Click(this.bApply, EventArgs.Empty);
                    }
                    this.bApply.Enabled = false;
                    if (this.series.Function != null)
                    {
                        this.smoothingFunction = new SmoothingFunctionEditor(this.series.Function);
                        this.smoothingFunction.controlToEnable = this.bApply;
                        EditorUtils.InsertForm(this.smoothingFunction, this.tabSmoothing);
                        EditorUtils.Translate(this.tabSmoothing);
                    }
                }
            }
        }

        private void TryCreateNewFunction()
        {
            Function function = this.FunctionClass();
            if (function != null)
            {
                if (this.series.Function == null)
                {
                    this.series.Function = function;
                    this.bApply.Enabled = false;
                }
                else if (function.GetType() != this.series.Function.GetType())
                {
                    this.series.Function.Dispose();
                    this.series.Function = null;
                    this.series.Function = function;
                    this.bApply.Enabled = false;
                }
                else
                {
                    this.bApply.Enabled = true;
                }
            }
        }

        private System.Type FunctionType
        {
            get
            {
                if (this.cbFunctions.SelectedIndex < 1)
                {
                    return null;
                }
                return (System.Type) Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1];
            }
        }
    }
}

