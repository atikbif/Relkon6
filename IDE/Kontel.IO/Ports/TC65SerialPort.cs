using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon
{
    public sealed class TC65SerialPort: SerialPort485
    {
        public TC65SerialPort(string PortName, int BaudeRate, ProtocolType Protocol)
            : base(PortName, BaudeRate, Protocol)
        {

        }

        public TC65SerialPort(SerialPort485 Port)
            : base(Port.PortName, Port.BaudRate, Port.Protocol)
        {
            port.Close();
        }
        /// <summary>
        /// Переводит модем в режим настройки
        /// </summary>
        /// <param name="RemoteAccess">Определяет, осуществляется ли доступ к модему непосредственно через COM-порт или удаленно, через GSM-канал</param>
        public void SetEditModeOn(bool RemoteAccess)
        {
            List<byte> request = new List<byte>();
            request.Add((byte)0xFF);
            request.Add((byte)0x41);
            request.AddRange(AppliedMath.IntToBytes(115200));
            request.Add(2);
            byte[] response = this.SendRequest(request.ToArray(), 8 + 2, 2);
            if (response == null || response.Length < 6)
                throw new Exception("Ошибка перевода модема в режим программирования: " + this.errorType);
            int BaudRate = AppliedMath.BytesToInt(Utils.GetSubArray<byte>(response, 1, 4));
            if (!RemoteAccess && this.BaudRate != BaudRate)
            {
                this.port.Close();
                this.BaudRate = BaudRate;
                this.Open();
            }
            this.protocol = (response[5] == 2 ? ProtocolType.RC51BIN : ProtocolType.RC51ASCII);
        }
        /// <summary>
        /// Стирает данные проекта модема
        /// </summary>
        public void ClearProjectData()
        {
            if (this.SendRequest(new byte[] { 0xFF, 0x43 }, 2 + 2, 2) == null)
                throw new Exception("Сбой очистки настроек: " + this.errorType);
        }
        /// <summary>
        /// Записывает данные проекта в модем
        /// </summary>
        /// <param name="data">Собственно данные проекта</param>
        /// <param name="ProgressStateChangedEvent">Функция отображения состояния загрузки, в качестве параметра в функцию передается доля загруженных данных</param>
        public void WriteProjectData(byte[] data)
        {
            int c = 128; // размер одного блока данных
            for (int i = 0; i < data.Length && !this.canceled; i += c)
            {
                // Запись данных
                byte[] buffer = Utils.GetSubArray<byte>(data, i, Math.Min(data.Length - i, c));
                List<byte> request = new List<byte>();
                request.Add((byte)0xFF);
                request.Add((byte)0x44);
                request.Add((byte)AppliedMath.Hi(buffer.Length));
                request.Add((byte)AppliedMath.Low(buffer.Length));
                request.AddRange(buffer);
                if (this.SendRequest(request.ToArray(), 2 + 2, 2) == null)
                    throw new Exception("Сбой записи настроек: " + this.errorType);
                // Верификация данных
                request.Clear();
                request.Add((byte)0xFF);
                request.Add((byte)0x45);
                request.Add((byte)AppliedMath.Hi(i));
                request.Add((byte)AppliedMath.Low(i));
                request.Add((byte)AppliedMath.Hi(buffer.Length));
                request.Add((byte)AppliedMath.Low(buffer.Length));
                byte[] response = this.SendRequest(request.ToArray(), buffer.Length + 1 + 2, 2);
                if (response == null || !Utils.CompareArrays(Utils.GetSubArray<byte>(response, 1), buffer))
                    throw new Exception("Сбой верификации");
                this.RaiseProgressChangedEvent(1.0 * i / data.Length);
            }
        }
        /// <summary>
        /// Выводи модем из режима редактирования
        /// </summary>
        /// <param name="RemoteAccess">Определяет, осуществляется ли доступ к модему непосредственно через COM-порт или удаленно, через GSM-канал</param>
        public void SetEditModeOff(bool RemoteAccess)
        {
            this.minimalTimeout = RemoteAccess ? 25000 : 45000;
            if (this.SendRequest(new byte[] { 0xFF, 0x42 }, 2 + 2, 2) == null && !this.canceled)
                throw new Exception("Сбой при выводе модема из режима программирования");
        }
    }
}
