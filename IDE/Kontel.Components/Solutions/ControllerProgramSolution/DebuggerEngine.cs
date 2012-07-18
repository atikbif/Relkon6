using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Classes.IO.Ports;
using Kontel.Components.Forms;
using Kontel.Relkon.Solutions;
using Kontel.Classes;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;

namespace Kontel.Relkon.Debugger
{
    public class DebuggerEngine
    {
        #region Fields
        private Relkon37SerialPort _serialPort = null; // порт, через который осущетвляется опрос контроллера
        private Queue<DebuggerPrimitive.QueueElement> _Queue = new Queue<DebuggerPrimitive.QueueElement>(); // очередь опроса контроллера
        private List<DebuggerPrimitive.QueueElement> _AddElementInQueue = new List<DebuggerPrimitive.QueueElement>(); // список элементов на добавлени в очередь
        private List<Guid> _AddElementOutQueue = new List<Guid>(); // список элементов на удаление из очереди
        private int _BufferSize = 64; // размер буфера для чтения / записи данных контроллера
        private int _CountElementInQueue = 0; // текущее количество объектов в очереди опроса контроллера
        private AsyncOperation asyncOp = null; // управляет асинхронными операциями отладчика
        private SendOrPostCallback exReadPartDelegate = null; // делегат для вызова ReadPartDelegate в главном потоке
        private SendOrPostCallback exReadDelegate = null; // делегат для вызова события ReadDelegate в главном потоке
        #endregion

        #region Delegates
        /// <summary>
        /// Описывает основной метод отладчика - опрос контроллера
        /// </summary>
        private delegate void WorkingDelegate(SerialPort485 port, ProcessorType ProcessorType);
        #endregion

        #region Events declarations
        /// <summary>
        /// Генерируется при смене режима работы движка
        /// </summary>
        public event EventHandler<DebuggerEngineStatusChangedEventArgs> EngineStatusChanged;
        /// <summary>
        /// Генерируется при смене интервала времени, необходимого для опроса всей очереди
        /// </summary>
        public event EventHandler<EventArgs<int>> RequestTimeChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Текущее состояние движка
        /// </summary>
        public DebuggerEngineStatus EngineStatus { get; private set; }
        /// <summary>
        /// Количество успешно обработанных пакетов чтение
        /// </summary>
        public int ReadedPacketsCount { get; private set; }
        /// <summary>
        /// Количество успешно обработанных пакетов записи
        /// </summary>
        public int WritedPacketsCount { get; private set; }
        /// <summary>
        /// Количество ошибочных пакетов чтения
        /// </summary>
        public int ErrorReadedPacketsCount { get; private set; }
        /// <summary>
        /// Количество ошибочных пакетов записи
        /// </summary>
        public int ErrorWritedPacketsCount { get; private set; }
        /// <summary>
        /// Возвращает или устанавливает параметры работы отладчика
        /// </summary>
        public DebuggerParameters Parameters { get; set; }
        #endregion

        public DebuggerEngine()
        {
            this.EngineStatus = DebuggerEngineStatus.Stopped;
            this.exReadDelegate = this.ExecReadDelegate;
            this.exReadPartDelegate = this.ExecReadPartDelegate;
        }

