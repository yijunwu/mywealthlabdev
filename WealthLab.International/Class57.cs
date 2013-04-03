using System;
using System.Windows.Forms;
using WealthLab.International;

internal class Class57 : Button
{
    private ButtonBehaviour buttonBehaviour_0;

    public Class57()
    {
        this.method_1(ButtonBehaviour.Next);
    }

    public ButtonBehaviour method_0()
    {
        return this.buttonBehaviour_0;
    }

    public void method_1(ButtonBehaviour buttonBehaviour_1)
    {
        this.buttonBehaviour_0 = buttonBehaviour_1;
        this.Text = buttonBehaviour_1.ToString();
        if (buttonBehaviour_1 == ButtonBehaviour.Next)
        {
            this.Text = this.Text + " ->";
        }
        if (buttonBehaviour_1 == ButtonBehaviour.Previous)
        {
            this.Text = "<- " + this.Text;
        }
    }
}

