using System;

namespace Basics_Tool.ORM
{
    /// <summary>
    /// 对应数据库字段名称
    /// thy | 2021年1月28日14:58:57
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnNameAttribute : Attribute
    {

        private string _columnName;
        /// <summary>
        /// 对应数据库字段名称
        /// </summary>
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private bool _primaryKey = false;
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool PrimaryKey
        {
            get { return _primaryKey; }
            set { _primaryKey = value; }
        }

        private bool _isDbColumn = true;
        /// <summary>
        /// 是否是db字段
        /// </summary>
        public bool IsDbColumn
        {
            get { return _isDbColumn; }
            set { _isDbColumn = value; }
        }

        /// <summary> 
        /// 中文名称
        /// </summary>
        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
    }
}
