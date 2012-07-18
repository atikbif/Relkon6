using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Kontel.Relkon;
using Kontel.Relkon.Solutions;
using System.Text.RegularExpressions;

namespace Kontel.Relkon
{
    static class Program
    {
        ///
        /// <summary>
        /// Внутренние настройки Relkon
        /// </summary>
        public static Relkon.Properties.Settings Settings
        {
            get
            {
                return Relkon.Properties.Settings.Default;
            }
        }
        /// <summary>
        /// Возвращает каталог, в который установлен Relkon
        /// </summary>
        public static string RelkonDirectory
        {
            get
            {
                DirectoryInfo di = new DirectoryInfo(Utils.ApplicationDirectory);
                return di.FullName;
            }
        }
        /// <summary>
        /// Возвращает имя каталога, в котором размещаются новые проекты
        /// </summary>
        public static string NewProjectDirectory
        {
            get
            {
                return Program.RelkonDirectory + "\\project";
            }
        }
        /// <summary>
        /// Возвращает каталог, в котором лежат программаторы
        /// </summary>
        public static string ProgrammatorsDirectory
        {
            get
            {
                return Program.RelkonDirectory + "\\Programmators";
            }
        }
        /// <summary>
        /// Параметр коммандной строки может быть только 1, функция
        /// собирает все параметры в 1
        /// </summary>
        private static string CollectArgs(string[] args)
        {
            if (args == null)
                return null;
            string res = "";
            for (int i = 0; i < args.Length; i++)
                res += args[i];
            return res;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(CollectArgs(args)));
        }
    }
}