namespace WealthLab
{
    using System;

    public abstract class MenuItemHook
    {
        protected MenuItemHook()
        {
        }

        public abstract void AddMenuItems(IMenuItemAdder adder);
    }
}

