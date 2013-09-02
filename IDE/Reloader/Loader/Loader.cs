using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Kontel.Relkon;

namespace Reloader
{
    public class Loader : BackgroundWorker
    {
        protected AbstractChannel _port = null;
        protected LoaderMode _mode = LoaderMode.None;
        protected byte[] _progBuffer = null;
        protected byte[] _confBuffer = null;


        public byte[] ProgramBinBuffer
        {
            set { _progBuffer = value; }
            get { return _progBuffer; }
        }

        public byte[] ConfigurationBinBuffer
        {
            set { _confBuffer = value; }
            get { return _confBuffer; }
        }

        protected Loader()
        {
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
        }

        public void Start(AbstractChannel port, LoaderMode mode)
        {
            _port = port;
            _mode = mode;
            this.RunWorkerAsync();
        }

        public void Abort()
        {
            this.CancelAsync();
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            if (_port != null && _port.IsOpen())
                _port.Close();

            base.OnRunWorkerCompleted(e);
        }
    }

    public enum LoaderMode
    {
        None,
        UploadProgram, 
        DownloadProgram,
        UploadConfiguration,
        DownloadConfiguration,
        UploadProgramAndConfiguration,
        DownloadProgramAndConfiguration
    }
}
