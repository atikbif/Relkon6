using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.Solutions;
using Kontel;
using System.Xml.Serialization;
using System.IO;

namespace Kontel.Relkon
{
    public sealed class DebuggerParameters
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
            // Флаг, показывающий, имеет ли переменная знак
            [XmlAttribute]
            public bool HasSign;
            [XmlAttribute]
            public bool Real;
            // Тип переменной
            [XmlAttribute]
            public int Type;
            //значение датчика в момент сохраниня файла(после последнего прочтения, в том виде в котором пришло)
            [XmlAttribute]
            public byte[] Value;
        }

        /// <summary>
        /// Описывает параметры опашиваемой структуры
        /// </summary>
        public class StructDescription : VarDescription
        {
            // Список переменных структуры
            public List<VarDescription> Vars = new List<VarDescription>();
        }

        /// <summary>
        /// Описывает параметры опрашиваемого аналогого датчика ввода-вывода
        /// </summary>
        public class AnalogSensorDescription : VarDescription
        {
            [XmlAttribute]
            public string Caption;
            // Показывает, что для аналоговых датчиков нужно отображать только один байт
            public bool DisplayOneByte = false;
        }
       
        /// <summary>
        /// Описывает параметры опрашиваемого цифрового датчика ввода-вывода
        /// </summary>
        public class DigitalSensorDescription : VarDescription
        {
            [XmlAttribute]
            public int BitNumber;//количество
            public List<SensorLabels> Labels = new List<SensorLabels>();
        }

        /// <summary>
        /// Метки цифрового датчика ввода-вывода
        /// </summary>
        public class SensorLabels
        {
            [XmlAttribute]
            public int Number;
            [XmlAttribute]
            public string Caption;
        }
        /// <summary>
        /// Блок опроса модулей
        /// </summary>
        public class Block
        {
            [XmlAttribute]
            public string Caption;
            [XmlAttribute]
            public int Number;
            [XmlAttribute]
            public int FirstSpliter;//Положение диелителя между цифровыми и аналоговыми датчиками 
            [XmlAttribute]
            public int SecondSpliter;//положение длелителя междц аналоговыми входами и выходами
            public List<string> Vars = new List<string>();
        }

        [XmlAttribute]
        public string SolutionPath = "";

        /// <summary>
        /// Тип процессора опрашиваемого контроллера
        /// </summary>
        [XmlAttribute]
        public ProcessorType ProcessorType = ProcessorType.STM32F107;
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
        /// показывает, что контроллер подключен к COM порту
        /// </summary>
        [XmlAttribute]
        public bool ComConection = true;
        /// <summary>
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
        /// Номер порта Ethernet
        /// </summary>
        [XmlAttribute]
        public int PortNumber = 101;
        /// <summary>
        /// IP адрес Ethernet
        /// </summary>
        [XmlAttribute]
        public string PortIP = "192.168.100.201";
        /// <summary>
        /// Протокол передачи данных по сети Ethernet
        /// </summary>
        [XmlAttribute]
        //public System.Net.Sockets.ProtocolType InterfaceProtocol = System.Net.Sockets.ProtocolType.Tcp;
        public string InterfaceProtocol = "Tcp";
        /// <summary>
        /// Флаг, определяющий использование обратного порядкак байт в контроллере (младший-старший)
        /// </summary>
        [XmlAttribute]
        public bool InverseByteOrder = true;//Обратный порядок следования байт
        /// <summary>
        /// Пароль на чтение
        /// </summary>
        [XmlAttribute]
        public string ReadPassword = "";
        /// <summary>
        /// Пароль на запись
        /// </summary>
        [XmlAttribute]
        public string WritePassword = "";
        /// <summary>
        /// Список опрашиваемых цифровых входных датчиков
        /// </summary>
        public List<DigitalSensorDescription> DINSensors = new List<DigitalSensorDescription>();
        /// <summary>
        /// Список опрашиваемых цифровых выходных датчиков
        /// </summary>
        public List<DigitalSensorDescription> DOUTSensors = new List<DigitalSensorDescription>();
        /// <summary>
        /// Список опрашиваемых аналоговых входных датчиков
        /// </summary>
        public List<AnalogSensorDescription> ADCSensors = new List<AnalogSensorDescription>();
        /// <summary>
        /// Список опрашиваемых аналоговых выходных датчиков
        /// </summary>
        public List<AnalogSensorDescription> DACSensors = new List<AnalogSensorDescription>();
        /// <summary>
        /// Показывает, что для аналоговых датчиков нужно отображать только один байт
        /// </summary>
        public List<Block> ModulBlocks = new List<Block>();
        /// <summary>
        /// Показывает надо ли отображать встроенные датчики
        /// </summary>
        public bool DisplayDefault = true;
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
        /// Список опрашиваемых структур
        /// </summary>
        public List<StructDescription> ReadingStructs = new List<StructDescription>();
        /// <summary>
        /// Список переменных, для которых строятся графики
        /// </summary>
        public List<VarDescription> ChartVars = new List<VarDescription>();
        /// <summary>
        /// Интервал отображения данных на графике (c.)
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
