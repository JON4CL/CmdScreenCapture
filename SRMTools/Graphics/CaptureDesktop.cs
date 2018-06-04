using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SRMTools.GraphicsTools
{
    public class ScreenCapture
    {
        private ScreenCapture() { }

        public static bool Capture(String FileName)
        {
            return Capture(".\\", FileName);
        }

        public static bool Capture(String Path, String FileName)
        {
            using (Bitmap fullScreen = CaptureDesktop())
            {
                fullScreen.Save(Path + FileName, ImageFormat.Jpeg);
            }
            return true;
        }

        public static bool CaptureThumbnail(String FileName)
        {
            return CaptureThumbnail(".\\", FileName);
        }

        public static bool CaptureThumbnail(String Path, String FileName)
        {
            using (Image fullScreen = CaptureDesktop())
            {
                using (Image thumbnailScreen = fullScreen.GetThumbnailImage(300, (fullScreen.Height / (fullScreen.Width / 300)), null, IntPtr.Zero))
                {
                    thumbnailScreen.Save(Path + FileName, ImageFormat.Jpeg);
                }
            }
            return true;
        }

        public static Bitmap Resize(String srcImage, int newWidth, int newHeight)
        {
            Bitmap bmp = null;
            using (Image img = Image.FromFile(srcImage))
            { 
                bmp = Resize(img, newWidth, newHeight);
            }
            return bmp;
        }

        public static Bitmap Resize(Image srcImage, int newWidth, int newHeight)
        {
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
            }
            return new Bitmap(newImage);
        }

        public static Bitmap ResizeByWidth(String srcImage, int newWidth)
        {
            Bitmap bmp = null;
            using (Image img = Image.FromFile(srcImage))
            {
                bmp = ResizeByWidth(img, newWidth);
            }
            return bmp;
        }

        public static Bitmap ResizeByHeight(String srcImage, int newHeight)
        {
            Bitmap bmp = null;
            using (Image img = Image.FromFile(srcImage))
            {
                bmp = ResizeByHeight(img, newHeight);
            }
            return bmp;
        }

        public static Bitmap ResizeByWidth(Image srcImage, int newWidth)
        {
            float scale = (float)((float)srcImage.Height / (float)srcImage.Width);
            int newHeight = (int)(newWidth * scale);

            return Resize(srcImage, newWidth, newHeight);
        }

        public static Bitmap ResizeByHeight(Image srcImage, int newHeight)
        {
            float scale = (float)((float)srcImage.Width / (float)srcImage.Height);
            int newWidth = (int)(newHeight * scale);

            return Resize(srcImage, newWidth, newHeight);
        }

        public static Bitmap ResizeScale(Image srcImage, float scalePercent)
        {
            int newWidth = (int)(srcImage.Width * (scalePercent / 100));
            int newHeight = (int)(srcImage.Height * (scalePercent / 100));

            return Resize(srcImage, newWidth, newHeight);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public static Bitmap CaptureDesktop()
        {
            return new Bitmap(CaptureWindow(GetDesktopWindow()));
        }

        private static Bitmap CaptureActiveWindow()
        {
            return new Bitmap(CaptureWindow(GetForegroundWindow()));
        }

        private static Bitmap CaptureWindow(IntPtr handle)
        {
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return new Bitmap(result);
        }
    }
}
