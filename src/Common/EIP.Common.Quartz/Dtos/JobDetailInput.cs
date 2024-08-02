/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
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
namespace EIP.Common.Quartz.Dtos
{
    /// <summary>
    /// JobDetail输入
    /// </summary>
    public class JobDetailInput
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 作业组
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// 触发器名称
        /// </summary>
        public string TriggerName { get; set; }

        /// <summary>
        /// 触发器组
        /// </summary>
        public string TriggerGroup { get; set; }
    }
}