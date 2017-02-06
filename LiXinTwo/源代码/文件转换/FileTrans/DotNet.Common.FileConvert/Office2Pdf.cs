using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Interop.Word;
using ApplicationClass = Microsoft.Office.Interop.Word.ApplicationClass;

namespace DotNet.Common.FileConvert
{
    public class Office2Pdf
    {
        /// <summary>
        /// Word格式转换(默认转成PDF)
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="targetFileType">转换类型</param>
        /// <returns></returns>
        public static bool WordConvert(string sourcePath, string targetPath, WdExportFormat targetFileType = WdExportFormat.wdExportFormatPDF)
        {
            ApplicationClass wordApplication = null;
            Document wordDocument = null;
            try
            {
                var file = new FileInfo(sourcePath);
                var listP = System.Diagnostics.Process.GetProcessesByName("winword");
                foreach (var p in listP.Where(p => p.MainWindowTitle.Contains(file.Name)))
                {
                    p.Kill();
                }

                wordApplication = new ApplicationClass();
                wordDocument = wordApplication.Documents.Open(sourcePath);
                if (wordDocument != null)
                {
                    wordDocument.SaveAs(targetPath, targetFileType);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                if (wordDocument != null)
                {
                    wordDocument.Close();
                }
                if (wordApplication != null)
                {
                    wordApplication.Quit();
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return true;
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowThreadProcessId(IntPtr hwnd, out int id);

        /// <summary>
        /// Excel格式转换(Excel文件不能为空，默认转成PDF)
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="targetFileType">转换类型</param>
        /// <returns></returns>
        public static bool ExcelConvert(string sourcePath, string targetPath, XlFixedFormatType targetFileType = XlFixedFormatType.xlTypePDF)
        {
            Microsoft.Office.Interop.Excel.ApplicationClass excelApplication = null;
            Workbook excelWorkBook = null;
            try
            {
                excelApplication = new Microsoft.Office.Interop.Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Open(sourcePath);
                excelWorkBook.ExportAsFixedFormat(targetFileType, targetPath, XlFixedFormatQuality.xlQualityStandard);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                if (excelWorkBook != null)
                {
                    excelWorkBook.Close();
                }
                if (excelApplication != null)
                {
                    int k;
                    var t = new IntPtr(excelApplication.Hwnd);
                    GetWindowThreadProcessId(t, out k);
                    var p = System.Diagnostics.Process.GetProcessById(k);
                    excelApplication.Quit();
                    p.Kill();
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return true;
        }

        /// <summary>
        /// PowerPoint格式转换(默认转成PDF)
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="targetFileType">转换类型</param>
        /// <returns></returns>
        public static bool PowerPointConvert(string sourcePath, string targetPath, PpSaveAsFileType targetFileType = PpSaveAsFileType.ppSaveAsPDF)
        {
            Microsoft.Office.Interop.PowerPoint.ApplicationClass pptApplication = null;
            Presentation pptPersentation = null;
            try
            {
                pptApplication = new Microsoft.Office.Interop.PowerPoint.ApplicationClass();
                pptPersentation = pptApplication.Presentations.Open(sourcePath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                pptPersentation.SaveAs(targetPath, targetFileType, MsoTriState.msoTrue);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                if (pptPersentation != null)
                {
                    pptPersentation.Close();
                }
                if (pptApplication != null)
                {
                    int k;
                    var t = new IntPtr(pptApplication.HWND);
                    GetWindowThreadProcessId(t, out k);
                    var p = System.Diagnostics.Process.GetProcessById(k);
                    pptApplication.Quit();
                    p.Kill();
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return true;
        }
    }
}

