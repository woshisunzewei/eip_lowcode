using EIP.Common.Message.WeiXinMp.Dto;
using EIP.Workflow.Models.Enums;
using System;
using System.Collections.Generic;

namespace EIP.Workflow.Models.Dtos.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityDto
    {
        /// <summary>
        /// 节点类型 task
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WorkflowActivityProps Props { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityProps
    {
        /// <summary>
        /// 基础配置
        /// </summary>
        public WorkflowActivityPropsBase Base { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WorkflowActivityUser User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<WorkflowActivityTimeout> TimeOut { get; set; } = new List<WorkflowActivityTimeout>();

        /// <summary>
        /// 
        /// </summary>
        public List<WorkflowActivityNotice> Notice { get; set; } = new List<WorkflowActivityNotice>();

        /// <summary>
        /// 
        /// </summary>
        public List<WorkflowActivityEvent> Event { get; set; } = new List<WorkflowActivityEvent>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityPropsBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOpinion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsArchive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CommentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid FormId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityUser
    {
        /// <summary>
        /// 
        /// </summary>
        public int Strategy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Chosen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Auto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Pass { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? PassConfig { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<WorkflowActivityUserApprove> Approve { get; set; } = new List<WorkflowActivityUserApprove>();

        /// <summary>
        /// 
        /// </summary>
        public int OrganizationRange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Pattern { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityUserApprove
    {
        /// <summary>
        /// 
        /// </summary>
        public EnumActivityProcessorType Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RangeTxt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<WorkflowActivityUserApproveRange> Range { get; set; } = new List<WorkflowActivityUserApproveRange>();

        /// <summary>
        /// 
        /// </summary>
        public int? OrganizationRange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Pattern { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityUserApproveRange
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityUserApproveAddActivity
    {


    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityTimeout
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityNotice
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Use { get; set; }

        /// <summary>
        /// 用户选择
        /// </summary>
        public bool UserChosen { get; set; }

        /// <summary>
        /// 0下一步成功,1退回成功,2拒绝成功
        /// </summary>
        public int Trigger { get; set; }

        /// <summary>
        /// 0邮件,1站内,2短信,3微信公众号,4微信小程序,5企业微信,6钉钉,7App
        /// </summary>
        public EnumNoticeType Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WorkflowActivityNoticeConfig Config { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityEvent
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityNoticeConfig
    {
        /// <summary>
        /// 提示
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string ContentTxt { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 处理人员类型
        /// </summary>
        public EnumNoticeDoUserType Type { get; set; }

        /// <summary>
        /// 处理人员
        /// </summary>
        public string RangeTxt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<WorkflowActivityNoticeConfigRangeId> Range { get; set; } = new List<WorkflowActivityNoticeConfigRangeId>();

        /// <summary>
        /// 短信服务商 0阿里云,2腾讯云,4凌凯
        /// </summary>
        public int Provide { get; set; }

        /// <summary>
        /// 公众号AppId
        /// </summary>
        public string MpAppId { get; set; }

        /// <summary>
        /// 小程序AppId
        /// </summary>
        public string MiniAppId { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>
        public string UrlContentTxt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UrlContent { get; set; }

        /// <summary>
        /// 模版代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SendWeiXinMpNoticeMessageOutput> Param { get; set; } = new List<SendWeiXinMpNoticeMessageOutput>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityNoticeConfigRangeId
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityJoinDto
    {
        /// <summary>
        /// 节点类型 task
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WorkflowActivityJoinPropsDto Props { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityJoinPropsDto
    {
        /// <summary>
        /// 
        /// </summary>
        public WorkflowActivityJoinPropsBasecDto Base { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowActivityJoinPropsBasecDto
    {
        public int Type { get; set; }

        public int Pass { get; set; }

        public decimal PassPercent { get; set; }
    }
}
