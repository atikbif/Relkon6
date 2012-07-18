using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using QWhale.Editor;
using Kontel.Relkon.Components.Documents;
using Kontel.Relkon;
using System.Text.RegularExpressions;
using Kontel.Relkon.Solutions;

namespace Kontel.Relkon
{
    public sealed partial class FindReplaceForm : Form
    {
        private enum SearchingMode
        {
            Find = 1,
            Replace = 2
        }

        private SearchingMode searchingMode = SearchingMode.Replace; // определ€ет заданный режим поиска: только поиск или замена
        private EditorTabbedDocument searchedDocument = null; // документ, в котором осуществл€етс€ поиск
        private bool isInProgress = false; // показывает, продолжаетс€ ли процесс поиска или следует начать новый поиск

        public FindReplaceForm(EditorTabbedDocument SearchingDocument)
        {
            InitializeComponent();
            this.SwitchToFindMode();
            this.searchedDocument = SearchingDocument;
            this.Owner = this.searchedDocument.FindForm();
            this.SetSearchingList();
            this.SetReplaceList();
            if (this.searchedDocument is EditorTabbedDocument)
            {
                this.ddlSearchingList.SelectedIndex = -1;
                this.ddlSearchingList.Text = Regex.Match(((EditorTabbedDocument)this.searchedDocument).Editor.Selection.SelectedText, "[^\r\n\t]*").Value;
                if (this.ddlSearchingList.Text == "" && Clipboard.ContainsText())
                    this.ddlSearchingList.Text = Regex.Match(Clipboard.GetText(), "[^\r\n\t]*").Value;
            }
        }

        public FindReplaceForm(bool ReplaceMode, EditorTabbedDocument SearchingDocument)
        {
            InitializeComponent();
            if (ReplaceMode)
                this.SwitchToReplaceMode();
            else
                this.SwitchToFindMode();
            this.searchedDocument = SearchingDocument;
            this.Owner = this.searchedDocument.FindForm();
            this.SetSearchingList();
            this.SetReplaceList();
            if (this.searchedDocument is EditorTabbedDocument)
            {
                this.ddlSearchingList.SelectedIndex = -1;
                this.ddlSearchingList.Text = Regex.Match(((EditorTabbedDocument)this.searchedDocument).Editor.Selection.SelectedText, "[^\r\n\t]*").Value;
                if (this.ddlSearchingList.Text == "" && Clipboard.ContainsText())
                    this.ddlSearchingList.Text = Regex.Match(Clipboard.GetText(), "[^\r\n\t]*").Value;
            }
        }

        private void SetSearchingList()
        {
            if (this.searchedDocument.Solution is ControllerProgramSolution)
            {
                List<string> searchList = ((ControllerProgramSolution)this.searchedDocument.Solution).SearchList;
                if (Program.Settings.SearchString != "")
                {
                    if (searchList.Contains(Program.Settings.SearchString))
                        searchList.Remove(Program.Settings.SearchString);
                    searchList.Insert(0, Program.Settings.SearchString);
                }
                else if (searchList.Count > 0)
                    Program.Settings.SearchString = searchList[0];
                while (searchList.Count > 10)
                    searchList.RemoveAt(searchList.Count - 1);

                this.ddlSearchingList.Items.Clear();
                this.ddlSearchingList.Items.AddRange(searchList.ToArray());
            }
            this.ddlSearchingList.Text = Program.Settings.SearchString;
        }

        private void SetReplaceList()
        {
            if (this.searchedDocument.Solution is ControllerProgramSolution)
            {
                List<string> replaceList = ((ControllerProgramSolution)this.searchedDocument.Solution).ReplaceList;
                if (Program.Settings.ReplaceString != "")
                {
                    if (replaceList.Contains(Program.Settings.ReplaceString))
                        replaceList.Remove(Program.Settings.ReplaceString);
                    replaceList.Insert(0, Program.Settings.ReplaceString);
                }
                else if (replaceList.Count > 0)
                    Program.Settings.ReplaceString = replaceList[0];
                while (replaceList.Count > 10)
                    replaceList.RemoveAt(replaceList.Count - 1);
                this.ddlReplacingList.Items.Clear();
                this.ddlReplacingList.Items.AddRange(replaceList.ToArray());
            }
            this.ddlReplacingList.Text = Program.Settings.ReplaceString;
        }

