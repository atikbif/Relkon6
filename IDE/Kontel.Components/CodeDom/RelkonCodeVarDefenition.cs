using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.CodeDom
{
    /// <summary>
    /// ������������ ����������� ���������� ���������
    /// </summary>
    public class RelkonCodeVarDefenition: RelkonCodeObject
    {
        protected string name; // ��� ����������
        protected string type; // ��� ����������
        private bool far; // ���� true, �� ���������� ��������� ����� far
        protected bool hasSign; // ����� �� ���������� ����        

        /// <summary>
        /// ���������� ��� ����������
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        /// <summary>
        /// ���������� ��� ����������
        /// </summary>
        public string Type
        {
            get
            {
                return this.type;
            }
        }
        /// <summary>
        /// ���� true, �� ���������� ��������� ����� far
        /// </summary>
        public bool Far
        {
            get
            {
                return this.far;
            }
        }

        /// <summary>
        /// ����������, ����� �� ���������� ����
        /// </summary>
        public bool HasSign
        {
            get
            {
                return this.hasSign;
            }
        }

        public RelkonCodeVarDefenition(int LineNumber, string Code, string Name, string Type, bool HasSign, bool Far): base(LineNumber, Code)
        {
            this.name = Name;
            this.type = Type;
            this.far = Far;
            this.hasSign = HasSign;
        }
    }
}
