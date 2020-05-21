using System;
using System.Data;
using System.Collections;
using System.Data.OracleClient;

namespace Auto_send_platform
{
    public sealed class OracleHelper
    {

        #region 私有构造函数和方法

        private OracleHelper() { }


        /// <summary>
        /// 将OracleParameter参数数组(参数值)分配给OracleCommand命令. 
        /// 这个方法将给任何一个参数分配DBNull.Value; 
        /// 该操作将阻止默认值的使用. 
        /// </summary>
        /// <param name="command">命令名</param>
        /// <param name="commandParameters">OracleParameter数组</param>
        private static void AttachParameters(OracleCommand command, OracleParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (OracleParameter commandParameter in commandParameters)
                {
                    if (commandParameter != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value. 
                        if ((commandParameter.Direction == ParameterDirection.InputOutput || commandParameter.Direction == ParameterDirection.Input) &&
                            (commandParameter.Value == null))
                        {
                            commandParameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(commandParameter);
                    }
                }
            }
        }


        /// <summary>
        /// 将DataRow类型的列值分配到SqlParameter参数数组. 
        /// </summary>
        /// <param name="commandParameters">要分配值的SqlParameter参数数组</param>
        /// <param name="dataRow">将要分配给存储过程参数的DataRow</param>
        private static void AssignParameterValues(OracleParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
            {
                return;
            }

            int i = 0;

            // 设置参数值
            foreach (OracleParameter commandParameter in commandParameters)
            {
                // 创建参数名称,如果不存在,只抛出一个异常. 
                if (commandParameter.ParameterName == null ||
                    commandParameter.ParameterName.Length <= 1)
                    throw new Exception(
                        string.Format("请提供参数{0}一个有效的名称{1}.", i, commandParameter.ParameterName));

                // 从dataRow的表中获取为参数数组中数组名称的列的索引. 
                // 如果存在和参数名称相同的列,则将列值赋给当前名称的参数. 
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i++;
            }
        }


        /// <summary>
        /// 将一个对象数组分配给SqlParameter参数数组. 
        /// </summary>
        /// <param name="commandParameters">要分配值的SqlParameter参数数组</param>
        /// <param name="parameterValues">将要分配给存储过程参数的对象数组</param>
        private static void AssignParameterValues(OracleParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                return;
            }

            // 确保对象数组个数与参数个数匹配,如果不匹配,抛出一个异常. 
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("参数值个数与参数不匹配.");
            }

