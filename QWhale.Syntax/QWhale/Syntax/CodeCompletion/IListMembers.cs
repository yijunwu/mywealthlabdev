namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IListMembers : ICodeCompletionProvider, IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
        IListMember AddListMember();
        IListMember CreateListMember();
        IListMember InsertListMember(int index);
        void ResetShowHints();
        void ResetShowParams();
        void ResetShowQualifiers();
        void ResetShowResults();

        IListMember this[int index] { get; }

        bool ShowHints { get; set; }

        bool ShowParams { get; set; }

        bool ShowQualifiers { get; set; }

        bool ShowResults { get; set; }
    }
}

