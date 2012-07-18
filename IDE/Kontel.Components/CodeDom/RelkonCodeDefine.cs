using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.CodeDom
{
    public sealed  class RelkonCodeDefine: RelkonCodeObject
    {
        private string name; // ��� ������������� #define ���������
        private string value; // �������� ������������� #define ���������

        /// <summary>
        /// ���������� ��� ������������� ����� #define ���������
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        /// <summary>
        /// ���������� �������� ������������� ����� #define ���������
        /// </summary>
        public string Value
        {
            get
            {
                return this.value;
            }
        }

        public RelkonCodeDefine(int LineNumber, string Code, string Name, string Value)
            : base(LineNumber, Code)
        {
            this.name = Name;
            this.value = Value;
        }
    }
}
