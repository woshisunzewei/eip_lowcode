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
using System;

namespace EIP.Workflow.Models.Dtos.Process
{
    /// <summary>
    /// 流程实例输出
    /// </summary>
    public class WorkflowProcessFindByIdOutput
    {
        /// <summary>
        /// 实例Id
        /// </summary>		
        public Guid ProcessId { get; set; }

        /// <summary>
        /// 父级:存在多个版本
        /// </summary>		
        public Guid? ProcessParentId { get; set; }

        /// <summary>
        /// 表单Id
        /// </summary>		
        public Guid? FormId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
		public string Name { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string ShortName { get; set; }

        /// <summary>
        /// 流程版本号
        /// </summary>		
		public string Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SaveJson { get; set; }

        /// <summary>
        /// 设计Json
        /// </summary>		
		public string PublicJson { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>		
        public bool? IsFreeze { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 图标主体
        /// </summary>		
		public string Theme { get; set; }

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
        /// 显示类库
        /// </summary>
        public bool ShowLibrary { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
		public int OrderNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public Guid? TypeId { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }
    }
}