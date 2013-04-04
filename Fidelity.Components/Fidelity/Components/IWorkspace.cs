namespace Fidelity.Components
{
    using System;
    using System.Collections.Generic;

    public interface IWorkspace
    {
        void LoadWorkspaceItems(IList<string> items, int version);
        int SaveWorkspaceItems(IList<string> items);
    }
}

