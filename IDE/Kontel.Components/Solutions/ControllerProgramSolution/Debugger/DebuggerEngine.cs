using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon.Forms;
using Kontel.Relkon.Solutions;
using Kontel.Relkon;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Collections;

namespace Kontel.Relkon.Debugger
{
    /// <summary>
    /// Движок отладчика
    /// </summary>
    public sealed class DebuggerEngine
    {
        private int NearErrors = 0;//количество ошибок подряд

        #region Request definitions
        /// <summary>
        /// Запрос на чтение данных с контроллера
        /// </summary>
        private sealed class ReadRequest : IComparable<ReadRequest>
        {
            private int minBytesToRead = 1; // минимальное число байт, которое может считать запрос
            private int maxBytesToRead = 1; // максимальное число байт, которое может считать запрос


            /// <summary>
            /// Адрес области памяти, к которой обращается запрос
            /// </summary>
            public int Address { get; private set; }
            /// <summary>
            /// Тип области памяти
            /// </summary>
            public MemoryType MemoryType { get; private set; }
            /// <summary>
            /// Возвращает число байт, которое должен обработать запрос
            /// </summary>
            public int BytesToRead { get; private set; }
            /// <summary>
            /// Возвращает абсолютное смещение последнего байта
            /// </summary>
            public int EndByteOffset
            {
                get
                {
                    return this.Address + this.BytesToRead - 1;
                }
            }
            /// <summary>
            /// Возвращает число байт, на которое можном увеличить текущее число считываемых байт
            /// </summary>
            public int GrowUpBytesCount
            {
                get
                {
                    return this.maxBytesToRead - this.BytesToRead;
                }
            }
            /// <summary>
            /// Возвращает число байт, на которое можно уменьшить текущее число считываемых байт
            /// </summary>
            public int GrowDownBytesCount
            {
                get
                {
                    return this.BytesToRead - this.minBytesToRead;
                }
            }
            /// <summary>
            /// Возвращает список элементов, опрашиваемых запросом
            /// </summary>
            public List<RequestItem> Items { get; private set; }

            public ReadRequest(int Address, MemoryType MemoryType, int BytesToRead, int MinBytesToRead, int MaxBytesToRead)
            {
                this.Address = Address;
                this.MemoryType = MemoryType;
                this.Items = new List<RequestItem>();
                this.Address = Address;
                this.maxBytesToRead = MaxBytesToRead;
                this.minBytesToRead = MinBytesToRead;
                this.BytesToRead = Math.Min(Math.Max(BytesToRead, this.minBytesToRead), this.maxBytesToRead);
            }
            /// <summary>
            /// Добавляет новый элемент в запрос, с сохранением сортировки по адресу,
            /// ДОБАВЛЯТЬ ТОЛЬКО ТАК
            /// </summary>
            public void AddItem(RequestItem item)
            {
                if (this.Items.Contains(item))
                    return;
                if (item.MemoryType != this.MemoryType)
                    throw new Exception("Типы областей памяти должны совпадать");
                int idx = this.Items.BinarySearch(item);
                if (idx < 0)
                    idx = ~idx;
                this.Items.Insert(idx, item);
            }
            /// <summary>
            /// Добавляет несколько элементов в список элементов запроса
            /// </summary>
            public void AddItems(List<RequestItem> items)
            {
                foreach (RequestItem item in items)
                {
                    this.AddItem(item);
                }
            }
            /// <summary>
            /// Меняет число байт для чтения
            /// </summary>
            public void ChangeBytesToRead(int value)
            {
                if (value < this.minBytesToRead || value > this.maxBytesToRead)
                    throw new Exception("Неверный параметр");
                this.BytesToRead = value;
            }
            /// <summary>
            /// Изменяет адрес запроса; в случае необходимости увеличивает или уменьшает число байт для чтения
            /// </summary>
            public void ChangeAddress(int value)
            {
                int newBytesToRead = Math.Max(this.EndByteOffset + 1 - value, this.minBytesToRead);
                //if (this.Items.Count > 0 && value > this.Items[0].Address)
                //    throw new Exception("Неверный параметр");
                this.BytesToRead = newBytesToRead;
                this.Address = value;
            }

            #region IComparable<Request> Members

            int IComparable<ReadRequest>.CompareTo(ReadRequest other)
            {
                return this.Address - other.Address;
            }

