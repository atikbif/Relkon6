using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TD.SandDock;
using Kontel.Relkon.Solutions;
using System.IO;
using Kontel.Relkon;
using Kontel.Relkon.Forms;

namespace Kontel.Relkon.Components
{
    public sealed partial class SolutionExplorer : UserDockableWindow
    {
        #region Events declarations
        /// <summary>
        /// Возникает при двойном щелчке на узле свойств
        /// </summary>
        public event ProperitiesTreeNodeDoubleClickEventHandler ProperitiesTreeNodeDoubleClick;
        /// <summary>
        /// Возникает при двойном щелчке на узле файла
        /// </summary>
        public event FileTreeNodeDoubleClickEventHandler FileTreeNodeDoubleClick;
        /// <summary>
        /// Возникает при смене текста узла файла
        /// </summary>
        public event FileTreeNodeTextChangedEventHandler FileTreeNodeTextChanged;
        /// <summary>
        /// Возникает при смене текста узла проекта Relkon
        /// </summary>
        public event ControllerProgramSolutionTreeNodeTextChangedEventHandler ControllerProgramSolutionTreeNodeTextChanged;
        /// <summary>
        /// Возникает при смене текста узла проекта диспетчеризации
        /// </summary>
        //public event DispatheringSolutionTreeNodeTextChageEventHandler DispatheringSolutionTreeNodeTextChanged;
        /// <summary>
        /// Возникает, когда выбран пункт меню "Компилировать проекта" Relkon
        /// </summary>
        public event EventHandler<EventArgs<ControllerProgramSolution>> ControllerProgramSolutionNodeCompileClick;       
        #endregion

        public SolutionExplorer()
        {
            InitializeComponent();
            this.TabImage = this.ItemsIcons.Images["SolutionExplorer.bmp"];
        }

        public override string TabText
        {
            get
            {
                return this.Text;
            }
        }

        public override string Text
        {
            get
            {
                return "Обозреватель проекта";
            }
        }
        /// <summary>
        /// Добавляет проект к списку проектов
        /// </summary>
        public void AddSolutionNode(ControllerProgramSolution Solution) 
        {
            TreeNode node = null;
            if (Solution is ControllerProgramSolution)
                node = new ControllerProgramSolutionTreeNode((ControllerProgramSolution)Solution);            
            else
                throw new Exception("Проекты типа " + Solution.GetType().ToString() + " не поддерживаются");
            this.tvSolutionExplorer.Nodes.Add(node);
            node.Text = Solution.Name;
            node.Expand();
        }
        /// <summary>
        /// Добавляет новый файл к проекту Relkon
        /// </summary>
        private void AddFileToRelkonSolution(ControllerProgramSolutionTreeNode SolutionNode, string FileName)
        {
            string s = SolutionNode.Solution.DirectoryName + "\\" + Path.GetFileName(FileName);
            if (Path.GetExtension(FileName) == ".epj")
                s = FileName;
            bool fileExists = false;
            if (SolutionNode.Solution.Files.Contains(s))
            {
                if (Utils.QuestionMessage("Проект уже содержит файл " + Path.GetFileName(s) + ". Перезаписать ?", "Relkon") != DialogResult.Yes)
                    return;
                fileExists = true;
            }
            if (FileName != s)
                File.Copy(FileName, s, true);
            if (!fileExists)
            {
                SolutionNode.Solution.Files.Add(s);
                SolutionNode.AddFileNode(s);
                SolutionNode.Solution.Save();
            }
        }
        /// <summary>
        /// Добавляет новый файл к указанному проекту Rekon
        /// </summary>
        public void AddFileToRelkonSolution(ControllerProgramSolution Solution, string FileName)
        {
            ControllerProgramSolutionTreeNode SolutionNode = null;
            if (this.tvSolutionExplorer.Nodes[0].GetType() == typeof(ControllerProgramSolutionTreeNode))
            {
                SolutionNode = (ControllerProgramSolutionTreeNode)this.tvSolutionExplorer.Nodes[0];
                if (SolutionNode.Solution != Solution)
                    return;
            }
            else
            {
                foreach (TreeNode node in this.tvSolutionExplorer.Nodes[0].Nodes)
                {
                    if (node.GetType() == typeof(ControllerProgramSolutionTreeNode) && ((ControllerProgramSolutionTreeNode)node).Solution == Solution)
                    {
                        SolutionNode = (ControllerProgramSolutionTreeNode)node;
                        break;
                    }
                }
            }
            this.AddFileToRelkonSolution(SolutionNode, FileName);
        }
        /// <summary>
        /// Очищает окно
        /// </summary>
        public void Clear()
        {
            this.tvSolutionExplorer.Nodes.Clear();
        }
      

