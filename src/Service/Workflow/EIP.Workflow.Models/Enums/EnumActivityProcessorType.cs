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
    /// 审批节点-策略-处理者类型:
    /// </summary>
    public enum EnumActivityProcessorType
    {
        /// <summary>
        /// 所有人员
        /// </summary>
        所有人员 = 0,

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
        岗位 = 8,

        /// <summary>
        /// 组
        /// </summary>
        组 = 10,

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
        /// 自定义Sql
        /// </summary>
        自定义 = 90,

        /// <summary>
        /// 程序指定
        /// </summary>
        程序指定 = 100
    }
}
