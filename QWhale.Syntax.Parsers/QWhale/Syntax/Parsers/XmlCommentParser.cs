namespace QWhale.Syntax.Parsers
{
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ToolboxItem(false)]
    public class XmlCommentParser : XmlParser
    {
        private Hashtable indents = new Hashtable();

        protected bool IsXmlComment(ref string source, out int indent)
        {
            indent = 0;
            if (source.TrimStart(new char[0]).StartsWith("///"))
            {
                indent = source.Length;
                source = source.TrimStart(new char[0]).Substring("///".Length);
                indent -= source.Length;
                return true;
            }
            if (source.TrimStart(new char[0]).StartsWith("'''"))
            {
                indent = source.Length;
                source = source.TrimStart(new char[0]).Substring("'''".Length);
                indent -= source.Length;
                return true;
            }
            return false;
        }

        protected override bool ParseUnit()
        {
            bool flag = base.ParseUnit();
            this.ShiftNode(this.SyntaxTree.Root);
            return flag;
        }

        public override void Reset()
        {
            this.indents.Clear();
            base.Reset();
        }

        protected override void ResetLine(int line)
        {
            base.ResetLine(line);
            while (base.source != null)
            {
                int num;
                if (this.IsXmlComment(ref this.source, out num))
                {
                    this.indents[line] = num;
                    break;
                }
                line++;
                base.ResetLine(line);
            }
            base.lineIndex = line;
        }

        protected virtual void ShiftAttribute(ISyntaxAttribute attr)
        {
            object obj2 = this.indents[attr.Position.Y];
            if (obj2 != null)
            {
                attr.Position = new Point(attr.Position.X + ((int) obj2), attr.Position.Y);
            }
        }

        protected virtual void ShiftError(ISyntaxError err)
        {
            object obj2 = this.indents[err.Position.Y];
            if (obj2 != null)
            {
                err.Position = new Point(err.Position.X + ((int) obj2), err.Position.Y);
            }
        }

        protected virtual void ShiftNode(ISyntaxNode node)
        {
            node.Options = SyntaxNodeOptions.None;
            object obj2 = this.indents[node.Range.StartPoint.Y];
            if (obj2 != null)
            {
                node.Range.StartPoint = new Point(node.Range.StartPoint.X + ((int) obj2), node.Range.StartPoint.Y);
            }
            obj2 = this.indents[node.Range.EndPoint.Y];
            if (obj2 != null)
            {
                node.Range.EndPoint = new Point(node.Range.EndPoint.X + ((int) obj2), node.Range.EndPoint.Y);
            }
            if (node.HasAttributes)
            {
                foreach (ISyntaxAttribute attribute in node.AttributeList)
                {
                    this.ShiftAttribute(attribute);
                }
            }
            if (node.HasErrors)
            {
                foreach (ISyntaxError error in node.ErrorList)
                {
                    this.ShiftError(error);
                }
            }
            if (node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    this.ShiftNode(node2);
                }
            }
        }
    }
}

