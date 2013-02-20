using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon;
using System.Xml.Serialization;
using Kontel;

namespace Kontel.Relkon.Classes
{
    /// <summary>
    /// ќписывает произвольную переменную контроллера
    /// </summary>
    [XmlInclude(typeof(ControllerSystemVar)),
     XmlInclude(typeof(ControllerIOVar)),
     XmlInclude(typeof(ControllerEmbeddedVar)),
     XmlInclude(typeof(ControllerUserVar)),    
     XmlInclude(typeof(ControllerStructVar))]        
    public abstract class ControllerVar
    {
        /// <summary>
        /// »м€ переменной
        /// </summary>
        [XmlAttribute]
        public string Name = "";
        /// <summary>
        /// јдрес, по кототрому располагаетс€ переменна€
        /// </summary>
        [XmlAttribute]
        public int Address = -1;
        /// <summary>
        /// –азмер переменной в байтах (по-умолчанию - 1)
        /// </summary>
        [XmlAttribute]
        public int Size = 1;
        /// <summary>
        /// ќбласть пам€ти, в которой располагаетс€ переменна€,
        /// (по-умолчанию - RAM)
        /// </summary>
        [XmlAttribute]
        public MemoryType Memory = MemoryType.RAM;
        /// <summary>
        /// ѕоказывает, имеет ли переменна€ знак (по-умолчанию - false)
        /// </summary>
        [XmlAttribute]
        public bool HasSign = false;     
        [XmlAttribute]
        public bool Real = false;

        public ControllerVar()
        {

        }

        /// <summary>
        /// ¬озвращает адрес переменной в шестадцвтеричном формате
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
