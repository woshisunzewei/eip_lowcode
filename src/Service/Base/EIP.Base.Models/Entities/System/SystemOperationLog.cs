/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
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
using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// System_OperationLog表实体类
    /// </summary>
    [Table("System_OperationLog")]
    public class SystemOperationLog
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid OperationLogId { get; set; }

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
        /// 请求内容大小
        /// </summary>
        public long? RequestContentLength { get; set; }

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
        public double ActionExecutionTime { get; set; }

        /// <summary>
        /// 页面展示时间(秒)
        /// </summary>
        public double ResultExecutionTime { get; set; }

        /// <summary>
        /// 响应状态
        /// </summary>
        public string ResponseStatus { get; set; }

        /// <summary>
        /// 响应数据
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
        public Guid CreateUserId { get; set; }

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
}
