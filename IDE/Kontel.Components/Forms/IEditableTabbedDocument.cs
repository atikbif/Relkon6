using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Kontel.TabbedDocumentsForm
{
    /// <summary>
    /// Описывает набор методов, которыми должен
    /// обладать редактируемый документ
    /// </summary>
    public interface IEditableTabbedDocument
    {
        /// <summary>
        /// Показывает, требуется ли сохранить документ
        /// </summary>
        bool SaveRequired
        {
            get;
            set;
        }
        /// <summary>
        /// Показывает, можно ли отменить последнее редактирование документа
        /// </summary>
        bool CanUndo
        {
            get;
        }
        /// <summary>
        /// Показывает, можно ли вернуть документ к состоянию перед
        /// последней отменой изменений
        /// </summary>
        bool CanRedo
        {
            get;
        }
        /// <summary>
        /// Показывает, можно ли вставить в документ объект из буфера обмена

        /// </summary>
        bool CanPaste
        {
            get;
        }
        /// <summary>
        /// Сохраняет документ
        /// </summary>
        void Save();
        /// <summary>
        /// Сохраняет документ в файл с указанным именем, в самом документе остается загруженным старый файл
        /// </summary>
        void SaveAs(string FileName);
        /// <summary>
        /// Отменяет последнее редактирование документа
        /// </summary>
        void Undo();
        /// <summary>
        /// Возвращает документ в состояние перед последней отменой изменений
        /// </summary>
        void Redo();
        /// <summary>
        /// Вырезает выделенный объект из документа и копирует его в буффер обмена
        /// </summary>
        void Cut();
        /// <summary>
        /// Копирует выделенный объект в буффер обмена
        /// </summary>
        void Copy();
        /// <summary>
        /// Вставляет ранее скопированый объект из буффера обмена в документ, 
        /// в позицию курсора
        /// </summary>
        void Paste();
        /// <summary>
        /// Удаляет выделенный объект
        /// </summary>
        void Delete();
        /// <summary>
        /// Выделяет все объекты документа
        /// </summary>
        void SelectAll();
    }
}
