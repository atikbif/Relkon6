using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Kontel.Relkon
{
    public sealed class SerialportChannel : AbstractChannel
    {
        private SerialPort _serialPort = null; // обеспечивает рабоу с COM-портом    


        public SerialportChannel(SerialportChannel channel)
        {
            _serialPort = channel.DirectPort;
            _relkonProtocolType = RelkonProtocolType;
        }

        public SerialportChannel(string portName, int baudRate, ProtocolType relkonPotocolType)
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRate;
            _serialPort.DtrEnable = true;
            _serialPort.RtsEnable = true;
            _relkonProtocolType = relkonPotocolType;
        }

        /// <summary>
        /// Возвращает или устанавливает имя рабочего порта
        /// </summary>
        public string PortName
        {
            get
            {
                return _serialPort.PortName;
            }
            set
            {
                _serialPort.PortName = value;
            }
        }

        /// <summary>
        /// Возвращает или устанавливает скорость обмена данными с портом
        /// </summary>
        public int BaudRate
        {
            get
            {
                return _serialPort.BaudRate;
            }
            set
            {
                _serialPort.BaudRate = value;
            }
        }


        public SerialPort DirectPort
        {
            get
            {
                return _serialPort;
            }
        }       
      
        /// <summary>
        /// Отсылает буфер на порт и читает с него ответ
        /// </summary>
        /// <param name="ExpectedResponseSize">Ожидаемый размер ответа</param>
        /// <param name="DelayFactor">Коэффицент задержки для достижения запроса контроллера</param>
        protected override byte[] Send(byte[] buffer, int ExpectedResponseSize)
        {
            _canceled = false;
            _serialPort.Write(buffer, 0, buffer.Length);
            int t = (int)Math.Round(1.5 * buffer.Length * 8 * 1000 / _serialPort.BaudRate); // время необходимое для передачи массива при текущей скорости порта
            int timeout = _minimalTimeout;
            if (_serialPort.BaudRate < 9600)
                timeout = 500;
            if (ExpectedResponseSize != -1)
                timeout = Math.Max(timeout, (int)Math.Round(1.5 * ExpectedResponseSize * 8 * 1000 / _serialPort.BaudRate + 1));
            Thread.Sleep(t * 2); // задержка на время, достаточное для достижения запросом контроллера умноженная на десять, что бы наверняка контроллер успел послать ответ
            bool finish = false;
            List<byte> res = new List<byte>();
            int DelayInterval = 1; // Интервал задержки между попытками чтения
            int idx = 0; // счетчик попыток чтения
            while (!finish && !_canceled)
            {
                int c = _serialPort.BytesToRead;
                if (c == 0)
                {
                    if ((++idx) * DelayInterval < timeout)
                        Thread.Sleep(DelayInterval);
                    else
                        finish = true;
                }
                else
                {
                    byte[] tmp = new byte[c];
                    _serialPort.Read(tmp, 0, c);
                    res.AddRange(tmp);
                    if (this.ResponseRecieved(res.ToArray(), ExpectedResponseSize))
                    {
                        if (_relkonProtocolType == ProtocolType.RC51BIN && (res[res.Count - 2] == 0 || res[res.Count - 1] == 0))
                        {
                            Thread.Sleep(10);
                            if (_serialPort.BytesToRead != 0)
                            {
                                tmp = new byte[_serialPort.BytesToRead];
                                _serialPort.Read(tmp, 0, tmp.Length);
                                res.AddRange(tmp);
                                if (this.ResponseRecieved(res.ToArray(), ExpectedResponseSize))
                                    finish = true;
                            }
                            else
                                finish = true;
                        }
                        else
                            finish = true;
                    }
                    idx = 0;
                }
            }
            _serialPort.DiscardInBuffer();
            return (res.Count == 0) ? null : res.ToArray();
        }

        /// <summary>
        /// Открывает порт
        /// </summary>
        public override void Open()
        {
            if (!_serialPort.IsOpen)
                _serialPort.Open();
        }
        /// <summary>
        /// Закрывает порт
        /// </summary>
        public override void Close()
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();
        }

        public override bool IsOpen()
        {
            return _serialPort.IsOpen;
        }

        /// <summary>
        /// Очищает входной буфер порта
        /// </summary>
        public void DiscardInBuffer()
        {
            _serialPort.DiscardInBuffer();
        }   
    }
}