        private void tvSolutionExplorer_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node == null)
                return;
            if (e.Node.GetType() == typeof(ControllerProgramSolutionTreeNode) && this.ControllerProgramSolutionTreeNodeTextChanged != null)
            {
                SolutionTreeNodeTextChangedEventArgs re = new SolutionTreeNodeTextChangedEventArgs((ControllerProgramSolutionTreeNode)e.Node, e.Label);
                this.ControllerProgramSolutionTreeNodeTextChanged(this, re);
                e.CancelEdit = re.Cancel;
                this.tvSolutionExplorer.LabelEdit = false;
            }            
            else if (e.Node.GetType() == typeof(FileTreeNode) && this.FileTreeNodeTextChanged != null)
            {
                FileTreeNodeTextChangedEventArgs re = new FileTreeNodeTextChangedEventArgs((FileTreeNode)e.Node, e.Label);
                this.FileTreeNodeTextChanged(this, re);
                e.CancelEdit = re.Cancel;
            }
        }

        private void tvSolutionExplorer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.tvSolutionExplorer.SelectedNode.EndEdit(true);
            if (e.Node.GetType() == typeof(PropertiesTreeNode) && this.ProperitiesTreeNodeDoubleClick != null)
            {
                this.ProperitiesTreeNodeDoubleClick(this, new ProperitiesTreeNodeEventArgs((PropertiesTreeNode)e.Node));
            }
            if (e.Node.GetType() == typeof(FileTreeNode) && this.FileTreeNodeDoubleClick != null)
            {
                this.FileTreeNodeDoubleClick(this, new FileTreeNodeEventArgs((FileTreeNode)e.Node));
            }          
        }
        private void SolutionExplorerContextMenu_Opening(object sender, CancelEventArgs e)
        {
            while (this.SolutionExplorerContextMenu.Items.Count > 0)
                this.SolutionExplorerContextMenu.Items.Remove(this.SolutionExplorerContextMenu.Items[0]);
            if (this.tvSolutionExplorer.SelectedNode == null)
                return;
            if (this.tvSolutionExplorer.SelectedNode is PropertiesTreeNode)
                this.SolutionExplorerContextMenu.Items.Add(this.cmiOpen);
            if (this.tvSolutionExplorer.SelectedNode is FileTreeNode)
            {
                string FileName = ((FileTreeNode)this.tvSolutionExplorer.SelectedNode).FileName;
                this.SolutionExplorerContextMenu.Items.Add(this.cmiOpen);
                if (Path.GetExtension(FileName) != ".cntr")
                    this.SolutionExplorerContextMenu.Items.Add(this.cmiRename);
                if (Path.GetExtension(FileName) == ".epj" && this.tvSolutionExplorer.SelectedNode.Parent is ControllerProgramSolutionTreeNode)
                {
                    this.SolutionExplorerContextMenu.Items.Add(new ToolStripSeparator());
                    this.SolutionExplorerContextMenu.Items.Add(this.cmiExcludeFromSolution);
                    this.SolutionExplorerContextMenu.Items.Add(new ToolStripSeparator());
                    this.SolutionExplorerContextMenu.Items.Add(this.cmiConvertLCDPanelProject);
                }
                if ((Path.GetExtension(FileName) == ".plt"))
                {
                    if (FileName != ((ControllerProgramSolution)((FileTreeNode)this.tvSolutionExplorer.SelectedNode).Solution).PultFileName)
                    {
                        this.SolutionExplorerContextMenu.Items.Add(new ToolStripSeparator());
                        this.SolutionExplorerContextMenu.Items.Add(this.cmiExcludeFromSolution);
                        this.SolutionExplorerContextMenu.Items.Add(this.cmiSetAsActivePult);
                    }
                }
            }
            if (this.tvSolutionExplorer.SelectedNode is ControllerProgramSolutionTreeNode)
            {
                this.SolutionExplorerContextMenu.Items.Add(this.cmiAddNewItem);
                this.SolutionExplorerContextMenu.Items.Add(this.cmiAddExistingItem);
                this.SolutionExplorerContextMenu.Items.Add(this.cmiRename);
                this.SolutionExplorerContextMenu.Items.Add(new ToolStripSeparator());
                if (this.tvSolutionExplorer.SelectedNode.Parent != null)
                {
                    this.SolutionExplorerContextMenu.Items.Add(this.cmiExcludeFromSolution);
                    this.SolutionExplorerContextMenu.Items.Add(new ToolStripSeparator());
                }
                this.SolutionExplorerContextMenu.Items.Add(this.cmiCompile);
                this.SolutionExplorerContextMenu.Items.Add(this.cmiLoadToController);
                this.SolutionExplorerContextMenu.Items.Add(new ToolStripSeparator());
                this.SolutionExplorerContextMenu.Items.Add(this.cmiProperities);
            }           
        }

        private void cmiOpen_Click(object sender, System.EventArgs e)
        {
            if (this.tvSolutionExplorer.SelectedNode.GetType() == typeof(FileTreeNode) && this.FileTreeNodeDoubleClick != null)
                this.FileTreeNodeDoubleClick(this, new FileTreeNodeEventArgs((FileTreeNode)this.tvSolutionExplorer.SelectedNode));
            if (this.tvSolutionExplorer.SelectedNode.GetType() == typeof(PropertiesTreeNode) && this.ProperitiesTreeNodeDoubleClick != null)
                this.ProperitiesTreeNodeDoubleClick(this, new ProperitiesTreeNodeEventArgs((PropertiesTreeNode)this.tvSolutionExplorer.SelectedNode));
        }

        private void cmiRename_Click(object sender, System.EventArgs e)
        {
            if (this.tvSolutionExplorer.SelectedNode != null)
            {
                this.tvSolutionExplorer.LabelEdit = true;
                this.tvSolutionExplorer.SelectedNode.BeginEdit();
            }
        }

        private void cmiProperities_Click(object sender, System.EventArgs e)
        {
            if (this.tvSolutionExplorer.SelectedNode is SolutionTreeNode && this.ProperitiesTreeNodeDoubleClick != null)
            {
                foreach (TreeNode node in this.tvSolutionExplorer.SelectedNode.Nodes)
                {
                    if (node is PropertiesTreeNode)
                    {
                        this.ProperitiesTreeNodeDoubleClick(this, new ProperitiesTreeNodeEventArgs((PropertiesTreeNode)node));
                        break;
                    }
                }
            }
        }

        private void cmiAddLCDPanelProject_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "Файлы проектов EasyBuilder (*.epj)|*.epj|Все файлы|*.*";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.AddFileToRelkonSolution((ControllerProgramSolutionTreeNode)this.tvSolutionExplorer.SelectedNode, this.openFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage(ex.Message);
                }
            }
        }

        private void cmiConvertLCDPanelProject_Click(object sender, EventArgs e)
        {
            try
            {
                ((ControllerProgramSolutionTreeNode)this.tvSolutionExplorer.SelectedNode.Parent).Solution.ConvertLCDPanelProject(((FileTreeNode)this.tvSolutionExplorer.SelectedNode).FileName, ((ControllerProgramSolutionTreeNode)this.tvSolutionExplorer.SelectedNode.Parent).Solution.Vars);
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
            }
        }

        private void cmiCompile_Click(object sender, EventArgs e)
        {
            if (this.ControllerProgramSolutionNodeCompileClick != null)
                this.ControllerProgramSolutionNodeCompileClick(this, new EventArgs<ControllerProgramSolution>(((ControllerProgramSolutionTreeNode)this.tvSolutionExplorer.SelectedNode).Solution));
        }

        private void cmiAddNewPult_Click(object sender, EventArgs e)
        {
            ControllerProgramSolution solution = ((ControllerProgramSolutionTreeNode)this.tvSolutionExplorer.SelectedNode).Solution;
            string FileName = solution.DirectoryName + "\\NewPult1.plt";
            for (int i = 2; solution.Files.Contains(FileName) || File.Exists(FileName); i++)
                FileName = FileName = solution.DirectoryName + "\\NewPult" + i + ".plt";
            RelkonPultModel pult = new RelkonPultModel(solution.PultParams.DefaultPultType);
            pult.Save(FileName);
            this.AddFileToRelkonSolution(solution, FileName);
            MainForm.MainFormInstance.LoadFile(FileName);
        }

        private void cmiExcludeFromSolution_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(this.tvSolutionExplorer.SelectedNode is ControllerProgramSolutionTreeNode))
                {
                    FileTreeNode node = (FileTreeNode)this.tvSolutionExplorer.SelectedNode;
                    node.Solution.Files.Remove(node.FileName);
                    node.Solution.Save();
                    node.Remove();
                }               
            }
            catch (Exception ex)
            {
                Utils.ErrorMessage(ex.Message);
            }
        }

        private void cmiPultProperties_Click(object sender, EventArgs e)
        {
            if (this.ProperitiesTreeNodeDoubleClick != null)
                this.ProperitiesTreeNodeDoubleClick(this, new ProperitiesTreeNodeEventArgs(new PultPropertiesTreeNode(((ControllerProgramSolutionTreeNode)this.tvSolutionExplorer.SelectedNode.Parent).Solution, ((FileTreeNode)this.tvSolutionExplorer.SelectedNode).FileName)));
        }

        private void cmiSetAsActivePult_Click(object sender, EventArgs e)
        {
            ControllerProgramSolution solution = ((ControllerProgramSolutionTreeNode)this.tvSolutionExplorer.SelectedNode.Parent).Solution;
            FileTreeNode cn = (FileTreeNode)this.tvSolutionExplorer.SelectedNode;
            foreach (TreeNode node in this.tvSolutionExplorer.SelectedNode.Parent.Nodes)
            {
                if (node is FileTreeNode && ((FileTreeNode)node).FileName == solution.PultFileName)
                {
                    node.NodeFont = new Font(this.Font, FontStyle.Regular);
                    break;
                }
            }
            solution.PultFileName = cn.FileName;
            string s = cn.Text;
            cn.NodeFont = new Font(this.Font, FontStyle.Bold);
            cn.Text = s;
            MainForm.MainFormInstance.ReloadSolutionPropertiesDocument(solution);
        }

        private void cmiAddExistingPult_Click(object sender, EventArgs e)
        {
            ControllerProgramSolution solution = ((ControllerProgramSolutionTreeNode)this.tvSolutionExplorer.SelectedNode).Solution;
            this.openFileDialog1.Filter = "Файлы пультов (*.fpr, *.plt)|*.fpr;*.plt|Все файлы|*.*";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = this.openFileDialog1.FileName.Replace(".fpr", "").Replace(".plt", "").Replace(".bak", "bak") + ".plt";
                if (File.Exists(solution.DirectoryName + "\\" + Path.GetFileName(FileName)) && Path.GetDirectoryName(FileName) != solution.DirectoryName &&
                    Utils.QuestionMessage("Файл \"" + Path.GetFileName(FileName) + "\" уже существует в каталоге проекта. Перезаписать ?", "Relkon") != DialogResult.Yes)
                    return;
                try
                {
                    RelkonPultModel pult = RelkonPultModel.FromFile(this.openFileDialog1.FileName);
                    pult.Save(FileName);
                    this.AddFileToRelkonSolution((ControllerProgramSolutionTreeNode)this.tvSolutionExplorer.SelectedNode, FileName);
                }
                catch (Exception ex)
                {
                    Utils.ErrorMessage(ex.Message);
                }
            }
        }

        private void cmiAddExistingItem_DropDownOpening(object sender, EventArgs e)
        {
            if (this.tvSolutionExplorer.SelectedNode is ControllerProgramSolutionTreeNode)
            {
                ControllerProgramSolution sln = ((ControllerProgramSolutionTreeNode)this.tvSolutionExplorer.SelectedNode).Solution;
                //if (sln is AT89C51ED2Solution && this.cmiAddExistingItem.DropDown.Items.Contains(this.cmiAddLCDPanelProject))
                //    this.cmiAddExistingItem.DropDown.Items.Remove(this.cmiAddLCDPanelProject);
                if (sln is STM32F107Solution && !this.cmiAddExistingItem.DropDown.Items.Contains(this.cmiAddLCDPanelProject))
                    this.cmiAddExistingItem.DropDown.Items.Add(this.cmiAddLCDPanelProject);
            }
        }

        private void tvSolutionExplorer_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node==null || e.Node is PropertiesTreeNode || e.Node is ArchiveTreeNode)
            {
                e.CancelEdit = true;
            }
        }

        private void tvSolutionExplorer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
                this.tvSolutionExplorer.LabelEdit = true;
        }

        private void tvSolutionExplorer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
                this.tvSolutionExplorer.LabelEdit = true;
        }

        private void cmiLoadToController_Click(object sender, EventArgs e)
        {
            SolutionTreeNode node = (SolutionTreeNode)this.tvSolutionExplorer.SelectedNode;
            MainForm.MainFormInstance.UploadToDevice(node.Solution, true, true, false);
        }
    }

    #region TreeNode's declarations

    #region FileTreeNode
    /// <summary>
    /// Используется для отображения файлов проекта
    /// </summary>
    public class FileTreeNode : TreeNode
    {
        private string fileName = ""; // имя файла, который отображает узел (без пути доступа)
        private ControllerProgramSolution solution = null;
        /// <param name="FileName">Имя файла, который отображает узел (с путем доступа)</param>
        /// <param name="Solution">Проект, к которому относится файл</param>
        public FileTreeNode(string FileName, ControllerProgramSolution Solution)
        {
            string ImageKey = this.GetImageKey(FileName);
            if (!File.Exists(FileName))
                ImageKey = "Unavailable" + ImageKey;
            this.ImageKey = ImageKey;
            this.SelectedImageKey = ImageKey;
            this.Text = Path.GetFileName(FileName);
            this.ToolTipText = FileName;
            this.fileName = Path.GetFileName(FileName);
            this.solution = Solution;
        }
        /// <summary>
        /// Возвращает (с путем доступа) или устанавливает (без пути доступа) имя файла, который отображает узел
        /// </summary>
        public string FileName
        {
            get
            {
                return this.solution.DirectoryName + "\\" + this.fileName;
            }
            set
            {
                if (!ControllerProgramSolution.IsValidIdentifier(value))
                {
                    throw new Exception("Идентификаторы и имена файлов не могут:\r\n" +
                                        "- содержать любые из следующих символов: / ? : & \\ * \" < > | # %\r\n" +
                                        "- содержать управляющие символы Unicode\r\n" +
                                        "- быть зарезервированными системными именами, например 'CON', 'AUX', 'PRN','COM1', LPT2 и т.д.\r\n" +
                                        "- быть '.' или '..'\r\n\r\n" +
                                        "Пожалуйста введите правильное имя");
                }
                this.fileName = value;
                this.Text = value;
            }
        }
        /// <summary>
        /// Возвращает проект, к которому относится файл, отображаемый узлом
        /// </summary>
        public ControllerProgramSolution Solution
        {
            get
            {
                return this.solution;
            }
        }
        /// <summary>
        /// Возвращает ImageKey для указанного файла
        /// </summary>
        private string GetImageKey(string FileName)
        {
            string res = "";
            switch (Path.GetExtension(FileName))
            {
                case ".kon":
                    res = "RelkonProgram.bmp";
                    break;
                case ".plt":
                    res = "PultFile.bmp";
                    break;
                case ".c":
                    res = "CFile.bmp";
                    break;
                case ".h":
                    res = "HFile.bmp";
                    break;
                case ".map":
                    res = "MapFile.bmp";
                    break;
                case ".asm":
                    res = "AsmFile.bmp";
                    break;
                case ".epj":
                    res = "LCDPanelProject.bmp";
                    break;
                case ".cntr":
                    res = "ControllersFile.bmp";
                    break;
                default:
                    res = "UnknownFile.bmp";
                    break;
            }
            return res;
        }
    }
    #endregion

    #region ProperitiesTreeNode
    /// <summary>
    /// Отображает узел свойств проекта
    /// </summary>
    public class PropertiesTreeNode : TreeNode
    {
        private ControllerProgramSolution solution = null;

        public PropertiesTreeNode(ControllerProgramSolution Solution)
        {
            this.ImageKey = "ProjectProperities.bmp";
            this.SelectedImageKey = "ProjectProperities.bmp";
            this.Text = "Настройки проекта";
            this.solution = Solution;
        }
        /// <summary>
        /// Возвращает проект, свойства которого представляет узел
        /// </summary>
        public ControllerProgramSolution Solution
        {
            get
            {
                return this.solution;
            }
        }
    }
    /// <summary>
    /// Отображает узел сойств пульта
    /// </summary>
    public class PultPropertiesTreeNode : PropertiesTreeNode
    {
        string fileName;

        public PultPropertiesTreeNode(ControllerProgramSolution Solution, string FileName)
            : base(Solution)
        {
            this.fileName = FileName;
        }
        /// <summary>
        /// Возвращает имя файла, свойства которого отображает узел
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }
    }

    #endregion

    /// <summary>
    /// Базовый класс для узлов отоборажающих какой либо тип проекта
    /// </summary>
    public abstract class SolutionTreeNode : TreeNode
    {
        protected ControllerProgramSolution solution; // отображаемый узлом проект

        public SolutionTreeNode(ControllerProgramSolution Solution)
        {
            this.solution = Solution;
            this.ToolTipText = Solution.SolutionFileName;
        }

        /// <summary>
        /// Возвращает проект, отображаемый узлом
        /// </summary>
        public ControllerProgramSolution Solution
        {
            get
            {
                return this.solution;
            }
        }
        /// <summary>
        /// Обновляет дерево проекта
        /// </summary>
        public abstract void Refresh();
    }
    /// <summary>
    /// Используется для отображения файлов проекта Relkon
    /// </summary>
    public class ControllerProgramSolutionTreeNode : SolutionTreeNode
    {
        /// <summary>
        /// Возвращает проект, который соответствует узлу
        /// </summary>
        public new ControllerProgramSolution Solution
        {
            get
            {
                return this.solution as ControllerProgramSolution;
            }
        }

        public ControllerProgramSolutionTreeNode(ControllerProgramSolution Solution)
            : base(Solution)
        {
            this.solution = Solution;
            this.Text = Solution.Name;
            this.ImageKey = "RelkonProject.bmp";
            this.SelectedImageKey = "RelkonProject.bmp";
            this.NodeFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this.CreateNodesTree();
        }
        /// <summary>
        /// Добавляет узел, отображаюий указанный файл пультов
        /// </summary>
        public void AddPultFileNode(string FileName)
        {
            FileTreeNode fnode = new FileTreeNode(FileName, this.Solution);
            if (FileName == this.Solution.PultFileName)
                fnode.NodeFont = new Font(this.NodeFont, FontStyle.Bold);
            int idx = 3; // 0-узел свойств, 1 - программа, 2 - пульт по-умолчанию
            while (idx < this.Nodes.Count && this.Nodes[idx] is FileTreeNode && Path.GetExtension(((FileTreeNode)this.Nodes[idx]).FileName) == ".plt")
                idx++;
            this.Nodes.Insert(idx, fnode);
        }
        /// <summary>
        /// Добавляет узел, отображающий указанный файл
        /// </summary>
        public void AddFileNode(string FileName)
        {
            if (Path.GetExtension(FileName) == ".plt")
                this.AddPultFileNode(FileName);
            else
            {
                FileTreeNode fnode = new FileTreeNode(FileName, this.solution);
                if (FileName == this.Solution.ProgramFileName)
                    fnode.NodeFont = new Font(this.NodeFont, FontStyle.Bold);
                this.Nodes.Add(fnode);
            }
        }
        /// <summary>
        /// Создает дерево узлов проекта
        /// </summary>
        private void CreateNodesTree()
        {
            this.Nodes.Add(new PropertiesTreeNode(Solution));
            for (int i = 0; i < Solution.Files.Count; i++)
            {
                string FileName = Solution.Files[i];
                bool b = (Path.GetExtension(FileName) == ".kon" || Path.GetExtension(FileName) == ".plt" || Path.GetExtension(FileName) == ".epj") ? true : false;
                if (!b && !File.Exists(FileName))
                {
                    Solution.Files.RemoveAt(i--);
                    continue;
                }
                this.AddFileNode(FileName);
            }
        }

        public override void Refresh()
        {
            this.Nodes.Clear();
            this.CreateNodesTree();
        }
    }
    /// <summary>
    /// Класс отображающий узел архива
    /// </summary>
    public class ArchiveTreeNode : TreeNode
    {
        public ArchiveTreeNode()
        {
            this.ImageKey = "Archive";
            this.SelectedImageKey = "Archive";
            this.Text = "Просмотр архива";
        }
    }
    
    #endregion

    #region EventArgs's declarations

    public class ProperitiesTreeNodeEventArgs : EventArgs
    {
        private PropertiesTreeNode node = null;
        /// <summary>
        /// Узел, сгенерировавший событие
        /// </summary>
        public PropertiesTreeNode Node
        {
            get
            {
                return this.node;
            }
        }
        public ProperitiesTreeNodeEventArgs(PropertiesTreeNode Node)
        {
            this.node = Node;
        }
    }

    public class FileTreeNodeEventArgs : EventArgs
    {
        private FileTreeNode node = null;
        /// <summary>
        /// Узел, сгенерировавший событие
        /// </summary>
        public FileTreeNode Node
        {
            get
            {
                return this.node;
            }
        }
        public FileTreeNodeEventArgs(FileTreeNode Node)
        {
            this.node = Node;
        }
    }

    public class FileTreeNodeTextChangedEventArgs : FileTreeNodeEventArgs
    {
        private string text = "";
        private bool cancel = false;

        /// <summary>
        /// Новый текст узла
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }
        }
        /// <summary>
        /// В случае необходимости отменяет изменение текста
        /// </summary>
        public bool Cancel
        {
            get
            {
                return this.cancel;
            }
            set
            {
                this.cancel = value;
            }
        }

        public FileTreeNodeTextChangedEventArgs(FileTreeNode Node, string Text)
            : base(Node)
        {
            this.text = Text;
        }
    }

    public class SolutionTreeNodeTextChangedEventArgs : EventArgs
    {
        private SolutionTreeNode node = null;
        private string text = "";
        private bool cancel = false;
        /// <summary>
        /// Узел, сгенерировавший событие
        /// </summary>
        public SolutionTreeNode Node
        {
            get
            {
                return this.node;
            }
        }
        /// <summary>
        /// Новый текст узла
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }
        }
        /// <summary>
        /// В случае необходимости отменяет изменение текста
        /// </summary>
        public bool Cancel
        {
            get
            {
                return this.cancel;
            }
            set
            {
                this.cancel = value;
            }
        }

        public SolutionTreeNodeTextChangedEventArgs(SolutionTreeNode Node, string Text)
        {
            this.node = Node;
            this.text = Text;
        }

    }

    #endregion

    #region Events delegates declarations
    public delegate void ProperitiesTreeNodeDoubleClickEventHandler(object sender, ProperitiesTreeNodeEventArgs e);
    public delegate void FileTreeNodeDoubleClickEventHandler(object sender, FileTreeNodeEventArgs e);
    public delegate void FileTreeNodeTextChangedEventHandler(object sender, FileTreeNodeTextChangedEventArgs e);
    public delegate void ControllerProgramSolutionTreeNodeTextChangedEventHandler(object sender, SolutionTreeNodeTextChangedEventArgs e);
    public delegate void DispatheringSolutionTreeNodeTextChageEventHandler(object sender, SolutionTreeNodeTextChangedEventArgs e);
    #endregion
}