        /// <summary>
        /// Генерирует событие EngineStatusChanged
        /// </summary>
        private void RaiseEngineStatusChangedEvent(DebuggerEngineStatusChangedEventArgs e)
        {
            this.asyncOp.Post(
                new SendOrPostCallback(delegate(object o) 
                    { 
                        if (this.EngineStatusChanged != null) 
                            this.EngineStatusChanged(this, o as DebuggerEngineStatusChangedEventArgs);
                    }), o);
        }
        /// <summary>
        /// Генерирует событие RequestTimeChanged
        /// </summary>
        private void RaiseRequestTimeChangedEvent(EventArgs<int> e)
        {
            this.asyncOp.Post(
                new SendOrPostCallback(delegate(object o)
                {
                    if (this.RequestTimeChanged != null)
                        this.RequestTimeChanged(this, o as EventArgs<int>);
                }), e);
        }
        /// <summary>
        /// Выполняет делегат ReadPartDelegate
        /// </summary>
        /// <param name="o">
        /// массив object[2]; o[0] - выполняемый делегат, o[1] - массив object[3], содержащий параметры делегата
        /// </param>
        private void ExecReadPartDelegate(object o)
        {
            ProceedingProgressChangedDelegate rpd = (ProceedingProgressChangedDelegate)((object[])o)[0];
            object[] prm = (ProceedingProgressChangedDelegate)((object[])o)[1];
            rpd((double)prm[0], (int)prm[1], (byte[])prm[2]);
        }
        /// <summary>
        /// Выполняет делегат ReadDelegate
        /// </summary>
        /// <param name="o">
        /// массив object[2]; o[0] - выполняемый делегат, o[1] - массив object[3], содержащий параметры делегата
        /// </param>
        private void ExecReadDelegate(object o)
        {
            ProceedingCompleetedDelegate rd = (ProceedingProgressChangedDelegate)((object[])o)[0];
            object[] prm = (ProceedingProgressChangedDelegate)((object[])o)[1];
            rd((string)prm[0], (byte[])prm[1]);
        }

