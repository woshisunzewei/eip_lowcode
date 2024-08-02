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
namespace EIP.Workflow.Engine.Models.Resx
{
    /// <summary>
    /// 节点类型
    /// </summary>
    internal class ResourceWorkflowEngine
    {
        /// <summary>
        /// 未匹配类型
        /// </summary>
        internal const string 未匹配活动类型 = "未匹配活动类型";

        /// <summary>
        /// 流程配置错误无开始节点
        /// </summary>
        internal const string 流程配置错误无开始节点 = "流程配置错误无开始节点";

        /// <summary>
        /// 未找到后续流程节点
        /// </summary>
        internal const string 未找到后续流程节点 = "未找到后续流程节点";

        /// <summary>
        /// 未找到分支活动判断条件表达式
        /// </summary>
        internal const string 未找到分支活动判断条件表达式 = "未找到分支活动判断条件表达式";

        /// <summary>
        /// 未找到分支活动判断节点是和否连线
        /// </summary>
        internal const string 未找到分支活动判断节点表达式 = "未找到分支活动判断节点表达式";

        /// <summary>
        /// 未找到分支活动后续连线
        /// </summary>
        internal const string 未找到分支活动后续连线 = "未找到分支活动后续连线";

        /// <summary>
        /// 判断条件有效
        /// </summary>
        internal const string 判断条件有效 = "判断条件有效";

        /// <summary>
        /// 判断条件无效
        /// </summary>
        internal const string 判断条件无效 = "判断条件无效";

        /// <summary>
        /// 配置判断表达式为空
        /// </summary>
        internal const string 配置判断表达式为空 = "配置判断表达式为空";

        /// <summary>
        /// 未找到流程处理人
        /// </summary>
        internal const string 未找到流程处理人 = "未找到流程处理人";

        /// <summary>
        /// 无流程处理人
        /// </summary>
        internal const string 无流程处理人 = "无流程处理人";

        /// <summary>
        /// 无发起表单
        /// </summary>
        internal const string 无发起表单 = "无发起表单";
        /// <summary>
        /// 发起流程成功
        /// </summary>
        internal const string 发起流程成功 = "提交已成功！业务流水号：{0},您提交的表单已传送给:";

        /// <summary>
        /// 发起流程成功任务退回到下一步
        /// </summary>
        internal const string 发起流程成功任务退回到下一步 = "退出成功:{0}";

        /// <summary>
        /// 退回
        /// </summary>
        internal const string 退回 = "退回:{0}";

        /// <summary>
        /// 已拒绝
        /// </summary>
        internal const string 已拒绝 = "已拒绝";
        /// <summary>
        /// 拒绝成功
        /// </summary>
        internal const string 拒绝成功 = "处理成功！任务已被拒绝,流水号:{0}";
        /// <summary>
        /// 已取消
        /// </summary>
        internal const string 已取消 = "已取消";
        /// <summary>
        /// 取消成功
        /// </summary>
        internal const string 取消成功 = "处理成功！任务已被取消,流水号:{0}";

        /// <summary>
        /// 未找到流程活动信息
        /// </summary>
        internal const string 未找到流程活动信息 = "未找到流程活动信息";

        /// <summary>
        /// 未找到流程实例信息
        /// </summary>
        internal const string 未找到流程实例信息 = "未找到流程实例信息";

        /// <summary>
        /// 任务已处理
        /// </summary>
        internal const string 任务已处理 = "任务已处理,处理人:{0}";

        /// <summary>
        /// 流程已结束
        /// </summary>
        internal const string 流程已结束 = "{0}流程已结束";

        /// <summary>
        /// 处理完毕
        /// </summary>
        internal const string 处理完毕 = "处理完毕";

        /// <summary>
        /// 处理人员为空
        /// </summary>
        internal const string 处理人员为空 = "人员为空";

        /// <summary>
        /// 知会成功
        /// </summary>
        internal const string 知会成功 = "知会成功:{0}";

