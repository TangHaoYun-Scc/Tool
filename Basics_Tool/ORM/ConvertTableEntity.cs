using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace Basics_Tool.ORM
{
    /// <summary>
    /// 表和实体之间的转换
    /// </summary>
    public class ConvertTableEntity
    {
        /// <summary>
        /// 创建同态实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>()
        {
            return Activator.CreateInstance<T>();
        }
        /// <summary>
        /// 表转换为list实体集合
        /// thy | 2021年1月28日10:58:18
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static List<T> ConventTableToListModel<T>(DataTable dt)
        {
            var list = new List<T>();
            if (dt == null)
                return list;
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //将表每一行转换为一个实体 然后在将每行放到list中去
                    list.Add(ConventTableRowToModel<T>(dt.Rows[i]));
                }
                return list;
            }

        }
        /// <summary>
        /// 把一行转换为一个实体类
        /// thy | 2021年1月28日10:58:35
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ConventTableRowToModel<T>(DataRow dr)
        {
            if (dr == null)
                throw new Exception("数据行不能为空");
            //创建实体
            var Entity = CreateInstance<T>();
            var pubPropertys = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                var tbColoumnName = dr.Table.Columns[i].ColumnName;//获取列的名称
                foreach (PropertyInfo item in pubPropertys)
                {
                    if (CustomAttributeTool.GetCustomColumnName(item) == tbColoumnName)
                    {
                        object colValue = dr[i];
                        Type coIType = dr.Table.Columns[i].DataType;
                        if (item.CanWrite)
                        {
                            if (colValue != DBNull.Value)
                            {
                                item.SetValue(Entity, Convert.ChangeType(colValue, item.PropertyType), null);
                            }
                        }
                    }
                }
            }
            return Entity;
        }
    }
}