        /// <summary>
        /// Поток опроса контроллера
        /// </summary>
        /// <param name="Sender">Указатель на порт</param>
        private void Questioning_TH()
        {
            Trace.WriteLine("Запуск потока опроса.", "DebuggerEngine");
            this.RaiseEngineStatusChangedEvent(new DebuggerEngineStatusChangedEventArgs(DebuggerEngineStatus.Worked, null));
            DateTime m_STime = DateTime.Now;

            Trace.WriteLine("Открытие порта " + port.PortName, "DebuggerEngine");
            try
            {
                m_port.Open();
            }
            catch (Exception ex)
            {
                this.RaiseEngineStatusChangedEvent(new DebuggerEngineStatusChangedEventArgs(new Exception(Error = "Ошибка окрытия порта: " + ex.Message), DebuggerEngineStatus.Stopped));
                goto end;
            }

            //Проверка на наличие контроллера
            String m_retType = m_port.ReadControllerType();
            if (
                m_type == ProcessorType.AT89C51ED2 && m_retType != "89C51ED2" ||
                m_type == ProcessorType.MB90F347 && m_retType == "89C51ED2" || m_retType == null
               )
            {
                this.RaiseEngineStatusChangedEvent(new DebuggerEngineStatusChangedEventArgs(new Exception("Контроллер не найден"), DebuggerEngineStatus.Stopped));
                goto end;
            }
            // Определение размеров буфера
            if (this.Parameters.ProcessorType == ProcessorType.AT89C51ED2)
                this._BufferSize = 8;
            else if(this.Parameters.ProcessorType == ProcessorType.MB90F347)
            {
                try
                {
                    this._BufferSize = ((Relkon4SerialPort)this._serialPort).BufferSize = this.ReadMB90F347BufferSize(this._serialPort);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Ошибка чтения размера буфера с контроллера: " + ex.Message);
                    this._BufferSize = ((Relkon4SerialPort)this._serialPort).BufferSize = 64;
                }
            }
            while (true)
            {
                //Добавление новых элементов
                if (this.EngineStatus == DebuggerEngineStatus.Stopping)
                    goto end;
                if (_AddElementInQueue.Count != 0)
                {
                    lock (_AddElementInQueue)
                    {
                        foreach (DebuggerPrimitive.QueueElement m_Add in _AddElementInQueue)
                        {
                            _Queue.Enqueue(m_Add);
                            _countElementInQueue++;
                        }
                        List<int> t;
                        _AddElementInQueue.Clear();
                    }
                }

                //Задержка когда отладчик работает, но ничего не делает
                if (_Queue.Count == 0 || _statusEngine != DebuggerEngineStatus.Worked)
                {
                    Thread.Sleep(500);
                    this.RaiseRequestTimeChangedEvent(new EventArgs<int>(0));
                    continue;
                }
                DebuggerPrimitive.QueueElement m_Queue = new DebuggerPrimitive.QueueElement();
                //m_Queue.CycleElement = false;

                try
                {
                    m_Queue = _Queue.Dequeue();

                    //Удаление элементов из очереди
                    if (_AddElementOutQueue.Count != 0 && _AddElementOutQueue.Contains(m_Queue.ID))
                    {
                        lock (_AddElementOutQueue)
                        {
                            m_Queue.CycleElement = false;
                            _AddElementOutQueue.Remove(m_Queue.ID);
                            m_Queue.isRuningReadLongBuffer = false;
                            continue;
                        }
                        IList<int> i;
                    }

                    if (m_Queue.Period != 1 && !m_Queue.isRuningReadLongBuffer)
                    {
                        if (m_Queue.CurrentCycle != 0)
                        {
                            m_Queue.CurrentCycle--;
                            continue;
                        }
                        else
                            m_Queue.CurrentCycle = m_Queue.Period;
                    }

                    m_STime = DateTime.Now;

                    switch (m_Queue.Queuetype)
                    {
                        case DebuggerPrimitive.ActionType.Reading:
                            #region Чтение данных из контроллера
                            if (this.EngineStatus == DebuggerEngineStatus.Stopping)
                                goto end;
                            try
                            {
                                int m_Address = 0;
                                int m_Count = 0;

                                if (m_Queue.isReadLongBuffer)
                                {
                                    m_Address = m_Queue.iAddress;
                                    m_Count = Math.Min(_BufferSize, m_Queue.Count - (m_Queue.iAddress - m_Queue.Address));
                                    m_Queue.isRuningReadLongBuffer = true;
                                }
                                else
                                {
                                    m_Address = m_Queue.Address;
                                    m_Count = m_Queue.Count;
                                }
                                Byte[] m_ret = null;
                                //Запрос к контроллеру
                                try
                                {
                                    m_ret = m_port.ReadFromMemory(m_Queue.Memorytype, m_Address, (Int32)m_Count);
                                }
                                catch (Exception) { }

                                //Если мы не получили ответ
                                if (m_ret != null && m_Queue.Callback != null)
                                {
                                    _ReadPacket++;
                                    //Загрузка большого размера архива
                                    if (m_Queue.isReadLongBuffer)
                                    {
                                        m_Queue.ReadLongBuffer.SetSegmentArray(m_ret, (Int16)(m_Queue.iAddress - m_Queue.Address));

                                        if (m_Queue.PartCallBack != null)
                                        {
                                            double value = ((double)(m_Queue.iAddress - m_Queue.Address)) / m_Queue.Count;
                                            this.asyncOp.Post(this.exReadPartDelegate, new object[] { m_Queue.PartCallBack, new object[] { value, m_Queue.iAddress, m_ret } });
                                        }

                                        if (m_Count == _BufferSize)
                                        {
                                            m_Queue.iAddress = (m_Queue.iAddress + m_Count);
                                            if (!m_Queue.CycleElement) _Queue.Enqueue(m_Queue);
                                        }
                                        else
                                        {
                                            m_Queue.isRuningReadLongBuffer = false;
                                            m_Queue.iAddress = m_Queue.Address;
                                            this.asyncOp.Post(this.exReadPartDelegate, new object[] { m_Queue.CallBack, new object[] { m_Queue.Marker, m_Queue.ReadLongBuffer.ReadBuffer } });
                                        }
                                        continue;
                                    }

                                    //Обратный вызов
                                    this.asyncOp.Post(this.exReadPartDelegate, new object[] { m_Queue.CallBack, new object[] { m_Queue.Marker, m_ret } });
                                }
                                else
                                {
                                    //Загрузка большого размера архива
                                    if (m_Queue.isReadLongBuffer && !m_Queue.CycleElement) _Queue.Enqueue(m_Queue);
                                    _ErrorReadPacket++;

                                    //Обратный вызов
                                    this.asyncOp.Post(this.exReadPartDelegate, new object[] { m_Queue.CallBack, new object[] { m_Queue.Marker, new Bytep[0] } });
                                }

                            }
                            catch (Exception Ex)
                            {
                                Trace.WriteLine(Ex.Message, "DebuggerEngine");
                                //Если произошла ошибка помещаем элемент в очередь
                                if (!m_Queue.CycleElement)
                                    _Queue.Enqueue(m_Queue);
                                _ErrorReadPacket++;
                            }
                            #endregion
                            break;
                        case DebuggerPrimitive.ActionType.Writing:
                            #region Запись данный в контроллер
                            if (this.EngineStatus == DebuggerEngineStatus.Stopping)
                                goto end;
                            try
                            {
                                m_port.WriteToMemory(m_Queue.Memorytype, m_Queue.Address, m_Queue.Recording_Buffer);
                                _WritePacket++;
                            }
                            catch
                            {
                                _ErrorWritePacket++;
                            }
                            #endregion
                            break;
                    }
                }
                catch (Exception Ex)
                {
                    Trace.WriteLine("Exception " + Ex.Message, "DebuggerEngine");
                }
                finally
                {
                    int m_Seconds = new DateTime(DateTime.Now.Ticks - m_STime.Ticks).TimeOfDay.Milliseconds;
                    this.RaiseRequestTimeChangedEvent(new EventArgs<int>(m_Seconds));

                    if (m_Queue.CycleElement)
                        _Queue.Enqueue(m_Queue);
                    else if ((!m_Queue.isRuningReadLongBuffer))
                        _countElementInQueue--;
                }
                this.RaiseEngineStatusChangedEvent(new DebuggerEngineStatusChangedEventArgs(DebuggerEngineStatus.Stopped, null));
            }
        end:
            this.asyncOp.PostOperationCompleted(null, null);
            _Queue.Clear();
            _countElementInQueue = 0;
            Trace.WriteLine("Закрытие порта " + m_port.PortName, "DebuggerEngine");
            m_port.Close();
            this.EngineStatus = DebuggerEngineStatus.Stopped;
        }

