using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNet.Common.FileConvert;
using RabbitMQClient;
using System.Diagnostics;

namespace FileTransSoffice
{
    public class File2Pdf : BaseConfig
    {
        public static void File2PdfOffice(RabbitMQModel.FileModel file, string suffix)
        {
            //Console.WriteLine(file.SourcePath.Substring(0, file.SourcePath.LastIndexOf('\\')));
            //Console.WriteLine(file.SourcePath.Substring(0, file.SourcePath.LastIndexOf('.')) + ".pdf");

            bool isOK = false;
            var pc = new Process
            {
                StartInfo =
                {
                    FileName = ConfigSection.KeyValues["sofficePath"].Value,
                    Arguments = string.Format("{0} \"{1}\" \"{2}\"", ConfigSection.KeyValues["sofficeArgu"].Value, file.SourcePath.Substring(0, file.SourcePath.LastIndexOf('\\')), file.SourcePath),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            pc.Start();
            isOK = pc.WaitForExit(1000 * 60 * 30);
            pc.Close();
            pc.Dispose();
            if (isOK)
            {
                //添加消息队列
                RabbitMQHelper.Instance.SendMessage(new RabbitMQModel.FileModel
                {
                    SourcePath = file.SourcePath.Substring(0, file.SourcePath.LastIndexOf('.')) + ".pdf",
                    TargetPath = file.TargetPath
                });
            }
        }
    }
}
