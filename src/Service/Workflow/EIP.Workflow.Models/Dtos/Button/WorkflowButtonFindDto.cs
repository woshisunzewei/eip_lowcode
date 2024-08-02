/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using System;

namespace EIP.Workflow.Models.Dtos.Button
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class WorkflowButtonFindInput : QueryParam
    {
        
    }

    /// <summary>
    /// 流程实例输出
    /// </summary>
    public class WorkflowButtonFindOutput
    {
        /// <summary>
        /// 按钮Id
        /// </summary>
        public Guid ButtonId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>		
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// 脚本
        /// </summary>		
        public string Script { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 排序字段
        /// </summary>		
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 手机端是否显示
        /// </summary>
        public bool MobileShow { get; set; } = true;

        /// <summary>
        /// 按钮类型
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}