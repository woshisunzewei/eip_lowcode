using EIP.Base.Models.Entities.System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.AgileAutomation
{
    /// <summary>
    /// 自动化运行输入参数
    /// </summary>
    public class SystemAgileAautomationNextChildInput
    {
        /// <summary>
        /// 子项
        /// </summary>
        public SystemAgileAutomationJsonOutput Child { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<SystemAgileAutomationJsonDataAllOutput> DataAllOutputs { get; set; }

        /// <summary>
        /// 日志
        /// </summary>
        public SystemAgileDataLog AgileDataLog { get; set; }

        /// <summary>
        /// 授权，第三方接口使用
        /// </summary>
        public string Authorization { get; set;}
    }
}
