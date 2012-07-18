using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Kontel.Relkon.Solutions;
using System.Windows.Forms;
using Kontel.Relkon.Classes;
using System.Data;
using System.Drawing;
using System.IO;
using Kontel.Relkon;
using System.Text.RegularExpressions;

namespace Kontel.Relkon.Components.Documents
{
    public sealed partial class STM32F107PropertiesTabbedDocument : PropertiesTabbedDocument
    {
        private string _lastValideMac;

        public STM32F107PropertiesTabbedDocument(STM32F107Solution Solution) : 
            base(Solution)
        {
            InitializeComponent();
            this.InitializeDocument();
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SelectedIndex = 1;
            //this.tabControl1.SelectedIndex = 3;
            this.initialized = true;
        }
  
        private void InitializeDocument()
        {
            this.FillProtocolsList();
            this.FillSpeedsList();
            this.LoadFromSolution();
        }

        protected override void LoadFromSolution()
        {
            // Общие настройки
            this.tbProjectName.Text = this.Solution.Name;
            this.tbProcessor.Text = this.ControllerSolution.ProcessorParams.Type.ToString();
            this.nudControllerAddress.Value = this.ControllerSolution.ControllerAddress;                        
            this.nudProgrammingControllerAddress.Value = this.ControllerSolution.SearchedControllerAddress;
            this.standartEmbeddedVarsEditor1.Solution = this.ControllerSolution;
            // Настройки COM-портов
            // COM0
            this.ddlProtocolsList0.SelectedItem = this.ControllerSolution.Uarts[0].Protocol;
            this.ddlSpeedsList0.SelectedItem = this.ControllerSolution.Uarts[0].BaudRate;  
            // COM1
            this.ddlProtocolsList3.SelectedItem = this.ControllerSolution.Uarts[1].Protocol;
            this.ddlSpeedsList3.SelectedItem = this.ControllerSolution.Uarts[1].BaudRate;

            this.nudControllerIPAdress1byte.Value = this.ControllerSolution.ControllerIPAdress[0];
            this.nudControllerIPAdress2byte.Value = this.ControllerSolution.ControllerIPAdress[1];
            this.nudControllerIPAdress3byte.Value = this.ControllerSolution.ControllerIPAdress[2];
            this.nudControllerIPAdress4byte.Value = this.ControllerSolution.ControllerIPAdress[3];

            this.nudControllerIPMask1byte.Value = this.ControllerSolution.ControllerIPMask[0];
            this.nudControllerIPMask2byte.Value = this.ControllerSolution.ControllerIPMask[1];
            this.nudControllerIPMask3byte.Value = this.ControllerSolution.ControllerIPMask[2];
            this.nudControllerIPMask4byte.Value = this.ControllerSolution.ControllerIPMask[3];

            this.nudControllerGateway1byte.Value = this.ControllerSolution.ControllerIPGateway[0];
            this.nudControllerGateway2byte.Value = this.ControllerSolution.ControllerIPGateway[1];
            this.nudControllerGateway3byte.Value = this.ControllerSolution.ControllerIPGateway[2];
            this.nudControllerGateway4byte.Value = this.ControllerSolution.ControllerIPGateway[3];

            this.tBMacAdress.Text = this.ControllerSolution.ControllerMACAdress;

            this.cBEnablePult.Checked = this.ControllerSolution.PultEnable;
            this.cBEnableSD.Checked = this.ControllerSolution.SDEnable;

            _lastValideMac = this.ControllerSolution.ControllerMACAdress;
        }