        /// <summary>
        /// ¬озвращает или устанавливает документ, в котором производитс€ поиск
        /// </summary>
        public EditorTabbedDocument SearchedDocument
        {
            get
            {
                return this.searchedDocument;
            }
            set
            {
                this.searchedDocument = value;
            }
        }
        /// <summary>
        /// »щет указанный текст в документе
        /// </summary>
        public void FindNext()
        {
            if (!this.isInProgress)
            {
                if (this.searchedDocument.Find(this.ddlSearchingList.Text, this.cbMatchCase.Checked, this.cbWholeWord.Checked, this.cbSearchUp.Checked))
                    this.isInProgress = true;
                else
                    Utils.InformationMessage("”казанный текст не найден", "Relkon");
            }
            else if (!this.searchedDocument.FindNext())
            {
                Utils.InformationMessage("ѕоиск в документе завершен", "Relkon");
                this.isInProgress = false;
            }
            //((EditorTabbedDocument)this.searchedDocument).Editor.Focus();
        }
        /// <summary>
        /// ѕроизводит замены в документе
        /// </summary>
        private void Replace()
        {
            if (!this.isInProgress)
            {
                if (this.searchedDocument.Find(this.ddlSearchingList.Text, this.cbMatchCase.Checked, this.cbWholeWord.Checked, this.cbSearchUp.Checked))
                {
                    this.isInProgress = true;
                }
                else
                    Utils.InformationMessage("”казанный текст не найден", "Relkon");
            }
            else
            {
                this.searchedDocument.Editor.Selection.SelectionStart -= this.ddlReplacingList.Text.Length;
                this.searchedDocument.Replace(this.ddlSearchingList.Text, this.ddlReplacingList.Text, this.cbMatchCase.Checked, this.cbWholeWord.Checked, this.cbSearchUp.Checked);
                if (!this.searchedDocument.Find(this.ddlSearchingList.Text, this.cbMatchCase.Checked, this.cbWholeWord.Checked, this.cbSearchUp.Checked))
                {
                    Utils.InformationMessage("ѕоиск в документе завершен", "Relkon");
                    this.isInProgress = false;
                }
            }
        }
        /// <summary>
        /// ѕереключает форму в режим простого поиска
        /// </summary>
        public void SwitchToFindMode()
        {
            if (this.searchingMode == SearchingMode.Find)
                return;
            this.pReplaceWithContainer.Hide();
            this.pReplaceContainer.Hide();
            this.pReplaceAllContainer.Hide();
            this.Height -= this.pReplaceAllContainer.Height + this.pReplaceWithContainer.Height + 2;
            this.tsbFind.Checked = true;
            this.tsbReplace.Checked = false;
            this.searchingMode = SearchingMode.Find;
        }
        /// <summary>
        /// ѕереключает форму в режим замены
        /// </summary>
        public void SwitchToReplaceMode()
        {
            if (this.searchingMode == SearchingMode.Replace)
                return;
            this.pReplaceWithContainer.Show();
            this.pReplaceContainer.Show();
            this.pReplaceAllContainer.Show();
            this.Height += this.pReplaceAllContainer.Height + pReplaceWithContainer.Height + 2;
            this.tsbReplace.Checked = true;
            this.tsbFind.Checked = false;
            this.searchingMode = SearchingMode.Replace;
        }

        private void tsbFind_Click(object sender, EventArgs e)
        {
            this.SwitchToFindMode();
        }

        private void tsbReplace_Click(object sender, EventArgs e)
        {
            this.SwitchToReplaceMode();
        }

        private void bFindNext_Click(object sender, EventArgs e)
        {
            if (Program.Settings.SearchString != this.ddlSearchingList.Text)
            {
                Program.Settings.SearchString = this.ddlSearchingList.Text;
                this.SetSearchingList();
            }
            this.FindNext();
        }

        private void bReplace_Click(object sender, EventArgs e)
        {
            bool flag = true;
            if (Program.Settings.SearchString != this.ddlSearchingList.Text)
                Program.Settings.SearchString = this.ddlSearchingList.Text;
            else
                flag = false;
            if (Program.Settings.ReplaceString != this.ddlReplacingList.Text)
                Program.Settings.ReplaceString = this.ddlReplacingList.Text;
            else
                flag = false;
            if (flag)
            {
                this.SetReplaceList();
                this.SetReplaceList();
            }
            this.Replace();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = true;
            if (Program.Settings.SearchString != this.ddlSearchingList.Text)
                Program.Settings.SearchString = this.ddlSearchingList.Text;
            else
                flag = false;
            if (Program.Settings.ReplaceString != this.ddlReplacingList.Text)
                Program.Settings.ReplaceString = this.ddlReplacingList.Text;
            else
                flag = false;
            if (flag)
            {
                this.SetReplaceList();
                this.SetReplaceList();
            }
            Utils.InformationMessage("ѕоиск в документе завершен. ѕроизведено замен: " + this.searchedDocument.ReplaceAll(this.ddlSearchingList.Text, this.ddlReplacingList.Text, this.cbMatchCase.Checked, this.cbWholeWord.Checked, this.cbSearchUp.Checked), "Relkon");
        }

        private void ddlSearchingList_TextChanged(object sender, EventArgs e)
        {
            this.isInProgress = false;
        }

        private void ddlReplacingList_TextChanged(object sender, EventArgs e)
        {
            if (this.searchingMode == SearchingMode.Replace)
                this.isInProgress = false;
        }

        private void FindReplaceForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    this.FindNext();
                    break;
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.F:
                    if(e.Control)
                        this.SwitchToFindMode();
                    break;
                case Keys.H:
                    if (e.Control)
                        this.SwitchToReplaceMode();
                    break;
                case Keys.Enter:
                    bFindNext.PerformClick();
                    break;
            }
        }

        private void cbMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            this.isInProgress = false;
        }
    }
}