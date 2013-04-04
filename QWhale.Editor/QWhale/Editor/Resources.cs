namespace QWhale.Editor
{
    using System;
    using System.ComponentModel;

    [ToolboxItem(false)]
    public class Resources : Component
    {
        private Container components;

        public Resources()
        {
            this.InitializeComponent();
        }

        public Resources(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }
    }
}

