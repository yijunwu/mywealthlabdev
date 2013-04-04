namespace QWhale.Design
{
    using QWhale.Design.Dialogs;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms.Design;

    public class FlagEnumerationEditor : UITypeEditor
    {
        private IWindowsFormsEditorService service;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                this.service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
                if (this.service != null)
                {
                    DlgFlagEnumeration enumeration = new DlgFlagEnumeration(this) {
                        EditValue = value
                    };
                    this.service.DropDownControl(enumeration.ListBox);
                    value = enumeration.EditValue;
                    enumeration.Dispose();
                    this.service = null;
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
            {
                return UITypeEditorEditStyle.DropDown;
            }
            return base.GetEditStyle(context);
        }

        public IWindowsFormsEditorService Service
        {
            get
            {
                return this.service;
            }
        }
    }
}

