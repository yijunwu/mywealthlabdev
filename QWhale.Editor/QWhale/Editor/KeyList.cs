namespace QWhale.Editor
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    public class KeyList : IKeyList
    {
        private EventHandlers handlers;
        private ISyntaxEdit owner;
        private Hashtable states;

        public KeyList(ISyntaxEdit owner)
        {
            this.owner = owner;
            this.handlers = new EventHandlers(owner);
            this.states = new Hashtable();
            this.Init();
        }

        public virtual void Add(Keys keys, KeyEvent action)
        {
            this.Add(keys, action, null, null, -1, 0);
        }

        public virtual void Add(Keys keys, KeyEventEx action, object param)
        {
            this.Add(keys, null, action, param, -1, 0);
        }

        public virtual void Add(Keys keys, KeyEvent action, int state, int leaveState)
        {
            this.Add(keys, action, null, null, state, leaveState);
        }

        public virtual void Add(Keys keys, KeyEventEx action, object param, int state, int leaveState)
        {
            this.Add(keys, null, action, param, state, leaveState);
        }

        public virtual void Add(Keys keys, KeyEvent action, KeyEventEx actionEx, object param, int state, int leaveState)
        {
            Hashtable hashtable = (Hashtable) this.states[state];
            if (hashtable == null)
            {
                hashtable = new Hashtable();
                this.states.Add(state, hashtable);
            }
            hashtable[keys] = new KeyData(keys, action, actionEx, param, state, leaveState);
        }

        public virtual void AddNormal(Keys keys, KeyEvent action)
        {
            this.Add(keys, action, null, null, 0, 0);
        }

        public virtual void AddNormal(Keys keys, KeyEventEx action, object param)
        {
            this.Add(keys, null, action, param, 0, 0);
        }

        public virtual void Clear()
        {
            this.states.Clear();
        }

        public virtual bool ExecuteKey(Keys keys, ref int state)
        {
            IKeyData keyData = this.GetKeyData(keys, state);
            if (keyData == null)
            {
                return false;
            }
            if (this.owner.MacroRecording)
            {
                this.owner.RecordKeyData(new MacroKeyData(keyData.Keys, keyData.Action, keyData.ActionEx, keyData.Param, keyData.State, keyData.LeaveState));
            }
            if (keyData.Action != null)
            {
                keyData.Action();
            }
            else if (keyData.ActionEx != null)
            {
                keyData.ActionEx(keyData.Param);
            }
            state = keyData.LeaveState;
            return true;
        }

        public virtual bool FindKey(Keys keys, int state)
        {
            return (this.GetKeyData(keys, state) != null);
        }

        private IKeyData GetKeyData(Keys keys, int state)
        {
            IKeyData data = null;
            Hashtable hashtable = (Hashtable) this.states[state];
            if (hashtable != null)
            {
                data = (IKeyData) hashtable[keys];
            }
            if (data == null)
            {
                hashtable = (Hashtable) this.states[-1];
                if (hashtable != null)
                {
                    data = (IKeyData) hashtable[keys];
                }
            }
            return data;
        }

        private void Init()
        {
            this.InitStdKeys();
            this.InitWordKeys();
            this.InitCodeCompletionKeys();
            this.InitMovements();
            this.InitSelection();
            this.InitOutlining();
            this.InitBookMarks();
            this.InitSearch();
            this.InitClipboard();
            this.InitMacros();
        }

        private void InitBookMarks()
        {
            this.Add(Keys.Control | Keys.K, null, 0, 1);
            this.Add(Keys.Control | Keys.B, null, 0, 5);
            this.Add(Keys.Control | Keys.K, this.handlers.toggleBookMarkEvent, 1, 0);
            this.Add(Keys.Control | Keys.N, this.handlers.nextBookMarkEvent, 1, 0);
            this.Add(Keys.Control | Keys.P, this.handlers.prevBookMarkEvent, 1, 0);
            this.Add(Keys.N, this.handlers.nextBookMarkEvent, 1, 0);
            this.Add(Keys.P, this.handlers.prevBookMarkEvent, 1, 0);
            this.Add(Keys.K, this.handlers.toggleBookMarkEvent, 1, 0);
            this.Add(Keys.N, this.handlers.nextBookMarkEvent, 5, 0);
            this.Add(Keys.Control | Keys.N, this.handlers.nextBookMarkEvent, 5, 0);
            this.Add(Keys.P, this.handlers.prevBookMarkEvent, 5, 0);
            this.Add(Keys.Control | Keys.P, this.handlers.prevBookMarkEvent, 5, 0);
            this.Add(Keys.Control | Keys.D0, this.handlers.toggleBookMarkEventEx, 0, 1, 0);
            this.Add(Keys.Control | Keys.D1, this.handlers.toggleBookMarkEventEx, 1, 1, 0);
            this.Add(Keys.Control | Keys.D2, this.handlers.toggleBookMarkEventEx, 2, 1, 0);
            this.Add(Keys.Control | Keys.D3, this.handlers.toggleBookMarkEventEx, 3, 1, 0);
            this.Add(Keys.Control | Keys.D4, this.handlers.toggleBookMarkEventEx, 4, 1, 0);
            this.Add(Keys.Control | Keys.D5, this.handlers.toggleBookMarkEventEx, 5, 1, 0);
            this.Add(Keys.Control | Keys.D6, this.handlers.toggleBookMarkEventEx, 6, 1, 0);
            this.Add(Keys.Control | Keys.D7, this.handlers.toggleBookMarkEventEx, 7, 1, 0);
            this.Add(Keys.Control | Keys.D8, this.handlers.toggleBookMarkEventEx, 8, 1, 0);
            this.Add(Keys.Control | Keys.D9, this.handlers.toggleBookMarkEventEx, 9, 1, 0);
            this.Add(Keys.D0, this.handlers.toggleBookMarkEventEx, 0, 1, 0);
            this.Add(Keys.D1, this.handlers.toggleBookMarkEventEx, 1, 1, 0);
            this.Add(Keys.D2, this.handlers.toggleBookMarkEventEx, 2, 1, 0);
            this.Add(Keys.D3, this.handlers.toggleBookMarkEventEx, 3, 1, 0);
            this.Add(Keys.D4, this.handlers.toggleBookMarkEventEx, 4, 1, 0);
            this.Add(Keys.D5, this.handlers.toggleBookMarkEventEx, 5, 1, 0);
            this.Add(Keys.D6, this.handlers.toggleBookMarkEventEx, 6, 1, 0);
            this.Add(Keys.D7, this.handlers.toggleBookMarkEventEx, 7, 1, 0);
            this.Add(Keys.D8, this.handlers.toggleBookMarkEventEx, 8, 1, 0);
            this.Add(Keys.D9, this.handlers.toggleBookMarkEventEx, 9, 1, 0);
            this.Add(Keys.Control | Keys.D0, this.handlers.moveBookMarkEvent, 0);
            this.Add(Keys.Control | Keys.D1, this.handlers.moveBookMarkEvent, 1);
            this.Add(Keys.Control | Keys.D2, this.handlers.moveBookMarkEvent, 2);
            this.Add(Keys.Control | Keys.D3, this.handlers.moveBookMarkEvent, 3);
            this.Add(Keys.Control | Keys.D4, this.handlers.moveBookMarkEvent, 4);
            this.Add(Keys.Control | Keys.D5, this.handlers.moveBookMarkEvent, 5);
            this.Add(Keys.Control | Keys.D6, this.handlers.moveBookMarkEvent, 6);
            this.Add(Keys.Control | Keys.D7, this.handlers.moveBookMarkEvent, 7);
            this.Add(Keys.Control | Keys.D8, this.handlers.moveBookMarkEvent, 8);
            this.Add(Keys.Control | Keys.D9, this.handlers.moveBookMarkEvent, 9);
        }

        private void InitClipboard()
        {
            this.Add(Keys.Control | Keys.C, this.handlers.copyEvent);
            this.Add(Keys.Control | Keys.Insert, this.handlers.copyEvent);
            this.Add(Keys.Control | Keys.X, this.handlers.cutEvent);
            this.Add(Keys.Shift | Keys.Delete, this.handlers.cutEvent);
            this.Add(Keys.Control | Keys.V, this.handlers.pasteEvent);
            this.Add(Keys.Shift | Keys.Insert, this.handlers.pasteEvent);
            this.Add(Keys.Control | Keys.L, this.handlers.lineCutEvent);
            this.Add(Keys.Control | Keys.Shift | Keys.L, this.handlers.lineDeleteEvent);
        }

        private void InitCodeCompletionKeys()
        {
            this.Add(Keys.Alt | Keys.Right, this.handlers.completeWordEvent);
            this.Add(Keys.Control | Keys.Space, this.handlers.completeWordEvent);
            this.Add(Keys.Control | Keys.Shift | Keys.Space, this.handlers.parameterInfoEvent);
            this.Add(Keys.Control | Keys.Shift | Keys.Space, this.handlers.parameterInfoEvent);
            this.Add(Keys.Control | Keys.L, this.handlers.listMembersEvent, 1, 0);
            this.Add(Keys.L, this.handlers.listMembersEvent, 1, 0);
            this.Add(Keys.Control | Keys.I, this.handlers.quickInfoEvent, 1, 0);
            this.Add(Keys.I, this.handlers.quickInfoEvent, 1, 0);
            this.Add(Keys.Control | Keys.X, this.handlers.codeSnippetsEvent, 1, 0);
            this.Add(Keys.X, this.handlers.codeSnippetsEvent, 1, 0);
        }

        private void InitMacros()
        {
            this.Add(Keys.Control | Keys.Shift | Keys.R, this.handlers.toggleMacroRecording);
            this.Add(Keys.Control | Keys.Shift | Keys.P, this.handlers.playBackMacro);
        }

        private void InitMovements()
        {
            this.Add(Keys.Up, this.handlers.moveLineUpEvent);
            this.Add(Keys.Down, this.handlers.moveLineDownEvent);
            this.Add(Keys.Left, this.handlers.moveCharLeftEvent);
            this.Add(Keys.Right, this.handlers.moveCharRightEvent);
            this.Add(Keys.PageUp, this.handlers.movePageUpEvent);
            this.Add(Keys.Next, this.handlers.movePageDownEvent);
            this.Add(Keys.Control | Keys.PageUp, this.handlers.moveScreenTopEvent);
            this.Add(Keys.Control | Keys.Next, this.handlers.moveScreenBottomEvent);
            this.Add(Keys.Control | Keys.Home, this.handlers.moveFileBeginEvent);
            this.Add(Keys.Control | Keys.End, this.handlers.moveFileEndEvent);
            this.Add(Keys.Home, this.handlers.moveLineBeginEvent);
            this.Add(Keys.End, this.handlers.moveLineEndEvent);
            this.Add(Keys.Control | Keys.Left, this.handlers.moveWordLeftEvent);
            this.Add(Keys.Control | Keys.Right, this.handlers.moveWordRightEvent);
            this.Add(Keys.Control | Keys.Up, this.handlers.scrollLineUpEvent);
            this.Add(Keys.Control | Keys.Down, this.handlers.scrollLineDownEvent);
            this.Add(Keys.Control | Keys.Oem6, this.handlers.moveToBraceEvent);
            this.Add(Keys.Control | Keys.Shift | Keys.Oem6, this.handlers.moveToBraceExtendEvent);
        }

        private void InitOutlining()
        {
            this.Add(Keys.Control | Keys.M, null, 0, 3);
            this.Add(Keys.Control | Keys.L, this.handlers.toggleOutliningEvent, 3, 0);
            this.Add(Keys.Control | Keys.M, this.handlers.toggleOutliningSelectionEvent, 3, 0);
            this.Add(Keys.Control | Keys.O, this.handlers.collapseToDefinitionsEvent, 3, 0);
        }

        private void InitSearch()
        {
            this.Add(Keys.Control | Keys.F, this.handlers.findEvent);
            this.Add(Keys.F3, this.handlers.findNextEvent);
            this.Add(Keys.Control | Keys.F3, this.handlers.findNextSelectedEvent);
            this.Add(Keys.Shift | Keys.F3, this.handlers.findPreviousEvent);
            this.Add(Keys.Control | Keys.Shift | Keys.F3, this.handlers.findPreviousSelectedEvent);
            this.Add(Keys.Control | Keys.H, this.handlers.replaceEvent);
            this.Add(Keys.Control | Keys.G, this.handlers.gotolineEvent);
            this.Add(Keys.Control | Keys.I, this.handlers.initIncrementalSearchEvent);
            this.Add(Keys.Control | Keys.Shift | Keys.I, this.handlers.initReverseIncrementalSearchEvent);
            this.Add(Keys.Alt | Keys.F3, null, 0, 2);
            this.Add(Keys.H, this.handlers.toggleHiddenTextEvent, 2, 0);
            this.Add(Keys.C, this.handlers.toggleMatchCaseEvent, 2, 0);
            this.Add(Keys.R, this.handlers.toggleRegularExpressionsEvent, 2, 0);
            this.Add(Keys.B, this.handlers.toggleSearchUpEvent, 2, 0);
            this.Add(Keys.W, this.handlers.toggleWholeWordEvent, 2, 0);
        }

        private void InitSelection()
        {
            this.Add(Keys.Control | Keys.E, null, 0, 6);
            this.Add(Keys.Shift | Keys.Up, this.handlers.selectLineUpEvent, SelectionType.Stream);
            this.Add(Keys.Shift | Keys.Down, this.handlers.selectLineDownEvent, SelectionType.Stream);
            this.Add(Keys.Shift | Keys.Left, this.handlers.selectCharLeftEvent, SelectionType.Stream);
            this.Add(Keys.Shift | Keys.Right, this.handlers.selectCharRightEvent, SelectionType.Stream);
            this.Add(Keys.Shift | Keys.PageUp, this.handlers.selectPageUpEvent, SelectionType.Stream);
            this.Add(Keys.Shift | Keys.Next, this.handlers.selectPageDownEvent, SelectionType.Stream);
            this.Add(Keys.Control | Keys.Shift | Keys.PageUp, this.handlers.selectScreenTopEvent, SelectionType.Stream);
            this.Add(Keys.Control | Keys.Shift | Keys.Next, this.handlers.selectScreenBottomEvent, SelectionType.Stream);
            this.Add(Keys.Control | Keys.Shift | Keys.Home, this.handlers.selectFileBeginEvent, SelectionType.Stream);
            this.Add(Keys.Control | Keys.Shift | Keys.End, this.handlers.selectFileEndEvent, SelectionType.Stream);
            this.Add(Keys.Shift | Keys.Home, this.handlers.selectLineBeginEvent, SelectionType.Stream);
            this.Add(Keys.Shift | Keys.End, this.handlers.selectLineEndEvent, SelectionType.Stream);
            this.Add(Keys.Control | Keys.Shift | Keys.Left, this.handlers.selectWordLeftEvent, SelectionType.Stream);
            this.Add(Keys.Control | Keys.Shift | Keys.Right, this.handlers.selectWordRightEvent, SelectionType.Stream);
            this.Add(Keys.Control | Keys.W, this.handlers.selectWordEvent);
            this.Add(Keys.Control | Keys.A, this.handlers.selectAllEvent);
            this.Add(Keys.Control | Keys.Q, this.handlers.tabifyEvent);
            this.Add(Keys.Control | Keys.Shift | Keys.Q, this.handlers.unTabifyEvent);
            this.Add(Keys.Control | Keys.U, this.handlers.lowerCaseEvent);
            this.Add(Keys.Control | Keys.Shift | Keys.U, this.handlers.upperCaseEvent);
            this.Add(Keys.Control | Keys.Shift | Keys.C, this.handlers.capitalizeEvent);
            this.Add(Keys.Control | Keys.Oem5, this.handlers.deleteWhiteSpaceEvent, 1, 0);
            this.Add(Keys.Control | Keys.C, this.handlers.commentSelectionEvent, 1, 0);
            this.Add(Keys.Control | Keys.U, this.handlers.uncommentSelectionEvent, 1, 0);
            this.Add(Keys.C, this.handlers.commentSelectionEvent, 6, 0);
            this.Add(Keys.C, this.handlers.commentSelectionEvent, 6, 0);
            this.Add(Keys.Control | Keys.C, this.handlers.commentSelectionEvent, 6, 0);
            this.Add(Keys.U, this.handlers.uncommentSelectionEvent, 6, 0);
            this.Add(Keys.Control | Keys.U, this.handlers.uncommentSelectionEvent, 6, 0);
            this.Add(Keys.Alt | Keys.Shift | Keys.Up, this.handlers.selectLineUpEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Shift | Keys.Down, this.handlers.selectLineDownEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Shift | Keys.Left, this.handlers.selectCharLeftEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Shift | Keys.Right, this.handlers.selectCharRightEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Shift | Keys.PageUp, this.handlers.selectPageUpEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Shift | Keys.Next, this.handlers.selectPageDownEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Control | Keys.Shift | Keys.Home, this.handlers.selectFileBeginEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Control | Keys.Shift | Keys.End, this.handlers.selectFileEndEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Shift | Keys.Home, this.handlers.selectLineBeginEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Shift | Keys.End, this.handlers.selectLineEndEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Control | Keys.Shift | Keys.Left, this.handlers.selectWordLeftEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Control | Keys.Shift | Keys.Right, this.handlers.selectWordRightEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Control | Keys.Shift | Keys.PageUp, this.handlers.selectScreenTopEvent, SelectionType.Block);
            this.Add(Keys.Alt | Keys.Control | Keys.Shift | Keys.Next, this.handlers.selectScreenBottomEvent, SelectionType.Block);
            this.Add(Keys.Control | Keys.R, null, 0, 4);
            this.Add(Keys.Control | Keys.P, this.handlers.swapAnchorEvent, 4, 0);
            this.Add(Keys.Control | Keys.F, this.handlers.formatSelectionEvent, 1, 0);
            this.Add(Keys.Control | Keys.D, this.handlers.formatDocumentEvent, 1, 0);
        }

        private void InitStdKeys()
        {
            this.Add(Keys.Delete, this.handlers.deleteRightEvent);
            this.Add(Keys.Back, this.handlers.deleteLeftEvent);
            this.Add(Keys.Shift | Keys.Back, this.handlers.deleteLeftEvent);
            this.Add(Keys.Control | Keys.Back, this.handlers.deleteWordLeftEvent);
            this.Add(Keys.Control | Keys.Delete, this.handlers.deleteWordRightEvent);
            this.Add(Keys.Enter, this.handlers.newLineEvent);
            this.Add(Keys.Control | Keys.Enter, this.handlers.newLineAboveEvent);
            this.Add(Keys.Control | Keys.Shift | Keys.Enter, this.handlers.newLineBelowEvent);
            this.Add(Keys.Shift | Keys.Enter, this.handlers.newLineEvent);
            this.Add(Keys.Escape, this.handlers.processEscapeEvent);
            this.Add(Keys.Tab, this.handlers.processTabEvent);
            this.Add(Keys.Shift | Keys.Tab, this.handlers.processShiftTabEvent);
            this.Add(Keys.Alt | Keys.Back, this.handlers.undoEvent);
            this.Add(Keys.Alt | Keys.Shift | Keys.Back, this.handlers.redoEvent);
            this.Add(Keys.Control | Keys.Z, this.handlers.undoEvent);
            this.Add(Keys.Control | Keys.Shift | Keys.Z, this.handlers.redoEvent);
            this.Add(Keys.Insert, this.handlers.toggleOverwriteEvent);
        }

        private void InitWordKeys()
        {
            this.Add(Keys.Control | Keys.T, this.handlers.charTransposeEvent);
            this.Add(Keys.Control | Keys.Shift | Keys.T, this.handlers.wordTransposeEvent);
            this.Add(Keys.Alt | Keys.Shift | Keys.T, this.handlers.lineTransposeEvent);
        }

        public virtual void Remove(Keys keys)
        {
            this.Remove(keys, -1);
        }

        public virtual void Remove(Keys keys, int state)
        {
            Hashtable hashtable = (Hashtable) this.states[state];
            if (hashtable != null)
            {
                hashtable.Remove(keys);
            }
        }

        public IKeyData[] EventData
        {
            get
            {
                int num = 0;
                foreach (Hashtable hashtable in this.states.Values)
                {
                    num += hashtable.Values.Count;
                }
                IKeyData[] dataArray = new IKeyData[num];
                int index = 0;
                foreach (Hashtable hashtable2 in this.states.Values)
                {
                    foreach (IKeyData data in hashtable2.Values)
                    {
                        dataArray[index] = new KeyData(data.Keys, data.Action, data.ActionEx, data.Param, data.State, data.LeaveState);
                        index++;
                    }
                }
                return dataArray;
            }
        }

        public IEventHandlers Handlers
        {
            get
            {
                return this.handlers;
            }
        }

        internal enum KeyStates
        {
            BookMark = 1,
            BookMarkEx = 5,
            Outline = 3,
            Search = 2,
            Select = 4,
            SelectEx = 6
        }
    }
}

