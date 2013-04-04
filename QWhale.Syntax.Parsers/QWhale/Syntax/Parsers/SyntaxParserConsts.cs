namespace QWhale.Syntax.Parsers
{
    using QWhale.Syntax;
    using System;

    public class SyntaxParserConsts
    {
        public const string AbstractModifier = "abstract";
        public static string ClassDataType = "class";
        public const int ClassIndex = 3;
        public const int ConstIndex = 4;
        public const string Constructor = ".ctor";
        public static string DefaultCAutoIndentChars = ":{\r\n";
        public static string DefaultCCodeCompletionChars = ".([/ ";
        public static string DefaultCCodeCompletionStopChars = ")]";
        public static string DefaultCsAutoIndentChars = ":{\r\n";
        public static string DefaultCsCodeCompletionChars = ".(<>[/ ";
        public static string DefaultCsCodeCompletionStopChars = ")]";
        public static string DefaultCSmartFormatChars = "};";
        public static string DefaultCsSmartFormatChars = "};";
        public static string DefaultNetAutoIndentChars = "\r\n";
        public static string DefaultNetCodeCompletionChars = ".(";
        public static string DefaultNetCodeCompletionStopChars = ")";
        public static SyntaxOptions DefaultNetSyntaxOptions = (SyntaxOptions.FormatSpaces | SyntaxOptions.FormatCase | SyntaxOptions.AutoComplete | SyntaxOptions.SyntaxErrors | SyntaxOptions.CodeCompletion | SyntaxOptions.SmartIndent | SyntaxOptions.Outline);
        public static string DefaultVbCodeCompletionChars = ".(=,<>' ";
        public static string DefaultVbScriptCodeCompletionChars = ".( ";
        public static string DefaultVbSmartFormatChars = "";
        public static SyntaxOptions DefaultVbSyntaxOptions = ((DefaultNetSyntaxOptions | SyntaxOptions.ReparseOnLineChange) | SyntaxOptions.CodeCompletionTabs);
        public const int DelegateIndex = 5;
        public const string ElementAccess = ".ElementAccess";
        public static string EnumDataType = "enum";
        public const int EnumIndex = 6;
        public const int EnumMemberIndex = 1;
        public const int EventIndex = 7;
        public static string ExpressionTag = "Expression";
        public static string ExtendedNetCodeCompletionChars = ".(abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const int FieldIndex = 8;
        public static string InterfaceDataType = "interface";
        public const int InterfaceIndex = 9;
        public const string InternalModifier = "internal";
        public const int MethodIndex = 10;
        public static string NamespaceDataType = "namespace";
        public const int NamespaceIndex = 2;
        public static string NameTag = "name";
        public static string OutlineCommentText = "/**/";
        public static string OutlineImportsText = "imports";
        public static string OutlineImportText = "import";
        public static string OutlineUsingText = "using";
        public const string OutParameter = "out";
        public const string PartialModifier = "partial";
        public const string PrivateModifier = "private";
        public const string PropertyGetModifier = "get";
        public const int PropertyIndex = 11;
        public const string PropertySetModifier = "get";
        public const string ProtectedModifier = "protected";
        public const string PublicModifier = "public";
        public const string RefParameter = "ref";
        public const string SharedModifier = "shared";
        public const int SnippetIndex = 0x2c;
        public const int SnippetStatementIndex = 0x2b;
        public const string StaticModifier = "static";
        public static string StructDataType = "struct";
        public const int StructIndex = 12;
        public static string StructureResWord = "Structure";
        public static string TypeDataType = "type";
        public const int TypeIndex = 3;
        public static string TypeTag = "Type";
        public const string ValueModifier = "value";
        public const string VirtualModifier = "virtual";
        public const string XmlCommentStart = "///";
        public static string XmlComplexTag = "<{0} {1}=\"{2}\"></{0}>";
        public static string XmlEndTag = "</{0}>";
        public static string XmlFormatTag = "&lt;/{0}&gt;";
        public const int XmlIndex = 40;
        public static string XmlTag = "<{0}>";
        public const string XmlVbCommentStart = "'''";
    }
}