        /// <summary>
        /// Остановка отладчика
        /// </summary>
        public void Stop()
        {
            Trace.WriteLine("Остановка движка", "DebuggerEngine");
            this.EngineStatus = DebuggerEngineStatus.Stopping;
            this._serialPort.Stop();
        }
        /// <summary>
        /// Запуск отладчика
        /// </summary>
        public void Start()
        {
            if (this.EngineStatus != DebuggerEngineStatus.Stopped)
                throw new Exception("Отладчик уже запущен");
            SerialPort485 p = new SerialPort485(this.Parameters.PortName, this.Parameters.BaudRate, this.Parameters.ProcessorType);
            if (this.Parameters.ProcessorType == ProcessorType.AT89C51ED2)
            {
                this._BufferSize = 8;
                this._serialPort = new Relkon37SerialPort(p);
            }
            else if (this.Parameters.ProcessorType == ProcessorType.MB90F347)
            {
                this._serialPort = new Relkon4SerialPort(p);
            }
            else
                throw new Exception("Процессор " + this.Parameters.ProcessorType + " не поддерживается");
            this._serialPort.ControllerAddress = this.Parameters.ControllerNumber;
            this.asyncOp = AsyncOperationManager.CreateOperation(null);
            WorkingDelegate worker = new WorkingDelegate(this.Questioning_TH);
            worker.BeginInvoke(port, this.Parameters.ProcessorType, null, null);
        }
        /// <summary>
        /// Читает размер буффера с контроллера на базе процессора MB90F347
        /// </summary>
        /// <param name="port">Порт, через который осуществляется чтение; должен быть уже открыт</param>
        private int ReadMB90F347BufferSize(Relkon37SerialPort port)
        {
            byte[] addresses = { 340, 344 };
            int res = int.MaxValue;
            for (int i = 0; i < 2; i++)
            {
                byte[] response = port.ReadEEPROM(addresses[i], 2);
                if (this.Parameters.InverseByteOrder)
                    Array.Reverse(response);
                int SRX = AppliedMath.BytesToInt(response);
                response = port.ReadEEPROM(addresses[i] + 4, 2);
                if (this.InverseByteOrder)
                    Array.Reverse(response);
                NRX = AppliedMath.BytesToInt(response);
                res = Math.Min(res, NRX - SRX + 1);
            }
            return res;
        }

