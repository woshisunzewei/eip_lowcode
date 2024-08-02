﻿/**************************************************************
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
    /// 任务状态
    /// </summary>
    public enum EnumTask
    {
        /// <summary>
        /// 等待
        /// </summary>
        等待 = 2,
        /// <summary>
        /// 正在处理
        /// </summary>
        正在处理 = 4,
        /// <summary>
        /// 完成
        /// </summary>
        完成 = 6,
        /// <summary>
        /// 暂停
        /// </summary>
        暂停 = 8,
        /// <summary>
        /// 撤销
        /// </summary>
        撤销 = 10,
        /// <summary>
        /// 打回
        /// </summary>
        打回 = 12,
        /// <summary>
        /// 取消
        /// </summary>
        取消 = 14
    }
}