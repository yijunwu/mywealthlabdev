namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class CompletionHint : Control
    {
        private Size hintSize;
        private IPainter painter = new GdiPainter();
        private ICodeCompletionProvider provider;
        private int selectedIndex = 1;
        private HintSyntaxPaint syntaxPaint;
        private EventHandler updateSize;

        public CompletionHint()
        {
            this.painter.Font = this.Font;
            this.syntaxPaint = new HintSyntaxPaint(this.painter, this);
            this.hintSize = base.Size;
            base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.StandardDoubleClick | ControlStyles.StandardClick | ControlStyles.Opaque | ControlStyles.UserPaint, true);
        }

        public void ChangeSelection(bool inc)
        {
            if (this.provider != null)
            {
                int selectedIndex = this.selectedIndex;
                if (inc)
                {
                    selectedIndex++;
                    if (selectedIndex >= this.provider.Count)
                    {
                        selectedIndex = 0;
                    }
                }
                else
                {
                    selectedIndex--;
                    if (selectedIndex < 0)
                    {
                        selectedIndex = Math.Max(this.provider.Count - 1, 0);
                    }
                }
                this.SelectedIndex = selectedIndex;
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            Keys keys = keyData & Keys.KeyCode;
            return ((Array.IndexOf<Keys>(EditConsts.NavKeys, keys) >= 0) || base.IsInputKey(keyData));
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.painter.Clear();
            this.painter.Font = this.Font;
            this.UpdateControlSize();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if ((e.Button == MouseButtons.Left) && this.NeedArrows)
            {
                this.ChangeSelection(!this.syntaxPaint.LeftArrowArea.Contains(e.X, e.Y));
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            this.painter.BeginPaint(pe.Graphics);
            try
            {
                this.syntaxPaint.PaintSyntax(this.painter, 0, this.syntaxPaint.Strings.Count - 1, new Point(0, 0), base.ClientRectangle, false);
            }
            finally
            {
                this.painter.EndPaint();
            }
        }

        protected virtual void OnSelectedIndexChanged()
        {
            this.StringChanged();
        }

        protected virtual void ProviderChanged()
        {
            this.selectedIndex = 0;
            this.StringChanged();
        }

        public void ResetContent()
        {
            this.ProviderChanged();
        }

        protected void StringChanged()
        {
            this.syntaxPaint.ProviderChanged(this.provider, this.selectedIndex);
            this.UpdateHint();
            this.UpdateControlSize();
        }

        protected void UpdateControlSize()
        {
            this.hintSize = this.syntaxPaint.UpdateSize();
            if (this.UpdateSize != null)
            {
                this.UpdateSize(this, EventArgs.Empty);
            }
            base.Invalidate();
        }

        public bool UpdateHint()
        {
            if (((this.provider != null) && (this.selectedIndex < this.provider.Count)) && this.syntaxPaint.UpdateHint(this.provider, this.selectedIndex))
            {
                this.UpdateControlSize();
                return true;
            }
            return false;
        }

        public Size HintSize
        {
            get
            {
                return this.hintSize;
            }
        }

        public bool NeedArrows
        {
            get
            {
                return this.syntaxPaint.NeedArrows;
            }
        }

        public ICodeCompletionProvider Provider
        {
            get
            {
                return this.provider;
            }
            set
            {
                if (this.provider != value)
                {
                    this.provider = value;
                    this.ProviderChanged();
                }
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }
            set
            {
                if (this.selectedIndex != value)
                {
                    this.selectedIndex = value;
                    this.OnSelectedIndexChanged();
                }
            }
        }

        public ISyntaxPaint SyntaxPaint
        {
            get
            {
                return this.syntaxPaint;
            }
        }

        public EventHandler UpdateSize
        {
            get
            {
                return this.updateSize;
            }
            set
            {
                this.updateSize = value;
            }
        }
    }
}

