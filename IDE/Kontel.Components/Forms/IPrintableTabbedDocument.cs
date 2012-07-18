using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Kontel.TabbedDocumentsForm
{
    /// <summary>
    /// �������� ����� ������� � �������, ������� ������ �������������
    /// ��������, ���������� �������� ����� �����������
    /// </summary>
    public interface IPrintableTabbedDocument
    {
        /// <summary>
        /// �������� ��������
        /// </summary>
        void Print(PrintDialog dialog);
        ///// <summary>
        ///// ��������� ���������� ��������
        ///// </summary>
        //void PageSetup();
        /// <summary>
        /// �������������� ��������
        /// </summary>
        void PrintPreview(PrintPreviewDialog dialog);
    }
}
