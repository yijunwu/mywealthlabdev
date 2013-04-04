namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class EditHyperText : IEditHyperText, IHyperText
    {
        private ISyntaxEdit owner;
        private bool showHints;
        private Color urlColor;
        private UrlJumpEventArgs urlJumpArgs;
        private FontStyle urlStyle;

        [Browsable(false)]
        public event HyperTextEvent HyperText
        {
            add
            {
                this.owner.Source.HyperText += value;
            }
            remove
            {
                this.owner.Source.HyperText -= value;
            }
        }

        [Browsable(false)]
        public event UrlJumpEvent JumpToUrl;

        public EditHyperText()
        {
            this.urlColor = EditConsts.DefaultUrlForeColor;
            this.urlStyle = EditConsts.DefaultUrlFontStyle;
            this.showHints = true;
            this.urlJumpArgs = new UrlJumpEventArgs("", false);
        }

        public EditHyperText(ISyntaxEdit owner) : this()
        {
            this.owner = owner;
        }

        public virtual void Assign(IEditHyperText source)
        {
            this.HighlightHyperText = source.HighlightHyperText;
            this.UrlColor = source.UrlColor;
            this.UrlStyle = source.UrlStyle;
        }

        public virtual bool IsHyperText(string text)
        {
            return this.owner.Source.IsHyperText(text);
        }

        public virtual bool IsUrlAtPoint(int x, int y)
        {
            string str;
            return this.IsUrlAtPoint(x, y, out str, false);
        }

        public virtual bool IsUrlAtPoint(int x, int y, out string url)
        {
            return this.IsUrlAtPoint(x, y, out url, true);
        }

        public virtual bool IsUrlAtPoint(int x, int y, out string url, bool needUrl)
        {
            url = string.Empty;
            if (this.HighlightHyperText)
            {
                Point point = this.owner.ScreenToText(x, y);
                return this.IsUrlAtTextPoint(point.X, point.Y, out url, needUrl);
            }
            return false;
        }

        public virtual bool IsUrlAtTextPoint(int x, int y, out string url)
        {
            return this.IsUrlAtTextPoint(x, y, out url, true);
        }

        protected virtual bool IsUrlAtTextPoint(int x, int y, out string url, bool needUrl)
        {
            bool flag = false;
            url = string.Empty;
            if (this.HighlightHyperText)
            {
                IStringItem item = this.owner.Lines.GetItem(y);
                if (item == null)
                {
                    return flag;
                }
                short[] textData = item.TextData;
                flag = ((x >= 0) && (x < textData.Length)) && (((textData[x] >> 8) & 0x10) != 0);
                if (!flag || !needUrl)
                {
                    return flag;
                }
                int startIndex = x;
                int num2 = x;
                while ((startIndex > 0) && (((textData[startIndex - 1] >> 8) & 0x10) != 0))
                {
                    startIndex--;
                }
                while ((num2 < (textData.Length - 1)) && (((textData[num2 + 1] >> 8) & 0x10) != 0))
                {
                    num2++;
                }
                url = item.String.Substring(startIndex, (num2 - startIndex) + 1);
            }
            return flag;
        }

        protected virtual void OnShowHintsChanged()
        {
        }

        protected virtual void OnUrlColorChanged()
        {
            if (this.HighlightHyperText)
            {
                this.owner.Invalidate();
            }
        }

        protected virtual void OnUrlStyleChanged()
        {
            if (this.HighlightHyperText)
            {
                this.owner.Invalidate();
            }
        }

        public virtual void ResetHighlightHyperText()
        {
            this.HighlightHyperText = false;
        }

        public virtual void ResetShowHints()
        {
            this.ShowHints = true;
        }

        public virtual void ResetUrlColor()
        {
            this.UrlColor = EditConsts.DefaultUrlForeColor;
        }

        public virtual void ResetUrlStyle()
        {
            this.UrlStyle = EditConsts.DefaultUrlFontStyle;
        }

        public bool ShouldSerializeUrlColor()
        {
            return (this.urlColor != EditConsts.DefaultUrlForeColor);
        }

        public bool ShouldSerializeUrlStyle()
        {
            return (this.urlStyle != EditConsts.DefaultUrlFontStyle);
        }

        public virtual void UrlJump(string text)
        {
            this.urlJumpArgs.Text = text;
            this.urlJumpArgs.Handled = false;
            if (this.JumpToUrl != null)
            {
                this.JumpToUrl(this, this.urlJumpArgs);
            }
            if (!this.urlJumpArgs.Handled)
            {
                try
                {
                    Process.Start(text);
                }
                catch
                {
                }
            }
        }

        [DefaultValue(false), Description("Gets or sets a value indicating whether hypertext urls in the text should be highlighted.")]
        public virtual bool HighlightHyperText
        {
            get
            {
                return this.owner.Source.HighlightHyperText;
            }
            set
            {
                this.owner.Source.HighlightHyperText = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlEditHyperTextInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [DefaultValue(true), Description("Gets or sets value indicating whether default hint for hypertext section needs displaying when user moves mouse over the hypertext.")]
        public virtual bool ShowHints
        {
            get
            {
                return this.showHints;
            }
            set
            {
                if (this.showHints != value)
                {
                    this.showHints = value;
                    this.OnShowHintsChanged();
                }
            }
        }

        [Description("Gets or sets a value that represents color of highlighted urls.")]
        public virtual Color UrlColor
        {
            get
            {
                return this.urlColor;
            }
            set
            {
                if (this.urlColor != value)
                {
                    this.urlColor = value;
                    this.OnUrlColorChanged();
                }
            }
        }

        [Description("Gets or sets font style of highlighted urls.")]
        public virtual FontStyle UrlStyle
        {
            get
            {
                return this.urlStyle;
            }
            set
            {
                if (this.urlStyle != value)
                {
                    this.urlStyle = value;
                    this.OnUrlStyleChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Hashtable UrlTable
        {
            get
            {
                return this.owner.Source.UrlTable;
            }
        }
    }
}

