namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Editor;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class CompletionEdit : UserControl, ICompletionEdit, IControl
    {
        private TextBox editBox;
        private System.Windows.Forms.Label label;
        private IPainter painter = new GdiPainter();
        private System.Windows.Forms.Label pathLabel;

        public CompletionEdit()
        {
            base.Height = this.Font.Height;
            this.editBox = new TextBox();
            this.editBox.Multiline = false;
            this.editBox.Parent = this;
            this.editBox.BorderStyle = BorderStyle.None;
            this.editBox.Top = 0;
            this.editBox.KeyPress += new KeyPressEventHandler(this.DoKeyPress);
            this.pathLabel = new CompletionLabel(this);
            this.pathLabel.Parent = this;
            this.pathLabel.Font = new Font(this.PathLabel.Font, FontStyle.Underline);
            this.pathLabel.Top = 0;
            this.pathLabel.AutoSize = true;
            this.pathLabel.ForeColor = EditConsts.DefaultUrlForeColor;
            this.label = new System.Windows.Forms.Label();
            this.label.Parent = this;
            this.label.Font = new Font(this.label.Font, FontStyle.Bold);
            this.label.Top = 0;
            this.label.AutoSize = true;
            this.BackColor = EditConsts.DefaultSnippetBackColor;
            this.editBox.BackColor = EditConsts.DefaultSnippetBackColor;
            this.label.BackColor = EditConsts.DefaultSnippetBackColor;
            this.pathLabel.BackColor = EditConsts.DefaultSnippetBackColor;
            this.UpdateSize();
        }

        protected void DoKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = ((e.KeyChar == '\n') || (e.KeyChar == '\r')) || (e.KeyChar == '\x001b');
        }

        void QWhale.Common.IControl.add_Click(EventHandler handler1)
        {
            base.Click += handler1;
        }

        void IControl.BringToFront()
        {
            base.BringToFront();
        }

        Graphics IControl.CreateGraphics()
        {
            return base.CreateGraphics();
        }

        Form IControl.FindForm()
        {
            return base.FindForm();
        }

        bool IControl.Focus()
        {
            return base.Focus();
        }

        bool IControl.CanFocus
        {
            get { return base.CanFocus; }
            
        }

        Rectangle IControl.ClientRectangle
        {
            get
            {
                return base.ClientRectangle;
            }
        }

        bool IControl.Created
        {
            get { return base.Created; }
        }

        bool IControl.Enabled
        {
            get { return base.Enabled; }
            set { base.Enabled = value; }
        }

        int IControl.Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }

        bool IControl.IsHandleCreated
        {
            get {return base.IsHandleCreated;}
        }

        int IControl.Left
        {
            get { return base.Left; }
            set { base.Left = value; }
        }

        Point IControl.Location
        {
            get { return base.Location; }
            set { base.Location = value; }
        }

        Control IControl.Parent
        {
            get { return base.Parent; }
            set { base.Parent = value; }
        }

        int IControl.Top
        {
            get { return base.Top; }
            set { base.Top = value; }
        }

        bool IControl.Visible
        {
            get { return base.Visible; }
            set { base.Visible = value; }

        }

        int IControl.Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }

        void IControl.Invalidate()
        {
            base.Invalidate();
        }

        void IControl.Invalidate(Rectangle rectangle1)
        {
            base.Invalidate(rectangle1);
        }

        void IControl.Invalidate(Region region1)
        {
            base.Invalidate(region1);
        }

        void IControl.Invalidate(Region region1, bool flag1)
        {
            base.Invalidate(region1, flag1);
        }

        Point IControl.PointToClient(Point point1)
        {
            return base.PointToClient(point1);
        }

        Point IControl.PointToScreen(Point point1)
        {
            return base.PointToScreen(point1);
        }

        void IControl.remove_Click(EventHandler handler1)
        {
            base.Click -= handler1;
        }

        Rectangle IControl.Bounds
        {
            get { return base.Bounds; }
            set { base.Bounds = value; }
        }

        void IControl.Update()
        {
            base.Update();
        }

        public void UpdateSize()
        {
            this.painter.Font = this.pathLabel.Font;
            int num = Math.Max((this.pathLabel.Width - this.painter.StringWidth(this.pathLabel.Text)) / 2, 0);
            int num2 = (this.label.Width + this.pathLabel.Width) - num;
            int width = base.ClientRectangle.Width;
            this.label.Left = Math.Min((width - 2) - num2, 0);
            this.pathLabel.Left = this.label.Left + this.label.Width;
            this.editBox.Left = (this.pathLabel.Left + this.pathLabel.Width) - num;
            this.editBox.Width = width - this.editBox.Left;
        }

        public TextBox EditBox
        {
            get
            {
                return this.editBox;
            }
        }

        public System.Windows.Forms.Label Label
        {
            get
            {
                return this.label;
            }
        }

        public System.Windows.Forms.Label PathLabel
        {
            get
            {
                return this.pathLabel;
            }
        }

        internal class CompletionLabel : Label
        {
            private ICompletionEdit owner;

            public CompletionLabel(ICompletionEdit owner)
            {
                this.owner = owner;
            }

            public override string Text
            {
                get
                {
                    return base.Text;
                }
                set
                {
                    if (base.Text != value)
                    {
                        base.Text = value;
                        this.owner.UpdateSize();
                    }
                }
            }
        }
    }
}

