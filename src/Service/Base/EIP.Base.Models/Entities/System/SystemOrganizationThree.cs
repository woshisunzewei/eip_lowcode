using EIP.Common.Models.Attributes;
using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// System_Organization_Three表实体类
    /// </summary>
    [Serializable]
    [Table("System_Organization_Three")]
    [Db("EIP")]
    public class SystemOrganizationThree
    {/// <summary>
     /// 自增Id
     /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid ThreeOrganizationId { get; set; }

        /// <summary>
        /// 第三方平台组织Id
        /// </summary>		
        public string PlatformOrganizationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PlatformOrganizationParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 平台组织架构Id
        /// </summary>		
        public Guid SystemOrganizationId { get; set; }

        /// <summary>
        /// 性质:0集团,1公司,2分公司,3子公司,4部门
        /// </summary>
        public int? Nature { get; set; }

        /// <summary>
        /// 类型:3企业微信,4钉钉
        /// </summary>
        public int Type { get; set; }

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
