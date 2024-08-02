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
using System.Collections.Generic;

namespace EIP.Workflow.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 流程库输出
    /// </summary>
    public class WorkflowLibraryInput
    {
        /// <summary>
        /// 人员Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 流程类型
        /// </summary>
        public Guid? Type { get; set; }
    }

    /// <summary>
    /// 流程库输出
    /// </summary>
    public class WorkflowLibraryOutput
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 图标颜色
        /// </summary>
        public string IconColor { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 流程库明细
        /// </summary>
        public IList<WorkflowLibraryListOutput> List { get; set; }
    }

    /// <summary>
    /// 流程库明细
    /// </summary>
    public class WorkflowLibraryListOutput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public decimal Version { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 流程实例ID
        /// </summary>
        public Guid ProcessId { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 背景图标
        /// </summary>		
        public string BgColor { get; set; }

        /// <summary>
        /// 图标图标
        /// </summary>		
        public string IconColor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 子系统id
        /// </summary>
        public Guid? SubSystemId { get; set; }

        /// <summary>
        /// 域名
        /// </summary>		
        public string Domain { get; set; }
    }
}
