using System;
using System.Collections.Generic;
using System.Text;
using Kontel.Relkon;
using System.ComponentModel;
using System.Threading;
using System.IO.Ports;
using System.IO;

namespace Reloader
{
    public class Uploader : Loader
    {                
        public Uploader() : base()
        {
              
        }
     
        protected override void OnDoWork(DoWorkEventArgs e)
        {            
            int oldBaudRate = ((SerialportChannel)_port).BaudRate;
            ProtocolType oldProtocol = _port.RelkonProtocolType;

            _port.Open();
            _port.ControllerAddress = 0;

            if (_mode == LoaderMode.UploadProgram || _mode == LoaderMode.UploadProgramAndConfiguration)
            {               
                if (this.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                byte[] req = _port.SendRequest(new byte[] { 0x00, 0xA0 }, 2);

                string s = "";

                if (req != null)
                    s = Encoding.ASCII.GetString(req);

                bool bootLoaderMode = !s.ToLower().Contains("relkon 6");

                if (!bootLoaderMode)
                {
                    if (this.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                  
                    _port.Open();

                    this.ReportProgress(0, "Перевод контроллера в режим загрузчика...");
                    //Перетераем первый символ слова для загрузчика     
                    _port.WriteByteToMemory(MemoryType.FRAM, 0x7FF5, 0x00);
                    _port.ResetController();
                    this.ReportProgress(50, "Перевод контроллера в режим загрузчика...");
                    _port.Close();

                    if (this.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    Thread.Sleep(500);
                }

                if (this.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                ((SerialportChannel)_port).BaudRate = 115200;
                _port.RelkonProtocolType = ProtocolType.RC51BIN;
                _port.Open();

                this.ReportProgress(100, "Перевод контроллера в режим загрузчика...");

                ((SerialportChannel)_port).DirectPort.Write("1");                      
                waitfor(((SerialportChannel)_port).DirectPort, 'C');
                             

                byte[] bytes2 = new byte[_progBuffer.Length - 1];
                Buffer.BlockCopy(_progBuffer, 0, bytes2, 0, _progBuffer.Length - 1); 

                ushort packetnum = 0;

                YModemPacket initpacket = new YModemPacket();
                initpacket.isinit = true;
                initpacket.packetnum = packetnum;
                initpacket.filename = "program.bin";
                initpacket.filelength = bytes2.Length;
                initpacket.longpacket = false;               

                initpacket.createPacket();
                ((SerialportChannel)_port).DirectPort.Write(initpacket.packet, 0, initpacket.packet.Length);

                waitforack(((SerialportChannel)_port).DirectPort);
                waitfor(((SerialportChannel)_port).DirectPort, 'C');


                using (MemoryStream ms2 = new MemoryStream(bytes2))
                {
                    using (BinaryReader br2 = new BinaryReader(ms2))
                    {
                        while (ms2.Position != ms2.Length)
                        {
                            if (this.CancellationPending)
                            {
                                e.Cancel = true;
                                return;
                            }

                            int pers = (int)((float)100 / (float)ms2.Length * ms2.Position);
                            this.ReportProgress(pers, "Загрузка программы в контроллер...");

                            packetnum++;
                            YModemPacket sendPacket = new YModemPacket();
                            sendPacket.packetnum = packetnum;
                            sendPacket.longpacket = true;
                            sendPacket.isinit = false;
                            sendPacket.data = br2.ReadBytes(1024);
                            sendPacket.createPacket();
                            ((SerialportChannel)_port).DirectPort.Write(sendPacket.packet, 0, sendPacket.packet.Length);
                            waitforack(((SerialportChannel)_port).DirectPort);
                        }
                    }
                }
                this.ReportProgress(100, "Загрузка программы в контроллер...");
                sendEndOftransmision(((SerialportChannel)_port).DirectPort);
                waitforack(((SerialportChannel)_port).DirectPort);

                this.ReportProgress(100, "Перезапуск контроллера...");
                Thread.Sleep(2000);
                ((SerialportChannel)_port).DiscardInBuffer();
                _port.Close();
            }

            if (this.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            if (_mode == LoaderMode.UploadConfiguration || _mode == LoaderMode.UploadProgramAndConfiguration)
            {
                _port.Open();

                int beginAdress = 0x7B00; //End of arhive region
                int writeBlockSize = 128;
                int size = _confBuffer.Length;                

                int writeCount = _confBuffer.Length / writeBlockSize;

                for (int i = 0; i < writeCount; i++)
                {
                    if (this.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    int pers = (int)((float)100 / (float)writeCount * i);
                    this.ReportProgress(pers, "Загрузка конфигурации в контроллер...");

                    byte[] buf = new byte[128];
                    Buffer.BlockCopy(_confBuffer, writeBlockSize * i, buf, 0, buf.Length);
                    _port.WriteToMemory(MemoryType.FRAM, beginAdress, buf);
                    beginAdress += writeBlockSize;
                }

                this.ReportProgress(0, "Перезагрузка контроллера...");
                Thread.Sleep(2000);
                _port.ResetController();
                this.ReportProgress(100, "Перезагрузка контроллера...");
            }                                                          
        }          

        private void sendEndOftransmision(SerialPort sp)
        {
            sp.Write(new byte[] { (byte)0x04 }, 0, 1);
            YModemPacket endpacket = new YModemPacket();
            endpacket.isend = true;
            endpacket.longpacket = false;
            endpacket.packetnum = 0;
            endpacket.data = new byte[128];
            endpacket.createPacket();
            sp.Write(endpacket.packet, 0, endpacket.packet.Length);
            return;
        }

        private void waitforack(SerialPort sp)
        {
            int c;
            do
            {
                for (int i = 0; i < 200; i++)
                {
                    if (sp.BytesToRead > 0)
                        break;
                    Thread.Sleep(20);
                }
                if (sp.BytesToRead == 0)
                    throw new Exception("Обмен данными с контроллером не возможен!");

                c = sp.ReadByte();
            }
            while (c != (byte)0x06);
        }

        private void waitfor(SerialPort sp, char p)
        {
            int c;
            do
            {
                for (int i = 0; i < 200; i++)
                {
                    if (sp.BytesToRead > 0)
                        break;
                    Thread.Sleep(20);
                }
                if (sp.BytesToRead == 0)
                    throw new Exception("Обмен данными с контроллером не возможен!");

                c = sp.ReadChar();
            }
            while (c != (int)p);
        }
    }    
}
            