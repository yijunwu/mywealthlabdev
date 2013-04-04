namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class XmlMacroKeyDataInfo : ISerializationInfo
    {
        private string eventName;
        private string fullName;
        private System.Windows.Forms.Keys keys;
        private int leaveState;
        private IMacroKeyData owner;
        private string paramStr;
        private KeyEventType paramType;
        private int state;

        public XmlMacroKeyDataInfo()
        {
            this.eventName = string.Empty;
            this.fullName = string.Empty;
            this.paramStr = string.Empty;
        }

        public XmlMacroKeyDataInfo(IMacroKeyData owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IMacroKeyData) owner;
            this.EventName = this.eventName;
            this.FullName = this.fullName;
            this.Keys = this.keys;
            this.State = this.state;
            this.LeaveState = this.leaveState;
            this.ParamType = this.paramType;
            this.ParamStr = this.paramStr;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.eventName = this.EventName;
                this.fullName = this.FullName;
                this.keys = this.Keys;
                this.state = this.State;
                this.leaveState = this.LeaveState;
                this.paramType = this.ParamType;
                this.paramStr = this.ParamStr;
            }
        }

        [DefaultValue("")]
        public string EventName
        {
            get
            {
                if (this.owner == null)
                {
                    return this.eventName;
                }
                return this.owner.EventName;
            }
            set
            {
                this.eventName = value;
                if (this.owner != null)
                {
                    this.owner.EventName = value;
                }
            }
        }

        [DefaultValue("")]
        public string FullName
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fullName;
                }
                return this.owner.FullName;
            }
            set
            {
                this.fullName = value;
                if (this.owner != null)
                {
                    this.owner.FullName = value;
                }
            }
        }

        [DefaultValue(0)]
        public System.Windows.Forms.Keys Keys
        {
            get
            {
                if (this.owner == null)
                {
                    return this.keys;
                }
                return this.owner.Keys;
            }
            set
            {
                this.keys = value;
                if (this.owner != null)
                {
                    this.owner.Keys = value;
                }
            }
        }

        [DefaultValue(0)]
        public int LeaveState
        {
            get
            {
                if (this.owner == null)
                {
                    return this.leaveState;
                }
                return this.owner.LeaveState;
            }
            set
            {
                this.leaveState = value;
                if (this.owner != null)
                {
                    this.owner.LeaveState = value;
                }
            }
        }

        [DefaultValue("")]
        public string ParamStr
        {
            get
            {
                if (this.owner == null)
                {
                    return this.paramStr;
                }
                return this.owner.ParamStr;
            }
            set
            {
                this.paramStr = value;
                if (this.owner != null)
                {
                    this.owner.ParamStr = value;
                }
            }
        }

        [DefaultValue(0)]
        public KeyEventType ParamType
        {
            get
            {
                if (this.owner == null)
                {
                    return this.paramType;
                }
                return this.owner.ParamType;
            }
            set
            {
                this.paramType = value;
                if (this.owner != null)
                {
                    this.owner.ParamType = value;
                }
            }
        }

        [DefaultValue(0)]
        public int State
        {
            get
            {
                if (this.owner == null)
                {
                    return this.state;
                }
                return this.owner.State;
            }
            set
            {
                this.state = value;
                if (this.owner != null)
                {
                    this.owner.State = value;
                }
            }
        }
    }
}

