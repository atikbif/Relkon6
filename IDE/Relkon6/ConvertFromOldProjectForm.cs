using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon.Solutions;
using System.IO;
using Kontel.Relkon;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace Kontel.Relkon
{
    public sealed partial class ConvertFromOldProjectForm : Form
    {
        private string fileName = ""; // имя созданного проекта, если создано больше дного проекта, то имя первого созданого проекта
        private string backUpFolderName = ""; // имя каталога, в котором сохранены исходные файлы перед конвертацией
        private List<string> pultFileNames = new List<string>(); // имена файлов пультов, которые надо преобразовать

        public ConvertFromOldProjectForm()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
        }
        /// <summary>
        /// Возвращает имя файла созданного проекта
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }
        /// <summary>
        /// Перемещает файлы старого проекта в папку сохранения
        /// </summary>
        /// <param name="Solution">Новый проект, созданный на основе старых файлов</param>
        private void SaveOldProjectFiles()
        {
            string DirectoryName = Path.GetDirectoryName(this.tbProgramFileName.Text) + "\\_" + Path.GetFileNameWithoutExtension(this.tbProgramFileName.Text) + ".bak";
            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);
            File.Copy(this.tbProgramFileName.Text, DirectoryName + "\\" + Path.GetFileName(this.tbProgramFileName.Text), true);
            string s = "";
            int idx = 0;
            foreach (string FileName in this.lbPultFiles.Items)
            {
                try
                {
                    if (RelkonPultModel.GetPultFileVersion(FileName) == PultFileVersion.v50)
                    {
                        s += FileName + ", ";
                        idx++;
                    }
                    else
                    {
                        this.pultFileNames.Add(FileName);
                        File.Copy(FileName, DirectoryName + "\\" + Path.GetFileName(FileName), true);
                    }
                }
                catch { }
            }
            if(s!="")
                Utils.InformationMessage("Файл" + (idx == 1 ? "" : "ы") + " пультов " + s.Remove(s.Length - 2) + " уже в формате Relkon5. Преобразование не требуется.", "Relkon");
            this.backUpFolderName = DirectoryName;
        }
        /// <summary>
        /// Вычисляет значения двух- и четырехбайтных переменных заводских установок на основе
        /// значений однобайтных переменных для указанного проекта
        /// </summary>
        private void ComputeMultibyteEmbeddedVarsValues(ControllerProgramSolution solution)
        {
            for (int count = 2; count < 5; count += 2)
            {
                string s = (count == 2) ? "i" : "l";
                for (char c = 'W'; c <= 'Z'; c++)
                {
                    for (int i = 0; i < 16; i += count)
                    {
                        byte[] bytes = new byte[count];
                        for (int j = 0; j < count; j++)
                        {
                            bytes[j] = (byte)solution.Vars.GetEmbeddedVar(c.ToString() + (i + j)).Value;
                        }
                        if (solution.ProcessorParams.InverseByteOrder)
                            bytes = Utils.ReflectArray<byte>(bytes);
                        solution.Vars.GetEmbeddedVar(c.ToString() + i + s).Value = AppliedMath.BytesToLong(bytes);
                    }
                }
                for (int i = 0; i < 64; i += count)
                {
                    byte[] bytes = new byte[count];
                    for (int j = 0; j < count; j++)
                    {
                        bytes[j] = (byte)solution.Vars.GetEmbeddedVar("EE" + (i + j)).Value;
                    }
                    if (solution.ProcessorParams.InverseByteOrder)
                        bytes = Utils.ReflectArray<byte>(bytes);
                    solution.Vars.GetEmbeddedVar("EE" + i + s).Value = AppliedMath.BytesToLong(bytes);
                }
            }
        }
        /// <summary>
        /// Загружает настройки проекта из указанного файла пультов
        /// </summary>
        //private void LoadParamsToSolutionFromPult(ControllerProgramSolution solution, string FileName)
        //{
        //    PultFileVersion version = RelkonPultModel.GetPultFileVersion(FileName);
        //    switch (version)
        //    {                
        //        case PultFileVersion.v37:
        //            this.LoadParamsToSolutionFromPlt37(solution, FileName);
        //            break;
        //    }
        //    solution.ComputeMultibyteEmbeddedVarsValues();
        //}
        /// <summary>
        /// Загружает настройки в проект из fpr-файла
        /// </summary>
        //private void LoadParamsToSolutionFromFpr(AT89C51ED2Solution solution, string FileName)
        //{
        //    solution.Vars.EmbeddedVars.Clear();
        //    solution.Vars.EmbeddedVars.AddRange(RelkonPultModel.GetEmbeddedVarsFromFpr(FileName));
        //    using (StreamReader reader = new StreamReader(FileName, Encoding.Default))
        //    {
        //        solution.BaudRate = int.Parse(reader.ReadLine()); //считывание скорости
        //        solution.Protocol = reader.ReadLine() == "1" ? ProtocolType.RC51BIN : ProtocolType.RC51ASCII;
        //        solution.ReadPassword = reader.ReadLine(); //пароль на чтение
        //        solution.WritePassword = reader.ReadLine(); //пароль на запись 
        //    }
        //}
        /// <summary>
        /// Загружает настройки в проект из plt-файла версии 3.7
        /// </summary>
        //private void LoadParamsToSolutionFromPlt37(ControllerProgramSolution solution, string FileName)
        //{
        //    solution.Vars.EmbeddedVars.Clear();
        //    solution.Vars.EmbeddedVars.AddRange(RelkonPultModel.GetEmbeddedVarsFromPlt37(FileName));
        //    if (File.ReadAllText(FileName).Contains("Fujitsu"))
        //        this.LoadParamsToMB90F347SolutionFromPlt37((MB90F347Solution)solution, FileName);
        //    else
        //        this.LoadParamsToAT89C51ED2SolutionFromPlt37((AT89C51ED2Solution)solution, FileName);
        //}
        /// <summary>
        /// Загружает настройки в проект AT89C51ED2 из plt-файла версии 3.7
        /// </summary>
        //private void LoadParamsToAT89C51ED2SolutionFromPlt37(AT89C51ED2Solution solution, string FileName)
        //{
        //    XPathDocument xpDoc = new XPathDocument(FileName);
        //    XPathNavigator xpNav = ((IXPathNavigable)xpDoc).CreateNavigator();
        //    XPathNodeIterator xpList = xpNav.Select("/fpultProject");
        //    xpList.MoveNext();
        //    solution.ControllerAddress = int.Parse(xpList.Current.GetAttribute("number", ""));
        //    solution.Protocol = (xpList.Current.GetAttribute("protocol", "") == "RC51BIN") ? ProtocolType.RC51BIN : ProtocolType.RC51ASCII;
        //    solution.ReadPassword = xpList.Current.GetAttribute("readPass", "");
        //    solution.WritePassword = xpList.Current.GetAttribute("writePass", "");
        //    solution.BaudRate = int.Parse(xpList.Current.GetAttribute("speed", ""));
        //}
        /// <summary>
        /// Загружает настройки в проект MB90F347 из plt-файла версии 3.7
        /// </summary>
        //private void LoadParamsToMB90F347SolutionFromPlt37(MB90F347Solution solution, string FileName)
        //{
        //    XPathDocument xpDoc = new XPathDocument(FileName);
        //    XPathNavigator xpNav = ((IXPathNavigable)xpDoc).CreateNavigator();
        //    XPathNodeIterator xpList = xpNav.Select("/fpultProject");
        //    xpList.MoveNext();
        //    solution.ControllerAddress = int.Parse(xpList.Current.GetAttribute("number", ""));

        //    XPathNodeIterator xpPortOptions = xpNav.Select("/fpultProject/portOptions");
        //    int index = 0;
        //    while (xpPortOptions.MoveNext())
        //    {
        //        if (int.Parse(xpPortOptions.Current.GetAttribute("index", "")) == 1)
        //            continue;
        //        solution.Uarts[index].ReadPassword = xpPortOptions.Current.GetAttribute("readPassword", "");
        //        solution.Uarts[index].WritePassword = xpPortOptions.Current.GetAttribute("writePassword", "");
        //        solution.Uarts[index].ParseParametersByte(byte.Parse(xpPortOptions.Current.GetAttribute("parameters", "")));

        //        int start = int.Parse(xpPortOptions.Current.GetAttribute("rxStart", ""));
        //        int end = int.Parse(xpPortOptions.Current.GetAttribute("rxEnd", ""));
        //        solution.Uarts[index].BufferSize = end - start + 1;

        //        start = int.Parse(xpPortOptions.Current.GetAttribute("txStart", ""));
        //        end = int.Parse(xpPortOptions.Current.GetAttribute("txEnd", ""));
        //        solution.Uarts[index].BufferSize = end - start + 1;
        //        index++;
        //    }
        //    XPathNodeIterator xpConnectionSettings = xpNav.Select("/fpultProject/connectionSettings");
        //    while (xpConnectionSettings.MoveNext())
        //    {
        //        solution.SearchedControllerAddress = int.Parse(xpConnectionSettings.Current.GetAttribute("searchingControllerAddress", ""));
        //        solution.DispatcherPhone1 = xpConnectionSettings.Current.GetAttribute("modemPhone1", "");
        //        solution.DispatcherPhone1 = xpConnectionSettings.Current.GetAttribute("modemPhone2", "");
        //        solution.DispatcherPhone1 = xpConnectionSettings.Current.GetAttribute("modemPhone3", "");
        //        solution.SmsPhone1 = xpConnectionSettings.Current.GetAttribute("smsPhone1", "");
        //        solution.SmsPhone1 = xpConnectionSettings.Current.GetAttribute("smsPhone2", "");
        //        solution.SmsPhone1 = xpConnectionSettings.Current.GetAttribute("smsPhone3", "");
        //        solution.ModemInitializationString = xpConnectionSettings.Current.GetAttribute("modemInitString", "");
        //        solution.DenyProgrammingThrowProtocol = bool.Parse(xpConnectionSettings.Current.GetAttribute("denyProgramming", ""));
        //    }
        //}
        /// <summary>
        /// На основании указаных файла программы и файла пультов создает новый проект
        /// </summary>
        /// <param name="ProgramFileName">Имя файла программы</param>
        /// <param name="PultFileName">Имя файла пультов</param>
        /// <param name="DirectoryName">Имя каталога, в котором должен быть создан проект</param>
        /// <param name="CreateDirectoryForSolution">
        /// Если true, то в каталоге DirectoryName будет создан каталог для проекта,
        /// причем имя каталога будет именем файла пультов
        /// </param>
        private ControllerProgramSolution CreateSolution(string ProgramFileName, string  PultFileName, bool CreateDirectoryForSolution)
        {
            ControllerProgramSolution res = File.ReadAllText(ProgramFileName).Contains("f347") ? ControllerProgramSolution.Create(ProcessorType.MB90F347) : ControllerProgramSolution.Create(ProcessorType.AT89C51ED2);
            RelkonPultModel pult = RelkonPultModel.FromFile(PultFileName);
            if (Array.IndexOf(res.PultParams.AvailablePultTypes, pult.Type) == -1)
                throw new Exception("Тип пульта файла " + Path.GetFileName(PultFileName) + " не совместим с типом процессора программы " + Path.GetFileName(ProgramFileName));
            string SolutionDirectoryName = Path.GetDirectoryName(this.tbProgramFileName.Text) + (CreateDirectoryForSolution ? "\\" + Path.GetFileNameWithoutExtension(PultFileName) : "");
            if (CreateDirectoryForSolution && !Directory.Exists(SolutionDirectoryName))
                Directory.CreateDirectory(SolutionDirectoryName);
            //this.LoadParamsToSolutionFromPult(res, PultFileName);
            string SolutionProgramFileName = SolutionDirectoryName + "\\" + Path.GetFileNameWithoutExtension(ProgramFileName) + ".kon";
            string SolutionPultFileName = SolutionDirectoryName + "\\" + Path.GetFileNameWithoutExtension(PultFileName) + ".plt";
            File.Copy(ProgramFileName, SolutionProgramFileName);
            // Удаление из файла строки с типом процессора
            string[] s = File.ReadAllLines(SolutionProgramFileName, Encoding.Default);
            if (s.Length > 1 && (s[1].Contains("f347") || s[1].ToLower().Contains("тип контроллера")))
                s[1] = "";
            File.WriteAllLines(SolutionProgramFileName, s, Encoding.Default);
            ///////////////////////////////////////////////
            pult.Save(SolutionPultFileName);
            res.ProgramFileName = SolutionProgramFileName;
            res.PultFileName = SolutionPultFileName;
            res.Files.Add(SolutionProgramFileName);
            res.Files.Add(SolutionPultFileName);
            res.OpenedFiles.Add(SolutionProgramFileName);
            res.OpenedFiles.Add(SolutionPultFileName);
            res.SaveAs(SolutionDirectoryName + "\\" + Path.GetFileNameWithoutExtension(ProgramFileName) + ".rp6");
            return res;
        }
        /// <summary>
        /// Создает проект Relkon на основе старых файлов
        /// </summary>
        private void CreateRelkonSolutionFromOldFiles()
        {
            this.SaveOldProjectFiles();
            this.fileName = "";
            if (this.pultFileNames.Count != 0)
                File.Delete(this.tbProgramFileName.Text);
            foreach (string FileName in this.pultFileNames)
            {
                try
                {
                    ControllerProgramSolution sln = this.CreateSolution(this.backUpFolderName + "\\" + Path.GetFileName(this.tbProgramFileName.Text), FileName, this.lbPultFiles.Items.Count > 1);
                    if (this.fileName == "")
                        this.fileName = sln.SolutionFileName;
                    if (this.lbPultFiles.Items.Count > 1)
                        File.Delete(FileName);
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage(ex.Message);
                }
            }
        }
        /// <summary>
        /// Преобразует текст программы к формату Relkon37
        /// </summary>
        private void ConvertProgramToRelkon37Format(string FileName)
        {
            try
            {
                StringBuilder res = new StringBuilder(File.ReadAllText(FileName, Encoding.Default));
                MatchCollection mc1 = Regex.Matches(res.ToString(), @"\#define\s+([\S_]+)\s+\(\s*\*\s*\(\s*\(([^\*]*)\s*\*\s*\)\s*(0x[0-9A-F]{2,4})\s*\)\s*\)", RegexOptions.IgnoreCase);
                int k = 0;
                for (int i = 0; i < mc1.Count; i++)
                {
                    string s = "far " + mc1[i].Groups[2].Value + " at " + mc1[i].Groups[3].Value + " " + mc1[i].Groups[1].Value + ";";
                    res.Replace(mc1[i].Value, s, mc1[i].Index + k, mc1[i].Length);
                    k += s.Length - mc1[i].Length;
                }
                File.WriteAllText(FileName, res.ToString(), Encoding.Default);
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
            }
        }

        private void bBrowseProgram_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.FilterIndex = 1;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                this.tbProgramFileName.Text = this.openFileDialog1.FileName;
        }

        private void bBrowsePult_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.FilterIndex = 2;
            this.openFileDialog1.Multiselect = true;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                for(int i = 0; i<this.openFileDialog1.FileNames.Length; i++)
                {
                    if(!this.lbPultFiles.Items.Contains(this.openFileDialog1.FileNames[i]))
                        this.lbPultFiles.Items.Add(this.openFileDialog1.FileNames[i]);
                }
                if (this.lbPultFiles.Items.Count > 0)
                    this.bCreateProject.Enabled = this.bRemovePultFile.Enabled = true;
            }
            this.openFileDialog1.Multiselect = false;
            this.bCreateProject.Enabled = (this.tbProgramFileName.Text.Trim().Length > 0) && this.lbPultFiles.Items.Count > 0;
        }

        private void bCreateProject_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            try
            {
                this.CreateRelkonSolutionFromOldFiles();
                this.Close();
                if(this.fileName != "")
                    this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage("Ошибка создания проекта: " + Utils.FirstLetterToLower(ex.Message));
            }
        }

        private void bRemovePultFile_Click(object sender, EventArgs e)
        {
            if (this.lbPultFiles.SelectedIndex == -1)
                return;
            lbPultFiles.Items.RemoveAt(this.lbPultFiles.SelectedIndex);
            if (this.lbPultFiles.Items.Count == 0)
                this.bCreateProject.Enabled = this.bRemovePultFile.Enabled = false;
            this.bCreateProject.Enabled = (this.tbProgramFileName.Text.Trim().Length > 0) && this.lbPultFiles.Items.Count > 0;
        }

        private void tbProgramFileName_TextChanged(object sender, EventArgs e)
        {
            this.bCreateProject.Enabled = (this.tbProgramFileName.Text.Trim().Length > 0) && this.lbPultFiles.Items.Count > 0;
        }
    }
}