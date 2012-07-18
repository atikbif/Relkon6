using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Kontel.TabbedDocumentsForm
{
    /// <summary>
    /// ��������� ����� �������, �������� ������
    /// �������� ������������� ��������
    /// </summary>
    public interface IEditableTabbedDocument
    {
        /// <summary>
        /// ����������, ��������� �� ��������� ��������
        /// </summary>
        bool SaveRequired
        {
            get;
            set;
        }
        /// <summary>
        /// ����������, ����� �� �������� ��������� �������������� ���������
        /// </summary>
        bool CanUndo
        {
            get;
        }
        /// <summary>
        /// ����������, ����� �� ������� �������� � ��������� �����
        /// ��������� ������� ���������
        /// </summary>
        bool CanRedo
        {
            get;
        }
        /// <summary>
        /// ����������, ����� �� �������� � �������� ������ �� ������ ������

        /// </summary>
        bool CanPaste
        {
            get;
        }
        /// <summary>
        /// ��������� ��������
        /// </summary>
        void Save();
        /// <summary>
        /// ��������� �������� � ���� � ��������� ������, � ����� ��������� �������� ����������� ������ ����
        /// </summary>
        void SaveAs(string FileName);
        /// <summary>
        /// �������� ��������� �������������� ���������
        /// </summary>
        void Undo();
        /// <summary>
        /// ���������� �������� � ��������� ����� ��������� ������� ���������
        /// </summary>
        void Redo();
        /// <summary>
        /// �������� ���������� ������ �� ��������� � �������� ��� � ������ ������
        /// </summary>
        void Cut();
        /// <summary>
        /// �������� ���������� ������ � ������ ������
        /// </summary>
        void Copy();
        /// <summary>
        /// ��������� ����� ������������ ������ �� ������� ������ � ��������, 
        /// � ������� �������
        /// </summary>
        void Paste();
        /// <summary>
        /// ������� ���������� ������
        /// </summary>
        void Delete();
        /// <summary>
        /// �������� ��� ������� ���������
        /// </summary>
        void SelectAll();
    }
}