        #region Queue editing
        /// <summary>
        /// Добавление элемента в очередь
        /// </summary>
        /// <param name="Queuetype">Тип запроса (чтение/запись)</param>
        /// <param name="Memorytype">Тип памяти</param>
        /// <param name="Address">Начальный адрес</param>
        /// <param name="Count">Количество байт для чтения</param>
        /// <param name="Period">Период опроса</param>
        /// <param name="CycleElement">Флаг цикличности</param>
        /// <param name="Recording_Buffe">Записываемых буфер</param>
        /// <param name="Callback">Функция обратного вызова</param>
        /// <returns>Уникальный номер в очереди</returns>
        public Guid AddElementInQueue
            (
                DebuggerPrimitive.ActionType Queuetype,
                MemoryType Memorytype,
                int Address,
                byte[] Recording_Buffe,
                int Count,
                bool CycleElement,
                int Period,
                ProceedingCompleetedDelegate Callback
            )
        {
            Guid m_ret = AddElement(null, Queuetype, Memorytype, Address, Recording_Buffe, Count, CycleElement, Period, Callback, null);
            return m_ret;
        }

        /// <summary>
        /// Добавление элемента в очередь
        /// </summary>
        /// <param name="Queuetype">Тип запроса (чтение/запись)</param>
        /// <param name="Memorytype">Тип памяти</param>
        /// <param name="Address">Начальный адрес</param>
        /// <param name="Count">Количество байт для чнения</param>
        /// <param name="Period">Период опроса</param>
        /// <param name="CycleElement">Флаг цикличности</param>
        /// <param name="Recording_Buffe">Записываемых буфер</param>
        /// <param name="Callback">Функция обратного вызова</param>
        /// <returns>Уникальный номер в очереди</returns>
        public Guid AddElementInQueue
            (
                string Marker,
                DebuggerPrimitive.ActionType Queuetype,
                MemoryType Memorytype,
                int Address,
                byte[] Recording_Buffe,
                int Count,
                bool CycleElement,
                int Period,
                ProceedingCompleetedDelegate Callback
            )
        {
            Guid m_ret = AddElement(Marker, Queuetype, Memorytype, Address, Recording_Buffe, Count, CycleElement, Period, Callback, null);
            return m_ret;
        }

        /// <summary>
        /// Добавление элемента в очередь
        /// </summary>
        /// <param name="Queuetype">Тип запроса (чтение/запись)</param>
        /// <param name="Memorytype">Тип памяти</param>
        /// <param name="Address">Начальный адрес</param>
        /// <param name="Count">Количество байт для чнения</param>
        /// <param name="Period">Период опроса</param>
        /// <param name="CycleElement">Флаг цикличности</param>
        /// <param name="Recording_Buffe">Записываемых буфер</param>
        /// <param name="Callback">Функция обратного вызова</param>
        /// <returns>Уникальный номер в очереди</returns>
        public Guid AddElementInQueue
            (
                DebuggerPrimitive.ActionType Queuetype,
                MemoryType Memorytype,
                int Address,
                byte[] Recording_Buffe,
                int Count,
                bool CycleElement,
                int Period,
                ProceedingCompleetedDelegate Callback,
                ProceedingProgressChangedDelegate PartCallback

            )
        {
            Guid m_ret = AddElement(null, Queuetype, Memorytype, Address, Recording_Buffe, Count, CycleElement, Period, Callback, PartCallback);
            return m_ret;
        }