            // 给参数赋值 
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property 
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }


        /// <summary>
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数
        /// </summary>
        /// <param name="command">要处理的OracleCommand</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">一个有效的事务或者是null值</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">存储过程名或都T-SQL命令文本</param>
        /// <param name="commandParameters">和命令相关联的SqlParameter参数数组,如果没有参数为'null'</param>
        /// <param name="mustCloseConnection"><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param>
        private static void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it 
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // 给命令分配一个数据库连接. 
            command.Connection = connection;

            // 设置命令文本(存储过程名或SQL语句) 
            command.CommandText = commandText;

            // 分配事务 
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                //command.Transaction = transaction;
            }

            // 设置命令类型. 
            command.CommandType = commandType;

            // 分配命令参数 
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }


        #endregion 私有构造函数和方法结束


        #region 数据库连接


        /// <summary>
        /// 一个有效的数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetConnSting()
        {
            return null;
        }


        public static OracleConnection GetConnection(string connStr)
        {
            OracleConnection Connection = new OracleConnection(connStr);
            return Connection;
        }


        #endregion 数据库连接


        #region ExecuteNonQuery命令


        /// <summary>
        /// 执行指定连接字符串,类型的OracleCommand. 
        /// </summary>
        /// <remarks>
        /// 示例:  
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders"); 
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">存储过程名称或SQL语句</param>
        /// <returns>返回命令影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandType, commandText, (OracleParameter[])null);
        }


        /// <summary>
        /// 执行指定连接字符串,类型的OracleCommand.如果没有提供参数,不返回结果. 
        /// </summary>
        /// <remarks>
        /// 示例:  
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24)); 
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">存储过程名称或SQL语句</param>
        /// <param name="commandParameters">SqlParameter参数数组</param>
        /// <returns>返回命令影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }


        /// <summary>
        /// 执行指定连接字符串的存储过程,将对象数组的值赋给存储过程参数,
        /// 此方法需要在参数缓存方法中探索参数并生成参数. 
        /// </summary>
        /// <remarks>
        /// 这个方法没有提供访问输出参数和返回值. 
        /// 示例:  
        /// int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36); 
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="parameterValues">分配到存储过程输入参数的对象数组</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // 如果存在参数值 
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从探索存储过程参数(加载到缓存)并分配给存储过程参数数组. 
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // 给存储过程参数赋值 
                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 没有参数情况下 
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }


        /// <summary>
        /// 执行指定数据库连接对象的命令
        /// </summary>
        /// <remarks>
        /// 示例: 
        /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders"); 
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, (OracleParameter[])null);
        }


        /// <summary>
        /// 执行指定数据库连接对象的命令
        /// </summary>
        /// <remarks>
        /// 示例:  
        /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24)); 
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
        /// <param name="commandText">T存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">OracleParamter参数数组</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // 创建OracleCommand命令,并进行预处理
            OracleCommand cmd = new OracleCommand();

            bool mustCloseConnection = false;

            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // Finally, execute the command 
            int retval = cmd.ExecuteNonQuery();
            

            // 清除参数,以便再次使用. 
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }


        /// <summary>
        /// 执行指定数据库连接对象的命令,将对象数组的值赋给存储过程参数. 
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值 
        /// 示例:
        /// int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36); 
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(OracleConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // 如果有参数值 
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从缓存中加载存储过程参数 
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(connection, spName);

                // 给存储过程分配参数值 
                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }


        /// <summary>
        /// 执行带事务的SqlCommand. 
        /// </summary>
        /// 示例.:  
        /// int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders"); 
        /// <param name="transaction">一个有效的数据库连接对象</param>
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary>
        /// 执行带事务的SqlCommand(指定参数). 
        /// </summary>
        /// <remarks>
        /// 示例:  
        /// int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24)); 
        /// </remarks>
        /// <param name="transaction">一个有效的数据库连接对象</param>
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">SqlParamter参数数组</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // 预处理 
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // 执行 
            int retval = cmd.ExecuteNonQuery();

            // 清除参数集,以便再次使用. 
            cmd.Parameters.Clear();
            return retval;
        }


        /// <summary>
        /// 执行带事务的SqlCommand(指定参数值). 
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值 
        /// 示例:
        /// int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36); 
        /// </remarks>
        /// <param name="transaction">一个有效的数据库连接对象</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
        /// <returns>返回受影响的行数</returns>
        public static int ExecuteNonQuery(OracleTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // 如果有参数值 
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. 
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 给存储过程参数赋值 
                AssignParameterValues(commandParameters, parameterValues);

                // 调用重载方法 
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 没有参数值 
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteNonQuery命令


        #region ExecuteDataset方法


        /// <summary>
        /// 执行指定数据库连接字符串的命令,返回DataSet. 
        /// </summary>
        /// <remarks>
        /// 示例:  
        /// DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders"); 
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connectionString, commandType, commandText, (OracleParameter[])null);
        }


        /// <summary>
        /// 执行指定数据库连接字符串的命令,返回DataSet. 
        /// </summary>
        /// <remarks>
        /// 示例: 
        /// DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">OracleParamters参数数组</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            // 创建并打开数据库连接对象,操作完成释放对象. 
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                // 调用指定数据库连接字符串重载方法. 
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// 执行指定数据库连接字符串的命令,直接提供参数值,返回DataSet. 
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值.
        /// 示例: 
        /// DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36); 
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从缓存中检索存储过程参数 
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // 给存储过程参数分配值 
                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }


        /// <summary>
        /// 执行指定数据库连接对象的命令,返回DataSet. 
        /// </summary>
        /// <remarks>
        /// 示例:
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders"); 
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, commandType, commandText, (OracleParameter[])null);
        }


        /// <summary>
        /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataSet. 
        /// </summary>
        /// <remarks>
        /// 示例: 
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24)); 
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">SqlParamter参数数组</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // 预处理 
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // 创建SqlDataAdapter和DataSet. 
            using (OracleDataAdapter da = new OracleDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                // 填充DataSet. 
                da.Fill(ds);

                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    connection.Close();

                return ds;
            }
        }


        /// <summary>
        /// 执行指定数据库连接对象的命令,指定参数值,返回DataSet. 
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输入参数和返回值. 
        /// 示例.: 
        /// DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36); 
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet ExecuteDataset(OracleConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 比缓存中加载存储过程参数 
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(connection, spName);

                // 给存储过程参数分配值 
                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
        }


        /// <summary>
        /// 执行指定事务的命令,返回DataSet. 
        /// </summary>
        /// <remarks>
        /// 示例: 
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders"); 
        /// </remarks>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet ExecuteDataset(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataset(transaction, commandType, commandText, (OracleParameter[])null);
        }


        /// <summary>
        /// 执行指定事务的命令,指定参数,返回DataSet.
        /// </summary>
        /// <remarks>
        /// 示例:   
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">SqlParamter参数数组</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet ExecuteDataset(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // 预处理 
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // 创建 DataAdapter & DataSet 
            using (OracleDataAdapter da = new OracleDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
        }


        /// <summary>
        /// 执行指定事务的命令,指定参数值,返回DataSet.
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输入参数和返回值. 
        /// 示例.:
        /// DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36); 
        /// </remarks>
        /// <param name="transaction">事务</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public static DataSet ExecuteDataset(OracleTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从缓存中加载存储过程参数 
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 给存储过程参数分配值 
                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }


        #endregion ExecuteDataset数据集命令结束


        #region ExecuteScalar 返回结果集中的第一行第一列


        /// <summary>
        /// 执行指定数据库连接字符串的命令,返回结果集中的第一行第一列.
        /// </summary>
        /// <remarks>
        /// 示例: 
        /// int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount"); 
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            // 执行参数为空的方法 
            return ExecuteScalar(connectionString, commandType, commandText, (OracleParameter[])null);
        }


        /// <summary>
        /// 执行指定数据库连接字符串的命令,指定参数,返回结果集中的第一行第一列. 
        /// </summary>
        /// <remarks>
        /// 示例:  
        /// int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new OracleParameter("@prodid", 24)); 
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">分配给命令的OracleParamter参数数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            // 创建并打开数据库连接对象,操作完成释放对象. 
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                // 调用指定数据库连接字符串重载方法. 
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
        }


        /// <summary>
        /// 执行指定数据库连接字符串的命令,指定参数值,返回结果集中的第一行第一列. 
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数.
        /// int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", 24, 36); 
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // 如果有参数值 
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. () 
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // 给存储过程参数赋值 
                AssignParameterValues(commandParameters, parameterValues);

                // 调用重载方法 
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 没有参数值 
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }


        /// <summary>
        /// 执行指定数据库连接对象的命令,返回结果集中的第一行第一列. 
        /// </summary>
        /// <remarks>
        /// 示例:  
        /// int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount"); 
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText)
        {
            // 执行参数为空的方法 
            return ExecuteScalar(connection, commandType, commandText, (OracleParameter[])null);
        }


        /// <summary> 
        /// 执行指定数据库连接对象的命令,指定参数,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new OracleParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <param name="commandParameters">分配给命令的OracleParamter参数数组</param> 
        /// <returns>返回结果集中的第一行第一列</returns> 
        public static object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // 创建SqlCommand命令,并进行预处理 
            OracleCommand cmd = new OracleCommand();

            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // 执行SqlCommand命令,并返回结果. 
            object retval = cmd.ExecuteScalar();

            // 清除参数,以便再次使用. 
            cmd.Parameters.Clear();

            if (mustCloseConnection)
                connection.Close();

            return retval;
        }


        /// <summary>
        /// 执行指定数据库连接对象的命令,指定参数值,返回结果集中的第一行第一列. 
        /// </summary>
        /// <remarks>
        /// 此方法不提供访问存储过程输出参数和返回值参数. 
        /// 示例: 
        /// int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36); 
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(OracleConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // 如果有参数值 
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从缓存中加载存储过程参数,如果缓存中不存在则从数据库中检索参数信息并加载到缓存中. () 
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(connection, spName);

                // 给存储过程参数赋值 
                AssignParameterValues(commandParameters, parameterValues);

                // 调用重载方法 
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 没有参数值 
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
        }


        /// <summary>
        /// 执行指定数据库事务的命令,返回结果集中的第一行第一列. 
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount"); 
        /// </remarks>
        /// <param name="transaction">一个有效的连接事务</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // 执行参数为空的方法 
            return ExecuteScalar(transaction, commandType, commandText, (OracleParameter[])null);
        }


        /// <summary>
        /// 执行指定数据库事务的命令,指定参数,返回结果集中的第一行第一列. 
        /// </summary>
        /// <remarks>
        /// 示例: 
        /// int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)); 
        /// </remarks>
        /// <param name="transaction">一个有效的连接事务</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">分配给命令的OracleParamter参数数组</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // 创建SqlCommand命令,并进行预处理 
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // 执行SqlCommand命令,并返回结果. 
            object retval = cmd.ExecuteScalar();

            // 清除参数,以便再次使用. 
            cmd.Parameters.Clear();
            return retval;
        }


        /// <summary> 
        /// 执行指定数据库事务的命令,指定参数值,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <remarks> 
        /// 此方法不提供访问存储过程输出参数和返回值参数. 
        /// 
        /// 示例:  
        ///  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36); 
        /// </remarks> 
        /// <param name="transaction">一个有效的连接事务</param> 
        /// <param name="spName">存储过程名称</param> 
        /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param> 
        /// <returns>返回结果集中的第一行第一列</returns> 
        public static object ExecuteScalar(OracleTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // 如果有参数值 
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // PPull the parameters for this stored procedure from the parameter cache () 
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 给存储过程参数赋值 
                AssignParameterValues(commandParameters, parameterValues);

                // 调用重载方法 
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 没有参数值 
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }


        #endregion ExecuteScalar





        /// <summary>
        /// OracleHelperParameterCache提供缓存存储过程参数,并能够在运行时从存储过程中探索参数. 
        /// </summary>
        private sealed class OracleHelperParameterCache
        {

            #region 私有方法,字段,构造函数

            // 私有构造函数,妨止类被实例化.
            private OracleHelperParameterCache() { }


            // 这个方法要注意 
            private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());


            /// <summary>
            /// 探索运行时的存储过程,返回SqlParameter参数数组. 
            /// 初始化参数值为 DBNull.Value.
            /// </summary>
            /// <param name="connection">一个有效的数据库连接</param>
            /// <param name="spName">存储过程名称</param>
            /// <param name="includeReturnValueParameter">是否包含返回值参数</param>
            /// <returns>返回SqlParameter参数数组</returns>
            private static OracleParameter[] DiscoverSpParameterSet(OracleConnection connection, string spName, bool includeReturnValueParameter)
            {
                if (connection == null) throw new ArgumentNullException("connection");
                if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

                OracleCommand cmd = new OracleCommand(spName, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                // 检索cmd指定的存储过程的参数信息,并填充到cmd的Parameters参数集中. 
                OracleCommandBuilder.DeriveParameters(cmd);
                connection.Close();

                // 如果不包含返回值参数,将参数集中的每一个参数删除.
                if (!includeReturnValueParameter)
                {
                    cmd.Parameters.RemoveAt(0);
                }

                // 创建参数数组
                OracleParameter[] discoveredParameters = new OracleParameter[cmd.Parameters.Count];

                // 将cmd的Parameters参数集复制到discoveredParameters数组. 
                foreach (OracleParameter discoveredParameter in discoveredParameters)
                {
                    discoveredParameter.Value = DBNull.Value;
                }
                return discoveredParameters;
            }


            /// <summary>
            /// OracleParameter参数数组的深层拷贝. 
            /// </summary>
            /// <param name="originalParameters">原始参数数组</param>
            /// <returns>返回一个同样的参数数组</returns>
            private static OracleParameter[] CloneParameters(OracleParameter[] originalParameters)
            {
                OracleParameter[] clonedParameters = new OracleParameter[originalParameters.Length];

                for (int i = 0, j = originalParameters.Length; i < j; i++)
                {
                    clonedParameters[i] = (OracleParameter)((ICloneable)originalParameters[i]).Clone();
                }

                return clonedParameters;
            }

            #endregion 私有方法,字段,构造函数


            #region 缓存方法

            /// <summary>
            /// 追加参数数组到缓存. 
            /// </summary>
            /// <param name="connectionString">一个有效的数据库连接字符串</param>
            /// <param name="commandText">存储过程名或SQL语句</param>
            /// <param name="commandParameters">要缓存的参数数组</param>
            public static void CacheParameterSet(string connectionString, string commandText, params OracleParameter[] commandParameters)
            {
                if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

                string hashKey = connectionString + ":" + commandText;

                paramCache[hashKey] = commandParameters;
            }


            /// <summary>
            /// 从缓存中获取参数数组.
            /// </summary>
            /// <param name="connectionString">一个有效的数据库连接字符</param>
            /// <param name="commandText">存储过程名或SQL语句</param>
            /// <returns>参数数组</returns>
            public static OracleParameter[] GetCachedParameterSet(string connectionString, string commandText)
            {
                if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

                string hashKey = connectionString + ":" + commandText;

                OracleParameter[] cachedParameters = paramCache[hashKey] as OracleParameter[];
                if (cachedParameters == null)
                {
                    return null;
                }
                else
                {
                    return CloneParameters(cachedParameters);
                }
            }

            #endregion 缓存方法


            #region 检索指定的存储过程的参数集


            /// <summary>
            /// 返回指定的存储过程的参数集
            /// </summary>
            /// <remarks>
            /// 这个方法将查询数据库,并将信息存储到缓存. 
            /// </remarks>
            /// <param name="connectionString">一个有效的数据库连接字符</param>
            /// <param name="spName">存储过程名</param>
            /// <returns>返回SqlParameter参数数组</returns>
            public static OracleParameter[] GetSpParameterSet(string connectionString, string spName)
            {
                return GetSpParameterSet(connectionString, spName, false);
            }


            /// <summary>
            /// 返回指定的存储过程的参数集
            /// </summary>
            /// <remarks>
            /// 这个方法将查询数据库,并将信息存储到缓存. 
            /// </remarks>
            /// <param name="connectionString">一个有效的数据库连接字符.</param>
            /// <param name="spName">存储过程名</param>
            /// <param name="includeReturnValueParameter">是否包含返回值参数</param>
            /// <returns>返回SqlParameter参数数组</returns>
            public static OracleParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
            {
                if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
                }
            }


            /// <summary>
            /// [内部]返回指定的存储过程的参数集(使用连接对象). 
            /// </summary>
            /// <remarks>
            /// 这个方法将查询数据库,并将信息存储到缓存. 
            /// </remarks>
            /// <param name="connection">一个有效的数据库连接字符</param>
            /// <param name="spName">存储过程名</param>
            /// <returns>返回SqlParameter参数数组</returns>
            internal static OracleParameter[] GetSpParameterSet(OracleConnection connection, string spName)
            {
                return GetSpParameterSet(connection, spName, false);
            }


            /// <summary>
            /// [内部]返回指定的存储过程的参数集(使用连接对象)
            /// </summary>
            /// <remarks>
            /// 这个方法将查询数据库,并将信息存储到缓存. 
            /// </remarks>
            /// <param name="connection">一个有效的数据库连接对象</param>
            /// <param name="spName">存储过程名</param>
            /// <param name="includeReturnValueParameter">是否包含返回值参数 </param>
            /// <returns>返回OracleParameter参数数组</returns>
            internal static OracleParameter[] GetSpParameterSet(OracleConnection connection, string spName, bool includeReturnValueParameter)
            {
                if (connection == null) throw new ArgumentNullException("connection");
                using (OracleConnection clonedConnection = (OracleConnection)((ICloneable)connection).Clone())
                {
                    return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
                }
            }

            /// <summary>
            /// [私有]返回指定的存储过程的参数集(使用连接对象)
            /// </summary>
            /// <param name="connection">一个有效的数据库连接对象</param>
            /// <param name="spName">存储过程名</param>
            /// <param name="includeReturnValueParameter">是否包含返回值参数</param>
            /// <returns>返回SqlParameter参数数组</returns>
            private static OracleParameter[] GetSpParameterSetInternal(OracleConnection connection, string spName, bool includeReturnValueParameter)
            {
                if (connection == null) throw new ArgumentNullException("connection");
                if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

                string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

                OracleParameter[] cachedParameters;

                cachedParameters = paramCache[hashKey] as OracleParameter[];

                if (cachedParameters == null)
                {
                    OracleParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                    paramCache[hashKey] = spParameters;
                    cachedParameters = spParameters;
                }

                return CloneParameters(cachedParameters);
            }


            #endregion 检索指定的存储过程的参数集

        }
    }
}
