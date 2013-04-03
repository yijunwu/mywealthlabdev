namespace WealthLabPro
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class SymbolManagerForm : Form
    {
        private bool bool_0;
        private ToolStripButton btnDelete;
        private ToolStripButton btnFuturesMode;
        private ToolStripButton btnNew;
        private DataGridViewTextBoxColumn colDecimals;
        private DataGridViewTextBoxColumn colMargin;
        private DataGridViewTextBoxColumn colPointValue;
        private DataGridViewTextBoxColumn colSymbol;
        private DataGridViewTextBoxColumn colTick;
        private DataGridViewComboBoxColumn colType;
        private DataGridView dgSymbol;
        private IContainer icontainer_0;
        private StatusStrip status;
        private ToolStripStatusLabel statusSymbols;
        private string[] string_0 = Enum.GetNames(typeof(SecurityType));
        private static SymbolManagerForm symbolManagerForm_0;
        private ToolStrip toolbar;
        private ToolStripSeparator toolStripSeparator1;

        public SymbolManagerForm()
        {
            this.InitializeComponent();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dgSymbol.SelectedRows)
            {
                SymbolInfo tag = row.Tag as SymbolInfo;
                BarsLoader.SymbolInfo.Remove(tag);
                if (!row.IsNewRow)
                {
                    this.dgSymbol.Rows.Remove(row);
                }
            }
            BarsLoader.SaveSymbolInfo();
            this.method_1();
        }

        private void btnFuturesMode_Click(object sender, EventArgs e)
        {
            BarsLoader.FuturesMode = !BarsLoader.FuturesMode;
            this.method_2();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            SymbolInfo item = new SymbolInfo("Symbol", SecurityType.Future, 1000.0, 100.0, 0.01, 2);
            int num = this.dgSymbol.Rows.GetLastRow(DataGridViewElementStates.ReadOnly) - 1;
            if (num >= 0)
            {
                item = (SymbolInfo) this.dgSymbol.Rows[num].Tag;
            }
            BarsLoader.SymbolInfo.Add(item);
            int num2 = this.dgSymbol.Rows.Add(new object[] { item.Symbol, item.SecurityType.ToString(), item.Margin, item.PointValue, item.Tick, item.Decimals });
            this.dgSymbol.Rows[num2].Tag = item;
            this.method_1();
            BarsLoader.SaveSymbolInfo();
        }

        private void dgSymbol_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (this.bool_0)
            {
                return;
            }
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            string symbol = string.Empty;
            string s = string.Empty;
            bool flag = true;
            string str5 = string.Empty;
            double num4 = 0.0;
            if (this.dgSymbol[e.ColumnIndex, e.RowIndex].Value != null)
            {
                s = this.dgSymbol[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
            SymbolInfo tag = this.dgSymbol.Rows[rowIndex].Tag as SymbolInfo;
            if (tag == null)
            {
                return;
            }
            SymbolInfo item = new SymbolInfo(tag.Symbol, tag.SecurityType, tag.Margin, tag.PointValue, tag.Tick, tag.Decimals);
            BarsLoader.SymbolInfo.Remove(tag);
            string name = this.dgSymbol.Columns[columnIndex].Name;
            string headerText = this.dgSymbol.Columns[columnIndex].HeaderText;
            try
            {
                string str2 = name;
                if (str2 == null)
                {
                    goto Label_02D1;
                }
                if (str2 == "colSymbol")
                {
                    goto Label_0287;
                }
                if (str2 != "colType")
                {
                    if (str2 == "colMargin")
                    {
                        symbol = tag.Margin.ToString();
                        num4 = double.Parse(s);
                        if (num4 <= 0.0)
                        {
                            flag = false;
                            str5 = " value must be greater than zero.";
                        }
                        item.Margin = num4;
                    }
                    else if (str2 == "colPointValue")
                    {
                        symbol = tag.PointValue.ToString();
                        num4 = double.Parse(s);
                        if (num4 <= 0.0)
                        {
                            flag = false;
                            str5 = " must be greater than zero.";
                        }
                        item.PointValue = num4;
                    }
                    else if (str2 == "colTick")
                    {
                        symbol = tag.Tick.ToString();
                        item.Tick = double.Parse(s);
                    }
                    else if (str2 == "colDecimals")
                    {
                        symbol = tag.Decimals.ToString();
                        if (int.Parse(s) < 0)
                        {
                            flag = false;
                            str5 = " value must be greater than or equal to zero.";
                        }
                        item.Decimals = int.Parse(s);
                    }
                    goto Label_02D1;
                }
                string str6 = s;
                if (str6 != null)
                {
                    if (!(str6 == "Future"))
                    {
                        if (!(str6 == "MutualFund"))
                        {
                            goto Label_027E;
                        }
                        item.SecurityType = SecurityType.MutualFund;
                    }
                    else
                    {
                        item.SecurityType = SecurityType.Future;
                    }
                    goto Label_02D1;
                }
            Label_027E:
                item.SecurityType = SecurityType.Equity;
                goto Label_02D1;
            Label_0287:
                this.bool_0 = true;
                this.dgSymbol[e.ColumnIndex, e.RowIndex].Value = s;
                this.bool_0 = false;
                item.Symbol = s;
                symbol = tag.Symbol;
            }
            catch (Exception)
            {
                flag = false;
                str5 = " value must be numeric.";
            }
        Label_02D1:
            if (flag)
            {
                this.dgSymbol.Rows[rowIndex].Tag = item;
                BarsLoader.SymbolInfo.Add(item);
            }
            else
            {
                BarsLoader.SymbolInfo.Add(tag);
                this.dgSymbol[columnIndex, rowIndex].Value = symbol;
                MessageBox.Show(headerText + str5);
            }
            BarsLoader.SaveSymbolInfo();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SymbolManagerForm));
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            DataGridViewCellStyle style4 = new DataGridViewCellStyle();
            DataGridViewCellStyle style5 = new DataGridViewCellStyle();
            DataGridViewCellStyle style6 = new DataGridViewCellStyle();
            DataGridViewCellStyle style7 = new DataGridViewCellStyle();
            this.toolbar = new ToolStrip();
            this.btnNew = new ToolStripButton();
            this.btnDelete = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.btnFuturesMode = new ToolStripButton();
            this.status = new StatusStrip();
            this.statusSymbols = new ToolStripStatusLabel();
            this.dgSymbol = new DataGridView();
            this.colSymbol = new DataGridViewTextBoxColumn();
            this.colType = new DataGridViewComboBoxColumn();
            this.colMargin = new DataGridViewTextBoxColumn();
            this.colPointValue = new DataGridViewTextBoxColumn();
            this.colTick = new DataGridViewTextBoxColumn();
            this.colDecimals = new DataGridViewTextBoxColumn();
            this.toolbar.SuspendLayout();
            this.status.SuspendLayout();
            ((ISupportInitialize) this.dgSymbol).BeginInit();
            base.SuspendLayout();
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.btnNew, this.btnDelete, this.toolStripSeparator1, this.btnFuturesMode });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0x219, 0x19);
            this.toolbar.TabIndex = 1;
            this.toolbar.Text = "toolStrip1";
            this.btnNew.Image = (Image) manager.GetObject("btnNew.Image");
            this.btnNew.ImageTransparentColor = Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new Size(0x5e, 0x16);
            this.btnNew.Text = "New Symbol";
            this.btnNew.Click += new EventHandler(this.btnNew_Click);
            this.btnDelete.Image = (Image) manager.GetObject("btnDelete.Image");
            this.btnDelete.ImageTransparentColor = Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(0xa6, 0x16);
            this.btnDelete.Text = "Delete Selected Symbol(s)";
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.btnFuturesMode.Image = (Image) manager.GetObject("btnFuturesMode.Image");
            this.btnFuturesMode.ImageTransparentColor = Color.Magenta;
            this.btnFuturesMode.Name = "btnFuturesMode";
            this.btnFuturesMode.Size = new Size(0x92, 0x16);
            this.btnFuturesMode.Text = "Disable Futures Mode";
            this.btnFuturesMode.ToolTipText = "Use Futures Margin, Point Value, and Tick values during Backtesting";
            this.btnFuturesMode.Click += new EventHandler(this.btnFuturesMode_Click);
            this.status.Items.AddRange(new ToolStripItem[] { this.statusSymbols });
            this.status.Location = new Point(0, 0x1ad);
            this.status.Name = "status";
            this.status.Size = new Size(0x219, 0x16);
            this.status.TabIndex = 2;
            this.status.Text = "statusStrip1";
            this.statusSymbols.Name = "statusSymbols";
            this.statusSymbols.Size = new Size(0x6b, 0x11);
            this.statusSymbols.Text = "0 Symbols Defined";
            this.dgSymbol.BackgroundColor = SystemColors.ActiveCaptionText;
            this.dgSymbol.BorderStyle = BorderStyle.None;
            this.dgSymbol.CellBorderStyle = DataGridViewCellBorderStyle.None;
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;
            this.dgSymbol.ColumnHeadersDefaultCellStyle = style;
            this.dgSymbol.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSymbol.Columns.AddRange(new DataGridViewColumn[] { this.colSymbol, this.colType, this.colMargin, this.colPointValue, this.colTick, this.colDecimals });
            this.dgSymbol.Dock = DockStyle.Fill;
            this.dgSymbol.EditMode = DataGridViewEditMode.EditOnEnter;
            this.dgSymbol.Location = new Point(0, 0x19);
            this.dgSymbol.Name = "dgSymbol";
            this.dgSymbol.RowHeadersVisible = false;
            this.dgSymbol.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgSymbol.Size = new Size(0x219, 0x194);
            this.dgSymbol.TabIndex = 8;
            this.dgSymbol.CellValueChanged += new DataGridViewCellEventHandler(this.dgSymbol_CellValueChanged);
            style2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.colSymbol.DefaultCellStyle = style2;
            this.colSymbol.HeaderText = "Symbol";
            this.colSymbol.Name = "colSymbol";
            style3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.colType.DefaultCellStyle = style3;
            this.colType.DisplayStyleForCurrentCellOnly = true;
            this.colType.FlatStyle = FlatStyle.Flat;
            this.colType.HeaderText = "Type";
            this.colType.MaxDropDownItems = 3;
            this.colType.Name = "colType";
            this.colType.Resizable = DataGridViewTriState.True;
            this.colType.SortMode = DataGridViewColumnSortMode.Automatic;
            this.colType.Width = 80;
            style4.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.colMargin.DefaultCellStyle = style4;
            this.colMargin.HeaderText = "Margin";
            this.colMargin.Name = "colMargin";
            this.colMargin.Width = 70;
            style5.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.colPointValue.DefaultCellStyle = style5;
            this.colPointValue.HeaderText = "Point Value";
            this.colPointValue.Name = "colPointValue";
            style6.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.colTick.DefaultCellStyle = style6;
            this.colTick.HeaderText = "Tick";
            this.colTick.Name = "colTick";
            this.colTick.Width = 70;
            style7.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.colDecimals.DefaultCellStyle = style7;
            this.colDecimals.HeaderText = "Decimals";
            this.colDecimals.Name = "colDecimals";
            this.colDecimals.Width = 70;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x219, 0x1c3);
            base.Controls.Add(this.dgSymbol);
            base.Controls.Add(this.status);
            base.Controls.Add(this.toolbar);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "SymbolManagerForm";
            this.Text = "Symbol Info Manager";
            base.Load += new EventHandler(this.SymbolManagerForm_Load);
            base.FormClosed += new FormClosedEventHandler(this.SymbolManagerForm_FormClosed);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            ((ISupportInitialize) this.dgSymbol).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0()
        {
            foreach (string str in this.string_0)
            {
                this.colType.Items.Add(str);
            }
            foreach (DataGridViewRow row in (IEnumerable) this.dgSymbol.Rows)
            {
                if (row.IsNewRow)
                {
                    row.ReadOnly = true;
                }
            }
            int num2 = 0;
            foreach (SymbolInfo info in BarsLoader.SymbolInfo)
            {
                if (info != null)
                {
                    this.dgSymbol.Rows.Add(new object[] { info.Symbol, info.SecurityType.ToString(), info.Margin.ToString(), info.PointValue.ToString(), info.Tick.ToString(), info.Decimals.ToString() });
                    this.dgSymbol.Rows[num2].Tag = info;
                    num2++;
                }
            }
            this.dgSymbol.Sort(this.colSymbol, ListSortDirection.Ascending);
        }

        private void method_1()
        {
            this.statusSymbols.Text = (this.dgSymbol.RowCount - 1) + " Symbols defined";
        }

        private void method_2()
        {
            if (BarsLoader.FuturesMode)
            {
                this.btnFuturesMode.Text = "Disable Futures Mode";
            }
            else
            {
                this.btnFuturesMode.Text = "Enable Futures Mode";
            }
        }

        private void SymbolManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Instance = null;
        }

        private void SymbolManagerForm_Load(object sender, EventArgs e)
        {
            Instance = this;
            this.method_2();
            this.method_0();
            this.method_1();
        }

        public static SymbolManagerForm Instance
        {
            get
            {
                return symbolManagerForm_0;
            }
            set
            {
                symbolManagerForm_0 = value;
            }
        }
    }
}

