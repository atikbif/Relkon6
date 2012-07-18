using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.Solutions;
using Kontel.Classes.IO.Ports;
using System.Xml.Serialization;
using System.IO;

namespace Kontel.Relkon
{
    public class DebuggerParameters
    {
        /// <summary>
        /// Описывает параметры опашиваемой переменной
        /// </summary>
        public class VarDescription
        {
            // Имя переменной
            [XmlAttribute]
            public string Name;
            // Оласть памяти, в которой располагается переменная
            [XmlAttribute]
            public MemoryType MemoryType;
            // Размер переменной
            [XmlAttribute]
            public int Size;
            // Адрес переменной
            [XmlAttribute]
            public int Address;
        }
        /// <summary>
        /// Описывает параметры опрашиваемого датчика ввода-вывода
        /// </summary>
        public class SensorDescription : VarDescription
        {
            [XmlAttribute]
            public string Caption;
            [XmlAttribute]
            public int ByteNumber;
            [XmlAttribute]
            public int BitNumber;
        }

        /// <summary>
        /// Тип процессора опрашиваемого контроллера
        /// </summary>
        [XmlAttribute]
        public ProcessorType ProcessorType = ProcessorType.AT89C51ED2;
        /// <summary>
        /// Тип протокола, по которому работает опрашиваемый контроллер
        /// </summary>
        [XmlAttribute]
        public ProtocolType ProtocolType = ProtocolType.RC51BIN;
        /// <summary>
        /// Сетевой адрес опрашиваемого контроллера
        /// </summary>
        [XmlAttribute]
        public int ControllerNumber = 1;
        /// <summary>
        /// Имя COM-порта, по которому опрашивается контроллер
        /// </summary>
        [XmlAttribute]
        public string PortName = "COM1";
        /// <summary>
        /// Скорость, на которой опрашивается контроллер
        /// </summary>
        [XmlAttribute]
        public int BaudRate = 19200;
        /// <summary>
        /// Флаг, определяющий использование обратного порядкак байт в контроллере (младший-старший)
        /// </summary>
        [XmlAttribute]
        public bool InverseByteOrder = true;//Обратный порядок следования байт
        /// <summary>
        /// Список опрашиваемых цифровых входных датчиков
        /// </summary>
        public List<SensorDescription> DINSensors = new List<Sensor>();
        /// <summary>
        /// Список опрашиваемых цифровых выходных датчиков
        /// </summary>
        public List<SensorDescription> DOUTSensors = new List<Sensor>();
        /// <summary>
        /// Список опрашиваемых аналоговых входных датчиков
        /// </summary>
        public List<SensorDescription> ADCSensors = new List<Sensor>();
        /// <summary>
        /// Список опрашиваемых аналоговых выходных датчиков
        /// </summary>
        public List<SensorDescription> DACSensors = new List<Sensor>();
        /// <summary>
        /// Показывает, что для аналоговых датчиков нужно отображать только один байт
        /// </summary>
        public bool DisplayOneByte = false;
        /// <summary>
        /// Тип опрашиваемой области памяти
        /// </summary>
        public MemoryType ViewMemoryType = MemoryType.Clock;
        /// <summary>
        /// Адрес опрашиваемой области паяти
        /// </summary>
        public int ViewMemoryAddress = 0;
        /// <summary>
        /// Размер опрашиваемой области памяти
        /// </summary>
        public int ReadingSize = 8;
        /// <summary>
        /// Список опрашиваемых переменных
        /// </summary>
        public List<VarDescription> ReadingVars = new List<VarDescription>();
        /// <summary>
        /// Список переменных, для которых строятся графики
        /// </summary>
        public List<SensorDescription> ChartVars = new List<VarDescription>();
        /// <summary>
        /// Интервал отображения данных на графике (мин.)
        /// </summary>
        public Int32 ViewGraphicsDisplayInterval = 10;

        /// <summary>
        /// Сохраняет настройки в файл с указанным именем
        /// </summary>
        public void Save(string FileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DebuggerParameters));
            using (StreamWriter writer = new StreamWriter(FileName, false, Encoding.GetEncoding("UTF-16")))
            {
                serializer.Serialize(writer, this);
            }
        }
        /// <summary>
        /// Возращает настройки оталдчика из указанного файла
        /// </summary>
        public static DebuggerParameters FromFile(string FileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DebuggerParameters));
            DebuggerParameters res = null;
            using (StreamReader reader = new StreamReader(FileName, Encoding.GetEncoding("UTF-16")))
            {
                res = (DebuggerParameters)serializer.Deserialize(reader);
            }
            return res;
        }

    }
}
