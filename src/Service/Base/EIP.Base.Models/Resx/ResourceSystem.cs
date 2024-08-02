/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
namespace EIP.Base.Models.Resx
{
    /// <summary>
    /// 系统资源说明
    /// </summary>
    public class ResourceSystem
    {
        /// <summary>
        /// 登录用户已冻结
        /// </summary>
        public const string 登录用户已冻结 = "登录失败, 用户帐号被冻结";
        /// <summary>
        /// 具有模块权限
        /// </summary>
        public const string 具有模块权限 = " 已具有模块权限, 请取消模块权限后重试";

        /// <summary>
        /// 具有流程权限
        /// </summary>
        public const string 具有流程权限 = " 已具有流程权限, 请取消流程权限后重试";

        /// <summary>
        /// 具有岗位
        /// </summary>
        public const string 具有岗位 = "已具有岗位, 请删除岗位后重试";
        /// <summary>
        /// 具有功能项权限
        /// </summary>
        public const string 具有功能项权限 = " 已具有功能项权限, 请取消功能项权限后重试";
        /// <summary>
        /// 具有角色
        /// </summary>
        public const string 具有角色 = "已具有角色, 请删除角色后重试";
        /// <summary>
        /// 具有人员
        /// </summary>
        public const string 具有人员 = " 已具有人员, 请删除人员后重试";
        /// <summary>
        /// 具有下级项
        /// </summary>
        public const string 具有下级项 = " 具有下级项, 请删除下级项后重试";
        /// <summary>
        /// 具有组
        /// </summary>
        public const string 具有组 = " 已具有组, 请删除组后重试";
        /// <summary>
        /// 已被赋予权限
        /// </summary>
        public const string 已被赋予权限 = "已被赋予权限, 请取消权限后重试";
        /// <summary>
        /// 用户名或密码错误
        /// </summary>
        public const string 用户名或密码错误 = "用户名或密码错误";
        /// <summary>
        /// 重置密码成功
        /// </summary>
        public const string 重置密码成功 = "重置密码成功, 密码为【{0}】";

        /// <summary>
        /// 修改密码成功
        /// </summary>
        public const string 修改密码成功 = "修改密码成功";

        /// <summary>
        /// 旧密码不正确
        /// </summary>
        public const string 旧密码不正确 = "旧密码不正确";

        /// <summary>
        /// 角色不存在
        /// </summary>
        public const string 角色不存在 = "角色不存在";

        /// <summary>
        /// 人员不存在
        /// </summary>
        public const string 人员不存在 = "人员不存在";

        /// <summary>
        /// 退出成功
        /// </summary>
        public const string 退出成功 = "退出成功";

        /// <summary>
        /// 退出失败
        /// </summary>
        public const string 退出失败 = "退出失败";

        /// <summary>
        /// 请先配置数据库连接字符串
        /// </summary>
        public const string 请先配置数据库连接字符串 = "请先在EIP.System.Api项目下appsetting.json中添加连接字符串,如:'EIP_Hr': {'ConnectionString': 'server=.;database=EIP;uid=sa;pwd=123456'}";

        /// <summary>
        /// 数据库打开失败
        /// </summary>
        public const string 数据库打开失败 = "数据库打开失败:{0}";

        /// <summary>
        /// 已存在相同代码
        /// </summary>
        public const string 已存在相同代码 = "已存在相同代码";

        /// <summary>
        /// 请配置子表详细信息
        /// </summary>
        public const string 请配置子表详细信息 = "请配置子表详细信息";
    }
}