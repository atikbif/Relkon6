using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Kontel.Relkon.CodeDom
{
    public sealed class RelkonCodeProcess : RelkonCodeObject
    {
        private int index; // ����� ����������
        private List<RelkonCodeSituation> situations = new List<RelkonCodeSituation>(); // ������ �������� ��������
        private bool hasDelay; // ����������, �������� �� ������� �������� � �������� ��������
        private string otherCode; // ��������� ��� ��������(������ ��������)

        /// <summary>
        /// ���������� ��������� ��� �������� (������ ��������)
        /// </summary>
        public string OtherCode
        {
            get
            {
                return this.otherCode;
            }
        }
        /// <summary>
        /// ���������� ����� ��������
        /// </summary>
        public int Index
        {
            get
            {
                return this.index;
            }
        }
        /// <summary>
        /// ����������, �������� �� ������� �������� � �������� ��������
        /// </summary>
        public bool HasDelay
        {
            get
            {
                return this.hasDelay;
            }
        }
        /// <summary>
        /// ���������� ������ �������� ��������
        /// </summary>
        public List<RelkonCodeSituation> Situations
        {
            get
            {
                return this.situations;
            }
        }
        /// <summary>
        /// ���������� ������ �������� ���� �������� ��������
        /// </summary>
        public List<int> Periods
        {
            get
            {
                List<int> res = new List<int>();
                foreach (RelkonCodeSituation situation in this.situations)
                {
                    if (!res.Contains(situation.Period))
                        res.Add(situation.Period);
                }
                return res;
            }
        }
        /// <summary>
        /// ������� ������ �������� ��������
        /// </summary>
        private void CreateSituationsList()
        {
            string remainder = this.Code;
            MatchCollection mc1 = Regex.Matches(this.Code, @"/\*(\d+)\*/\s*#SIT(\d+)(?:\((\d+\.\d+)\))?\s+");
            int k = 0;
            for (int i = 0; i < mc1.Count; i++)
            {
                int SitNumber = int.Parse(mc1[i].Groups[2].Value);
                int SitLine = int.Parse(mc1[i].Groups[1].Value);
                int SitPeriod = 100;
                if (mc1[i].Groups[3].Value != "")
                    SitPeriod = (int)(1000 * double.Parse(mc1[i].Groups[3].Value, NumberFormatInfo.InvariantInfo));
                string SitCode = this.Code.Substring(mc1[i].Index + mc1[i].Length);
                if (i != mc1.Count - 1)
                     SitCode = this.Code.Substring(mc1[i].Index + mc1[i].Length, mc1[i + 1].Index - mc1[i].Index - mc1[i].Length);
                if (Regex.IsMatch(SitCode, @"#/\d+\.\d+"))
                    this.hasDelay = true;
                this.situations.Add(new RelkonCodeSituation(SitLine, SitCode, SitNumber, SitPeriod));
                remainder = remainder.Remove(mc1[i].Index - k, mc1[i].Value.Length + SitCode.Length);
                k += mc1[i].Value.Length + SitCode.Length;
            }
            this.otherCode = remainder;
        }

        public RelkonCodeProcess(int LineNumber, string Code, int Index)
            : base(LineNumber, Code)
        {
            this.index = Index;
            this.CreateSituationsList();
        }
        /// <summary>
        /// ���������, ���������� �� � �������� �������� � ��������� ��������
        /// </summary>
        public RelkonCodeSituation GetSituationByIndex(int index)
        {
            foreach (RelkonCodeSituation situation in this.situations)
            {
                if (situation.Index == index)
                    return situation;
            }
            return null;
        }
        /// <summary>
        /// ���������, ���������� �� � ������� �������� � ��������� ��������
        /// </summary>
        public bool ContainsPeriod(int Period)
        {
            foreach (RelkonCodeSituation situation in this.Situations)
            {
                if (situation.Period == Period)
                    return true;
            }
            return false;
        }
    }
}
