using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.CodeDom
{
    public sealed class RelkonCodeSituation : RelkonCodeObject
    {
        private int index; // ����� ��������
        private int period; // ������ �������� � ��

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
        /// ���������� ������ �������� � ��
        /// </summary>
        public int Period
        {
            get
            {
                return this.period;
            }
        }

        public RelkonCodeSituation(int LineNumber, string Code, int Index, int Period)
            : base(LineNumber, Code)
        {
            this.index = Index;
            this.period = Period;
        }
    }
}
