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
using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.Workflow
{
    /// <summary>
    /// 流程实例连线
    /// </summary>
    [Serializable]
	[Table("Workflow_ProcessLink")]
    public class WorkflowProcessLink
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 连线Id
        /// </summary>		
        public Guid LinkId{ get; set; }
       
        /// <summary>
        /// 流程Id
        /// </summary>		
		public Guid ProcessId{ get; set; }
       
        /// <summary>
        /// 姓名
        /// </summary>		
		public string Title{ get; set; }
       
        /// <summary>
        /// 连线的开始节点ID
        /// </summary>		
		public Guid Begin{ get; set; }
       
        /// <summary>
        /// 连线的结束节点ID
        /// </summary>		
		public Guid End{ get; set; }

        /// <summary>
        /// 连线类型:0普通,1条件
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// Json字符串
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// 判断
        /// </summary>
        public string Judge { get; set; }
    } 
}
