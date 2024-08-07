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
namespace EIP.Common.Log
{
    public class LogUser
    {
        #region 基础实体

        /// <summary>
        /// 主键Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string RemoteIp { get; set; }

        /// <summary>
        /// 客户端Ip对应地址
        /// </summary>
        public string RemoteIpAddress { get; set; }

        /// <summary>
        /// 客户端浏览器
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string OsInfo { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string Browser { get; set; }

        #endregion
    }

    /// <summary>
    /// 用户操作日志实体
    /// </summary>
    public class OperateLog
    {
        /// <summary>
        /// 操作日志Id
        /// </summary>
        public Guid OperationLogId { get; set; }

        /// <summary>
        /// 客户端
        /// </summary>
        public string RemoteIp { get; set; }

        /// <summary>
        /// 客户端
        /// </summary>
        public string RemoteIpAddress { get; set; }

        /// <summary>
        /// 客户端浏览器
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string OsInfo { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 请求内容大小
        /// </summary>
        public long RequestContentLength { get; set; } = 0;

        /// <summary>
        /// 请求类型 get or post
        /// </summary>
        public string RequestType { get; set; }

        /// <summary>
        /// 当前请求Url信息
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求数据
        /// </summary>
        public string RequestData { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Action执行时间(秒)
        /// </summary>
        public double ActionExecutionTime { get; set; } = 0;

        /// <summary>
        /// 页面展示时间(秒)
        /// </summary>
        public double ResultExecutionTime { get; set; } = 0;

        /// <summary>
        /// 响应状态
        /// </summary>
        public string ResponseStatus { get; set; }

        /// <summary>
        /// 响应内容
        /// </summary>
        public string ResponseData { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 描述来源
        /// </summary>
        public string RemarkFrom { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建人员登录代码
        /// </summary>
        public string CreateUserCode { get; set; }

        /// <summary>
        /// 创建人员名字
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }

    /// <summary>
    /// 异常日志实体
    /// </summary>
    public class ExceptionLog
    {
        /// <summary>
        /// 错误Id
        /// </summary>
        public Guid ExceptionLogId { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// 内部信息
        /// </summary>
        public string InnerException { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string RemoteIp { get; set; }

        /// <summary>
        /// 客户端Ip对应地址
        /// </summary>
        public string RemoteIpAddress { get; set; }

        /// <summary>
        /// 客户端浏览器
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string OsInfo { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 请求Url
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求数据
        /// </summary>
        public string RequestData { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建人员登录代码
        /// </summary>
        public string CreateUserCode { get; set; }

        /// <summary>
        /// 创建人员名字
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 描述来源
        /// </summary>
        public string RemarkFrom { get; set; }
    }
    /// <summary>
    /// 限流日志实体
    /// </summary>
    public class RateLimitLog
    {
        /// <summary>
        /// 限流Id
        /// </summary>
        public Guid RateLimitLogId { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string RemoteIp { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 请求内容大小
        /// </summary>
        public long RequestContentLength { get; set; } = 0;

        /// <summary>
        /// 请求类型 get or post
        /// </summary>
        public string RequestType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RequestData { get; set; }

        /// <summary>
        /// 当前请求Url信息
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建人员登录代码
        /// </summary>
        public string CreateUserCode { get; set; }

        /// <summary>
        /// 创建人员名字
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }

    /// <summary>
    /// 服务调用日志实体
    /// </summary>
    public class ServiceLog
    {
    }

    /// <summary>
    /// 登录日志
    /// </summary>
    [Serializable]
    public class LoginLog
    {
        public Guid LoginLogId { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string RemoteIp { get; set; }

        /// <summary>
        /// 客户端Ip对应地址
        /// </summary>
        public string RemoteIpAddress { get; set; }

        /// <summary>
        /// 客户端浏览器
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string OsInfo { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public string LoginTime { get; set; }

        /// <summary>
        /// 停留时间(分钟)
        /// </summary>
        public double StandingTime { get; set; } = 0;

        /// <summary>
        /// 创建人员
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建人员登录代码
        /// </summary>
        public string CreateUserCode { get; set; }

        /// <summary>
        /// 创建人员名字
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

    }

    public class LoginOutLog
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid LoginLogId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LoginOutTime { get; set; }

    }

    /// <summary>
    /// 数据日志
    /// </summary>
    public class DataLog
    {
        public Guid DataLogId { get; set; }

        /// <summary>
        /// 操作类型:0新增/2编辑/3删除
        /// </summary>
        public byte OperateType { get; set; }

        /// <summary>
        /// 操作表
        /// </summary>
        public string OperateTable { get; set; }

        /// <summary>
        /// 操作前数据:若为新增，删除等数据
        /// </summary>
        public string OperateData { get; set; }

        /// <summary>
        /// 操作后数据:编辑操作
        /// </summary>
        public string OperateAfterData { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建人员登录代码
        /// </summary>
        public string CreateUserCode { get; set; }

        /// <summary>
        /// 创建人员名字
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// Sql日志
    /// </summary>
    public class SqlLog
    {
        /// <summary>
        /// sql日志Id
        /// </summary>
        public Guid SqlLogId { get; set; }

        /// <summary>
        /// 操作sql
        /// </summary>
        public string OperateSql { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public double ElapsedTime { get; set; } = 0;

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 任务日志
    /// </summary>
    public class JobLog
    {
        /// <summary>
        /// 任务日志Id
        /// </summary>
        public Guid JobLogId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Correlation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }

}