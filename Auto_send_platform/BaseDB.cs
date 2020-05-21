using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;

namespace Auto_send_platform
{
    public sealed class BaseDB
    {
        /// <summary>
        /// 获取 OracleConnection 连接
        /// </summary>
        /// <returns>字符串</returns>
        public static string getOracleConnectionString()
        {
            IniFile inifile = new IniFile(@".\auto_send_platform.ini");
            string ora_conn_str = "Data Source = " + inifile.IniReadValue("DataBase", "ServerName",null) + "; User Id = " +
                inifile.IniReadValue("DataBase", "UserID",null) + "; Password = " + inifile.IniReadValue("DataBase", "DatabasePassword",null);
            return ora_conn_str;
        }
        public static string getOracleConnectionString_tj()
        {
            return null;
            //return ConfigurationManager.ConnectionStrings["connectionString_tj"].ConnectionString;
        }

        #region SQL语句 执行插入，修改，删除操作 开始

        public static bool textExecuteNonQuery(string textSql, OracleParameter[] commandParameters) 
        {
            try
            {
                return OracleHelper.ExecuteNonQuery(getOracleConnectionString(), CommandType.Text, textSql, commandParameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool textExecuteNonQuery(string textSql, OracleParameter commandParameter)
        {
            try
            {
                return OracleHelper.ExecuteNonQuery(getOracleConnectionString(), CommandType.Text, textSql, commandParameter) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool textExecuteNonQuery(string textSql)
        {
            try
            {
                return OracleHelper.ExecuteNonQuery(getOracleConnectionString(), CommandType.Text, textSql) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion SQL语句 执行插入，修改，删除操作 结束

        #region SQL语句 返回Table 开始

        public static DataTable textExecuteDataset(string textSql, OracleParameter[] commandParameters) 
        {
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(getOracleConnectionString(), CommandType.Text, textSql, commandParameters);
                if (ds != null && ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable textExecuteDataset(string textSql, OracleParameter commandParameter)
        {
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(getOracleConnectionString(), CommandType.Text, textSql, commandParameter);
                if (ds != null && ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable textExecuteDataset(string textSql)
        {
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(getOracleConnectionString(), CommandType.Text, textSql);
                if (ds != null && ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion SQL语句 返回Table 结束


        #region 存储过程 执行插入，修改，删除操作 开始

        /// <summary>
        /// 执行指定连接字符串,类型的OracleCommand
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="commandParameters">多个参数</param>
        /// <returns>布尔值（受影响的行是否大于0）</returns>
        public static bool spExecuteNonQuery(string spName, OracleParameter[] commandParameters)
        {
            try
            {
                return OracleHelper.ExecuteNonQuery(getOracleConnectionString(), CommandType.StoredProcedure, spName, commandParameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 执行指定连接字符串,类型的OracleCommand
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="commandParameter">单个参数</param>
        /// <returns>布尔值（受影响的行是否大于0）</returns>
        public static bool spExecuteNonQuery(string spName, OracleParameter commandParameter)
        {
            try
            {
                return OracleHelper.ExecuteNonQuery(getOracleConnectionString(), CommandType.StoredProcedure, spName, commandParameter) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 执行指定连接字符串,类型的OracleCommand
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <returns>布尔值（受影响的行是否大于0）</returns>
        public static bool spExecuteNonQuery(string spName)
        {
            try
            {
                return OracleHelper.ExecuteNonQuery(getOracleConnectionString(), CommandType.StoredProcedure, spName) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 执行指定连接字符串,类型的OracleCommand, 使用了OracleTransaction
        /// </summary>
        /// <param name="spNameLs">多条存储过程名</param>
        /// <param name="commandParametersLs">多组参数</param>
        /// <returns>布尔值(每条语句都执行成功返回True)</returns>
        public static bool spExecuteNonQuery(List<string> spNameLs, List<OracleParameter[]> commandParametersLs)
        {
            bool result = false;
            using (OracleConnection connection = new OracleConnection(getOracleConnectionString()))
            {
                connection.Open();
                OracleTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    for (int i = 0; i < spNameLs.Count; i++)
                    {
                        OracleHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spNameLs[i], commandParametersLs[i]);
                    }
                    transaction.Commit();
                    result = true;
                }
                catch
                {
                    transaction.Rollback();
                    result = false;
                }
                finally
                {
                    transaction.Dispose();
                }
            }

            return result;
        }

        #endregion 存储过程 执行插入，修改，删除操作 结束

        #region 存储过程 返回Table 开始

        public static DataSet spExecuteDataset(string spName, OracleParameter[] commandParameters)
        {
            try
            {
                return OracleHelper.ExecuteDataset(getOracleConnectionString(), CommandType.StoredProcedure, spName, commandParameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataSet spExecuteDataset(string spName, OracleParameter commandParameter)
        {
            try
            {
                return OracleHelper.ExecuteDataset(getOracleConnectionString(), CommandType.StoredProcedure, spName, commandParameter);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataSet spExecuteDataset(string spName)
        {
            try
            {
                return OracleHelper.ExecuteDataset(getOracleConnectionString(), CommandType.StoredProcedure, spName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion 存储过程 返回Table 结束
    }
}