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
            bool_0 = true;
            lock (list_0)
            {
                foreach (TreeNode node2 in base.Nodes)
                {
                    if (node2.Level == 0)
                    {
                        if (list_0.Contains(node2.Text))
                        {
                            flag = true;
                            node2.Expand();
                        }
                        else
                        {
                            node2.Collapse();
                        }
                    }
                }
            }
            bool_0 = false;
            if (!flag)
            {
                //using (IEnumerator enumerator = base.Nodes.GetEnumerator())
                IEnumerator enumerator = base.Nodes.GetEnumerator();
                {
                    TreeNode current;
                    while (enumerator.MoveNext())
                    {
                        current = (TreeNode) enumerator.Current;
                        if ((current.Level == 0) && (current.Text == this.string_0))
                        {
                            goto Label_00DB;
                        }
                    }
                    return;
                Label_00DB:
                    current.Expand();
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

