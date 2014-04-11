namespace WealthLabPro
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class DynamicMenuItem
    {
        private ClickMenuItem clickMenuItem_0;
        private Image itemImage;
        private string text;
        private string mainMenuItemText;
        private string subMenuItemText;

        public DynamicMenuItem(string text, string mainMenu, string subMenu, ClickMenuItem onClick)
        {
            this.text = text;
            this.mainMenuItemText = mainMenu;
            this.subMenuItemText = subMenu;
            this.clickMenuItem_0 = onClick;
            this.itemImage = null;
        }

        public DynamicMenuItem(string text, string mainMenu, string subMenu, ClickMenuItem onClick, Image itemImage)
        {
            this.text = text;
            this.mainMenuItemText = mainMenu;
            this.subMenuItemText = subMenu;
            this.clickMenuItem_0 = onClick;
            this.itemImage = itemImage;
        }

        public Image ItemImage
        {
            get
            {
                return this.itemImage;
            }
        }

        public string MainMenuItemText
        {
            get
            {
                return this.mainMenuItemText;
            }
        }

        public ClickMenuItem OnClick
        {
            get
            {
                return this.clickMenuItem_0;
            }
        }

        public string SubMenuItemText
        {
            get
            {
                return this.subMenuItemText;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
        }
    }
}

