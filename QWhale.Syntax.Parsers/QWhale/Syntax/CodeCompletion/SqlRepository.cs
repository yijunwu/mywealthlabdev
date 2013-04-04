namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using QWhale.Syntax.Parsers;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class SqlRepository : CodeCompletionRepository
    {
        private const int cFieldImageIndex = 1;
        private const int cTableImageIndex = 0;
        private ImageList internalImages;
        private IList<TableItem> tables;

        public SqlRepository(bool caseSensitive, ISyntaxTree syntaxTree) : base(caseSensitive, syntaxTree)
        {
            this.tables = new List<TableItem>();
            this.internalImages = new ImageList();
            this.internalImages.ImageSize = new Size(15, 15);
            try
            {
                ResourceManager manager = new ResourceManager(typeof(MSSQLParser));
                this.internalImages.ImageStream = (ImageListStreamer) manager.GetObject("SQLImages.ImageStream");
            }
            catch
            {
            }
            this.internalImages.TransparentColor = SyntaxConsts.DefaultTransparentColor;
        }

        public override void FillMembers(ISyntaxNode node, Point position, IListMembers members, object member, string name, CodeCompletionScope scope, ref int selIndex)
        {
            if (member is ISyntaxNode)
            {
                this.FillTables(members);
            }
            else if (member is TableItem)
            {
                this.FillTableFields((TableItem) member, members);
            }
            members.Images = this.internalImages;
        }

        protected virtual void FillTableFields(TableItem table, IListMembers members)
        {
            foreach (string str in table.Fields)
            {
                IListMember member = members.AddListMember();
                member.ImageIndex = 1;
                member.Name = str;
            }
        }

        protected virtual void FillTables(IListMembers members)
        {
            foreach (TableItem item in this.tables)
            {
                IListMember member = members.AddListMember();
                member.Name = item.TableName;
                member.ImageIndex = 0;
            }
        }

        protected virtual ISyntaxNode FindTableAliasNode(ISyntaxNode node, string tableName)
        {
            if (node.NodeType == 0x57)
            {
                ISyntaxNode node2 = node.FindNode(0x58);
                if (((node2 != null) && node2.HasChildren) && ((node2.ChildList[0].NodeType == 0x51) && (string.Compare(node2.ChildList[0].Name, tableName, true) == 0)))
                {
                    return node2.Parent;
                }
            }
            if (node.HasChildren)
            {
                foreach (ISyntaxNode node3 in node.ChildList)
                {
                    ISyntaxNode node4 = this.FindTableAliasNode(node3, tableName);
                    if (node4 != null)
                    {
                        return node4;
                    }
                }
            }
            return null;
        }

        protected virtual TableItem FindTableByName(string name)
        {
            foreach (TableItem item in this.tables)
            {
                if (string.Compare(item.TableName, name, !this.CaseSensitive) == 0)
                {
                    return item;
                }
            }
            return null;
        }

        protected virtual ISyntaxNode FindTableNameNode(ISyntaxNode node, string name)
        {
            while (node != null)
            {
                node = this.GetBlockNode(node, 5);
                if (node != null)
                {
                    ISyntaxNode node2 = this.FindTableAliasNode(node, name);
                    if (node2 != null)
                    {
                        return node2;
                    }
                    node = this.GetBlockNode(node.Parent, 50);
                }
            }
            return null;
        }

        protected virtual ISyntaxNode GetBlockNode(ISyntaxNode node, int nodeType)
        {
            while (node != null)
            {
                switch (((MsSqlNodeType) node.NodeType))
                {
                    case MsSqlNodeType.WhereStatement:
                    case MsSqlNodeType.FromStatement:
                    case MsSqlNodeType.HavingStatement:
                    case MsSqlNodeType.OrderbyStatement:
                    case MsSqlNodeType.GroupbyStatement:
                    case MsSqlNodeType.SelectQuery:
                        if ((nodeType != 0) && (nodeType != node.NodeType))
                        {
                            break;
                        }
                        return node;
                }
                node = node.Parent;
            }
            return null;
        }

        public override object GetMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out CodeCompletionScope scope)
        {
            string str = string.Empty;
            Point point = position;
            if (this.GetQualifiedName(text, ref str, ref point) && (str != string.Empty))
            {
                scope = CodeCompletionScope.Instance;
                int index = str.IndexOf('.');
                if (index >= 0)
                {
                    if (position.X >= (point.X + index))
                    {
                        name = str.Substring(index + 1);
                        str = str.Substring(0, index);
                        ISyntaxNode node2 = this.FindTableNameNode(node, str);
                        if (node2 != null)
                        {
                            str = node2.Name;
                        }
                        position = point;
                        position.X = (point.X + index) + 1;
                        return this.FindTableByName(str);
                    }
                    name = str.Substring(0, index);
                }
                position = point;
            }
            scope = CodeCompletionScope.Static;
            if (((name == string.Empty) && (position.X < text.Length)) && ((text[position.X] != ' ') && (text[position.X] != '\t')))
            {
                return null;
            }
            return this.GetBlockNode(node, 0);
        }

        private bool GetQualifiedName(string text, ref string name, ref Point position)
        {
            name = string.Empty;
            if (text == null)
            {
                return false;
            }
            if (text.Trim() == string.Empty)
            {
                return true;
            }
            int x = Math.Min(text.Length, position.X);
            int num2 = x;
            for (int i = x - 1; i >= 0; i--)
            {
                if (!this.IsQualifiedChar(text[i]))
                {
                    break;
                }
                x = i;
            }
            while ((num2 < text.Length) && this.IsQualifiedChar(text[num2]))
            {
                num2++;
            }
            position = new Point(x, position.Y);
            if (num2 > x)
            {
                name = text.Substring(x, num2 - x);
            }
            return (num2 > x);
        }

        private bool IsQualifiedChar(char ch)
        {
            if ((((ch < 'a') || (ch > 'z')) && ((ch < 'A') || (ch > 'Z'))) && (((ch < '0') || (ch > '9')) && (ch != '_')))
            {
                return (ch == '.');
            }
            return true;
        }

        public IList<TableItem> Tables
        {
            get
            {
                return this.tables;
            }
        }

        public class TableItem
        {
            private IList<string> fields;
            private string tableName;

            public TableItem()
            {
                this.fields = new List<string>();
            }

            public TableItem(string tableName) : this()
            {
                this.tableName = tableName;
            }

            protected virtual void OnTableNameChanged()
            {
            }

            public IList<string> Fields
            {
                get
                {
                    return this.fields;
                }
            }

            public string TableName
            {
                get
                {
                    return this.tableName;
                }
                set
                {
                    if (this.tableName != value)
                    {
                        this.tableName = value;
                        this.OnTableNameChanged();
                    }
                }
            }
        }
    }
}

