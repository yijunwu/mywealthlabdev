namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Design;
    using QWhale.Editor.Serialization;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ToolboxItem(false), TypeConverter(typeof(ScrollingButtonConverter))]
    public class ScrollingButton : IScrollingButton
    {
        private SpeedButton button;
        private string description = string.Empty;
        private int groupIndex;
        private IScrolling scrolling;
        private ToolTip toolTip;
        private bool visible = true;

        [Description("Occurs when the value of the Checked property changes.")]
        public event EventHandler CheckedChanged
        {
            add
            {
                this.button.CheckedChanged += value;
            }
            remove
            {
                this.button.CheckedChanged -= value;
            }
        }

        public ScrollingButton()
        {
            this.CreateButton();
        }

        public virtual void Assign(IScrollingButton source)
        {
            this.Name = source.Name;
            this.Description = source.Description;
            this.ImageIndex = source.ImageIndex;
            this.Images = source.Images;
            this.Visible = source.Visible;
            this.BorderStyle = source.BorderStyle;
            this.Checked = source.Checked;
            this.AllowCheck = source.AllowCheck;
            this.GroupIndex = source.GroupIndex;
        }

        protected void button_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateGroupChecked();
        }

        protected void button_MouseLeave(object sender, EventArgs e)
        {
            this.toolTip.Active = false;
        }

        protected void button_MouseMove(object sender, MouseEventArgs e)
        {
            this.toolTip.Active = this.description != string.Empty;
        }

        protected virtual void CreateButton()
        {
            this.button = new SpeedButton();
            this.button.BorderStyle = EditBorderStyle.None;
            this.button.MouseMove += new MouseEventHandler(this.button_MouseMove);
            this.button.MouseLeave += new EventHandler(this.button_MouseLeave);
            this.button.CheckedChanged += new EventHandler(this.button_CheckedChanged);
            this.toolTip = new ToolTip();
            this.toolTip.ShowAlways = true;
            this.toolTip.SetToolTip(this.button, this.description);
        }

        ~ScrollingButton()
        {
            if (this.button != null)
            {
                this.toolTip.Dispose();
                this.button.Dispose();
            }
        }

        protected virtual void OnAllowCheckChanged()
        {
        }

        protected virtual void OnBorderStyleChanged()
        {
        }

        protected virtual void OnCheckedChanged()
        {
        }

        protected virtual void OnDescriptionChanged()
        {
            this.toolTip.SetToolTip(this.button, this.description);
        }

        protected virtual void OnGroupIndexChanged()
        {
            this.UpdateGroupChecked();
        }

        protected virtual void OnImageIndexChanged()
        {
        }

        protected virtual void OnImageListChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnVisibleChanged()
        {
        }

        public bool ShouldSerializeDescription()
        {
            return (this.description != string.Empty);
        }

        private void UncheckGroup(IScrollingButtons buttons)
        {
            foreach (IScrollingButton button in buttons)
            {
                if ((button != this) && (this.groupIndex == button.GroupIndex))
                {
                    button.Checked = false;
                }
            }
        }

        protected void UpdateGroupChecked()
        {
            if (((this.scrolling != null) && (this.groupIndex > 0)) && this.Checked)
            {
                if (this.scrolling.HorzButtons.Contains(this))
                {
                    this.UncheckGroup(this.scrolling.HorzButtons);
                }
                else if (this.scrolling.VertButtons.Contains(this))
                {
                    this.UncheckGroup(this.scrolling.VertButtons);
                }
            }
        }

        [Description("Gets or sets a value indicating whehter button should automatically appear pressed in and not pressed in when clicked."), DefaultValue(false)]
        public virtual bool AllowCheck
        {
            get
            {
                return this.button.AllowCheck;
            }
            set
            {
                if (this.button.AllowCheck != value)
                {
                    this.button.AllowCheck = value;
                    this.OnAllowCheckChanged();
                }
            }
        }

        [Description("Gets or sets the border style for this \"ScrollingButton\"."), DefaultValue(0)]
        public virtual EditBorderStyle BorderStyle
        {
            get
            {
                return this.button.BorderStyle;
            }
            set
            {
                if (this.button.BorderStyle != value)
                {
                    this.button.BorderStyle = value;
                    this.OnBorderStyleChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual SpeedButton Button
        {
            get
            {
                return this.button;
            }
        }

        [Description("Gets or sets a boolean value indicating whether button appears pressed."), DefaultValue(false)]
        public virtual bool Checked
        {
            get
            {
                return this.button.Checked;
            }
            set
            {
                if (this.button.Checked != value)
                {
                    this.button.Checked = value;
                    this.OnCheckedChanged();
                }
            }
        }

        [Description("Gets or sets a string value that specifies short description of this \"ScrollingButton\"."), DefaultValue("")]
        public virtual string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    this.OnDescriptionChanged();
                }
            }
        }

        [Description("Gets or sets a value allows buttons to work together as a group."), DefaultValue(0)]
        public virtual int GroupIndex
        {
            get
            {
                return this.groupIndex;
            }
            set
            {
                if (this.groupIndex != value)
                {
                    this.groupIndex = value;
                    this.OnGroupIndexChanged();
                }
            }
        }

        [Description("Gets or sets the index of the image displayed for this \"ScrollingButton\"."), DefaultValue(-1)]
        public virtual int ImageIndex
        {
            get
            {
                return this.button.ImageIndex;
            }
            set
            {
                if (this.button.ImageIndex != value)
                {
                    this.button.ImageIndex = value;
                    this.OnImageIndexChanged();
                }
            }
        }

        [Description("Gets or sets the ImageList for this \"ScrollingButton\"."), DefaultValue((string) null)]
        public virtual ImageList Images
        {
            get
            {
                return this.button.ImageList;
            }
            set
            {
                if (this.button.ImageList != value)
                {
                    this.button.ImageList = value;
                    this.OnImageListChanged();
                }
            }
        }

        [DefaultValue(""), Description("Gets or sets a string value that specifies the name of this \"ScrollingButton\".")]
        public virtual string Name
        {
            get
            {
                return this.button.Name;
            }
            set
            {
                if (this.button.Name != value)
                {
                    this.button.Name = value;
                    this.OnNameChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IScrolling Scrolling
        {
            get
            {
                return this.scrolling;
            }
            set
            {
                if (this.scrolling != value)
                {
                    this.scrolling = value;
                    if ((value != null) && (value.Owner != null))
                    {
                        this.button.Parent = value.Owner as Control;
                        this.button.BringToFront();
                        this.button.Click += new EventHandler(value.OnScrollButtonClick);
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlScrollingButtonInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [Description("Gets or sets a value indicating whether this \"ScrollingButton\" is visible."), DefaultValue(true)]
        public virtual bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if ((this.visible != value) || (this.button.Visible != value))
                {
                    this.visible = value;
                    this.button.Visible = value;
                    this.OnVisibleChanged();
                }
            }
        }
    }
}

