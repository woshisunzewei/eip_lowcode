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
using EIP.Common.Models.Attributes;
using EIP.Common.Models.Paging;
using EIP.Workflow.Models.Enums;
using System;

namespace EIP.Workflow.Engine.Models.Dtos
{
    /// <summary>
    /// 超时任务输入参数
    /// </summary>
    public class WorkflowEngineFindOverTimeInput : QueryParam
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
    }

    /// <summary>
    /// 超时任务输出参数
    /// </summary>
    public class WorkflowEngineFindOverTimeOutput 
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        ///实例Id 
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        ///活动Id
        /// </summary>
        public Guid? ActivityId { get; set; }

        /// <summary>
        /// 活动列表
        /// </summary>
        public short ActivityType { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 流程类型简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 流程拥有人:发起者
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 上一步处理人员
        /// </summary>
        public string SendUserName { get; set; }

        /// <summary>
        /// 流程发起时间
        /// </summary>
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private string _activityName { get; set; }

        /// <summary>
        /// 当前活动名称
        /// </summary>
        public string ActivityName
        {
            get
            {
                var param = string.Empty;
                switch (ActivityType)
                {
                    case (short)EnumAcitvityType.知会:
                        param = EnumAcitvityType.知会.ToString();
                        break;
                    case (short)EnumAcitvityType.加签:
                        param = $"({EnumAcitvityType.加签})";
                        break;
                }
                return _activityName + param;
            }
        }

        /// <summary>
        /// 紧急程度:正常 重要 紧急
        /// </summary>
        public short Urgency { get; set; }

        /// <summary>
        /// 活动列表
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 上一任务
        /// </summary>
        public Guid? PrevTaskId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 超时
        /// </summary>
        public DateTime OverTime { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }
    }
}
