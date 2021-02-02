using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Reflection;
using System.ComponentModel;

namespace Tool
{
    public class ExportServer
    {
        public HSSFWorkbook Export<T>(string ExportName , List<T> list)
        {
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet(ExportName);
            IRow row = sheet.CreateRow(0);
            var listViewDisplaName = GetDisplayName(list.FirstOrDefault());//得到表头

            //设置表格样式
            ICellStyle cellStyle = book.CreateCellStyle();
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            //设置表头
            for (int i = 0 ; i < listViewDisplaName.Count ; i++)
            {
                row.CreateCell(i).SetCellValue(listViewDisplaName[i]);
            }
            //数据填充
            for (int i = 0 ; i < list.Count ; i++)
            {
                IRow ro = sheet.CreateRow(i + 1);
                var result = GetValue(list[i]);
                for (int j = 0 ; j < result.Count ; j++)
                {
                    ro.CreateCell(j).SetCellValue(result[j]);
                }
            }
            return book;
        }
        /// <summary>
        /// 获取表头
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eneityView"></param>
        /// <returns></returns>
        public List<string> GetDisplayName<T>(T eneityView)
        {
            PropertyInfo[] propertyInfos = eneityView.GetType().GetProperties();
            List<string> list = new List<string>();
            foreach (var item in propertyInfos)
            {
                var objs = item.GetCustomAttributes(typeof(DisplayNameAttribute) , true);
                if (objs != null)
                {
                    var disPlayName = ((DisplayNameAttribute)objs[0]).DisplayName;
                    list.Add(disPlayName);
                }
            }
            return list;
        }
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eneityView"></param>
        /// <returns></returns>
        public List<string> GetValue<T>(T eneityView)
        {
            List<string> list = new List<string>();
            Type type = eneityView.GetType();
            PropertyInfo[] propertyInfo = type.GetProperties();
            foreach (var item in propertyInfo)
            {
                string name = item.Name;
                PropertyInfo pro = type.GetProperty(name);
                var ValueEneity = pro.GetValue(eneityView).ToString();
                list.Add(ValueEneity);
            }
            return list;
        }
    }

}
