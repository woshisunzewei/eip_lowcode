using System;

namespace EIP.System.Models.Dtos.AgileAutomation
{
    /// <summary>
    /// 自动化运行输入参数
    /// </summary>
    public class SystemAgileAautomationButtonInput
    {
        /// <summary>
        /// 自动化配置Id
        /// </summary>
        public Guid AutomationConfigId { get; set; }

        /// <summary>
        /// 敏捷开发Id
        /// </summary>
        public Guid AgileConfigId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Authorization { get; set; } = string.Empty;

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户名代码
        /// </summary>
        public string UserCode { get; set; } = string.Empty;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 用户组织结构Id
        /// </summary>
        public Guid OrganizationId { get; set; } = Guid.Empty;

        /// <summary>
        /// 用户组织结构名称
        /// </summary>
        public string OrganizationName { get; set; } = string.Empty;

    }
}
