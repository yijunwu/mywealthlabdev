namespace QWhale.Design.Dialogs
{
    using QWhale.Common;
    using QWhale.Design;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public class DlgFlagEnumeration : Form
    {
        private Container components;
        private object editValue;
        private FlagEnumerationListBox listBox;
        private int lockCheckUpdate;
        private FlagEnumerationEditor mainEditor;
        private object originalValue;

        public DlgFlagEnumeration()
        {
            this.InitializeComponent();
            this.lockCheckUpdate = 0;
            base.TopLevel = false;
            this.Font = Control.DefaultFont;
            this.listBox = new FlagEnumerationListBox();
            this.listBox.Font = Control.DefaultFont;
            this.listBox.BorderStyle = BorderStyle.None;
            Rectangle clientRectangle = base.ClientRectangle;
            this.listBox.SetBounds(clientRectangle.Left, clientRectangle.Top, clientRectangle.Width, clientRectangle.Height);
            this.listBox.ItemCheck += new ItemCheckEventHandler(this.listBox_ItemCheckEventHandler);
            this.listBox.Visible = true;
            base.ClientSize = new Size(0, this.listBox.ItemHeight * 8);
            base.Controls.Add(this.listBox);
        }

        public DlgFlagEnumeration(FlagEnumerationEditor editor) : this()
        {
            this.mainEditor = editor;
        }

        protected void BeginUpdate()
        {
            this.lockCheckUpdate++;
        }

        protected void ClearAll()
        {
            foreach (string str in this.listBox.Items)
            {
                if (str != StringConsts.EmptyOption)
                {
                    this.DisableOption(str);
                }
            }
        }

        protected void DisableOption(string optionName)
        {
            if (optionName == StringConsts.EmptyOption)
            {
                this.SelectAll();
            }
            else
            {
                this.FromInt(((int) this.editValue) & ~this.GetOptionValue(optionName));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void EnableOption(string optionName)
        {
            if (optionName == StringConsts.EmptyOption)
            {
                this.ClearAll();
            }
            else
            {
                this.FromInt(((int) this.editValue) | this.GetOptionValue(optionName));
            }
        }

        protected void EndUpdate()
        {
            this.lockCheckUpdate--;
        }

        protected void FromInt(int value)
        {
            System.Type enumType = this.EditValue.GetType();
            this.editValue = Enum.ToObject(enumType, value);
        }

        protected IList<FieldInfo> GetFields(System.Type type)
        {
            IList<FieldInfo> list = new List<FieldInfo>();
            foreach (FieldInfo info in type.GetFields())
            {
                if (!info.IsSpecialName)
                {
                    list.Add(info);
                }
            }
            ((List<FieldInfo>) list).Sort(new FieldsComparer());
            return list;
        }

        protected int GetOptionValue(string optionName)
        {
            return (int) this.EditValue.GetType().GetField(optionName).GetValue(this.EditValue);
        }

        private void InitializeComponent()
        {
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x124, 0x10a);
            base.FormBorderStyle = FormBorderStyle.None;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DlgFlagEnumeration";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.WindowsDefaultBounds;
            this.Text = "DlgFlagEnumeration";
            base.TopMost = true;
        }

        protected bool IsOptionEnabled(string optionName)
        {
            int editValue = (int) this.editValue;
            if (optionName == StringConsts.EmptyOption)
            {
                return (editValue == this.GetOptionValue(optionName));
            }
            return ((editValue & this.GetOptionValue(optionName)) == this.GetOptionValue(optionName));
        }

        protected void listBox_ItemCheckEventHandler(object sender, ItemCheckEventArgs e)
        {
            if (this.lockCheckUpdate == 0)
            {
                this.BeginUpdate();
                try
                {
                    string optionName = this.listBox.Items[e.Index].ToString();
                    if (e.NewValue == CheckState.Checked)
                    {
                        this.EnableOption(optionName);
                    }
                    else
                    {
                        this.DisableOption(optionName);
                    }
                    this.UpdateListBox();
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (this.mainEditor != null)
                {
                    this.mainEditor.Service.CloseDropDown();
                }
                return true;
            }
            if (keyData != Keys.Escape)
            {
                return base.ProcessDialogKey(keyData);
            }
            this.editValue = this.originalValue;
            if (this.mainEditor != null)
            {
                this.mainEditor.Service.CloseDropDown();
            }
            return true;
        }

        protected void SelectAll()
        {
            foreach (string str in this.listBox.Items)
            {
                if (str != StringConsts.EmptyOption)
                {
                    this.EnableOption(str);
                }
            }
        }

        protected void UpdateListBox()
        {
            this.listBox.ListBoxBeginUpdate();
            try
            {
                for (int i = 0; i < this.listBox.Items.Count; i++)
                {
                    string optionName = this.listBox.Items[i].ToString();
                    this.listBox.SetItemChecked(i, this.IsOptionEnabled(optionName));
                }
            }
            finally
            {
                this.listBox.ListBoxEndUpdate();
            }
        }

        public object EditValue
        {
            get
            {
                return this.editValue;
            }
            set
            {
                if (this.editValue != value)
                {
                    this.editValue = value;
                    this.originalValue = value;
                    this.listBox.Items.Clear();
                    IList<FieldInfo> fields = this.GetFields(this.editValue.GetType());
                    this.BeginUpdate();
                    try
                    {
                        foreach (FieldInfo info in fields)
                        {
                            this.listBox.Items.Add(info.Name, this.IsOptionEnabled(info.Name) ? CheckState.Checked : CheckState.Unchecked);
                        }
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                    int num = Math.Min(this.listBox.Items.Count, 15);
                    this.listBox.ClientSize = new Size(base.Size.Width, this.listBox.ItemHeight * num);
                    base.ClientSize = new Size(base.Size.Width, this.listBox.ItemHeight * num);
                }
            }
        }

        public CheckedListBox ListBox
        {
            get
            {
                return this.listBox;
            }
        }

        private class FieldsComparer : IComparer<FieldInfo>
        {
            public int Compare(FieldInfo x, FieldInfo y)
            {
                if ((x == null) || (y == null))
                {
                    return Comparer<FieldInfo>.Default.Compare(x, y);
                }
                if (x == y)
                {
                    return 0;
                }
                if (x.Name == StringConsts.EmptyOption)
                {
                    return -1;
                }
                if (y.Name == StringConsts.EmptyOption)
                {
                    return 1;
                }
                return Comparer<string>.Default.Compare(x.Name, y.Name);
            }
        }

        internal class FlagEnumerationListBox : CheckedListBox
        {
            private int updateCount;

            public FlagEnumerationListBox()
            {
                base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            }

            public void ListBoxBeginUpdate()
            {
                base.BeginUpdate();
                this.updateCount++;
            }

            public void ListBoxEndUpdate()
            {
                base.EndUpdate();
                this.updateCount--;
                if (this.updateCount == 0)
                {
                    base.Invalidate();
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                if (this.updateCount == 0)
                {
                    base.OnPaint(e);
                }
            }
        }
    }
}

