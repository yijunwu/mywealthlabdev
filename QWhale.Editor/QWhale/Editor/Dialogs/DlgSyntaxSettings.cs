namespace QWhale.Editor.Dialogs
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax.Lexer;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgSyntaxSettings : Form, IEditorSettingsDialog
    {
        private TreeNode additionalNode;
        private Button btAddColorTheme;
        public Button btCancel;
        private Button btDeleteColorTheme;
        private Button btDeleteScheme;
        public Button btOK;
        private Button btSaveSchemeAs;
        public ColorBox cbBackColor;
        private ComboBox cbColorThemes;
        public ComboBox cbFontName;
        public ColorBox cbForeColor;
        private ComboBox cbKeyboardSchemes;
        private ComboBox cbShortcuts;
        public CheckBox chbAllowOutlining;
        public CheckBox chbBeyondEof;
        public CheckBox chbBeyondEol;
        public CheckBox chbBold;
        public CheckBox chbDragAndDrop;
        private CheckBox chbForced;
        public CheckBox chbHighlightUrls;
        public CheckBox chbHorzScrollBar;
        public CheckBox chbItalic;
        public CheckBox chbLineModificator;
        public CheckBox chbLineNumbers;
        public CheckBox chbLineNumbersOnGutter;
        private CheckBox chbLineSeparator;
        public CheckBox chbMoveOnRightButton;
        public CheckBox chbShowGutter;
        public CheckBox chbShowHints;
        public CheckBox chbShowMargin;
        public CheckBox chbUnderline;
        public CheckBox chbVertScrollBar;
        public CheckBox chbWhiteSpace;
        public CheckBox chbWordWrap;
        private const int closeFolderImage = 1;
        public ColorDialog colorDialog1;
        private IContainer components;
        private Color curBkColor;
        private string curDesc;
        private FontStyle curFontStyle;
        private Color curForeColor;
        private TreeNode fontsNode;
        private GroupBox gbColorThemes;
        public GroupBox gbDocument;
        public GroupBox gbFontAttributes;
        public GroupBox gbGutterMargin;
        public GroupBox gbLineNumbers;
        public GroupBox gbNavigateOptions;
        public GroupBox gbOutlineOptions;
        public GroupBox gbTabOptions;
        private TreeNode generalNode;
        public ImageList imageList1;
        private bool isControlUpdating;
        private bool isFontControlsUpdating;
        private TreeNode keyboardNode;
        public Label laBackColor;
        public Label laDescription;
        public Label laDisplayItems;
        public Label laFont;
        public Label laForeColor;
        public Label laGutterWidth;
        private Label laKeyboardMappingScheme;
        public Label laMarginPosition;
        public Label laSample;
        public Label laSampleText;
        private Label laShortcuts;
        private Label laShowCommands;
        public Label laSize;
        public Label laTabSizes;
        private ListBox lbEventHandlers;
        public ListBox lbStyles;
        private const int openFolderImage = 0;
        public Panel pnAdditional;
        public Panel pnButtons;
        public Panel pnFontsColors;
        public Panel pnGeneral;
        public Panel pnKeyboard;
        public Panel pnMain;
        public Panel pnManage;
        public Panel pnSampleText;
        public Panel pnTree;
        public RadioButton rbInsertSpaces;
        public RadioButton rbKeepTabs;
        private TreeNode rootNode;
        private string sAlt;
        private string sControl;
        private string sCtrl;
        private const int selectedImage = 2;
        private string sShift;
        private ISyntaxSettings syntaxSettings;
        public TextBox tbDescription;
        public TextBox tbFontSize;
        public TextBox tbGutterWidth;
        public TextBox tbMarginPosition;
        private TextBox tbShowCommands;
        public TextBox tbTabStops;
        public TabControl tcMain;
        public TabPage tpAdditional;
        public TabPage tpFontsAndColors;
        public TabPage tpGeneral;
        public TabPage tpKeyboard;
        public TreeView tvProperties;
        private const int unSelectedImage = 3;

        public DlgSyntaxSettings()
        {
            this.sControl = "control";
            this.sCtrl = "CTRL";
            this.sAlt = "alt";
            this.sShift = "shift";
            this.InitializeComponent();
            this.syntaxSettings = new QWhale.Editor.Dialogs.SyntaxSettings();
            this.rootNode = this.tvProperties.Nodes[0];
            this.generalNode = this.rootNode.Nodes[0];
            this.fontsNode = this.rootNode.Nodes[1];
            this.additionalNode = this.rootNode.Nodes[2];
            this.keyboardNode = this.rootNode.Nodes[3];
        }

        public DlgSyntaxSettings(EditorSettingsTab hiddenTabs) : this()
        {
            this.UpdateHiddenTabs(hiddenTabs);
        }

        private string ApplyKeyState(IKeyData key)
        {
            string str = string.Empty;
            if (key.State > 0)
            {
                foreach (IKeyData data in this.syntaxSettings.EventData)
                {
                    if ((data.LeaveState == key.State) && (data.State == 0))
                    {
                        return this.KeyDataToString(data.Keys);
                    }
                }
            }
            return str;
        }

        private void btAddColorTheme_Click(object sender, EventArgs e)
        {
            IColorThemes colorThemes = this.syntaxSettings.ColorThemes;
            ColorTheme item = new ColorTheme();
            ISerializationInfo serializationInfo = colorThemes.ActiveTheme.SerializationInfo;
            serializationInfo.Load();
            item.SerializationInfo = serializationInfo;
            colorThemes.Add(item);
            int num = colorThemes.Count - 1;
            string name = colorThemes[num].Name;
            colorThemes[num].Name = "Copy of " + name;
            colorThemes[num].Readonly = false;
            this.FillColorThemes();
            colorThemes.ActiveThemeIndex = num;
            this.cbColorThemes.SelectedIndex = num;
        }

        private void btDeleteColorTheme_Click(object sender, EventArgs e)
        {
            IColorThemes colorThemes = this.syntaxSettings.ColorThemes;
            if ((colorThemes.ActiveThemeIndex != -1) && !colorThemes[colorThemes.ActiveThemeIndex].Readonly)
            {
                colorThemes.RemoveAt(colorThemes.ActiveThemeIndex);
                colorThemes.ActiveThemeIndex--;
                this.FillColorThemes();
                this.cbColorThemes.SelectedIndex = colorThemes.ActiveThemeIndex;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            this.SettingsFromControl();
        }

        private void cbBackColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.isControlUpdating)
            {
                this.curBkColor = this.cbBackColor.SelectedColor;
                this.StyleFromControl();
            }
        }

        private void cbColorThemes_Leave(object sender, EventArgs e)
        {
            if (((this.cbColorThemes.SelectedIndex == -1) && !this.syntaxSettings.ColorThemes.ActiveTheme.Readonly) && (this.cbColorThemes.Text != string.Empty))
            {
                this.syntaxSettings.ColorThemes.ActiveTheme.Name = this.cbColorThemes.Text;
                this.FillColorThemes();
            }
        }

        private void cbColorThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.cbColorThemes.SelectedIndex >= 0) && (this.cbColorThemes.SelectedIndex < this.syntaxSettings.ColorThemes.Count))
            {
                this.syntaxSettings.ColorThemes.ActiveThemeIndex = this.cbColorThemes.SelectedIndex;
                this.btDeleteColorTheme.Enabled = !this.syntaxSettings.ColorThemes.ActiveTheme.Readonly;
                this.UpdateFontControls();
                this.FillStyles();
                this.StyleSelected();
            }
        }

        private void cbFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateActiveColorThemeFont();
        }

        private void cbForeColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.isControlUpdating)
            {
                this.curForeColor = this.cbForeColor.SelectedColor;
                this.StyleFromControl();
            }
        }

        private void ControlsFromSettings()
        {
            this.FillStyles();
            this.FillColorThemes();
            this.cbFontName.Items.AddRange(this.GetFonts());
            this.cbKeyboardSchemes.Items.Clear();
            this.cbKeyboardSchemes.Items.Add("Default Settings");
            this.cbKeyboardSchemes.SelectedIndex = 0;
            this.FillEventHandlers();
            this.chbDragAndDrop.Checked = (this.syntaxSettings.SelectionOptions & SelectionOptions.DisableDragging) == SelectionOptions.None;
            switch (this.syntaxSettings.ScrollBars)
            {
                case RichTextBoxScrollBars.None:
                    this.chbVertScrollBar.Checked = false;
                    this.chbHorzScrollBar.Checked = false;
                    this.chbForced.Checked = false;
                    break;

                case RichTextBoxScrollBars.Horizontal:
                case RichTextBoxScrollBars.ForcedHorizontal:
                    this.chbVertScrollBar.Checked = false;
                    this.chbHorzScrollBar.Checked = true;
                    this.chbForced.Checked = false;
                    break;

                case RichTextBoxScrollBars.Vertical:
                case RichTextBoxScrollBars.ForcedVertical:
                    this.chbVertScrollBar.Checked = true;
                    this.chbHorzScrollBar.Checked = false;
                    this.chbForced.Checked = false;
                    break;

                case RichTextBoxScrollBars.Both:
                    this.chbVertScrollBar.Checked = true;
                    this.chbHorzScrollBar.Checked = true;
                    this.chbForced.Checked = false;
                    break;

                case RichTextBoxScrollBars.ForcedBoth:
                    this.chbVertScrollBar.Checked = true;
                    this.chbHorzScrollBar.Checked = true;
                    this.chbForced.Checked = true;
                    break;
            }
            this.chbShowMargin.Checked = this.syntaxSettings.ShowMargin;
            this.chbWordWrap.Checked = this.syntaxSettings.WordWrap;
            this.chbLineNumbers.Checked = (this.syntaxSettings.GutterOptions & GutterOptions.PaintLineNumbers) != GutterOptions.None;
            this.chbLineNumbersOnGutter.Checked = (this.syntaxSettings.GutterOptions & GutterOptions.PaintLinesOnGutter) != GutterOptions.None;
            this.chbShowGutter.Checked = this.syntaxSettings.ShowGutter;
            this.tbGutterWidth.Text = this.syntaxSettings.GutterWidth.ToString();
            this.tbMarginPosition.Text = this.syntaxSettings.MarginPos.ToString();
            this.chbBeyondEol.Checked = (this.syntaxSettings.NavigateOptions & NavigateOptions.BeyondEol) != NavigateOptions.None;
            this.chbBeyondEof.Checked = (this.syntaxSettings.NavigateOptions & NavigateOptions.BeyondEof) != NavigateOptions.None;
            this.chbMoveOnRightButton.Checked = (this.syntaxSettings.NavigateOptions & NavigateOptions.MoveOnRightButton) != NavigateOptions.None;
            this.chbHighlightUrls.Checked = this.syntaxSettings.HighlightHyperText;
            this.chbAllowOutlining.Checked = this.syntaxSettings.AllowOutlining;
            this.chbShowHints.Checked = (this.syntaxSettings.OutlineOptions & OutlineOptions.ShowHints) != OutlineOptions.None;
            this.rbInsertSpaces.Checked = this.syntaxSettings.UseSpaces;
            this.rbKeepTabs.Checked = !this.syntaxSettings.UseSpaces;
            this.chbWhiteSpace.Checked = this.syntaxSettings.WhiteSpaceVisible;
            this.chbLineModificator.Checked = (this.syntaxSettings.GutterOptions & GutterOptions.PaintLineModificators) != GutterOptions.None;
            this.chbLineSeparator.Checked = (this.syntaxSettings.SeparatorOptions & SeparatorOptions.SeparateLines) != SeparatorOptions.None;
            string[] strArray = new string[this.syntaxSettings.TabStops.Length];
            for (int i = 0; i < this.syntaxSettings.TabStops.Length; i++)
            {
                strArray[i] = this.syntaxSettings.TabStops[i].ToString();
            }
            this.tbTabStops.Text = string.Join(",", strArray);
            this.UpdateFontControls();
        }

        private void DescriptionChanged(object sender, EventArgs e)
        {
            if (!this.isControlUpdating)
            {
                this.curDesc = this.tbDescription.Text;
                this.StyleFromControl();
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

        private void DlgSyntaxSettings_Activated(object sender, EventArgs e)
        {
            this.tvProperties.Focus();
        }

        private void DlgSyntaxSettings_Load(object sender, EventArgs e)
        {
            this.LoadFromResource();
            this.ControlsFromSettings();
            this.lbStyles.SelectedIndexChanged += new EventHandler(this.OnStyleSelected);
            this.lbStyles.SelectedIndex = 0;
            this.chbBold.CheckedChanged += new EventHandler(this.FontStyleChange);
            this.chbItalic.CheckedChanged += new EventHandler(this.FontStyleChange);
            this.chbUnderline.CheckedChanged += new EventHandler(this.FontStyleChange);
            this.tbDescription.TextChanged += new EventHandler(this.DescriptionChanged);
            this.cbFontName.SelectedIndexChanged += new EventHandler(this.FontNameChanged);
            this.tbFontSize.TextChanged += new EventHandler(this.FontSizeChanged);
            this.pnManage.Controls.Add(this.pnGeneral);
            this.pnManage.Controls.Add(this.pnFontsColors);
            this.pnManage.Controls.Add(this.pnAdditional);
            this.pnManage.Controls.Add(this.pnKeyboard);
            this.tvProperties.Nodes[0].ImageIndex = 0;
            this.tvProperties.Nodes[0].Expand();
            this.tvProperties.SelectedNode = (this.tvProperties.Nodes[0].Nodes.Count > 0) ? this.tvProperties.Nodes[0].Nodes[0] : this.tvProperties.Nodes[0];
        }

        public DialogResult Execute(EditorSettingsTab hiddenTabs)
        {
            return this.Execute(hiddenTabs, null);
        }

        public DialogResult Execute(EditorSettingsTab hiddenTabs, IWin32Window owner)
        {
            this.UpdateHiddenTabs(hiddenTabs);
            if (owner == null)
            {
                return base.ShowDialog();
            }
            return base.ShowDialog(owner);
        }

        private void FillColorThemes()
        {
            this.cbColorThemes.BeginUpdate();
            this.cbColorThemes.Items.Clear();
            foreach (IColorTheme theme in this.SyntaxSettings.ColorThemes)
            {
                this.cbColorThemes.Items.Add(theme.Name);
            }
            this.cbColorThemes.SelectedIndex = this.SyntaxSettings.ColorThemes.ActiveThemeIndex;
            this.cbColorThemes.EndUpdate();
        }

        private void FillEventHandlers()
        {
            this.UpdateEventHandlers();
        }

        private void FillStyles()
        {
            this.lbStyles.Items.Clear();
            for (int i = 0; i < this.syntaxSettings.LexStyles.Count; i++)
            {
                this.lbStyles.Items.Add(this.syntaxSettings.LexStyles[i].Desc);
            }
        }

        private void FontNameChanged(object sender, EventArgs e)
        {
            if (!this.isControlUpdating && (this.cbFontName.SelectedItem != null))
            {
                try
                {
                    this.laSampleText.Font = new Font(this.cbFontName.Text, this.laSampleText.Font.Size, this.curFontStyle);
                }
                catch
                {
                }
                this.WriteSampleText();
            }
        }

        private void FontSizeChanged(object sender, EventArgs e)
        {
            if (!this.isControlUpdating)
            {
                this.isControlUpdating = true;
                try
                {
                    int num = Math.Max(Math.Min(this.GetInt(this.tbFontSize.Text, 10), EditConsts.MaxFontSize), 1);
                    if (this.tbFontSize.Text != num.ToString())
                    {
                        this.tbFontSize.Text = num.ToString();
                    }
                    this.laSampleText.Font = new Font(this.laSampleText.Font.Name, (float) num, this.curFontStyle);
                    this.WriteSampleText();
                }
                finally
                {
                    this.isControlUpdating = false;
                }
            }
        }

        private void FontStyleChange(object sender, EventArgs e)
        {
            if (!this.isControlUpdating)
            {
                this.curFontStyle = FontStyle.Regular;
                if (this.chbBold.Checked)
                {
                    this.curFontStyle |= FontStyle.Bold;
                }
                else
                {
                    this.curFontStyle &= ~FontStyle.Bold;
                }
                if (this.chbItalic.Checked)
                {
                    this.curFontStyle |= FontStyle.Italic;
                }
                else
                {
                    this.curFontStyle &= ~FontStyle.Italic;
                }
                if (this.chbUnderline.Checked)
                {
                    this.curFontStyle |= FontStyle.Underline;
                }
                else
                {
                    this.curFontStyle &= ~FontStyle.Underline;
                }
                this.StyleFromControl();
            }
        }

        private Panel GetCurrentPanel()
        {
            Panel pnGeneral = null;
            TreeNode selectedNode = this.tvProperties.SelectedNode;
            if (selectedNode != null)
            {
                switch (this.GetNodeLevel(selectedNode))
                {
                    case 0:
                        pnGeneral = this.rootNode.Nodes.Contains(this.generalNode) ? this.pnGeneral : null;
                        break;

                    case 1:
                        if (!selectedNode.Equals(this.generalNode))
                        {
                            if (selectedNode.Equals(this.fontsNode))
                            {
                                pnGeneral = this.pnFontsColors;
                            }
                            else if (selectedNode.Equals(this.additionalNode))
                            {
                                pnGeneral = this.pnAdditional;
                            }
                            else if (selectedNode.Equals(this.keyboardNode))
                            {
                                pnGeneral = this.pnKeyboard;
                            }
                            else
                            {
                                pnGeneral = null;
                            }
                            break;
                        }
                        pnGeneral = this.pnGeneral;
                        break;
                }
            }
            if (pnGeneral != null)
            {
                pnGeneral.Location = new Point(0, 0);
                pnGeneral.Size = this.pnManage.Size;
                pnGeneral.Dock = DockStyle.Fill;
            }
            return pnGeneral;
        }

        private string[] GetFonts()
        {
            FontFamily[] families = FontFamily.Families;
            string[] strArray = new string[families.Length];
            for (int i = 0; i < families.Length; i++)
            {
                strArray[i] = families[i].Name;
            }
            return strArray;
        }

        private int GetInt(string s, int DefaultValue)
        {
            try
            {
                return int.Parse(s);
            }
            catch
            {
                return DefaultValue;
            }
        }

        private int GetNodeLevel(TreeNode node)
        {
            int num = 0;
            if (node != null)
            {
                while (node.Parent != null)
                {
                    node = node.Parent;
                    num++;
                }
            }
            return num;
        }

        private ILexStyle GetSelectedStyle()
        {
            if (this.lbStyles.SelectedItem != null)
            {
                return this.syntaxSettings.LexStyles[this.lbStyles.SelectedIndex];
            }
            return null;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            TreeNode node = new TreeNode("General");
            TreeNode node2 = new TreeNode("Fonts and Colors");
            TreeNode node3 = new TreeNode("Additional");
            TreeNode node4 = new TreeNode("Keyboard");
            TreeNode node5 = new TreeNode("Options", 1, 0, new TreeNode[] { node, node2, node3, node4 });
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgSyntaxSettings));
            this.pnButtons = new Panel();
            this.btCancel = new Button();
            this.btOK = new Button();
            this.pnMain = new Panel();
            this.pnManage = new Panel();
            this.tcMain = new TabControl();
            this.tpGeneral = new TabPage();
            this.pnGeneral = new Panel();
            this.gbLineNumbers = new GroupBox();
            this.chbLineNumbers = new CheckBox();
            this.chbLineNumbersOnGutter = new CheckBox();
            this.gbGutterMargin = new GroupBox();
            this.tbMarginPosition = new TextBox();
            this.tbGutterWidth = new TextBox();
            this.chbShowMargin = new CheckBox();
            this.laGutterWidth = new Label();
            this.chbShowGutter = new CheckBox();
            this.laMarginPosition = new Label();
            this.gbDocument = new GroupBox();
            this.chbLineSeparator = new CheckBox();
            this.chbLineModificator = new CheckBox();
            this.chbWhiteSpace = new CheckBox();
            this.chbForced = new CheckBox();
            this.chbDragAndDrop = new CheckBox();
            this.chbHorzScrollBar = new CheckBox();
            this.chbVertScrollBar = new CheckBox();
            this.chbWordWrap = new CheckBox();
            this.chbHighlightUrls = new CheckBox();
            this.tpAdditional = new TabPage();
            this.pnAdditional = new Panel();
            this.gbTabOptions = new GroupBox();
            this.tbTabStops = new TextBox();
            this.rbKeepTabs = new RadioButton();
            this.rbInsertSpaces = new RadioButton();
            this.laTabSizes = new Label();
            this.gbOutlineOptions = new GroupBox();
            this.chbAllowOutlining = new CheckBox();
            this.chbShowHints = new CheckBox();
            this.gbNavigateOptions = new GroupBox();
            this.chbMoveOnRightButton = new CheckBox();
            this.chbBeyondEof = new CheckBox();
            this.chbBeyondEol = new CheckBox();
            this.tpFontsAndColors = new TabPage();
            this.pnFontsColors = new Panel();
            this.cbForeColor = new ColorBox(this.components);
            this.cbBackColor = new ColorBox(this.components);
            this.gbColorThemes = new GroupBox();
            this.btDeleteColorTheme = new Button();
            this.btAddColorTheme = new Button();
            this.cbColorThemes = new ComboBox();
            this.tbFontSize = new TextBox();
            this.laDisplayItems = new Label();
            this.pnSampleText = new Panel();
            this.laSampleText = new Label();
            this.laSample = new Label();
            this.laSize = new Label();
            this.laFont = new Label();
            this.cbFontName = new ComboBox();
            this.laDescription = new Label();
            this.tbDescription = new TextBox();
            this.laBackColor = new Label();
            this.laForeColor = new Label();
            this.lbStyles = new ListBox();
            this.gbFontAttributes = new GroupBox();
            this.chbUnderline = new CheckBox();
            this.chbItalic = new CheckBox();
            this.chbBold = new CheckBox();
            this.tpKeyboard = new TabPage();
            this.pnKeyboard = new Panel();
            this.cbShortcuts = new ComboBox();
            this.laShortcuts = new Label();
            this.lbEventHandlers = new ListBox();
            this.tbShowCommands = new TextBox();
            this.laShowCommands = new Label();
            this.btDeleteScheme = new Button();
            this.btSaveSchemeAs = new Button();
            this.cbKeyboardSchemes = new ComboBox();
            this.laKeyboardMappingScheme = new Label();
            this.pnTree = new Panel();
            this.tvProperties = new TreeView();
            this.imageList1 = new ImageList(this.components);
            this.colorDialog1 = new ColorDialog();
            this.pnButtons.SuspendLayout();
            this.pnMain.SuspendLayout();
            this.pnManage.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.pnGeneral.SuspendLayout();
            this.gbLineNumbers.SuspendLayout();
            this.gbGutterMargin.SuspendLayout();
            this.gbDocument.SuspendLayout();
            this.tpAdditional.SuspendLayout();
            this.pnAdditional.SuspendLayout();
            this.gbTabOptions.SuspendLayout();
            this.gbOutlineOptions.SuspendLayout();
            this.gbNavigateOptions.SuspendLayout();
            this.tpFontsAndColors.SuspendLayout();
            this.pnFontsColors.SuspendLayout();
            this.gbColorThemes.SuspendLayout();
            this.pnSampleText.SuspendLayout();
            this.gbFontAttributes.SuspendLayout();
            this.tpKeyboard.SuspendLayout();
            this.pnKeyboard.SuspendLayout();
            this.pnTree.SuspendLayout();
            base.SuspendLayout();
            this.pnButtons.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.pnButtons.BorderStyle = BorderStyle.Fixed3D;
            this.pnButtons.Controls.Add(this.btCancel);
            this.pnButtons.Controls.Add(this.btOK);
            this.pnButtons.Location = new Point(0, 360);
            this.pnButtons.Name = "pnButtons";
            this.pnButtons.Size = new Size(0x22c, 40);
            this.pnButtons.TabIndex = 6;
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btCancel.FlatStyle = FlatStyle.System;
            this.btCancel.Location = new Point(0x1c8, 8);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new Size(0x4b, 0x17);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btOK.DialogResult = DialogResult.OK;
            this.btOK.FlatStyle = FlatStyle.System;
            this.btOK.Location = new Point(0x178, 8);
            this.btOK.Name = "btOK";
            this.btOK.Size = new Size(0x4b, 0x17);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.Click += new EventHandler(this.btOK_Click);
            this.pnMain.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnMain.Controls.Add(this.pnManage);
            this.pnMain.Controls.Add(this.pnTree);
            this.pnMain.Location = new Point(0, 0);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new Size(0x228, 360);
            this.pnMain.TabIndex = 7;
            this.pnManage.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnManage.Controls.Add(this.tcMain);
            this.pnManage.Location = new Point(0x88, 0);
            this.pnManage.Name = "pnManage";
            this.pnManage.Size = new Size(0x1a0, 360);
            this.pnManage.TabIndex = 1;
            this.tcMain.Controls.Add(this.tpGeneral);
            this.tcMain.Controls.Add(this.tpAdditional);
            this.tcMain.Controls.Add(this.tpFontsAndColors);
            this.tcMain.Controls.Add(this.tpKeyboard);
            this.tcMain.Dock = DockStyle.Fill;
            this.tcMain.Location = new Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new Size(0x1a0, 360);
            this.tcMain.TabIndex = 0;
            this.tcMain.Visible = false;
            this.tpGeneral.Controls.Add(this.pnGeneral);
            this.tpGeneral.Location = new Point(4, 0x16);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Size = new Size(0x198, 0x14e);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.pnGeneral.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnGeneral.BackColor = SystemColors.Control;
            this.pnGeneral.Controls.Add(this.gbLineNumbers);
            this.pnGeneral.Controls.Add(this.gbGutterMargin);
            this.pnGeneral.Controls.Add(this.gbDocument);
            this.pnGeneral.Location = new Point(0, 0);
            this.pnGeneral.Name = "pnGeneral";
            this.pnGeneral.Size = new Size(0x1a8, 0x170);
            this.pnGeneral.TabIndex = 1;
            this.pnGeneral.Visible = false;
            this.gbLineNumbers.Controls.Add(this.chbLineNumbers);
            this.gbLineNumbers.Controls.Add(this.chbLineNumbersOnGutter);
            this.gbLineNumbers.FlatStyle = FlatStyle.System;
            this.gbLineNumbers.Location = new Point(8, 200);
            this.gbLineNumbers.Name = "gbLineNumbers";
            this.gbLineNumbers.Size = new Size(0x180, 0x48);
            this.gbLineNumbers.TabIndex = 0x27;
            this.gbLineNumbers.TabStop = false;
            this.gbLineNumbers.Text = "Line Numbers";
            this.chbLineNumbers.FlatStyle = FlatStyle.System;
            this.chbLineNumbers.Location = new Point(8, 0x10);
            this.chbLineNumbers.Name = "chbLineNumbers";
            this.chbLineNumbers.Size = new Size(200, 0x19);
            this.chbLineNumbers.TabIndex = 0;
            this.chbLineNumbers.Text = "Show Line Numbers";
            this.chbLineNumbersOnGutter.FlatStyle = FlatStyle.System;
            this.chbLineNumbersOnGutter.Location = new Point(8, 0x29);
            this.chbLineNumbersOnGutter.Name = "chbLineNumbersOnGutter";
            this.chbLineNumbersOnGutter.Size = new Size(200, 0x19);
            this.chbLineNumbersOnGutter.TabIndex = 1;
            this.chbLineNumbersOnGutter.Text = "Display on Gutter";
            this.gbGutterMargin.Controls.Add(this.tbMarginPosition);
            this.gbGutterMargin.Controls.Add(this.tbGutterWidth);
            this.gbGutterMargin.Controls.Add(this.chbShowMargin);
            this.gbGutterMargin.Controls.Add(this.laGutterWidth);
            this.gbGutterMargin.Controls.Add(this.chbShowGutter);
            this.gbGutterMargin.Controls.Add(this.laMarginPosition);
            this.gbGutterMargin.FlatStyle = FlatStyle.System;
            this.gbGutterMargin.Location = new Point(8, 0x74);
            this.gbGutterMargin.Name = "gbGutterMargin";
            this.gbGutterMargin.Size = new Size(0x180, 0x48);
            this.gbGutterMargin.TabIndex = 0x26;
            this.gbGutterMargin.TabStop = false;
            this.gbGutterMargin.Text = "Gutter&&Margin";
            this.tbMarginPosition.Location = new Point(0x144, 40);
            this.tbMarginPosition.Name = "tbMarginPosition";
            this.tbMarginPosition.Size = new Size(0x30, 20);
            this.tbMarginPosition.TabIndex = 8;
            this.tbMarginPosition.Text = "0";
            this.tbMarginPosition.KeyPress += new KeyPressEventHandler(this.tbMarginPosition_KeyPress);
            this.tbGutterWidth.Location = new Point(0x144, 0x10);
            this.tbGutterWidth.Name = "tbGutterWidth";
            this.tbGutterWidth.Size = new Size(0x30, 20);
            this.tbGutterWidth.TabIndex = 7;
            this.tbGutterWidth.Text = "0";
            this.tbGutterWidth.KeyPress += new KeyPressEventHandler(this.tbGutterWidth_KeyPress);
            this.chbShowMargin.FlatStyle = FlatStyle.System;
            this.chbShowMargin.Location = new Point(8, 0x29);
            this.chbShowMargin.Name = "chbShowMargin";
            this.chbShowMargin.Size = new Size(0x88, 0x19);
            this.chbShowMargin.TabIndex = 1;
            this.chbShowMargin.Text = "Show Margin";
            this.laGutterWidth.FlatStyle = FlatStyle.System;
            this.laGutterWidth.Location = new Point(160, 0x10);
            this.laGutterWidth.Name = "laGutterWidth";
            this.laGutterWidth.Size = new Size(0x98, 0x19);
            this.laGutterWidth.TabIndex = 3;
            this.laGutterWidth.Text = "Gutter width:";
            this.chbShowGutter.FlatStyle = FlatStyle.System;
            this.chbShowGutter.Location = new Point(8, 0x10);
            this.chbShowGutter.Name = "chbShowGutter";
            this.chbShowGutter.Size = new Size(0x88, 0x19);
            this.chbShowGutter.TabIndex = 0;
            this.chbShowGutter.Text = "Show Gutter";
            this.laMarginPosition.FlatStyle = FlatStyle.System;
            this.laMarginPosition.Location = new Point(160, 0x29);
            this.laMarginPosition.Name = "laMarginPosition";
            this.laMarginPosition.Size = new Size(0x98, 0x19);
            this.laMarginPosition.TabIndex = 4;
            this.laMarginPosition.Text = "Margin position:";
            this.gbDocument.Controls.Add(this.chbLineSeparator);
            this.gbDocument.Controls.Add(this.chbLineModificator);
            this.gbDocument.Controls.Add(this.chbWhiteSpace);
            this.gbDocument.Controls.Add(this.chbForced);
            this.gbDocument.Controls.Add(this.chbDragAndDrop);
            this.gbDocument.Controls.Add(this.chbHorzScrollBar);
            this.gbDocument.Controls.Add(this.chbVertScrollBar);
            this.gbDocument.Controls.Add(this.chbWordWrap);
            this.gbDocument.Controls.Add(this.chbHighlightUrls);
            this.gbDocument.FlatStyle = FlatStyle.System;
            this.gbDocument.Location = new Point(8, 8);
            this.gbDocument.Name = "gbDocument";
            this.gbDocument.Size = new Size(0x180, 0x62);
            this.gbDocument.TabIndex = 11;
            this.gbDocument.TabStop = false;
            this.gbDocument.Text = "Document";
            this.chbLineSeparator.FlatStyle = FlatStyle.System;
            this.chbLineSeparator.Location = new Point(250, 0x42);
            this.chbLineSeparator.Name = "chbLineSeparator";
            this.chbLineSeparator.Size = new Size(0x7a, 0x19);
            this.chbLineSeparator.TabIndex = 8;
            this.chbLineSeparator.Text = "Line separator";
            this.chbLineModificator.FlatStyle = FlatStyle.System;
            this.chbLineModificator.Location = new Point(250, 0x29);
            this.chbLineModificator.Name = "chbLineModificator";
            this.chbLineModificator.Size = new Size(0x7a, 0x19);
            this.chbLineModificator.TabIndex = 7;
            this.chbLineModificator.Text = "Line modificator";
            this.chbWhiteSpace.FlatStyle = FlatStyle.System;
            this.chbWhiteSpace.Location = new Point(250, 0x10);
            this.chbWhiteSpace.Name = "chbWhiteSpace";
            this.chbWhiteSpace.Size = new Size(0x7a, 0x19);
            this.chbWhiteSpace.TabIndex = 6;
            this.chbWhiteSpace.Text = "White space";
            this.chbForced.FlatStyle = FlatStyle.System;
            this.chbForced.Location = new Point(130, 0x42);
            this.chbForced.Name = "chbForced";
            this.chbForced.Size = new Size(0x7a, 0x19);
            this.chbForced.TabIndex = 5;
            this.chbForced.Text = "Forced scroll bars";
            this.chbDragAndDrop.FlatStyle = FlatStyle.System;
            this.chbDragAndDrop.Location = new Point(8, 0x42);
            this.chbDragAndDrop.Name = "chbDragAndDrop";
            this.chbDragAndDrop.Size = new Size(0x90, 0x19);
            this.chbDragAndDrop.TabIndex = 2;
            this.chbDragAndDrop.Text = "&Drag and drop text";
            this.chbHorzScrollBar.FlatStyle = FlatStyle.System;
            this.chbHorzScrollBar.Location = new Point(130, 0x29);
            this.chbHorzScrollBar.Name = "chbHorzScrollBar";
            this.chbHorzScrollBar.Size = new Size(0x7a, 0x19);
            this.chbHorzScrollBar.TabIndex = 4;
            this.chbHorzScrollBar.Text = "&Horizontal scroll bar";
            this.chbVertScrollBar.FlatStyle = FlatStyle.System;
            this.chbVertScrollBar.Location = new Point(130, 0x10);
            this.chbVertScrollBar.Name = "chbVertScrollBar";
            this.chbVertScrollBar.Size = new Size(0x7a, 0x19);
            this.chbVertScrollBar.TabIndex = 3;
            this.chbVertScrollBar.Text = "&Vertical scroll bar";
            this.chbWordWrap.FlatStyle = FlatStyle.System;
            this.chbWordWrap.Location = new Point(8, 0x10);
            this.chbWordWrap.Name = "chbWordWrap";
            this.chbWordWrap.Size = new Size(0x7c, 0x19);
            this.chbWordWrap.TabIndex = 0;
            this.chbWordWrap.Text = "Word Wrap";
            this.chbHighlightUrls.FlatStyle = FlatStyle.System;
            this.chbHighlightUrls.Location = new Point(8, 0x29);
            this.chbHighlightUrls.Name = "chbHighlightUrls";
            this.chbHighlightUrls.Size = new Size(0x80, 0x19);
            this.chbHighlightUrls.TabIndex = 1;
            this.chbHighlightUrls.Text = "Highlight Urls";
            this.tpAdditional.Controls.Add(this.pnAdditional);
            this.tpAdditional.Location = new Point(4, 0x16);
            this.tpAdditional.Name = "tpAdditional";
            this.tpAdditional.Size = new Size(0x198, 0x14e);
            this.tpAdditional.TabIndex = 2;
            this.tpAdditional.Text = "Additional";
            this.pnAdditional.Controls.Add(this.gbTabOptions);
            this.pnAdditional.Controls.Add(this.gbOutlineOptions);
            this.pnAdditional.Controls.Add(this.gbNavigateOptions);
            this.pnAdditional.Location = new Point(0, 0);
            this.pnAdditional.Name = "pnAdditional";
            this.pnAdditional.Size = new Size(400, 0x128);
            this.pnAdditional.TabIndex = 8;
            this.pnAdditional.Visible = false;
            this.gbTabOptions.Controls.Add(this.tbTabStops);
            this.gbTabOptions.Controls.Add(this.rbKeepTabs);
            this.gbTabOptions.Controls.Add(this.rbInsertSpaces);
            this.gbTabOptions.Controls.Add(this.laTabSizes);
            this.gbTabOptions.FlatStyle = FlatStyle.System;
            this.gbTabOptions.Location = new Point(8, 200);
            this.gbTabOptions.Name = "gbTabOptions";
            this.gbTabOptions.Size = new Size(0x180, 0x48);
            this.gbTabOptions.TabIndex = 40;
            this.gbTabOptions.TabStop = false;
            this.gbTabOptions.Text = "Tab Options";
            this.tbTabStops.Location = new Point(8, 40);
            this.tbTabStops.Name = "tbTabStops";
            this.tbTabStops.Size = new Size(120, 20);
            this.tbTabStops.TabIndex = 5;
            this.rbKeepTabs.FlatStyle = FlatStyle.System;
            this.rbKeepTabs.Location = new Point(160, 0x29);
            this.rbKeepTabs.Name = "rbKeepTabs";
            this.rbKeepTabs.Size = new Size(0xb8, 0x19);
            this.rbKeepTabs.TabIndex = 4;
            this.rbKeepTabs.Text = "&Keep tabs";
            this.rbInsertSpaces.FlatStyle = FlatStyle.System;
            this.rbInsertSpaces.Location = new Point(160, 0x10);
            this.rbInsertSpaces.Name = "rbInsertSpaces";
            this.rbInsertSpaces.Size = new Size(0xb8, 0x19);
            this.rbInsertSpaces.TabIndex = 3;
            this.rbInsertSpaces.Text = "Insert s&paces";
            this.laTabSizes.AutoSize = true;
            this.laTabSizes.FlatStyle = FlatStyle.System;
            this.laTabSizes.Location = new Point(8, 0x15);
            this.laTabSizes.Name = "laTabSizes";
            this.laTabSizes.Size = new Size(0x39, 13);
            this.laTabSizes.TabIndex = 0;
            this.laTabSizes.Text = "Tab Sizes:";
            this.gbOutlineOptions.Controls.Add(this.chbAllowOutlining);
            this.gbOutlineOptions.Controls.Add(this.chbShowHints);
            this.gbOutlineOptions.FlatStyle = FlatStyle.System;
            this.gbOutlineOptions.Location = new Point(8, 0x74);
            this.gbOutlineOptions.Name = "gbOutlineOptions";
            this.gbOutlineOptions.Size = new Size(0x180, 0x48);
            this.gbOutlineOptions.TabIndex = 0x27;
            this.gbOutlineOptions.TabStop = false;
            this.gbOutlineOptions.Text = "Outline Options";
            this.chbAllowOutlining.FlatStyle = FlatStyle.System;
            this.chbAllowOutlining.Location = new Point(8, 0x10);
            this.chbAllowOutlining.Name = "chbAllowOutlining";
            this.chbAllowOutlining.Size = new Size(0x150, 0x19);
            this.chbAllowOutlining.TabIndex = 0;
            this.chbAllowOutlining.Text = "Allow outlining";
            this.chbShowHints.FlatStyle = FlatStyle.System;
            this.chbShowHints.Location = new Point(8, 0x29);
            this.chbShowHints.Name = "chbShowHints";
            this.chbShowHints.Size = new Size(0x150, 0x19);
            this.chbShowHints.TabIndex = 1;
            this.chbShowHints.Text = "Show Hints";
            this.gbNavigateOptions.Controls.Add(this.chbMoveOnRightButton);
            this.gbNavigateOptions.Controls.Add(this.chbBeyondEof);
            this.gbNavigateOptions.Controls.Add(this.chbBeyondEol);
            this.gbNavigateOptions.FlatStyle = FlatStyle.System;
            this.gbNavigateOptions.Location = new Point(8, 8);
            this.gbNavigateOptions.Name = "gbNavigateOptions";
            this.gbNavigateOptions.Size = new Size(0x180, 0x62);
            this.gbNavigateOptions.TabIndex = 0x26;
            this.gbNavigateOptions.TabStop = false;
            this.gbNavigateOptions.Text = "Navigate Options";
            this.chbMoveOnRightButton.FlatStyle = FlatStyle.System;
            this.chbMoveOnRightButton.Location = new Point(8, 0x42);
            this.chbMoveOnRightButton.Name = "chbMoveOnRightButton";
            this.chbMoveOnRightButton.Size = new Size(0x150, 0x19);
            this.chbMoveOnRightButton.TabIndex = 14;
            this.chbMoveOnRightButton.Text = "Move on Right Button";
            this.chbBeyondEof.FlatStyle = FlatStyle.System;
            this.chbBeyondEof.Location = new Point(8, 0x29);
            this.chbBeyondEof.Name = "chbBeyondEof";
            this.chbBeyondEof.Size = new Size(0x150, 0x19);
            this.chbBeyondEof.TabIndex = 13;
            this.chbBeyondEof.Text = "Beyond Eof";
            this.chbBeyondEol.FlatStyle = FlatStyle.System;
            this.chbBeyondEol.Location = new Point(8, 0x10);
            this.chbBeyondEol.Name = "chbBeyondEol";
            this.chbBeyondEol.Size = new Size(0x150, 0x19);
            this.chbBeyondEol.TabIndex = 12;
            this.chbBeyondEol.Text = "Beyond Eol";
            this.tpFontsAndColors.Controls.Add(this.pnFontsColors);
            this.tpFontsAndColors.Location = new Point(4, 0x16);
            this.tpFontsAndColors.Name = "tpFontsAndColors";
            this.tpFontsAndColors.Size = new Size(0x198, 0x14e);
            this.tpFontsAndColors.TabIndex = 1;
            this.tpFontsAndColors.Text = "Fonts&&Colors";
            this.pnFontsColors.Controls.Add(this.cbForeColor);
            this.pnFontsColors.Controls.Add(this.cbBackColor);
            this.pnFontsColors.Controls.Add(this.gbColorThemes);
            this.pnFontsColors.Controls.Add(this.tbFontSize);
            this.pnFontsColors.Controls.Add(this.laDisplayItems);
            this.pnFontsColors.Controls.Add(this.pnSampleText);
            this.pnFontsColors.Controls.Add(this.laSample);
            this.pnFontsColors.Controls.Add(this.laSize);
            this.pnFontsColors.Controls.Add(this.laFont);
            this.pnFontsColors.Controls.Add(this.cbFontName);
            this.pnFontsColors.Controls.Add(this.laDescription);
            this.pnFontsColors.Controls.Add(this.tbDescription);
            this.pnFontsColors.Controls.Add(this.laBackColor);
            this.pnFontsColors.Controls.Add(this.laForeColor);
            this.pnFontsColors.Controls.Add(this.lbStyles);
            this.pnFontsColors.Controls.Add(this.gbFontAttributes);
            this.pnFontsColors.Location = new Point(0, 0);
            this.pnFontsColors.Name = "pnFontsColors";
            this.pnFontsColors.Size = new Size(400, 0x148);
            this.pnFontsColors.TabIndex = 7;
            this.pnFontsColors.Visible = false;
            this.cbForeColor.DrawMode = DrawMode.OwnerDrawFixed;
            this.cbForeColor.Location = new Point(0x98, 0xd8);
            this.cbForeColor.Name = "cbForeColor";
            this.cbForeColor.SelectedColor = Color.Empty;
            this.cbForeColor.Size = new Size(120, 0x15);
            this.cbForeColor.TabIndex = 0x11;
            this.cbForeColor.SelectedIndexChanged += new EventHandler(this.cbForeColor_SelectedIndexChanged);
            this.cbBackColor.DrawMode = DrawMode.OwnerDrawFixed;
            this.cbBackColor.Location = new Point(0x98, 0x108);
            this.cbBackColor.Name = "cbBackColor";
            this.cbBackColor.SelectedColor = Color.Empty;
            this.cbBackColor.Size = new Size(120, 0x15);
            this.cbBackColor.TabIndex = 0x12;
            this.cbBackColor.SelectedIndexChanged += new EventHandler(this.cbBackColor_SelectedIndexChanged);
            this.gbColorThemes.Controls.Add(this.btDeleteColorTheme);
            this.gbColorThemes.Controls.Add(this.btAddColorTheme);
            this.gbColorThemes.Controls.Add(this.cbColorThemes);
            this.gbColorThemes.Location = new Point(8, 8);
            this.gbColorThemes.Name = "gbColorThemes";
            this.gbColorThemes.Size = new Size(0x188, 0x55);
            this.gbColorThemes.TabIndex = 20;
            this.gbColorThemes.TabStop = false;
            this.gbColorThemes.Text = "Color Themes";
            this.btDeleteColorTheme.Location = new Point(0xed, 0x38);
            this.btDeleteColorTheme.Name = "btDeleteColorTheme";
            this.btDeleteColorTheme.Size = new Size(0x93, 0x17);
            this.btDeleteColorTheme.TabIndex = 2;
            this.btDeleteColorTheme.Text = "&Delete Color Theme";
            this.btDeleteColorTheme.Click += new EventHandler(this.btDeleteColorTheme_Click);
            this.btAddColorTheme.Location = new Point(0xed, 0x13);
            this.btAddColorTheme.Name = "btAddColorTheme";
            this.btAddColorTheme.Size = new Size(0x93, 0x17);
            this.btAddColorTheme.TabIndex = 1;
            this.btAddColorTheme.Text = "&Add Color Theme";
            this.btAddColorTheme.Click += new EventHandler(this.btAddColorTheme_Click);
            this.cbColorThemes.Location = new Point(10, 0x13);
            this.cbColorThemes.Name = "cbColorThemes";
            this.cbColorThemes.Size = new Size(0xd7, 0x15);
            this.cbColorThemes.TabIndex = 0;
            this.cbColorThemes.SelectedIndexChanged += new EventHandler(this.cbColorThemes_SelectedIndexChanged);
            this.cbColorThemes.Leave += new EventHandler(this.cbColorThemes_Leave);
            this.tbFontSize.Location = new Point(0x98, 0x70);
            this.tbFontSize.Name = "tbFontSize";
            this.tbFontSize.Size = new Size(0x20, 20);
            this.tbFontSize.TabIndex = 0x13;
            this.tbFontSize.Text = "1";
            this.tbFontSize.Leave += new EventHandler(this.tbFontSize_Leave);
            this.laDisplayItems.AutoSize = true;
            this.laDisplayItems.FlatStyle = FlatStyle.System;
            this.laDisplayItems.Location = new Point(8, 0x90);
            this.laDisplayItems.Name = "laDisplayItems";
            this.laDisplayItems.Size = new Size(0x47, 13);
            this.laDisplayItems.TabIndex = 4;
            this.laDisplayItems.Text = "&Display items:";
            this.pnSampleText.BorderStyle = BorderStyle.FixedSingle;
            this.pnSampleText.Controls.Add(this.laSampleText);
            this.pnSampleText.Location = new Point(8, 0x131);
            this.pnSampleText.Name = "pnSampleText";
            this.pnSampleText.Size = new Size(0x180, 0x30);
            this.pnSampleText.TabIndex = 14;
            this.laSampleText.AutoSize = true;
            this.laSampleText.Location = new Point(160, 0x10);
            this.laSampleText.Name = "laSampleText";
            this.laSampleText.Size = new Size(0x39, 13);
            this.laSampleText.TabIndex = 0;
            this.laSampleText.Text = "AaBbYyZz";
            this.laSample.AutoSize = true;
            this.laSample.FlatStyle = FlatStyle.System;
            this.laSample.Location = new Point(8, 0x121);
            this.laSample.Name = "laSample";
            this.laSample.Size = new Size(0x2d, 13);
            this.laSample.TabIndex = 13;
            this.laSample.Text = "Sample:";
            this.laSize.AutoSize = true;
            this.laSize.FlatStyle = FlatStyle.System;
            this.laSize.Location = new Point(0x98, 0x60);
            this.laSize.Name = "laSize";
            this.laSize.Size = new Size(30, 13);
            this.laSize.TabIndex = 2;
            this.laSize.Text = "&Size:";
            this.laFont.AutoSize = true;
            this.laFont.FlatStyle = FlatStyle.System;
            this.laFont.Location = new Point(8, 0x60);
            this.laFont.Name = "laFont";
            this.laFont.Size = new Size(0x1f, 13);
            this.laFont.TabIndex = 0;
            this.laFont.Text = "Font:";
            this.cbFontName.Location = new Point(8, 0x70);
            this.cbFontName.Name = "cbFontName";
            this.cbFontName.Size = new Size(0x79, 0x15);
            this.cbFontName.TabIndex = 1;
            this.cbFontName.SelectedIndexChanged += new EventHandler(this.cbFontName_SelectedIndexChanged);
            this.laDescription.AutoSize = true;
            this.laDescription.FlatStyle = FlatStyle.System;
            this.laDescription.Location = new Point(0x98, 0x90);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new Size(0x3f, 13);
            this.laDescription.TabIndex = 6;
            this.laDescription.Text = "Description:";
            this.tbDescription.Location = new Point(0x98, 160);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new Size(240, 20);
            this.tbDescription.TabIndex = 7;
            this.laBackColor.AutoSize = true;
            this.laBackColor.FlatStyle = FlatStyle.System;
            this.laBackColor.Location = new Point(0x98, 240);
            this.laBackColor.Name = "laBackColor";
            this.laBackColor.Size = new Size(0x3e, 13);
            this.laBackColor.TabIndex = 10;
            this.laBackColor.Text = "Back Color:";
            this.laForeColor.AutoSize = true;
            this.laForeColor.FlatStyle = FlatStyle.System;
            this.laForeColor.Location = new Point(0x98, 0xc0);
            this.laForeColor.Name = "laForeColor";
            this.laForeColor.Size = new Size(0x3a, 13);
            this.laForeColor.TabIndex = 8;
            this.laForeColor.Text = "Fore Color:";
            this.lbStyles.Location = new Point(8, 160);
            this.lbStyles.Name = "lbStyles";
            this.lbStyles.Size = new Size(120, 0x79);
            this.lbStyles.TabIndex = 5;
            this.gbFontAttributes.Controls.Add(this.chbUnderline);
            this.gbFontAttributes.Controls.Add(this.chbItalic);
            this.gbFontAttributes.Controls.Add(this.chbBold);
            this.gbFontAttributes.FlatStyle = FlatStyle.System;
            this.gbFontAttributes.Location = new Point(280, 0xc0);
            this.gbFontAttributes.Name = "gbFontAttributes";
            this.gbFontAttributes.Size = new Size(0x70, 0x5c);
            this.gbFontAttributes.TabIndex = 12;
            this.gbFontAttributes.TabStop = false;
            this.gbFontAttributes.Text = "Attributes:";
            this.chbUnderline.FlatStyle = FlatStyle.System;
            this.chbUnderline.Location = new Point(8, 0x40);
            this.chbUnderline.Name = "chbUnderline";
            this.chbUnderline.Size = new Size(100, 0x18);
            this.chbUnderline.TabIndex = 2;
            this.chbUnderline.Text = "Underline";
            this.chbItalic.FlatStyle = FlatStyle.System;
            this.chbItalic.Location = new Point(8, 40);
            this.chbItalic.Name = "chbItalic";
            this.chbItalic.Size = new Size(100, 0x18);
            this.chbItalic.TabIndex = 1;
            this.chbItalic.Text = "Italic";
            this.chbBold.FlatStyle = FlatStyle.System;
            this.chbBold.Location = new Point(8, 0x10);
            this.chbBold.Name = "chbBold";
            this.chbBold.Size = new Size(100, 0x18);
            this.chbBold.TabIndex = 0;
            this.chbBold.Text = "Bold";
            this.tpKeyboard.Controls.Add(this.pnKeyboard);
            this.tpKeyboard.Location = new Point(4, 0x16);
            this.tpKeyboard.Name = "tpKeyboard";
            this.tpKeyboard.Size = new Size(0x198, 0x14e);
            this.tpKeyboard.TabIndex = 3;
            this.tpKeyboard.Text = "Keyboard";
            this.pnKeyboard.Controls.Add(this.cbShortcuts);
            this.pnKeyboard.Controls.Add(this.laShortcuts);
            this.pnKeyboard.Controls.Add(this.lbEventHandlers);
            this.pnKeyboard.Controls.Add(this.tbShowCommands);
            this.pnKeyboard.Controls.Add(this.laShowCommands);
            this.pnKeyboard.Controls.Add(this.btDeleteScheme);
            this.pnKeyboard.Controls.Add(this.btSaveSchemeAs);
            this.pnKeyboard.Controls.Add(this.cbKeyboardSchemes);
            this.pnKeyboard.Controls.Add(this.laKeyboardMappingScheme);
            this.pnKeyboard.Location = new Point(0, 0);
            this.pnKeyboard.Name = "pnKeyboard";
            this.pnKeyboard.Size = new Size(400, 0x14b);
            this.pnKeyboard.TabIndex = 0;
            this.pnKeyboard.Visible = false;
            this.cbShortcuts.Location = new Point(8, 0x123);
            this.cbShortcuts.Name = "cbShortcuts";
            this.cbShortcuts.Size = new Size(0x130, 0x15);
            this.cbShortcuts.TabIndex = 8;
            this.laShortcuts.AutoSize = true;
            this.laShortcuts.Location = new Point(8, 0x113);
            this.laShortcuts.Name = "laShortcuts";
            this.laShortcuts.Size = new Size(0xa8, 13);
            this.laShortcuts.TabIndex = 7;
            this.laShortcuts.Text = "Shor&tcut(s) for selected command:";
            this.lbEventHandlers.Location = new Point(8, 0x58);
            this.lbEventHandlers.Name = "lbEventHandlers";
            this.lbEventHandlers.Size = new Size(0x180, 0xad);
            this.lbEventHandlers.TabIndex = 6;
            this.lbEventHandlers.SelectedIndexChanged += new EventHandler(this.lbEventHandlers_SelectedIndexChanged);
            this.tbShowCommands.Location = new Point(8, 0x40);
            this.tbShowCommands.Name = "tbShowCommands";
            this.tbShowCommands.Size = new Size(0x180, 20);
            this.tbShowCommands.TabIndex = 5;
            this.tbShowCommands.TextChanged += new EventHandler(this.tbShowCommands_TextChanged);
            this.laShowCommands.AutoSize = true;
            this.laShowCommands.Location = new Point(8, 0x30);
            this.laShowCommands.Name = "laShowCommands";
            this.laShowCommands.Size = new Size(0x8f, 13);
            this.laShowCommands.TabIndex = 4;
            this.laShowCommands.Text = "Show &commands containing:";
            this.btDeleteScheme.Enabled = false;
            this.btDeleteScheme.Location = new Point(320, 0x18);
            this.btDeleteScheme.Name = "btDeleteScheme";
            this.btDeleteScheme.Size = new Size(0x4b, 0x17);
            this.btDeleteScheme.TabIndex = 3;
            this.btDeleteScheme.Text = "&Delete";
            this.btSaveSchemeAs.Enabled = false;
            this.btSaveSchemeAs.Location = new Point(0xd0, 0x18);
            this.btSaveSchemeAs.Name = "btSaveSchemeAs";
            this.btSaveSchemeAs.Size = new Size(0x68, 0x17);
            this.btSaveSchemeAs.TabIndex = 2;
            this.btSaveSchemeAs.Text = "&Save As...";
            this.cbKeyboardSchemes.Location = new Point(8, 0x18);
            this.cbKeyboardSchemes.Name = "cbKeyboardSchemes";
            this.cbKeyboardSchemes.Size = new Size(0xc0, 0x15);
            this.cbKeyboardSchemes.TabIndex = 1;
            this.laKeyboardMappingScheme.AutoSize = true;
            this.laKeyboardMappingScheme.Location = new Point(8, 8);
            this.laKeyboardMappingScheme.Name = "laKeyboardMappingScheme";
            this.laKeyboardMappingScheme.Size = new Size(0x8a, 13);
            this.laKeyboardMappingScheme.TabIndex = 0;
            this.laKeyboardMappingScheme.Text = "Keyboard &mapping scheme:";
            this.pnTree.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnTree.Controls.Add(this.tvProperties);
            this.pnTree.Location = new Point(0, 0);
            this.pnTree.Name = "pnTree";
            this.pnTree.Size = new Size(0x88, 360);
            this.pnTree.TabIndex = 0;
            this.tvProperties.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tvProperties.ImageIndex = 3;
            this.tvProperties.ImageList = this.imageList1;
            this.tvProperties.Location = new Point(0, 0);
            this.tvProperties.Name = "tvProperties";
            node.Name = "";
            node.Text = "General";
            node2.Name = "";
            node2.Text = "Fonts and Colors";
            node3.Name = "";
            node3.Text = "Additional";
            node4.Name = "";
            node4.Text = "Keyboard";
            node5.ImageIndex = 1;
            node5.Name = "";
            node5.SelectedImageIndex = 0;
            node5.Text = "Options";
            this.tvProperties.Nodes.AddRange(new TreeNode[] { node5 });
            this.tvProperties.Scrollable = false;
            this.tvProperties.SelectedImageIndex = 2;
            this.tvProperties.ShowLines = false;
            this.tvProperties.ShowPlusMinus = false;
            this.tvProperties.ShowRootLines = false;
            this.tvProperties.Size = new Size(0x88, 360);
            this.tvProperties.TabIndex = 0;
            this.tvProperties.AfterSelect += new TreeViewEventHandler(this.tvProperties_AfterSelect);
            this.imageList1.ImageStream = (ImageListStreamer) manager.GetObject("imageList1.ImageStream");
            this.imageList1.TransparentColor = Color.Red;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            base.AcceptButton = this.btOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btCancel;
            base.ClientSize = new Size(0x228, 0x18e);
            base.Controls.Add(this.pnMain);
            base.Controls.Add(this.pnButtons);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DlgSyntaxSettings";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Syntax Settings";
            base.Load += new EventHandler(this.DlgSyntaxSettings_Load);
            base.Activated += new EventHandler(this.DlgSyntaxSettings_Activated);
            this.pnButtons.ResumeLayout(false);
            this.pnMain.ResumeLayout(false);
            this.pnManage.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.pnGeneral.ResumeLayout(false);
            this.gbLineNumbers.ResumeLayout(false);
            this.gbGutterMargin.ResumeLayout(false);
            this.gbGutterMargin.PerformLayout();
            this.gbDocument.ResumeLayout(false);
            this.tpAdditional.ResumeLayout(false);
            this.pnAdditional.ResumeLayout(false);
            this.gbTabOptions.ResumeLayout(false);
            this.gbTabOptions.PerformLayout();
            this.gbOutlineOptions.ResumeLayout(false);
            this.gbNavigateOptions.ResumeLayout(false);
            this.tpFontsAndColors.ResumeLayout(false);
            this.pnFontsColors.ResumeLayout(false);
            this.pnFontsColors.PerformLayout();
            this.gbColorThemes.ResumeLayout(false);
            this.pnSampleText.ResumeLayout(false);
            this.pnSampleText.PerformLayout();
            this.gbFontAttributes.ResumeLayout(false);
            this.tpKeyboard.ResumeLayout(false);
            this.pnKeyboard.ResumeLayout(false);
            this.pnKeyboard.PerformLayout();
            this.pnTree.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private string KeyDataToString(Keys keyData)
        {
            string[] strArray = keyData.ToString().Split(new char[] { ',' });
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            string str = string.Empty;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].ToLower().IndexOf(this.sControl) >= 0)
                {
                    flag = true;
                }
                else if (strArray[i].ToLower().IndexOf(this.sAlt) >= 0)
                {
                    flag2 = true;
                }
                else if (strArray[i].ToLower().IndexOf(this.sShift) >= 0)
                {
                    flag3 = true;
                }
                else
                {
                    str = (str != string.Empty) ? string.Format("{0} + {1}", str, strArray[i]) : strArray[i];
                }
            }
            if (flag2)
            {
                str = (str != string.Empty) ? string.Format("{0} + {1}", this.sAlt.ToUpper(), str) : this.sAlt.ToUpper();
            }
            if (flag3)
            {
                str = (str != string.Empty) ? string.Format("{0} + {1}", this.sShift.ToUpper(), str) : this.sShift.ToUpper();
            }
            if (flag)
            {
                str = (str != string.Empty) ? string.Format("{0} + {1}", this.sCtrl, str) : this.sCtrl;
            }
            return str;
        }

        private void lbEventHandlers_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateShortcut(this.lbEventHandlers.SelectedIndex);
        }

        private void LoadFromResource()
        {
            this.Text = StringConsts.DlgSyntaxSettingsCaption;
            if (this.tvProperties.Nodes.Count > 0)
            {
                this.rootNode.Text = StringConsts.PropertiesOptionsCaption;
                if (this.tvProperties.Nodes[0].Nodes.Count >= 3)
                {
                    this.generalNode.Text = StringConsts.PropertiesGeneralCaption;
                    this.fontsNode.Text = StringConsts.PropertiesFontsColorsCaption;
                    this.additionalNode.Text = StringConsts.PropertiesAdditionalCaption;
                    this.keyboardNode.Text = StringConsts.PropertiesKeyboradCaption;
                }
            }
            this.tpGeneral.Text = StringConsts.GeneralCaption;
            this.tpFontsAndColors.Text = StringConsts.FontsAndColorsCaption;
            this.tpAdditional.Text = StringConsts.AdditionalCaption;
            this.gbDocument.Text = StringConsts.DocumentCaption;
            this.gbGutterMargin.Text = StringConsts.GutterMarginCaption;
            this.gbLineNumbers.Text = StringConsts.GroupBoxLineNumbersCaption_SyntaxSettingsDlg;
            this.chbWordWrap.Text = StringConsts.WordWrapCaption_SyntaxSettingsDlg;
            this.chbHighlightUrls.Text = StringConsts.HighlightUrlsCaption;
            this.chbDragAndDrop.Text = StringConsts.DragAndDropCaption;
            this.chbVertScrollBar.Text = StringConsts.VertScrollBarCaption;
            this.chbHorzScrollBar.Text = StringConsts.HorzScrollBarCaption;
            this.chbForced.Text = StringConsts.ForcedCaption;
            this.chbShowGutter.Text = StringConsts.ShowGutterCaption;
            this.chbShowMargin.Text = StringConsts.ShowMarginCaption;
            this.laGutterWidth.Text = StringConsts.GutterWidthCaption;
            this.laMarginPosition.Text = StringConsts.MarginPositionCaption;
            this.chbLineNumbers.Text = StringConsts.CheckBoxLineNumbersCaption_SyntaxSettingsDlg;
            this.chbLineNumbersOnGutter.Text = StringConsts.LineNumbersOnGutterCaption;
            this.laFont.Text = StringConsts.FontCaption;
            this.laSize.Text = StringConsts.SizeCaption;
            this.btAddColorTheme.Text = StringConsts.AddColorTheme;
            this.btDeleteColorTheme.Text = StringConsts.DeleteColorTheme;
            this.laDisplayItems.Text = StringConsts.DisplayItemsCaption;
            this.laDescription.Text = StringConsts.DescriptionCaption;
            this.laForeColor.Text = StringConsts.ForeColorCaption;
            this.laBackColor.Text = StringConsts.BackColorCaption;
            this.gbFontAttributes.Text = StringConsts.FontAttributesCaption;
            this.chbBold.Text = StringConsts.BoldCaption;
            this.chbItalic.Text = StringConsts.ItalicCaption;
            this.chbUnderline.Text = StringConsts.UnderlineCaption;
            this.laSample.Text = StringConsts.SampleCaption;
            this.laSampleText.Text = StringConsts.SampleTextCaption;
            this.gbNavigateOptions.Text = StringConsts.NavigateOptionsCaption;
            this.gbOutlineOptions.Text = StringConsts.OutlineOptionsCaption;
            this.gbTabOptions.Text = StringConsts.TabOptionsCaption;
            this.chbBeyondEol.Text = StringConsts.BeyondEolCaption;
            this.chbBeyondEof.Text = StringConsts.BeyondEofCaption;
            this.chbMoveOnRightButton.Text = StringConsts.MoveOnRightButtonCaption;
            this.chbAllowOutlining.Text = StringConsts.AllowOutliningCaption;
            this.chbShowHints.Text = StringConsts.ShowHintsCaption;
            this.laTabSizes.Text = StringConsts.TabSizesCaption;
            this.rbInsertSpaces.Text = StringConsts.InsertSpacesCaption;
            this.rbKeepTabs.Text = StringConsts.KeepTabsCaption;
            this.btOK.Text = StringConsts.OKCaption_SyntaxSettingsDlg;
            this.btCancel.Text = StringConsts.CancelCaption_SyntaxSettingsDlg;
            this.chbWhiteSpace.Text = StringConsts.WhiteSpaceCaption_SyntaxSettingsDlg;
            this.chbLineModificator.Text = StringConsts.LineModificatorCaption_SyntaxSettingsDlg;
            this.chbLineSeparator.Text = StringConsts.LineSeparatorCaption_SyntaxSettingsDlg;
            this.laKeyboardMappingScheme.Text = StringConsts.KeyboardMappingSchemeCaption;
            this.laShowCommands.Text = StringConsts.ShowCommandsCaption;
            this.laShortcuts.Text = StringConsts.ShortcutsCaption;
            this.btSaveSchemeAs.Text = StringConsts.SaveSchemeAsCaption;
            this.btDeleteScheme.Text = StringConsts.DeleteSchemeCaption;
        }

        private void OnStyleSelected(object sender, EventArgs e)
        {
            this.StyleSelected();
        }

        event HelpEventHandler IEditorSettingsDialog.HelpRequested
        {
            add
            {
                base.HelpRequested += value;
            }
            remove
            {
                base.HelpRequested -= value;
            }
        }

        DialogResult IEditorSettingsDialog.ShowDialog()
        {
            return base.ShowDialog();
        }

        DialogResult IEditorSettingsDialog.ShowDialog(IWin32Window window1)
        {
            return base.ShowDialog(window1);
        }

        private void SettingsFromControl()
        {
            bool flag = this.chbVertScrollBar.Checked;
            bool flag2 = this.chbHorzScrollBar.Checked;
            bool flag3 = this.chbForced.Checked;
            if (flag2)
            {
                if (flag)
                {
                    this.syntaxSettings.ScrollBars = flag3 ? RichTextBoxScrollBars.ForcedBoth : RichTextBoxScrollBars.Both;
                }
                else
                {
                    this.syntaxSettings.ScrollBars = flag3 ? RichTextBoxScrollBars.ForcedHorizontal : RichTextBoxScrollBars.Horizontal;
                }
            }
            else if (flag)
            {
                this.syntaxSettings.ScrollBars = flag3 ? RichTextBoxScrollBars.ForcedVertical : RichTextBoxScrollBars.Vertical;
            }
            else
            {
                this.syntaxSettings.ScrollBars = RichTextBoxScrollBars.None;
            }
            if (this.chbDragAndDrop.Checked)
            {
                this.syntaxSettings.SelectionOptions &= ~SelectionOptions.DisableDragging;
            }
            else
            {
                this.syntaxSettings.SelectionOptions |= SelectionOptions.DisableDragging;
            }
            this.syntaxSettings.ShowMargin = this.chbShowMargin.Checked;
            this.syntaxSettings.WordWrap = this.chbWordWrap.Checked;
            this.syntaxSettings.WhiteSpaceVisible = this.chbWhiteSpace.Checked;
            this.syntaxSettings.ShowGutter = this.chbShowGutter.Checked;
            this.syntaxSettings.GutterWidth = this.GetInt(this.tbGutterWidth.Text, EditConsts.DefaultGutterWidth);
            this.syntaxSettings.MarginPos = this.GetInt(this.tbMarginPosition.Text, EditConsts.DefaultMarginPosition);
            if (this.chbLineNumbers.Checked)
            {
                this.syntaxSettings.GutterOptions |= GutterOptions.PaintLineNumbers;
            }
            else
            {
                this.syntaxSettings.GutterOptions &= ~GutterOptions.PaintLineNumbers;
            }
            if (this.chbLineNumbersOnGutter.Checked)
            {
                this.syntaxSettings.GutterOptions |= GutterOptions.PaintLinesOnGutter;
            }
            else
            {
                this.syntaxSettings.GutterOptions &= ~GutterOptions.PaintLinesOnGutter;
            }
            if (this.chbLineModificator.Checked)
            {
                this.syntaxSettings.GutterOptions |= GutterOptions.PaintLineModificators;
            }
            else
            {
                this.syntaxSettings.GutterOptions &= ~GutterOptions.PaintLineModificators;
            }
            if (this.chbLineSeparator.Checked)
            {
                this.syntaxSettings.SeparatorOptions |= SeparatorOptions.SeparateLines;
            }
            else
            {
                this.syntaxSettings.SeparatorOptions &= ~SeparatorOptions.SeparateLines;
            }
            if (this.chbBeyondEol.Checked)
            {
                this.syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEol;
            }
            else
            {
                this.syntaxSettings.NavigateOptions &= ~NavigateOptions.BeyondEol;
            }
            if (this.chbBeyondEof.Checked)
            {
                this.syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEof;
            }
            else
            {
                this.syntaxSettings.NavigateOptions &= ~NavigateOptions.BeyondEof;
            }
            if (this.chbMoveOnRightButton.Checked)
            {
                this.syntaxSettings.NavigateOptions |= NavigateOptions.MoveOnRightButton;
            }
            else
            {
                this.syntaxSettings.NavigateOptions &= ~NavigateOptions.MoveOnRightButton;
            }
            if (this.chbShowHints.Checked)
            {
                this.syntaxSettings.OutlineOptions |= OutlineOptions.ShowHints;
            }
            else
            {
                this.syntaxSettings.OutlineOptions &= ~OutlineOptions.ShowHints;
            }
            this.syntaxSettings.HighlightHyperText = this.chbHighlightUrls.Checked;
            this.syntaxSettings.AllowOutlining = this.chbAllowOutlining.Checked;
            this.syntaxSettings.UseSpaces = this.rbInsertSpaces.Checked;
            string[] strArray = this.tbTabStops.Text.Split(new char[] { ',' });
            int[] numArray = new int[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                numArray[i] = this.GetInt(strArray[i], EditConsts.DefaultTabStop);
            }
            this.syntaxSettings.TabStops = numArray;
            Math.Max(Math.Min(this.GetInt(this.tbFontSize.Text, 10), EditConsts.MaxFontSize), 1);
        }

        private void StyleFromControl()
        {
            if (this.lbStyles.SelectedItem != null)
            {
                ILexStyle selectedStyle = this.GetSelectedStyle();
                selectedStyle.ForeColor = this.curForeColor;
                selectedStyle.BackColor = this.curBkColor;
                selectedStyle.FontStyle = this.curFontStyle;
                selectedStyle.Desc = this.curDesc;
                this.UpdateStyleControls();
            }
        }

        private void StyleSelected()
        {
            this.tbDescription.Enabled = this.syntaxSettings.IsDescriptionEnabled(this.lbStyles.SelectedIndex);
            this.laDescription.Enabled = this.syntaxSettings.IsDescriptionEnabled(this.lbStyles.SelectedIndex);
            this.gbFontAttributes.Enabled = this.syntaxSettings.IsFontStyleEnabled(this.lbStyles.SelectedIndex);
            this.laBackColor.Enabled = this.syntaxSettings.IsBackColorEnabled(this.lbStyles.SelectedIndex);
            this.cbBackColor.Enabled = this.syntaxSettings.IsBackColorEnabled(this.lbStyles.SelectedIndex);
            ILexStyle selectedStyle = this.GetSelectedStyle();
            if (selectedStyle != null)
            {
                this.laDescription.Enabled = true;
                this.tbDescription.Enabled = true;
                this.laForeColor.Enabled = selectedStyle.ForeColorEnabled;
                this.cbForeColor.Enabled = selectedStyle.ForeColorEnabled;
                this.laBackColor.Enabled = selectedStyle.BackColorEnabled;
                this.cbBackColor.Enabled = selectedStyle.BackColorEnabled;
                this.chbBold.Enabled = selectedStyle.BoldEnabled;
                this.chbItalic.Enabled = selectedStyle.ItalicEnabled;
                this.chbUnderline.Enabled = selectedStyle.UnderlineEnabled;
                this.curForeColor = selectedStyle.ForeColor;
                this.curBkColor = selectedStyle.BackColor;
                this.curFontStyle = selectedStyle.FontStyle;
                this.curDesc = selectedStyle.Desc;
            }
            this.UpdateStyleControls();
        }

        private void tbFontSize_Leave(object sender, EventArgs e)
        {
            this.UpdateActiveColorThemeFont();
        }

        private void tbGutterWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > ' ')
            {
                e.Handled = (e.KeyChar < '0') || (e.KeyChar > '9');
                if (!e.Handled)
                {
                    try
                    {
                        string text = this.tbGutterWidth.Text;
                        int.Parse(text.Insert(Math.Min(this.tbGutterWidth.SelectionStart, text.Length), e.KeyChar.ToString()));
                    }
                    catch
                    {
                        e.Handled = true;
                        OSUtils.MessageBeep();
                    }
                }
                else
                {
                    OSUtils.MessageBeep();
                }
            }
        }

        private void tbMarginPosition_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > ' ')
            {
                e.Handled = (e.KeyChar < '0') || (e.KeyChar > '9');
                if (!e.Handled)
                {
                    try
                    {
                        string text = this.tbMarginPosition.Text;
                        int.Parse(text.Insert(Math.Min(this.tbMarginPosition.SelectionStart, text.Length), e.KeyChar.ToString()));
                    }
                    catch
                    {
                        e.Handled = true;
                        OSUtils.MessageBeep();
                    }
                }
                else
                {
                    OSUtils.MessageBeep();
                }
            }
        }

        private void tbShowCommands_TextChanged(object sender, EventArgs e)
        {
            this.UpdateEventHandlers();
        }

        private void tvProperties_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Panel currentPanel = this.GetCurrentPanel();
            if (currentPanel != null)
            {
                currentPanel.Visible = true;
                currentPanel.BringToFront();
                this.UpdateImages();
            }
        }

        private void UpdateActiveColorThemeFont()
        {
            if (!this.isFontControlsUpdating && ((this.cbFontName.SelectedIndex != -1) && (this.tbFontSize.Text != null)))
            {
                this.syntaxSettings.Font = new Font(this.cbFontName.SelectedItem.ToString(), (float) this.GetInt(this.tbFontSize.Text, 10));
            }
        }

        private void UpdateEventHandlers()
        {
            this.lbEventHandlers.BeginUpdate();
            try
            {
                this.lbEventHandlers.Sorted = false;
                this.lbEventHandlers.Items.Clear();
                foreach (IKeyData data in this.syntaxSettings.EventData)
                {
                    string str = (data.Param != null) ? string.Format("{0}{1}", data.EventName, data.Param.ToString()) : data.EventName;
                    if (str != string.Empty)
                    {
                        if (this.tbShowCommands.Text != string.Empty)
                        {
                            if ((str.IndexOf(this.tbShowCommands.Text) >= 0) && (this.lbEventHandlers.Items.IndexOf(str) < 0))
                            {
                                this.lbEventHandlers.Items.Add(str);
                            }
                        }
                        else if (this.lbEventHandlers.Items.IndexOf(str) < 0)
                        {
                            this.lbEventHandlers.Items.Add(str);
                        }
                    }
                }
                this.lbEventHandlers.Sorted = true;
                if (this.lbEventHandlers.Items.Count > 0)
                {
                    this.lbEventHandlers.SelectedIndex = 0;
                }
            }
            finally
            {
                this.lbEventHandlers.EndUpdate();
            }
        }

        private void UpdateFontControls()
        {
            try
            {
                this.isFontControlsUpdating = true;
                this.cbFontName.SelectedIndex = this.cbFontName.Items.IndexOf(this.syntaxSettings.Font.Name);
                this.tbFontSize.Text = this.syntaxSettings.Font.Size.ToString();
            }
            finally
            {
                this.isFontControlsUpdating = false;
            }
            this.FontNameChanged(this, new EventArgs());
            this.OnStyleSelected(this, new EventArgs());
        }

        private void UpdateHiddenTabs(EditorSettingsTab hiddenTabs)
        {
            if ((hiddenTabs & EditorSettingsTab.General) != EditorSettingsTab.None)
            {
                this.rootNode.Nodes.Remove(this.generalNode);
            }
            if ((hiddenTabs & EditorSettingsTab.FontsAndColors) != EditorSettingsTab.None)
            {
                this.rootNode.Nodes.Remove(this.fontsNode);
            }
            if ((hiddenTabs & EditorSettingsTab.Additional) != EditorSettingsTab.None)
            {
                this.rootNode.Nodes.Remove(this.additionalNode);
            }
            if ((hiddenTabs & EditorSettingsTab.Keymapping) != EditorSettingsTab.None)
            {
                this.rootNode.Nodes.Remove(this.keyboardNode);
            }
        }

        private void UpdateImages()
        {
            if (this.tvProperties.SelectedNode == this.rootNode)
            {
                this.generalNode.ImageIndex = 2;
            }
            else
            {
                this.generalNode.ImageIndex = 3;
            }
        }

        private void UpdateShortcut(int index)
        {
            this.cbShortcuts.Text = string.Empty;
            this.cbShortcuts.Items.Clear();
            string str = this.lbEventHandlers.Items[index].ToString();
            foreach (IKeyData data in this.syntaxSettings.EventData)
            {
                if ((data.EventName != string.Empty) && str.StartsWith(data.EventName))
                {
                    string str2 = (str.Length > data.EventName.Length) ? str.Remove(0, data.EventName.Length) : string.Empty;
                    if ((data.Param == null) || (data.Param.ToString() == str2))
                    {
                        string str3 = this.ApplyKeyState(data);
                        string item = (str3 != string.Empty) ? string.Format("{0}, {1}", str3, this.KeyDataToString(data.Keys)) : this.KeyDataToString(data.Keys);
                        this.cbShortcuts.Items.Add(item);
                    }
                }
            }
            if (this.cbShortcuts.Items.Count > 0)
            {
                this.cbShortcuts.SelectedIndex = 0;
            }
        }

        private void UpdateStyleControls()
        {
            this.isControlUpdating = true;
            try
            {
                this.chbBold.Checked = (this.curFontStyle & FontStyle.Bold) != FontStyle.Regular;
                this.chbItalic.Checked = (this.curFontStyle & FontStyle.Italic) != FontStyle.Regular;
                this.chbUnderline.Checked = (this.curFontStyle & FontStyle.Underline) != FontStyle.Regular;
                this.tbDescription.Text = this.curDesc;
                this.laSampleText.Font = new Font(this.laSampleText.Font.Name, (float) ((int) this.laSampleText.Font.Size), this.curFontStyle);
                this.cbForeColor.SelectedColor = this.curForeColor;
                this.cbBackColor.SelectedColor = this.curBkColor;
                this.laSampleText.ForeColor = this.curForeColor;
                this.pnSampleText.BackColor = (this.curBkColor != Color.Empty) ? this.curBkColor : this.syntaxSettings.ColorThemes.ActiveTheme[StringConsts.WindowColorInternalName].BackColor;
                this.WriteSampleText();
            }
            finally
            {
                this.isControlUpdating = false;
            }
        }

        private void WriteSampleText()
        {
            this.laSampleText.Location = new Point((this.pnSampleText.Width - this.laSampleText.Width) / 2, (this.pnSampleText.Height - this.laSampleText.Height) / 2);
        }

        public ISyntaxSettings SyntaxSettings
        {
            get
            {
                return this.syntaxSettings;
            }
            set
            {
                this.syntaxSettings.Assign(value);
            }
        }
    }
}

