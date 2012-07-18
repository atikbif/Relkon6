using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Kontel.TabbedDocumentsForm
{
    /// <summary>
    /// Содержит набор свойств и методов, которые должен реализовывать
    /// документ, содержимое которого можно распечатать
    /// </summary>
    public interface IPrintableTabbedDocument
    {
        /// <summary>
        /// Печатает документ
        /// </summary>
        void Print(PrintDialog dialog);
        ///// <summary>
        ///// Установка параметров страницы
        ///// </summary>
        //void PageSetup();
        /// <summary>
        /// Предваритеьный просмотр
        /// </summary>
        void PrintPreview(PrintPreviewDialog dialog);
    }
}
