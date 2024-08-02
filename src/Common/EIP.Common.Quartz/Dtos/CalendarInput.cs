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
using EIP.Common.Quartz.Enums;

namespace EIP.Common.Quartz.Dtos
{
    /// <summary>
    /// 日历input
    /// </summary>
    public class CalendarInput
    { /// <summary>
        /// 日历名称
        /// </summary>
        public string CalendarName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否替换现有日历
        /// </summary>
        public bool ReplaceExists { get; set; }

        /// <summary>
        /// 更新相关作业
        /// </summary>
        public bool UpdateTriggers { get; set; }

        /// <summary>
        /// 当为Cron时条件表达式
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 日历类型枚举
        /// </summary>
        public EnumCalendar EnumCalendar { get; set; }
    }
}