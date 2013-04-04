namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    public class MacroKeyList : List<IMacroKeyData>, IMacroKeyList, IList<IMacroKeyData>, ICollection<IMacroKeyData>, IEnumerable<IMacroKeyData>, IEnumerable
    {
        private object FindTarget(object owner, string target)
        {
            if (owner != null)
            {
                if (owner.GetType().Name == target)
                {
                    return owner;
                }
                foreach (PropertyInfo info in owner.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if ((info.PropertyType.Name == target) || (info.PropertyType.Name == ("I" + target)))
                    {
                        return info.GetValue(owner, null);
                    }
                }
            }
            return null;
        }

        public void LinkMacros(object owner, IEventHandlers keyHandlers)
        {
            foreach (IMacroKeyData data in this)
            {
                if ((data.FullName != null) && (data.FullName != string.Empty))
                {
                    data.Action = null;
                    data.ActionEx = null;
                    int length = data.FullName.LastIndexOf('.');
                    if (length >= 0)
                    {
                        object obj2 = this.FindTarget(owner, data.FullName.Substring(0, length));
                        if (obj2 != null)
                        {
                            string str = data.FullName.Substring(length + 1);
                            foreach (FieldInfo info in keyHandlers.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                            {
                                object obj3 = info.GetValue(keyHandlers);
                                if (obj3 is KeyEvent)
                                {
                                    KeyEvent event2 = (KeyEvent) obj3;
                                    if ((event2.Target == obj2) && (event2.Method.Name == str))
                                    {
                                        data.Action = event2;
                                        break;
                                    }
                                }
                                if (obj3 is KeyEventEx)
                                {
                                    KeyEventEx ex = (KeyEventEx) obj3;
                                    if ((ex.Target == obj2) && (ex.Method.Name == str))
                                    {
                                        data.ActionEx = ex;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if ((data.ParamType != KeyEventType.None) && (data.ParamStr != string.Empty))
                {
                    switch (data.ParamType)
                    {
                        case KeyEventType.IntParam:
                            data.Param = int.Parse(data.ParamStr);
                            break;

                        case KeyEventType.CharParam:
                            data.Param = (char) int.Parse(data.ParamStr);
                            break;

                        case KeyEventType.SelectionParam:
                            if (string.Compare(data.ParamStr, SelectionType.Block.ToString(), true) != 0)
                            {
                                goto Label_01C9;
                            }
                            data.Param = SelectionType.Block;
                            break;
                    }
                }
                continue;
            Label_01C9:
                if (string.Compare(data.ParamStr, SelectionType.Stream.ToString(), true) == 0)
                {
                    data.Param = SelectionType.Stream;
                }
                else
                {
                    data.Param = SelectionType.None;
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlMacroKeysDataInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

