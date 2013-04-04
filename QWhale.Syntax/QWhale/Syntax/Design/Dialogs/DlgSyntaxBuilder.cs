namespace QWhale.Syntax.Design.Dialogs
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Design;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class DlgSyntaxBuilder : Form
    {
        private Button btBlockAdd;
        private Button btBlockDelete;
        private Button btBlockEdit;
        private Button btCancel;
        private Button btClear;
        private Button btLoadScheme;
        private Button btOk;
        private Button btreswordSetAdd;
        private Button btreswordSetDelete;
        private Button btreswordSetEdit;
        private Button btSave;
        private Button btStateAdd;
        private Button btStateDelete;
        private Button btStateEdit;
        private Button btStyleAdd;
        private Button btStyleDelete;
        private Button btStyleEdit;
        public ComboBox cbLeaveState;
        public ComboBox cbreswordStyle;
        public ComboBox cbStyle;
        private const int cCloseFolder = 1;
        public CheckBox chbBold;
        public CheckBox chbCaseSensitive;
        public CheckBox chbItalic;
        public CheckBox chbPlainText;
        public CheckBox chbStrikeout;
        public CheckBox chbUnderline;
        public ColorBox clbBkColor;
        public ColorBox clbForeColor;
        private ContextMenu cmExpressions;
        private ContextMenu cmLexer;
        private ColorDialog colorDialog1;
        private IContainer components;
        private const int cOpenFolder = 0;
        private const int cSelItem = 2;
        private const int cState = 4;
        private const int cUnSelItem = 3;
        public GroupBox gbFontStyles;
        private GroupBox gbreswordsProperties;
        public GroupBox gbStateProperties;
        public GroupBox gbStyleProperties;
        public GroupBox gbSyntaxBlockProperties;
        private TreeNode generalNode;
        private ImageList ilButtons;
        private ImageList imLexer;
        public Label laAuthor;
        public Label laBlockDesc;
        public Label laBlockExpr;
        private Label laBlockName;
        public Label laBlockreswords;
        public Label laCopyright;
        public Label laDescription;
        public Label laFileExt;
        public Label laFileType;
        public Label laLeaveState;
        private Label lareswordSetName;
        public Label lareswordStyle;
        public Label laSample;
        public Label laSchemeName;
        public Label laStateDesc;
        private Label laStateName;
        public Label laStyle;
        public Label laStyleBkColor;
        public Label laStyleDesc;
        public Label laStyleForeColor;
        public Label laStyleName;
        private ILexer lexer;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private MenuItem menuItem4;
        private MenuItem menuItem5;
        private OpenFileDialog openFileDialog;
        private Panel pnBlockButtons;
        private Panel pnButtons;
        public Panel pnGeneral;
        private Panel pnMain;
        private Panel pnreswordButtons;
        public Panel pnSample;
        private Panel pnStateButtons;
        public Panel pnStates;
        private Panel pnStyleButtons;
        public Panel pnStyles;
        private TreeNode rootNode;
        private SaveFileDialog saveFileDialog;
        private string sRemoveScroll;
        private TreeNode statesNode;
        private TreeNode stylesNode;
        public TextBox tbAuthor;
        public TextBox tbBlockDesc;
        private TextBox tbBlockName;
        public TextBox tbCopyright;
        public TextBox tbDescription;
        public TextBox tbExpressions;
        public TextBox tbFileExt;
        public TextBox tbFileType;
        public TextBox tbreswords;
        private TextBox tbreswordSetName;
        public TextBox tbSchemeName;
        public TextBox tbStateDesc;
        public TextBox tbStateName;
        public TextBox tbStyleDesc;
        public TextBox tbStyleName;
        private TabControl tcPanels;
        private TabPage tpGeneral;
        private TabPage tpStates;
        private TabPage tpStyles;
        private TreeView tvLexer;
        private bool updating;

        public DlgSyntaxBuilder()
        {
            this.sRemoveScroll = new string('x', 100);
            this.InitializeComponent();
            this.lexer = new QWhale.Syntax.Lexer.Lexer();
            this.rootNode = this.tvLexer.Nodes[0];
            this.generalNode = this.rootNode.Nodes[0];
            this.stylesNode = this.rootNode.Nodes[1];
            this.statesNode = this.rootNode.Nodes[2];
        }

        public DlgSyntaxBuilder(SyntaxBuilderEditor editor) : this()
        {
            base.StartPosition = FormStartPosition.CenterScreen;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.TopLevel = true;
            base.ShowInTaskbar = false;
        }

        private void AddBlockClick(object sender, EventArgs e)
        {
            ILexState lexState = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            string newSyntaxBlockName = this.GetNewSyntaxBlockName(lexState);
            lexState.SyntaxBlocks.AddLexSyntaxBlock().Name = newSyntaxBlockName;
            TreeNode stateNode = this.GetStateNode(this.tvLexer.SelectedNode);
            if (stateNode != null)
            {
                stateNode.Nodes.Add(new TreeNode(newSyntaxBlockName));
            }
            stateNode.Expand();
            this.tvLexer.SelectedNode = stateNode.LastNode;
        }

        private void AddreswordSetClick(object sender, EventArgs e)
        {
            ILexSyntaxBlock lexSyntaxBlock = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
            string newReswordSetName = this.GetNewReswordSetName(lexSyntaxBlock);
            lexSyntaxBlock.ReswordSets.AddLexReswordSet().Name = newReswordSetName;
            TreeNode syntaxBlockNode = this.GetSyntaxBlockNode(this.tvLexer.SelectedNode);
            if (syntaxBlockNode != null)
            {
                syntaxBlockNode.Nodes.Add(new TreeNode(newReswordSetName));
            }
            syntaxBlockNode.Expand();
            this.tvLexer.SelectedNode = syntaxBlockNode.LastNode;
        }

        private void AddStateClick(object sender, EventArgs e)
        {
            string newStateName = this.GetNewStateName();
            ILexState state = this.Scheme.States.AddLexState();
            TreeNode node = new TreeNode(newStateName) {
                ImageIndex = 4,
                SelectedImageIndex = 4
            };
            this.statesNode.Nodes.Add(node);
            this.statesNode.Expand();
            this.tvLexer.SelectedNode = this.statesNode.LastNode;
            state.Name = newStateName;
            this.UpdateStates(this.tvLexer.SelectedNode.Index);
        }

        private void AddStyleClick(object sender, EventArgs e)
        {
            string newStyleName = this.GetNewStyleName();
            ILexStyle style = this.Scheme.Styles.AddLexStyle();
            style.FontStyle = FontStyle.Regular;
            style.ForeColor = Color.Black;
            this.stylesNode.Nodes.Add(new TreeNode(newStyleName));
            this.stylesNode.Expand();
            this.tvLexer.SelectedNode = this.stylesNode.LastNode;
            style.Name = newStyleName;
            this.UpdateLexStyles();
        }

        private void btBlockEdit_Click(object sender, EventArgs e)
        {
            this.tbBlockName.Focus();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            this.ClearScheme();
        }

        private void btColor_Click(object sender, EventArgs e)
        {
            if ((this.colorDialog1.ShowDialog() == DialogResult.OK) && (this.GetNodeKind(this.tvLexer.SelectedNode) == NodeKind.nkStyle))
            {
                this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme).ForeColor = this.colorDialog1.Color;
            }
        }

        private void btLoadScheme_Click(object sender, EventArgs e)
        {
            this.LoadScheme();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void btreswordSetEdit_Click(object sender, EventArgs e)
        {
            this.tbreswordSetName.Focus();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            this.SaveScheme();
        }

        private void btStateEdit_Click(object sender, EventArgs e)
        {
            this.tbStateName.Focus();
        }

        private void btStyleEdit_Click(object sender, EventArgs e)
        {
            this.tbStyleName.Focus();
        }

        private void cbLeaveState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                ILexSyntaxBlock lexSyntaxBlock = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
                if ((lexSyntaxBlock != null) && (this.cbLeaveState.SelectedIndex >= 0))
                {
                    lexSyntaxBlock.LeaveState = this.Scheme.States[this.cbLeaveState.SelectedIndex];
                }
            }
        }

        private void cbreswordStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                ILexReswordSet lexreswordSet = this.GetLexreswordSet(this.tvLexer.SelectedNode, this.Scheme);
                if ((lexreswordSet != null) && (this.cbreswordStyle.SelectedIndex >= 0))
                {
                    lexreswordSet.ReswordStyle = this.Scheme.Styles[this.cbreswordStyle.SelectedIndex];
                }
            }
        }

        private void cbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                ILexSyntaxBlock lexSyntaxBlock = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
                if ((lexSyntaxBlock != null) && (this.cbStyle.SelectedIndex >= 0))
                {
                    lexSyntaxBlock.Style = this.Scheme.Styles[this.cbStyle.SelectedIndex];
                }
            }
        }

        private void chbBold_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                this.UpdateFontStyle();
            }
        }

        private void chbCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                switch (this.GetNodeKind(this.tvLexer.SelectedNode))
                {
                    case NodeKind.nkState:
                    case NodeKind.nkSyntaxBlock:
                        this.GetLexState(this.tvLexer.SelectedNode, this.Scheme).CaseSensitive = this.chbCaseSensitive.Checked;
                        break;
                }
            }
        }

        private void chbCaseSensitive_TextChanged(object sender, EventArgs e)
        {
            ILexState lexState = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            if (lexState != null)
            {
                lexState.CaseSensitive = this.chbCaseSensitive.Checked;
            }
        }

        private void chbItalic_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                this.UpdateFontStyle();
            }
        }

        private void chbPlainText_CheckedChanged(object sender, EventArgs e)
        {
            ILexStyle lexStyle = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (lexStyle != null)
            {
                lexStyle.PlainText = this.chbPlainText.Checked;
            }
        }

        private void chbStrikeout_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                this.UpdateFontStyle();
            }
        }

        private void chbUnderline_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                this.UpdateFontStyle();
            }
        }

        private void clbBkColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ILexStyle lexStyle = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (lexStyle != null)
            {
                lexStyle.BackColor = this.clbBkColor.SelectedColor;
            }
            this.UpdateSample();
        }

        private void clbForeColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ILexStyle lexStyle = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (lexStyle != null)
            {
                lexStyle.ForeColor = this.clbForeColor.SelectedColor;
            }
            this.UpdateSample();
        }

        private void ClearScheme()
        {
            this.Scheme.Clear();
            this.statesNode.Nodes.Clear();
            this.stylesNode.Nodes.Clear();
            this.Text = SyntaxBuilderConsts.SyntaxFormCaption;
        }

        private void DeleteBlockClick(object sender, EventArgs e)
        {
            ILexSyntaxBlock lexSyntaxBlock = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
            this.tvLexer.SelectedNode.Remove();
            if (lexSyntaxBlock != null)
            {
                LexState lexState = (LexState) this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
                lexState.SyntaxBlocks.Remove(lexSyntaxBlock);
            }
        }

        private void DeletereswordSetClick(object sender, EventArgs e)
        {
            ILexReswordSet lexreswordSet = this.GetLexreswordSet(this.tvLexer.SelectedNode, this.Scheme);
            this.tvLexer.SelectedNode.Remove();
            if (lexreswordSet != null)
            {
                LexSyntaxBlock lexSyntaxBlock = (LexSyntaxBlock) this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
                lexSyntaxBlock.ReswordSets.Remove(lexreswordSet);
            }
        }

        private void DeleteStateClick(object sender, EventArgs e)
        {
            ILexState lexState = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            this.GetStateNode(this.tvLexer.SelectedNode).Remove();
            if (lexState != null)
            {
                this.Scheme.States.Remove(lexState);
            }
            TreeNode stateNode = this.GetStateNode(this.tvLexer.SelectedNode);
            this.UpdateStates((stateNode != null) ? stateNode.Index : -1);
        }

        private void DeleteStyleClick(object sender, EventArgs e)
        {
            ILexStyle lexStyle = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            TreeNode styleNode = this.GetStyleNode(this.tvLexer.SelectedNode);
            if (styleNode != null)
            {
                styleNode.Remove();
            }
            if (lexStyle != null)
            {
                this.Scheme.Styles.Remove(lexStyle);
            }
            if (!this.tvLexer.Focused)
            {
                this.tvLexer.Focus();
            }
            this.UpdateLexStyles();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DlgSyntaxBuilder_Closing(object sender, CancelEventArgs e)
        {
            this.DoExit();
        }

        private void DoExit()
        {
            if ((base.DialogResult == DialogResult.Cancel) && (MessageBox.Show(SyntaxBuilderConsts.CancelText, SyntaxBuilderConsts.CancelCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes))
            {
                base.DialogResult = DialogResult.None;
            }
        }

        private Panel GetCurrentPanel(NodeKind kind)
        {
            switch (kind)
            {
                case NodeKind.nkStyles:
                case NodeKind.nkStyle:
                    return this.pnStyles;

                case NodeKind.nkStates:
                case NodeKind.nkState:
                case NodeKind.nkSyntaxBlock:
                case NodeKind.nkreswordSet:
                    return this.pnStates;

                case NodeKind.nkGeneral:
                    return this.pnGeneral;
            }
            return this.pnGeneral;
        }

        private int GetFirstNumber(IList<int> list)
        {
            int num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == num)
                {
                    num++;
                }
            }
            return num;
        }

        private int GetFirstNumber(string[] list, string start)
        {
            List<int> list2 = new List<int>();
            for (int i = 0; i < list.Length; i++)
            {
                string str = list[i];
                if ((str != null) && str.StartsWith(start))
                {
                    string s = str.Substring(start.Length);
                    try
                    {
                        int item = int.Parse(s);
                        list2.Add(item);
                    }
                    catch
                    {
                    }
                }
            }
            list2.Sort();
            int num3 = 0;
            for (int j = 0; j < list2.Count; j++)
            {
                if (list2[j] == num3)
                {
                    num3++;
                }
            }
            return num3;
        }

        private TreeNode GetFolderNode()
        {
            TreeNode selectedNode = this.tvLexer.SelectedNode;
            while (selectedNode != null)
            {
                if ((selectedNode.ImageIndex != 0) && (selectedNode.ImageIndex != 1))
                {
                    selectedNode = selectedNode.Parent;
                }
                else
                {
                    return selectedNode;
                }
            }
            return selectedNode;
        }

        private int GetLeaveStateIndex(ILexSyntaxBlock block)
        {
            if ((block != null) && (block.LeaveState != null))
            {
                for (int i = 0; i < this.cbLeaveState.Items.Count; i++)
                {
                    if (string.Compare(this.cbLeaveState.Items[i].ToString(), block.LeaveState.Name) == 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private ILexReswordSet GetLexreswordSet(TreeNode node, ILexScheme scheme)
        {
            if (this.GetNodeKind(node) != NodeKind.nkreswordSet)
            {
                return null;
            }
            ILexSyntaxBlock lexSyntaxBlock = this.GetLexSyntaxBlock(node.Parent, scheme);
            if (lexSyntaxBlock == null)
            {
                return null;
            }
            return lexSyntaxBlock.ReswordSets[node.Index];
        }

        protected ILexState GetLexState(TreeNode node, ILexScheme scheme)
        {
            switch (this.GetNodeKind(node))
            {
                case NodeKind.nkStates:
                    if ((this.statesNode.Nodes.Count <= 0) || (scheme.States.Count <= 0))
                    {
                        return null;
                    }
                    return scheme.States[0];

                case NodeKind.nkState:
                    return scheme.States[node.Index];

                case NodeKind.nkSyntaxBlock:
                    return scheme.States[node.Parent.Index];

                case NodeKind.nkreswordSet:
                    return scheme.States[node.Parent.Parent.Index];
            }
            return null;
        }

        protected ILexStyle GetLexStyle(TreeNode node, ILexScheme scheme)
        {
            switch (this.GetNodeKind(node))
            {
                case NodeKind.nkStyles:
                    if ((node.Nodes.Count <= 0) || (scheme.Styles.Count <= 0))
                    {
                        return null;
                    }
                    return scheme.Styles[0];

                case NodeKind.nkStyle:
                    return scheme.Styles[node.Index];
            }
            return null;
        }

        private ILexSyntaxBlock GetLexSyntaxBlock(TreeNode node, ILexScheme scheme)
        {
            NodeKind nodeKind = this.GetNodeKind(node);
            if ((nodeKind != NodeKind.nkSyntaxBlock) && (nodeKind != NodeKind.nkreswordSet))
            {
                return null;
            }
            ILexState lexState = this.GetLexState(node.Parent, scheme);
            if (nodeKind == NodeKind.nkreswordSet)
            {
                node = node.Parent;
            }
            if (lexState == null)
            {
                return null;
            }
            return lexState.SyntaxBlocks[node.Index];
        }

        private string GetNewReswordSetName(ILexSyntaxBlock lexBlock)
        {
            ILexReswordSets reswordSets = lexBlock.ReswordSets;
            string[] list = new string[reswordSets.Count];
            for (int i = 0; i < reswordSets.Count; i++)
            {
                list[i] = reswordSets[i].Name;
            }
            int firstNumber = this.GetFirstNumber(list, SyntaxBuilderConsts.ReswordSetText);
            return (SyntaxBuilderConsts.ReswordSetText + firstNumber.ToString());
        }

        private string GetNewStateName()
        {
            string[] list = new string[this.Scheme.States.Count];
            for (int i = 0; i < this.Scheme.States.Count; i++)
            {
                list[i] = this.Scheme.States[i].Name;
            }
            int firstNumber = this.GetFirstNumber(list, SyntaxBuilderConsts.StateText);
            return (SyntaxBuilderConsts.StateText + firstNumber.ToString());
        }

        private string GetNewStyleName()
        {
            string[] list = new string[this.Scheme.Styles.Count];
            for (int i = 0; i < this.Scheme.Styles.Count; i++)
            {
                list[i] = this.Scheme.Styles[i].Name;
            }
            int firstNumber = this.GetFirstNumber(list, SyntaxBuilderConsts.StyleText);
            return (SyntaxBuilderConsts.StyleText + firstNumber.ToString());
        }

        private string GetNewSyntaxBlockName(ILexState lexState)
        {
            ILexSyntaxBlocks syntaxBlocks = lexState.SyntaxBlocks;
            string[] list = new string[syntaxBlocks.Count];
            for (int i = 0; i < syntaxBlocks.Count; i++)
            {
                list[i] = syntaxBlocks[i].Name;
            }
            int firstNumber = this.GetFirstNumber(list, SyntaxBuilderConsts.BlockText);
            return (SyntaxBuilderConsts.BlockText + firstNumber.ToString());
        }

        private NodeKind GetNodeKind(TreeNode node)
        {
            int nodeLevel = this.GetNodeLevel(node);
            TreeNode parent = node.Parent;
            switch (nodeLevel)
            {
                case 0:
                    return NodeKind.nkGeneral;

                case 1:
                    switch (node.Index)
                    {
                        case 0:
                            return NodeKind.nkGeneral;

                        case 1:
                            return NodeKind.nkStyles;

                        case 2:
                            return NodeKind.nkStates;
                    }
                    return NodeKind.nkNone;

                case 2:
                    if (parent != this.stylesNode)
                    {
                        if (parent == this.statesNode)
                        {
                            return NodeKind.nkState;
                        }
                        return NodeKind.nkNone;
                    }
                    return NodeKind.nkStyle;

                case 3:
                    return NodeKind.nkSyntaxBlock;

                case 4:
                    return NodeKind.nkreswordSet;
            }
            return NodeKind.nkNone;
        }

        private int GetNodeLevel(TreeNode node)
        {
            if (node == null)
            {
                return -1;
            }
            TreeNode parent = node;
            int num = -1;
            while (parent != null)
            {
                parent = parent.Parent;
                num++;
            }
            return num;
        }

        private TreeNode GetParent(TreeNode node)
        {
            TreeNode parent = node;
            while (parent.Parent != null)
            {
                parent = parent.Parent;
            }
            return parent;
        }

        private int GetreswordStyleIndex(ILexReswordSet reswordSet)
        {
            if ((reswordSet != null) && (reswordSet.ReswordStyle != null))
            {
                for (int i = 0; i < this.cbreswordStyle.Items.Count; i++)
                {
                    if (string.Compare(this.cbreswordStyle.Items[i].ToString(), reswordSet.ReswordStyle.Name) == 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private string GetSampleExpression(string text)
        {
            string str = text;
            int index = str.IndexOf(" ");
            if (index >= 0)
            {
                str = str.Remove(0, index).Trim();
            }
            return str;
        }

        private TreeNode GetStateNode(TreeNode node)
        {
            switch (this.GetNodeKind(node))
            {
                case NodeKind.nkStates:
                    if (this.statesNode.Nodes.Count <= 0)
                    {
                        return null;
                    }
                    return node.Nodes[0];

                case NodeKind.nkState:
                    return node;

                case NodeKind.nkSyntaxBlock:
                    return node.Parent;

                case NodeKind.nkreswordSet:
                    return node.Parent.Parent;
            }
            return null;
        }

        private int GetStyleIndex(ILexSyntaxBlock block)
        {
            if ((block != null) && (block.Style != null))
            {
                for (int i = 0; i < this.cbStyle.Items.Count; i++)
                {
                    if (string.Compare(this.cbStyle.Items[i].ToString(), block.Style.Name) == 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private TreeNode GetStyleNode(TreeNode node)
        {
            switch (this.GetNodeKind(node))
            {
                case NodeKind.nkStyles:
                    if (node.Nodes.Count <= 0)
                    {
                        return null;
                    }
                    return node.Nodes[0];

                case NodeKind.nkStyle:
                    return node;
            }
            return null;
        }

        private TreeNode GetSyntaxBlockNode(TreeNode node)
        {
            switch (this.GetNodeKind(node))
            {
                case NodeKind.nkSyntaxBlock:
                    return node;

                case NodeKind.nkreswordSet:
                    return node.Parent;
            }
            return null;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            TreeNode node = new TreeNode("General", 3, 2);
            TreeNode node2 = new TreeNode("Styles", 1, 0);
            TreeNode node3 = new TreeNode("States", 1, 0);
            TreeNode node4 = new TreeNode("Syntax Scheme Editor", 0, 0, new TreeNode[] { node, node2, node3 });
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgSyntaxBuilder));
            this.pnButtons = new Panel();
            this.btCancel = new Button();
            this.btOk = new Button();
            this.btLoadScheme = new Button();
            this.btClear = new Button();
            this.btSave = new Button();
            this.tvLexer = new TreeView();
            this.imLexer = new ImageList(this.components);
            this.pnMain = new Panel();
            this.tcPanels = new TabControl();
            this.tpGeneral = new TabPage();
            this.pnGeneral = new Panel();
            this.laFileExt = new Label();
            this.tbFileExt = new TextBox();
            this.laFileType = new Label();
            this.tbFileType = new TextBox();
            this.laCopyright = new Label();
            this.laDescription = new Label();
            this.laSchemeName = new Label();
            this.laAuthor = new Label();
            this.tbCopyright = new TextBox();
            this.tbDescription = new TextBox();
            this.tbSchemeName = new TextBox();
            this.tbAuthor = new TextBox();
            this.tpStyles = new TabPage();
            this.pnStyles = new Panel();
            this.pnStyleButtons = new Panel();
            this.btStyleDelete = new Button();
            this.ilButtons = new ImageList(this.components);
            this.btStyleEdit = new Button();
            this.btStyleAdd = new Button();
            this.pnSample = new Panel();
            this.laSample = new Label();
            this.gbStyleProperties = new GroupBox();
            this.chbPlainText = new CheckBox();
            this.tbStyleName = new TextBox();
            this.laStyleName = new Label();
            this.laStyleBkColor = new Label();
            this.laStyleForeColor = new Label();
            this.tbStyleDesc = new TextBox();
            this.laStyleDesc = new Label();
            this.clbBkColor = new ColorBox(this.components);
            this.clbForeColor = new ColorBox(this.components);
            this.gbFontStyles = new GroupBox();
            this.chbStrikeout = new CheckBox();
            this.chbItalic = new CheckBox();
            this.chbUnderline = new CheckBox();
            this.chbBold = new CheckBox();
            this.tpStates = new TabPage();
            this.pnStates = new Panel();
            this.gbreswordsProperties = new GroupBox();
            this.tbreswordSetName = new TextBox();
            this.lareswordSetName = new Label();
            this.pnreswordButtons = new Panel();
            this.btreswordSetDelete = new Button();
            this.btreswordSetEdit = new Button();
            this.btreswordSetAdd = new Button();
            this.cbreswordStyle = new ComboBox();
            this.tbreswords = new TextBox();
            this.laBlockreswords = new Label();
            this.lareswordStyle = new Label();
            this.gbSyntaxBlockProperties = new GroupBox();
            this.tbExpressions = new TextBox();
            this.tbBlockName = new TextBox();
            this.laBlockName = new Label();
            this.pnBlockButtons = new Panel();
            this.btBlockDelete = new Button();
            this.btBlockEdit = new Button();
            this.btBlockAdd = new Button();
            this.laBlockExpr = new Label();
            this.tbBlockDesc = new TextBox();
            this.laBlockDesc = new Label();
            this.laLeaveState = new Label();
            this.laStyle = new Label();
            this.cbLeaveState = new ComboBox();
            this.cbStyle = new ComboBox();
            this.gbStateProperties = new GroupBox();
            this.pnStateButtons = new Panel();
            this.btStateDelete = new Button();
            this.btStateEdit = new Button();
            this.btStateAdd = new Button();
            this.tbStateName = new TextBox();
            this.laStateName = new Label();
            this.laStateDesc = new Label();
            this.tbStateDesc = new TextBox();
            this.chbCaseSensitive = new CheckBox();
            this.colorDialog1 = new ColorDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.cmLexer = new ContextMenu();
            this.openFileDialog = new OpenFileDialog();
            this.cmExpressions = new ContextMenu();
            this.menuItem1 = new MenuItem();
            this.menuItem2 = new MenuItem();
            this.menuItem3 = new MenuItem();
            this.menuItem4 = new MenuItem();
            this.menuItem5 = new MenuItem();
            this.pnButtons.SuspendLayout();
            this.pnMain.SuspendLayout();
            this.tcPanels.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.pnGeneral.SuspendLayout();
            this.tpStyles.SuspendLayout();
            this.pnStyles.SuspendLayout();
            this.pnStyleButtons.SuspendLayout();
            this.pnSample.SuspendLayout();
            this.gbStyleProperties.SuspendLayout();
            this.gbFontStyles.SuspendLayout();
            this.tpStates.SuspendLayout();
            this.pnStates.SuspendLayout();
            this.gbreswordsProperties.SuspendLayout();
            this.pnreswordButtons.SuspendLayout();
            this.gbSyntaxBlockProperties.SuspendLayout();
            this.pnBlockButtons.SuspendLayout();
            this.gbStateProperties.SuspendLayout();
            this.pnStateButtons.SuspendLayout();
            base.SuspendLayout();
            this.pnButtons.BackColor = SystemColors.Control;
            this.pnButtons.BorderStyle = BorderStyle.Fixed3D;
            this.pnButtons.Controls.Add(this.btCancel);
            this.pnButtons.Controls.Add(this.btOk);
            this.pnButtons.Controls.Add(this.btLoadScheme);
            this.pnButtons.Controls.Add(this.btClear);
            this.pnButtons.Controls.Add(this.btSave);
            this.pnButtons.Dock = DockStyle.Bottom;
            this.pnButtons.Location = new Point(0, 480);
            this.pnButtons.Name = "pnButtons";
            this.pnButtons.Size = new Size(0x248, 40);
            this.pnButtons.TabIndex = 12;
            this.btCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btCancel.Location = new Point(0x1f6, 10);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new Size(0x4b, 0x17);
            this.btCancel.TabIndex = 0x21;
            this.btCancel.Text = "Cancel";
            this.btCancel.Click += new EventHandler(this.btCancel_Click);
            this.btOk.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btOk.DialogResult = DialogResult.OK;
            this.btOk.Location = new Point(0x1a6, 10);
            this.btOk.Name = "btOk";
            this.btOk.Size = new Size(0x4b, 0x17);
            this.btOk.TabIndex = 0x20;
            this.btOk.Text = "&OK";
            this.btOk.Click += new EventHandler(this.btOk_Click);
            this.btLoadScheme.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btLoadScheme.Location = new Point(0xa6, 10);
            this.btLoadScheme.Name = "btLoadScheme";
            this.btLoadScheme.Size = new Size(0x4b, 0x17);
            this.btLoadScheme.TabIndex = 0x1c;
            this.btLoadScheme.Text = "Load";
            this.btLoadScheme.Click += new EventHandler(this.btLoadScheme_Click);
            this.btClear.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btClear.Location = new Point(0x146, 10);
            this.btClear.Name = "btClear";
            this.btClear.Size = new Size(0x4b, 0x17);
            this.btClear.TabIndex = 0x1f;
            this.btClear.Text = "Clear";
            this.btClear.Click += new EventHandler(this.btClear_Click);
            this.btSave.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btSave.Location = new Point(0xf6, 10);
            this.btSave.Name = "btSave";
            this.btSave.Size = new Size(0x4b, 0x17);
            this.btSave.TabIndex = 0x1d;
            this.btSave.Text = "Save";
            this.btSave.Click += new EventHandler(this.btSave_Click);
            this.tvLexer.BackColor = SystemColors.Control;
            this.tvLexer.Dock = DockStyle.Left;
            this.tvLexer.HideSelection = false;
            this.tvLexer.ImageIndex = 3;
            this.tvLexer.ImageList = this.imLexer;
            this.tvLexer.LabelEdit = true;
            this.tvLexer.Location = new Point(0, 0);
            this.tvLexer.Name = "tvLexer";
            node.ImageIndex = 3;
            node.Name = "";
            node.SelectedImageIndex = 2;
            node.Text = "General";
            node2.ImageIndex = 1;
            node2.Name = "";
            node2.SelectedImageIndex = 0;
            node2.Text = "Styles";
            node3.ImageIndex = 1;
            node3.Name = "";
            node3.SelectedImageIndex = 0;
            node3.Text = "States";
            node4.ImageIndex = 0;
            node4.Name = "";
            node4.SelectedImageIndex = 0;
            node4.Text = "Syntax Scheme Editor";
            this.tvLexer.Nodes.AddRange(new TreeNode[] { node4 });
            this.tvLexer.SelectedImageIndex = 2;
            this.tvLexer.ShowLines = false;
            this.tvLexer.ShowRootLines = false;
            this.tvLexer.Size = new Size(0xb8, 480);
            this.tvLexer.TabIndex = 0x22;
            this.tvLexer.AfterCollapse += new TreeViewEventHandler(this.tvLexer_AfterCollapse);
            this.tvLexer.AfterLabelEdit += new NodeLabelEditEventHandler(this.tvLexer_AfterLabelEdit);
            this.tvLexer.AfterSelect += new TreeViewEventHandler(this.tvLexer_AfterSelect);
            this.tvLexer.MouseDown += new MouseEventHandler(this.tvLexer_MouseDown);
            this.tvLexer.BeforeLabelEdit += new NodeLabelEditEventHandler(this.tvLexer_BeforeLabelEdit);
            this.tvLexer.AfterExpand += new TreeViewEventHandler(this.tvLexer_AfterExpand);
            this.imLexer.ImageStream = (ImageListStreamer) manager.GetObject("imLexer.ImageStream");
            this.imLexer.TransparentColor = Color.Red;
            this.imLexer.Images.SetKeyName(0, "");
            this.imLexer.Images.SetKeyName(1, "");
            this.imLexer.Images.SetKeyName(2, "");
            this.imLexer.Images.SetKeyName(3, "");
            this.imLexer.Images.SetKeyName(4, "");
            this.pnMain.BackColor = SystemColors.Control;
            this.pnMain.Controls.Add(this.tcPanels);
            this.pnMain.Dock = DockStyle.Fill;
            this.pnMain.Location = new Point(0xb8, 0);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new Size(400, 480);
            this.pnMain.TabIndex = 0x23;
            this.tcPanels.Controls.Add(this.tpGeneral);
            this.tcPanels.Controls.Add(this.tpStyles);
            this.tcPanels.Controls.Add(this.tpStates);
            this.tcPanels.Location = new Point(0, 0);
            this.tcPanels.Name = "tcPanels";
            this.tcPanels.SelectedIndex = 0;
            this.tcPanels.Size = new Size(0x18a, 0x1e8);
            this.tcPanels.TabIndex = 0;
            this.tcPanels.Visible = false;
            this.tpGeneral.Controls.Add(this.pnGeneral);
            this.tpGeneral.Location = new Point(4, 0x16);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Size = new Size(0x182, 0x1ce);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.pnGeneral.Controls.Add(this.laFileExt);
            this.pnGeneral.Controls.Add(this.tbFileExt);
            this.pnGeneral.Controls.Add(this.laFileType);
            this.pnGeneral.Controls.Add(this.tbFileType);
            this.pnGeneral.Controls.Add(this.laCopyright);
            this.pnGeneral.Controls.Add(this.laDescription);
            this.pnGeneral.Controls.Add(this.laSchemeName);
            this.pnGeneral.Controls.Add(this.laAuthor);
            this.pnGeneral.Controls.Add(this.tbCopyright);
            this.pnGeneral.Controls.Add(this.tbDescription);
            this.pnGeneral.Controls.Add(this.tbSchemeName);
            this.pnGeneral.Controls.Add(this.tbAuthor);
            this.pnGeneral.Location = new Point(0, 0);
            this.pnGeneral.Name = "pnGeneral";
            this.pnGeneral.Size = new Size(0x180, 0x1c0);
            this.pnGeneral.TabIndex = 1;
            this.pnGeneral.Visible = false;
            this.laFileExt.AutoSize = true;
            this.laFileExt.Location = new Point(0x10, 0x100);
            this.laFileExt.Name = "laFileExt";
            this.laFileExt.Size = new Size(0x4b, 13);
            this.laFileExt.TabIndex = 10;
            this.laFileExt.Text = "File Extension:";
            this.tbFileExt.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbFileExt.Location = new Point(0x10, 280);
            this.tbFileExt.Name = "tbFileExt";
            this.tbFileExt.Size = new Size(360, 20);
            this.tbFileExt.TabIndex = 11;
            this.laFileType.AutoSize = true;
            this.laFileType.Location = new Point(0x10, 0xca);
            this.laFileType.Name = "laFileType";
            this.laFileType.Size = new Size(0x35, 13);
            this.laFileType.TabIndex = 8;
            this.laFileType.Text = "File Type:";
            this.tbFileType.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbFileType.Location = new Point(0x10, 0xe2);
            this.tbFileType.Name = "tbFileType";
            this.tbFileType.Size = new Size(360, 20);
            this.tbFileType.TabIndex = 9;
            this.laCopyright.AutoSize = true;
            this.laCopyright.Location = new Point(0x10, 0x98);
            this.laCopyright.Name = "laCopyright";
            this.laCopyright.Size = new Size(0x36, 13);
            this.laCopyright.TabIndex = 6;
            this.laCopyright.Text = "Copyright:";
            this.laDescription.AutoSize = true;
            this.laDescription.Location = new Point(0x10, 0x68);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new Size(0x3f, 13);
            this.laDescription.TabIndex = 4;
            this.laDescription.Text = "Description:";
            this.laSchemeName.AutoSize = true;
            this.laSchemeName.Location = new Point(0x10, 0x38);
            this.laSchemeName.Name = "laSchemeName";
            this.laSchemeName.Size = new Size(80, 13);
            this.laSchemeName.TabIndex = 2;
            this.laSchemeName.Text = "Scheme Name:";
            this.laAuthor.AutoSize = true;
            this.laAuthor.Location = new Point(0x10, 8);
            this.laAuthor.Name = "laAuthor";
            this.laAuthor.Size = new Size(0x29, 13);
            this.laAuthor.TabIndex = 0;
            this.laAuthor.Text = "Author:";
            this.tbCopyright.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbCopyright.Location = new Point(0x10, 0xb0);
            this.tbCopyright.Name = "tbCopyright";
            this.tbCopyright.Size = new Size(360, 20);
            this.tbCopyright.TabIndex = 7;
            this.tbDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbDescription.Location = new Point(0x10, 0x80);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new Size(360, 20);
            this.tbDescription.TabIndex = 5;
            this.tbSchemeName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbSchemeName.Location = new Point(0x10, 80);
            this.tbSchemeName.Name = "tbSchemeName";
            this.tbSchemeName.Size = new Size(360, 20);
            this.tbSchemeName.TabIndex = 3;
            this.tbAuthor.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbAuthor.Location = new Point(0x10, 0x20);
            this.tbAuthor.Name = "tbAuthor";
            this.tbAuthor.Size = new Size(360, 20);
            this.tbAuthor.TabIndex = 1;
            this.tpStyles.Controls.Add(this.pnStyles);
            this.tpStyles.Location = new Point(4, 0x16);
            this.tpStyles.Name = "tpStyles";
            this.tpStyles.Size = new Size(0x182, 0x1ce);
            this.tpStyles.TabIndex = 1;
            this.tpStyles.Text = "Styles";
            this.pnStyles.BackColor = SystemColors.Control;
            this.pnStyles.Controls.Add(this.pnStyleButtons);
            this.pnStyles.Controls.Add(this.pnSample);
            this.pnStyles.Controls.Add(this.gbStyleProperties);
            this.pnStyles.Location = new Point(0, 0);
            this.pnStyles.Name = "pnStyles";
            this.pnStyles.Size = new Size(0x1c8, 480);
            this.pnStyles.TabIndex = 4;
            this.pnStyles.Visible = false;
            this.pnStyleButtons.Controls.Add(this.btStyleDelete);
            this.pnStyleButtons.Controls.Add(this.btStyleEdit);
            this.pnStyleButtons.Controls.Add(this.btStyleAdd);
            this.pnStyleButtons.Location = new Point(8, 0x130);
            this.pnStyleButtons.Name = "pnStyleButtons";
            this.pnStyleButtons.Size = new Size(0x34, 0x12);
            this.pnStyleButtons.TabIndex = 7;
            this.btStyleDelete.ImageIndex = 2;
            this.btStyleDelete.ImageList = this.ilButtons;
            this.btStyleDelete.Location = new Point(0x11, 0);
            this.btStyleDelete.Name = "btStyleDelete";
            this.btStyleDelete.Size = new Size(0x11, 0x11);
            this.btStyleDelete.TabIndex = 1;
            this.btStyleDelete.Click += new EventHandler(this.DeleteStyleClick);
            this.ilButtons.ImageStream = (ImageListStreamer) manager.GetObject("ilButtons.ImageStream");
            this.ilButtons.TransparentColor = SystemColors.Control;
            this.ilButtons.Images.SetKeyName(0, "");
            this.ilButtons.Images.SetKeyName(1, "");
            this.ilButtons.Images.SetKeyName(2, "");
            this.btStyleEdit.ImageIndex = 1;
            this.btStyleEdit.ImageList = this.ilButtons;
            this.btStyleEdit.Location = new Point(0x22, 0);
            this.btStyleEdit.Name = "btStyleEdit";
            this.btStyleEdit.Size = new Size(0x11, 0x11);
            this.btStyleEdit.TabIndex = 2;
            this.btStyleEdit.Click += new EventHandler(this.btStyleEdit_Click);
            this.btStyleAdd.ImageIndex = 0;
            this.btStyleAdd.ImageList = this.ilButtons;
            this.btStyleAdd.Location = new Point(0, 0);
            this.btStyleAdd.Name = "btStyleAdd";
            this.btStyleAdd.Size = new Size(0x11, 0x11);
            this.btStyleAdd.TabIndex = 0;
            this.btStyleAdd.Click += new EventHandler(this.AddStyleClick);
            this.pnSample.BorderStyle = BorderStyle.Fixed3D;
            this.pnSample.Controls.Add(this.laSample);
            this.pnSample.Location = new Point(8, 0xe8);
            this.pnSample.Name = "pnSample";
            this.pnSample.Size = new Size(0x17a, 0x40);
            this.pnSample.TabIndex = 6;
            this.laSample.AutoSize = true;
            this.laSample.BorderStyle = BorderStyle.Fixed3D;
            this.laSample.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
            this.laSample.Location = new Point(120, 0x10);
            this.laSample.Name = "laSample";
            this.laSample.Size = new Size(0x7d, 0x1f);
            this.laSample.TabIndex = 0;
            this.laSample.Text = "AaBbYyZz";
            this.gbStyleProperties.Controls.Add(this.chbPlainText);
            this.gbStyleProperties.Controls.Add(this.tbStyleName);
            this.gbStyleProperties.Controls.Add(this.laStyleName);
            this.gbStyleProperties.Controls.Add(this.laStyleBkColor);
            this.gbStyleProperties.Controls.Add(this.laStyleForeColor);
            this.gbStyleProperties.Controls.Add(this.tbStyleDesc);
            this.gbStyleProperties.Controls.Add(this.laStyleDesc);
            this.gbStyleProperties.Controls.Add(this.clbBkColor);
            this.gbStyleProperties.Controls.Add(this.clbForeColor);
            this.gbStyleProperties.Controls.Add(this.gbFontStyles);
            this.gbStyleProperties.Location = new Point(8, 8);
            this.gbStyleProperties.Name = "gbStyleProperties";
            this.gbStyleProperties.Size = new Size(380, 0xd8);
            this.gbStyleProperties.TabIndex = 5;
            this.gbStyleProperties.TabStop = false;
            this.gbStyleProperties.Text = "Style Properties";
            this.chbPlainText.Location = new Point(0x100, 160);
            this.chbPlainText.Name = "chbPlainText";
            this.chbPlainText.Size = new Size(80, 0x18);
            this.chbPlainText.TabIndex = 8;
            this.chbPlainText.Text = "Plain Text";
            this.tbStyleName.Location = new Point(0x10, 0x20);
            this.tbStyleName.Name = "tbStyleName";
            this.tbStyleName.Size = new Size(0xd8, 20);
            this.tbStyleName.TabIndex = 1;
            this.laStyleName.AutoSize = true;
            this.laStyleName.Location = new Point(0x10, 0x10);
            this.laStyleName.Name = "laStyleName";
            this.laStyleName.Size = new Size(0x26, 13);
            this.laStyleName.TabIndex = 0;
            this.laStyleName.Text = "Name:";
            this.laStyleBkColor.AutoSize = true;
            this.laStyleBkColor.Location = new Point(0x10, 0x90);
            this.laStyleBkColor.Name = "laStyleBkColor";
            this.laStyleBkColor.Size = new Size(0x3e, 13);
            this.laStyleBkColor.TabIndex = 6;
            this.laStyleBkColor.Text = "Back Color:";
            this.laStyleForeColor.AutoSize = true;
            this.laStyleForeColor.Location = new Point(0x10, 0x62);
            this.laStyleForeColor.Name = "laStyleForeColor";
            this.laStyleForeColor.Size = new Size(0x3a, 13);
            this.laStyleForeColor.TabIndex = 4;
            this.laStyleForeColor.Text = "Fore Color:";
            this.tbStyleDesc.Location = new Point(0x10, 0x48);
            this.tbStyleDesc.Name = "tbStyleDesc";
            this.tbStyleDesc.Size = new Size(0xd8, 20);
            this.tbStyleDesc.TabIndex = 3;
            this.laStyleDesc.AutoSize = true;
            this.laStyleDesc.Location = new Point(0x10, 0x38);
            this.laStyleDesc.Name = "laStyleDesc";
            this.laStyleDesc.Size = new Size(0x3f, 13);
            this.laStyleDesc.TabIndex = 2;
            this.laStyleDesc.Text = "Description:";
            this.clbBkColor.DrawMode = DrawMode.OwnerDrawFixed;
            this.clbBkColor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.clbBkColor.Location = new Point(0x10, 160);
            this.clbBkColor.Name = "clbBkColor";
            this.clbBkColor.SelectedColor = Color.Empty;
            this.clbBkColor.Size = new Size(0xd8, 0x15);
            this.clbBkColor.TabIndex = 7;
            this.clbForeColor.DrawMode = DrawMode.OwnerDrawFixed;
            this.clbForeColor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.clbForeColor.Location = new Point(0x10, 0x72);
            this.clbForeColor.Name = "clbForeColor";
            this.clbForeColor.SelectedColor = Color.Empty;
            this.clbForeColor.Size = new Size(0xd8, 0x15);
            this.clbForeColor.TabIndex = 5;
            this.gbFontStyles.Controls.Add(this.chbStrikeout);
            this.gbFontStyles.Controls.Add(this.chbItalic);
            this.gbFontStyles.Controls.Add(this.chbUnderline);
            this.gbFontStyles.Controls.Add(this.chbBold);
            this.gbFontStyles.Location = new Point(0x100, 0x20);
            this.gbFontStyles.Name = "gbFontStyles";
            this.gbFontStyles.Size = new Size(0x70, 120);
            this.gbFontStyles.TabIndex = 9;
            this.gbFontStyles.TabStop = false;
            this.gbFontStyles.Text = "Font Styles";
            this.chbStrikeout.Location = new Point(0x10, 0x58);
            this.chbStrikeout.Name = "chbStrikeout";
            this.chbStrikeout.Size = new Size(80, 0x18);
            this.chbStrikeout.TabIndex = 3;
            this.chbStrikeout.Text = "StrikeOut";
            this.chbItalic.Location = new Point(0x10, 40);
            this.chbItalic.Name = "chbItalic";
            this.chbItalic.Size = new Size(80, 0x18);
            this.chbItalic.TabIndex = 1;
            this.chbItalic.Text = "Italic";
            this.chbUnderline.Location = new Point(0x10, 0x40);
            this.chbUnderline.Name = "chbUnderline";
            this.chbUnderline.Size = new Size(80, 0x18);
            this.chbUnderline.TabIndex = 2;
            this.chbUnderline.Text = "Underline";
            this.chbBold.Location = new Point(0x10, 0x10);
            this.chbBold.Name = "chbBold";
            this.chbBold.Size = new Size(80, 0x18);
            this.chbBold.TabIndex = 0;
            this.chbBold.Text = "Bold";
            this.tpStates.Controls.Add(this.pnStates);
            this.tpStates.Location = new Point(4, 0x16);
            this.tpStates.Name = "tpStates";
            this.tpStates.Size = new Size(0x182, 0x1ce);
            this.tpStates.TabIndex = 2;
            this.tpStates.Text = "States";
            this.pnStates.BackColor = SystemColors.Control;
            this.pnStates.Controls.Add(this.gbreswordsProperties);
            this.pnStates.Controls.Add(this.gbSyntaxBlockProperties);
            this.pnStates.Controls.Add(this.gbStateProperties);
            this.pnStates.Location = new Point(0, 0);
            this.pnStates.Name = "pnStates";
            this.pnStates.Size = new Size(390, 490);
            this.pnStates.TabIndex = 3;
            this.pnStates.Visible = false;
            this.gbreswordsProperties.Controls.Add(this.tbreswordSetName);
            this.gbreswordsProperties.Controls.Add(this.lareswordSetName);
            this.gbreswordsProperties.Controls.Add(this.pnreswordButtons);
            this.gbreswordsProperties.Controls.Add(this.cbreswordStyle);
            this.gbreswordsProperties.Controls.Add(this.tbreswords);
            this.gbreswordsProperties.Controls.Add(this.laBlockreswords);
            this.gbreswordsProperties.Controls.Add(this.lareswordStyle);
            this.gbreswordsProperties.Location = new Point(8, 0x138);
            this.gbreswordsProperties.Name = "gbreswordsProperties";
            this.gbreswordsProperties.Size = new Size(380, 0x90);
            this.gbreswordsProperties.TabIndex = 2;
            this.gbreswordsProperties.TabStop = false;
            this.gbreswordsProperties.Text = "resword Set Properties";
            this.tbreswordSetName.Location = new Point(0x70, 0x10);
            this.tbreswordSetName.Name = "tbreswordSetName";
            this.tbreswordSetName.Size = new Size(0x100, 20);
            this.tbreswordSetName.TabIndex = 1;
            this.lareswordSetName.AutoSize = true;
            this.lareswordSetName.Location = new Point(8, 0x13);
            this.lareswordSetName.Name = "lareswordSetName";
            this.lareswordSetName.Size = new Size(0x26, 13);
            this.lareswordSetName.TabIndex = 0;
            this.lareswordSetName.Text = "Name:";
            this.pnreswordButtons.Controls.Add(this.btreswordSetDelete);
            this.pnreswordButtons.Controls.Add(this.btreswordSetEdit);
            this.pnreswordButtons.Controls.Add(this.btreswordSetAdd);
            this.pnreswordButtons.Location = new Point(8, 0x7c);
            this.pnreswordButtons.Name = "pnreswordButtons";
            this.pnreswordButtons.Size = new Size(0x34, 0x12);
            this.pnreswordButtons.TabIndex = 7;
            this.btreswordSetDelete.ImageIndex = 2;
            this.btreswordSetDelete.ImageList = this.ilButtons;
            this.btreswordSetDelete.Location = new Point(0x11, 0);
            this.btreswordSetDelete.Name = "btreswordSetDelete";
            this.btreswordSetDelete.Size = new Size(0x11, 0x11);
            this.btreswordSetDelete.TabIndex = 1;
            this.btreswordSetDelete.Click += new EventHandler(this.DeletereswordSetClick);
            this.btreswordSetEdit.ImageIndex = 1;
            this.btreswordSetEdit.ImageList = this.ilButtons;
            this.btreswordSetEdit.Location = new Point(0x22, 0);
            this.btreswordSetEdit.Name = "btreswordSetEdit";
            this.btreswordSetEdit.Size = new Size(0x11, 0x11);
            this.btreswordSetEdit.TabIndex = 2;
            this.btreswordSetEdit.Click += new EventHandler(this.btreswordSetEdit_Click);
            this.btreswordSetAdd.ImageIndex = 0;
            this.btreswordSetAdd.ImageList = this.ilButtons;
            this.btreswordSetAdd.Location = new Point(0, 0);
            this.btreswordSetAdd.Name = "btreswordSetAdd";
            this.btreswordSetAdd.Size = new Size(0x11, 0x11);
            this.btreswordSetAdd.TabIndex = 0;
            this.btreswordSetAdd.Click += new EventHandler(this.AddreswordSetClick);
            this.cbreswordStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbreswordStyle.Location = new Point(0x70, 40);
            this.cbreswordStyle.Name = "cbreswordStyle";
            this.cbreswordStyle.Size = new Size(0x100, 0x15);
            this.cbreswordStyle.TabIndex = 3;
            this.tbreswords.Location = new Point(0x70, 0x40);
            this.tbreswords.Multiline = true;
            this.tbreswords.Name = "tbreswords";
            this.tbreswords.ScrollBars = ScrollBars.Vertical;
            this.tbreswords.Size = new Size(0x100, 0x40);
            this.tbreswords.TabIndex = 5;
            this.laBlockreswords.AutoSize = true;
            this.laBlockreswords.Location = new Point(8, 0x43);
            this.laBlockreswords.Name = "laBlockreswords";
            this.laBlockreswords.Size = new Size(0x34, 13);
            this.laBlockreswords.TabIndex = 4;
            this.laBlockreswords.Text = "reswords:";
            this.lareswordStyle.AutoSize = true;
            this.lareswordStyle.Location = new Point(8, 0x2b);
            this.lareswordStyle.Name = "lareswordStyle";
            this.lareswordStyle.Size = new Size(0x49, 13);
            this.lareswordStyle.TabIndex = 2;
            this.lareswordStyle.Text = "resword Style:";
            this.gbSyntaxBlockProperties.Controls.Add(this.tbExpressions);
            this.gbSyntaxBlockProperties.Controls.Add(this.tbBlockName);
            this.gbSyntaxBlockProperties.Controls.Add(this.laBlockName);
            this.gbSyntaxBlockProperties.Controls.Add(this.pnBlockButtons);
            this.gbSyntaxBlockProperties.Controls.Add(this.laBlockExpr);
            this.gbSyntaxBlockProperties.Controls.Add(this.tbBlockDesc);
            this.gbSyntaxBlockProperties.Controls.Add(this.laBlockDesc);
            this.gbSyntaxBlockProperties.Controls.Add(this.laLeaveState);
            this.gbSyntaxBlockProperties.Controls.Add(this.laStyle);
            this.gbSyntaxBlockProperties.Controls.Add(this.cbLeaveState);
            this.gbSyntaxBlockProperties.Controls.Add(this.cbStyle);
            this.gbSyntaxBlockProperties.Location = new Point(8, 0x66);
            this.gbSyntaxBlockProperties.Name = "gbSyntaxBlockProperties";
            this.gbSyntaxBlockProperties.Size = new Size(380, 0xc2);
            this.gbSyntaxBlockProperties.TabIndex = 1;
            this.gbSyntaxBlockProperties.TabStop = false;
            this.gbSyntaxBlockProperties.Text = "Block Properties";
            this.tbExpressions.Location = new Point(0x70, 0x70);
            this.tbExpressions.Multiline = true;
            this.tbExpressions.Name = "tbExpressions";
            this.tbExpressions.Size = new Size(0x100, 0x40);
            this.tbExpressions.TabIndex = 12;
            this.tbBlockName.Location = new Point(0x70, 0x10);
            this.tbBlockName.Name = "tbBlockName";
            this.tbBlockName.Size = new Size(0x100, 20);
            this.tbBlockName.TabIndex = 1;
            this.laBlockName.AutoSize = true;
            this.laBlockName.Location = new Point(8, 0x13);
            this.laBlockName.Name = "laBlockName";
            this.laBlockName.Size = new Size(0x26, 13);
            this.laBlockName.TabIndex = 0;
            this.laBlockName.Text = "Name:";
            this.pnBlockButtons.Controls.Add(this.btBlockDelete);
            this.pnBlockButtons.Controls.Add(this.btBlockEdit);
            this.pnBlockButtons.Controls.Add(this.btBlockAdd);
            this.pnBlockButtons.Location = new Point(8, 170);
            this.pnBlockButtons.Name = "pnBlockButtons";
            this.pnBlockButtons.Size = new Size(0x34, 0x12);
            this.pnBlockButtons.TabIndex = 11;
            this.btBlockDelete.ImageIndex = 2;
            this.btBlockDelete.ImageList = this.ilButtons;
            this.btBlockDelete.Location = new Point(0x11, 0);
            this.btBlockDelete.Name = "btBlockDelete";
            this.btBlockDelete.Size = new Size(0x11, 0x11);
            this.btBlockDelete.TabIndex = 1;
            this.btBlockDelete.Click += new EventHandler(this.DeleteBlockClick);
            this.btBlockEdit.ImageIndex = 1;
            this.btBlockEdit.ImageList = this.ilButtons;
            this.btBlockEdit.Location = new Point(0x22, 0);
            this.btBlockEdit.Name = "btBlockEdit";
            this.btBlockEdit.Size = new Size(0x11, 0x11);
            this.btBlockEdit.TabIndex = 2;
            this.btBlockEdit.Click += new EventHandler(this.btBlockEdit_Click);
            this.btBlockAdd.ImageIndex = 0;
            this.btBlockAdd.ImageList = this.ilButtons;
            this.btBlockAdd.Location = new Point(0, 0);
            this.btBlockAdd.Name = "btBlockAdd";
            this.btBlockAdd.Size = new Size(0x11, 0x11);
            this.btBlockAdd.TabIndex = 0;
            this.btBlockAdd.Click += new EventHandler(this.AddBlockClick);
            this.laBlockExpr.AutoSize = true;
            this.laBlockExpr.Location = new Point(8, 0x73);
            this.laBlockExpr.Name = "laBlockExpr";
            this.laBlockExpr.Size = new Size(0x42, 13);
            this.laBlockExpr.TabIndex = 8;
            this.laBlockExpr.Text = "Expressions:";
            this.tbBlockDesc.Location = new Point(0x70, 40);
            this.tbBlockDesc.Name = "tbBlockDesc";
            this.tbBlockDesc.Size = new Size(0x100, 20);
            this.tbBlockDesc.TabIndex = 3;
            this.laBlockDesc.AutoSize = true;
            this.laBlockDesc.Location = new Point(8, 0x2b);
            this.laBlockDesc.Name = "laBlockDesc";
            this.laBlockDesc.Size = new Size(0x3f, 13);
            this.laBlockDesc.TabIndex = 2;
            this.laBlockDesc.Text = "Description:";
            this.laLeaveState.AutoSize = true;
            this.laLeaveState.Location = new Point(8, 0x43);
            this.laLeaveState.Name = "laLeaveState";
            this.laLeaveState.Size = new Size(0x44, 13);
            this.laLeaveState.TabIndex = 4;
            this.laLeaveState.Text = "Leave State:";
            this.laStyle.AutoSize = true;
            this.laStyle.Location = new Point(8, 0x5b);
            this.laStyle.Name = "laStyle";
            this.laStyle.Size = new Size(0x21, 13);
            this.laStyle.TabIndex = 6;
            this.laStyle.Text = "Style:";
            this.cbLeaveState.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbLeaveState.Location = new Point(0x70, 0x40);
            this.cbLeaveState.Name = "cbLeaveState";
            this.cbLeaveState.Size = new Size(0x100, 0x15);
            this.cbLeaveState.TabIndex = 5;
            this.cbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStyle.Location = new Point(0x70, 0x58);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new Size(0x100, 0x15);
            this.cbStyle.TabIndex = 7;
            this.gbStateProperties.Controls.Add(this.pnStateButtons);
            this.gbStateProperties.Controls.Add(this.tbStateName);
            this.gbStateProperties.Controls.Add(this.laStateName);
            this.gbStateProperties.Controls.Add(this.laStateDesc);
            this.gbStateProperties.Controls.Add(this.tbStateDesc);
            this.gbStateProperties.Controls.Add(this.chbCaseSensitive);
            this.gbStateProperties.Location = new Point(8, 8);
            this.gbStateProperties.Name = "gbStateProperties";
            this.gbStateProperties.Size = new Size(380, 0x5c);
            this.gbStateProperties.TabIndex = 0;
            this.gbStateProperties.TabStop = false;
            this.gbStateProperties.Text = "State Properties";
            this.pnStateButtons.Controls.Add(this.btStateDelete);
            this.pnStateButtons.Controls.Add(this.btStateEdit);
            this.pnStateButtons.Controls.Add(this.btStateAdd);
            this.pnStateButtons.Location = new Point(8, 0x44);
            this.pnStateButtons.Name = "pnStateButtons";
            this.pnStateButtons.Size = new Size(0x34, 0x12);
            this.pnStateButtons.TabIndex = 8;
            this.btStateDelete.ImageIndex = 2;
            this.btStateDelete.ImageList = this.ilButtons;
            this.btStateDelete.Location = new Point(0x11, 0);
            this.btStateDelete.Name = "btStateDelete";
            this.btStateDelete.Size = new Size(0x11, 0x11);
            this.btStateDelete.TabIndex = 1;
            this.btStateDelete.Click += new EventHandler(this.DeleteStateClick);
            this.btStateEdit.ImageIndex = 1;
            this.btStateEdit.ImageList = this.ilButtons;
            this.btStateEdit.Location = new Point(0x22, 0);
            this.btStateEdit.Name = "btStateEdit";
            this.btStateEdit.Size = new Size(0x11, 0x11);
            this.btStateEdit.TabIndex = 2;
            this.btStateEdit.Click += new EventHandler(this.btStateEdit_Click);
            this.btStateAdd.ImageIndex = 0;
            this.btStateAdd.ImageList = this.ilButtons;
            this.btStateAdd.Location = new Point(0, 0);
            this.btStateAdd.Name = "btStateAdd";
            this.btStateAdd.Size = new Size(0x11, 0x11);
            this.btStateAdd.TabIndex = 0;
            this.btStateAdd.Click += new EventHandler(this.AddStateClick);
            this.tbStateName.Location = new Point(0x70, 0x10);
            this.tbStateName.Name = "tbStateName";
            this.tbStateName.Size = new Size(0x100, 20);
            this.tbStateName.TabIndex = 1;
            this.laStateName.AutoSize = true;
            this.laStateName.Location = new Point(8, 0x13);
            this.laStateName.Name = "laStateName";
            this.laStateName.Size = new Size(0x26, 13);
            this.laStateName.TabIndex = 0;
            this.laStateName.Text = "Name:";
            this.laStateDesc.AutoSize = true;
            this.laStateDesc.Location = new Point(8, 0x2b);
            this.laStateDesc.Name = "laStateDesc";
            this.laStateDesc.Size = new Size(0x3f, 13);
            this.laStateDesc.TabIndex = 2;
            this.laStateDesc.Text = "Description:";
            this.tbStateDesc.Location = new Point(0x70, 40);
            this.tbStateDesc.Name = "tbStateDesc";
            this.tbStateDesc.Size = new Size(0x100, 20);
            this.tbStateDesc.TabIndex = 3;
            this.chbCaseSensitive.Location = new Point(0x108, 0x40);
            this.chbCaseSensitive.Name = "chbCaseSensitive";
            this.chbCaseSensitive.Size = new Size(0x68, 0x18);
            this.chbCaseSensitive.TabIndex = 4;
            this.chbCaseSensitive.Text = "CaseSensitive";
            this.cmExpressions.MenuItems.AddRange(new MenuItem[] { this.menuItem1, this.menuItem2, this.menuItem3, this.menuItem4, this.menuItem5 });
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Identifiers   [a-zA-Z_][a-zA-Z0-9_]*";
            this.menuItem1.Click += new EventHandler(this.menuItem5_Click);
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Comments   //.";
            this.menuItem2.Click += new EventHandler(this.menuItem5_Click);
            this.menuItem3.Index = 2;
            this.menuItem3.Text = @"Numbers     ([0-9]+\.[0-9]*(e|E)(\+|\-)?[0-9]+)|([0-9]+\.[0-9]*)|([0-9]+)";
            this.menuItem3.Click += new EventHandler(this.menuItem5_Click);
            this.menuItem4.Index = 3;
            this.menuItem4.Text = "Strings        '[^']*'";
            this.menuItem4.Click += new EventHandler(this.menuItem5_Click);
            this.menuItem5.Index = 4;
            this.menuItem5.Text = @"Whitespace (\s)*";
            this.menuItem5.Click += new EventHandler(this.menuItem5_Click);
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0x248, 520);
            base.Controls.Add(this.pnMain);
            base.Controls.Add(this.tvLexer);
            base.Controls.Add(this.pnButtons);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Name = "DlgSyntaxBuilder";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Syntax Scheme Builder";
            base.Load += new EventHandler(this.MainForm_Load);
            base.Closing += new CancelEventHandler(this.DlgSyntaxBuilder_Closing);
            this.pnButtons.ResumeLayout(false);
            this.pnMain.ResumeLayout(false);
            this.tcPanels.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.pnGeneral.ResumeLayout(false);
            this.pnGeneral.PerformLayout();
            this.tpStyles.ResumeLayout(false);
            this.pnStyles.ResumeLayout(false);
            this.pnStyleButtons.ResumeLayout(false);
            this.pnSample.ResumeLayout(false);
            this.pnSample.PerformLayout();
            this.gbStyleProperties.ResumeLayout(false);
            this.gbStyleProperties.PerformLayout();
            this.gbFontStyles.ResumeLayout(false);
            this.tpStates.ResumeLayout(false);
            this.pnStates.ResumeLayout(false);
            this.gbreswordsProperties.ResumeLayout(false);
            this.gbreswordsProperties.PerformLayout();
            this.pnreswordButtons.ResumeLayout(false);
            this.gbSyntaxBlockProperties.ResumeLayout(false);
            this.gbSyntaxBlockProperties.PerformLayout();
            this.pnBlockButtons.ResumeLayout(false);
            this.gbStateProperties.ResumeLayout(false);
            this.gbStateProperties.PerformLayout();
            this.pnStateButtons.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private bool IsSchemeComplete(ILexScheme scheme, out string errorMsg)
        {
            errorMsg = string.Empty;
            for (int i = 0; i < scheme.States.Count; i++)
            {
                if (!this.IsStateComplete(scheme, scheme.States[i], out errorMsg))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsStateComplete(ILexScheme scheme, ILexState state, out string errorMsg)
        {
            errorMsg = string.Empty;
            foreach (ILexSyntaxBlock block in state.SyntaxBlocks)
            {
                if (!this.IsSyntaxBlockComplete(scheme, state, block, out errorMsg))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsStateUsed()
        {
            bool flag = false;
            ILexState lexState = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            if (lexState != null)
            {
                for (int i = 0; i < this.Scheme.States.Count; i++)
                {
                    if (lexState != this.Scheme.States[i])
                    {
                        for (int j = 0; j < this.Scheme.States[i].SyntaxBlocks.Count; j++)
                        {
                            if (lexState.Equals(this.Scheme.States[i].SyntaxBlocks[j].LeaveState))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                }
            }
            return flag;
        }

        private bool IsStyleUsed()
        {
            bool flag = false;
            ILexStyle lexStyle = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (lexStyle != null)
            {
                for (int i = 0; i < this.Scheme.States.Count; i++)
                {
                    for (int j = 0; j < this.Scheme.States[i].SyntaxBlocks.Count; j++)
                    {
                        if (lexStyle.Equals(this.Scheme.States[i].SyntaxBlocks[j].Style))
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }
            return flag;
        }

        private bool IsSyntaxBlockComplete(ILexScheme scheme, ILexState state, ILexSyntaxBlock block, out string errorMsg)
        {
            errorMsg = string.Empty;
            if (block.LeaveState == null)
            {
                errorMsg = string.Format(SyntaxBuilderConsts.IncompleteLeaveState, state.Name, block.Name);
                return false;
            }
            if (block.Style == null)
            {
                errorMsg = string.Format(SyntaxBuilderConsts.IncompleteLeaveStyle, state.Name, block.Name);
                return false;
            }
            foreach (LexReswordSet set in block.ReswordSets)
            {
                if ((set.Reswords.Count > 0) && (set.ReswordStyle == null))
                {
                    errorMsg = string.Format(SyntaxBuilderConsts.IncompleteReswordStyle, state.Name, block.Name, set.Name);
                    return false;
                }
            }
            return true;
        }

        private void LoadFromResource()
        {
            this.Text = SyntaxBuilderConsts.DlgSyntaxBuilderCaption;
            if (this.tvLexer.Nodes.Count > 0)
            {
                this.tvLexer.Nodes[0].Text = SyntaxBuilderConsts.SyntaxSchemeEditorCaption;
                if (this.tvLexer.Nodes[0].Nodes.Count >= 3)
                {
                    this.tvLexer.Nodes[0].Nodes[0].Text = SyntaxBuilderConsts.SyntaxSchemeGeneralCaption;
                    this.tvLexer.Nodes[0].Nodes[1].Text = SyntaxBuilderConsts.SyntaxSchemeStylesCaption;
                    this.tvLexer.Nodes[0].Nodes[2].Text = SyntaxBuilderConsts.SyntaxSchemeStatesCaption;
                }
                this.tpGeneral.Text = SyntaxBuilderConsts.GeneralCaption_SyntaxBuilderDlg;
                this.tpStyles.Text = SyntaxBuilderConsts.StylesCaption;
                this.tpStates.Text = SyntaxBuilderConsts.StatesCaption;
                this.laAuthor.Text = SyntaxBuilderConsts.AuthorCaption;
                this.laSchemeName.Text = SyntaxBuilderConsts.SchemeNameCaption;
                this.laDescription.Text = SyntaxBuilderConsts.DescriptionCaption_SyntaxBuilderDlg;
                this.laCopyright.Text = SyntaxBuilderConsts.CopyrightCaption;
                this.gbStyleProperties.Text = SyntaxBuilderConsts.StylePropertiesCaption;
                this.laStyleName.Text = SyntaxBuilderConsts.StyleNameCaption;
                this.laStyleDesc.Text = SyntaxBuilderConsts.StyleDescriptionCaption;
                this.laStyleForeColor.Text = SyntaxBuilderConsts.StyleForeColorCaption;
                this.laStyleBkColor.Text = SyntaxBuilderConsts.StyleBackColorCaption;
                this.gbFontStyles.Text = SyntaxBuilderConsts.FontStylesCaption;
                this.chbBold.Text = SyntaxBuilderConsts.BoldCaption_SyntaxBuilderDlg;
                this.chbItalic.Text = SyntaxBuilderConsts.ItalicCaption_SyntaxBuilderDlg;
                this.chbUnderline.Text = SyntaxBuilderConsts.UnderlineCaption_SyntaxBuilderDlg;
                this.chbStrikeout.Text = SyntaxBuilderConsts.StrikeOutCaption_SyntaxBuilderDlg;
                this.chbPlainText.Text = SyntaxBuilderConsts.PlainTextCaption;
                this.laSample.Text = SyntaxBuilderConsts.SampleCaption_SyntaxBuilderDlg;
                this.gbStateProperties.Text = SyntaxBuilderConsts.StatePropertiesCaption;
                this.laStateName.Text = SyntaxBuilderConsts.StateNameCaption;
                this.laStateDesc.Text = SyntaxBuilderConsts.StateDescriptionCaption;
                this.chbCaseSensitive.Text = SyntaxBuilderConsts.CaseSensitiveCaption;
                this.gbSyntaxBlockProperties.Text = SyntaxBuilderConsts.BlockPropertiesCaption;
                this.laBlockName.Text = SyntaxBuilderConsts.BlockNameCaption;
                this.laBlockDesc.Text = SyntaxBuilderConsts.BlockDescriptionCaption;
                this.laLeaveState.Text = SyntaxBuilderConsts.LeaveStateCaption;
                this.laStyle.Text = SyntaxBuilderConsts.BlockStyleCaption;
                this.laBlockExpr.Text = SyntaxBuilderConsts.BlockExpressionCaption;
                this.gbreswordsProperties.Text = SyntaxBuilderConsts.ReswordSetPropertiesCaption;
                this.lareswordSetName.Text = SyntaxBuilderConsts.ReswordSetNameCaption;
                this.lareswordStyle.Text = SyntaxBuilderConsts.ReswordStyleCaption;
                this.laBlockreswords.Text = SyntaxBuilderConsts.ReswordsCaption;
                this.btLoadScheme.Text = SyntaxBuilderConsts.LoadCaption;
                this.btSave.Text = SyntaxBuilderConsts.SaveCaption;
                this.btClear.Text = SyntaxBuilderConsts.ClearCaption;
                this.btOk.Text = SyntaxBuilderConsts.OKCaption_SyntaxBuilderDlg;
                this.btCancel.Text = SyntaxBuilderConsts.CancelCaption_SyntaxBuilderDlg;
            }
        }

        private void LoadScheme()
        {
            if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.ClearScheme();
                this.lexer.Scheme.LoadFile(this.openFileDialog.FileName);
                this.Text = SyntaxBuilderConsts.SyntaxFormCaption + " " + this.openFileDialog.FileName;
                this.UpdateScheme();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            TreeNode node = new TreeNode(this.sRemoveScroll);
            this.tvLexer.Nodes.Add(node);
            this.tvLexer.Nodes.Remove(node);
            this.tbExpressions.ContextMenu = this.cmExpressions;
            this.tvLexer.SelectedNode = this.rootNode;
            this.rootNode.Expand();
            this.saveFileDialog.Filter = "xml files (*.xml)|*.xml";
            this.openFileDialog.Filter = "Scheme files (*.xml)|*.xml";
            this.LoadFromResource();
            this.UpdateEvents(true);
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            string sampleExpression = this.GetSampleExpression(((MenuItem) sender).Text);
            this.tbExpressions.Text = (this.tbExpressions.Text == string.Empty) ? sampleExpression : (this.tbExpressions.Text + "\r\n" + sampleExpression);
        }

        private void SaveScheme()
        {
            string errorMsg = string.Empty;
            if ((this.IsSchemeComplete(this.Scheme, out errorMsg) || (MessageBox.Show(SyntaxBuilderConsts.InvalidScheme + ": " + errorMsg + "\n" + SyntaxBuilderConsts.QueryInvalidScheme, StringConsts.ErrorCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)) && (this.saveFileDialog.ShowDialog(this) == DialogResult.OK))
            {
                this.lexer.Scheme.Version = SyntaxConsts.DefaultLexSchemeVersion;
                this.lexer.Scheme.SaveFile(this.saveFileDialog.FileName);
                this.Text = SyntaxBuilderConsts.SyntaxFormCaption + " " + this.saveFileDialog.FileName;
            }
        }

        private void tbAuthor_TextChanged(object sender, EventArgs e)
        {
            this.Scheme.Author = this.tbAuthor.Text;
        }

        private void tbBlockDesc_TextChanged(object sender, EventArgs e)
        {
            ILexSyntaxBlock lexSyntaxBlock = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
            if (lexSyntaxBlock != null)
            {
                lexSyntaxBlock.Desc = this.tbBlockDesc.Text;
            }
        }

        private void tbBlockName_TextChanged(object sender, EventArgs e)
        {
            ILexSyntaxBlock lexSyntaxBlock = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
            if (lexSyntaxBlock != null)
            {
                this.GetSyntaxBlockNode(this.tvLexer.SelectedNode).Text = this.tbBlockName.Text;
                lexSyntaxBlock.Name = this.tbBlockName.Text;
            }
        }

        private void tbCopyright_TextChanged(object sender, EventArgs e)
        {
            this.Scheme.Copyright = this.tbCopyright.Text;
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            this.Scheme.Desc = this.tbDescription.Text;
        }

        private void tbExpressions_TextChanged(object sender, EventArgs e)
        {
            this.UpdateBlockExpressions();
        }

        private void tbFileExt_TextChanged(object sender, EventArgs e)
        {
            this.Scheme.FileExtension = this.tbFileExt.Text;
        }

        private void tbFileType_TextChanged(object sender, EventArgs e)
        {
            this.Scheme.FileType = this.tbFileType.Text;
        }

        private void tbreswords_TextChanged(object sender, EventArgs e)
        {
            this.Updatereswords();
        }

        private void tbreswordSetName_TextChanged(object sender, EventArgs e)
        {
            ILexReswordSet lexreswordSet = this.GetLexreswordSet(this.tvLexer.SelectedNode, this.Scheme);
            if (lexreswordSet != null)
            {
                this.tvLexer.SelectedNode.Text = this.tbreswordSetName.Text;
                lexreswordSet.Name = this.tbreswordSetName.Text;
            }
        }

        private void tbSchemeName_TextChanged(object sender, EventArgs e)
        {
            this.Scheme.Name = this.tbSchemeName.Text;
        }

        private void tbStateDesc_TextChanged(object sender, EventArgs e)
        {
            ILexState lexState = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            if (lexState != null)
            {
                lexState.Desc = this.tbStateDesc.Text;
            }
        }

        private void tbStateName_TextChanged(object sender, EventArgs e)
        {
            ILexState lexState = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            if (lexState != null)
            {
                TreeNode stateNode = this.GetStateNode(this.tvLexer.SelectedNode);
                if (stateNode != null)
                {
                    stateNode.Text = this.tbStateName.Text;
                }
                lexState.Name = this.tbStateName.Text;
                this.UpdateStates((stateNode != null) ? stateNode.Index : -1);
            }
        }

        private void tbStyleDesc_TextChanged(object sender, EventArgs e)
        {
            ILexStyle lexStyle = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (lexStyle != null)
            {
                lexStyle.Desc = this.tbStyleDesc.Text;
            }
        }

        private void tbStyleName_TextChanged(object sender, EventArgs e)
        {
            ILexStyle lexStyle = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (lexStyle != null)
            {
                TreeNode styleNode = this.GetStyleNode(this.tvLexer.SelectedNode);
                if (styleNode != null)
                {
                    styleNode.Text = this.tbStyleName.Text;
                }
                lexStyle.Name = this.tbStyleName.Text;
                this.UpdateLexStyles();
            }
        }

        private void tvLexer_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Equals(this.stylesNode) || e.Node.Equals(this.statesNode))
            {
                e.Node.SelectedImageIndex = 1;
            }
        }

        private void tvLexer_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Equals(this.stylesNode) || e.Node.Equals(this.statesNode))
            {
                e.Node.SelectedImageIndex = 0;
            }
        }

        private void tvLexer_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                switch (this.GetNodeKind(e.Node))
                {
                    case NodeKind.nkStyle:
                        this.Scheme.Styles[e.Node.Index].Name = e.Label;
                        this.UpdateLexStyles();
                        return;

                    case NodeKind.nkStates:
                        return;

                    case NodeKind.nkState:
                        this.Scheme.States[e.Node.Index].Name = e.Label;
                        this.UpdateStates(e.Node.Index);
                        return;

                    case NodeKind.nkSyntaxBlock:
                    {
                        ILexSyntaxBlock lexSyntaxBlock = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
                        if (lexSyntaxBlock != null)
                        {
                            lexSyntaxBlock.Name = e.Label;
                        }
                        return;
                    }
                }
            }
        }

        private void tvLexer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.UpdateTree(e.Node);
        }

        private void tvLexer_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            int nodeLevel = this.GetNodeLevel(e.Node);
            e.CancelEdit = nodeLevel <= 1;
        }

        private void tvLexer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode nodeAt = this.tvLexer.GetNodeAt(e.X, e.Y);
                if (nodeAt != null)
                {
                    this.tvLexer.SelectedNode = nodeAt;
                }
                this.cmLexer.MenuItems.Clear();
                NodeKind nodeKind = this.GetNodeKind(this.tvLexer.SelectedNode);
                switch (nodeKind)
                {
                    case NodeKind.nkStyles:
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.StyleText, new EventHandler(this.AddStyleClick)));
                        break;

                    case NodeKind.nkStyle:
                    {
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.StyleText, new EventHandler(this.AddStyleClick)));
                        MenuItem item = new MenuItem(SyntaxBuilderConsts.DeleteText + " " + SyntaxBuilderConsts.StyleText, new EventHandler(this.DeleteStyleClick)) {
                            Enabled = !this.IsStyleUsed()
                        };
                        this.cmLexer.MenuItems.Add(item);
                        break;
                    }
                    case NodeKind.nkStates:
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.StateText, new EventHandler(this.AddStateClick)));
                        break;

                    case NodeKind.nkState:
                    {
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.StateText, new EventHandler(this.AddStateClick)));
                        MenuItem item2 = new MenuItem(SyntaxBuilderConsts.DeleteText + " " + SyntaxBuilderConsts.StateText, new EventHandler(this.DeleteStateClick)) {
                            Enabled = !this.IsStateUsed()
                        };
                        this.cmLexer.MenuItems.Add(item2);
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.BlockText, new EventHandler(this.AddBlockClick)));
                        break;
                    }
                    case NodeKind.nkSyntaxBlock:
                    case NodeKind.nkreswordSet:
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.BlockText, new EventHandler(this.AddBlockClick)));
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.DeleteText + " " + SyntaxBuilderConsts.BlockText, new EventHandler(this.DeleteBlockClick)));
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.ReswordSetText, new EventHandler(this.AddreswordSetClick)));
                        if (nodeKind == NodeKind.nkreswordSet)
                        {
                            this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.DeleteText + " " + SyntaxBuilderConsts.ReswordSetText, new EventHandler(this.DeletereswordSetClick)));
                        }
                        break;
                }
                this.cmLexer.Show(this.tvLexer, new Point(e.X, e.Y));
            }
        }

        private void UpdateBlockExpressions()
        {
            ILexSyntaxBlock lexSyntaxBlock = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
            if (lexSyntaxBlock != null)
            {
                lexSyntaxBlock.Expressions = new string[0];
                foreach (string str in this.tbExpressions.Lines)
                {
                    string expression = str.Trim();
                    if (expression != string.Empty)
                    {
                        lexSyntaxBlock.AddExpression(expression);
                    }
                }
            }
        }

        private void UpdateEvents(bool update)
        {
            if (update)
            {
                this.cbStyle.SelectedIndexChanged += new EventHandler(this.cbStyle_SelectedIndexChanged);
                this.cbreswordStyle.SelectedIndexChanged += new EventHandler(this.cbreswordStyle_SelectedIndexChanged);
                this.cbLeaveState.SelectedIndexChanged += new EventHandler(this.cbLeaveState_SelectedIndexChanged);
                this.chbBold.CheckedChanged += new EventHandler(this.chbBold_CheckedChanged);
                this.chbItalic.CheckedChanged += new EventHandler(this.chbItalic_CheckedChanged);
                this.chbUnderline.CheckedChanged += new EventHandler(this.chbUnderline_CheckedChanged);
                this.chbStrikeout.CheckedChanged += new EventHandler(this.chbStrikeout_CheckedChanged);
                this.chbPlainText.CheckedChanged += new EventHandler(this.chbPlainText_CheckedChanged);
                this.clbBkColor.SelectedIndexChanged += new EventHandler(this.clbBkColor_SelectedIndexChanged);
                this.clbForeColor.SelectedIndexChanged += new EventHandler(this.clbForeColor_SelectedIndexChanged);
                this.chbCaseSensitive.CheckedChanged += new EventHandler(this.chbCaseSensitive_TextChanged);
                this.tbAuthor.TextChanged += new EventHandler(this.tbAuthor_TextChanged);
                this.tbDescription.TextChanged += new EventHandler(this.tbDescription_TextChanged);
                this.tbCopyright.TextChanged += new EventHandler(this.tbCopyright_TextChanged);
                this.tbFileType.TextChanged += new EventHandler(this.tbFileType_TextChanged);
                this.tbFileExt.TextChanged += new EventHandler(this.tbFileExt_TextChanged);
                this.tbSchemeName.TextChanged += new EventHandler(this.tbSchemeName_TextChanged);
                this.tbStateDesc.TextChanged += new EventHandler(this.tbStateDesc_TextChanged);
                this.tbStyleName.TextChanged += new EventHandler(this.tbStyleName_TextChanged);
                this.tbStateName.TextChanged += new EventHandler(this.tbStateName_TextChanged);
                this.tbBlockName.TextChanged += new EventHandler(this.tbBlockName_TextChanged);
                this.tbreswordSetName.TextChanged += new EventHandler(this.tbreswordSetName_TextChanged);
                this.tbreswords.TextChanged += new EventHandler(this.tbreswords_TextChanged);
                this.tbExpressions.TextChanged += new EventHandler(this.tbExpressions_TextChanged);
                this.tbStyleDesc.TextChanged += new EventHandler(this.tbStyleDesc_TextChanged);
                this.tbBlockDesc.TextChanged += new EventHandler(this.tbBlockDesc_TextChanged);
            }
            else
            {
                this.cbStyle.SelectedIndexChanged -= new EventHandler(this.cbStyle_SelectedIndexChanged);
                this.cbreswordStyle.SelectedIndexChanged -= new EventHandler(this.cbreswordStyle_SelectedIndexChanged);
                this.cbLeaveState.SelectedIndexChanged -= new EventHandler(this.cbLeaveState_SelectedIndexChanged);
                this.chbBold.CheckedChanged -= new EventHandler(this.chbBold_CheckedChanged);
                this.chbItalic.CheckedChanged -= new EventHandler(this.chbItalic_CheckedChanged);
                this.chbUnderline.CheckedChanged -= new EventHandler(this.chbUnderline_CheckedChanged);
                this.chbStrikeout.CheckedChanged -= new EventHandler(this.chbStrikeout_CheckedChanged);
                this.chbPlainText.CheckedChanged -= new EventHandler(this.chbPlainText_CheckedChanged);
                this.clbBkColor.SelectedIndexChanged -= new EventHandler(this.clbBkColor_SelectedIndexChanged);
                this.clbForeColor.SelectedIndexChanged -= new EventHandler(this.clbForeColor_SelectedIndexChanged);
                this.chbCaseSensitive.CheckedChanged -= new EventHandler(this.chbCaseSensitive_TextChanged);
                this.tbAuthor.TextChanged -= new EventHandler(this.tbAuthor_TextChanged);
                this.tbDescription.TextChanged -= new EventHandler(this.tbDescription_TextChanged);
                this.tbCopyright.TextChanged -= new EventHandler(this.tbCopyright_TextChanged);
                this.tbFileType.TextChanged -= new EventHandler(this.tbFileType_TextChanged);
                this.tbFileExt.TextChanged -= new EventHandler(this.tbFileExt_TextChanged);
                this.tbSchemeName.TextChanged -= new EventHandler(this.tbSchemeName_TextChanged);
                this.tbStateDesc.TextChanged -= new EventHandler(this.tbStateDesc_TextChanged);
                this.tbStyleName.TextChanged -= new EventHandler(this.tbStyleName_TextChanged);
                this.tbStateName.TextChanged -= new EventHandler(this.tbStateName_TextChanged);
                this.tbBlockName.TextChanged -= new EventHandler(this.tbBlockName_TextChanged);
                this.tbreswordSetName.TextChanged -= new EventHandler(this.tbreswordSetName_TextChanged);
                this.tbreswords.TextChanged -= new EventHandler(this.tbreswords_TextChanged);
                this.tbExpressions.TextChanged -= new EventHandler(this.tbExpressions_TextChanged);
                this.tbStyleDesc.TextChanged -= new EventHandler(this.tbStyleDesc_TextChanged);
                this.tbBlockDesc.TextChanged -= new EventHandler(this.tbBlockDesc_TextChanged);
            }
        }

        private void UpdateFontStyle()
        {
            switch (this.GetNodeKind(this.tvLexer.SelectedNode))
            {
                case NodeKind.nkStyle:
                case NodeKind.nkStyles:
                    this.updating = true;
                    try
                    {
                        ILexStyle lexStyle = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
                        if (lexStyle != null)
                        {
                            lexStyle.FontStyle = FontStyle.Regular;
                            if (this.chbBold.Checked)
                            {
                                lexStyle.FontStyle |= FontStyle.Bold;
                            }
                            if (this.chbItalic.Checked)
                            {
                                lexStyle.FontStyle |= FontStyle.Italic;
                            }
                            if (this.chbUnderline.Checked)
                            {
                                lexStyle.FontStyle |= FontStyle.Underline;
                            }
                            if (this.chbStrikeout.Checked)
                            {
                                lexStyle.FontStyle |= FontStyle.Strikeout;
                            }
                        }
                    }
                    finally
                    {
                        this.updating = false;
                    }
                    this.UpdateSample();
                    break;
            }
        }

        private void UpdateGeneralPanel()
        {
            this.pnGeneral.Visible = true;
            this.UpdateEvents(false);
            try
            {
                this.tbAuthor.Text = this.Scheme.Author;
                this.tbDescription.Text = this.Scheme.Desc;
                this.tbCopyright.Text = this.Scheme.Copyright;
                this.tbSchemeName.Text = this.Scheme.Name;
                this.tbFileType.Text = this.Scheme.FileType;
                this.tbFileExt.Text = this.Scheme.FileExtension;
            }
            finally
            {
                this.UpdateEvents(false);
            }
        }

        private void UpdateImages()
        {
            TreeNode folderNode = this.GetFolderNode();
            if (folderNode == this.rootNode)
            {
                this.generalNode.ImageIndex = 2;
            }
            else
            {
                this.generalNode.ImageIndex = 3;
            }
            if (folderNode == this.stylesNode)
            {
                this.stylesNode.ImageIndex = 0;
            }
            else
            {
                this.stylesNode.ImageIndex = 1;
            }
            if (folderNode == this.statesNode)
            {
                this.statesNode.ImageIndex = 0;
            }
            else
            {
                this.statesNode.ImageIndex = 1;
            }
            if (this.stylesNode.Nodes.Count > 0)
            {
                if (this.stylesNode == this.tvLexer.SelectedNode)
                {
                    this.stylesNode.Nodes[0].ImageIndex = 2;
                }
                else
                {
                    this.stylesNode.Nodes[0].ImageIndex = 3;
                }
            }
        }

        private void UpdateLexStyles()
        {
            this.cbStyle.Items.Clear();
            this.cbreswordStyle.Items.Clear();
            for (int i = 0; i < this.Scheme.Styles.Count; i++)
            {
                if (this.Scheme.Styles[i].Name != string.Empty)
                {
                    this.cbStyle.Items.Add(this.Scheme.Styles[i].Name);
                    this.cbreswordStyle.Items.Add(this.Scheme.Styles[i].Name);
                }
                else
                {
                    this.cbStyle.Items.Add("");
                    this.cbreswordStyle.Items.Add("");
                }
            }
            ILexStyle lexStyle = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            this.tbStyleName.Text = (lexStyle != null) ? lexStyle.Name : string.Empty;
        }

        private void UpdatePanelControls(NodeKind kind)
        {
            switch (kind)
            {
                case NodeKind.nkStyles:
                    this.UpdateStylePanel(false);
                    return;

                case NodeKind.nkStyle:
                    this.UpdateStylePanel(true);
                    return;

                case NodeKind.nkStates:
                case NodeKind.nkState:
                case NodeKind.nkSyntaxBlock:
                case NodeKind.nkreswordSet:
                    this.UpdateStatePanel();
                    return;

                case NodeKind.nkGeneral:
                    this.UpdateGeneralPanel();
                    return;
            }
        }

        private void Updatereswords()
        {
            ILexReswordSet lexreswordSet = this.GetLexreswordSet(this.tvLexer.SelectedNode, this.Scheme);
            if (lexreswordSet != null)
            {
                lexreswordSet.Reswords = new string[0];
                foreach (string str in this.tbreswords.Lines)
                {
                    string resword = str.Trim();
                    if ((resword != string.Empty) && !lexreswordSet.FindResword(resword))
                    {
                        lexreswordSet.AddResword(resword);
                    }
                }
            }
            this.cbreswordStyle.Enabled = (lexreswordSet != null) && (lexreswordSet.Reswords.Count > 0);
        }

        private void UpdateSample()
        {
            ILexStyle lexStyle = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (lexStyle != null)
            {
                this.laSample.Font = new Font(this.laSample.Font.Name, this.laSample.Font.Size, lexStyle.FontStyle);
            }
            else
            {
                this.laSample.Font = new Font(this.laSample.Font.Name, this.laSample.Font.Size, FontStyle.Regular);
            }
            this.laSample.ForeColor = this.clbForeColor.SelectedColor;
            this.laSample.BackColor = this.clbBkColor.SelectedColor;
            int x = (this.pnSample.Width - this.laSample.Width) / 2;
            int y = (this.pnSample.Height - this.laSample.Height) / 2;
            this.laSample.Location = new Point(x, y);
        }

        private void UpdateScheme()
        {
            this.cbStyle.Items.Clear();
            this.cbreswordStyle.Items.Clear();
            this.cbLeaveState.Items.Clear();
            this.stylesNode.Nodes.Clear();
            this.statesNode.Nodes.Clear();
            for (int i = 0; i < this.Scheme.Styles.Count; i++)
            {
                this.stylesNode.Nodes.Add(new TreeNode(this.Scheme.Styles[i].Name));
                this.cbStyle.Items.Add(this.Scheme.Styles[i].Name);
                this.cbreswordStyle.Items.Add(this.Scheme.Styles[i].Name);
            }
            for (int j = 0; j < this.Scheme.States.Count; j++)
            {
                this.statesNode.Nodes.Add(new TreeNode(this.Scheme.States[j].Name, 4, 4));
                TreeNode node = this.statesNode.Nodes[j];
                for (int k = 0; k < this.Scheme.States[j].SyntaxBlocks.Count; k++)
                {
                    node.Nodes.Add(new TreeNode(this.Scheme.States[j].SyntaxBlocks[k].Name));
                    for (int m = 0; m < this.Scheme.States[j].SyntaxBlocks[k].ReswordSets.Count; m++)
                    {
                        string name = this.Scheme.States[j].SyntaxBlocks[k].ReswordSets[m].Name;
                        if (((name == null) || (name == string.Empty)) || (name.Trim() == ""))
                        {
                            name = SyntaxBuilderConsts.ReswordSetText + ((m + 1)).ToString();
                        }
                        node.Nodes[k].Nodes.Add(new TreeNode(name));
                    }
                }
                this.cbLeaveState.Items.Add(this.Scheme.States[j].Name);
            }
            this.tvLexer.CollapseAll();
            this.tvLexer.SelectedNode = this.generalNode;
        }

        private void UpdateStatePanel()
        {
            this.UpdateEvents(false);
            try
            {
                NodeKind nodeKind = this.GetNodeKind(this.tvLexer.SelectedNode);
                this.pnStates.Visible = (nodeKind != NodeKind.nkStates) || ((nodeKind == NodeKind.nkStates) && (this.statesNode.Nodes.Count > 0));
                switch (nodeKind)
                {
                    case NodeKind.nkStates:
                        this.gbStateProperties.Visible = this.statesNode.Nodes.Count > 0;
                        this.gbSyntaxBlockProperties.Visible = false;
                        this.gbreswordsProperties.Visible = false;
                        break;

                    case NodeKind.nkState:
                        this.gbStateProperties.Visible = true;
                        this.gbSyntaxBlockProperties.Visible = false;
                        this.gbreswordsProperties.Visible = false;
                        break;

                    case NodeKind.nkSyntaxBlock:
                        this.gbStateProperties.Visible = true;
                        this.gbSyntaxBlockProperties.Visible = true;
                        this.gbreswordsProperties.Visible = false;
                        break;

                    case NodeKind.nkreswordSet:
                        this.gbStateProperties.Visible = true;
                        this.gbSyntaxBlockProperties.Visible = true;
                        this.gbreswordsProperties.Visible = true;
                        break;
                }
                ILexState lexState = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
                ILexSyntaxBlock lexSyntaxBlock = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
                ILexReswordSet lexreswordSet = this.GetLexreswordSet(this.tvLexer.SelectedNode, this.Scheme);
                if (lexState != null)
                {
                    this.chbCaseSensitive.Checked = lexState.CaseSensitive;
                    this.tbStateDesc.Text = lexState.Desc;
                    this.tbStateName.Text = lexState.Name;
                }
                this.cbStyle.SelectedIndex = this.GetStyleIndex(lexSyntaxBlock);
                this.btStateDelete.Enabled = !this.IsStateUsed();
                this.cbLeaveState.SelectedIndex = this.GetLeaveStateIndex(lexSyntaxBlock);
                this.tbExpressions.Text = string.Empty;
                this.tbreswords.Text = string.Empty;
                if (lexSyntaxBlock != null)
                {
                    this.tbBlockName.Text = lexSyntaxBlock.Name;
                    this.tbBlockDesc.Text = lexSyntaxBlock.Desc;
                    foreach (string str in lexSyntaxBlock.Expressions)
                    {
                        this.tbExpressions.Text = (this.tbExpressions.Text == string.Empty) ? str : (this.tbExpressions.Text + "\r\n" + str);
                    }
                }
                if (lexreswordSet != null)
                {
                    this.tbreswordSetName.Text = lexreswordSet.Name;
                    this.cbreswordStyle.Enabled = lexreswordSet.Reswords.Count > 0;
                    this.cbreswordStyle.SelectedIndex = this.GetreswordStyleIndex(lexreswordSet);
                    foreach (string str2 in lexreswordSet.Reswords)
                    {
                        this.tbreswords.Text = (this.tbreswords.Text == string.Empty) ? str2 : (this.tbreswords.Text + "\r\n" + str2);
                    }
                }
            }
            finally
            {
                this.UpdateEvents(true);
            }
        }

        private void UpdateStates(int index)
        {
            this.cbLeaveState.Items.Clear();
            for (int i = 0; i < this.Scheme.States.Count; i++)
            {
                if (this.Scheme.States[i].Name != null)
                {
                    this.cbLeaveState.Items.Add(this.Scheme.States[i].Name);
                }
                else
                {
                    this.cbLeaveState.Items.Add("");
                }
            }
            if (this.cbLeaveState.Items.Count > index)
            {
                this.cbLeaveState.SelectedIndex = index;
            }
            ILexState lexState = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            this.tbStateName.Text = (lexState != null) ? lexState.Name : string.Empty;
        }

        private void UpdateStylePanel(bool isStyle)
        {
            this.UpdateEvents(false);
            try
            {
                this.pnStyles.Visible = isStyle || (!isStyle && (this.stylesNode.Nodes.Count > 0));
                if (this.pnStyles.Visible)
                {
                    ILexStyle lexStyle = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
                    if (lexStyle != null)
                    {
                        this.tbStyleName.Text = lexStyle.Name;
                        this.tbStyleDesc.Text = lexStyle.Desc;
                        this.clbForeColor.SelectedColor = lexStyle.ForeColor;
                        this.clbBkColor.SelectedColor = lexStyle.BackColor;
                        this.chbBold.Checked = (lexStyle.FontStyle & FontStyle.Bold) != FontStyle.Regular;
                        this.chbItalic.Checked = (lexStyle.FontStyle & FontStyle.Italic) != FontStyle.Regular;
                        this.chbUnderline.Checked = (lexStyle.FontStyle & FontStyle.Underline) != FontStyle.Regular;
                        this.chbStrikeout.Checked = (lexStyle.FontStyle & FontStyle.Strikeout) != FontStyle.Regular;
                        this.chbPlainText.Checked = lexStyle.PlainText;
                    }
                    this.btStyleDelete.Enabled = !this.IsStyleUsed();
                    this.UpdateSample();
                }
            }
            finally
            {
                this.UpdateEvents(true);
            }
        }

        private void UpdateTree(TreeNode node)
        {
            this.updating = true;
            try
            {
                this.UpdateImages();
                this.UpdateVisible(this.GetNodeKind(node));
            }
            finally
            {
                this.updating = false;
            }
        }

        private void UpdateVisible(NodeKind kind)
        {
            Panel currentPanel = this.GetCurrentPanel(kind);
            if (currentPanel != null)
            {
                this.pnMain.Controls.Add(currentPanel);
            }
            for (int i = 0; i < this.pnMain.Controls.Count; i++)
            {
                if (!this.pnMain.Controls[i].Equals(currentPanel) && (this.pnMain.Controls[i] is Panel))
                {
                    this.pnMain.Controls[i].Visible = false;
                }
            }
            currentPanel.Dock = DockStyle.Fill;
            currentPanel.BringToFront();
            this.UpdatePanelControls(kind);
        }

        public ILexScheme Scheme
        {
            get
            {
                return this.lexer.Scheme;
            }
            set
            {
                this.lexer.Scheme = value;
                this.UpdateScheme();
            }
        }

        internal enum NodeKind
        {
            nkNone,
            nkStyles,
            nkStyle,
            nkStates,
            nkState,
            nkSyntaxBlock,
            nkGeneral,
            nkreswordSet
        }
    }
}

