using System;
using System.Windows.Forms;
using WealthLab;
using WealthLab.DataProviders.MarketManagerService;
using WealthLab.DataProviders.MarketManagerService.Properties;

internal class Class37 : MenuItemHook
{
    private Form form_0;
    private MarketMangerForm marketMangerForm_0;

    private void method_0(object sender, EventArgs e)
    {
        Application.DoEvents();
        if ((this.marketMangerForm_0 == null) || this.marketMangerForm_0.IsDisposed)
        {
            this.method_1();
            if (this.form_0 == null)
            {
                throw new Exception("MainForm not found.");
            }
            this.form_0.Cursor = Cursors.WaitCursor;
            try
            {
                this.marketMangerForm_0 = new MarketMangerForm();
                this.marketMangerForm_0.MdiParent = this.form_0;
            }
            finally
            {
                this.form_0.Cursor = Cursors.Default;
            }
        }
        this.marketMangerForm_0.Show();
        this.marketMangerForm_0.Activate();
        if (this.marketMangerForm_0.WindowState == FormWindowState.Minimized)
        {
            this.marketMangerForm_0.WindowState = FormWindowState.Normal;
        }
    }

    private void method_1()
    {
        if (Application.OpenForms != null)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "MainForm")
                {
                    this.form_0 = form;
                }
            }
        }
    }

    public override void AddMenuItems(IMenuItemAdder adder)
    {
        adder.AddMenuItem("Market Manager", "&Tools", "Index-Lab \x00ae", new ClickMenuItem(this.method_0), Resources.clock);
    }
}

