namespace QWhale.Editor.Dialogs
{
    using QWhale.Syntax;
    using System;

    public interface IPersistentSettings : IImport, IExport
    {
        void Assign(IPersistentSettings source);
    }
}

