using EIP.System.Models.Enums;
using System;

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemUserFindCommonInput
    {
        /// <summary>
        /// 数据权限
        /// </summary>
        public string DataSql { get; set; }

        /// <summary>
        /// 对应Id值
        /// </summary>
        public Guid? PrivilegeMasterValue { get; set; }

        /// <summary>
        /// 归属人员类型:组织机构、角色、岗位、组
        /// </summary>
        public EnumPrivilegeMaster? PrivilegeMaster { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemUserFindCommonOutput
    {

        /// <summary>
        /// 
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>	
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Exist { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string ParentIdsName { get; set; }

        /// <summary>
        /// 性别:0男,1女
        /// </summary>
        public short? Sex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 组织架构
        /// </summary>
        public string OrganizationName { get; set; }
    }
}
