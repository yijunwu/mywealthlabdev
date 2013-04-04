namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IReflectionRepository : ICodeCompletionRepository
    {
        void AllowTypeMembers(Type type);
        void ClearAssemblies();
        void ClearNamespaces();
        void ClearObjects();
        void ClearTypes();
        void RegisterAllAssemblies();
        void RegisterAssembly(Assembly assembly);
        bool RegisterAssembly(string name);
        void RegisterDefaultAssemblies();
        void RegisterNamespace(string nspace);
        void RegisterObject(string name, object obj);
        void RegisterType(string name, Type type);
        void RegisterType(string name, Type type, bool global);
        void RestrictTypeMembers(Type type);
        bool UnregisterAssembly(Assembly assembly, bool removeReferences);
        bool UnregisterAssembly(string name, bool removeReferences);
        bool UnregisterNamespace(string nspace);
        bool UnregisterObject(string name);
        bool UnregisterType(string name);

        IList<Assembly> Assemblies { get; }

        IList<string> Namespaces { get; }

        Hashtable Objects { get; }

        Hashtable Types { get; }
    }
}

