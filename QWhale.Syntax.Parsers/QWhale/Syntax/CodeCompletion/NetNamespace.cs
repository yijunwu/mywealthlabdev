namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public class NetNamespace : INetNamespace
    {
        private string alias = string.Empty;
        private string nspace = string.Empty;
        private bool system;

        public NetNamespace(string nspace, bool system)
        {
            this.nspace = nspace;
            this.system = system;
        }

        public virtual string GetName()
        {
            if (!(this.alias != string.Empty))
            {
                return this.nspace;
            }
            return this.alias;
        }

        protected virtual void OnAliasChanged()
        {
        }

        protected virtual void OnNamespaceChanged()
        {
        }

        protected virtual void OnSystemChanged()
        {
        }

        public virtual string Alias
        {
            get
            {
                return this.alias;
            }
            set
            {
                if (this.alias != value)
                {
                    this.alias = value;
                    this.OnAliasChanged();
                }
            }
        }

        public virtual string Namespace
        {
            get
            {
                return this.nspace;
            }
            set
            {
                if (this.nspace != value)
                {
                    this.nspace = value;
                    this.OnNamespaceChanged();
                }
            }
        }

        public virtual bool System
        {
            get
            {
                return this.system;
            }
            set
            {
                if (this.system != value)
                {
                    this.system = value;
                    this.OnSystemChanged();
                }
            }
        }
    }
}

