using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNet.Common.FileConvert;
using RabbitMQClient;
using System.Diagnostics;

namespace FileTrans
{
    public class File2Pdf : BaseConfig
    {
        public static void File2PdfOffice(RabbitMQModel.FileModel file, string suffix)
        {
            var targetPath = file.TargetPath + ".pdf";
            switch (suffix)
            {
                case ".doc":
                case ".docx":
                    Office2Pdf.WordConvert(file.SourcePath, targetPath);
                    break;
                case ".ppt":
                case ".pptx":
                    Office2Pdf.PowerPointConvert(file.SourcePath, targetPath);
                    break;
                case ".xls":
                case ".xlsx":
                    Office2Pdf.ExcelConvert(file.SourcePath, targetPath);
                    break;
                case ".txt":
                    Txt2Pdf.TextConvert(file.SourcePath, targetPath);
                    break;
            }

            //添加消息队列
            RabbitMQHelper.Instance.SendMessage(new RabbitMQModel.FileModel
            {
                SourcePath = targetPath,
                TargetPath = file.TargetPath
            });
        }
    }
}
