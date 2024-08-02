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
using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// System_MenuButton表实体类
    /// </summary>
	[Serializable]
    [Table("System_MenuButton")]
    public class SystemMenuButton
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid MenuButtonId { get; set; }

        /// <summary>
        /// 模块id
        /// </summary>		
        public Guid MenuId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 若是按钮则可为其添加图标
        /// </summary>		
        public string Icon { get; set; }
        /// <summary>
        /// 按钮图标主题
        /// </summary>		
        public string Theme { get; set; }
        /// <summary>
        /// 名称:新增、删除、编辑、读取数据....
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
        /// 自动化
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
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 冻结
        /// </summary>
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
		public string Remark { get; set; }

        /// <summary>
        /// 设置按钮类型，可选值为 primary dashed danger link 或者不设
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 设置按钮形状，可选值为 circle、 round 或者不设
        /// </summary>
        public string Shape { get; set; }

        /// <summary>
        /// 列表上是否显示:用于某些列表上不需要显示,但是又需要赋予权限情况
        /// </summary>
        public bool IsShowTable { get; set; } = true;

        /// <summary>
        /// 是否显示移动端
        /// </summary>
        public bool IsShowMobile { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建用户Id
        /// </summary>		
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>		
        public Guid? UpdateUserId { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}
