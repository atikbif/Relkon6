using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.Classes;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using Kontel.Relkon;
using Kontel.Relkon.Solutions;
using System.Xml;
using System.CodeDom.Compiler;

namespace Kontel.Relkon.Solutions
{
    public enum PultFileVersion
    {
        v35,
        v37,
        v50
    }

    public sealed class RelkonPultModel : IDisposable
    {
        private List<Row> rows; // ������ ����� ������
        private PultType type; // ��� ������
        private string fileName; // �������� ��� �����, �� �������� ���� ��������� ������
        
        public RelkonPultModel(PultType Type)
        {
            this.Initialize(Type);
        }

        public RelkonPultModel()
        {
            this.rows = new List<Row>();
        }
        
        public Row this[int index]
        {
            get
            {
                return this.rows[index];
            }
        }
        /// <summary>
        /// ���������� ��� �����, �� �������� ���� ��������� ������
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }
        /// <summary>
        /// �������������� ��������� ������
        /// </summary>
        private void Initialize(PultType Type)
        {
            this.type = Type;
            this.rows = new List<Row>();
            for (int i = 0; i < Type.RowCount; i++)
            {
                Row r = new Row();
                r.Views.Add(new View());
                this.rows.Add(r);
            }
        }
        /// <summary>
        /// ����������, �������� �� ������ ������
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                bool res = true;
                foreach (Row row in this.rows)
                {
                    res &= (row.Views.Count == 1) && (row.Views[0].Text.Trim() == "") && (row.Views[0].Vars.Count == 0);
                    if(!res)
                        break;
                }
                return res;
            }
        }
        /// <summary>
        /// ���������� ��� ������������� ��� ������, ������� ������������ �����
        /// </summary>
        public PultType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
        /// <summary>
        /// ���������� ������ ����� ������
        /// </summary>
        public List<Row> Rows
        {
            get
            {
                return this.rows;
            }
        }
        /// <summary>
        /// ���������� ����� ��-��������� (2x16)
        /// </summary>
        public static RelkonPultModel Default
        {
            get
            {
                RelkonPultModel res = new RelkonPultModel();
                res.Initialize(PultType.Pult2x16);
                return res;
            }
        }
        /// <summary>
        /// ���������� ������ ���������� ����� �������
        /// </summary>
        public static PultFileVersion GetPultFileVersion(string FileName)
        {
            using (StreamReader reader = new StreamReader(FileName, Encoding.Default))
            {
                string line = reader.ReadLine() + reader.ReadLine();
                if (line.StartsWith("<?xml"))
                {
                    if (line.Contains("fpultProject"))
                        return PultFileVersion.v37;
                    else
                        return PultFileVersion.v50;
                }
                else
                    return PultFileVersion.v35;
            }
        }
        /// <summary>
        /// ��������� ������ ������ �� ���������� �����
        /// </summary>
        public static RelkonPultModel FromFile(string FileName)
        {
            RelkonPultModel res = null;
            PultFileVersion version = RelkonPultModel.GetPultFileVersion(FileName);
            switch (version)
            {
                //case PultFileVersion.v35:
                //    res = RelkonPultModel.GetPultModelFromFpr(FileName);
                //    break;
                //case PultFileVersion.v37:
                //    res = RelkonPultModel.GetPultModelFromPlt37(FileName);
                //    break;
                case PultFileVersion.v50:
                    res = RelkonPultModel.GetPultModelFromPlt5(FileName);
                    break;
            }
            res.fileName = FileName;
            if (res.type == null)
                res.type = PultType.GetPultType(res.rows.Count, res.rows[0].Views[0].Text.Length);
            return res;
        }
        /// <summary>
        /// ���������� ������ ������ �� ���������� fpr-����� 
        /// </summary>
        //private static RelkonPultModel GetPultModelFromFpr(string FileName)
        //{
        //    RelkonPultModel res = new RelkonPultModel(PultType.Pult2x16);
        //    res.rows[0].Views.Clear();
        //    res.rows[1].Views.Clear();
        //    StreamReader reader = new StreamReader(FileName, Encoding.Default);
        //    reader.ReadLine(); // ��������
        //    reader.ReadLine(); // ��������
        //    reader.ReadLine(); // ������ �� ������
        //    reader.ReadLine(); // ������ �� ������ 
        //    int K = reader.ReadLine() == "1" ? 0 : 3; // K52 - 16 ����������, K51ED2 - 64; "1" - K51ED2
        //    for (int i = K; i < 4; i++) // ���������� ��������� ���������
        //    {
        //        for (int j = 0; j < 16; j++)
        //            reader.ReadLine();
        //    }
        //    for (int i = 0; i < 175; i++) // ������� 175 �����
        //        reader.ReadLine();
        //    string sr = reader.ReadLine(); // ����� �����������
        //    for (int i = 0; i < 4; i++) // ������� 4 �����
        //        reader.ReadLine();
        //    for (int i = 0; i < 200; i++)
        //    {
        //        for (int j = 0; j < 2; j++)
        //        {
        //            res[j].Views.Add(new View());
        //            res[j][i].Text = new string(' ', 16);
        //        }
        //    }
        //    string s = "";
        //    int maxTop = 0;   // ������������ ����� ���� ������ ������
        //    int maxBottom = 0; // ������������ ����� ���� ������ ������
        //    List<ControllerEmbeddedVar> evars = RelkonPultModel.GetEmbeddedVarsFromFpr(FileName);
        //    while ((s = reader.ReadLine()) != null)
        //    {
        //        Match m = null;
        //        while ((m = Regex.Match(s, @".*(.*)\:.*������� (\d)\..*��� (\d+)", RegexOptions.IgnoreCase)).Value == "")
        //            s = reader.ReadLine();
        //        char lineType = reader.ReadLine().ToLower()[1];
        //        for (int i = 0; i < 4; i++)
        //            reader.ReadLine();
        //        string text = reader.ReadLine(); // ����� ��������
        //        if (text.Length > 4)
        //            text = text.Substring(0, 4);
        //        text += new string(' ', 4 - text.Length);
        //        string varName = reader.ReadLine(); // ��� ����������               
        //        int seg = int.Parse(m.Groups[2].Value) - 1;
        //        int view = int.Parse(m.Groups[3].Value);
        //        List<View> CurrentList = res[0].Views;
        //        if (lineType == 'v')
        //        {
        //            if (maxBottom < view)
        //                maxBottom = view;
        //            CurrentList = res[1].Views;
        //        }
        //        else
        //        {
        //            if (maxTop < view)
        //                maxTop = view;
        //        }
        //        if (varName == "�����")
        //        {
        //            // � �������� ��� ����������
        //            CurrentList[view].Text = CurrentList[view].Text.Remove(seg * 4, 4).Insert(seg * 4, text);
        //            for (int i = 0; i < 5; i++)
        //                reader.ReadLine();
        //        }
        //        else
        //        {
        //            //� �������� ���� ����������
        //            Match m1 = Regex.Match(text, @"(\d+(?:[\.|,]\d+)?)[^\d]*\b");
        //            if (m1.Success)
        //            {
        //                CurrentList[view].Text = CurrentList[view].Text.Remove(seg * 4, 4).Insert(seg * 4, text);
        //                // ��������� ���������� ����������
        //                int position = seg * 4 + m1.Groups[1].Index;
        //                int length = m1.Groups[1].Value.Length;
        //                string mask = m1.Groups[1].Value;
        //                bool hasSign = reader.ReadLine() == "1";
        //                for (int i = 0; i < 3; i++)
        //                    reader.ReadLine();
        //                bool readOnly = reader.ReadLine() == "0";
        //                varName = varName.Substring(1);
        //                CurrentList[view].Vars.Add(new PultVar(varName, mask, position, hasSign, readOnly));
        //                s = "";
        //            }
        //            else
        //            {
        //                CurrentList[view].Text = CurrentList[view].Text.Remove(seg * 4, 4).Insert(seg * 4, text);
        //                for (int i = 0; i < 5; i++)
        //                    reader.ReadLine();
        //            }
        //        }
        //        RelkonPultModel.ConvertViewTextForEmbeddedVars(CurrentList[view], evars);
        //    }
        //    reader.Close();
        //    for (int i = maxTop + 1; i < 200; i++)
        //        res[0].Views.RemoveAt(maxTop + 1);
        //    for (int i = maxBottom + 1; i < 200; i++)
        //        res[1].Views.RemoveAt(maxBottom + 1);
        //    for (int i = 0; i < 2; i++)
        //    {
        //        if (res[i].Views.Count == 0)
        //            res[i].Views.Add(new View());
        //    }
        //    return res;
        //}
        /// <summary>
        /// ��������� ������ ������ �� ���������� ����� ������� ������ 37
        /// </summary>
        //private static RelkonPultModel GetPultModelFromPlt37(string FileName)
        //{
        //    // �������� ����������� ���������
        //    string fs = File.ReadAllText(FileName, Encoding.Default);
        //    fs = Regex.Replace(fs, "\\&#x[0-9A-F]+;", "");
        //    File.WriteAllText(FileName, fs, Encoding.Default);
        //    // ������� XML
        //    XPathDocument xpDoc = new XPathDocument(FileName);
        //    XPathNavigator xpNav = ((IXPathNavigable)xpDoc).CreateNavigator();
        //    XPathNodeIterator xpList = xpNav.Select("/fpultProject");
        //    xpList.MoveNext();
        //    string processor = xpList.Current.GetAttribute("processor", "");
        //    RelkonPultModel res = new RelkonPultModel(PultType.Pult2x16);
        //    if (processor == "Fujitsu")
        //        res = new RelkonPultModel(PultType.Pult4x20);
        //    foreach (Row r in res.rows)
        //        r.Views.Clear();
        //    xpList = xpNav.Select("/fpultProject/row");
        //    List<ControllerEmbeddedVar> evars = RelkonPultModel.GetEmbeddedVarsFromPlt37(FileName);
        //    while (xpList.MoveNext())
        //    {
        //        int rowIndex = int.Parse(xpList.Current.GetAttribute("index", ""));
        //        XPathNodeIterator xpViewList = xpList.Current.SelectChildren("view", "");
        //        while (xpViewList.MoveNext())
        //        {
        //            View view = new View();
        //            view.Text = xpViewList.Current.GetAttribute("value", "").Substring(4);
        //            XPathNodeIterator xpVarList = xpViewList.Current.SelectChildren("var", "");
        //            while (xpVarList.MoveNext())
        //            {
        //                PultVar v = new PultVar();
        //                v.Name = xpVarList.Current.GetAttribute("address", "");
        //                if (v.Name[0] == '_')
        //                    v.Name = v.Name.Substring(1);
        //                v.ReadOnly = bool.Parse(xpVarList.Current.GetAttribute("readOnly", ""));
        //                v.HasSign = bool.Parse(xpVarList.Current.GetAttribute("hasSign", ""));
        //                v.Mask = xpVarList.Current.GetAttribute("format", "");
        //                v.Position = int.Parse(xpVarList.Current.GetAttribute("offset", ""));
        //                view.Vars.Add(v);
        //            }
        //            RelkonPultModel.ConvertViewTextForEmbeddedVars(view, evars);
        //            res[rowIndex].Views.Add(view);
        //        }
        //    }
        //    foreach(Row r in res.rows)
        //    {
        //        if (r.Views.Count == 0)
        //            r.Views.Add(new View());
        //    }
        //    return res;
        //}
        /// <summary>
        /// ��������� ������ ������ �� ���������� ����� ������� ������ 5
        /// </summary>
        private static RelkonPultModel GetPultModelFromPlt5(string FileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RelkonPultModel));
            RelkonPultModel res = null;
            using (StreamReader reader = new StreamReader(FileName, Encoding.GetEncoding("UTF-16")))
            {
                res = (RelkonPultModel)serializer.Deserialize(reader);
            }
            return res;
        }
        /// <summary>
        /// ����������� ������ � ���������� ���� ������
        /// </summary>
        public void ChangePultType(PultType NewPultType)
        {
            if (!this.IsEmpty && (this.type.RowCount > NewPultType.RowCount || this.type.SymbolsInRow > NewPultType.SymbolsInRow))
                throw new Exception("���������� ������������ ����� � ���������� ����");
            if (this.IsEmpty)
            {
                this.rows.Clear();
                for (int i = 0; i < NewPultType.RowCount; i++)
                {
                    Row r = new Row();
                    r.Views.Add(new View());
                    this.rows.Add(r);
                }
                this.Type = NewPultType;
                return;
            }
            string s = new string(' ', NewPultType.SymbolsInRow - this.type.SymbolsInRow);
            foreach (Row row in this.rows)
            {
                foreach (View view in row.Views)
                {
                    view.Text += s;
                }
            }
            //for (int i = this.type.RowCount; i < NewPultType.RowCount; i++)
            //{
            //    Row r = new Row();
            //    r.Views.Add(new View());
            //    this.rows.Insert(i - this.type.RowCount, r);
            //}
            for (int i = 1; i < 3; i++)
            {
                Row r = new Row();
                r.Views.Add(new View());
                this.rows.Insert(i, r);
            }
          
            this.type = NewPultType;
        }
        /// <summary>
        /// ���������� ���������, � ������� ������ ����������� � ����
        /// </summary>
        public Encoding FileEncoding
        {
            get
            {
                return Encoding.GetEncoding("UTF-16");
            }
        }
        /// <summary>
        /// ��������� ������ � ���� � ��������� ������
        /// </summary>
        public void Save(string FileName)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using (StreamWriter writer = new StreamWriter(FileName, false, this.FileEncoding))
            {
                serializer.Serialize(writer, this);
            }
            this.fileName = FileName;
        }
        /// <summary>
        /// ����������������� ���������� ��������� ��������� �� ����� ������� ������ 3.3
        /// </summary>
        /// <param name="FileName">��� ����� �������</param>
        //public static List<ControllerEmbeddedVar> GetEmbeddedVarsFromFpr(string FileName)
        //{
        //    ControllerVarCollection vars = new ControllerVarCollection();
        //    using (StreamReader reader = new StreamReader(FileName, Encoding.Default))
        //    {
        //        for (int i = 0; i < 4; i++)
        //            reader.ReadLine();
        //        AT89C51ED2Solution sln = new AT89C51ED2Solution();
        //        vars.EmbeddedVars.AddRange(sln.GetEmbeddedVarsList());
        //        int K = reader.ReadLine() == "1" ? 0 : 3; // K52 - 16 ����������, K51ED2 - 64; "1" - K51ED2
        //        for (int i = K; i < 4; i++)
        //        {
        //            string prefix = ((char)(((byte)'W') + i)).ToString();
        //            for (int j = 0; j < 16; j++)
        //            {
        //                string s = reader.ReadLine();
        //                vars.GetEmbeddedVar(prefix + j).Value = (s.Contains("h") || s.Contains("H")) ? AppliedMath.HexToDec(s) : int.Parse(s);
        //            }
        //        }
        //    }
        //    return vars.EmbeddedVars;
        //}
        /// <summary>
        /// ������������� ����������� ��������� ������ �� ����� ������� ������ 3.3
        /// </summary>
        /// <param name="FileName">��� ����� �������</param>
        //public static List<ControllerEmbeddedVar> GetEmbeddedVarsFromPlt37(string FileName)
        //{
        //    ControllerVarCollection vars = new ControllerVarCollection();
        //    AT89C51ED2Solution sln = new AT89C51ED2Solution(); // ��������� ������ ��� ��������� ������ ���������� ��������� ���������; �.�. � MB90F347Solution, �.�. ��� ������ � ��� ���������
        //    vars.EmbeddedVars.AddRange(sln.GetEmbeddedVarsList()); //
        //    XPathDocument xpDoc = new XPathDocument(FileName);
        //    XPathNavigator xpNav = ((IXPathNavigable)xpDoc).CreateNavigator();
        //    XPathNodeIterator xpFactory = xpNav.Select("/fpultProject/factory");
        //    while (xpFactory.MoveNext())
        //    {
        //        int idx = int.Parse(xpFactory.Current.GetAttribute("index", ""));
        //        string prefix = ((char)(((byte)'W') + idx)).ToString();
        //        XPathNodeIterator xpValues = xpFactory.Current.SelectChildren("value", "");
        //        int i = 0;
        //        while (xpValues.MoveNext())
        //        {
        //            vars.GetEmbeddedVar(prefix + i++).Value = (xpValues.Current.Value.Contains("h") || xpValues.Current.Value.Contains("H")) ? AppliedMath.HexToDec(xpValues.Current.Value) : int.Parse(xpValues.Current.Value);
        //        }
        //    }
        //    xpFactory = xpNav.Select("/fpultProject/EEVars");
        //    while (xpFactory.MoveNext())
        //    {
        //        XPathNodeIterator xpValues = xpFactory.Current.SelectChildren("ee", "");
        //        int i = 0;
        //        while (xpValues.MoveNext())
        //        {
        //            vars.GetEmbeddedVar("EE" + i++).Value = (xpValues.Current.Value.Contains("h") || xpValues.Current.Value.Contains("H")) ? AppliedMath.HexToDec(xpValues.Current.Value) : int.Parse(xpValues.Current.Value);
        //        }
        //    }
        //    return vars.EmbeddedVars;
        //}
        /// <summary>
        /// �������� � ���� ����� ���������� ��������� ��������� �� ���������
        /// </summary>
        private static void ConvertViewTextForEmbeddedVars(View view, List<ControllerEmbeddedVar> EmbeddedVars)
        {
            ControllerVarCollection vars = new ControllerVarCollection();
            vars.EmbeddedVars.AddRange(EmbeddedVars);
            foreach (PultVar var in view.Vars)
            {
                ControllerEmbeddedVar evar = vars.GetEmbeddedVar(var.Name);
                if (evar != null)
                {
                    if (evar.Value.ToString().Length > var.Mask.Replace(".", "").Replace(",", "").Length)
                        return;
                    string evalue = evar.Value.ToString();
                    string mask = "";
                    for (int i = 0, j = 0; i < var.Mask.Length; i++, j++)
                    {
                        if (var.Mask[var.Mask.Length - i - 1] == '.' || var.Mask[var.Mask.Length - i - 1] == ',')
                        {
                            mask = var.Mask[var.Mask.Length - i - 1] + mask;
                            j--;
                        }
                        else
                            mask = ((evalue.Length - j - 1 >= 0) ? evalue[evalue.Length - j - 1] : '0') + mask;
                    }
                    mask = Utils.AddChars('0', mask, var.Mask.Length);
                    var.Mask = mask;
                    view.Text = view.Text.Remove(var.Position, var.Mask.Length).Insert(var.Position, var.Mask);
                }
            }
        }
        /// <summary>
        /// �������� �� ���� ����� ����� ��������� ��������� ��������� �� ����������
        /// </summary>
        public void ConvertViewsTextsForEmbeddedVars(List<ControllerEmbeddedVar> EmbeddedVars)
        {
            foreach (Row row in this.rows)
            {
                foreach (View view in row.Views)
                {
                    RelkonPultModel.ConvertViewTextForEmbeddedVars(view, EmbeddedVars);
                }
            }
        }
        /// <summary>
        /// ��������� ������ ������ � ���� ������� Relkon 4.2
        /// </summary>
    //    public void SaveToRelkon42Format(string FileName, ControllerProgramSolution solution)
    //    {
    //        XmlTextWriter xmlDoc = new XmlTextWriter(FileName, Encoding.Default);
    //        xmlDoc.Formatting = Formatting.Indented;
    //        xmlDoc.WriteStartDocument();
    //        xmlDoc.WriteStartElement("fpultProject");

    //        string s = (solution is AT89C51ED2Solution) ? "K51ED2" : "Fujitsu";
    //        xmlDoc.WriteAttributeString("processor", s);
    //        xmlDoc.WriteAttributeString("number", solution.ControllerAddress.ToString());
    //        xmlDoc.WriteAttributeString("numRows", this.type.RowCount.ToString());
    //        xmlDoc.WriteAttributeString("numCols", this.type.SymbolsInRow.ToString());
    //        xmlDoc.WriteAttributeString("protocol", (solution is AT89C51ED2Solution) ? ((AT89C51ED2Solution)solution).Protocol.ToString() : "RC51BIN");
    //        xmlDoc.WriteAttributeString("readPass", (solution is AT89C51ED2Solution) ? ((AT89C51ED2Solution)solution).ReadPassword : "");
    //        xmlDoc.WriteAttributeString("speed", (solution is AT89C51ED2Solution) ? ((AT89C51ED2Solution)solution).BaudRate.ToString() : "19200");
    //        xmlDoc.WriteAttributeString("writePass", (solution is AT89C51ED2Solution) ? ((AT89C51ED2Solution)solution).WritePassword : "");
    //        xmlDoc.WriteAttributeString("projectName", solution.Name);

    //        for (int i = 0; i < this.rows.Count; i++)
    //        {
    //            xmlDoc.WriteStartElement("row");
    //            xmlDoc.WriteAttributeString("index", i.ToString());
    //            for (int j = 0; j < this.rows[i].Views.Count; j++)
    //            {
    //                View view = this.rows[i].Views[j];
    //                xmlDoc.WriteStartElement("view");
    //                xmlDoc.WriteAttributeString("index", j.ToString());
    //                string ss = j.ToString() + ".";
    //                ss += new string(' ', 4 - ss.Length);
    //                xmlDoc.WriteAttributeString("value", ss + view.Text);
    //                if (view.Vars.Count > 0)
    //                {
    //                    foreach (PultVar v in view.Vars)
    //                    {
    //                        xmlDoc.WriteStartElement("var");
    //                        xmlDoc.WriteAttributeString("address", v.Name);
    //                        xmlDoc.WriteAttributeString("hasSign", v.HasSign.ToString());
    //                        xmlDoc.WriteAttributeString("format", v.Mask);
    //                        xmlDoc.WriteAttributeString("offset", v.Position.ToString());
    //                        xmlDoc.WriteAttributeString("readOnly", v.ReadOnly.ToString());
    //                        xmlDoc.WriteEndElement();
    //                    }
    //                }
    //                xmlDoc.WriteEndElement();
    //            }
    //            xmlDoc.WriteEndElement();
    //        }
    //        for (int i = 0; i < 4; i++)
    //        {
    //            char c = (char)((int)'W' + i);
    //            xmlDoc.WriteStartElement("factory");
    //            xmlDoc.WriteAttributeString("index", i.ToString());
    //            for (int j = 0; j < 16; j++)
    //            {
    //                xmlDoc.WriteStartElement("value");
    //                ControllerEmbeddedVar var = solution.Vars.GetEmbeddedVar(c.ToString() + j);
    //                if (var == null)
    //                    var = solution.Vars.GetEmbeddedVar(c.ToString() + j);
    //                xmlDoc.WriteString(var.Value.ToString());
    //                xmlDoc.WriteEndElement();
    //            }
    //            xmlDoc.WriteEndElement();
    //        }
    //        xmlDoc.WriteStartElement("EEVars");
    //        for (int i = 0; i < 64; i++)
    //        {
    //            ControllerEmbeddedVar var = solution.Vars.GetEmbeddedVar("EE" + i);
    //            if (var == null)
    //                var = solution.Vars.GetEmbeddedVar("EE" + i);
    //            xmlDoc.WriteElementString("ee", var.Value.ToString());
    //        }
    //        xmlDoc.WriteEndElement();
    //        xmlDoc.WriteEndDocument();
    //        xmlDoc.Flush();
    //        xmlDoc.Close();
    //    }

    #region IDisposable Members

    public void Dispose()
    {
        this.rows.Clear();
    }

    #endregion
    }
    
    public class Row
    {
        private List<View> views; //������ ����� ������

        public Row()
        {
            this.views = new List<View>();
        }
        /// <summary>
        /// ���������� ��� ������ �� ���������� ������
        /// </summary>
        public View this[int index]
        {
            get
            {
                return this.views[index];
            }
        }
        /// <summary>
        /// ���������� ������ ����� ������
        /// </summary>
        public List<View> Views
        {
            get
            {
                return this.views;
            }
        }
        /// <summary>
        /// ���������� ������ �������� ����� ������
        /// </summary>
        [XmlIgnore]
        public List<View> EnabledViews
        {
            get
            {
                List<View> res = new List<View>();
                foreach (View view in this.views)
                {
                    if (view.Enabled)
                        res.Add(view);
                }
                return res;
            }
        }
    }

    [Serializable]
    public class View
    {
        private string text; // ����� ������ ����
        private bool enabled; // ����������, ������� �� ��� 
        private List<PultVar> vars = new List<PultVar>(); // ���������� ����

        public View()
        {
            this.text = "";
            this.enabled = true;
            this.vars = new List<PultVar>();
        }
        /// <summary>
        /// ���������� ��� ������������� ����� ������ ����
        /// </summary>
        [XmlAttribute]
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
        /// <summary>
        /// ���������� ��� ������������� ���� ���������� ����
        /// </summary>
        [XmlAttribute]
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
            }
        }
        /// <summary>
        /// ���������� �������� ���������� �������
        /// </summary>
        public List<PultVar> Vars
        {
            get
            {
                return this.vars;
            }
        }
        /// <summary>
        /// ���������� ������ ����� �������
        /// </summary>
        public View Copy()
        {
            View res = new View();
            res.text = this.text;
            res.enabled = this.enabled;
            foreach (PultVar var in this.vars)
            {
                res.vars.Add(var.Copy());
            }
            return res;
        }
        /// <summary>
        /// ���������, ���������� �� � ��������� ������� ������ ���������� � 
        /// ���� ���������, �� ���������� �� � ��������� var
        /// </summary>
        public bool ContainsVar(int position, ref PultVar var)
        {
            foreach (PultVar v in this.vars)
            {
                if (v.Position <= position && v.Position + v.Mask.Length > position)
                {
                    var = v;
                    return true;
                }
            }
            return false;
        }

        private int CompareVars(PultVar v1, PultVar v2)
        {
            return (v1.Position - v2.Position);
        }
        /// <summary>
        /// ��������� ���������� �� ����������� �� �� �������� � ����
        /// </summary>
        public void SortVars()
        {
            Comparison<PultVar> comparer = new Comparison<PultVar>(this.CompareVars);
            this.vars.Sort(comparer);
        }
    }

    [Serializable]
    public class PultVar: ILCDIndicatorRowObject
    {
        private string mask = ""; // ����� ������ ��������� �� ������� ������
        private int position = 0; // ������� � ������, � ������� ���������� ����� �������� ���������� 

        /// <summary>
        /// ���������� ��� ������������� ������� � ������, � ������� ���������� ����� �������
        /// </summary>
        [XmlAttribute]
        public int Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }
        /// <summary>
        /// ���������� ��� ������������� ����� ������ ������� �� ������� ������
        /// </summary>
        [XmlAttribute]
        public string Mask
        {
            get
            {
                return this.mask;
            }
            set
            {
                this.mask = value;
            }
        }
        /// <summary>
        /// ��� ����������; ������ ��������� � ������ �����-���� ���������� ���������
        /// </summary>
        [XmlAttribute]
        public string Name = "";
        /// <summary>
        /// ���� true, �� ���������� ��������� �� ������
        /// </summary>
        [XmlAttribute]
        public bool HasSign = false;
        /// <summary>
        /// ���� true, �� ���������� ������ ������������� ����� �����
        /// </summary>
        [XmlAttribute]
        public bool ReadOnly = false;

        public PultVar()
        {
            return;
        }

        public PultVar(string Name, string Mask, int Position, bool HasSign, bool ReadOnly)
        {
            this.Name = Name;
            this.mask = Mask;
            this.position = Position;
            this.HasSign = HasSign;
            this.ReadOnly = ReadOnly;
        }
        /// <summary>
        /// ���������� ������ ����� �������
        /// </summary>
        public PultVar Copy()
        {
            return new PultVar(this.Name, this.mask, this.position, this.HasSign, this.ReadOnly);
        }
        /// <summary>
        /// ���������� 2 ���������� �� �� �������� ������
        /// </summary>
        /// <returns>��������� ���������</returns>
        public static int CompareByPosition(PultVar x, PultVar y)
        {
            return x.Position - y.Position;
        }

    }
}
