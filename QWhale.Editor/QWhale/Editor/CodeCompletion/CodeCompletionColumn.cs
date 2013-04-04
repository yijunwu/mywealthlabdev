namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class CodeCompletionColumn : ICodeCompletionColumn
    {
        private System.Drawing.FontStyle fontStyle;
        private Color foreColor = Consts.DefaultControlForeColor;
        private string name;
        private bool visible = true;

        protected virtual void OnFontStyleChanged()
        {
        }

        protected virtual void OnForeColorChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnVisibleChanged()
        {
        }

        public virtual void ResetFontStyle()
        {
            this.fontStyle = System.Drawing.FontStyle.Regular;
        }

        public virtual void ResetForeColor()
        {
            this.ForeColor = Consts.DefaultControlForeColor;
        }

        public virtual void ResetVisible()
        {
            this.Visible = true;
        }

        [Description("Gets or sets style information for \"CodeCompletionColumn\" font.")]
        public virtual System.Drawing.FontStyle FontStyle
        {
            get
            {
                return this.fontStyle;
            }
            set
            {
                if (this.fontStyle != value)
                {
                    this.fontStyle = value;
                    this.OnFontStyleChanged();
                }
            }
        }

        [Description("Gets or sets Color structure that represents foreground color of the \"CodeCompletionColumn\".")]
        public virtual Color ForeColor
        {
            get
            {
                return this.foreColor;
            }
            set
            {
                if (this.foreColor != value)
                {
                    this.foreColor = value;
                    this.OnForeColorChanged();
                }
            }
        }

        [Description("Gets or sets the name of the \"CodeCompletionColumn\".")]
        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.OnNameChanged();
                }
            }
        }

        [Description("Gets or sets a value indicating whether a column is visible.")]
        public virtual bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    this.OnVisibleChanged();
                }
            }
        }
    }
}

