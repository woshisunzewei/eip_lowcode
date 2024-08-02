using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIP.DataRoom.Models.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class DataSourceListInput
    {
        public string? sourceName { get; set; }

        public string? sourceType { get; set; }
    }

    public class DataSourceListOutput
    {
        public int id { get; set; }
        /// <summary>
        /// 数据源名称
        /// </summary>
        public string sourceName { get; set; }

        /// <summary>
        /// 数据源类型
        /// </summary>
        public string sourceType { get; set; }

        /// <summary>
        /// 连接驱动
        /// </summary>
        public string driverClassName { get; set; }

        /// <summary>
        /// 连接url
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int? port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string tableName { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public string moduleCode { get; set; }

        /// <summary>
        /// 是否可编辑，0 不可编辑 1 可编辑
        /// </summary>
        public int editable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
    }
}
