/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using Dm;
using EIP.Common.Models.Resx;
using EIP.Common.Core.Util;
using Kdbndp;
using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace EIP.Common.Repository
{
    public class DbHelper
    {
        private readonly string _connectionString;
        private readonly string _connectionType;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public DbHelper(string connectionString)
        {
            _connectionString = connectionString;
            _connectionType = ConfigurationUtil.GetDbConnectionType();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public DbHelper(string connectionString,string connectionType)
        {
            _connectionString = connectionString;
            _connectionType = connectionType;
        }
        #region 测试
        public void Test(string type, string connectionString)
        {
            try
            {
                DbConnection conn;
                switch (type)
                {
                    case ResourceDataBaseType.Mysql:
                        conn = new MySqlConnection(connectionString);
                        break;
                    case ResourceDataBaseType.Postgresql:
                        conn = new NpgsqlConnection(connectionString);
                        break;
                    default:
                        conn = new SqlConnection(connectionString);
                        break;
                }
                DbCommand comd = conn.CreateCommand();
                ConnOpen(ref comd);
                ConnClose(ref comd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ** 打开/关闭链接 **
        /// <summary>
        /// 打开链接
        /// </summary>
        private void ConnOpen(ref DbCommand comd)
        {
            if (comd.Connection.State == ConnectionState.Closed)
                comd.Connection.Open();
        }

        /// <summary>
        /// 关闭链接
        /// </summary>
        private void ConnClose(ref DbCommand comd)
        {
            if (comd.Connection.State == ConnectionState.Open)
            {
                comd.Connection.Close();
            }
            comd.Dispose();
        }
        #endregion
        
        #region ** 创建 DbCommand 对象
        /// <summary>
        /// 生成comd对象
        /// </summary>
        public DbCommand CreateComd(string spName)
        {
            try
            {
                DbConnection conn;
                //判断执行类型
                switch (_connectionType)
                {
                    case ResourceDataBaseType.Mysql:
                        conn = new MySqlConnection(_connectionString);
                        break;
                    case ResourceDataBaseType.Postgresql:
                        conn = new NpgsqlConnection(_connectionString);
                        break;
                    case ResourceDataBaseType.Dm:
                        conn = new DmConnection(_connectionString);
                        break;
                    case ResourceDataBaseType.Kingbase:
                        conn = new Kdbndp.KdbndpConnection(_connectionString);
                        break;
                    default:
                        conn = new SqlConnection(_connectionString);
                        break;
                }
                DbCommand comd = conn.CreateCommand();
                comd.CommandText = spName;
                comd.CommandType = CommandType.StoredProcedure;
                return comd;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public DbCommand CreateComd(string spName, DbParameters p)
        {
            try
            {
                DbCommand comd = CreateComd(spName);

                int len = p.Length;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        comd.Parameters.Add(p[i]);
                    }
                }
                return comd;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public DbCommand CreateSqlComd(string strSql)
        {
            try
            {
                DbConnection conn;
                //判断执行类型
                switch (_connectionType)
                {
                    case ResourceDataBaseType.Mysql:
                        conn = new MySqlConnection(_connectionString);
                        break;
                    case ResourceDataBaseType.Postgresql:
                        conn = new NpgsqlConnection(_connectionString);
                        break;
                    case ResourceDataBaseType.Dm:
                        conn = new DmConnection(_connectionString);
                        break;
                    case ResourceDataBaseType.Kingbase:
                        conn = new KdbndpConnection(_connectionString);
                        break;
                    default://mssql
                        conn = new SqlConnection(_connectionString);
                        break;
                }
                DbCommand comd = conn.CreateCommand();
                comd.CommandText = strSql;
                comd.CommandType = CommandType.Text;
                return comd;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public DbCommand CreateSqlComd(string strSql, DbParameters p)
        {
            try
            {
                DbCommand comd = CreateSqlComd(strSql);

                int len = p.Length;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        comd.Parameters.Add(p[i]);
                    }
                }
                return comd;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ** 创建 SqlDataAdapter 对象
        /// <summary>
        /// 根据存储过程名，生成SqlDataAdapter对象
        /// </summary>
        public DbDataAdapter CreateAdapter(string spName)
        {
            try
            {
                DbDataAdapter comdAdapter;
                switch (_connectionType)
                {
                    case ResourceDataBaseType.Mysql:
                        comdAdapter = new MySqlDataAdapter(spName, new MySqlConnection(_connectionString));
                        break;
                    case ResourceDataBaseType.Postgresql:
                        comdAdapter = new NpgsqlDataAdapter(spName, new NpgsqlConnection(_connectionString));
                        break;
                    case ResourceDataBaseType.Dm:
                        comdAdapter = new DmDataAdapter(spName, new DmConnection(_connectionString));
                        break;
                    case ResourceDataBaseType.Kingbase:
                        comdAdapter = new KdbndpDataAdapter(spName, new KdbndpConnection(_connectionString));
                        break;
                    default:
                        comdAdapter = new SqlDataAdapter(spName, new SqlConnection(_connectionString));
                        break;
                }
                comdAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                return comdAdapter;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据存储过程名和参数，生成SqlDataAdapter对象
        /// </summary>
        public DbDataAdapter CreateAdapter(string spName, DbParameters p)
        {
            try
            {
                DbDataAdapter comdAdapter = CreateAdapter(spName);

                int len = p.Length;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        comdAdapter.SelectCommand.Parameters.Add(p[i]);
                    }
                }

                return comdAdapter;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据SQL语句,生成DataAdapter对象
        /// </summary>
        public DbDataAdapter CreateSqlAdapter(string strSql)
        {
            try
            {
                DbDataAdapter apter;
                switch (_connectionType)
                {
                    case ResourceDataBaseType.Mysql:
                        apter = new MySqlDataAdapter(strSql, new MySqlConnection(_connectionString));
                        break;
                    case ResourceDataBaseType.Postgresql:
                        apter = new NpgsqlDataAdapter(strSql, new NpgsqlConnection(_connectionString));
                        break;
                    case ResourceDataBaseType.Dm:
                        apter = new DmDataAdapter(strSql, new DmConnection(_connectionString));
                        break;
                    case ResourceDataBaseType.Kingbase:
                        apter = new KdbndpDataAdapter(strSql, new KdbndpConnection(_connectionString));
                        break;
                    default:
                        apter = new SqlDataAdapter(strSql, new SqlConnection(_connectionString));
                        break;
                }
                apter.SelectCommand.CommandType = CommandType.Text;
                return apter;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据SQL语句和参数,生成DataAdapter对象
        /// </summary>
        public DbDataAdapter CreateSqlAdapter(string strSql, DbParameters p)
        {
            try
            {
                DbDataAdapter apter = CreateSqlAdapter(strSql);

                int len = p.Length;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        apter.SelectCommand.Parameters.Add(p[i]);
                    }
                }

                return apter;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ** 创建 DataReader 对象
        /// <summary>
        /// 根据存储过程生成生DbDataReader
        /// </summary>
        public DbDataReader CreateDataReader(string spName)
        {
            DbCommand comd = CreateComd(spName);
            return GetDataReader(comd);
        }
        /// <summary>
        /// 根据存储过程和参数生成DbDataReader
        /// </summary>
        public DbDataReader CreateDataReader(string spName, DbParameters p)
        {
            DbCommand comd = CreateComd(spName, p);
            return GetDataReader(comd);
        }
        /// <summary>
        /// 根据SQL语句生成DbDataReader
        /// </summary>
        public DbDataReader CreateDbDataReader(string strSql)
        {
            DbCommand comd = CreateSqlComd(strSql);
            return GetDataReader(comd);
        }
        /// <summary>
        /// 根据SQL语句和参数生成DbDataReader
        /// </summary>
        public DbDataReader CreateDbDataReader(string strSql, DbParameters p)
        {
            DbCommand comd = CreateSqlComd(strSql, p);
            return GetDataReader(comd);
        }

        #region - GetDataReader()
        //获取DataReader
        private DbDataReader GetDataReader(DbCommand comd)
        {
            try
            {
                ConnOpen(ref comd);
                return comd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                ConnClose(ref comd);
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #endregion

        #region ** 创建 DataTable 对象
        /// <summary>
        /// 根据存储过程创建 DataTable 
        /// </summary>
        public DataTable CreateDataTable(string spName)
        {
            DbDataAdapter adapter = CreateAdapter(spName);
            return GetDataTable(adapter);
        }
        /// <summary>
        /// 根据存储过程和参数创建 DataTable 
        /// </summary>
        public DataTable CreateDataTable(string spName, DbParameters p)
        {
            DbDataAdapter adapter = CreateAdapter(spName, p);
            return GetDataTable(adapter);
        }
        /// <summary>
        /// 根据SQL语句,创建DataTable
        /// </summary>
        public DataTable CreateSqlDataTable(string strSql)
        {
            DbDataAdapter adapter = CreateSqlAdapter(strSql);
            return GetDataTable(adapter);
        }
        /// <summary>
        /// 根据SQL语句和参数,创建DataTable
        /// </summary>
        public DataTable CreateSqlDataTable(string strSql, DbParameters p)
        {
            DbDataAdapter adapter = CreateSqlAdapter(strSql, p);
            return GetDataTable(adapter);
        }

        #region  - GetDataTable()
        private DataTable GetDataTable(DbDataAdapter adapter)
        {
            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                if (adapter.SelectCommand.Connection.State == ConnectionState.Open)
                {
                    adapter.SelectCommand.Connection.Close();
                }
                adapter.Dispose();
            }
        }
        #endregion

        #endregion

        #region ** 创建 Scalar 对象
        /// <summary>
        /// 创建无参数的 Scalar 对象
        /// </summary>
        public object CreateScalar(string spName)
        {
            DbCommand comd = CreateComd(spName);
            return GetScalar(comd);
        }
        /// <summary>
        /// 有参数的 Scalar 对象
        /// </summary>
        public object CreateScalar(string spName, DbParameters p)
        {
            DbCommand comd = CreateComd(spName, p);
            return GetScalar(comd);
        }
        /// <summary>
        /// 根据SQL语句，创建Scalar对象
        /// </summary>
        public object CreateSqlScalar(string strSql)
        {
            DbCommand comd = CreateSqlComd(strSql);
            return GetScalar(comd);
        }
        /// <summary>
        /// 根据SQL语句和参数，创建Scalar对象
        /// </summary>
        public object CreateSqlScalar(string strSql, DbParameters p)
        {
            DbCommand comd = CreateSqlComd(strSql, p);
            return GetScalar(comd);
        }

        #region - GetScalar()
        private object GetScalar(DbCommand comd)
        {
            try
            {
                ConnOpen(ref comd);
                object o = comd.ExecuteScalar();
                ConnClose(ref comd);

                return o;
            }
            catch (Exception ex)
            {
                ConnClose(ref comd);

                throw new Exception(ex.Message);
            }
        }
        #endregion
        #endregion

        #region ** 执行数据库操作 - ToExecute() **
        /// <summary>
        /// 执行数据库操作
        /// </summary>
        private int ToExecute(DbCommand comd)
        {
            try
            {
                ConnOpen(ref comd);
                int iOk = comd.ExecuteNonQuery();
                ConnClose(ref comd);
                return iOk;
            }
            catch (Exception ex)
            {
                ConnClose(ref comd);
                throw new Exception(ex.Message);
            }
        }

        private int ToExecuteInt(DbCommand comd)
        {
            try
            {
                ConnOpen(ref comd);
                int iOk;
                int.TryParse(comd.ExecuteScalar().ToString(), out iOk);
                ConnClose(ref comd);
                return iOk;
            }
            catch (Exception ex)
            {
                ConnClose(ref comd);

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ** 仅执行，不返回输出参数 **
        /// <summary>
        /// 根据存储过程执行
        /// </summary>
        public int Execute(string spName)
        {
            DbCommand comd = CreateComd(spName);
            return ToExecute(comd);
        }
        /// <summary>
        /// 根据存储过程和参数执行
        /// </summary>
        public int Execute(string spName, DbParameters p)
        {
            DbCommand comd = CreateComd(spName, p);
            return ToExecute(comd);
        }
        /// <summary> 
        /// 执行sql语句
        /// </summary> 
        public int ExecuteSql(string sql)
        {
            DbCommand comd = CreateSqlComd(sql);
            return ToExecute(comd);
        }

        /// <summary> 
        /// 执行带参数的SQL语句
        /// </summary> 
        public int ExecuteSqlInt(string sql, DbParameters p)
        {
            DbCommand comd = CreateSqlComd(sql, p);
            return ToExecuteInt(comd);
        }
        public int ExecuteSql(string sql, DbParameters p)
        {
            DbCommand comd = CreateSqlComd(sql, p);
            return ToExecute(comd);
        }

        #endregion

        #region ** 执行并返回输出参数 **
        /// <summary>
        /// 执行并返回输出参数
        /// </summary>
        public string ExecuteOut(string spName, DbParameters p, string outParamName)
        {
            DbCommand comd = CreateComd(spName, p);
            //comd.Parameters.Add(new SqlParameter(outParamName, SqlDbType.VarChar, 50));
            //comd.Parameters[outParamName].Direction = ParameterDirection.Output;

            try
            {
                ConnOpen(ref comd);
                comd.ExecuteNonQuery();
                object o = comd.Parameters[outParamName].Value;
                ConnClose(ref comd);

                return (o == null) ? "" : o.ToString();
            }
            catch (Exception ex)
            {
                ConnClose(ref comd);

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 执行并返回输出参数：默认输出参数 @Result Varchar(50)
        /// </summary>
        public string ExecuteOut(string spName, DbParameters p)
        {
            DbCommand comd = CreateComd(spName, p);
            comd.Parameters.Add(new SqlParameter("@Result", SqlDbType.VarChar, 50));
            comd.Parameters["@Result"].Direction = ParameterDirection.Output;

            try
            {
                ConnOpen(ref comd);
                comd.ExecuteNonQuery();
                object o = comd.Parameters["@Result"].Value;
                ConnClose(ref comd);

                return (o == null) ? "" : o.ToString();
            }
            catch (Exception ex)
            {
                ConnClose(ref comd);

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ** 执行并返回输出参数 **
        /// <summary>
        /// 执行存储过程，并返回输出参数
        /// </summary>
        public string ExecuteReturn(string spName, DbParameters p, string retParam)
        {
            DbCommand comd = CreateComd(spName, p);
            comd.Parameters.Add(new SqlParameter(retParam, SqlDbType.VarChar, 50));
            comd.Parameters[retParam].Direction = ParameterDirection.ReturnValue;

            //comd.Parameters.Add(new SqlParameter("ReturnValue",SqlDbType.Int,4, ParameterDirection.ReturnValue, false, 0, 0,String.Empty, DataRowVersion.Default, null));

            try
            {
                ConnOpen(ref comd);
                comd.ExecuteNonQuery();
                object o = comd.Parameters[retParam].Value;
                ConnClose(ref comd);

                return (o == null) ? "" : o.ToString();
            }
            catch (Exception ex)
            {
                ConnClose(ref comd);

                throw new Exception(ex.Message);
            }
        }
        public string ExecuteReturn(string spName, DbParameters p)
        {
            DbCommand comd = CreateComd(spName, p);
            comd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.VarChar, 50));
            comd.Parameters["ReturnValue"].Direction = ParameterDirection.ReturnValue;

            //comd.Parameters.Add(new SqlParameter("ReturnValue",SqlDbType.Int,4, ParameterDirection.ReturnValue, false, 0, 0,String.Empty, DataRowVersion.Default, null));

            try
            {
                ConnOpen(ref comd);
                comd.ExecuteNonQuery();
                object o = comd.Parameters["ReturnValue"].Value;
                ConnClose(ref comd);

                return (o == null) ? "" : o.ToString();
            }
            catch (Exception ex)
            {
                ConnClose(ref comd);

                throw new Exception(ex.Message);
            }
        }
        /// <summary> 
        /// 执行Sql语句，并返回返回值
        /// </summary> 
        public string ExecuteSqlReturn(string sql, DbParameters p, string retParam)
        {
            DbCommand comd = CreateSqlComd(sql, p);
            comd.Parameters.Add(new SqlParameter(retParam, SqlDbType.VarChar, 50));
            comd.Parameters[retParam].Direction = ParameterDirection.ReturnValue;

            //comd.Parameters.Add(new SqlParameter("ReturnValue",SqlDbType.Int,4, ParameterDirection.ReturnValue, false, 0, 0,String.Empty, DataRowVersion.Default, null));

            try
            {
                ConnOpen(ref comd);
                comd.ExecuteNonQuery();
                object o = comd.Parameters[retParam].Value;
                ConnClose(ref comd);

                return (o == null) ? "" : o.ToString();
            }
            catch (Exception ex)
            {
                ConnClose(ref comd);

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据Sql语句执行
        /// </summary>
        public string ExecuteSqlReturn(string sql, DbParameters p)
        {
            DbCommand comd = CreateSqlComd(sql, p);
            comd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.VarChar, 50));
            comd.Parameters["ReturnValue"].Direction = ParameterDirection.ReturnValue;

            //comd.Parameters.Add(new SqlParameter("ReturnValue",SqlDbType.Int,4, ParameterDirection.ReturnValue, false, 0, 0,String.Empty, DataRowVersion.Default, null));

            try
            {
                ConnOpen(ref comd);
                comd.ExecuteNonQuery();
                object o = comd.Parameters["ReturnValue"].Value;
                ConnClose(ref comd);

                return (o == null) ? "" : o.ToString();
            }
            catch (Exception ex)
            {
                ConnClose(ref comd);

                throw new Exception(ex.Message);
            }
        }

        #endregion

    }

    public class DbParameters
    {
        private List<SqlParameter> li;

        //构造函数
        public DbParameters()
        {
            li = new List<SqlParameter>();
        }

        //单个参数的构造函数
        public DbParameters(string strName, object strValue)
        {
            li = new List<SqlParameter>();
            this.Add(strName, strValue);
        }


        #region ** 属性 ** 
        //长度
        public int Length
        {
            get { return li.Count; }
        }
        //索引
        public SqlParameter this[int k]
        {
            get
            {
                if (li.Contains(li[k]))
                {
                    SqlParameter parm = li[k];
                    return parm;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region ** 添加参数
        //添加 Input 类型参数
        public void Add(string sName, object sValue)
        {
            li.Add(new SqlParameter()
            {
                ParameterName = sName.Trim(),
                Value = sValue ?? DBNull.Value,
                Direction = ParameterDirection.Input,
            });
        }
        //添加 Output 类型参数
        public void AddOut()
        {
            AddOut("@Result", "int", 4);
        }
        public void AddOut(string sName, string sDbType, int iSize)
        {
            li.Add(new SqlParameter()
            {
                ParameterName = sName,
                SqlDbType = ConvertSqlDbType(sDbType),
                Size = iSize,
                Direction = ParameterDirection.Output,
            });
        }
        public void AddInputOutput(string sName)
        {
            li.Add(new SqlParameter()
            {
                ParameterName = sName,
                Direction = ParameterDirection.InputOutput,
            });
        }
        public void AddInputOutput(string sName, string sDbType, int iSize)
        {
            li.Add(new SqlParameter()
            {
                ParameterName = sName,
                SqlDbType = ConvertSqlDbType(sDbType),
                Size = iSize,
                Direction = ParameterDirection.InputOutput,
            });
        }

        #endregion

        #region ** 参数转换函数
        //SqlDbType数据类型转换
        private SqlDbType ConvertSqlDbType(string strDbType)
        {
            SqlDbType t = new SqlDbType();
            switch (strDbType.Trim().ToLower())
            {
                case "nvarchar": t = SqlDbType.NVarChar; break;
                case "nchar": t = SqlDbType.NChar; break;
                case "varchar": t = SqlDbType.VarChar; break;
                case "char": t = SqlDbType.Char; break;
                case "int": t = SqlDbType.Int; break;
                case "datetime": t = SqlDbType.DateTime; break;
                case "decimal": t = SqlDbType.Decimal; break;
                case "bit": t = SqlDbType.Bit; break;
                case "text": t = SqlDbType.Text; break;
                case "ntext": t = SqlDbType.NText; break;
                case "money": t = SqlDbType.Money; break;
                case "float": t = SqlDbType.Float; break;
                case "binary": t = SqlDbType.Binary; break;
            }
            return t;
        }

        #endregion

        #region ** 清空参数集合
        public void Clear()
        {
            li.Clear();
        }
        #endregion
    }
}
