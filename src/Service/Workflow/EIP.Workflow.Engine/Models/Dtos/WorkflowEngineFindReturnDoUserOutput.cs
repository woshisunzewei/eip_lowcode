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

namespace EIP.Workflow.Engine.Models.Dtos
{
    /// <summary>
    /// 退回活动
    /// </summary>
    public class WorkflowEngineFindReturnDoUserOutput
    {
        /// <summary>
        /// 处理人Id
        /// </summary>
        public Guid DoUserId { get; set; }
        /// <summary>
        /// 处理人代码
        /// </summary>
        public string DoUserCode { get; set; }
        /// <summary>
        /// 处理人名称
        /// </summary>
        public string DoUserName { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string Title { get; set; }

    }
}