using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kontel.Relkon.Classes
{
    /// <summary>
    /// Описывает системную пременную контроллера
    /// </summary>
    public sealed class ControllerSystemVar : ControllerVar
    {
        /// <summary>
        /// Системное имя переменной (под этм именем она будет видна в Flash.map)
        /// </summary>
        [XmlAttribute]
        public string SystemName = "";
    }
}
