using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kontel.Relkon.Classes
{
    /// <summary>
    /// Описывает пользовательскую переменную контроллера (определенную пользователем в 
    /// тексте программы)
    /// </summary>
    public class ControllerUserVar : ControllerVar
    {
        /// <summary>
        /// Показывает, является ли переменная массивом (по-умолчанию - false)
        /// </summary>
        [XmlAttribute]
        public bool Array = false;
    }
}
