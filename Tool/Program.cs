
using Basics_Tool.ORM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
namespace Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FileStream file = new FileStream("D:\\demo.xls" , FileMode.Open , FileAccess.Read))
            {
                DataTable dt = ExportServer.ExcelImportToDataTable(file);
            }
            //Studen st = new Studen();
            //ExportServer.ExportToFle<Studen>(st);
            //ExportServer.SaveTofle("学生表" , GetListStuden() , "D:\\" , "demo");
        }
        static List<Studen> GetListStuden()
        {
            //测试数据
            List<Studen> list = new List<Studen>();
            Random r = new Random();
            for (int i = 0 ; i < 200 ; i++)
            {
                Studen st = new Studen();
                st.Id = i + 1;
                st.Name = "Tom" + i.ToString();
                st.Age = r.Next(40) + 18;
                st.Address = $"北京市西{r.Next(4) + 1}环xx路{r.Next(100)}号";
                list.Add(st);
            }
            return list;
        }

    }
    [TableName("Studen")]
    class Studen
    {
        [DisplayName("编号")]
        public int Id { get; set; }

        [DisplayName("姓名")]
        public string Name { get; set; }


        [DisplayName("年龄")]
        public int Age { get; set; }

        [DisplayName("地址")]
        public string Address { get; set; }
    }
}
