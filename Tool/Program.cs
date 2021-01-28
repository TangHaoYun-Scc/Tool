using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basics_Tool.ORM;
using System.Reflection;
using Model;
using Basics_Tool;

namespace Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            string As = StringTool.String2Ascii("My Name is Thy");

            Console.WriteLine(As);
            Console.WriteLine(StringTool.Ascii2String(As));
            Console.ReadLine();
        }
    }

}
