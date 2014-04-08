namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class DataBehaviorUserControl : UserControl
    {
        private IContainer icontainer_0;
        private IDataHost idataHost_0;
        private List<IExtendedBehaviorHost> list_0 = new List<IExtendedBehaviorHost>();

        public DataBehaviorUserControl()
        {
            this.InitializeComponent();
        }

        public void CreateNewDataSet()
        {
            foreach (IExtendedBehaviorHost host in this.list_0)
            {
                host.CreateNewDataSet();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public virtual void Initialize(IDataHost dataHost)
        {
            this.idataHost_0 = dataHost;
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Name = "DataBehaviorUserControl";
            base.Size = new Size(410, 0x12b);
            base.ResumeLayout(false);
        }

        public void RegisterObserver(IExtendedBehaviorHost observer)
        {
            this.list_0.Add(observer);
        }

        public void SetFirstTab()
        {
            foreach (IExtendedBehaviorHost host in this.list_0)
            {
                host.SetFirstTab();
            }
        }

        public void UnregisterObserver(IExtendedBehaviorHost observer)
        {
            this.list_0.Remove(observer);
        }

        public IDataHost DataHost
        {
            get
            {
                return this.idataHost_0;
            }
        }
    }
}

