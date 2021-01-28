using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basics_Tool.ORM;
using System.Reflection;
using Model;

namespace Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            Student std1 = new Student() { ID = 1, name = "张三", height = 175.4 };
            Student std2 = new Student() { ID = 2, name = "王五", height = 175.4 };
            //添加语句
            string sql = CustomAttributeTool.GetInsertSql2<Student>(std1);
            List<string> ex = new List<string>() { "3", "1", "2" };
            //修改语句
            string sql2 = CustomAttributeTool.GetUpdateSql2<Student>(std1, ex);
            Console.ReadLine();
        }
    }
    
}
