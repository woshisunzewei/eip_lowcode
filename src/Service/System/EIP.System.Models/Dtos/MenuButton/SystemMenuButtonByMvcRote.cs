/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/1 10:20:59
* 文件名: SystemMenuButtonByMvcRote
* 描述: 根据区域,控制器,方法获取对应菜单下按钮
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Attributes.MicroOrm.Joins;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIP.System.Models.Dtos.MenuButton
{
    /// <summary>
    /// 根据区域,控制器,方法获取对应菜单下按钮
    /// </summary>
    [Table("System_Menu")]
    public class SystemMenuButtonDataByMvcRote
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [Key]
        public Guid MenuId { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string Router { get; set; } = string.Empty;

        /// <summary>
        /// 地址
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 所有按钮
        /// </summary>
        [LeftJoin("System_MenuButton", "MenuId", "MenuId")]
        [JsonIgnore]
        public List<SystemMenuButtonByViewRote> Buttons { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemMenuButtonByViewRote
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public Guid MenuButtonId { get; set; } = Guid.Empty;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 触发执行类型:1方法，2脚本，3接口
        /// </summary>
        public int TriggerType { get; set; }

        /// <summary>
        /// 方法
        /// </summary>		
        public string Method { get; set; }

        /// <summary>
        /// 脚本方法
        /// </summary>		
        public string Script { get; set; }

        /// <summary>
        /// Api配置
        /// </summary>
        public string Api { get; set; }

        /// <summary>
        /// 自动化配置
        /// </summary>
        public string Automation { get; set; }

        /// <summary>
        /// 打印
        /// </summary>
        public string Prints { get; set; }

        /// <summary>
        /// 导出
        /// </summary>
        public string Export { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 按钮图标主题
        /// </summary>		
        public string Theme { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 设置按钮类型，可选值为 primary dashed danger link 或者不设
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 设置按钮形状，可选值为 circle、 round 或者不设
        /// </summary>
        public string Shape { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsShowTable { get; set; }

        /// <summary>
        /// 是否冻结
        /// </summary>
        [JsonIgnore]
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

    }
}
