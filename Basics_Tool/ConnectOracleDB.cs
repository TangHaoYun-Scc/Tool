using System.Data;
using System.Data.OracleClient;

namespace Basics_Tool
{
    public class ConnectOracleDB
    {
        private OracleConnection Conn;
        private OracleCommand Cmd;
        private string ConnStr { get; set; }

        public ConnectOracleDB()
        {
            ConnStr = "Data Source=WJ_KFN;User ID=AHSY;Password=AHSY;Unicode=True;Pooling = true;Min Pool Size = 1;Max Pool Size = 75;Connection Lifetime = 60;";
            OpenConn();
        }

        public ConnectOracleDB(string Key)
        {
            ConnStr = Key;
            OpenConn();
        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        private void OpenConn()
        {
            if (Conn == null)
                Conn = new OracleConnection(ConnStr);
            else if (Conn.State == System.Data.ConnectionState.Closed)
                Conn.Open();
            else if (Conn.State == System.Data.ConnectionState.Broken)
            {
                Conn.Close();
                Conn.Open();
            }
        }
        /// <summary>
        /// 关闭数据库
        /// </summary>
        private void CloseConn()
        {
            if (Conn != null && Conn.State == System.Data.ConnectionState.Open)
                Conn.Close();
        }

        /// <summary>
        /// 获取DataSet
        /// thy | 2021年1月28日17:17:10
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql)
        {
            using (OracleDataAdapter da = new OracleDataAdapter(sql , Conn))
            {
                DataSet ds = new DataSet();
                da.Fill(ds);
                CloseConn();
                return ds;
            }
        }
        /// <summary>
        /// 获取DataTable 
        /// thy | 2021年1月28日17:18:32
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            return GetDataSet(sql).Tables[0];
        }
        /// <summary>
        /// 执行sql语句
        /// thy | 2021年1月28日17:37:05
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Parm"></param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteSql(string sql , OracleParameter[] Parm)
        {
            OracleCommand cmd = new OracleCommand();
            BuilderCommand(sql , out cmd , CommandType.Text , Parm);
            CloseConn();
            return cmd.ExecuteNonQuery();

        }
        /// <summary>
        /// 构建返回Command
        /// thy | 2021年1月28日17:37:12
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="cmd">需要返回的Command</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="param">参数列表</param>
        public void BuilderCommand(string sql , out OracleCommand cmd , CommandType cmdType , params OracleParameter[] parameter)
        {
            cmd = new OracleCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = sql;
            cmd.Connection = Conn;
            cmd = Conn.CreateCommand();
            foreach (var item in parameter)
            {
                cmd.Parameters.Add(item);
            }
        }


    }
}
