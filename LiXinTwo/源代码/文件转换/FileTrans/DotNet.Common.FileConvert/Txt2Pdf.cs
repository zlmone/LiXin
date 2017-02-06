using System;
using System.IO;
using System.Text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;
using Paragraph = iTextSharp.text.Paragraph;

namespace DotNet.Common.FileConvert
{
    public class Txt2Pdf : BaseConfig
    {
        /// <summary>
        /// 字体路径
        /// </summary>
        private static string FontPath
        {
            get
            {
                var path = "";
                if (ConfigSection != null)
                    path = ConfigSection.KeyValues["fontPath"].Value;
                return path == "" ? @"C:\Windows\Fonts\STFANGSO.TTF" : path;
            }
        }

        /// <summary>
        /// Text转成PDF
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="fontPath">系统字体路径</param>
        /// <returns></returns>
        public static bool TextConvert(string sourcePath, string targetPath, string fontPath = default(string))
        {
            var path = fontPath != default(string) ? fontPath : FontPath;

            using (var objReader = new StreamReader(sourcePath, Encoding.Default))
            {
                iTextSharp.text.Document textDocument = null;
                try
                {
                    var baseFont = BaseFont.createFont(path, BaseFont.IDENTITY_H,
                                                            BaseFont.NOT_EMBEDDED);
                    var font = new Font(baseFont);
                    textDocument = new iTextSharp.text.Document();
                    PdfWriter.getInstance(textDocument, new FileStream(targetPath, FileMode.Create));
                    textDocument.Open();
                    var strRead = objReader.ReadToEnd();
                    textDocument.Add(new Paragraph(strRead == "" ? " " : strRead, font));
                    textDocument.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                finally
                {
                    if (textDocument != null && textDocument.isOpen())
                    {
                        textDocument.Close();
                    }
                }
            }
            return true;
        }
    }
}
