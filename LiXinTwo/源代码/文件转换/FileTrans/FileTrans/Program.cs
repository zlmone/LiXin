using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using DotNet.Common.FileConvert;
using RabbitMQClient;

namespace FileTrans
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("文件转换启动...（请勿关闭）");

            //发送空消息，确认服务器
            RabbitMQHelper.Instance.SendMessage(null);
            //rabbitmq订阅式接收消息并转换
            RabbitMQHelper.Instance.ReceivingMessagesPassive(FileTask);

            Console.ReadLine();
        }

        static void FileTask(object fileModel)
        {
            //var sw = new Stopwatch();
            //sw.Start();

            try
            {
                var file = (RabbitMQModel.FileModel)fileModel;
                Console.WriteLine(file.SourcePath);

                var fileInfo = new FileInfo(file.SourcePath);

                var i = 0;
                while (true)
                {
                    if (fileInfo.Exists)
                    {
                        break;
                    }
                    Thread.Sleep(500);

                    //10分钟左右
                    if (i > 1000)
                    {
                        const string str = "文件不存在！";
                        Console.WriteLine(str);
                        LogHelper.WriteLog(file.SourcePath + "    " + str);
                        return;
                    }
                    i++;
                }

                var suffix = fileInfo.Extension.ToLower();

                switch (suffix)
                {
                    case ".avi":
                    case ".rmvb":
                    case ".wmv":
                        Media2Flv.MediaConvert(file.SourcePath, file.TargetPath, (file.TargetPath.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase)));
                        break;
                    case ".flv":
                        string isTransFlv2Mp4 = System.Configuration.ConfigurationManager.AppSettings["IsTransFlv2Mp4"];
                        if (string.IsNullOrWhiteSpace(isTransFlv2Mp4))
                            break;
                        if (isTransFlv2Mp4 == "1")
                        {
                            Media2Flv.MediaConvert(file.SourcePath, file.TargetPath, (file.TargetPath.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase)));
                        }
                        break;
                    case ".mp4":
                        break;
                    case ".doc":
                    case ".docx":
                    case ".ppt":
                    case ".pptx":
                    case ".xls":
                    case ".xlsx":
                    case ".txt":
                        File2Pdf.File2PdfOffice(file, suffix);
                        break;
                    case ".pdf":
                        Pdf2Swf.PdfToSwf(file.SourcePath, file.TargetPath);
                        break;
                    default:
                        const string str = "文件格式有误！";
                        LogHelper.WriteLog(file.SourcePath + "    " + str);
                        Console.WriteLine(str);
                        break;
                }

                Console.WriteLine("该文件转换结束！");
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(e.Message, e);
                Console.WriteLine(e.Message);
            }

            //sw.Stop();
            //Console.WriteLine(sw.Elapsed);
        }
    }
}