            #endregion
        }
        #endregion

        #region RequestItem definition
        /// <summary>
        /// Основная структурная единица отладчика, описывает опрашиваемый элемент;
        /// на основе списка таких объектов формируется очередь опроса отладчика
        /// </summary>
        private class RequestItem : IComparable<RequestItem>
        {
            /// <summary>
            /// Адрес области памяти, к которой обращается объект
            /// </summary>
            public int Address { get; set; }
            /// <summary>
            /// Тип области памяти, к которой обращается объект;
            /// </summary>
            public MemoryType MemoryType { get; set; }
            /// <summary>
            /// Метод, вызывающийся когда отладчик опросил текущий
            /// </summary>
            public ProceedingCompleetedDelegate ProceedingCompletedCallback { get; set; }
            /// <summary>
            /// Метод, периодически вызывающийся в процессе опроса элемента, для отображения процесса опроса элемента
            /// (используется только при одноразовом чтении области памяти)
            /// </summary>
            public ProceedingProgressChangedDelegate ProgressChangedCallback { get; set; }
            /// <summary>
            /// Тип действия, которое движок отладчика должен выполнит в отношении элемента: считать или записать данные
            /// </summary>
            public RequestType RequestType { get; set; }
            /// <summary>
            /// Данные которые считаны / записаны в контроллер (в зависимости от значения RequestType)
            /// </summary>
            public byte[] Data { get; set; }
            /// <summary>
            /// Возвращает размер данных, обрабатываеых элементом
            /// </summary>
            public int Size
            {
                get
                {
                    return this.Data.Length;
                }
            }
            /// <summary>
            /// Указывает, что были ошибки в загрузке данных
            /// </summary>
            public bool Error { get; set; }
            /// <summary>
            /// Используется для идентификации элемента после завершения обработки движком отладчика
            /// </summary>
            public object Sender { get; set; }
            /// <summary>
            /// Возвращает абсолютное смещение последнего байта
            /// </summary>
            public int EndByteOffset
            {
                get
                {
                    return this.Address + this.Size - 1;
                }
            }

            #region IComparable<RequestItem> Members

            public int CompareTo(RequestItem other)
            {
                return this.Address - other.Address;
            }

            #endregion
        }
        #endregion

        private Hashtable requestsByMemory = new Hashtable(); // ключ - тип области памяти, к которой обращаются запросы, значение - список запросов, отсортированный по адресу
        private List<RequestItem> readItems = new List<RequestItem>(); // список элементов на чтение
        private List<RequestItem> writeItems = new List<RequestItem>(); // список элементов на запись
        private int maxRequestSize = -1; // максимальное число байт, которое может обработать один запрос

        private AsyncOperation asyncOp = null; // управляет асинхронными операциями отладчика
        private SendOrPostCallback exProgressChangedDelegate = null; // делегат для вызова ReadPartDelegate в главном потоке
        private SendOrPostCallback exProceedingCompleetedDelegate = null; // делегат для вызова события ReadDelegate в главном потоке
        private MemoryType lockingMemoryType; // тип памяти, в котором есть заблокированная область (который не должна опрашиваться)
        private int lockingAddress; // адрес заблокированной области памяти
        private int lockingLength; // размер заблокированной области памяти
        private bool lockingActive = false; // флаг, показывающий, активна ли сейчас блокировка


        /// <summary>
        /// Описывает основной метод отладчика - опрос контроллера
        /// </summary>
        private delegate void WorkingDelegateCOM(AbstractChannel port);
   

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
        public int ReadedRequestsCount { get; private set; }
        /// <summary>
        /// Количество успешно обработанных пакетов записи
        /// </summary>
        public int WritedRequestsCount { get; private set; }
        /// <summary>
        /// Количество ошибочных пакетов чтения
        /// </summary>
        public int ErrorReadedRequestsCount { get; private set; }
        /// <summary>
        /// Количество ошибочных пакетов записи
        /// </summary>
        public int ErrorWritedRequestsCount { get; private set; }
        /// <summary>
        /// Возвращает или устанавливает параметры работы отладчика
        /// </summary>
        public DebuggerParameters Parameters { get; set; }
        #endregion

