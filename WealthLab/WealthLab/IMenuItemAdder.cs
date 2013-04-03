namespace WealthLab
{
    using System;
    using System.Drawing;

    public interface IMenuItemAdder
    {
        void AddMenuItem(string text, string mainMenuItemText, string subMenuItemText, ClickMenuItem callback, Image itemImage);
    }
}

