using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.Solutions
{
    /// <summary>
    /// �������� ��������� ������� ��� 
    /// </summary>
    public sealed class PultParams
    {
        private int maxVarMaskLength = 0; // ����������� ��������� ������ ����� ���������� ������
        private int maxVarMaskDigitsCountAfterComma; // ����������� ��������� ����� ���� � ����� ���������� ����� �����
        private PultType defaultPultType; // ��� ������ ��-���������
        private PultType[] availablePultTypes; // ��������� ��� ������� ���� ������

        /// <summary>
        /// ��������� ����������� ��������� ������ ����� ���������� ������
        /// </summary>
        public int MaxVarMaskLength
        {
            get
            {
                return this.maxVarMaskLength;
            }
            internal set
            {
                this.maxVarMaskLength = value;
            }
        }
        /// <summary>
        /// ��������� ����������� ��������� ����� ���� � ����� ���������� ����� �����
        /// </summary>
        public int MaxVarMaskDigitsCountAfterComma
        {
            get
            {
                return this.maxVarMaskDigitsCountAfterComma;
            }
            internal set
            {
                this.maxVarMaskDigitsCountAfterComma = value;
            }
        }
        /// <summary>
        /// ���������� ��� ������ ��-���������
        /// </summary>
        public PultType DefaultPultType
        {
            get
            {
                return this.defaultPultType;
            }
            internal set
            {
                this.defaultPultType = value;
            }
        }
        /// <summary>
        /// ���������� ������ ��������� ��� ������� ����� �������
        /// </summary>
        public PultType[] AvailablePultTypes
        {
            get
            {
                return this.availablePultTypes;
            }
            internal set
            {
                this.availablePultTypes = value;
            }
        }
        /// <summary>
        /// ���������, �������� �� ��������� ��� ������ ����������
        /// ��� �������� ����������
        /// </summary>
        public bool PultTypeAllowed(PultType type)
        {
            bool res = false;
            for (int i = 0; i < this.AvailablePultTypes.Length; i++)
            {
                if (this.AvailablePultTypes[i].Equals(type))
                    res = true;
            }
            return res;
        }
    }
}
