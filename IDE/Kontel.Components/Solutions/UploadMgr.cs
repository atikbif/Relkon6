using System;
using System.Collections.Generic;
using System.Text;
using Kontel;
using System.Threading;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;

namespace Kontel.Relkon.Solutions
{
    /// <summary>
    /// Описывает событие UploadMgr.LoadToDeviceProgressChanged
    /// </summary>
    public delegate void UploadMgrProgressChangedEventHandler(object sender, UploadMgrProgressChangedEventArgs e);
    /// <summary>
    /// Осуществляет загрузку данных проекта в контроллер
    /// </summary>
    public abstract class UploadMgr
    {
        protected List<AsyncOperation> userStateToLifeTime = new List<AsyncOperation>();
        protected SerialPortDeviceSearcher deviceSearcher = null;
        private SendOrPostCallback onProgressReportDelegate;
        private SendOrPostCallback onCompletedDelegate;
        protected bool canceled = false;
        protected SerialPort485 devicePort = null;

        /// <summary>
        /// Периодически возникает в процессе загузки данных проекта в контроллер
        /// </summary>
        public event UploadMgrProgressChangedEventHandler ProgressChanged;
        /// <summary>
        /// Генерируется по завершении загрузки данных проекта в контроллер
        /// </summary>
        public event AsyncCompletedEventHandler UploadingCompleted;

        public UploadMgr()
        {
            this.deviceSearcher = new SerialPortDeviceSearcher(null, null);
            this.deviceSearcher.ProgressChanged += new ProgressChangedEventHandler(deviceSearcher_ProgressChanged);
            this.deviceSearcher.DeviceSearchCompleted += new DeviceSearchCompletedEventHandler(deviceSearcher_DeviceSearchCompleted);

            this.onCompletedDelegate = new SendOrPostCallback(this.RaiseUploadingCompletedEvent);
            this.onProgressReportDelegate = new SendOrPostCallback(this.RaiseProgressChangeEvent);
        }

        /// <summary>
        /// Показывает, выполняется ли процесс загрузки данных
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.userStateToLifeTime.Count != 0 || this.deviceSearcher.IsBusy;
            }
        }
        /// <summary>
        /// Запускает процесс загрузки данных
        /// </summary>
        public void StartUploading()
        {
            lock (this.userStateToLifeTime)
            {
                if (this.userStateToLifeTime.Count != 0)
                    throw new Exception("Процесс загрузки уже запущен");
            }
            this.devicePort = null;
            this.canceled = false;
            this.UpdateSeacherParams();
            this.deviceSearcher_ProgressChanged(this, new ProgressChangedEventArgs(0, null)); // чтобы при создании Message ProgressForm было непустым
            this.deviceSearcher.StartSearch();
        }
        /// <summary>
        /// Останавливает процесс загрузки данных
        /// </summary>
        public void StopUploading()
        {
            if (!this.deviceSearcher.IsBusy)            
                this.canceled = true;
                if (this.devicePort != null)                
                    lock (this.devicePort)                    
                        this.devicePort.Stop();                                              
            else
                this.deviceSearcher.StopSearch();
        }

        protected virtual void UpdateSeacherParams()
        {

        }

        protected void CompletionMethod(Exception exception, bool canceled)
        {
            AsyncCompletedEventArgs e = new AsyncCompletedEventArgs(exception, canceled, null);
            this.userStateToLifeTime[0].PostOperationCompleted(this.onCompletedDelegate, e);
        }

        protected void RaiseUploadingCompletedEvent(object state)
        {
            if (this.UploadingCompleted != null)
                this.UploadingCompleted(this, (AsyncCompletedEventArgs)state);
        }

        protected void ChangeProgressMethod(string StepName, int ProgressPercentage)
        {
            UploadMgrProgressChangedEventArgs e = new UploadMgrProgressChangedEventArgs(StepName, ProgressPercentage);
            this.userStateToLifeTime[0].Post(this.onProgressReportDelegate, e);
        }

        protected void RaiseProgressChangeEvent(object state)
        {
            if (this.ProgressChanged != null)
                this.ProgressChanged(this, (UploadMgrProgressChangedEventArgs)state);
        }

        protected abstract void deviceSearcher_DeviceSearchCompleted(object sender, DeviceSearchCompletedEventArgs e);

        protected abstract void deviceSearcher_ProgressChanged(object sender, ProgressChangedEventArgs e);
    }

    /// <summary>
    /// Аргумент события UploadMgr.ProgressChanged
    /// </summary>
    public class UploadMgrProgressChangedEventArgs : ProgressChangedEventArgs
    {
        private string stepName = ""; // имя текущего шага загрузки

        public UploadMgrProgressChangedEventArgs(string StepName, int ProgressPercentage)
            : base(ProgressPercentage, null)
        {
            this.stepName = StepName;
        }
        /// <summary>
        /// Возвращает имя текущего шага загрузки
        /// </summary>
        public string StepName
        {
            get
            {
                return this.stepName;
            }
        }
    }
    /// <summary>
    /// Исключение, возникающее, которое будет генерироваться тогда, когда надо будет остановить процесс загрузки
    /// </summary>
    public class StopUploadinException : Exception
    {
        
    }
}
