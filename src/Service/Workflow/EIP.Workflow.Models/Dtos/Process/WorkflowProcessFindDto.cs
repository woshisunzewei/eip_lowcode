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

namespace EIP.Workflow.Models.Dtos.Process
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class WorkflowProcessFindInput : QueryParam
    {
        /// <summary>
        /// 类型
        /// </summary>
        public Guid? Type { get; set; }
    }

    /// <summary>
    /// 流程实例输出
    /// </summary>
    public class WorkflowProcessFindOutput
    {
        /// <summary>
        /// 实例Id
        /// </summary>
        public Guid ProcessId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>		
        public string ShortName { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>		
        public string Version { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }
      
        /// <summary>
        /// 冻结
        /// </summary>		
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>		
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新人员名称
        /// </summary>		
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 显示类库
        /// </summary>
        public bool ShowLibrary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 是否为敏捷开发流程
        /// </summary>
        public int IsAgile { get; set; }

        /// <summary>
        /// 流程图
        /// </summary>
        public string Thumbnail { get; set; }
    }
}