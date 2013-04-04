namespace QWhale.Editor
{
    using System;
    using System.Windows.Forms;

    public class KeyData : IKeyData
    {
        private KeyEvent action;
        private KeyEventEx actionEx;
        private System.Windows.Forms.Keys keys;
        private int leaveState;
        private object param;
        private int state;

        public KeyData()
        {
        }

        public KeyData(System.Windows.Forms.Keys keys, KeyEvent action, KeyEventEx actionEx, object param, int state, int leaveState)
        {
            this.keys = keys;
            this.action = action;
            this.actionEx = actionEx;
            this.param = param;
            this.state = state;
            this.leaveState = leaveState;
        }

        public override string ToString()
        {
            if (this.Action != null)
            {
                return (this.Action.Target.GetType().Name + "." + this.Action.Method.Name);
            }
            if (this.ActionEx != null)
            {
                return (this.ActionEx.Target.GetType().Name + "." + this.ActionEx.Method.Name + "(" + this.Param.ToString() + ")");
            }
            return base.ToString();
        }

        public KeyEvent Action
        {
            get
            {
                return this.action;
            }
            set
            {
                this.action = value;
            }
        }

        public KeyEventEx ActionEx
        {
            get
            {
                return this.actionEx;
            }
            set
            {
                this.actionEx = value;
            }
        }

        public string EventName
        {
            get
            {
                if (this.Action != null)
                {
                    return this.Action.Method.Name;
                }
                if (this.ActionEx != null)
                {
                    return this.ActionEx.Method.Name;
                }
                return string.Empty;
            }
            set
            {
            }
        }

        public System.Windows.Forms.Keys Keys
        {
            get
            {
                return this.keys;
            }
            set
            {
                this.keys = value;
            }
        }

        public int LeaveState
        {
            get
            {
                return this.leaveState;
            }
            set
            {
                this.leaveState = value;
            }
        }

        public object Param
        {
            get
            {
                return this.param;
            }
            set
            {
                this.param = value;
            }
        }

        public int State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }
    }
}

