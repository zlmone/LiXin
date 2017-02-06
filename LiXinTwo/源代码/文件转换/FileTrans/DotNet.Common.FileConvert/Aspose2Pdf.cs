using System.IO;

namespace DotNet.Common.FileConvert
{
    public class Aspose2Pdf
    {
        /// <summary>
        /// Word格式转换(Word文件不能为空，默认转成PDF)
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="targetFileType">转换类型</param>
        /// <returns></returns>
        public static bool WordConvert(string sourcePath, string targetPath, Aspose.Words.SaveFormat targetFileType = Aspose.Words.SaveFormat.Pdf)
        {
            var doc = new Aspose.Words.Document(@sourcePath);
            doc.Save(@targetPath, targetFileType);
            return true;
        }

        /// <summary>
        /// Word格式转换(Word文件不能为空，默认转成PDF)
        /// </summary>
        /// <param name="sourceStream">源文件</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="targetFileType">转换类型</param>
        /// <returns></returns>
        public static bool WordConvert(Stream sourceStream, string targetPath, Aspose.Words.SaveFormat targetFileType = Aspose.Words.SaveFormat.Pdf)
        {
            var doc = new Aspose.Words.Document(sourceStream);
            doc.Save(@targetPath, targetFileType);
            return true;
        }

        /// <summary>
        /// Excel格式转换(Excel文件不能为空，默认转成PDF)
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="targetFileType">转换类型</param>
        /// <returns></returns>
        public static bool ExcelConvert(string sourcePath, string targetPath, Aspose.Cells.SaveFormat targetFileType = Aspose.Cells.SaveFormat.Pdf)
        {
            var xls = new Aspose.Cells.Workbook(@sourcePath);
            xls.Save(@targetPath, targetFileType);
            return true;
        }

        /// <summary>
        /// Excel格式转换(Excel文件不能为空，默认转成PDF)
        /// </summary>
        /// <param name="sourceStream">源文件</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="targetFileType">转换类型</param>
        /// <returns></returns>
        public static bool ExcelConvert(Stream sourceStream, string targetPath, Aspose.Cells.SaveFormat targetFileType = Aspose.Cells.SaveFormat.Pdf)
        {
            var xls = new Aspose.Cells.Workbook(sourceStream);
            xls.Save(@targetPath, targetFileType);
            return true;
        }

        /// <summary>
        /// PowerPoint格式转换(PowerPoint文件不能为空，默认转成PDF)
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="targetFileType">转换类型</param>
        /// <returns></returns>
        public static bool PowerPointConvert(string sourcePath, string targetPath, Aspose.Slides.Export.SaveFormat targetFileType = Aspose.Slides.Export.SaveFormat.Pdf)
        {
            var extension = new FileInfo(sourcePath).Extension.ToLower();

            if (extension == ".ppt")
            {
                var ppt = new Aspose.Slides.Presentation(@sourcePath);
                ppt.Save(@targetPath, targetFileType);
                return true;
            }

            if (extension == ".pptx")
            {
                var pptx = new Aspose.Slides.Pptx.PresentationEx(@sourcePath);
                pptx.Save(@targetPath, targetFileType);
                return true;
            }

            throw new IOException(sourcePath + "  非PowerPoint文件");
        }
    }
}
