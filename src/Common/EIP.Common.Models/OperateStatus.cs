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
using EIP.Common.Core.Resource;
namespace EIP.Common.Models
{
    /// <summary>
    /// 调用调用服务或业务逻辑的操作状态,使用DataContract特性,表示可序列化
    /// </summary>
    public class OperateStatus
    {
        #region 构造函数

        /// <summary>
        /// 构造函数:默认为失败
        /// </summary>
        public OperateStatus()
        {
            Code = ResultCode.Error;
            Msg = Chs.Error;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        public OperateStatus(OperateStatus status)
        {
            Code = status.Code;
            Msg = status.Msg;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 返回标记
        /// </summary>
        public ResultCode Code { get; set; } = ResultCode.Error;

        /// <summary>
        /// 消息字符串(有多语言后将删除该属性)
        /// </summary>
        public object Msg { get; set; } = Chs.Error;
        #endregion

        /// <summary>
        /// /操作成功
        /// </summary>
        /// <returns></returns>
        public static OperateStatus Success()
        {
            return operateStatus(ResultCode.Success, "操作成功");
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static OperateStatus Success(string message)
        {
            return operateStatus(ResultCode.Success, message);
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <returns></returns>
        public static OperateStatus Error()
        {
            return operateStatus(ResultCode.Error, "操作失败");
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static OperateStatus Error(string msg)
        {
            return operateStatus(ResultCode.Error, msg);
        }

        /// <summary>
        /// 操作结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static OperateStatus operateStatus(ResultCode code, string msg)
        {
            OperateStatus operateStatus = new OperateStatus();
            operateStatus.Code = code;
            operateStatus.Msg = msg;
            return operateStatus;
        }

    }

    /// <summary>
    /// 返回结果带实体信息
    /// </summary>
    /// <typeparam name="T">实体信息</typeparam>
    public class OperateStatus<T> : OperateStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static OperateStatus<T> Success(T data)
        {
            return operateStatus(data, ResultCode.Success, "操作成功");
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static OperateStatus<T> Success(T data, string message)
        {
            return operateStatus(data, ResultCode.Success, message);
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static OperateStatus<T> Error(T data)
        {
            return operateStatus(data, ResultCode.Error, "操作失败");
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static OperateStatus<T> Error(T data, string message)
        {
            return operateStatus(data, ResultCode.Error, message);
        }
        /// <summary>
        /// 操作结果
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static new OperateStatus<T> Error(string message)
        {
            OperateStatus<T> operateStatus = new OperateStatus<T>();
            operateStatus.Code = ResultCode.Error;
            operateStatus.Msg = message;
            return operateStatus;
        }

        /// <summary>
        /// 操作结果
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static OperateStatus<T> operateStatus(T data, ResultCode code, string message)
        {
            OperateStatus<T> operateStatus = new OperateStatus<T>();
            operateStatus.Code = code;
            operateStatus.Msg = message;
            operateStatus.Data = data;
            return operateStatus;
        }

    }

    /// <summary>
    /// 操作返回码
    /// </summary>
    public enum ResultCode
    {

        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// 其他地方登录
        /// </summary>
        OtherLogin = 400,
        /// <summary>
        /// 失败
        /// </summary>
        Error = 500,

    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OperateStatusTfs<T> : OperateStatusTfsBase
    {
        /// <summary>
        /// 
        /// </summary>
        public T Result { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name=""></typeparam>
    public class OperateStatusTfsBase
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// 返回结果带实体信息
    /// </summary>
    /// <typeparam name="T">实体信息</typeparam>
    public class OperateStatusTfsOpenApi<T>
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }
    }
}