using System;
using SRMTools.GraphicsTools;
using System.Drawing.Imaging;
using System.IO;

namespace SRMTools
{
    class Program
    {
        static void Main(string[] args)
        {
            String filePath = "";
            String fileName = "SRMTools.jpg";

            ArgsParser.Options("-p|--path,-f|--fullscreen,-t|--thumbnail,-r|--resize,-wr|resizebywidth,-hr|resizebyheight");
            ArgsParser.Parse(args);
            if (!ArgsParser.HasValidArgs())
            {
                ShowHelp(ArgsParser.GetArgArray());
                return;
            }

            String filePathArgs = ArgsParser.GetValueString("-p");
            if (filePathArgs.Length > 0)
            {
                filePath = filePathArgs;
                if (!filePath.EndsWith("\\"))
                {
                    filePath += "\\";
                }
            }

            String fileNameArgs = ArgsParser.GetValueString("-f");
            if (fileNameArgs.Length > 0)
            {
                fileName = fileNameArgs;
                if(filePath.Length > 0)
                {
                    ScreenCapture.Capture(filePath, fileName);
                }
                else
                {
                    ScreenCapture.Capture(fileName);
                }
                
            }

            String thumbFileNameArgs = ArgsParser.GetValueString("-t");
            if (thumbFileNameArgs.Length > 0)
            {
                fileName = thumbFileNameArgs;
                if(filePath.Length > 0)
                {
                    ScreenCapture.CaptureThumbnail(filePath, fileName);
                }
                else
                {
                    ScreenCapture.CaptureThumbnail(fileName);
                }
            }

            String resizeArgs = ArgsParser.GetValueString("-r");
            if (resizeArgs.Length > 0)
            {
                String[] newSize = resizeArgs.ToUpper().Split('X');
                if (newSize.Length > 0)
                {
                    var screen = ScreenCapture.Resize(filePath + fileName, int.Parse(newSize[0]), int.Parse(newSize[1]));
                    if (File.Exists(filePath + fileName))
                        File.Delete(filePath + fileName);
                    screen.Save(filePath + fileName, ImageFormat.Jpeg);
                }
            }

            String resizeWidthArgs = ArgsParser.GetValueString("-wr");
            if (resizeWidthArgs.Length > 0)
            {
                var screen = ScreenCapture.ResizeByWidth(filePath + fileName, int.Parse(resizeWidthArgs));
                if (File.Exists(filePath + fileName))
                    File.Delete(filePath + fileName);
                screen.Save(filePath + fileName, ImageFormat.Jpeg);
            }

            String resizeHeightArgs = ArgsParser.GetValueString("-hr");
            if (resizeHeightArgs.Length > 0)
            {
                var screen = ScreenCapture.ResizeByHeight(filePath + fileName, int.Parse(resizeHeightArgs));
                if (File.Exists(filePath + fileName))
                    File.Delete(filePath + fileName);
                screen.Save(filePath + fileName, ImageFormat.Jpeg);
            }
        }

        static void ShowHelp(String[] args)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine();
            Console.WriteLine("Options:");
            foreach(var item in args)
            {
                Console.WriteLine(item);
            }
        }
    }
}
