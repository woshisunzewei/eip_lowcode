using System;

namespace EIP.System.Models.Dtos.Login
{
    /// <summary>
    /// 登录
    /// </summary>
    public class SystemLoginInput
    {
        /// <summary>
        /// 登录Id
        /// </summary>
        public string LoginId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 浏览器代理
        /// </summary>
        public string UserAgent { get; set; } = string.Empty;

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string Browser { get; set; } = string.Empty;

        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; } = string.Empty;

    }

    /// <summary>
    /// 登录输出
    /// </summary>
    public class SystemLoginOutput
    {
        /// <summary>
        /// 人员Id
        /// </summary>		
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Code { get; set; }

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
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 登录Id
        /// </summary>
        public Guid LoginId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 是否需要修改密码
        /// </summary>
        public bool ChangePassword { get; set; }

        /// <summary>
        /// 强制修改密码类型
        /// </summary>
        public int? ChangePasswordType { get; set; }
    }

    /// <summary>
    /// 根据用户Id登录
    /// </summary>
    public class SystemLoginByUserIdInput
    {
        /// <summary>
        /// 用户Id，加密用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 浏览器代理
        /// </summary>
        public string UserAgent { get; set; } = string.Empty;

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string Browser { get; set; } = string.Empty;
    }

    /// <summary>
    /// 根据用户帐号登录
    /// </summary>
    public class SystemLoginByCodeInput
    {
        /// <summary>
        /// 用户帐号，加密用户帐号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 浏览器代理
        /// </summary>
        public string UserAgent { get; set; } = string.Empty;

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string Browser { get; set; } = string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemLoginByDingTalkInput
    {
        /// <summary>
        /// 免登陆Code
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 浏览器代理
        /// </summary>
        public string UserAgent { get; set; } = string.Empty;

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string Browser { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string LoginId { get; set; }
    }
}
