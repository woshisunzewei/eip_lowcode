using EIP.Common.Core.Util;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
namespace EIP.Common.Repository
{
    public class MySqlBulkLoaderHelper
    {
        /// <summary>
        /// MySql批量导入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public static int BulkInsert<T>(List<T> entities, bool isDel = false)
        {
            DataTable dt = entities.ToDataTable();
            var tableAttribute = typeof(T).GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
            var tableName = ((TableAttribute)tableAttribute).Name;
            return BulkInsertDataTable(dt, tableName, isDel);
        }
        /// <summary>
        /// MySql批量导入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public static int BulkInsertDataTable(DataTable dt,string tableName, bool isDel = false)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var value = dt.Rows[i][j];

                    if (!value.IsNullOrEmpty())
                    {
                        if (dt.Columns[j].DataType != (new DateTime()).GetType())
                        {
                            dt.Rows[i][j] = dt.Rows[i][j].ToString().Trim().Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
                        }
                    }
                }
            }
            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = ConfigurationUtil.GetDbConnectionString();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                if (isDel)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $"truncate table {tableName}";
                    cmd.ExecuteNonQuery();
                }

                int insertCount = 0;
                string tmpPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString() + "_" + Guid.NewGuid().ToString() + ".tmp");
                string csv = dt.ToCsvStr();
                File.WriteAllText(tmpPath, csv, Encoding.UTF8);

                using (MySqlTransaction tran = conn.BeginTransaction())
                {
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = ",",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = Environment.NewLine,
                        FileName = tmpPath,
                        Local = true,
                        NumberOfLinesToSkip = 0,
                        TableName = tableName,
                        CharacterSet = "utf8"
                    };
                    try
                    {
                        bulk.Columns.AddRange(dt.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList());
                        insertCount = bulk.Load();
                        tran.Commit();
                    }
                    catch (MySqlException ex)
                    {
                        if (tran != null)
                            tran.Rollback();

                        throw ex;
                    }
                }
                //File.Delete(tmpPath);
                return insertCount;
            }
        }
    }

    /// <summary>
    /// 扩展帮类
    /// </summary>
    public static class MySqlBulkLoaderDateTable
    {
        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 将Json字符串转为DataTable
        /// </summary>
        /// <param name="jsonStr">Json字符串</param>
        ///<returns></returns>
        public static DataTable ToDataTable(this string jsonStr)
        {
            //return jsonStr == null ? null : JsonSerializer.Deserialize<DataTable>(jsonStr);
            return jsonStr == null ? null : JsonConvert.DeserializeObject<DataTable>(jsonStr);
        }
        /// <summary>
        /// 将IEnumerable'T'转为对应的DataTable
        /// </summary>
        /// <typeparam name="T">数据模型</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> iEnumberable)
        {
            return iEnumberable.ToJson().ToDataTable();
        }
        /// <summary>
        /// 判断是否为Null或者空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
                return true;
            else
            {
                string objStr = obj.ToString();
                return string.IsNullOrEmpty(objStr);
            }
        }

        /// <summary>
        ///将DataTable转换为标准的CSV字符串
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>返回标准的CSV</returns>
        public static string ToCsvStr(this DataTable dt)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colum = dt.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}

