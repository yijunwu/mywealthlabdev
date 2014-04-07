namespace WealthLabPro
{
    using QWhale.Editor;
    using QWhale.Syntax;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxItem(false)]
    public class Editor : UserControl
    {
        private bool bool_0;
        private ToolStripButton btnCompile;
        private ToolStripButton btnDebug;
        private ToolStripButton btnEditorOptions;
        private ToolStripDropDownButton btnPrint;
        private ToolStripMenuItem btnPrintOptions;
        private ToolStripMenuItem btnPrintPreview;
        private ToolStripMenuItem btnPrintPrint;
        private ToolStripButton btnQuickRef;
        private ToolStripButton btnReferences;
        private ToolStripButton btnRun;
        private ChartForm chartForm_0;
        private Class60 class60_0;
        private CsParserWealthScript csParserWealthScript_0;
        private IContainer components;
        private ListBox lbResults;
        private Panel pnlBottom;
        private ToolStripSeparator sepCompile;
        private SyntaxEdit syntaxEdit;
        private TimeSpan timeSpan_0;
        private ToolStrip toolBar;
        private ToolStripLabel tslblExTimeValue;
        private WealthScriptCompiler wealthScriptCompiler_0;

        public Editor(ChartForm chartWindow)
        {
            this.InitializeComponent();
            this.chartForm_0 = chartWindow;
            this.class60_0 = new Class60(this.syntaxEdit, this.csParserWealthScript_0);
            if (chartWindow.Strategy != null)
            {
                this.syntaxEdit.Printing.Header.CenterText = "***** Wealth-Lab Strategy - " + chartWindow.Strategy.Name + " - " + DateTime.Now.ToShortDateString() + " *****";
                this.syntaxEdit.Printing.Header.FontColor = Color.Blue;
                this.syntaxEdit.Printing.Footer.FontColor = Color.Blue;
            }
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            this.bool_0 = false;
            this.lbResults.Items.Clear();
            List<StrategyParameter> list = new List<StrategyParameter>();
            if (this.chartForm_0.WealthScript != null)
            {
                foreach (StrategyParameter parameter in this.chartForm_0.WealthScript.Parameters)
                {
                    list.Add(parameter);
                }
            }
            this.wealthScriptCompiler_0.SourceCode = this.syntaxEdit.Text;
            string references = (this.chartForm_0.Strategy == null) ? "" : this.chartForm_0.Strategy.References;
            WealthScript script = this.wealthScriptCompiler_0.CompileSource(references);
            if (script != null)
            {
                this.lbResults.Items.Add("Strategy \"" + script.GetType().Name + "\" compiled successfully!");
            }
            else
            {
                foreach (CompilerError error in this.wealthScriptCompiler_0.CompilerErrors)
                {
                    this.lbResults.Items.Add(error);
                    this.bool_0 = true;
                }
            }
            this.chartForm_0.WealthScript = script;
            MainModule.Instance.Strategies.LoadStrategyParameters(this.chartForm_0.Strategy, script);
            if (script != null)
            {
                foreach (StrategyParameter parameter3 in list)
                {
                    foreach (StrategyParameter parameter2 in script.Parameters)
                    {
                        if (parameter3.Name == parameter2.Name)
                        {
                            parameter2.Value = parameter3.Value;
                        }
                    }
                }
            }
            this.chartForm_0.MyMainForm.BuildParameterSliders();
            if (this.chartForm_0.Optimization != null)
            {
                this.chartForm_0.Optimization.LoadParameterList();
            }
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            if (DebugForm.Instance == null)
            {
                DebugForm.Instance = new DebugForm();
                DebugForm.Instance.Show();
            }
            DebugForm.Instance.BringToFront();
        }

        private void btnEditorOptions_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.syntaxEdit.DisplayEditorSettingsDialog())
            {
                this.class60_0.method_3();
            }
        }

        private void btnPrintOptions_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.syntaxEdit.Printing.ExecutePrintOptionsDialog())
            {
                this.syntaxEdit.Printing.Options = this.syntaxEdit.Printing.PrintOptionsDialog.Options;
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            this.syntaxEdit.Printing.ExecutePrintPreviewDialog();
        }

        private void btnPrintPrint_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.syntaxEdit.Printing.ExecutePrintDialog())
            {
                this.syntaxEdit.Printing.Print();
            }
        }

        private void btnQuickRef_Click(object sender, EventArgs e)
        {
            this.method_0();
        }

        private void btnReferences_Click(object sender, EventArgs e)
        {
            ReferencesForm form = new ReferencesForm(this.chartForm_0.Strategy.References);
            if ((form.ShowDialog(this) == DialogResult.OK) && (this.chartForm_0.Strategy.References != form.References))
            {
                this.chartForm_0.Strategy.References = form.References;
                this.chartForm_0.NeedSave = true;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            this.btnCompile.PerformClick();
            if (!this.bool_0)
            {
                this.chartForm_0.GoButtonPressed(this.chartForm_0.MyMainForm.Symbol, true);
                this.chartForm_0.SelectTab("Chart");
            }
        }

        public void ClearErrors()
        {
            this.lbResults.Items.Clear();
        }

        public void Compile()
        {
            this.btnCompile.PerformClick();
        }

        public void DisplayRuntimeError(Exception exception_0)
        {
            this.lbResults.Items.Clear();
            this.lbResults.ForeColor = Color.Red;
            if (exception_0.InnerException != null)
            {
                exception_0 = exception_0.InnerException;
            }
            this.lbResults.Items.Add("Runtime error: " + exception_0.Message);
            string[] strArray = exception_0.StackTrace.Split(new char[] { '\r', '\n' });
            for (int i = strArray.Length - 1; i >= 0; i--)
            {
                string item = strArray[i];
                if (!item.Contains("TradingSystemExecutor") && (item.Trim() != ""))
                {
                    this.lbResults.Items.Add(item);
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

        public void EditCopy()
        {
            this.syntaxEdit.Selection.Copy();
        }

        public void EditCut()
        {
            this.syntaxEdit.Selection.Cut();
        }

        public void EditDelete()
        {
            this.syntaxEdit.Selection.DeleteRight();
        }

        public void EditFind()
        {
            this.syntaxEdit.DisplaySearchDialog();
        }

        public void EditFindReplace()
        {
            this.syntaxEdit.DisplayReplaceDialog();
        }

        public void EditPaste()
        {
            this.syntaxEdit.Selection.Paste();
        }

        public void EditUndo()
        {
            this.syntaxEdit.Source.Undo();
        }

        public void EnableControls(bool enable)
        {
            this.toolBar.Enabled = enable;
            this.syntaxEdit.Enabled = enable;
        }

        public void FocusEditor()
        {
            this.syntaxEdit.Focus();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Editor));
            this.toolBar = new ToolStrip();
            this.btnRun = new ToolStripButton();
            this.btnCompile = new ToolStripButton();
            this.sepCompile = new ToolStripSeparator();
            this.tslblExTimeValue = new ToolStripLabel();
            this.btnQuickRef = new ToolStripButton();
            this.btnDebug = new ToolStripButton();
            this.btnEditorOptions = new ToolStripButton();
            this.btnPrint = new ToolStripDropDownButton();
            this.btnPrintPrint = new ToolStripMenuItem();
            this.btnPrintPreview = new ToolStripMenuItem();
            this.btnPrintOptions = new ToolStripMenuItem();
            this.pnlBottom = new Panel();
            this.lbResults = new ListBox();
            this.wealthScriptCompiler_0 = new WealthScriptCompiler(this.components);
            this.syntaxEdit = new SyntaxEdit(this.components);
            this.csParserWealthScript_0 = new CsParserWealthScript();
            this.btnReferences = new ToolStripButton();
            this.toolBar.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            base.SuspendLayout();
            this.toolBar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolBar.Items.AddRange(new ToolStripItem[] { this.btnRun, this.btnCompile, this.sepCompile, this.tslblExTimeValue, this.btnQuickRef, this.btnDebug, this.btnEditorOptions, this.btnReferences, this.btnPrint });
            this.toolBar.Location = new Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new Size(0x273, 0x19);
            this.toolBar.TabIndex = 0;
            this.toolBar.Text = "toolStrip1";
            this.btnRun.Image = (Image) resources.GetObject("btnRun.Image");
            this.btnRun.ImageTransparentColor = Color.Magenta;
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new Size(110, 0x16);
            this.btnRun.Text = "Run the Strategy";
            this.btnRun.ToolTipText = "Compile and Run the Strategy";
            this.btnRun.Click += new EventHandler(this.btnRun_Click);
            this.btnCompile.Image = (Image) resources.GetObject("btnCompile.Image");
            this.btnCompile.ImageTransparentColor = Color.White;
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new Size(0x40, 0x16);
            this.btnCompile.Text = "Compile";
            this.btnCompile.ToolTipText = "Compile the Source Code and check for errors";
            this.btnCompile.Click += new EventHandler(this.btnCompile_Click);
            this.sepCompile.Name = "sepCompile";
            this.sepCompile.Size = new Size(6, 0x19);
            this.tslblExTimeValue.ForeColor = SystemColors.ActiveCaption;
            this.tslblExTimeValue.Name = "tslblExTimeValue";
            this.tslblExTimeValue.Size = new Size(0, 0x16);
            this.tslblExTimeValue.ToolTipText = "Time it took last Strategy to Execute";
            this.btnQuickRef.Alignment = ToolStripItemAlignment.Right;
            this.btnQuickRef.Image = (Image) resources.GetObject("btnQuickRef.Image");
            this.btnQuickRef.ImageTransparentColor = Color.Magenta;
            this.btnQuickRef.Name = "btnQuickRef";
            this.btnQuickRef.Size = new Size(70, 0x16);
            this.btnQuickRef.Text = "QuickRef";
            this.btnQuickRef.ToolTipText = "View the WealthScript QuickRef";
            this.btnQuickRef.Click += new EventHandler(this.btnQuickRef_Click);
            this.btnDebug.Alignment = ToolStripItemAlignment.Right;
            this.btnDebug.Image = (Image) resources.GetObject("btnDebug.Image");
            this.btnDebug.ImageTransparentColor = Color.Magenta;
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new Size(0x4e, 0x16);
            this.btnDebug.Text = "Debug Log";
            this.btnDebug.ToolTipText = "View the Debug and Error Message Log Window";
            this.btnDebug.Click += new EventHandler(this.btnDebug_Click);
            this.btnEditorOptions.Alignment = ToolStripItemAlignment.Right;
            this.btnEditorOptions.Image = (Image) resources.GetObject("btnEditorOptions.Image");
            this.btnEditorOptions.ImageTransparentColor = Color.Magenta;
            this.btnEditorOptions.Name = "btnEditorOptions";
            this.btnEditorOptions.Size = new Size(0x4c, 0x16);
            this.btnEditorOptions.Text = "Options...";
            this.btnEditorOptions.ToolTipText = "View WealthScript Editor Options";
            this.btnEditorOptions.Click += new EventHandler(this.btnEditorOptions_Click);
            this.btnPrint.Alignment = ToolStripItemAlignment.Right;
            this.btnPrint.DropDownItems.AddRange(new ToolStripItem[] { this.btnPrintPrint, this.btnPrintPreview, this.btnPrintOptions });
            this.btnPrint.Image = (Image) resources.GetObject("btnPrint.Image");
            this.btnPrint.ImageTransparentColor = Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x3a, 0x16);
            this.btnPrint.Text = "Print";
            this.btnPrintPrint.Name = "btnPrintPrint";
            this.btnPrintPrint.Size = new Size(0x98, 0x16);
            this.btnPrintPrint.Text = "Print...";
            this.btnPrintPrint.Click += new EventHandler(this.btnPrintPrint_Click);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new Size(0x98, 0x16);
            this.btnPrintPreview.Text = "Preview...";
            this.btnPrintPreview.Click += new EventHandler(this.btnPrintPreview_Click);
            this.btnPrintOptions.Name = "btnPrintOptions";
            this.btnPrintOptions.Size = new Size(0x98, 0x16);
            this.btnPrintOptions.Text = "Options...";
            this.btnPrintOptions.Click += new EventHandler(this.btnPrintOptions_Click);
            this.pnlBottom.Controls.Add(this.lbResults);
            this.pnlBottom.Dock = DockStyle.Bottom;
            this.pnlBottom.Location = new Point(0, 0x18e);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new Size(0x273, 0x3b);
            this.pnlBottom.TabIndex = 1;
            this.lbResults.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lbResults.DrawMode = DrawMode.OwnerDrawFixed;
            this.lbResults.ForeColor = Color.Green;
            this.lbResults.FormattingEnabled = true;
            this.lbResults.Items.AddRange(new object[] { "Compiler results" });
            this.lbResults.Location = new Point(3, 3);
            this.lbResults.Name = "lbResults";
            this.lbResults.Size = new Size(0x26d, 0x38);
            this.lbResults.TabIndex = 0;
            this.lbResults.DrawItem += new DrawItemEventHandler(this.lbResults_DrawItem);
            this.lbResults.SelectedIndexChanged += new EventHandler(this.lbResults_SelectedIndexChanged);
            this.wealthScriptCompiler_0.SourceCode = null;
            this.syntaxEdit.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.syntaxEdit.BackColor = SystemColors.Window;
            this.syntaxEdit.Cursor = Cursors.IBeam;
            this.syntaxEdit.Font = new Font("Courier New", 10f);
            this.syntaxEdit.Gutter.Options = GutterOptions.PaintLineModificators | GutterOptions.PaintBookMarks | GutterOptions.PaintLineNumbers;
            this.syntaxEdit.Lexer = this.csParserWealthScript_0;
            this.syntaxEdit.Location = new Point(0, 0x1c);
            this.syntaxEdit.Name = "syntaxEdit";
            this.syntaxEdit.Outlining.AllowOutlining = true;
            this.syntaxEdit.Size = new Size(0x270, 0x16f);
            this.syntaxEdit.TabIndex = 3;
            this.syntaxEdit.Text = "";
            this.syntaxEdit.TextChanged += new EventHandler(this.syntaxEdit_TextChanged);
            this.syntaxEdit.HelpRequested += new HelpEventHandler(this.syntaxEdit_HelpRequested);
            this.csParserWealthScript_0.DefaultState = 0;
            this.csParserWealthScript_0.Options = SyntaxOptions.SyntaxErrors | SyntaxOptions.CodeCompletion | SyntaxOptions.SmartIndent | SyntaxOptions.Outline;
            this.csParserWealthScript_0.XmlScheme = resources.GetString("csParserWealthScript.XmlScheme");
            this.btnReferences.Alignment = ToolStripItemAlignment.Right;
            this.btnReferences.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnReferences.Image = (Image) resources.GetObject("btnReferences.Image");
            this.btnReferences.ImageTransparentColor = Color.Magenta;
            this.btnReferences.Name = "btnReferences";
            this.btnReferences.Size = new Size(0x51, 0x16);
            this.btnReferences.Text = "References ...";
            this.btnReferences.ToolTipText = "Specify .NET Assembly References for this Strategy";
            this.btnReferences.Click += new EventHandler(this.btnReferences_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Controls.Add(this.syntaxEdit);
            base.Controls.Add(this.pnlBottom);
            base.Controls.Add(this.toolBar);
            base.Name = "Editor";
            base.Size = new Size(0x273, 0x1c9);
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lbResults_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {
                Brush green;
                string str;
                e.DrawBackground();
                if (this.lbResults.Items[e.Index] is CompilerError)
                {
                    CompilerError error = (CompilerError) this.lbResults.Items[e.Index];
                    green = error.IsWarning ? Brushes.Blue : Brushes.Red;
                    str = string.Concat(new object[] { error.IsWarning ? "warning" : "error", " ", error.ErrorNumber, " @ (", error.Line, ", ", error.Column, ") : ", error.ErrorText });
                }
                else
                {
                    green = Brushes.Green;
                    str = this.lbResults.Items[e.Index].ToString();
                }
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    green = SystemBrushes.HighlightText;
                }
                e.Graphics.DrawString(str, e.Font, green, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
        }

        private void lbResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = this.lbResults.SelectedIndex;
            if ((-1 != selectedIndex) && (this.lbResults.Items[selectedIndex] is CompilerError))
            {
                CompilerError error = (CompilerError) this.lbResults.Items[selectedIndex];
                this.syntaxEdit.MoveTo(error.Column - 1, error.Line - 1);
                this.syntaxEdit.Selection.SelectWordRight();
                this.syntaxEdit.Focus();
            }
        }

        private void method_0()
        {
            QuickRefForm instance = QuickRefForm.Instance;
            if (instance == null)
            {
                instance = new QuickRefForm();
                instance.Show();
            }
            else
            {
                instance.BringToFront();
            }
            string textToSearchAtCursor = this.syntaxEdit.GetTextToSearchAtCursor();
            if (0 < textToSearchAtCursor.Length)
            {
                instance.ShowTopicByName(textToSearchAtCursor);
            }
        }

        internal void method_1(WealthScript wealthScript_0)
        {
            bool flag;
            int num6;
            List<string> list = new List<string>();
            foreach (string str3 in this.syntaxEdit.Lines)
            {
                list.Add(str3);
            }
            int index = -1;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                string str2 = list[i];
                if (!str2.ToUpper().Contains("CREATEPARAMETER(") && !str2.ToUpper().Contains("CREATEPARAMETER ("))
                {
                    if (str2.Trim().StartsWith("StrategyParameter ") || str2.Trim().StartsWith("private StrategyParameter"))
                    {
                        list.RemoveAt(i);
                        index--;
                    }
                }
                else
                {
                    int length = str2.IndexOf('"');
                    string name = str2.Substring(length + 1);
                    length = name.IndexOf('"');
                    name = name.Substring(0, length);
                    StrategyParameter parameter2 = wealthScript_0.FindParameter(name);
                    if (parameter2 != null)
                    {
                        string str5 = str2.Trim();
                        length = str5.IndexOf(' ');
                        str5 = str5.Substring(0, length);
                        parameter2.VariableName = str5;
                    }
                    index = i;
                    list.RemoveAt(i);
                }
            }
            string str7 = "MyStrategy";
            if (flag = index == -1)
            {
                int num4 = 0;
                using (List<string>.Enumerator enumerator4 = list.GetEnumerator())
                {
                    string current;
                    while (enumerator4.MoveNext())
                    {
                        current = enumerator4.Current;
                        if (current.Trim().StartsWith("public class"))
                        {
                            ///goto  Label_0175; ///WYJ fix, simplify the flow 
                            str7 = current.Trim().Split(new char[] { ' ' })[2];
                            index = num4;
                            break;
                        }
                        num4++;
                    }
                }
            }
            num6 = 0;
            int num3 = flag ? (index + 2) : (index - 2);
            foreach (StrategyParameter parameter3 in wealthScript_0.Parameters)
            {
                num6++;
                if ((parameter3.VariableName == null) || (parameter3.VariableName == ""))
                {
                    parameter3.VariableName = "strategyParameter" + num6;
                }
                string item = "\t\tStrategyParameter " + parameter3.VariableName + ";";
                list.Insert(num3, item);
                num3++;
                index++;
            }
            if (flag)
            {
                list.Insert(num3++, "");
                index++;
            }
            if (flag)
            {
                index += 2;
                list.Insert(index, "\t\tpublic " + str7 + "()");
                index++;
                list.Insert(index, "\t\t{");
                index++;
            }
            foreach (StrategyParameter parameter in wealthScript_0.Parameters)
            {
                string str = "\t\t\t" + parameter.VariableName + " = CreateParameter(";
                parameter.Name = parameter.NameEdited;
                str = ((((str + "\"" + parameter.Name + "\", ") + parameter.DefaultValue + ", ") + parameter.Start + ", ") + parameter.Stop + ", ") + parameter.Step + ");";
                list.Insert(index, str);
                index++;
            }
            if (flag)
            {
                list.Insert(index, "\t\t}");
                index++;
                list.Insert(index, "");
            }
            this.syntaxEdit.Lines.Clear();
            foreach (string str9 in list)
            {
                this.syntaxEdit.Lines.Add(str9);
            }
        }

        public void SelectAll()
        {
            this.syntaxEdit.Selection.SelectAll();
        }

        private void syntaxEdit_HelpRequested(object sender, HelpEventArgs e)
        {
            this.method_0();
        }

        private void syntaxEdit_TextChanged(object sender, EventArgs e)
        {
            this.chartForm_0.NeedSave = true;
        }

        public void UpdateExecutionTime()
        {
            this.tslblExTimeValue.Text = this.timeSpan_0.TotalMilliseconds.ToString("N0") + " ms";
        }

        public string Code
        {
            get
            {
                return this.syntaxEdit.Text;
            }
            set
            {
                this.syntaxEdit.Text = value;
            }
        }

        public TimeSpan ExecutionTime
        {
            get
            {
                return this.timeSpan_0;
            }
            set
            {
                this.timeSpan_0 = value;
            }
        }
    }
}

