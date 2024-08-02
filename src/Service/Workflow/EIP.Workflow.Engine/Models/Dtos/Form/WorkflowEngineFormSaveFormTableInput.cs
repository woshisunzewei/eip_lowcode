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

using System;

namespace EIP.Workflow.Engine.Models.Dtos.Form
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFormSaveFormTableInput
    {
        /// <summary>
        /// 数据库连接Id
        /// </summary>
        public Guid DataBaseId { get; set; }

        /// <summary>
        /// 修改前名称
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        ///  表备注名称
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 当前代码
        /// </summary>
        public string TableName { get; set; }
    }
}