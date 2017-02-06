using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = "F:/FileTrans/FileTransSoffice/abcd.pdf";
            Console.WriteLine(path.Substring(0, path.LastIndexOf('/')));
            Console.WriteLine(path.Substring(0, path.LastIndexOf('.')));
            Console.ReadLine();
        }
    }
}
