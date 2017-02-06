using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace HandSend
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly List<string> _flv = new List<string> { ".avi", ".rmvb", ".wmv", ".flv" };
        readonly List<string> _swf = new List<string> { ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx", ".txt" };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            txtFilePath.Text = "";

            var type = new List<string> { "doc", "docx", "ppt", "pptx", "xls", "xlsx", "pdf", "txt", "avi", "rmvb", "wmv", "mp4", ".flv" };
            var fileType = type.Aggregate("|", (current, v) => current + ("*." + v + ";"));
            var filePaths = OpenFileDialog(fileType);
            var strPath = "";
            if (filePaths.Length != 0)
            {
                strPath = filePaths.Aggregate(strPath, (current, v) => current + (v + "\r\n"));
            }

            txtFilePath.Text = strPath.TrimEnd("\r\n".ToCharArray());
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filePaths = txtFilePath.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (filePaths.Length == 0)
                {
                    System.Windows.MessageBox.Show("先选择文件！");
                    return;
                }

                foreach (var v in filePaths)
                {
                    var targetPath = "";
                    var file = new FileInfo(v);
                    var suffix = file.Extension.ToLower();

                    if (_flv.Contains(suffix))
                    {
                        targetPath = v + ".mp4";
                    }
                    if (_swf.Contains(suffix))
                    {
                        targetPath = v + ".swf";
                    }

                    RabbitMQClient.RabbitMQHelper.Instance.SendMessage(new RabbitMQModel.FileModel
                                                                           {
                                                                               SourcePath = v,
                                                                               TargetPath = targetPath
                                                                           });
                }

                txtFilePath.Text = "";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private static string[] OpenFileDialog(string fileType)
        {
            var open = new OpenFileDialog
            {
                Multiselect = true,
                RestoreDirectory = true,
                Filter = fileType
            };
            open.ShowDialog();
            return open.FileNames;
        }
    }
}
