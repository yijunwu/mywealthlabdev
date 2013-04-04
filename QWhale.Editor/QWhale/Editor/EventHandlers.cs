namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Reflection;

    public class EventHandlers : IEventHandlers
    {
        public KeyEvent capitalizeEvent;
        public KeyEvent charTransposeEvent;
        public KeyEvent clearBookMarkEvent;
        public KeyEvent codeSnippetsEvent;
        public KeyEvent collapseToDefinitionsEvent;
        public KeyEvent commentSelectionEvent;
        public KeyEvent completeWordEvent;
        public KeyEvent copyEvent;
        public KeyEvent cutEvent;
        public KeyEvent deleteLeftEvent;
        public KeyEvent deleteRightEvent;
        public KeyEvent deleteWhiteSpaceEvent;
        public KeyEvent deleteWordLeftEvent;
        public KeyEvent deleteWordRightEvent;
        public KeyEvent findEvent;
        public KeyEvent findNextEvent;
        public KeyEvent findNextSelectedEvent;
        public KeyEvent findPreviousEvent;
        public KeyEvent findPreviousSelectedEvent;
        public KeyEvent formatDocumentEvent;
        public KeyEvent formatSelectionEvent;
        public KeyEvent gotolineEvent;
        public KeyEvent initIncrementalSearchEvent;
        public KeyEvent initReverseIncrementalSearchEvent;
        public KeyEvent lineCutEvent;
        public KeyEvent lineDeleteEvent;
        public KeyEvent lineTransposeEvent;
        public KeyEvent listMembersEvent;
        public KeyEvent lowerCaseEvent;
        public KeyEventEx moveBookMarkEvent;
        public KeyEvent moveCharLeftEvent;
        public KeyEvent moveCharRightEvent;
        public KeyEvent moveFileBeginEvent;
        public KeyEvent moveFileEndEvent;
        public KeyEvent moveLineBeginEvent;
        public KeyEvent moveLineDownEvent;
        public KeyEvent moveLineEndEvent;
        public KeyEvent moveLineUpEvent;
        public KeyEvent movePageDownEvent;
        public KeyEvent movePageUpEvent;
        public KeyEvent moveScreenBottomEvent;
        public KeyEvent moveScreenTopEvent;
        public KeyEvent moveToBraceEvent;
        public KeyEvent moveToBraceExtendEvent;
        public KeyEvent moveToCloseBraceEvent;
        public KeyEvent moveToCloseBraceExtendEvent;
        public KeyEvent moveToOpenBraceEvent;
        public KeyEvent moveToOpenBraceExtendEvent;
        public KeyEvent moveWordLeftEvent;
        public KeyEvent moveWordRightEvent;
        public KeyEvent newLineAboveEvent;
        public KeyEvent newLineBelowEvent;
        public KeyEvent newLineEvent;
        public KeyEvent nextBookMarkEvent;
        private ISyntaxEdit owner;
        public KeyEvent parameterInfoEvent;
        public KeyEvent pasteEvent;
        public KeyEvent playBackMacro;
        public KeyEvent prevBookMarkEvent;
        public KeyEvent processEscapeEvent;
        public KeyEventEx processKey;
        public KeyEvent processShiftTabEvent;
        public KeyEvent processTabEvent;
        public KeyEvent quickInfoEvent;
        public KeyEvent redoEvent;
        public KeyEvent replaceEvent;
        public KeyEvent scrollLineDownEvent;
        public KeyEvent scrollLineUpEvent;
        public KeyEvent selectAllEvent;
        public KeyEventEx selectCharLeftEvent;
        public KeyEventEx selectCharRightEvent;
        public KeyEventEx selectFileBeginEvent;
        public KeyEventEx selectFileEndEvent;
        public KeyEventEx selectLineBeginEvent;
        public KeyEventEx selectLineDownEvent;
        public KeyEventEx selectLineEndEvent;
        public KeyEventEx selectLineUpEvent;
        public KeyEventEx selectPageDownEvent;
        public KeyEventEx selectPageUpEvent;
        public KeyEventEx selectScreenBottomEvent;
        public KeyEventEx selectScreenTopEvent;
        public KeyEvent selectWordEvent;
        public KeyEventEx selectWordLeftEvent;
        public KeyEventEx selectWordRightEvent;
        public KeyEvent swapAnchorEvent;
        public KeyEvent tabifyEvent;
        public KeyEvent toggleBookMarkEvent;
        public KeyEventEx toggleBookMarkEventEx;
        public KeyEvent toggleHiddenTextEvent;
        public KeyEvent toggleMacroRecording;
        public KeyEvent toggleMatchCaseEvent;
        public KeyEvent toggleOutliningEvent;
        public KeyEvent toggleOutliningSelectionEvent;
        public KeyEvent toggleOverwriteEvent;
        public KeyEvent toggleRegularExpressionsEvent;
        public KeyEvent toggleSearchUpEvent;
        public KeyEvent toggleWholeWordEvent;
        public KeyEvent uncommentSelectionEvent;
        public KeyEvent undoEvent;
        public KeyEvent unTabifyEvent;
        public KeyEvent upperCaseEvent;
        public KeyEvent wordTransposeEvent;

        public EventHandlers(ISyntaxEdit owner)
        {
            this.owner = owner;
            ISelection selection = owner.Selection;
            this.tabifyEvent = new KeyEvent(selection.Tabify);
            this.unTabifyEvent = new KeyEvent(selection.UnTabify);
            this.lowerCaseEvent = new KeyEvent(selection.LowerCase);
            this.upperCaseEvent = new KeyEvent(selection.UpperCase);
            this.capitalizeEvent = new KeyEvent(selection.Capitalize);
            this.deleteWhiteSpaceEvent = new KeyEvent(selection.DeleteWhiteSpace);
            this.commentSelectionEvent = new KeyEvent(selection.CommentSelection);
            this.uncommentSelectionEvent = new KeyEvent(selection.UncommentSelection);
            this.moveCharLeftEvent = new KeyEvent(owner.MoveCharLeft);
            this.moveCharRightEvent = new KeyEvent(owner.MoveCharRight);
            this.moveLineUpEvent = new KeyEvent(owner.MoveLineUp);
            this.moveLineDownEvent = new KeyEvent(owner.MoveLineDown);
            this.moveWordLeftEvent = new KeyEvent(owner.MoveWordLeft);
            this.moveWordRightEvent = new KeyEvent(owner.MoveWordRight);
            this.movePageUpEvent = new KeyEvent(owner.MovePageUp);
            this.movePageDownEvent = new KeyEvent(owner.MovePageDown);
            this.moveScreenTopEvent = new KeyEvent(owner.MoveScreenTop);
            this.moveScreenBottomEvent = new KeyEvent(owner.MoveScreenBottom);
            this.moveLineBeginEvent = new KeyEvent(owner.MoveLineBeginCycled);
            this.moveLineEndEvent = new KeyEvent(owner.MoveLineEnd);
            this.moveFileBeginEvent = new KeyEvent(owner.MoveFileBegin);
            this.moveFileEndEvent = new KeyEvent(owner.MoveFileEnd);
            this.selectCharLeftEvent = new KeyEventEx(this.SelectCharLeft);
            this.selectCharRightEvent = new KeyEventEx(this.SelectCharRight);
            this.selectLineUpEvent = new KeyEventEx(this.SelectLineUp);
            this.selectLineDownEvent = new KeyEventEx(this.SelectLineDown);
            this.selectWordLeftEvent = new KeyEventEx(this.SelectWordLeft);
            this.selectWordRightEvent = new KeyEventEx(this.SelectWordRight);
            this.selectPageUpEvent = new KeyEventEx(this.SelectPageUp);
            this.selectPageDownEvent = new KeyEventEx(this.SelectPageDown);
            this.selectScreenTopEvent = new KeyEventEx(this.SelectScreenTop);
            this.selectScreenBottomEvent = new KeyEventEx(this.SelectScreenBottom);
            this.selectLineBeginEvent = new KeyEventEx(this.SelectLineBegin);
            this.selectLineEndEvent = new KeyEventEx(this.SelectLineEnd);
            this.selectFileBeginEvent = new KeyEventEx(this.SelectFileBegin);
            this.selectFileEndEvent = new KeyEventEx(this.SelectFileEnd);
            this.selectAllEvent = new KeyEvent(this.SelectAll);
            this.toggleBookMarkEvent = new KeyEvent(this.ToggleBookMark);
            this.nextBookMarkEvent = new KeyEvent(this.GotoNextBookMark);
            this.prevBookMarkEvent = new KeyEvent(this.GotoPrevBookMark);
            this.clearBookMarkEvent = new KeyEvent(this.ClearAllBookMarks);
            this.toggleBookMarkEventEx = new KeyEventEx(this.ToggleBookMark);
            this.moveBookMarkEvent = new KeyEventEx(this.GotoBookMark);
            this.deleteLeftEvent = new KeyEvent(this.DeleteLeft);
            this.deleteWordLeftEvent = new KeyEvent(selection.DeleteWordLeft);
            this.deleteWordRightEvent = new KeyEvent(selection.DeleteWordRight);
            this.deleteRightEvent = new KeyEvent(selection.DeleteRight);
            this.newLineEvent = new KeyEvent(selection.NewLine);
            this.processEscapeEvent = new KeyEvent(this.ProcessEscape);
            this.processTabEvent = new KeyEvent(selection.ProcessTab);
            this.processShiftTabEvent = new KeyEvent(selection.ProcessShiftTab);
            this.toggleOverwriteEvent = new KeyEvent(selection.ToggleOverWrite);
            IOutlining outlining = owner.Outlining;
            this.toggleOutliningEvent = new KeyEvent(outlining.ToggleOutlining);
            this.toggleOutliningSelectionEvent = new KeyEvent(selection.ToggleOutlining);
            IOutlining outlining2 = owner.Outlining;
            this.collapseToDefinitionsEvent = new KeyEvent(outlining2.CollapseToDefinitions);
            this.swapAnchorEvent = new KeyEvent(selection.SwapAnchor);
            this.undoEvent = new KeyEvent(this.Undo);
            this.redoEvent = new KeyEvent(this.Redo);
            this.findEvent = new KeyEvent(this.Find);
            this.findNextEvent = new KeyEvent(this.FindNext);
            this.findPreviousEvent = new KeyEvent(this.FindPrevious);
            this.findNextSelectedEvent = new KeyEvent(this.FindNextSelected);
            this.findPreviousSelectedEvent = new KeyEvent(this.FindPreviousSelected);
            this.replaceEvent = new KeyEvent(this.Replace);
            this.gotolineEvent = new KeyEvent(this.GotoLine);
            this.initIncrementalSearchEvent = new KeyEvent(owner.StartIncrementalSearch);
            this.initReverseIncrementalSearchEvent = new KeyEvent(this.StartReverseIncrementalSearch);
            this.cutEvent = new KeyEvent(selection.Cut);
            this.copyEvent = new KeyEvent(selection.Copy);
            this.pasteEvent = new KeyEvent(selection.Paste);
            this.completeWordEvent = new KeyEvent(owner.CompleteWord);
            this.listMembersEvent = new KeyEvent(owner.ListMembers);
            this.quickInfoEvent = new KeyEvent(owner.QuickInfo);
            this.parameterInfoEvent = new KeyEvent(owner.ParameterInfo);
            this.codeSnippetsEvent = new KeyEvent(owner.CodeSnippets);
            this.formatSelectionEvent = new KeyEvent(selection.SmartFormat);
            this.formatDocumentEvent = new KeyEvent(selection.SmartFormatDocument);
            this.charTransposeEvent = new KeyEvent(selection.CharTransponse);
            this.wordTransposeEvent = new KeyEvent(selection.WordTransponse);
            this.lineTransposeEvent = new KeyEvent(selection.LineTransponse);
            this.toggleHiddenTextEvent = new KeyEvent(this.ToggleHiddenText);
            this.toggleMatchCaseEvent = new KeyEvent(this.ToggleMatchCase);
            this.toggleRegularExpressionsEvent = new KeyEvent(this.ToggleRegularExpressions);
            this.toggleSearchUpEvent = new KeyEvent(this.ToggleSearchUp);
            this.toggleWholeWordEvent = new KeyEvent(this.ToggleWholeWord);
            this.scrollLineUpEvent = new KeyEvent(owner.ScrollLineUp);
            this.scrollLineDownEvent = new KeyEvent(owner.ScrollLineDown);
            this.moveToOpenBraceEvent = new KeyEvent(owner.MoveToOpenBrace);
            this.moveToOpenBraceExtendEvent = new KeyEvent(selection.SelectToOpenBrace);
            this.moveToCloseBraceEvent = new KeyEvent(owner.MoveToCloseBrace);
            this.moveToCloseBraceExtendEvent = new KeyEvent(selection.SelectToCloseBrace);
            this.moveToBraceEvent = new KeyEvent(owner.MoveToBrace);
            this.moveToBraceExtendEvent = new KeyEvent(selection.SelectToBrace);
            this.newLineAboveEvent = new KeyEvent(selection.NewLineAbove);
            this.newLineBelowEvent = new KeyEvent(selection.NewLineBelow);
            this.selectWordEvent = new KeyEvent(selection.SelectWord);
            this.lineCutEvent = new KeyEvent(selection.CutLine);
            this.lineDeleteEvent = new KeyEvent(selection.DeleteLine);
            this.toggleMacroRecording = new KeyEvent(owner.ToggleMacroRecording);
            this.playBackMacro = new KeyEvent(owner.PlayBackMacro);
            this.processKey = new KeyEventEx(this.PressKey);
        }

        public void ClearAllBookMarks()
        {
            this.owner.Source.BookMarks.ClearAllBookMarks();
        }

        public void DeleteLeft()
        {
            if (this.owner.InIncrementalSearch && (this.owner.IncrementalSearchString != string.Empty))
            {
                this.owner.IncrementalSearch(string.Empty, true);
            }
            else
            {
                this.owner.Selection.DeleteLeft();
            }
        }

        public void Find()
        {
            this.owner.DisplaySearchDialog();
        }

        public void FindNext()
        {
            if (!this.owner.FindNext())
            {
                this.owner.ShowNotFound(StringConsts.DlgSearchCaption);
            }
        }

        public void FindNextSelected()
        {
            if (this.owner.CanFindNextSelected() && !this.owner.FindNextSelected())
            {
                this.owner.ShowNotFound(StringConsts.DlgSearchCaption);
            }
        }

        public void FindPrevious()
        {
            if (!this.owner.FindPrevious())
            {
                this.owner.ShowNotFound(StringConsts.DlgSearchCaption);
            }
        }

        public void FindPreviousSelected()
        {
            if (this.owner.CanFindPreviousSelected() && !this.owner.FindPreviousSelected())
            {
                this.owner.ShowNotFound(StringConsts.DlgSearchCaption);
            }
        }

        public void GotoBookMark(object param)
        {
            this.owner.Source.BookMarks.GotoBookMark((int) param);
        }

        public void GotoLine()
        {
            this.owner.DisplayGotoLineDialog();
        }

        public void GotoNextBookMark()
        {
            this.owner.Source.BookMarks.GotoNextBookMark();
        }

        public void GotoPrevBookMark()
        {
            this.owner.Source.BookMarks.GotoPrevBookMark();
        }

        public void PressKey(object param)
        {
            this.owner.ProcessKeyPress((char) param);
        }

        public void ProcessEscape()
        {
            if (this.owner.InIncrementalSearch)
            {
                this.owner.FinishIncrementalSearch();
            }
            else
            {
                this.owner.Selection.ProcessEscape();
            }
        }

        public void Redo()
        {
            this.owner.Source.Redo();
        }

        public void Replace()
        {
            this.owner.DisplayReplaceDialog();
        }

        public void SelectAll()
        {
            this.owner.MoveFileEnd();
            this.owner.Selection.SelectAll();
        }

        public void SelectCharLeft(object selectionType)
        {
            this.owner.Selection.SelectCharLeft((SelectionType) selectionType);
        }

        public void SelectCharRight(object selectionType)
        {
            this.owner.Selection.SelectCharRight((SelectionType) selectionType);
        }

        public void SelectFileBegin(object selectionType)
        {
            this.owner.Selection.SelectFileBegin((SelectionType) selectionType);
        }

        public void SelectFileEnd(object selectionType)
        {
            this.owner.Selection.SelectFileEnd((SelectionType) selectionType);
        }

        public void SelectLineBegin(object selectionType)
        {
            this.owner.Selection.SelectLineBegin((SelectionType) selectionType);
        }

        public void SelectLineDown(object selectionType)
        {
            this.owner.Selection.SelectLineDown((SelectionType) selectionType);
        }

        public void SelectLineEnd(object selectionType)
        {
            this.owner.Selection.SelectLineEnd((SelectionType) selectionType);
        }

        public void SelectLineUp(object selectionType)
        {
            this.owner.Selection.SelectLineUp((SelectionType) selectionType);
        }

        public void SelectPageDown(object selectionType)
        {
            this.owner.Selection.SelectPageDown((SelectionType) selectionType);
        }

        public void SelectPageUp(object selectionType)
        {
            this.owner.Selection.SelectPageUp((SelectionType) selectionType);
        }

        public void SelectScreenBottom(object selectionType)
        {
            this.owner.Selection.SelectScreenBottom((SelectionType) selectionType);
        }

        public void SelectScreenTop(object selectionType)
        {
            this.owner.Selection.SelectScreenTop((SelectionType) selectionType);
        }

        public void SelectWordLeft(object selectionType)
        {
            this.owner.Selection.SelectWordLeft((SelectionType) selectionType);
        }

        public void SelectWordRight(object selectionType)
        {
            this.owner.Selection.SelectWordRight((SelectionType) selectionType);
        }

        public void StartReverseIncrementalSearch()
        {
            this.owner.StartIncrementalSearch(true);
        }

        public void ToggleBookMark()
        {
            this.owner.Source.BookMarks.ToggleBookMark();
        }

        public void ToggleBookMark(object param)
        {
            this.owner.Source.BookMarks.ToggleBookMark((int) param);
        }

        public void ToggleHiddenText()
        {
            if (this.owner.SearchDialog != null)
            {
                this.owner.SearchDialog.ToggleHiddenText();
            }
        }

        public void ToggleMatchCase()
        {
            if (this.owner.SearchDialog != null)
            {
                this.owner.SearchDialog.ToggleMatchCase();
            }
        }

        public void ToggleRegularExpressions()
        {
            if (this.owner.SearchDialog != null)
            {
                this.owner.SearchDialog.ToggleRegularExpressions();
            }
        }

        public void ToggleSearchUp()
        {
            if (this.owner.SearchDialog != null)
            {
                this.owner.SearchDialog.ToggleSearchUp();
            }
        }

        public void ToggleWholeWord()
        {
            if (this.owner.SearchDialog != null)
            {
                this.owner.SearchDialog.ToggleWholeWord();
            }
        }

        public void Undo()
        {
            this.owner.Source.Undo();
        }

        public string[] EventNames
        {
            get
            {
                FieldInfo[] fields = base.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                string[] strArray = new string[fields.Length];
                for (int i = 0; i < fields.Length; i++)
                {
                    string name = fields[i].Name;
                    if (name != string.Empty)
                    {
                        char ch2 = name[0];
                        char ch = ch2.ToString().ToUpper()[0];
                        name = name.Remove(0, 1).Insert(0, ch.ToString());
                        int startIndex = name.LastIndexOf("Event", name.Length);
                        if (startIndex >= 0)
                        {
                            name = name.Remove(startIndex, name.Length - startIndex);
                        }
                    }
                    strArray[i] = name;
                }
                return strArray;
            }
        }

        public virtual KeyEventEx MacroRecordEvent
        {
            get
            {
                return this.processKey;
            }
        }
    }
}

