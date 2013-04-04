namespace QWhale.Editor.TextSource
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public interface IUndo
    {
        event QWhale.Editor.TextSource.UndoEvent UndoEvent;

        void AddUndo(UndoOperation operation, object data);
        int BeginUndoUpdate();
        bool CanRedo();
        bool CanUndo();
        void ClearRedo();
        void ClearUndo();
        int DisableUndo();
        int EnableUndo();
        int EndUndoUpdate();
        bool LineIsModified(int index);
        bool LineIsModified(int index, out bool saved);
        void Redo();
        void ResetUndoLimit();
        void ResetUndoOptions();
        void Undo();
        void Undo(IUndoData undoData);

        IUndoList RedoList { get; }

        int UndoLimit { get; set; }

        IUndoList UndoList { get; }

        QWhale.Editor.TextSource.UndoOptions UndoOptions { get; set; }

        int UndoUpdateCount { get; }
    }
}

