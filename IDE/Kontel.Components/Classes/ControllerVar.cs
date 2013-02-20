using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon;
using System.Xml.Serialization;
using Kontel;

namespace Kontel.Relkon.Classes
{
    /// <summary>
    /// ��������� ������������ ���������� �����������
    /// </summary>
    [XmlInclude(typeof(ControllerSystemVar)),
     XmlInclude(typeof(ControllerIOVar)),
     XmlInclude(typeof(ControllerEmbeddedVar)),
     XmlInclude(typeof(ControllerUserVar)),    
     XmlInclude(typeof(ControllerStructVar))]        
    public abstract class ControllerVar
    {
        /// <summary>
        /// ��� ����������
        /// </summary>
        [XmlAttribute]
        public string Name = "";
        /// <summary>
        /// �����, �� ��������� ������������� ����������
        /// </summary>
        [XmlAttribute]
        public int Address = -1;
        /// <summary>
        /// ������ ���������� � ������ (��-��������� - 1)
        /// </summary>
        [XmlAttribute]
        public int Size = 1;
        /// <summary>
        /// ������� ������, � ������� ������������� ����������,
        /// (��-��������� - RAM)
        /// </summary>
        [XmlAttribute]
        public MemoryType Memory = MemoryType.RAM;
        /// <summary>
        /// ����������, ����� �� ���������� ���� (��-��������� - false)
        /// </summary>
        [XmlAttribute]
        public bool HasSign = false;     
        [XmlAttribute]
        public bool Real = false;

        public ControllerVar()
        {

        }

        /// <summary>
        /// ���������� ����� ���������� � ���������������� �������
        /// </summary>
        /// <returns></returns>
        public string GetAddressAsHexString()
        {
            return Utils.AddChars('0', Convert.ToString(this.Address, 16), 4);
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
