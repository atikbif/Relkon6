using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Kontel.Relkon
{
    public sealed class EthernetChannel : AbstractChannel
    {
        private UdpClient _client;       
        private IPEndPoint _ip;//ip контроллера   
        private IPEndPoint _groupEP;   


        public EthernetChannel(string ip, int port, ProtocolType protocol)
        {            
            _ip = new IPEndPoint(IPAddress.Parse(ip), port);
            _relkonProtocolType = protocol;            
        }

        /// <summary>
        /// Отсылает буфер на порт и читает с него ответ
        /// </summary>
        /// <param name="ExpectedResponseSize">Ожидаемый размер ответа</param>
        protected override byte[] Send(byte[] buffer, int ExpectedResponseSize)
        {                     
            _client.Send(buffer, buffer.Length);

            for (int i = 0; i < _minimalTimeout / 10; i++)
            {
                int c = _client.Available;
                if (c == 0)
                    Thread.Sleep(10);
                else
                    return _client.Receive(ref _groupEP);                 
            }

            return null;                                       
        }       

        /// <summary>
        /// Открывает порт
        /// </summary>
        public override void Open()
        {
            _client = new UdpClient();            
            _client.Connect(_ip);          
            _groupEP = new IPEndPoint(System.Net.IPAddress.Any, _ip.Port);
           
        }
        /// <summary>
        /// Закрывает порт
        /// </summary>
        public override void Close()
        {
            if (_client != null)
                _client.Close();
        }

        public override bool IsOpen()
        {
            return false;
        }  
    }
}
