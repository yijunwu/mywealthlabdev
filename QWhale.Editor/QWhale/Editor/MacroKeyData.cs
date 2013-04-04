namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class MacroKeyData : KeyData, IMacroKeyData, IKeyData
    {
        private string fullName;
        private string paramStr;
        private KeyEventType paramType;

        public MacroKeyData()
        {
            this.fullName = string.Empty;
            this.paramStr = string.Empty;
        }

        public MacroKeyData(Keys keyData, KeyEvent action, KeyEventEx actionEx, object param, int state, int leaveState) : base(keyData, action, actionEx, param, state, leaveState)
        {
            this.fullName = string.Empty;
            this.paramStr = string.Empty;
        }

        protected virtual void OnFullNameChanged()
        {
        }

        protected virtual void OnParamStrChanged()
        {
        }

        protected virtual void OnParamTypeChanged()
        {
        }

        public string FullName
        {
            get
            {
                if (base.Action != null)
                {
                    return (base.Action.Target.GetType().Name + "." + base.Action.Method.Name);
                }
                if (base.ActionEx != null)
                {
                    return (base.ActionEx.Target.GetType().Name + "." + base.ActionEx.Method.Name);
                }
                return this.fullName;
            }
            set
            {
                if (this.fullName != value)
                {
                    this.fullName = value;
                    this.OnFullNameChanged();
                }
            }
        }

        public string ParamStr
        {
            get
            {
                if (base.Param == null)
                {
                    return this.paramStr;
                }
                if (!(base.Param is char))
                {
                    return base.Param.ToString();
                }
                int param = (char) base.Param;
                return param.ToString();
            }
            set
            {
                if (this.paramStr != value)
                {
                    this.paramStr = value;
                    this.OnParamStrChanged();
                }
            }
        }

        public KeyEventType ParamType
        {
            get
            {
                if (base.Param is int)
                {
                    return KeyEventType.IntParam;
                }
                if (base.Param is char)
                {
                    return KeyEventType.CharParam;
                }
                if (base.Param is SelectionType)
                {
                    return KeyEventType.SelectionParam;
                }
                return this.paramType;
            }
            set
            {
                if (this.paramType != value)
                {
                    this.paramType = value;
                    this.OnParamTypeChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlMacroKeyDataInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

