namespace QWhale.Syntax.Design
{
    using QWhale.Syntax.Design.Dialogs;
    using QWhale.Syntax.Lexer;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class SyntaxBuilderEditor : UITypeEditor
    {
        private IWindowsFormsEditorService service;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                this.service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
                if (this.service != null)
                {
                    DlgSyntaxBuilder dialog = new DlgSyntaxBuilder(this);
                    try
                    {
                        dialog.Scheme = (ILexScheme) value;
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Not implemented: Error loading scheme:" + exception.ToString());
                        dialog.Dispose();
                        this.service = null;
                        return value;
                    }
                    if (this.service.ShowDialog(dialog) == DialogResult.OK)
                    {
                        value = dialog.Scheme;
                    }
                    dialog.Dispose();
                    this.service = null;
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }
    }
}

