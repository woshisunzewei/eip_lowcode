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
using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.MenuButton
{
    /// <summary>
    /// 根据模块Id查询模块按钮
    /// </summary>
    public class SystemMenuButtonFindInput : QueryParam
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        public Guid? Id { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class SystemMenuButtonFindOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid MenuButtonId { get; set; } = Guid.Empty;

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
        /// 是否显示到列表
        /// </summary>
        public bool IsShowTable { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string MenuNames { get; set; }

        /// <summary>
        /// 设置按钮类型，可选值为 primary dashed danger link 或者不设
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 设置按钮形状，可选值为 circle、 round 或者不设
        /// </summary>
        public string Shape { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}