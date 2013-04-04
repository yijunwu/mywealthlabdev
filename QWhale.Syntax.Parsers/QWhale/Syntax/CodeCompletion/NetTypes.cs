namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public class NetTypes
    {
        private static Hashtable assemblies = new Hashtable();
        private static Hashtable globalTypes = new Hashtable();
        private static Hashtable namespaces = new Hashtable();
        private static Hashtable types = new Hashtable();

        public static void AddAssembly(Assembly assembly)
        {
            if (!assemblies.Contains(assembly))
            {
                assemblies[assembly] = assembly;
                try
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (!IsSpecialType(type))
                        {
                            AddType(type, false);
                            if ((type.Namespace != null) && (type.Namespace != string.Empty))
                            {
                                AddNamespace(type.Namespace, assembly);
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public static void AddNamespace(string nspace, Assembly assembly)
        {
            object obj2 = namespaces[nspace];
            Hashtable hashtable = (obj2 != null) ? ((Hashtable) obj2) : new Hashtable();
            hashtable[assembly] = assembly;
            if (obj2 == null)
            {
                namespaces[nspace] = hashtable;
            }
        }

        public static void AddType(Type type, bool global)
        {
            types[GetGenericName(type)] = type;
            if (global)
            {
                globalTypes[type] = type;
            }
        }

        protected static string GetGenericName(Type type)
        {
            string fullName = type.FullName;
            if (type.IsGenericType)
            {
                int index = fullName.IndexOf('`');
                if (index >= 0)
                {
                    fullName = fullName.Substring(0, index);
                }
            }
            return fullName;
        }

        public static int GetGlobalTypes(IList<Type> types, string nspace, bool caseSensitive)
        {
            types.Clear();
            foreach (Type type in globalTypes.Keys)
            {
                if (string.Compare(type.Namespace, nspace, !caseSensitive) == 0)
                {
                    types.Add(type);
                }
            }
            return types.Count;
        }

        public static IList<string> GetNamespaces(IList<Assembly> assemblies)
        {
            IList<string> list = new List<string>();
            IDictionaryEnumerator enumerator = namespaces.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                Hashtable hashtable = (Hashtable) enumerator.Value;
                foreach (Assembly assembly in assemblies)
                {
                    if (hashtable.Contains(assembly))
                    {
                        list.Add(enumerator.Key.ToString());
                        continue;
                    }
                }
            }
            return list;
        }

        public static object GetTypeByName(string name, IList<Assembly> assemblies, bool caseSensitive)
        {
            object obj2 = types[name];
            if (obj2 != null)
            {
                if ((obj2 != null) && assemblies.Contains(((Type) obj2).Assembly))
                {
                    return obj2;
                }
                return null;
            }
            try
            {
                obj2 = Type.GetType(name, false, !caseSensitive);
                if (obj2 == null)
                {
                    foreach (Assembly assembly in assemblies)
                    {
                        obj2 = assembly.GetType(name, false, !caseSensitive);
                        if ((obj2 != null) && !IsSpecialType((Type) obj2))
                        {
                            return obj2;
                        }
                    }
                }
                return obj2;
            }
            catch
            {
            }
            return obj2;
        }

        public static bool IsSpecialType(Type type)
        {
            if (((!type.IsSpecialName && !type.IsNotPublic) && (!type.IsNestedPublic && !type.IsNestedPrivate)) && ((!type.IsNestedAssembly && !type.IsNestedFamily) && (!type.Name.StartsWith("$$") && !type.Name.StartsWith("<"))))
            {
                return type.Name.StartsWith("__");
            }
            return true;
        }

        public static void RemoveAssembly(Assembly assembly)
        {
            if (assemblies.Contains(assembly))
            {
                try
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (!type.IsSpecialName && !type.Name.StartsWith("_"))
                        {
                            RemoveType(type);
                            if ((type.Namespace != null) && (type.Namespace != string.Empty))
                            {
                                RemoveNamespace(type.Namespace, assembly);
                            }
                        }
                    }
                }
                catch
                {
                }
                assemblies.Remove(assembly);
            }
        }

        public static void RemoveNamespace(string nspace, Assembly assembly)
        {
            object obj2 = namespaces[nspace];
            if (obj2 != null)
            {
                Hashtable hashtable = (Hashtable) obj2;
                if (hashtable.Contains(assembly))
                {
                    hashtable.Remove(assembly);
                }
                if (hashtable.Count == 0)
                {
                    namespaces.Remove(nspace);
                }
            }
        }

        public static void RemoveType(Type type)
        {
            string genericName = GetGenericName(type);
            if (types.Contains(genericName))
            {
                types.Remove(genericName);
            }
            if (globalTypes.Contains(type))
            {
                globalTypes.Remove(type);
            }
        }

        public static Hashtable Assemblies
        {
            get
            {
                return assemblies;
            }
        }

        public static Hashtable GlobalTypes
        {
            get
            {
                return globalTypes;
            }
        }

        public static Hashtable Namespaces
        {
            get
            {
                return namespaces;
            }
        }

        public static Hashtable Types
        {
            get
            {
                return types;
            }
        }
    }
}

