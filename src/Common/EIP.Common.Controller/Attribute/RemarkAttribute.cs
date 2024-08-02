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
using Microsoft.AspNetCore.Mvc.Filters;

namespace EIP.Common.Controller.Attribute
{
    /// <summary>
    /// 备注
    /// </summary>
    public class RemarkAttribute : System.Attribute, IFilterMetadata
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="describe">内容</param>
        /// <param name="from">来源</param>
        public RemarkAttribute(string describe, RemarkFrom from,bool writeLog=false)
        {
            Describe = describe;
            From = from;
            WriteLog= writeLog;
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 日志来源
        /// </summary>
        public RemarkFrom From { get; set; }

        /// <summary>
        /// 是否写日志
        /// </summary>
        public bool WriteLog { get; set; } = false;
    }

    public enum RemarkFrom
    {
        System,
        Workflow,
        Mobile,
        Job,
        WeChat,
        DingTalk,
        BigScreen
    }
}