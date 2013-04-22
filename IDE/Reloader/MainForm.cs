using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kontel.Relkon;
using System.Threading;
using System.IO.Ports;
using System.IO;
using System.Xml;

namespace Reloader
{
    public partial class MainForm : Form
    {
        private const int _configurationSize = 32 * 1024 - 0x7B00;

        private Loader _loader = null;
        private BackgroundWorker _searcher = new BackgroundWorker();
        private LoaderMode _mode = LoaderMode.None;
        private Relkon4SerialPort _currentPort = null;
        private bool _mainFormClosing = false;

        private string _progBinPath = null;
        private byte[] _progBuf = null;
        private string _confBinPath = null;
        private byte[] _confBuf = null;

        public MainForm()
        {            
            InitializeComponent();
            _searcher.WorkerReportsProgress = true;
            _searcher.WorkerSupportsCancellation = true;
            _searcher.DoWork += new DoWorkEventHandler(_searcher_DoWork);
            _searcher.ProgressChanged += new ProgressChangedEventHandler(_searcher_ProgressChanged);
            _searcher.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_searcher_RunWorkerCompleted);                                    

            Text = "Kontel Reloader v1.0.0b";
        }

        void _searcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {               
               Utils.WarningMessage("Операция прервана пользователем!");                
            }
            else if (e.Error != null)
                MessageBox.Show(e.Error.Message);
            else if (_currentPort != null)
            {
                _loader.ProgressChanged += new ProgressChangedEventHandler(_loader_ProgressChanged);
                _loader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_loader_RunWorkerCompleted);
                _loader.Start(_currentPort, _mode);
            }            

            GlobProcProgressDefault();
            UpdateControlStatus();
        }

        void _searcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            GlobProcProgress((string)e.UserState, e.ProgressPercentage, true);
        }

        void _searcher_DoWork(object sender, DoWorkEventArgs e)
        {
            _currentPort = new Relkon4SerialPort((string)e.Argument, 19200, ProtocolType.RC51BIN);

            string pattern = "relkon";
            string bootPattern = "boot"; 
            byte[] request = new byte[] { 0x00, 0xA0 };
            bool searchingStopped = false;
            int[] baudRates = new int[] { 19200, 115200, 57600, 38400, 9600, 4800 };
            ProtocolType[] protocols = new ProtocolType[] { ProtocolType.RC51BIN, ProtocolType.RC51ASCII };

            int totalProgress = baudRates.Length * protocols.Length;

            for (int i = 0; i < protocols.Length && !searchingStopped; i++)
            {
                _currentPort.Protocol = protocols[i];
                for (int j = 0; j < baudRates.Length && !searchingStopped; j++)
                {
                    _currentPort.BaudRate = baudRates[j];

                    if (_searcher.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    try
                    {
                        _currentPort.Open();
                        byte[] res = _currentPort.SendRequest(request, pattern.Length, 2);
                        _currentPort.DiscardInBuffer();
                        if (res != null && Encoding.ASCII.GetString(res).ToLower().Contains(pattern.ToLower())
                                        || Encoding.ASCII.GetString(res).ToLower().Contains(bootPattern.ToLower()))
                        {
                            return;
                        }
                    }
                    catch { }
                    finally
                    {
                        _currentPort.Close();
                    }
                    _searcher.ReportProgress((int)((100 / totalProgress) * (i * j + j)), "Поиск параметров контроллера...");
                }
            }

            _currentPort = null;
        }

        void _loader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                if (_mainFormClosing)
                {                    
                    this._loader = null;
                    this.Close();
                    return;
                }
                else
                    Utils.WarningMessage("Операция прервана пользователем!");
            }
            else if (e.Error != null)
                MessageBox.Show(e.Error.Message);
            else
            {
                if (_mode == LoaderMode.DownloadProgram || _mode == LoaderMode.DownloadProgramAndConfiguration)
                {
                    _progBuf = _loader.ProgramBinBuffer;
                    lProgram.Text = "Не сохранён";
                    lProgram.ForeColor = Color.SteelBlue;
                }
                if (_mode == LoaderMode.DownloadConfiguration || _mode == LoaderMode.DownloadProgramAndConfiguration)
                {
                    _confBuf = _loader.ConfigurationBinBuffer;
                    lConfig.Text = "Не сохранён";
                    lConfig.ForeColor = Color.SteelBlue;
                }
            }

           
            _loader.ProgressChanged -= new ProgressChangedEventHandler(_loader_ProgressChanged);
            _loader.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_loader_RunWorkerCompleted);
            

            _mode = LoaderMode.None;

            GlobProcProgressDefault();
            UpdateControlStatus();
        }


        void _loader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_loader != null)
                GlobProcProgress((string)e.UserState, e.ProgressPercentage, true);
        }        

        private void mmiExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ssStopBtn_MouseDown(object sender, MouseEventArgs e)
        {
            if (_loader != null && _loader.IsBusy)
                _loader.Abort();
            if (_searcher.IsBusy)
                _searcher.CancelAsync();              
        }

        private void GlobProcProgress(string status, int pers, bool aborted)
        {            
            if (pers < 0)
            {
                ssLabel.Text = "Готово";
                ssProgressBar.Value = 0;
                ssProgressBar.Visible = false;
                ssStopBtn.Visible = false;
            }
            else
            {
                ssStopBtn.Enabled = aborted;
                ssProgressBar.Visible = true;
                ssStopBtn.Visible = true;
                pers = pers < 100 ? pers : 100;
                ssProgressBar.Value = pers;
                ssLabel.Text = status + ' ' + pers.ToString() + '%';                    
            }                        
        }

        private void GlobProcProgressDefault()
        {
            GlobProcProgress("Готово", -1, false);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {                 
            _loader = new Uploader();
            _mode = LoaderMode.UploadProgramAndConfiguration;
            StartLoader();                          
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            _loader = new Downloader();
            _mode = LoaderMode.DownloadProgramAndConfiguration;
            StartLoader();                                 
        }


        private void StartLoader()
        {          
            if (_mode == LoaderMode.DownloadProgramAndConfiguration)
            {
                if (cBUseProgram.Checked && cBUseConfig.Checked)
                    _mode = LoaderMode.DownloadProgramAndConfiguration;
                else
                {
                    if (cBUseProgram.Checked)
                        _mode = LoaderMode.DownloadProgram;
                    else
                        _mode = LoaderMode.DownloadConfiguration;
                }
            }
            else //_mode = LoaderMode.UploadProgramAndConfiguration
            {
                bool b1 = cBUseProgram.Checked && _progBuf != null;
                bool b2 = cBUseConfig.Checked && _confBuf != null;

                if (b1 && b2)
                {
                    _mode = LoaderMode.UploadProgramAndConfiguration;
                    _loader.ProgramBinBuffer = _progBuf;
                    _loader.ConfigurationBinBuffer = _confBuf;
                }
                else
                {
                    if (b1)
                    {
                        _loader.ProgramBinBuffer = _progBuf;
                        _mode = LoaderMode.UploadProgram;
                    }
                    else
                    {
                        _loader.ConfigurationBinBuffer = _confBuf;
                        _mode = LoaderMode.UploadConfiguration;
                    }
                }
            }                        

            mmiSaveProgramAs.Enabled = false;
            mmiSaveConfigurationAs.Enabled = false;
            mmiUpload.Enabled = btnUpload.Enabled = false;
            mmiDownload.Enabled = btnDownload.Enabled = false;
            mmiClearBuffer.Enabled = false;
            cBPortNumber.Enabled = false;

            byte[] req = null;
            if (_currentPort != null)
            {
                try
                {
                    _currentPort.Open();
                    req = _currentPort.SendRequest(new byte[] { 0x00, 0xA0 }, 2, 2);
                }
                finally
                {
                    _currentPort.Close();
                }
            }
          
            if (req != null)
            {
                _loader.ProgressChanged += new ProgressChangedEventHandler(_loader_ProgressChanged);
                _loader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_loader_RunWorkerCompleted);
                _loader.Start(_currentPort, _mode);
            }
            else            
                _searcher.RunWorkerAsync(cBPortNumber.SelectedItem.ToString());                            
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                if (_loader != null && _loader.IsBusy)
                    _loader.Abort();
                if (_searcher.IsBusy)
                    _searcher.CancelAsync();
            }
        }
             
        private void ClearProgrammBuffer()
        {
            _progBuf = null;
            _progBinPath = null;
            lProgram.Text = "Пусто";
            lProgram.ForeColor = Color.Red;
            toolTip.RemoveAll();
            if (_confBinPath != null)
                toolTip.SetToolTip(lConfig, _confBinPath);
        }

        private void ClearConfigurationBuffer()
        {
            _confBuf = null;
            _confBinPath = null;
            lConfig.Text = "Пусто";
            lConfig.ForeColor = Color.Red;
            toolTip.RemoveAll();
            if (_progBinPath != null)
                toolTip.SetToolTip(lProgram, _progBinPath);
        }


        private string CropFileName(string name)
        {
            if (name.Length > 28)
                name = name.Remove(28, name.Length - 28).Trim() + "...";            
            return name;
        }

        private void cBUseProgram_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlStatus();
        }

        private void cBUseConfig_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlStatus();
        }


        private void UpdateControlStatus()
        {
            mmiSaveProgramAs.Enabled = _progBuf != null;
            mmiSaveConfigurationAs.Enabled = _confBuf != null;

            bool b1 = _progBuf != null && cBUseProgram.Checked;
            bool b2 = _confBuf != null && cBUseConfig.Checked;

            mmiUpload.Enabled = btnUpload.Enabled = (b1 || b2) && cBPortNumber.SelectedItem != null;
            mmiDownload.Enabled = btnDownload.Enabled = (cBUseProgram.Checked || cBUseConfig.Checked) && cBPortNumber.SelectedItem != null;
            mmiClearBuffer.Enabled = _progBuf != null || _confBuf != null;

            cBPortNumber.Enabled = true;
        }

        private void mmiClearBuffer_Click(object sender, EventArgs e)
        {
            ClearProgrammBuffer();
            ClearConfigurationBuffer();            
            UpdateControlStatus();
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (_loader != null && _loader.IsBusy)
            {
                _mainFormClosing = true;
                _loader.CancelAsync();
                e.Cancel = true;
            }
            else
                base.OnClosing(e);
        }

        #region Open/Save
        private void mmiSaveConfigurationAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Файлы двоичных данных (*.bin)|*.bin";
            sfd.Title = "Сохранение файла конфигурации...";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(sfd.FileName, FileMode.Create);
                    fs.Write(_confBuf, 0, _confBuf.Length);
                    lConfig.Text = CropFileName(Path.GetFileName(sfd.FileName));
                    lConfig.ForeColor = Color.Green;
                    toolTip.SetToolTip(lConfig, sfd.FileName);                                                
                }
                catch(Exception ex)
                {                   
                    lConfig.Text = "Не сохранён";
                    lConfig.ForeColor = Color.SteelBlue;
                    Utils.ErrorMessage("Не возможно сохранить файл файл:" + Environment.NewLine +
                                                        sfd.FileName + Environment.NewLine +
                                                        "Оригинальный код ошибки: " + ex.Message + Environment.NewLine +
                                                        "StackTrace: " + ex.StackTrace);
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
                UpdateControlStatus();                 
            }
        }


        
        private void mmiSaveProgramAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Файлы двоичных данных (*.bin)|*.bin";
            sfd.Title = "Сохранение файла программы...";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(sfd.FileName, FileMode.Create);
                    fs.Write(_progBuf, 0, _progBuf.Length);
                    lProgram.Text = CropFileName(Path.GetFileName(sfd.FileName));
                    lProgram.ForeColor = Color.Green;
                    toolTip.SetToolTip(lProgram, sfd.FileName);                                                
                }
                catch(Exception ex)
                {
                    lProgram.Text = "Не сохранён";
                    lProgram.ForeColor = Color.SteelBlue;
                    Utils.ErrorMessage("Не возможно сохранить файл файл:" + Environment.NewLine +
                                                        sfd.FileName + Environment.NewLine +
                                                        "Оригинальный код ошибки: " + ex.Message + Environment.NewLine +
                                                        "StackTrace: " + ex.StackTrace);
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
                UpdateControlStatus();                 
            }
        
        }

        private void mmiOpenConfiguration_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файл проекта Relkon 6 (*.rp6)|*.rp6|Файлы двоичных данных (*.bin)|*.bin";
            ofd.Title = "Укажите файл конфигурации";
            if (ofd.ShowDialog() == DialogResult.OK)
            {                
                FileStream fs = null;
                try
                {
                    fs = new FileStream(ofd.FileName, FileMode.Open);

                    switch (Path.GetExtension(ofd.FileName))
                    {
                        case ".bin":                            
                            if (fs.Length != _configurationSize)
                                throw new Exception("Размер файла не соответствует размеру конфигурации контроллера!");

                            _confBuf = new byte[fs.Length];
                            fs.Read(_confBuf, 0, (int)fs.Length);
                            break;
                        case ".rp6":
                            XmlDocument doc = new XmlDocument();
                            doc.Load(fs);
                            _confBuf = RelkonProjectToBin(doc);
                            break;
                    }                    
                }
                catch(Exception ex)
                {
                    ClearProgrammBuffer();
                    Utils.ErrorMessage("Не возможно прочитать файл:" + Environment.NewLine +
                                                        ofd.FileName + Environment.NewLine +
                                                        "Оригинал исключения:" + ex.Message.ToString() + Environment.NewLine +
                                                        "Стэк:" + ex.StackTrace.ToString());
                    return;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }                     
            }  
         
            lConfig.Text = CropFileName(ofd.SafeFileName);
            lConfig.ForeColor = Color.Green;
            toolTip.SetToolTip(lConfig, ofd.FileName);
            cBUseConfig.Checked = true;
            UpdateControlStatus();
        }    
        
        private void mmiOpenProgram_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файлы двоичных данных (*.bin)|*.bin";
            ofd.Title = "Укажите файл программы";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(ofd.FileName, FileMode.Open);

                    if (fs.Length < 100 * 1024 || fs.Length > 256 * 1024)
                        throw new Exception("Размер файла не соответствует программе контроллера!");

                    _progBuf = new byte[fs.Length];
                    fs.Read(_progBuf, 0, (int)fs.Length);
                }
                catch
                {
                    ClearProgrammBuffer();
                    Utils.ErrorMessage("Не возможно прочитать файл:" + Environment.NewLine +
                                                        ofd.FileName + Environment.NewLine +
                                                        "или размер файла не соответствует программе контроллера!");
                    return;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();                   
                }

                lProgram.Text = CropFileName(ofd.SafeFileName);
                lProgram.ForeColor = Color.Green;
                toolTip.SetToolTip(lProgram, ofd.FileName);
                cBUseProgram.Checked = true;
                UpdateControlStatus();
            }
        }

        #endregion

        private void cBPortNumber_MouseDown(object sender, MouseEventArgs e)
        {
            cBPortNumber.Items.Clear();
            cBPortNumber.Items.AddRange(SerialPort.GetPortNames());
        }       

        private void cBPortNumber_DropDownClosed(object sender, EventArgs e)
        {
            UpdateControlStatus();
        }


        private byte[] RelkonProjectToBin(XmlDocument doc)
        {
            byte[] res = new byte[_configurationSize];

            int byteCount = 0;
            XmlNodeList nodes = doc.GetElementsByTagName("ControllerVar");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlAttributeCollection atts = nodes[i].Attributes;
                if (atts["Name"].Value == "EE" + byteCount)
                {
                    res[byteCount] = byte.Parse(atts["Value"].Value);
                    byteCount++;
                }
            }


            string adr = doc.GetElementsByTagName("ControllerProgramSolution")[0].Attributes["ControllerAddress"].Value;
            res[byteCount++] = byte.Parse(adr);

            nodes = doc.GetElementsByTagName("Uart");            

            string brChan1 = nodes[0].Attributes["BaudRate"].Value;
            switch (brChan1)
            {
                case "4800":
                    res[byteCount++] = 0;
                    break;
                case "9600":
                   res[byteCount++] = 1;
                    break;
                case "19200":
                    res[byteCount++] = 2;
                    break;
                case "38400":
                   res[byteCount++] = 3;
                    break;
                case "57600":
                   res[byteCount++] = 4;
                    break;
                case "115200":
                    res[byteCount++] = 5;
                    break;
            }

            string protChan1 = nodes[0].Attributes["Protocol"].Value;
            switch (protChan1)
            {
                case "RC51BIN":
                    res[byteCount++] = 0;
                    break;
                case "RC51ASCII":
                    res[byteCount++] = 1;
                    break;
            }
         
            string brChan2 = nodes[1].Attributes["BaudRate"].Value;
            switch (brChan2)
            {
                case "4800":
                    res[byteCount++] = 0;
                    break;
                case "9600":
                    res[byteCount++] = 1;
                    break;
                case "19200":
                    res[byteCount++] = 2;
                    break;
                case "38400":
                    res[byteCount++] = 3;
                    break;
                case "57600":
                    res[byteCount++] = 4;
                    break;
                case "115200":
                    res[byteCount++] = 5;
                    break;
            }

            string protChan2 = nodes[1].Attributes["Protocol"].Value;
            switch (protChan2)
            {
                case "RC51BIN":
                    res[byteCount++] = 0;
                    break;
                case "RC51ASCII":
                    res[byteCount++] = 1;
                    break;
            }

            byteCount++; //0 т.к. эмуляции нет
            byteCount += 6; //размер проекта (не нужен)

            XmlNode label = doc.GetElementsByTagName("Label")[0];
            byte[] bytes = Encoding.ASCII.GetBytes(label.InnerText);
            Array.Copy(bytes, 0, res, byteCount, bytes.Length);

            byteCount += 64;

            XmlNode ipAdress = doc.GetElementsByTagName("ControllerIPAdress")[0];
            bytes = Convert.FromBase64String(ipAdress.InnerText);
            Array.Copy(bytes, 0, res, byteCount, bytes.Length);

            bytes = Encoding.ASCII.GetBytes("Relkon 001");              
            Array.Copy(bytes, 0, res, 0x7FF5 - 0x7B00, bytes.Length);

            return res;
        }
    }
}
