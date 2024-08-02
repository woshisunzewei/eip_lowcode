using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 工作表获取
    /// </summary>
    public class SystemDataBaseFindTableInput : QueryParam
    {
        /// <summary>
        /// 工作表
        /// </summary>
        public Guid Table { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public dynamic FormValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Columns { get; set; }
    }
    /// <summary>
    /// 工作表获取
    /// </summary>
    public class SystemDataBaseFindTableDataColumnsInput 
    {
        /// <summary>
        /// 
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Mask { get; set; } = "";
    }

}
