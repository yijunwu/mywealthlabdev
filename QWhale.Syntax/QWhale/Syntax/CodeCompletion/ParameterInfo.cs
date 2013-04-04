namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ParameterInfo : ListMembers, IParameterInfo, IListMembers, ICodeCompletionProvider, IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
        public override bool FormatDisplayText
        {
            get
            {
                return this.UseHtmlFormatting;
            }
        }
    }
}

