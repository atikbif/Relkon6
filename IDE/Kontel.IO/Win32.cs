using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Kontel.Relkon
{
    public class Win32
    {
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("Gdi32.dll")]
        public static extern int BitBlt(
            IntPtr hdcDest,
            int nXDest,
            int nYDest,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int nXSrc,
            int nYSrc,
            int dwRop
            );

        [DllImport("Gdi32.dll")]
        public static extern int StretchBlt(
            IntPtr hdcDest,
            int nXOriginDest,
            int nYOriginDest,
            int nWidthDest,
            int nHeightDest,
            IntPtr hdcSrc,
            int nXOriginSrc,
            int nYOriginSrc,
            int nWidthSrc,
            int nHeightSrc,
            int dwRop
            );

        public const int SRCCOPY = 0x00CC0020;

        [DllImport("Gdi32.dll")]
        public static extern int SetROP2(IntPtr hDC, int fnDrawMode);

        public const int R2_NOT = 6;
        public const int R2_NOP = 11;

        [DllImport("Gdi32.dll")]
        static extern int MoveToEx(IntPtr hDC, int x, int y, IntPtr lpPoint);

        public static int MoveTo(IntPtr hDC, int x, int y)
        {
            return MoveToEx(hDC, x, y, IntPtr.Zero);
        }

        [DllImport("Gdi32.dll")]
        public static extern int LineTo(IntPtr hDC, int nXEnd, int nYEnd);

        [DllImport("Gdi32.dll")]
        static extern int GetPixel(
            IntPtr hdc,
            int nXPos,
            int nYPos
            );

        public static Color GetPixelColor(IntPtr hDC, int x, int y)
        {
            long colorRef = GetPixel(hDC, x, y);
            return Color.FromArgb((byte)colorRef, (byte)(colorRef >> 8), (byte)(colorRef >> 16));
        }

        [DllImport("Gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hGdiObj);

        [DllImport("Gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("Gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

        [DllImport("Gdi32.dll")]
        public static extern int DeleteObject(IntPtr hObject);

    }
}
