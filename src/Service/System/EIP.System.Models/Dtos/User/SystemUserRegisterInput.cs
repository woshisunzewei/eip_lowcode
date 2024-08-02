using System;

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemUserRegisterInput
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 组织架构
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组织架构
        /// </summary>
        public string OrganizationName { get; set; }
      
        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }
    }
}
