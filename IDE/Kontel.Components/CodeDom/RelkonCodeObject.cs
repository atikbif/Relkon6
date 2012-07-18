using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.CodeDom
{
    public class RelkonCodeObject
    {
        private int lineNumber; // ����� ������ � ������ ���������, � ������� ���������� �������
        private string code; // ��� ���������, ����������� �������
        /// <summary>
        /// ���������� ����� ������ � ������ ���������, � ������� ���������� �������
        /// </summary>
        public int LineNumber
        {
            get
            {
                return this.lineNumber;
            }
        }
        /// <summary>
        /// ��� ���������, ����������� �������
        /// </summary>
        public string Code
        {
            get
            {
                return this.code;
            }
        }
        /// <summary>
        /// ������� ����� �������� ������
        /// </summary>
        /// <param name="LineNumber">����� ����� � ���������, � ������� ���������� �������</param>
        /// <param name="Code">��� ���������, ����������� �������</param>
        public RelkonCodeObject(int LineNumber, string Code)
        {
            this.code = Code;
            this.lineNumber = LineNumber;
        }
    }
}
