
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
namespace Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            ExportServer ex = new ExportServer();
            var reauslt = ex.Export("商品信息表" , GetListStuden());

            MemoryStream ms = new MemoryStream();
            reauslt.Write(ms);
            ms.Seek(0 , SeekOrigin.Begin);
            string filename ="D:\\"+ Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddhhmmss") + ".xls";
            FileStream file = new FileStream(filename , FileMode.Create);
            file.Write(ms.GetBuffer() , 0 , ms.GetBuffer().Length);//将文件流写入到新建的EXCEL
            file.Close();//关闭文件
            //saveTofle(ms , @"D://demo.xlsx");
            Console.WriteLine("导出成功");
        }

        private static void saveTofle(MemoryStream file , string fileName)
        {
            using (FileStream fs = new FileStream(fileName , FileMode.OpenOrCreate , FileAccess.Write))
            {
                byte[] buffer = file.ToArray();//转化为byte格式存储
                fs.Write(buffer , 0 , buffer.Length);
                fs.Flush();
                buffer = null;
            }//使用using可以最后不用关闭fs 比较方便
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
