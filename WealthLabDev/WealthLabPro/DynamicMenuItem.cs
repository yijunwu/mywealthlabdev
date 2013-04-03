namespace WealthLabPro
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class DynamicMenuItem
    {
        private ClickMenuItem clickMenuItem_0;
        private Image image_0;
        private string string_0;
        private string string_1;
        private string string_2;

        public DynamicMenuItem(string text, string mainMenu, string subMenu, ClickMenuItem onClick)
        {
            this.string_0 = text;
            this.string_1 = mainMenu;
            this.string_2 = subMenu;
            this.clickMenuItem_0 = onClick;
            this.image_0 = null;
        }

        public DynamicMenuItem(string text, string mainMenu, string subMenu, ClickMenuItem onClick, Image itemImage)
        {
            this.string_0 = text;
            this.string_1 = mainMenu;
            this.string_2 = subMenu;
            this.clickMenuItem_0 = onClick;
            this.image_0 = itemImage;
        }

        public Image ItemImage
        {
            get
            {
                return this.image_0;
            }
        }

        public string MainMenuItemText
        {
            get
            {
                return this.string_1;
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
                return this.string_2;
            }
        }

        public string Text
        {
            get
            {
                return this.string_0;
            }
        }
    }
}

