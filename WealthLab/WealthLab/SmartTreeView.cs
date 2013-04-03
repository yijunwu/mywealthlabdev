namespace WealthLab
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class SmartTreeView : TreeView
    {
        private static bool bool_0 = false;
        private static List<string> list_0 = new List<string>();
        private string string_0 = "";

        protected override void OnAfterCollapse(TreeViewEventArgs treeViewEventArgs_0)
        {
            base.OnAfterCollapse(treeViewEventArgs_0);
            if (!bool_0)
            {
                lock (list_0)
                {
                    if (list_0.Contains(treeViewEventArgs_0.Node.Text))
                    {
                        list_0.Remove(treeViewEventArgs_0.Node.Text);
                    }
                }
            }
        }

        protected override void OnAfterExpand(TreeViewEventArgs treeViewEventArgs_0)
        {
            base.OnAfterExpand(treeViewEventArgs_0);
            if (!bool_0)
            {
                lock (list_0)
                {
                    if (!list_0.Contains(treeViewEventArgs_0.Node.Text))
                    {
                        list_0.Add(treeViewEventArgs_0.Node.Text);
                    }
                }
            }
        }

        public void RecallExpandState()
        {
            bool flag = false;
            SmartTreeView.bool_0 = true;
            lock (SmartTreeView.list_0)
            {
                foreach (TreeNode node in base.Nodes)
                {
                    if (node.Level != 0)
                    {
                        continue;
                    }
                    if (!SmartTreeView.list_0.Contains(node.Text))
                    {
                        node.Collapse();
                    }
                    else
                    {
                        flag = true;
                        node.Expand();
                    }
                }
            }
            SmartTreeView.bool_0 = false;
            if (!flag)
            {
                IEnumerator enumerator = base.Nodes.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            TreeNode current = (TreeNode)enumerator.Current;
                            if (current.Level == 0 && current.Text == this.string_0)
                            {
                                current.Expand();
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }


        public string StandardNodeName
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }
    }
}