        private Guid AddElement
            (
                string Marker,
                DebuggerPrimitive.ActionType Queuetype,
                MemoryType Memorytype,
                int Address,
                byte[] Recording_Buffe,
                int Count,
                bool CycleElement,
                int Period,
                ProceedingCompleetedDelegate Callback,
                ProceedingProgressChangedDelegate PartCallback)
        {
            Guid m_ret = Guid.NewGuid();
            Trace.WriteLine("Добавление запроса в очередь :" + m_ret.ToString(), "DebuggerEngine");

            DebuggerPrimitive.QueueElement m_newQuery = new DebuggerPrimitive.QueueElement();

            if (Count > BufferSize)
            {
                //Добавляем часть для чтения частями
                m_newQuery.isReadLongBuffer = true;
                m_newQuery.ReadLongBuffer = new DebuggerPrimitive.ReadLongData(Count);
                m_newQuery.iAddress = Address;
                m_newQuery.PartCallBack = PartCallback;
            }
            else
            {
                m_newQuery.isReadLongBuffer = false;
                m_newQuery.ReadLongBuffer = null;
            }

            m_newQuery.Marker = Marker;
            m_newQuery.Period = Period;
            m_newQuery.ID = m_ret;
            m_newQuery.Address = Address;
            m_newQuery.Callback = Callback;
            m_newQuery.Count = Count;
            m_newQuery.CycleElement = CycleElement;
            m_newQuery.Memorytype = Memorytype;
            m_newQuery.Queuetype = Queuetype;
            m_newQuery.Recording_Buffer = Recording_Buffe;

            //Блокируем список и добавляем элемент
            lock (_AddElementInQueue)
            {
                _AddElementInQueue.Add(m_newQuery);
            }
            return m_ret;
        }

        /// <summary>
        /// Удаляет элемент из очереди опроса контроллера
        /// </summary>
        /// <param name="IDElement">УИН элемента (Возвращается методом добавления элемента в очередь)</param>
        public void RemoveElementInQueue(Guid IDElement)
        {
            Trace.WriteLine("Удаление запроса из очереди :" + IDElement, "DebuggerEngine");
            lock (_AddElementOutQueue)
            {
                _AddElementOutQueue.Add(IDElement);
            }
        }
        #endregion
    }

