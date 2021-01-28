using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Basics_Tool.ORM
{
    /// <summary>
    /// 自定义属性工具
    /// </summary>
    public class CustomAttributeTool
    {
        /// <summary>
        /// 获取自定义表名
        /// thy | 2021年1月28日11:29:14
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetCustomTableName<T>()
        {
            var classPropertys = typeof(T).GetCustomAttributes(false);
            for (int i = 0; i < classPropertys.Length; i++)
            {
                var tableAttribute = classPropertys[i] as TableNameAttribute;
                //是否是自定义表属性
                if (tableAttribute != null && !string.IsNullOrEmpty(tableAttribute.TableName))
                {
                    return tableAttribute.TableName;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取自定义字段名
        /// thy | 2021年1月28日10:59:06
        /// </summary>
        /// <param name="_Prop"></param>
        /// <returns></returns>
        public static string GetCustomColumnName(PropertyInfo _Prop)
        {
            ColumnNameAttribute colAttribute = GetCustomColumnNameAttribute(_Prop);
            //是否是自定义字段属性
            if (colAttribute != null && !string.IsNullOrEmpty(colAttribute.ColumnName))
                return colAttribute.ColumnName;//返回字段名
            return string.Empty;//返回空字符串
        }
        /// <summary>
        /// 获取自定义字段名
        /// thy | 2021年1月28日10:58:57
        /// </summary>
        /// <param name="Prop"></param>
        /// <returns></returns>
        public static ColumnNameAttribute GetCustomColumnNameAttribute(PropertyInfo pInfo)
        {
            var cps = pInfo.GetCustomAttributes(typeof(ColumnNameAttribute),false);

            if (cps != null && cps.Length > 0)
                return cps[0] as ColumnNameAttribute;
            else return null;
        }
        /// <summary>
        /// 生成Update sql语句
        /// thy | 2021年1月28日13:46:08
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="Expects">修改参数</param>
        /// <returns></returns>
        public static string GetUpdateSql<T>(T obj, List<string> Expects)
        {
            //uppdate table_name set value =newValue and value = newValue where id = id
            //获取表名
            var tableName = GetCustomTableName<T>();
            //容器
            System.Text.StringBuilder stbUpdate = new System.Text.StringBuilder();
            System.Text.StringBuilder stbWhere = new System.Text.StringBuilder();
            stbUpdate.Append("UPDATE " + tableName + " SET ");

            var proPertyInfoes = obj.GetType().GetProperties();
            foreach (PropertyInfo item in proPertyInfoes)
            {
                if (Expects.Any(x => x == item.Name))
                    continue;
                if (item.Equals("CUD"))
                    continue;
                //获取自定义字段名
                var c = GetCustomColumnNameAttribute(item);
                var tmpName = item.Name;
                if (c.PrimaryKey)//判断是否是主键
                {
                    stbWhere.Append(string.Format(" WHERE {0}={1}", tmpName, item.GetValue(obj, null)));
                    continue;
                }
            }


            return "";
        }
        public static string GetUpdateSql2<T>(T obj, List<string> Expects)
        {

            string tableName = GetCustomTableName<T>();
            System.Text.StringBuilder stbUpdate = new System.Text.StringBuilder();
            System.Text.StringBuilder stbWhere = new System.Text.StringBuilder();
            stbUpdate.Append("Update " + tableName);
            stbUpdate.Append(" set ");
            var proPertyInfoes = obj.GetType().GetProperties();
            try
            {
                foreach (var x in proPertyInfoes)
                {
                    if (Expects.Any(xx => xx == x.Name))
                        continue;

                    if (x.Name.Equals("CUD"))
                        continue;
                    var c = GetCustomColumnNameAttribute(x);
                    var tmpName = x.Name;
                    if (c.PrimaryKey == true)
                    {
                        stbWhere.Append(string.Format(" where {0}='{1}'", tmpName, x.GetValue(obj, null)));
                        continue;
                    }

                    if (x.Name.EndsWith("_LSTUPDDT"))
                    {
                        stbUpdate.Append(string.Format("{0}=sysdate ,", tmpName));
                    }
                    else if ((x.Name.EndsWith("_INSERTDT") || x.Name.EndsWith("INSERTDT")))
                    {
                        continue;
                    }
                    else if (x.Name == "DP_DELIVERTIME" || x.Name == "INSERT_DATE" || x.Name == "ALTER_DATE" || x.Name == "WARNING_START" || x.Name == "IYC_BEFULLTM" || x.Name == "SP_APPLYTIME" || x.Name == "SP_SENDTIME" || x.Name == "SP_EXAMINETIME" || x.Name == "SP_CUSTOMTIME" || x.Name == "SP_CHANGTIME")
                    {
                        if (x.GetValue(obj, null) == null)
                        {
                            continue;
                        }
                        else
                            stbUpdate.Append(string.Format("{0}=to_date('" + x.GetValue(obj, null) + "','yyyy-mm-dd HH24:MI:SS'),", tmpName));
                    }
                    else
                        stbUpdate.Append(string.Format("{0}='{1}',", tmpName, x.GetValue(obj, null)));
                }
                return stbUpdate.ToString().TrimEnd(',') + stbWhere.ToString();
            }
            catch (Exception)
            {

                throw;
            }


        }

        /// <summary>
        /// 生成insert sql 语句
        /// thy | 2021年1月28日14:00:53
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetInsertSql<T>(T obj)
        {
            string tableName = GetCustomTableName<T>();
            System.Text.StringBuilder stbInsert = new System.Text.StringBuilder();
            System.Text.StringBuilder stbValues = new System.Text.StringBuilder();
            stbInsert.Append("Insert into " + tableName + "(");
            stbValues.Append(" values(");
            var proPertyInfoes = obj.GetType().GetProperties();
            foreach (var x in proPertyInfoes)
            {
                if (x.Name.Equals("CUD") || x.Name.Equals("计划开工时间") || x.Name.Equals("实际开工时间") || x.Name.Equals("计划完工时间") || x.Name.Equals("实际完工时间") || x.Name.Equals("船舶代码"))
                    continue;
                var tmpName = x.Name;
                stbInsert.Append(string.Format("{0},", tmpName));

                if ((x.Name.EndsWith("_INSERTDT") || x.Name.EndsWith("_LSTUPDDT") || x.Name.EndsWith("INSERTDT")))
                {
                    stbValues.Append("sysdate,");
                }
                else
                    stbValues.Append(string.Format(":{0},", tmpName));
            }
            return stbInsert.ToString().TrimEnd(',') + ")" + stbValues.ToString().TrimEnd(',') + ")";
        }
        public static string GetInsertSql2<T>(T obj)
        {
            string tableName = GetCustomTableName<T>();
            System.Text.StringBuilder stbInsert = new System.Text.StringBuilder();
            System.Text.StringBuilder stbValues = new System.Text.StringBuilder();
            stbInsert.Append("Insert into " + tableName + "(");
            stbValues.Append(" values(");
            var proPertyInfoes = obj.GetType().GetProperties();
            foreach (var x in proPertyInfoes)
            {
                if (x.Name.Equals("CGD_CGPKS") || x.Name.Equals("CGD_CGVOL") || x.Name.Equals("CGD_CGWTG") || x.Name.Equals("计划完工时间") || x.Name.Equals("实际完工时间") || x.Name.Equals("船舶代码"))
                    continue;
                var tmpName = x.Name;
                if ((x.Name.EndsWith("SP_INSERTTIME") || x.Name.EndsWith("SP_CHANGMAN") || x.Name.EndsWith("INSERTDT") || x.Name.EndsWith("INSERTDT") || x.Name.Equals("INPORTENDDATE")))
                {
                    stbInsert.Append(string.Format("{0},", tmpName));
                    stbValues.Append("sysdate,");
                    //stbValues.Append("to_date('" + x.GetValue(obj, null) + "','yyyy-mm-dd HH24:MI:SS'),");
                }
                else if (x.Name == "DP_DELIVERTIME" || x.Name == "INSERT_DATE" || x.Name == "ALTER_DATE" || x.Name == "WARNING_START" || x.Name == "IYC_BEFULLTM" || x.Name == "SP_APPLYTIME" || x.Name == "SP_TWOSHIPTIME" || x.Name == "SP_SENDTIME" || x.Name == "SP_EXAMINETIME" || x.Name == "SP_CUSTOMTIME")
                {
                    if (x.GetValue(obj, null) == null)
                    {
                        continue;
                    }
                    else
                    {
                        stbInsert.Append(string.Format("{0},", tmpName));
                        stbValues.Append("to_date('" + x.GetValue(obj, null) + "','yyyy-mm-dd HH24:MI:SS'),");
                    }
                }
                else if (x.Name == "SCD_ETD")
                {
                    stbInsert.Append(string.Format("{0},", tmpName));
                    stbValues.Append("to_date('" + x.GetValue(obj, null) + "','yyyy-mm-dd HH24:MI:SS'),");
                }
                else
                {
                    stbInsert.Append(string.Format("{0},", tmpName));
                    stbValues.Append(string.Format("'{0}',", x.GetValue(obj, null)));
                }
            }
            return stbInsert.ToString().TrimEnd(',') + ")" + stbValues.ToString().TrimEnd(',') + ")";
        }
    }
}
