using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Kontel.Relkon
{
    /// <summary>
    /// Предоставляет дополнительные методы для работы
    /// с файлами и каталогами
    /// </summary>
    public sealed class FileSystem
    {
        /// <summary>
        /// Переименовывает файл
        /// </summary>
        /// <param name="OldFileName">Старое имя файла</param>
        /// <param name="NewFileName">Новое имя файла</param>
        /// <param name="Owerwrite">Показыает, требуется ли перезаписать файл NewFileName, если он уже существует</param>
        public static void Rename(string OldFileName, string NewFileName, bool Owerwrite)
        {
            if (OldFileName == NewFileName)
                return;
            File.Copy(OldFileName, NewFileName, Owerwrite);
            File.SetAttributes(OldFileName, FileAttributes.Normal);
            File.Delete(OldFileName);
        }
        /// <summary>
        /// Копирует все файлы из каталога OldPath в каталог NewPath, 
        /// если такие файлы уже существуют, то они перезаписываются
        /// </summary>
        public static void CopyDirectory(string OldDirectoryName, string NewDirectoryName)
        {
            string[] Files = Directory.GetFiles(OldDirectoryName);
            foreach (string FileName in Files)
            {
                File.Copy(FileName, NewDirectoryName + "\\" + Path.GetFileName(FileName), true);
            }
        }
        public static void CopyDirectoryWithRename(string OldDirectoryName, string NewDirectoryName, string OldValue, string NewValue)
        {
            string[] Files = Directory.GetFiles(OldDirectoryName);
            foreach (string FileName in Files)
            {

                File.Copy(FileName, NewDirectoryName + "\\" + Path.GetFileName(FileName).Replace(OldValue, NewValue), true);
            }
        }
        /// <summary>
        /// Удаляет все файлы из указанного каталога
        /// </summary>
        public static void DeleteDirectory(string DirectoryName)
        {
            string[] Files = Directory.GetFiles(DirectoryName);
            foreach (string FileName in Files)
            {
                FileInfo fi1 = new FileInfo(FileName);
                fi1.Attributes = FileAttributes.Normal;
                File.Delete(FileName);
            }
        }
    }
}
