using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace Reloader
{
    public class Downloader : Loader
    {
        private int _beginAdress;
        private int _size;
        private int _readBlockSize = 128;

        public Downloader()
            : base()
        {            
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            _port.ControllerAddress = 0;
            _port.Open();


           if (_mode == LoaderMode.DownloadProgram || _mode == LoaderMode.DownloadProgramAndConfiguration)
           {
               _beginAdress = 0x2000; //End of bootloader
               _size = 256 * 1024; //Controller flash size
               _progBuffer = new byte[_size - _beginAdress];


               int readCount = (_size - _beginAdress) / _readBlockSize;

               for (int i = 0; i < readCount; i++)
               {
                   int pers = (int)((float)100 / (float)readCount * i);
                   this.ReportProgress(pers, "Чтение программы из контроллера...");

                   Buffer.BlockCopy(_port.ReadFlash(_beginAdress, 128), 0, _progBuffer, i * _readBlockSize, _readBlockSize);

                   _beginAdress += _readBlockSize;

                   if (this.CancellationPending)
                   {
                       e.Cancel = true;
                       return;
                   }
               }            
           }

           if (_mode == LoaderMode.DownloadConfiguration || _mode == LoaderMode.DownloadProgramAndConfiguration)
           {
                _beginAdress = 0x7B00; //End of arhive region
                _size = 32 * 1024;     //Controller fram size              
                _confBuffer = new byte[_size - _beginAdress];

                int readCount = (_size - _beginAdress) / _readBlockSize;

                for (int i = 0; i < readCount; i++)
                {
                    int pers = (int)((float)100 / (float)readCount * i);
                    this.ReportProgress(pers, "Чтение конфигурации из контроллера...");

                    byte[] buf = _port.ReadFRAM(_beginAdress, 128);
                    Buffer.BlockCopy(buf, 0, _confBuffer, i * _readBlockSize, _readBlockSize);

                    _beginAdress += _readBlockSize;

                    if (this.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
           }                                     
        }        
    }
}
