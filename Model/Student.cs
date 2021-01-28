using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basics_Tool.ORM;
namespace Model
{
    [TableName("Student")]
    public class Student
    {
        /// <summary>
        /// ID
        /// </summary>
        [ColumnName(ColumnName = "ID", PrimaryKey = true)]
        public int ID { get; set; }
        /// <summary>
        /// name
        /// </summary>
        [ColumnName(ColumnName = "name")]
        public string name { get; set; }
        /// <summary>
        /// height
        /// </summary>
        [ColumnName(ColumnName = "height")]
        public double height { get; set; }
    }
}
