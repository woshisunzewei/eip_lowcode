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
using EIP.Common.Models.Attributes.MicroOrm;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
	[Table("System_JobLog")]
    public class SystemJobLog
    {
        /// <summary>
        /// 
        /// </summary>		
        [Key, Identity, JsonIgnore]
        public long JobLogId { get; set; }
       
        /// <summary>
        /// 触发类型
        /// </summary>		
        public string TriggerType { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>		
        public string RequestParam { get; set; }

        /// <summary>
        /// 消息
        /// </summary>		
        public string Message{ get; set; }
       
        /// <summary>
        /// 创建时间
        /// </summary>		
		public DateTime CreateTime{ get; set; }
   } 
}
