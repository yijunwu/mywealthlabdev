namespace QWhale.Syntax
{
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Drawing;

    public class CodeCompletionArgs : EventArgs
    {
        public CodeCompletionType CompletionType;
        public Point DisplayPosition;
        public Point EndPosition;
        public bool Handled;
        public int Interval;
        public char KeyChar;
        public bool NeedReparse;
        public bool NeedShow;
        public ICodeCompletionProvider Provider;
        public int SelIndex = -1;
        public Point StartPosition;
        public bool ToolTip;
        public bool UseFormat;

        public void Init()
        {
            this.CompletionType = CodeCompletionType.None;
            this.Provider = null;
            this.KeyChar = '\0';
            this.ToolTip = false;
            this.Interval = 0;
            this.StartPosition = new Point(-1, -1);
            this.EndPosition = new Point(-1, -1);
            this.DisplayPosition = new Point(-1, -1);
            this.Handled = false;
            this.NeedShow = false;
            this.NeedReparse = true;
            this.SelIndex = -1;
        }

        public void Init(CodeCompletionType completionType, Point position)
        {
            this.Init(completionType, position, true);
        }

        public void Init(CodeCompletionType completionType, Point position, bool needReparse)
        {
            this.Init();
            this.CompletionType = completionType;
            this.StartPosition = position;
            this.DisplayPosition = position;
            this.NeedReparse = needReparse;
        }
    }
}

