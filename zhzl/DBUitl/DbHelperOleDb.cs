using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Configuration;


namespace DBUitl
{

    /// <summary>
    /// Copyright (C) 2004-2008 CB_Article 
    /// 数据访问基础类(基于SQLServer)
    /// 用户可以修改满足自己项目的需要。
    /// </summary>
    public abstract class DbHelperOleDb
    {
        //数据库连接字符串(web.config来配置)
        //<add key="ConnectionString" value="server=.;database=DATABASE;uid=sa;pwd=" />		
        //protected static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public static readonly string connectionString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["dbPath"]);

        public DbHelperOleDb()
        {
        }

        #region  执行简单SQL语句

        public static bool Exists(string strSql, params OleDbParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行SQL语句,返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (OleDbException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句,返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (OleDbException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句,返回OleDbDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string strSQL)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand(strSQL, connection);
            try
            {
                connection.Open();
                OleDbDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (OleDbException e)
            {
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// 执行查询语句,返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OleDbDataAdapter command = new OleDbDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (OleDbException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }


        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句,返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (OleDbException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        public static int ExecuteSql2(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.CommandText = "select @@identity as id";
                        cmd.Parameters.Clear();
                        int id = Convert.ToInt32(cmd.ExecuteScalar());
                        return id;
                    }
                    catch (OleDbException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }


        /// <summary>
        /// 执行一条计算查询结果语句,返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (OleDbException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句,返回OleDbDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string SQLString, params OleDbParameter[] cmdParms)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                OleDbDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (OleDbException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句,返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (OleDbException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }


        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, string cmdText, OleDbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion


        #region 自定义方法

        //public static string GetTableName(string Sql)
        //{
        //    string x, GetTableName;
        //    int y;
        //    Sql = (Sql + " ").ToLower();

        //    x = Sql.Substring(Sql.IndexOf("from") + 5);
        //    y = x.IndexOf(" ");
        //    GetTableName = x.Substring(0, y);
        //    return GetTableName;
        //}

        public static DataTable GetTable(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand();
                PrepareCommand(cmd, connection, null, SQLString, null);
                using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        //string Tb = GetTableName(SQLString);
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                        da.Dispose();
                        return ds.Tables[0];
                    }
                    catch (OleDbException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public static int ExeCount(string Sql)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand();
                PrepareCommand(cmd, connection, null, Sql, null);
                try
                {
                    int Count = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();
                    return Count;
                }
                catch
                {
                    cmd.Dispose();
                    return 0;
                }
            }
        }

        public static object ExecuteSqlStringScalar(string cmdText)
        {
            return ExecuteScalar(cmdText, CommandType.Text, null);
        }


        /// <summary>
        /// 执行Sql语句,返回查询结果(Object).
        /// </summary>
        /// <param name="cmdText">Sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        private static object ExecuteScalar(string cmdText, CommandType cmdType, params OleDbParameter[] parameters)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);

                        object result = cmd.ExecuteScalar();

                        if (cmd.Parameters.Count > 0)
                        {
                            cmd.Parameters.Clear();
                        }

                        return result;
                    }
                    catch (OleDbException ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
        }

        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, CommandType cmdType, string cmdText, OleDbParameter[] parameters)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            if (parameters != null)
            {
                foreach (OleDbParameter parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
        }



        public static DataSet ExecuteSqlStringPagedDataSet(string cmdText, int startIndex, int pageSize)
        {
            return ExecutePagedDataSet(cmdText, CommandType.Text, startIndex, pageSize, null);
        }


        /// <summary>
        /// 执行Sql语句,返回分页 DataSet.
        /// </summary>
        /// <param name="cmdText">Sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        private static DataSet ExecutePagedDataSet(string cmdText, CommandType cmdType, int startIndex, int pageSize, params OleDbParameter[] parameters)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, cmdType, cmdText, parameters);

                        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds, startIndex, pageSize, "ds");

                        if (cmd.Parameters.Count > 0)
                        {
                            cmd.Parameters.Clear();
                        }

                        return ds;
                    }
                    catch (OleDbException ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
        }

        #endregion
    }
}
