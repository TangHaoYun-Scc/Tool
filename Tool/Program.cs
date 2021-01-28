using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basics_Tool.ORM;
using System.Reflection;
using Model;
using Basics_Tool;
using System.Data.OracleClient;
using System.Data;

namespace Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            string sql = "select  * from user_tab_comments";
            DataTable dt = new ConnectOracleDB().GetDataTable(sql);
            for (int i = 0 ; i < dt.Rows.Count ; i++)
            {
                Console.WriteLine(dt.Rows[i]["TABLE_NAME"]);
            }
            Console.ReadLine();
        }
    }
    

}
