using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basics_Tool.ORM
{
    /// <summary>
    /// 对应数据表名称
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">数据库字段名称</param>
        public TableNameAttribute(string tableName)
        {
            this._tableName = tableName;
        }

        private string _tableName;
        /// <summary>
        /// 对应数据库表名称
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
    }
}
