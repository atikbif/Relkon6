using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Kontel.Relkon
{
    public delegate void SimpleDelegate();
    public delegate void ParameterizedDelegate<T>(T value);
    /// <summary>
    /// Содержит несколько полезных в работе методов
    /// </summary>
    public sealed class Utils
    {
        /// <summary>
        /// Делает первую букву строки строчной
        /// </summary>
        public static string FirstLetterToLower(string s)
        {
            return (s.Substring(0, 1).ToLower() + s.Substring(1));
        }
        /// <summary>
        /// Возвращает окончание для для существительного мужского рода, которое 
        /// употребляется во множественном числе, в количестве Number
        /// </summary>
        /// <param name="Number"></param>
        public static string GetTermination(int Number)
        {
            string s = Number.ToString();
            char c = s[s.Length - 1];
            char cc = '2';
            if (s.Length > 1)
                cc = s[s.Length - 2];
            s = "";
            if (cc != '1' && ((c == '2' || c == '3' || c == '4')))
                s += "а";
            return s;
        }
        /// <summary>
        /// Возвращает подмассив указанного массива
        /// </summary>
        /// <param name="Array">Основной массив</param>
        /// <param name="Index">Индекс первого элемента подмассива в исходном массиве</param>
        /// <param name="Count">Число элементов подмассива; если оно больше числа оставшихся элементов массива, чтение идет до конца массива</param>
        /// <returns>Подмассив</returns>
        public static T[] GetSubArray<T>(T[] Source, int Index, int Count)
        {
            int c = (Source.Length >= Index + Count) ? Count : Source.Length - Index;
            T[] res = new T[c];
            Array.Copy(Source, Index, res, 0, c);
            //for (int i = 0; i < c; i++)
            //    res[i] = Source[i + Index];
            return res;
        }
        /// <summary>
        /// Возвращает подмассив указанного массива
        /// </summary>
        /// <param name="Array">Основной массив</param>
        /// <param name="Index">Индекс первого элемента подмассива в исходном массиве</param>
        /// <returns>Подмассив</returns>
        public static T[] GetSubArray<T>(T[] Array, int Index)
        {
            return Utils.GetSubArray<T>(Array, Index, Array.Length - Index);
        }
        /// <summary>
        /// Возвращает массив в обратном порядке
        /// </summary>
        public static T[] ReflectArray<T>(T[] Array)
        {
            T[] res = new T[Array.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = Array[Array.Length - i - 1];
            return res;
        }
        /// <summary>
        /// Сравнивает два массива; возвращает true в случае успеха
        /// </summary>
        public static bool CompareArrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Возвращает индекс первого вхождения любой из указанного набора строк в заданную сроку;
        /// в случае неудачи возвращает -1
        /// </summary>
        /// <param name="value">Строка, в которой производится поиск</param>
        /// <param name="anyOf">Массив строк, позиции которых ищутся</param>
        /// <param name="startIndex">Индекс, с которого начинается поиск</param>
        public static int GetIndexOfAnySubString(string value, string[] anyOf, int startIndex)
        {
            List<int> positions = new List<int>();
            for (int i = 0; i < anyOf.Length; i++)
            {
                int pos = value.IndexOf(anyOf[i], startIndex);
                if (pos > -1)
                    positions.Add(pos);
            }
            positions.Sort();
            return (positions.Count > 0 ? positions[0] : -1);
        }
        /// <summary>
        /// Возвращает индекс первого вхождения любой из указанного набора строк в заданную сроку;
        /// в случае неудачи возвращает -1; Допускается использование конструкций регулярных выражений
        /// </summary>
        /// <param name="value">Строка, в которой производится поиск</param>
        /// <param name="anyOf">Массив строк, позиции которых ищутся</param>
        /// <param name="startIndex">Индекс, с которого начинается поиск</param>
        /// <param name="Prefix">Общий префикс для всех строк</param>
        public static int GetIndexOfAnySubString(string value, string[] anyOf, int startIndex, string Prefix)
        {
            List<int> positions = new List<int>();
            value = value.Substring(startIndex);
            for (int i = 0; i < anyOf.Length; i++)
            {
                Match m = Regex.Match(value, Prefix + anyOf[i]);
                int pos = m.Success ? m.Index : -1;
                if (pos > -1)
                    positions.Add(pos);
            }
            positions.Sort();
            return (positions.Count > 0 ? positions[0] + startIndex : -1);
        }
        /// <summary>
        /// Посылает сообщение Windows указанному объекту
        /// </summary>
        /// <param name="hWnd">Дескриптор объекта</param>
        /// <param name="Msg">Код сообщения</param>
        /// <param name="wParam">WParam сообщения</param>
        /// <param name="lParam">LParam сообщения</param>
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        /// <summary>
        /// Вставляет вначале строки столько символов c, чтобы
        /// длина строки стала равной Length
        /// </summary>
        public static string AddChars(char c, string Value, int Length)
        {
            return new string(c, Math.Max(Length - Value.Length, 0)) + Value;
        }
        /// <summary>
        /// Вставляет вначале строки столько символов c, чтобы
        /// длина строки стала равной Length
        /// </summary>
        public static string AddCharsToEnd(char c, string Value, int Length)
        {
            return Value + new string(c, Math.Max(Length - Value.Length, 0));
        }
        /// <summary>
        /// Запускает указанный exe-файл
        /// </summary>
        public static void RunProgram(string ExeFileName)
        {
            Process p1 = new Process();
            p1.StartInfo.UseShellExecute = true;
            p1.StartInfo.FileName = ExeFileName;
            p1.StartInfo.WorkingDirectory = Path.GetDirectoryName(ExeFileName);
            p1.Start();
        }
        /// <summary>
        /// Отображает информационное сообщение
        /// </summary>
        /// <param name="Caption">Заголовок окна</param>
        /// <param name="Message">Текст сообщения</param>
        public static void InformationMessage(string Message, string Caption)
        {
            MessageBox.Show(Message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Отображает окно с сообщением об ошибке
        /// </summary>
        /// <param name="Message">Текст сообщения</param>
        public static void ErrorMessage(string Message)
        {
            MessageBox.Show(Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Отображает окно с сообщением о предупреждении
        /// </summary>
        /// <param name="Message">Текст сообщения</param>
        public static void WarningMessage(string Message)
        {
            MessageBox.Show(Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// Отображает окно с вопросом
        /// </summary>
        /// <param name="Caption">Заголовок окна</param>
        /// <param name="Message">Текст сообщения</param>
        public static DialogResult QuestionMessage(string Message, string Caption)
        {
            return MessageBox.Show(Message, Caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
        /// <summary>
        /// Возвращает путь к каталогу, в котором расположен exe-файл приложения
        /// </summary>
        public static string ApplicationDirectory
        {
            get
            {
                return Application.StartupPath;
            }
        }
        /// <summary>
        /// Возвращает полное имя exe-файла приложения
        /// </summary>
        public static string ApplicationFileName
        {
            get
            {
                return Application.ExecutablePath;
            }
        }
        /// <summary>
        /// Преобразует массив байт в строку из элементов массива в шестнадцатеричной системе
        /// </summary>
        public static string BytesToStringOfHex(byte[] array)
        {
            string res = "";
            for (int i = 0; i < array.Length; i++)
                res += Utils.AddChars('0', Convert.ToString(array[i], 16), 2) + " ";
            return res;
        }
      
        public static Bitmap CreateBitmapFromGraphics(Graphics graphicsSource, int x, int y, int width, int height)
        {  //Получаем HDC (контекст устройства)  
            IntPtr hDCSource = graphicsSource.GetHdc();  //Создаем HDC, совместимый с только-что полученным  
            IntPtr hDCDestination = Win32.CreateCompatibleDC(hDCSource);  //Создаем Win32 bitmap, совместимый с HDC (полученном на первом шаге)  
            IntPtr hBitmap = Win32.CreateCompatibleBitmap(hDCSource, width, height);  //Выбираем bitmap в DC (делаем его текущим), чтобы затем рисовать на нем  
            Win32.SelectObject(hDCDestination, hBitmap);  //Выполняем копирование из полученного DC во вновь созданный совместимый DC,  //получая таким образом в совместимом bitmap?e нужную картинку  
            Win32.BitBlt(hDCDestination, 0, 0, width, height, hDCSource, x, y, Win32.SRCCOPY);  //Создаем объект System.Drawing.Bitmap  
            Bitmap bitmap = Image.FromHbitmap(hBitmap);  //Удаляем уже ненужные объекты  
            Win32.DeleteObject(hDCDestination);
            Win32.DeleteObject(hBitmap);  //Освобождаем DC  
            graphicsSource.ReleaseHdc(hDCSource);
            return bitmap;
        }    
    }
}
