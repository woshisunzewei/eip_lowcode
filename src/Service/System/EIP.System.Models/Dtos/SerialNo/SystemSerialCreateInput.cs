using EIP.System.Models.Dtos.DataBase;
using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.SerialNo
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemSerialCreateInput
    {
        /// <summary>
        /// 控件名称
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 配置Id
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 加载页面就显示
        /// </summary>
        public bool LoadDisplay { get; set; }

        /// <summary>
        /// 用户占用
        /// </summary>
        public bool UserOccupation { get; set; }

        /// <summary>
        /// 规则
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<SystemDataBaseSaveBusinessDataColumns> Columns { get; set; } = new List<SystemDataBaseSaveBusinessDataColumns>();
    }
}
