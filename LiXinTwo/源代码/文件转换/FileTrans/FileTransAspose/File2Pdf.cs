using DotNet.Common.FileConvert;
using RabbitMQClient;

namespace FileTransAspose
{
    public class File2Pdf
    {
        public static void File2PdfAspose(RabbitMQModel.FileModel file, string suffix)
        {
            var targetPath = file.TargetPath + ".pdf";
            switch (suffix)
            {
                case ".doc":
                case ".docx":
                    Aspose2Pdf.WordConvert(file.SourcePath, file.TargetPath);
                    break;
                case ".ppt":
                case ".pptx":
                    Aspose2Pdf.PowerPointConvert(file.SourcePath, file.TargetPath);
                    break;
                case ".xls":
                case ".xlsx":
                    Aspose2Pdf.ExcelConvert(file.SourcePath, file.TargetPath);
                    break;
                case ".txt":
                    Txt2Pdf.TextConvert(file.SourcePath, file.TargetPath);
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
