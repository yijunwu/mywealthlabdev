namespace QWhale.Editor.Dialogs
{
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class SearchDialog : ISearchDialog
    {
        private static Form ownerForm;
        private static bool regularExpressionsEnabled = true;
        private static bool regularExpressionsVisible = true;
        private static DlgSearch searchDlg = null;
        private static ISearchSettings searchSettings = new QWhale.Editor.Dialogs.SearchSettings();

        private event HelpEventHandler helpRequested;

        public event HelpEventHandler HelpRequested;

        private void CheckOption(SearchOptions value)
        {
            if ((searchSettings.SearchOptions & value) == SearchOptions.None)
            {
                searchSettings.SearchOptions |= value;
            }
            else
            {
                searchSettings.SearchOptions &= ~value;
            }
        }

        public virtual void Close()
        {
            if (searchDlg != null)
            {
                searchDlg.Close();
            }
        }

        private void CopyList(IList fromList, IList<string> toList)
        {
            toList.Clear();
            foreach (object obj2 in fromList)
            {
                toList.Add(obj2.ToString());
            }
        }

        private void CopyList(IList<string> fromList, IList toList)
        {
            toList.Clear();
            foreach (string str in fromList)
            {
                toList.Add(str);
            }
        }

        private void DoClose(object Sender, EventArgs e)
        {
            if (searchDlg != null)
            {
                searchSettings.SearchOptions = searchDlg.Options;
                searchSettings.ClearBookmarks = searchDlg.ClearBookmarks;
                this.CopyList(searchDlg.cbFindWhat.Items, searchSettings.SearchList);
                this.CopyList(searchDlg.cbReplaceWith.Items, searchSettings.ReplaceList);
            }
            searchDlg = null;
        }

        private void DoHelpEvent(object sender, HelpEventArgs e)
        {
            if (this.helpRequested != null)
            {
                this.helpRequested(this, e);
            }
        }

        public virtual void DoneSearch(ISearch search)
        {
            if ((searchDlg != null) && (searchDlg.Search == search))
            {
                searchDlg.Search = null;
            }
        }

        public void EnableRegularExpressions(bool enable)
        {
            regularExpressionsEnabled = enable;
            if (searchDlg != null)
            {
                searchDlg.chbUseRegularExpressions.Enabled = enable;
            }
        }

        public virtual void EnsureVisible(Rectangle rect)
        {
            if (searchDlg != null)
            {
                Rectangle rectangle = rect;
                rectangle.Intersect(searchDlg.Bounds);
                if (!rectangle.IsEmpty)
                {
                    if ((rect.Bottom + searchDlg.Height) < SystemInformation.WorkingArea.Height)
                    {
                        searchDlg.Top = rect.Bottom;
                    }
                    else
                    {
                        searchDlg.Top = Math.Max(rect.Top - searchDlg.Height, 0);
                    }
                    if ((rect.Right + searchDlg.Width) < SystemInformation.WorkingArea.Width)
                    {
                        searchDlg.Left = rect.Right;
                    }
                    else
                    {
                        searchDlg.Left = Math.Max(rect.Left - searchDlg.Width, 0);
                    }
                }
            }
        }

        public virtual DialogResult Execute(ISearch search, bool isModal, bool isReplace)
        {
            return this.Execute(search, isModal, isReplace, null);
        }

        public virtual DialogResult Execute(ISearch search, bool isModal, bool isReplace, IWin32Window owner)
        {
            string selectedText = string.Empty;
            bool flag = search.CanSearchSelection(out selectedText) && (selectedText.Trim() != string.Empty);
            bool flag2 = flag && (StringItem.Split(selectedText).Length <= 1);
            if (SearchDialog.searchDlg == null)
            {
                SearchDialog.searchDlg = new DlgSearch();
                SearchDialog.searchDlg.HelpRequested += new HelpEventHandler(this.DoHelpEvent);
                SearchOptions searchOptions = searchSettings.SearchOptions;
                SearchDialog.searchDlg.ClearBookmarks = searchSettings.ClearBookmarks;
                if (flag && (!flag2 || ((searchSettings.SearchOptions & SearchOptions.FindSelectedText) == SearchOptions.None)))
                {
                    searchOptions |= SearchOptions.SelectionOnly;
                }
                else
                {
                    searchOptions &= ~SearchOptions.SelectionOnly;
                }
                SearchDialog.searchDlg.Options = searchOptions;
                this.ShowRegularExpressions(regularExpressionsVisible);
                this.EnableRegularExpressions(regularExpressionsEnabled);
                this.CopyList(searchSettings.SearchList, SearchDialog.searchDlg.cbFindWhat.Items);
                this.CopyList(searchSettings.ReplaceList, SearchDialog.searchDlg.cbReplaceWith.Items);
                SearchDialog.searchDlg.Closed += new EventHandler(this.DoClose);
            }
            if (flag)
            {
                selectedText = selectedText.Trim();
                if (flag2 && ((searchSettings.SearchOptions & SearchOptions.FindSelectedText) != SearchOptions.None))
                {
                    if (selectedText != string.Empty)
                    {
                        SearchDialog.searchDlg.cbFindWhat.Text = selectedText;
                    }
                }
                else
                {
                    selectedText = string.Empty;
                }
            }
            if (((searchSettings.SearchOptions & SearchOptions.FindTextAtCursor) != SearchOptions.None) && (selectedText == string.Empty))
            {
                selectedText = search.GetTextToSearchAtCursor().Trim();
                if (selectedText != string.Empty)
                {
                    SearchDialog.searchDlg.cbFindWhat.Text = selectedText;
                }
            }
            SearchDialog.searchDlg.Search = search;
            SearchDialog.searchDlg.IsReplace = isReplace;
            SearchDialog.searchDlg.SelectionEnabled = flag;
            SearchDialog.searchDlg.Init();
            if (isModal)
            {
                Form searchDlg = SearchDialog.searchDlg;
                DialogResult result = (owner != null) ? SearchDialog.searchDlg.ShowDialog(owner) : SearchDialog.searchDlg.ShowDialog();
                searchDlg.Dispose();
                return result;
            }
            if (ownerForm == null)
            {
                SearchDialog.searchDlg.TopMost = true;
                SearchDialog.searchDlg.Owner = null;
            }
            else
            {
                SearchDialog.searchDlg.TopMost = false;
                SearchDialog.searchDlg.Owner = ownerForm;
            }
            SearchDialog.searchDlg.Show();
            return DialogResult.None;
        }

        protected virtual void OnOnwerFormChanged()
        {
            if (searchDlg != null)
            {
                searchDlg.Owner = ownerForm;
            }
        }

        protected virtual void OnVisibleChanged()
        {
        }

        public void ShowRegularExpressions(bool show)
        {
            regularExpressionsVisible = show;
            if (searchDlg != null)
            {
                searchDlg.chbUseRegularExpressions.Visible = show;
            }
        }

        public virtual void ToggleHiddenText()
        {
            this.CheckOption(SearchOptions.SearchHiddenText);
            if (searchDlg != null)
            {
                searchDlg.chbSearchHiddenText.Checked = !searchDlg.chbSearchHiddenText.Checked;
            }
        }

        public virtual void ToggleMatchCase()
        {
            this.CheckOption(SearchOptions.CaseSensitive);
            if (searchDlg != null)
            {
                searchDlg.chbMatchCase.Checked = !searchDlg.chbMatchCase.Checked;
            }
        }

        public void TogglePromptOnReplace()
        {
            this.CheckOption(SearchOptions.PromptOnReplace);
            if (searchDlg != null)
            {
                searchDlg.chbPromptOnReplace.Checked = !searchDlg.chbPromptOnReplace.Checked;
            }
        }

        public virtual void ToggleRegularExpressions()
        {
            this.CheckOption(SearchOptions.RegularExpressions);
            if (searchDlg != null)
            {
                searchDlg.chbUseRegularExpressions.Checked = !searchDlg.chbUseRegularExpressions.Checked;
            }
        }

        public virtual void ToggleSearchUp()
        {
            this.CheckOption(SearchOptions.BackwardSearch);
            if (searchDlg != null)
            {
                searchDlg.chbSearchUp.Checked = !searchDlg.chbSearchUp.Checked;
            }
        }

        public virtual void ToggleWholeWord()
        {
            this.CheckOption(SearchOptions.WholeWordsOnly);
            if (searchDlg != null)
            {
                searchDlg.chbMatchWholeWord.Checked = !searchDlg.chbMatchWholeWord.Checked;
            }
        }

        public Form OwnerForm
        {
            get
            {
                if (searchDlg == null)
                {
                    return ownerForm;
                }
                return searchDlg.Owner;
            }
            set
            {
                if (ownerForm != value)
                {
                    ownerForm = value;
                    this.OnOnwerFormChanged();
                }
            }
        }

        public virtual ISearchSettings SearchSettings
        {
            get
            {
                return searchSettings;
            }
        }

        public bool Visible
        {
            get
            {
                return ((searchDlg != null) && searchDlg.Visible);
            }
            set
            {
                if ((searchDlg != null) && (searchDlg.Visible != value))
                {
                    searchDlg.Visible = value;
                    this.OnVisibleChanged();
                }
            }
        }
    }
}

