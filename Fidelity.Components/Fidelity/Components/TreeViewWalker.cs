using System;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace Fidelity.Components
{
    public class TreeViewWalker
    {
        private TreeView treeView_0;

        private bool bool_0;

        private ProcessNodeEventHandler processNodeEventHandler_0;

        public TreeView TreeView
        {
            get
            {
                return this.treeView_0;
            }
            set
            {
                this.treeView_0 = value;
            }
        }

        public TreeViewWalker()
        {
        }

        public TreeViewWalker(TreeView treeView)
        {
            this.treeView_0 = treeView;
        }

        private bool method_0(TreeNode treeNode_0)
        {
            ProcessNodeEventArgs processNodeEventArg = ProcessNodeEventArgs.smethod_0(treeNode_0);
            this.OnProcessNode(processNodeEventArg);
            bool processSiblings = processNodeEventArg.ProcessSiblings;
            if (!processNodeEventArg.StopProcessing)
            {
                if (processNodeEventArg.ProcessDescendants)
                {
                    int num = 0;
                    while (num < treeNode_0.Nodes.Count && this.method_0(treeNode_0.Nodes[num]) && !this.bool_0)
                    {
                        num++;
                    }
                }
            }
            else
            {
                this.bool_0 = true;
            }
            return processSiblings;
        }

        protected virtual void OnProcessNode(ProcessNodeEventArgs processNodeEventArgs_0)
        {
            ProcessNodeEventHandler processNodeEventHandler0 = this.processNodeEventHandler_0;
            if (processNodeEventHandler0 != null)
            {
                processNodeEventHandler0(this, processNodeEventArgs_0);
            }
        }

        public void ProcessBranch(TreeNode rootNode)
        {
            if (rootNode != null)
            {
                this.bool_0 = false;
                this.method_0(rootNode);
                return;
            }
            else
            {
                throw new ArgumentNullException("rootNode");
            }
        }

        public void ProcessTree()
        {
            if (this.TreeView != null)
            {
                IEnumerator enumerator = this.TreeView.Nodes.GetEnumerator();
                try
                {
                    do
                    {
                        if (!enumerator.MoveNext())
                        {
                            break;
                        }
                        TreeNode current = (TreeNode)enumerator.Current;
                        this.ProcessBranch(current);
                    }
                    while (!this.bool_0);
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                return;
            }
            else
            {
                throw new InvalidOperationException("The TreeViewWalker must reference a TreeView when ProcessTree is called.");
            }
        }

        public event ProcessNodeEventHandler ProcessNode
        {
            add
            {
                ProcessNodeEventHandler processNodeEventHandler;
                ProcessNodeEventHandler processNodeEventHandler0 = this.processNodeEventHandler_0;
                do
                {
                    processNodeEventHandler = processNodeEventHandler0;
                    ProcessNodeEventHandler processNodeEventHandler1 = (ProcessNodeEventHandler)Delegate.Combine(processNodeEventHandler, value);
                    processNodeEventHandler0 = Interlocked.CompareExchange<ProcessNodeEventHandler>(ref this.processNodeEventHandler_0, processNodeEventHandler1, processNodeEventHandler);
                }
                while (processNodeEventHandler0 != processNodeEventHandler);
            }
            remove
            {
                ProcessNodeEventHandler processNodeEventHandler;
                ProcessNodeEventHandler processNodeEventHandler0 = this.processNodeEventHandler_0;
                do
                {
                    processNodeEventHandler = processNodeEventHandler0;
                    ProcessNodeEventHandler processNodeEventHandler1 = (ProcessNodeEventHandler)Delegate.Remove(processNodeEventHandler, value);
                    processNodeEventHandler0 = Interlocked.CompareExchange<ProcessNodeEventHandler>(ref this.processNodeEventHandler_0, processNodeEventHandler1, processNodeEventHandler);
                }
                while (processNodeEventHandler0 != processNodeEventHandler);
            }
        }
    }
}

