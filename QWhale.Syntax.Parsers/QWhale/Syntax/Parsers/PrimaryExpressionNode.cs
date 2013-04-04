namespace QWhale.Syntax.Parsers
{
    using QWhale.Syntax;
    using System;
    using System.Drawing;

    public class PrimaryExpressionNode : SyntaxNode
    {
        private int token;

        public PrimaryExpressionNode(Point position, string name, int nodeType, int token) : base(position, name, nodeType)
        {
            this.token = token;
        }

        public int Token
        {
            get
            {
                return this.token;
            }
        }
    }
}

