using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kontel.Relkon.Classes
{
    /// <summary>
    /// Описывает переменные, соответствующие датчикам ввода-вывода
    /// (в т.ч. подключаемым модулям ВВ)
    /// </summary>
    public sealed class ControllerIOVar : ControllerVar
    {
        [XmlAttribute]
        public string SystemName = "";
        /// <summary>
        /// Показывает, является ли переменная встроенной в контроллер или
        /// принадлежит внещ\шнему модулю ввода-вывода (по-умолчанию - false)
        /// </summary>
        [XmlAttribute]
        public bool ExternalModule = false;
    }
}
