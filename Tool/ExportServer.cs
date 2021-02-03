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
using System.IO;
using System.Data;

namespace Tool
{
    public class ExportServer
    {
        /// <summary>
        /// 生成Workbook
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ExportName">工作簿名</param>
        /// <param name="list">数据源</param>
        /// <returns></returns>
        public static HSSFWorkbook Export<T>(string ExportName , List<T> list)
        {
            //创建workbook
            HSSFWorkbook book = new HSSFWorkbook();
            //创建并设置工作簿的名字
            ISheet sheet = book.CreateSheet(ExportName);
            //在工作簿上创建第一行
            IRow row = sheet.CreateRow(0);
            var listViewDisplaName = GetDisplayName(list.FirstOrDefault());//得到表头
            //设置表格样式
            ICellStyle cellStyle = book.CreateCellStyle();
            //水平居中
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //设置边框
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            //设置表头
            for (int i = 0 ; i < listViewDisplaName.Count ; i++)
            {
                ICell cel = row.CreateCell(i);//表头创建单元格
                cel.SetCellValue(listViewDisplaName[i]);//给单元格填充数据
                cel.CellStyle = cellStyle;
            }
            //数据填充
            for (int i = 0 ; i < list.Count ; i++)
            {
                IRow ro = sheet.CreateRow(i + 1);
                var result = GetValue(list[i]);
                for (int j = 0 ; j < result.Count ; j++)
                {
                    ICell icl = ro.CreateCell(j);
                    icl.SetCellValue(result[j]);
                    icl.CellStyle = cellStyle;
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
        public static List<string> GetDisplayName<T>(T eneityView)
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
        public static List<string> GetValue<T>(T eneityView)
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
        /// <summary>
        /// 导出文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ExportName">工作簿名</param>
        /// <param name="list">数据源</param>
        /// <param name="Path">保存地址</param>
        public static void SaveTofle<T>(string ExportName , List<T> list , string Path , string FileName = "")
        {
            MemoryStream ms = new MemoryStream();
            Export(ExportName , list).Write(ms);
            ms.Seek(0 , SeekOrigin.Begin);
            if (FileName == "")
                Path += "\\" + Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddhhmmss") + ".xls";
            else
                Path += "\\" + FileName + ".xls";
            using (FileStream file = new FileStream(Path , FileMode.Create))
            {
                file.Write(ms.GetBuffer() , 0 , ms.GetBuffer().Length);//将文件写入到新建的文件
            }
            #region  1.0注释
            //reauslt.Write(ms);
            //ms.Seek(0 , SeekOrigin.Begin);
            //string filename = "D:\\" + Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddhhmmss") + ".xls";
            //FileStream file = new FileStream(filename , FileMode.Create);
            //file.Write(ms.GetBuffer() , 0 , ms.GetBuffer().Length);//将文件流写入到新建的
            //file.Close();//关闭文件

            //using (FileStream fs = new FileStream(fileName , FileMode.OpenOrCreate , FileAccess.Write))
            //{
            //    byte[] buffer = file.ToArray();//转化为byte格式存储
            //    fs.Write(buffer , 0 , buffer.Length);
            //    fs.Flush();
            //    buffer = null;
            //}//使用using可以最后不用关闭fs 比较方便
            #endregion
        }

        public static DataTable ExportToFle<T>(T eneityView , int SheetIndex = 0)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(@"D:\demo.xls" , FileMode.Open , FileAccess.Read))
            {
                if (fs.Length > 0)
                {
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    ISheet ishe = workbook.GetSheetAt(SheetIndex);
                    ImportDt(ishe , 0 , eneityView);
                }
            }
            return dt;
        }
        /// <summary>
        /// 将ISheet导入到List<T>中
        /// </summary>
        /// <param name="isheet"></param>
        /// <returns></returns>
        public static void ImportDt<T>(ISheet isheet , int HeaderRowIndex , T eneityView)
        {
            List<Studen> list = new List<Studen>();
            //获取表头
            var empart = GetDisplayName(eneityView);



            //获取第一行表头
            IRow row = isheet.GetRow(HeaderRowIndex);
            for (int i = 0 ; i < row.Cells.Count ; i++)
            {
                ICell cel = row.GetCell(i);
                string ser = cel.StringCellValue;
                //dic.Add(ser , new List<T>());
            }
        }

        public static DataTable ExcelImportToDataTable(FileStream file)
        {
            DataTable dt = new DataTable();
            //.xlsx  //.xlsm
            #region .xlsx文件处理:XSSFWorkbook
            try
            {

                IWorkbook wb = WorkbookFactory.Create(file);
                ISheet sheet = wb.GetSheetAt(0);//.GetSheet(sheetName);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                IRow headerRow = sheet.GetRow(0);
                //一行最后一个方格的编号 即总的列数 
                for (int j = 0 ; j < sheet.GetRow(0).LastCellNum ; j++)
                {
                    //SET EVERY COLUMN NAME
                    ICell cell = headerRow.GetCell(j);
                    if (cell != null && cell.ToString() != "")
                    {
                        dt.Columns.Add(cell.ToString());
                    }
                    else
                    { continue; }
                }
                int colCount = dt.Columns.Count;
                while (rows.MoveNext())
                {
                    //IRow row = (XSSFRow)sheet.GetRow(j);
                    IRow row = (IRow)rows.Current;
                    DataRow dr = dt.NewRow();
                    bool addDR = false;
                    if (row.RowNum == 0)
                        continue;//The firt row is title,no need import
                    for (int i = 0 ; i < colCount ; i++)
                    {
                        //cell count>column count,then break //每条记录的单元格数量不能大于表格栏位数量 20140213　　　　　　　　　　　　　　　　　
                        //cell count>column count,then break //每条记录的单元格数量不能大于DataTable的title
                        if (i >= colCount)
                        { break; }

                        ICell cell = row.GetCell(i);

                        if ((i == 0) && (string.IsNullOrEmpty(cell.ToString()) == true))//每行第一个cell为空,break
                        {
                            break;
                        }
                        if (cell != null)
                        {
                            object o = cell;
                            //读取Excel格式，根据格式读取数据类型
                            switch (cell.CellType)
                            {
                                case CellType.Blank: //空数据类型处理
                                    o = "";
                                    break;
                                case CellType.String: //字符串类型
                                    o = cell.StringCellValue;
                                    break;
                                case CellType.Numeric: //数字类型  
                                    if (DateUtil.IsCellDateFormatted(cell))
                                    { o = cell.DateCellValue; }
                                    else
                                    {
                                        o = cell.ToString();
                                    }
                                    break;
                                case CellType.Formula:
                                    //HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(hssfworkbook);
                                    IFormulaEvaluator e = WorkbookFactory.CreateFormulaEvaluator(wb);
                                    o = e.Evaluate(cell).StringValue;
                                    break;
                                default:
                                    o = "";
                                    break;
                            }
                            dr[i] = Convert.ToString(o);//row.GetCell(j).StringCellValue;
                            addDR = true;
                        }
                    }
                    if (addDR)
                    {
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            #endregion
            return dt;
        }
    }

}
