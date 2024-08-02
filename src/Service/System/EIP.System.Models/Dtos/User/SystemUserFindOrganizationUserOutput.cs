using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemUserFindOrganizationUserOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组织机构父id
        /// </summary>		
        public Guid? ParentId { get; set; }
        /// <summary>
        ///  父级名称
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>		
        public string ShortName { get; set; } = string.Empty;

        /// <summary>
        /// 上级所有字符串
        /// </summary>
        public string ParentIdsName { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string ParentIds { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public IList<FindOrganizationUserOutput> Users { get; set; } = new List<FindOrganizationUserOutput>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class FindOrganizationUserOutput
    {
        /// <summary>
        /// 人员Id
        /// </summary>	
        public Guid UserId { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>		
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// QQ
        /// </summary>		
        public string Qq { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>		
        public string WeChat { get; set; }

        /// <summary>
        /// 办公电话
        /// </summary>		
        public string OfficeMobile { get; set; }

        /// <summary>
        /// 其他联系信息
        /// </summary>		
        public string OtherContactInformation { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 头像显示
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PostName { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string ParentIdsName { get; set; }

    }
}
