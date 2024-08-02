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
using System;
using System.Runtime.Serialization;

namespace EIP.Common.Core
{
    /// <summary>
    /// EIP异常
    /// </summary>
    [Serializable]
    public class EipException:Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public EipException()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public EipException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">异常消息</param>
        public EipException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public EipException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}