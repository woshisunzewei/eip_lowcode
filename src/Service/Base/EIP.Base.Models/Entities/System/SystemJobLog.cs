/**************************************************************
* Copyright (C) www.eipflow.com ����ΰ��Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 1039318332)
* ����ʱ��: 2018/10/30 22:40:15
* �ļ���: 
* ����: 
* 
* �޸���ʷ
* �޸��ˣ�
* ʱ�䣺
* �޸�˵����
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
        /// ��������
        /// </summary>		
        public string TriggerType { get; set; }

        /// <summary>
        /// �������
        /// </summary>		
        public string RequestParam { get; set; }

        /// <summary>
        /// ��Ϣ
        /// </summary>		
        public string Message{ get; set; }
       
        /// <summary>
        /// ����ʱ��
        /// </summary>		
		public DateTime CreateTime{ get; set; }
   } 
}
