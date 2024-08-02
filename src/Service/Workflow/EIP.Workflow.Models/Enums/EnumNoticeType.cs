/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
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
namespace EIP.Workflow.Models.Enums
{
    /// <summary>
    /// 通知类型枚举
    /// </summary>
    public enum EnumNoticeType
    {
        /// <summary>
        /// 
        /// </summary>
        邮件 = 0,
        /// <summary>
        /// 
        /// </summary>
        站内 = 1,
        /// <summary>
        /// 
        /// </summary>
        短信 = 2,
        /// <summary>
        /// 
        /// </summary>
        微信公众号 = 3,
        /// <summary>
        /// 
        /// </summary>
        微信小程序 = 4,
        /// <summary>
        /// 企业微信
        /// </summary>
        企业微信 = 5,
        /// <summary>
        /// QQ
        /// </summary>
        Qq = 6,
        /// <summary>
        /// 钉钉
        /// </summary>
        钉钉 = 7,
        /// <summary>
        /// App
        /// </summary>
        App = 8,
    }

    /// <summary>
    /// 通知人员类型枚举
    /// </summary>
    public enum EnumNoticeDoUserType
    {
        /// <summary>
        /// 所有成员
        /// </summary>
        所有成员 = 0,
        /// <summary>
        /// 组织
        /// </summary>
        组织 = 2,
        /// <summary>
        /// 人员
        /// </summary>
        人员 = 4,
        /// <summary>
        /// 角色
        /// </summary>
        角色 = 6,
        /// <summary>
        /// 岗位
        /// </summary>
        岗位 = 7,
        /// <summary>
        /// 组
        /// </summary>
        组 = 10,
        /// <summary>
        /// 下一步处理接收人
        /// </summary>
        下一步处理接收人 = 12,
        /// <summary>
        /// 当前处理人
        /// </summary>
        当前处理人 = 14,
        /// <summary>
        /// 发起者
        /// </summary>
        发起者 = 22,
        /// <summary>
        /// 发起者直线领导
        /// </summary>
        发起者直线领导 = 24,
        /// <summary>
        /// 提交人直线领导
        /// </summary>
        提交人直线领导 = 26,

        /// <summary>
        /// 表单人员
        /// </summary>
        表单人员 = 28,
        /// <summary>
        /// 自定义
        /// </summary>
        自定义 = 90,
        /// <summary>
        /// 程序指定
        /// </summary>
        程序指定 = 100,
    }

    /// <summary>
    /// 通知触发类型
    /// </summary>
    public enum EnumNoticeTrigger
    {
        /// <summary>
        /// 
        /// </summary>
        下一步成功 = 0,
        /// <summary>
        /// 
        /// </summary>
        退回成功 = 1,
        /// <summary>
        /// 
        /// </summary>
        拒绝成功 = 2,
    }
}