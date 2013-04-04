namespace Fidelity.Components
{
    using System;
    using System.Windows.Forms;

    public class ProcessNodeEventArgs : EventArgs
    {
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private static ProcessNodeEventArgs processNodeEventArgs_0;
        private TreeNode treeNode_0;

        private ProcessNodeEventArgs()
        {
        }

        internal static ProcessNodeEventArgs smethod_0(TreeNode treeNode_1)
        {
            if (processNodeEventArgs_0 == null)
            {
                processNodeEventArgs_0 = new ProcessNodeEventArgs();
            }
            processNodeEventArgs_0.treeNode_0 = treeNode_1;
            processNodeEventArgs_0.bool_0 = true;
            processNodeEventArgs_0.bool_1 = true;
            processNodeEventArgs_0.bool_2 = false;
            return processNodeEventArgs_0;
        }

        public TreeNode Node
        {
            get
            {
                return this.treeNode_0;
            }
        }

        public bool ProcessDescendants
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public bool ProcessSiblings
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public bool StopProcessing
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
            }
        }
    }
}