        public DebuggerEngine()
        {
            this.exProceedingCompleetedDelegate = new SendOrPostCallback(this.ExecProceedingCompleetedDelegate);
            this.exProgressChangedDelegate = new SendOrPostCallback(this.ExecProgressChangedDelegate);
        }

        /// <summary>
        /// Возвращает минимально число байт, которое может считать запрос отладчика
        /// </summary>
        private int GetRequestMinBytesToRead()
        {
            if (this.Parameters.ProcessorType == ProcessorType.AT89C51ED2)
                return 8;
            else if (this.Parameters.ProcessorType == ProcessorType.MB90F347)
                return 1;
            else if (this.Parameters.ProcessorType == ProcessorType.STM32F107)
                return 1;
            else
                throw new Exception("Процессор " + this.Parameters.ProcessorType + " не поддерживается");
        }
        /// <summary>
        /// Возвращает максимальное число байт, которое запрос может считать из указанной области памяти
        /// </summary>
        private int GetRequestMaxBytesToRead(MemoryType MemoryType)
        {
            if (this.asyncOp != null && (this.asyncOp.UserSuppliedState is AbstractChannel))
                return ((AbstractChannel)this.asyncOp.UserSuppliedState).GetPacketSize(MemoryType);
            else
                return 0x40;
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
                }), e);
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
        /// Выполняет делегат ProceedingCompleetedDelegate
        /// </summary>
        /// <param name="Params">
        /// массив object[4]; o[0] - выполняемый делегат, o[1] - Sender, o[2] - Data, o[3] - Error
        /// </param>
        private void ExecProceedingCompleetedDelegate(object Params)
        {
            object[] prm = (object[])Params;
            try
            {
                ProceedingCompleetedDelegate pcd = (ProceedingCompleetedDelegate)prm[0];
                pcd(prm[1], (byte[])prm[2], (bool)prm[3]);
            }
            catch
            {
                //Я не знаю почему здесь вылетает исключение "доступ к ликвидированному объекту не возможен"
            }
        }
        /// <summary>
        /// Выполняет делегат ProceedingProgressChangedDelegate
        /// </summary>
        /// <param name="Params">
        /// массив object[4]; o[0] - выполняемый делегат, o[1] - Sender, o[2] - Percent, o[3] - Buffer
        /// </param>
        private void ExecProgressChangedDelegate(object Params)
        {
            object[] prm = (object[])Params;
            ProceedingProgressChangedDelegate ppcd = (ProceedingProgressChangedDelegate)prm[0];
            ppcd(prm[1], (double)prm[2], (byte[])prm[3]);
        }
        /// <summary>
        /// Запуск отладчика
        /// </summary>
        public void Start()
        {
            if (this.EngineStatus != DebuggerEngineStatus.Stopped)
                throw new Exception("Отладчик уже запущен");

            AbstractChannel port = null;

            if (this.Parameters.ComConection)            
                port = new SerialportChannel(this.Parameters.PortName, this.Parameters.BaudRate, this.Parameters.ProtocolType);
            else
                port = new EthernetChannel(this.Parameters.PortIP, 12144, this.Parameters.ProtocolType);                                                                 

            port.ControllerAddress = this.Parameters.ControllerNumber;
            this.asyncOp = AsyncOperationManager.CreateOperation(port);
            WorkingDelegateCOM worker = new WorkingDelegateCOM(this.ProceedRequests);
            worker.BeginInvoke(port, null, null);
        }
        /// <summary>
        /// Остановка отладчика
        /// </summary>
        public void Stop()
        {
            Trace.WriteLine("Остановка движка", "DebuggerEngine");
            this.EngineStatus = DebuggerEngineStatus.Stopping;
            ((AbstractChannel)this.asyncOp.UserSuppliedState).Stop();            
        }
        
        /// <summary>
        /// Удаляет опрашиваемый элемент из очереди запрсов отладчика
        /// </summary>
        /// <param name="Address">Адрес удаляемого элемента</param>
        /// <param name="MemoryType">Тип памяти, с которым работает удаляемый элемент</param>
        /// <param name="Sender">Идентификатор удаляемого элемента</param>
        public void RemoveReadItem(int Address, MemoryType MemoryType, object Sender)
        {
            // Ищем удаляемый элемент
            List<ReadRequest> requests = this.requestsByMemory[MemoryType] as List<ReadRequest>;
            if (requests == null)
            {
                List<RequestItem> read_items = this.readItems;
                if (read_items == null) return;
                //Удаление элемента из списка, если размер считывания не известен
                RequestItem removed_item = read_items.Find(new Predicate<RequestItem>(x => x.Address == Address && x.Sender.Equals(Sender)));
                // Элемент не найден
                if (removed_item == null) return;
                read_items.Remove(removed_item);
                return;
            }
            int idx = requests.BinarySearch(new ReadRequest(Address, MemoryType, 0, 0, 0));
            if (idx < 0)
                idx = ~idx - 1;
            if (idx < 0)
                return;
            RequestItem RemovedItem = requests[idx].Items.Find(new Predicate<RequestItem>(x => x.Address == Address && x.Sender.Equals(Sender)));
            if(RemovedItem == null)
                // Элемент не найден
                return;
            // Получаем все запросы, в которых используется элемент
            List<ReadRequest> RequestsWithRemovedItem = requests.FindAll(new Predicate<ReadRequest>(x => (x.EndByteOffset >= RemovedItem.Address && x.EndByteOffset <= RemovedItem.EndByteOffset)
                                                                                || (x.Address >= RemovedItem.Address && x.EndByteOffset >= RemovedItem.EndByteOffset)
                                                                                || (x.Address <= RemovedItem.Address && x.EndByteOffset >= RemovedItem.EndByteOffset)));
            this.lockingAddress = RequestsWithRemovedItem[0].Address;
            this.lockingMemoryType = MemoryType;
            this.lockingLength = RequestsWithRemovedItem[RequestsWithRemovedItem.Count - 1].EndByteOffset + 1 - this.lockingAddress;
            this.lockingActive = true;
            // Удаляем эти запросы из списка запросов
            foreach (ReadRequest request in RequestsWithRemovedItem)
                requests.Remove(request);
            // Получаем список оставшихся элементов
            List<RequestItem> items = new List<RequestItem>();
            foreach (ReadRequest request in RequestsWithRemovedItem)
            {
                foreach (RequestItem item in request.Items)
                {
                    if (item != RemovedItem && !items.Contains(item))
                        items.Add(item);
                }
            }
            // Добавляем оставшиеся элементы обратно в список запросов
            foreach (RequestItem item in items)
            {
                this.AddReadItem(item);
            }
            this.lockingActive = false;
        }
        /// <summary>
        /// Добавляет в очередь опороса отладчика элемент чтения данных данных
        /// </summary>
        /// <param name="Address">Адрес области памяти, к которой производится обращение</param>
        /// <param name="MemoryType">Тип области памяти, которой производится обращение</param>
        /// <param name="Buffer">Записываемые даные</param>
        /// <param name="Sender">Опрашиваемый объект</param>
        /// <param name="ProgressChangedCallback">Callback для получения информации о процессе опроса</param>
        /// <param name="CompleetingCallback">Callback для порлучения информации о завершении опроса</param>
        public void AddReadItem(int Address, MemoryType MemoryType, int Count, object Sender, 
            ProceedingProgressChangedDelegate ProgressChangedCallback, ProceedingCompleetedDelegate CompleetingCallback)
        {
            RequestItem item = new RequestItem()
            {
                Address = Address,
                Data = new byte[Count],
                MemoryType = MemoryType,
                RequestType = RequestType.Read,
                Sender = Sender,
                ProgressChangedCallback = ProgressChangedCallback,
                ProceedingCompletedCallback = CompleetingCallback
            };
            this.lockingMemoryType = MemoryType;
            this.lockingAddress = Address;
            this.lockingLength = Count;
            this.lockingActive = true;
            this.AddReadItem(item);
            this.lockingActive = false;
        }
        /// <summary>
        /// Добавляет в очередь опороса отладчика объект записи данных
        /// </summary>
        private void AddReadItem(RequestItem item)
        {
            if (this.maxRequestSize == -1)
            {
                if (this.Parameters.ProcessorType == ProcessorType.AT89C51ED2)
                    this.maxRequestSize = 8;
                else
                {
                    // Максимальный размер запроса неизвестен
                    this.readItems.Add(item);
                    return;
                }
            }
            ReadRequest request = new ReadRequest(item.Address, item.MemoryType, item.Size, this.GetRequestMinBytesToRead(), this.GetRequestMaxBytesToRead(item.MemoryType));
            request.AddItem(item);
            // Получаем список запросов, в который надо добавить элемент
            List<ReadRequest> requests = this.requestsByMemory[item.MemoryType] as List<ReadRequest>;
            if (requests == null)
            {
                requests = new List<ReadRequest>();
                this.requestsByMemory.Add(item.MemoryType, requests);
            }
            // Получаем индекс запроса, адрес которого больше или равен адресу добавляемого элемента
            int idx = requests.BinarySearch(request);
            if (idx < 0)
                idx = ~idx;
            // Добавляем элемент в очередь запросов
            if (requests.Count == 0)
            {
                requests.Add(request);
            }
            else if (idx < requests.Count && requests[idx].Address == item.Address)
            {
                // Адрес idx-запроса совпадает с адресом переменной
                requests[idx].AddItem(item);
                if (item.EndByteOffset > requests[idx].EndByteOffset)
                    requests[idx].ChangeBytesToRead(requests[idx].BytesToRead + Math.Min(item.EndByteOffset - requests[idx].EndByteOffset, requests[idx].GrowUpBytesCount));
            }
            else if (idx == 0)
            {
                // Адрес запроса наименьший из всех опрашиваемых
                requests.Insert(idx, request);
            }
            else if (requests[idx - 1].EndByteOffset + Math.Min(7, requests[idx - 1].GrowUpBytesCount) >= item.Address)
            {
                // По краеней мере часть элемента опрашивается текущим запросом
                requests[--idx].AddItem(item);
                if (item.EndByteOffset > requests[idx].EndByteOffset)
                    requests[idx].ChangeBytesToRead(requests[idx].BytesToRead + Math.Min(item.EndByteOffset - requests[idx].EndByteOffset, requests[idx].GrowUpBytesCount));
            }
            else
            {
                // Элемент находитя между запросами и не может быт опрошен левым даже частично
                requests.Insert(idx, request);
            }
            // Опрос элемента начинается запросом с индексом idx
            int c = idx;
            bool exit = true;
            do
            {
                // Убираем накладывающиеся друг на друга вопросы
                for (int i = c + 1; i < requests.Count && requests[i].Address <= requests[c].EndByteOffset + 1 + Math.Min(7, requests[c].GrowUpBytesCount); i++)
                {
                    if (requests[c].Address + requests[c].BytesToRead + requests[c].GrowUpBytesCount - 1 >= requests[i].EndByteOffset)
                    {
                        // Запрос целиком поглощается текущим => элементы переносим в текущий, а сам запрос удаляем из списка
                        requests[c].AddItems(requests[i].Items);
                        // Изменяем размер запроса
                        if (requests[c].GrowUpBytesCount > 0)
                            requests[c].ChangeBytesToRead(requests[i].EndByteOffset + 1 - requests[c].Address);
                        requests.RemoveAt(i--);
                    }
                    else
                    {
                        // Запрос накладывается на текущий частично => урезаем или смещаем его, элементы переносим в текущий
                        for (int k = i; k < requests.Count && requests[k - 1].EndByteOffset >= requests[k].Address; k++ )
                        {
                            int newAddress = requests[k-1].Address + requests[k-1].BytesToRead + requests[k-1].GrowUpBytesCount;
                            for (int j = 0; j < requests[k].Items.Count && requests[k].Items[j].Address < newAddress; j++)
                            {
                                requests[k - 1].AddItem(requests[k].Items[j]);
                                if (requests[k].Items[j].EndByteOffset < newAddress)
                                    requests[k].Items.RemoveAt(j--);
                            }
                            if (requests[k].Items.Count == 0)
                                requests.RemoveAt(k--);
                            else
                                requests[k].ChangeAddress(newAddress);
                        }
                    }
                }
                // Вычисляем условие выхода
                bool ItemCoveredByRequests = (item.EndByteOffset <= requests[c].EndByteOffset);
                if (!ItemCoveredByRequests)
                {
                    // Элемент не покрывается запросами
                    c++;
                    if (c >= requests.Count || requests[c].Address > requests[c - 1].EndByteOffset + 1)
                    {
                        // Зпросов больше нет, либо между ними ести "дыра"
                        requests.Insert(c, new ReadRequest(requests[c - 1].EndByteOffset + 1, item.MemoryType, item.EndByteOffset - requests[c-1].EndByteOffset, this.GetRequestMinBytesToRead(), this.GetRequestMaxBytesToRead(item.MemoryType)));
                    }
                    requests[c].AddItem(item);
                }
                exit = ItemCoveredByRequests;
            }
            while (!exit);
        }
        /// <summary>
        /// Добавляет в очередь опороса отладчика объект записи данных
        /// </summary>
        /// <param name="Address">Адрес области памяти, к которой производится обращение</param>
        /// <param name="MemoryType">Тип области памяти, которой производится обращение</param>
        /// <param name="Count">Число байт, которое надо считать</param>
        /// <param name="Sender">Опрашиваемый объект</param>
        /// <param name="ProgressChangedCallback">Callback для получения информации о процессе опроса</param>
        /// <param name="CompleetingCallback">Callback для порлучения информации о завершении опроса</param>
        public void AddWriteItem(int Address, MemoryType MemoryType, byte[] Buffer, object Sender,
            ProceedingProgressChangedDelegate ProgressChangedCallback, ProceedingCompleetedDelegate CompleetingCallback)
        {
            lock (this.writeItems)
            {
                this.writeItems.Add(new RequestItem()
                {
                    Address = Address,
                    MemoryType = MemoryType,
                    Data = Buffer,
                    Sender = Sender,
                    ProgressChangedCallback = ProgressChangedCallback,
                    ProceedingCompletedCallback = CompleetingCallback
                });
            }
        }
        /// <summary>
        /// Обработка запросов на запись
        /// </summary>
        private void ProceedWriteItems(AbstractChannel channel)
        {
            for (int i = 0; i < this.writeItems.Count; i++)
            {
                RequestItem item = this.writeItems[i];
                // Определяем, сколько можно записать за один запрос
                int packetSize = channel.GetPacketSize(item.MemoryType);
              
                
                // Разбиваем буфер по запросам и записываем их в контроллер
                for (int j = 0; j < item.Data.Length; j += packetSize)
                {
                    int c = Math.Min(packetSize, item.Data.Length - j);
                    byte[] buffer = new byte[c];
                    Array.Copy(item.Data, j, buffer, 0, c);
                    try
                    {
                        channel.WriteToMemory(item.MemoryType, item.Address + j, buffer);
                        this.WritedRequestsCount++;
                    }
                    catch
                    {
                        this.ErrorWritedRequestsCount++;
                        item.Error = true;
                    }
                    // В случае необходимости, информируем о процессе записи 
                    if (item.ProgressChangedCallback != null)
                    {
                        // Устанавливаем параметры для выполнения события
                        object[] Params = new object[4];
                        Params[0] = item.ProgressChangedCallback;
                        Params[1] = item.Sender;
                        Params[2] = (double)(j + c) / (item.Data.Length);
                        Params[3] = null;
                        // Выплняем Callback в главном потоке
                        this.asyncOp.Post(this.exProgressChangedDelegate, Params);
                    }
                }
                if (item.ProceedingCompletedCallback != null)
                {
                    // Нужно проинформировать элемент о завершении получения данных
                    // Устанавливаем параметры для выполнения события
                    object[] Params = new object[4];
                    Params[0] = item.ProceedingCompletedCallback;
                    Params[1] = item.Sender;
                    Params[2] = null;
                    Params[3] = item.Error;
                    // Выполняем Callback в главном потоке
                    this.asyncOp.Post(this.exProceedingCompleetedDelegate, Params);
                }
                this.writeItems.Remove(item);
                i--;
            }
        }
        /// <summary>
        /// Перестраивает очереь запросов при изменении параметров отладчика
        /// </summary>
        private void RebuildRequests()
        {
            int lastMaxRequestSize = -1;
            // Формируем список запросов
            foreach(List<ReadRequest> requests in this.requestsByMemory.Values)
            {
                foreach (ReadRequest request in requests)
                {
                    lastMaxRequestSize = request.BytesToRead + request.GrowUpBytesCount;
                    break;
                }
                if (lastMaxRequestSize != -1)
                    break;
            }

            // Старый размер запроса не совпадает с новым - очищаем очередь запросов и формируем заново
            if (this.maxRequestSize != lastMaxRequestSize)
            {
                foreach (List<ReadRequest> requests in this.requestsByMemory.Values)
                {
                    while (requests.Count > 0)
                    {
                        while (requests[0].Items.Count > 0)
                        {
                            if (!this.readItems.Contains(requests[0].Items[0]))
                                this.readItems.Add(requests[0].Items[0]);
                            requests[0].Items.RemoveAt(0);
                        }
                        requests.RemoveAt(0);
                    }
                }
            }
            while (this.readItems.Count > 0)
            {
                this.AddReadItem(this.readItems[0]);
                this.readItems.RemoveAt(0);
            }
        }       
        /// <summary>
        /// Основной метод движка; выполняет обработку запросов пользователя
        /// </summary>
        /// <param name="port">Порт, через который происходит отсылка запросов</param>
        private void ProceedRequests(AbstractChannel channel)
        {
            Exception error = null; // используется в событии DebuggerEngineStatusChanged при остановке отладчика
            // Открытие порта
            try
            {
                channel.Open();
            }
            catch (Exception ex)
            {
                error = new Exception("Ошибка окрытия канала: " + ex.Message);
                goto end;
            }

            this.EngineStatus = DebuggerEngineStatus.Started;
            this.RaiseEngineStatusChangedEvent(new DebuggerEngineStatusChangedEventArgs(DebuggerEngineStatus.Started, null));
            // Получение типа подключенного контроллера
            string ControllerType = channel.ReadControllerType();;
           
            if (ControllerType == null)
            {
                // Контроллер либо вообще не найден, либо не соответствует настройкам отладчика
                error = new Exception("Контроллер не найден");
                goto end;
            }         

            this.maxRequestSize = 128;
            channel.BufferSize = this.maxRequestSize;            

            this.RebuildRequests();
            // Выполнение запросов
            Stopwatch watch = new Stopwatch(); // отслеживает время выполнения очереди запросов
            while (true)
            {
                bool requestsProceeded = false;
                watch.Start();
                try
                {
                    // Если требуется остановить отладчик, то выходим из цикла
                    if (this.EngineStatus == DebuggerEngineStatus.Stopping)
                        goto end;
                    // Если есть запросы на запись, то обрабатываем их
                    if (this.writeItems.Count != 0)
                        this.ProceedWriteItems(channel);
                    // Цикл по всем типам памяти, которые присутствуют в очереди
                    foreach (List<ReadRequest> requests in this.requestsByMemory.Values)
                    {
                        // Если требуется остановить отладчик, то выходим из цикла
                        if (this.EngineStatus == DebuggerEngineStatus.Stopping)
                            goto end;
                        // Цикл по всем запросам в текущей области памяти
                        for (int i = 0; i < requests.Count; i++)
                        {
                            //Console.WriteLine("/---------------" + DateTime.Now.Millisecond.ToString());
                            // Если есть запросы на запись, то обрабатываем их
                            if (this.writeItems.Count != 0)
                                this.ProceedWriteItems(channel);
                            // Обрабатываем запрос на чтение
                            ReadRequest request = requests[i];
                            if (this.lockingActive && request.MemoryType == this.lockingMemoryType &&
                                request.Address <= this.lockingAddress &&
                                request.EndByteOffset >= this.lockingAddress + this.lockingLength - 1)
                                continue;
                            requestsProceeded = true;
                            // Если требуется остановить отладчик, то выходим из цикла
                            if (this.EngineStatus == DebuggerEngineStatus.Stopping)
                                goto end;
                            byte[] response = channel.ReadFromMemory(request.MemoryType, request.Address, request.BytesToRead);;                           
                            // Учтснавливаем число прочитанных пакетов
                            if (response == null)
                            {
                                Console.WriteLine("-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-");
                                this.NearErrors = (this.NearErrors % 3) + 1;
                                if (this.NearErrors==3) this.ErrorReadedRequestsCount++;
                            }
                            else
                            {
                                this.NearErrors = 0;
                                this.ReadedRequestsCount++;
                            }
                            // Обработка всех элементов запроса
                            foreach (RequestItem item in request.Items)
                            {
                                int itemStart = Math.Max(item.Address, request.Address); // индекс первого байта элемента в даных запроса
                                int itemEnd = Math.Min(item.EndByteOffset, request.EndByteOffset); // индекс последнего байта элемента в данных запроса
                                if (response != null)
                                {
                                    // Добавляем в элемент считанные данные
                                    try
                                    {
                                        Array.Copy(response, itemStart - request.Address, item.Data, itemStart - item.Address, itemEnd - itemStart + 1);
                                    }
                                    catch
                                    {
                                        ;
                                    }
                                }
                                else
                                {
                                    item.Error = true;
                                }
                                if (item.ProgressChangedCallback != null)
                                {
                                    // Элемент надо информировать о процессе получения данных
                                    byte[] buffer = null;
                                    if (response != null)
                                    {
                                        // Получаем данные элемента, считанные за текущий запрос
                                        buffer = new byte[itemEnd - itemStart + 1];
                                        Array.Copy(response, itemStart - request.Address, buffer, 0, itemEnd - itemStart + 1);
                                    }
                                    // Устанавливаем параметры для выполнения события
                                    object[] Params = new object[4];
                                    Params[0] = item.ProgressChangedCallback;
                                    Params[1] = item.Sender;
                                    Params[2] = (double)(itemEnd - item.Address + 1) / (item.Data.Length);
                                    Params[3] = buffer;
                                    // Выплняем Callback в главном потоке
                                    this.asyncOp.Post(this.exProgressChangedDelegate, Params);
                                }
                                if (item.ProceedingCompletedCallback != null && item.EndByteOffset <= request.EndByteOffset)
                                {
                                    // Нужно проинформировать элемент о завершении получения данных
                                    // Устанавливаем параметры для выполнения события
                                    object[] Params = new object[4];
                                    Params[0] = item.ProceedingCompletedCallback;
                                    Params[1] = item.Sender;
                                    Params[2] = item.Data;
                                    Params[3] = item.Error;
                                    // Выполняем Callback в главном потоке
                                    this.asyncOp.Post(this.exProceedingCompleetedDelegate, Params);
                                    item.Error = false;
                                }
                            }
                            //Console.WriteLine("\\---------------" + DateTime.Now.Millisecond.ToString());
                        }
                    }
                }
                catch { }
                watch.Stop();
                if (!requestsProceeded)
                {
                    // Движок работает "в холостую"
                    this.RaiseRequestTimeChangedEvent(new EventArgs<int>(0));
                    Thread.Sleep(500);
                }
                else
                    // Возвращаем новое значение времени обработки всех запросов
                    this.RaiseRequestTimeChangedEvent(new EventArgs<int>((int)watch.ElapsedMilliseconds));
                watch.Reset();
            }
        end:
            // Завершение работы отладчика
            this.RaiseRequestTimeChangedEvent(new EventArgs<int>(0));
            channel.Close();
            this.EngineStatus = DebuggerEngineStatus.Stopped;
            this.RaiseEngineStatusChangedEvent(new DebuggerEngineStatusChangedEventArgs(DebuggerEngineStatus.Stopped, error));
            this.asyncOp.OperationCompleted();
        }
    }

    #region Enums
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
        Started
    }
    /// <summary>
    /// Тип запрос к отладчику
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// Заппрос на чтение данных с контроллера
        /// </summary>
        Read,
        /// <summary>
        /// Запрос на запись данных в контроллер
        /// </summary>
        Write
    }
    #endregion

    #region Delegates
    /// <summary>
    /// Делегат для получения промежуточного буфера и информировании о процессе завершения 
    /// при обрабоке элемента с большим объемом данных
    /// </summary>
    /// <param name="Buffer">При чтении данных - блок прочитанных данных, при записи - null</param>
    /// <param name="Percent">Доля завершения процесса обработки данных</param>
    /// <param name="Sender">Идентификатор объекта, чью данные обрабатывает отладчик</param>
    public delegate void ProceedingProgressChangedDelegate(object Sender, double Percent, byte[] Buffer);
    /// <summary>
    /// Делегат для информирования о завершении процесса обработки элемента
    /// </summary>
    /// <param name="Data">При чтении данных - считанные данные, при записи - null</param>
    /// <param name="Error">True, если произошла ошибка обработки данных</param>
    /// <param name="Sender">Идентификатор объекта, чью данные обрабатывает отладчик</param>
    public delegate void ProceedingCompleetedDelegate(object Sender, byte[] Data, bool Error);
    #endregion

    /// <summary>
    /// Аргумент события DebuggerEngine.EngineStatusChaged
    /// </summary>
    public class DebuggerEngineStatusChangedEventArgs: EventArgs
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
}
