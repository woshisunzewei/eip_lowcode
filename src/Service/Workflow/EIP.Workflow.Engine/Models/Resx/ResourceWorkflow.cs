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
namespace EIP.Workflow.Models.Resx
{
    /// <summary>
    /// 流程资源
    /// </summary>
    public class ResourceWorkflow
    {
        /// <summary>
        /// 版本号改变
        /// </summary>
        public const string 版本号改变 = "版本号改变,将新建流程,新流程代码不能重复,请重新修改流程代码";
        /// <summary>
        /// 表单发布成功
        /// </summary>
        public const string 表单发布成功 = "表单发布成功";
        /// <summary>
        /// 处理人为空
        /// </summary>
        public const string 处理人为空 = "未查询到活动【{0}】流程处理人员,请配置流程处理人员";
        /// <summary>
        /// 系统默认意见
        /// </summary>
        public const string 系统默认意见 = "该项为系统默认意见,不能进行删除,若要删除请联系系统管理员...";

        /// <summary>
        /// 请配置子表详细信息
        /// </summary>
        public const string 请配置子表详细信息 = "请配置子表详细信息";
    }
}