        /// <summary>
        /// 知会已阅成功
        /// </summary>
        internal const string 知会已阅成功 = "知会已阅成功!";

        /// <summary>
        /// 知会已阅
        /// </summary>
        internal const string 知会已阅 = "知会已阅!";

        /// <summary>
        /// 退回重填成功
        /// </summary>
        internal const string 退回重填 = "退回重填到开始:{0}";

        /// <summary>
        /// 退回重填成功
        /// </summary>
        internal const string 退回重填成功 = "退回重填成功!";

        /// <summary>
        /// 加签
        /// </summary>
        internal const string 加签 = "加签:{0}";

        /// <summary>
        /// 加签成功
        /// </summary>
        internal const string 加签成功 = "加签成功,下一步处理人:{0}";

        /// <summary>
        /// 加签处理成功
        /// </summary>
        internal const string 加签处理成功 = "加签处理成功:{0}";

        /// <summary>
        /// 加签串行处理成功
        /// </summary>
        internal const string 加签串行处理成功 = "提交已成功！：{0}";

        /// <summary>
        /// 加签串行处理成功
        /// </summary>
        internal const string 加签回到发起关卡 = "处理成功,回到发起关卡：{0}";

        /// <summary>
        /// 加签成功
        /// </summary>
        internal const string 加签并行处理成功 = "提交已成功！正在等待以下人员处理：{0}";

        /// <summary>
        /// 阅示成功
        /// </summary>
        internal const string 阅示成功 = "阅示成功:{0}";

        /// <summary>
        /// 阅示已阅
        /// </summary>
        internal const string 阅示已阅 = "阅示已阅!";

        /// <summary>
        /// 阅示已阅待审核
        /// </summary>
        internal const string 阅示已阅待审核 = "阅示已阅待审核!";

        /// <summary>
        /// 阅示已阅成功
        /// </summary>
        internal const string 阅示已阅成功 = "阅示已阅成功!";

        /// <summary>
        /// 阅示已阅成功
        /// </summary>
        internal const string 阅示已阅待审核成功 = "阅示已阅待审核成功!";

        /// <summary>
        /// 保存到草稿箱失败
        /// </summary>
        internal const string 保存到草稿箱失败 = "保存到草稿箱失败";

        /// <summary>
        /// 保存到范本夹失败
        /// </summary>
        internal const string 保存到范本夹失败 = "保存到范本夹失败";

        /// <summary>
        /// 子流程启动失败,未找到子流程
        /// </summary>
        internal const string 未找到子流程 = "子流程启动失败,未找到子流程";

        /// <summary>
        /// 召回失败,下级任务已处理
        /// </summary>
        internal const string 下级任务已处理 = "召回失败,下级任务已处理";

        /// <summary>
        /// 已召回
        /// </summary>
        internal const string 已召回 = "已召回";

        /// <summary>
        /// 召回失败
        /// </summary>
        internal const string 召回失败 = "召回失败！改任务已召回";

        /// <summary>
        /// 召回成功
        /// </summary>
        internal const string 召回成功 = "召回成功！流水号:{0}";

        /// <summary>
        /// 撤销失败
        /// </summary>
        internal const string 撤销失败 = "撤销失败！改任务已撤销";

        /// <summary>
        /// 撤销成功
        /// </summary>
        internal const string 撤销成功 = "撤销成功！流水号:{0}";

        /// <summary>
        /// 已撤销
        /// </summary>
        internal const string 已撤销 = "已撤销";


        /// <summary>
        /// 删除失败
        /// </summary>
        internal const string 删除失败 = "删除失败！改任务已删除";

        /// <summary>
        /// 删除成功
        /// </summary>
        internal const string 删除成功 = "删除成功！流水号:{0}";

        /// <summary>
        /// 已删除
        /// </summary>
        internal const string 已删除 = "已删除";

        /// <summary>
        /// 删除成功
        /// </summary>
        internal const string 彻底删除 = "彻底删除成功！流水号:{0}";
    }
}