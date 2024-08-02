using EIP.Common.Models;
using System;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseFindBusinessDataFilterSearchInput
    {
        /// <summary>
        /// 代码生成Id
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 路由信息
        /// </summary>
        public ViewRote? Rote { get; set; }

        /// <summary>
        /// 是否具有数据权限
        /// </summary>
        public bool HaveDataPermission { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Header { get; set; }

        /// <summary>
        /// 统计字段
        /// </summary>
        public string? Field { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? DataSql { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseFindBusinessDataFilterSearchOutput
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 字段中文
        /// </summary>
        public string Field_Txt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; }

    }
}
