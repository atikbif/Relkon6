using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Kontel.Relkon;
using Kontel.Relkon.Solutions;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Kontel.Relkon
{
    static class Program
    {
        ///
        /// <summary>
        /// ���������� ��������� Relkon
        /// </summary>
        public static Relkon.Properties.Settings Settings
        {
            get
            {
                return Relkon.Properties.Settings.Default;
            }
        }
        /// <summary>
        /// ���������� �������, � ������� ���������� Relkon
        /// </summary>
        public static string RelkonDirectory
        {
            get
            {
                DirectoryInfo di = new DirectoryInfo(Utils.ApplicationDirectory);
                return di.FullName;
            }
        }

        public static string AssemblyVersion
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyFileVersionAttribute)attributes[0]).Version;
                //return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }       

        /// <summary>
        /// ���������� ��� ��������, � ������� ����������� ����� �������
        /// </summary>
        public static string NewProjectDirectory
        {
            get
            {
                return Program.RelkonDirectory + "\\project";
            }
        }
        /// <summary>
        /// ���������� �������, � ������� ����� �������������
        /// </summary>
        public static string ProgrammatorsDirectory
        {
            get
            {
                return Program.RelkonDirectory + "\\Programmators";
            }
        }
        /// <summary>
        /// �������� ���������� ������ ����� ���� ������ 1, �������
        /// �������� ��� ��������� � 1
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