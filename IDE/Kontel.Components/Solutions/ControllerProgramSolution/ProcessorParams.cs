using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon.Solutions
{
    /// <summary>
    /// ������� ��������� ��������� ������� �����������
    /// </summary>
    public sealed class ProcessorParams
    {
        private string sdkDirectory = ""; // ������� SDK ���������� �������
        private ProcessorType type = ProcessorType.AT89C51ED2; // ��� ���������� �������
        private bool inverseByteOrder = false; // ������� ������������� ����� � ������ ����������

        /// <summary>
        /// ���������� ��� ������������� ������� SDK ���������� �������
        /// </summary>
        public string SDKDirectory
        {
            get
            {
                return this.sdkDirectory;
            }
            set
            {
                this.sdkDirectory = value;
            }
        }
        /// <summary>
        /// ���������� ��� ���������� �������
        /// </summary>
        public ProcessorType Type
        {
            get
            {
                return this.type;
            }
            internal set
            {
                this.type = value;
            }
        }
        /// <summary>
        /// ���������� ������� ���������� ���� � ������ ���������� (������ - ������� ���� 
        /// ������� ����, � �� ��� �������, ��� �������� - ������� �������, � �� ��� - �������)
        /// </summary>
        public bool InverseByteOrder
        {
            get
            {
                return this.inverseByteOrder;
            }
            internal set
            {
                this.inverseByteOrder = value;
            }
        }
    }
}
