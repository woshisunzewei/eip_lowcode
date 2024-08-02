using System;

namespace EIP.System.Models.Dtos.WeChat.Work
{
    /// <summary>
    /// 
    /// </summary>
    public class WeChatWorkDepartmentOutput
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public long? id { get; set; }

        /// <summary>
        /// 父部门ID
        /// </summary>
        public long? parentid { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组织机构父id
        /// </summary>		
        public Guid? ParentId { get; set; }
    }
}
