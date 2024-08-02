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
namespace EIP.Workflow.Engine.Core
{
    /// <summary>
    /// 工作流引擎异常
    /// </summary>
    internal class WorkflowEngineException : ApplicationException
    {
        private readonly OperateStatus _operateStatus;
        /// <summary>
        /// 无参构造函数
        /// </summary>
        internal WorkflowEngineException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        internal WorkflowEngineException(string message) : base(message)
        {
            _operateStatus = new OperateStatus
            {
                Code = ResultCode.Error,
                Msg = message
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        internal WorkflowEngineException(string message, Exception inner) : base(message, inner)
        {
            _operateStatus = new OperateStatus
            {
                Code = ResultCode.Error,
                Msg = message + ":" + inner.Message
            };
        }

        /// <summary>
        /// 获取异常状态
        /// </summary>
        /// <returns></returns>
        internal OperateStatus GetExceptionOperateStatus()
        {
            return _operateStatus;
        }
    }
}