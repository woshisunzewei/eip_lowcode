using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.System
{

    /// <summary>
    /// 第三方用户信息
    /// </summary>
    [Serializable]
    [Table("System_UserInfo_Three")]
    public class SystemUserInfoThree
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid ThreeUserId { get; set; }

        /// <summary>
        /// 类型:1微信公众号,2微信小程序,3企业微信,4钉钉
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 第三方平台帐号Id（微信公众号，小程序对应OpenId，企业微信及钉钉对应UserId）
        /// </summary>
        public string PlatformUserId { get; set; }

        /// <summary>
        /// 第三方平台组织Id
        /// </summary>
        public string PlatformOrganizationId { get; set; }

        /// <summary>
        /// 钉钉组织名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户在当前开发者企业帐号范围内的唯一标识。
        /// </summary>
        public string UnionId { get; set; }

        /// <summary>
        /// 头像地址。
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 国际电话区号。
        /// </summary>
        public string StateCode { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 员工工号
        /// </summary>
        public string JobNumber { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 所属部门id列表
        /// </summary>
        public string OrganizationIds { get; set; }

        /// <summary>
        /// 所属部门名称列表
        /// </summary>
        public string OrganizationNames { get; set; }

        /// <summary>
        /// 系统用户Id
        /// </summary>
        public Guid? SystemUserId { get; set; }

        /// <summary>
        /// 系统组织架构Id
        /// </summary>
        public Guid SystemOrganizationId { get; set; }

        /// <summary>
        /// 关注微信公众号
        /// </summary>
        public bool? IsSubscribeMp { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建用户Id
        /// </summary>		
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>		
        public Guid? UpdateUserId { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}