    #region enums
    /// <summary>
    /// Перечисление состояний движка
    /// </summary>
    public enum DebuggerEngineStatus
    {
        /// <summary>
        /// Движок остановлен
        /// </summary>
        Stopped,
        /// <summary>
        /// Движок в процессе остановки
        /// </summary>
        Stopping,
        /// <summary>
        /// Движок находиться в рабочем состоянии
        /// </summary>
        Worked
    }
    /// <summary>
    /// Тип действия элемента элемента очереди отладчика
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// Чтение данных с контроллера
        /// </summary>
        Reading,
        /// <summary>
        /// Запись данных в контроллер
        /// </summary>
        Writing
    }
    #endregion

    /// <summary>
    /// Делегат для получения промежуточного буфера при чтении большого пакета
    /// </summary>
    /// <param name="PercentRead">Процент считанного (0...1)</param>
    /// <param name="StartIndex">Начальный индекс части</param>
    /// <param name="Buffer">Считанный буфер</param>
    public delegate void ProceedingProgressChangedDelegate(double PercentRead, int StartIndex, byte[] Buffer);
    /// <summary>
    /// Делегат для считанного буфера
    /// </summary>
    /// <param name="Marker">Маркер</param>
    /// <param name="Buffer">Считанный буфер</param>
    public delegate void ProceedingCompleetedDelegate(string Marker, byte[] Buffer);

    /// <summary>
    /// Аргумент события DebuggerEngine.EngineStatusChaged
    /// </summary>
    public class DebuggerEngineStatusChangedEventArgs
    {
        /// <summary>
        /// Ошибка приведшая к смене статуса отладчика
        /// </summary>
        public Exception Error { get; private set; }
        /// <summary>
        /// Новый статус отладчика
        /// </summary>
        public DebuggerEngineStatus Status { get; private set; }

        public DebuggerEngineStatusChangedEventArgs(DebuggerEngineStatus Status, Exception Error)
        {
            this.Status = Status;
            this.Error = Error;
        }
    }

    /// <summary>
    /// Основная структурная еденица отладчика, описывает опрашиваемый элемент;
    /// на основе списка таких объектов формируется очередь опроса отладчика
    /// </summary>
    public class RequestItem
    {
        /// <summary>
        /// Адрес области памяти, к которой обращается объект
        /// </summary>
        public int Address;
        /// <summary>
        /// Тип области памяти, к которой обращается объект;
        /// </summary>
        public MemoryType MemoryType;
        /// <summary>
        /// Метод, вызывающийся когда отладчик опросил текущий
        /// </summary>
        public DebuggerEngine.ProceedingCompleetedDelegate ReadCompleted;
        /// <summary>
        /// Метод, периодически вызывающийся в процессе опроса элемента, для отображения процесса опроса элемента
        /// (используется только при одноразовом чтении области памяти)
        /// </summary>
        public DebuggerEngine.ProceedingProgressChangedDelegate ReadPart;
        /// <summary>
        /// Тип действия, которое движок отладчика должен выполнит в отношении элемента: считать или записать данные
        /// </summary>
        public ActionType ActionType;
        /// <summary>
        /// Если ActionType == ActionType.Writing, то содержит данные, которые должны быть записаны в контроллер
        /// </summary>
        public byte[] Data;
        /// <summary>
        /// Используется для идентификации элемента после завершения обработки движком отладчика
        /// </summary>
        public object Sender;
    }

    /// <summary>
    /// Примитивы для отладчика
    /// </summary>
    public class DebuggerPrimitive
    {
        /// <summary>
        /// Возможные дейтвия элемента в очереди
        /// </summary>
        public enum ActionType
        {
            /// <summary>
            /// Чтение из контроллера
            /// </summary>
            Reading,
            /// <summary>
            /// Запись в контроллер
            /// </summary>
            Writing
        }
        /// <summary>
        /// Структура элемента в очереди
        /// </summary>
        public struct QueueElement
        {
            public string Marker;
            /// <summary>
            /// Уникальный номер в очереди
            /// </summary>
            public Guid ID;
            /// <summary>
            /// Тип элемента в очереди (чтение/запись в контроллер)
            /// </summary>
            public ActionType Queuetype;
            /// <summary>
            /// Тип памяти
            /// </summary>
            public MemoryType Memorytype;
            /// <summary>
            /// Начальный адрес
            /// </summary>
            public int Address;
            /// <summary>
            /// Записываемый буфер
            /// </summary>
            public byte[] Recording_Buffer;
            /// <summary>
            /// Длина чнения/записи
            /// </summary>
            public int Count;
            /// <summary>
            /// Флаг цикличного повторения элемента в очереди
            /// </summary>
            public bool CycleElement;
            /// <summary>
            /// Период повторения
            /// </summary>
            public int Period;
            /// <summary>
            /// Текущий цикл
            /// </summary>
            public int CurrentCycle;
            /// <summary>
            /// Вызов метода после выполнения элемента из очереди
            /// </summary>
            public DebuggerEngine.ProceedingCompleetedDelegate Callback;

            /// <summary>
            /// Флаг чнения массива большой длинны
            /// </summary>
            internal bool isReadLongBuffer;
            internal bool isRuningReadLongBuffer;
            /// <summary>
            /// Класс для чнения массива большой длинны
            /// </summary>
            internal ReadLongData ReadLongBuffer;
            /// <summary>
            /// Начальный адрес (для чтения буфура большого размера)
            /// </summary>
            internal int iAddress;

            public DebuggerEngine.ProceedingProgressChangedDelegate PartCallBack;
        }
        /// <summary>
        /// Чтение массивов большой длинны
        /// </summary>
        internal class ReadLongData
        {
            /// <summary>
            /// Конечный буфер
            /// </summary>
            private byte[] _buffer = null;

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="Count">Размер буфера</param>
            /// <param name="CountParts">Количество частей</param>
            public ReadLongData(int Count)
            {
                //Создаем буфер
                _buffer = new byte[Count];
            }

            /// <summary>
            /// Добавляет сегмент в общий массив
            /// </summary>
            /// <param name="Addres">Начальный адрес</param>
            /// <param name="Buffer">Массив</param>
            internal void SetSegmentArray(byte[] Buffer, int StartAddress)
            {
                int m_Len = Math.Min(_buffer.Length - StartAddress, Buffer.Length);
                Array.Copy(Buffer, 0, _buffer, StartAddress, m_Len);
            }

            /// <summary>
            /// Считанный массив
            /// </summary>
            public byte[] ReadBuffer
            {
                get { return _buffer; }
                set { _buffer = value; }
            }
        }
    }
}
