using System;

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 系统用户类
    /// </summary>
    public class SystemUserFindHeadByIdOutput
    {
        /// <summary>
        /// 用户Guid
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>		
        public string OrganizationName { get; set; }

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
        /// 其他联系信息
        /// </summary>		
        public string OtherContactInformation { get; set; }

        /// <summary>
        /// 登录类型:0默认为普通用户
        /// </summary>
        public int LoginType { get; set; }

        /// <summary>
        /// 第一次访问时间
        /// </summary>		
        public DateTime? FirstVisitTime { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>		
        public DateTime? LastVisitTime { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>		
        public int Status { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>		
        public string HeadImage { get; set; }

        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 性别:0男,1女
        /// </summary>
        public int? Sex { get; set; }
        /// <summary>
        /// 格式化
        /// </summary>
        public string HeadImageFormat { get; set; }

        /// <summary>
        /// 用户关联组织架构Ids
        /// </summary>
        public string UserOrganizationIds { get; set; }

        /// <summary>
        /// 用户关联组织架构名称
        /// </summary>
        public string UserOrganizationNames { get; set; }

    }
}