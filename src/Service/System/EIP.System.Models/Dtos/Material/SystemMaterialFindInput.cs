using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.Material
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemMaterialFindInput : QueryParam
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
    }
}