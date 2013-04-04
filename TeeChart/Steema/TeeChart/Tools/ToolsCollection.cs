namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors.Tools;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Reflection;

    [Serializable, Editor(typeof(ToolsCollection.Editor), typeof(UITypeEditor))]
    public class ToolsCollection : CollectionBase
    {
        public Chart chart;

        public ToolsCollection(Chart c)
        {
            this.chart = c;
        }

        public int Add(Tool tool)
        {
            tool.Chart = this.chart;
            int index = this.IndexOf(tool);
            if ((index == -1) && !tool.InternalUse)
            {
                index = base.List.Add(tool);
            }
            return index;
        }

        public int Add(Type tool)
        {
            return this.Add(Tool.NewFromType(tool));
        }

        public void Clear()
        {
            this.Clear(true);
        }

        public void Clear(bool dispose)
        {
            while (base.Count > 0)
            {
                Tool tool = this[0];
                base.RemoveAt(0);
                tool.OnDisposing();
                if (dispose)
                {
                    tool.Dispose();
                }
            }
            base.Clear();
            this.chart.Invalidate();
        }

        public int IndexOf(Tool s)
        {
            return base.List.IndexOf(s);
        }

        internal void MoveDown(Tool t1)
        {
            int index = this.IndexOf(t1);
            if ((index >= 0) && (index < (this.chart.Tools.Count - 1)))
            {
                Tool tool = this.chart.Tools[index];
                base.RemoveAt(index);
                if (index == (this.chart.Tools.Count - 1))
                {
                    base.InnerList.Add(tool);
                }
                else
                {
                    base.InnerList.Insert(index + 1, tool);
                }
                this.chart.Invalidate();
            }
        }

        internal void MoveUp(Tool t1)
        {
            int index = this.IndexOf(t1);
            if (index > 0)
            {
                Tool tool = this.chart.Tools[index];
                base.RemoveAt(index);
                base.InnerList.Insert(index - 1, tool);
                this.chart.Invalidate();
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            ((Tool) value).chart = this.chart;
        }

        public void Remove(Tool s)
        {
            int index = this.IndexOf(s);
            if (index != -1)
            {
                base.RemoveAt(index);
                s.OnDisposing();
                s.Dispose();
                this.chart.Invalidate();
            }
        }

        public Tool this[int index]
        {
            get
            {
                return (Tool) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        internal sealed class Editor : CollectionEditor
        {
            public Editor(Type type) : base(type)
            {
            }

            protected override object CreateInstance(Type itemType)
            {
                Chart instance = null;
                if (base.Context.Instance is Chart)
                {
                    instance = (Chart) base.Context.Instance;
                }
                else if (base.Context.Instance is TChart)
                {
                    instance = ((TChart) base.Context.Instance).Chart;
                }
                if (instance != null)
                {
                    return ToolsGallery.CreateNew(instance, base.Context.Container);
                }
                return null;
            }
        }
    }
}