        protected override bool LoadToSolution()
        {
            this.standartEmbeddedVarsEditor1.EndEdit();
            //int TotalBuffersSize = 0;
            // Общие настройки
            this.ControllerSolution.ControllerAddress = (int)this.nudControllerAddress.Value;         
            this.ControllerSolution.SearchedControllerAddress = (int)this.nudProgrammingControllerAddress.Value;
            // Настройки COM-портов
            // COM0
            this.ControllerSolution.Uarts[0].Protocol = (ProtocolType)this.ddlProtocolsList0.SelectedItem;
            this.ControllerSolution.Uarts[0].BaudRate = (int)this.ddlSpeedsList0.SelectedItem;
           
            // COM1
            this.ControllerSolution.Uarts[1].Protocol = (ProtocolType)this.ddlProtocolsList3.SelectedItem;
            this.ControllerSolution.Uarts[1].BaudRate = (int)this.ddlSpeedsList3.SelectedItem;

            this.ControllerSolution.ControllerIPAdress[0] = (byte)this.nudControllerIPAdress1byte.Value;
            this.ControllerSolution.ControllerIPAdress[1] = (byte)this.nudControllerIPAdress2byte.Value;
            this.ControllerSolution.ControllerIPAdress[2] = (byte)this.nudControllerIPAdress3byte.Value;
            this.ControllerSolution.ControllerIPAdress[3] = (byte)this.nudControllerIPAdress4byte.Value;

            this.ControllerSolution.ControllerIPMask[0] = (byte)this.nudControllerIPMask1byte.Value;
            this.ControllerSolution.ControllerIPMask[1] = (byte)this.nudControllerIPMask2byte.Value;
            this.ControllerSolution.ControllerIPMask[2] = (byte)this.nudControllerIPMask3byte.Value;
            this.ControllerSolution.ControllerIPMask[3] = (byte)this.nudControllerIPMask4byte.Value;

            this.ControllerSolution.ControllerIPGateway[0]= (byte)this.nudControllerGateway1byte.Value;
            this.ControllerSolution.ControllerIPGateway[1] = (byte)this.nudControllerGateway2byte.Value;
            this.ControllerSolution.ControllerIPGateway[2] = (byte)this.nudControllerGateway3byte.Value;
            this.ControllerSolution.ControllerIPGateway[3] = (byte)this.nudControllerGateway4byte.Value;

            this.ControllerSolution.ControllerMACAdress = this.tBMacAdress.Text;

            this.ControllerSolution.PultEnable = this.cBEnablePult.Checked;
            this.ControllerSolution.SDEnable = this.cBEnableSD.Checked;

            return true;
        }

        public override void UpdateEmbeddedVarsValues()
        {
            this.standartEmbeddedVarsEditor1.Update();
        }
        /// <summary>
        /// Устаналивает фокус ввода на компоненте, вызвавшем ошибку
        /// и отображает сообщение об ошибке
        /// </summary>
        private void SetError(Control ErrorControl, string Message)
        {
            this.tabControl1.TabPages[1].Focus();
            ErrorControl.Focus();
            Utils.ErrorMessage(Message);
        }
        /// <summary>
        /// Возвращает проект типа RelkonSolution, свойства которого отображает документ
        /// </summary>
        private STM32F107Solution ControllerSolution
        {
            get
            {
                return (STM32F107Solution)this.Solution;
            }
        }
        /// <summary>
        /// Заполняет списки протоколов для всех портов
        /// </summary>
        private void FillProtocolsList()
        {
            object[] protocols = { ProtocolType.RC51ASCII, ProtocolType.RC51BIN };
            this.ddlProtocolsList0.Items.AddRange(protocols);
            this.ddlProtocolsList3.Items.AddRange(protocols);
        }
        /// <summary>
        /// Заполняет списки скоростей всех портов
        /// </summary>
        private void FillSpeedsList()
        {
            object[] speeds = { 115200, 57600, 38400, 19200, 9600, 4800 };
            this.ddlSpeedsList0.Items.AddRange(speeds);
            this.ddlSpeedsList3.Items.AddRange(speeds);
        }       

        private void tabControl1_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Property_Changed(object sender, EventArgs e)
        {
            if (this.initialized)
                this.DocumentModified = true;
        }
       

        private bool IsValidMacAdress(string s)
        {
            return Regex.IsMatch(s, @"[0-9a-fA-F]{12}");
        }    

        private void tBMacAdress_ModifiedChanged(object sender, EventArgs e)
        {
          
        }

        private void tBMacAdress_TextChanged(object sender, EventArgs e)
        {
            //tBMacAdress.e
            //if (tBMacAdress.Text.Length == 12 && IsValidMacAdress(tBMacAdress.Text))
            //{
            //    _lastValideMac = tBMacAdress.Text;
            //}
            //else
            //    tBMacAdress.Text = _lastValideMac;
        }
    }
}
