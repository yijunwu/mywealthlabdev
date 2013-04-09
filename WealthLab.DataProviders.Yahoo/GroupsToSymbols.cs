using System;
using WealthLab.DataProviders.Helper;

///WYJ fix, original name: Class22
internal class GroupsToSymbols
{
    private SymbolList deleteList = new SymbolList();  ///WYJ note, symbols deleted
    private SymbolList addList = new SymbolList();  ///WYJ note, symbols added
    private ClassificationGroup classificationGroup_0;
    private string symbols;

    public GroupsToSymbols(ClassificationGroup classificationGroup_1)
    {
        this.classificationGroup_0 = classificationGroup_1;
    }

    public SymbolList GetDeleteList()
    {
        return this.deleteList;
    }

    public SymbolList GetAddList()
    {
        return this.addList;
    }

    ///WYJ fix, original name: method_2
    private void getSymbolsOfGroup(string groupId, ClassificationGroup classificationGroup_1) ///WYJ note, get Symbols of group, the symbols are saved to this.symbols
    {
        for (int i = 0; i < classificationGroup_1.Groups.Count; i++)
        {
            if ((classificationGroup_1.Groups[i].Type == "Symbols") && (classificationGroup_1.Groups[i].ID == groupId))
            {
                this.symbols = classificationGroup_1.Groups[i].Symbols;
                return;
            }
            else 
                this.getSymbolsOfGroup(groupId, classificationGroup_1.Groups[i]);
        }
    }

    ///WYJ fix, original name: method_3
    public SymbolList generateNewSymbolList(SymbolList symbolList, params string[] groupArray)  
    {
        Logger.LogParameters(new object[] { symbolList.ToString() });

        //1. Get symbols of all the groups specified in groupArray
        SymbolList newSymbolList = new SymbolList();
        foreach (string group in groupArray)
        {
            this.symbols = string.Empty;
            this.getSymbolsOfGroup(group, this.classificationGroup_0);
            newSymbolList.AddSymbols(this.symbols, Enum0.const_1);
        }
        this.deleteList.list.Clear();
        this.addList.list.Clear();
        foreach (string symbol in newSymbolList.list)
        {
            if (!symbolList.list.Contains(symbol))
            {
                this.addList.list.Add(symbol);
            }
        }
        foreach (string symbol in symbolList.list)
        {
            if (!newSymbolList.list.Contains(symbol))
            {
                this.deleteList.list.Add(symbol);
            }
        }
        return newSymbolList;
    }
}

