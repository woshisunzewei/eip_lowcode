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

namespace EIP.System.Models.Dtos.Menu
{
    /// <summary>
    /// 根据父级查询模块
    /// </summary>
    public class SystemMenuFindInput : QueryParam
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 是否包含本身
        /// </summary>
        public bool HaveSelf { get; set; } = true;
    }

    /// <summary>
    /// 根据父级获取模块
    /// </summary>
    public class SystemMenuFindOutput 
    {

        /// <summary>
        /// 
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid MenuId { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>		
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>		
        public string Icon { get; set; }

        /// <summary>
        /// 图片
        /// </summary>		
        public string Image { get; set; }

        /// <summary>
        /// 按钮图标主题
        /// </summary>		
        public string Theme { get; set; } 
        /// <summary>
        /// 打开类型
        /// </summary>		
        public byte OpenType { get; set; }

        /// <summary>
        /// 打开地址
        /// </summary>		
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 允许删除
        /// </summary>		
        public bool CanbeDelete { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 是否具有模块权限
        /// </summary>
        public bool HaveMenuPermission { get; set; }

        /// <summary>
        /// 是否具有数据权限
        /// </summary>
        public bool HaveDataPermission { get; set; }

        /// <summary>
        /// 是否具有字段权限
        /// </summary>
        public bool HaveFieldPermission { get; set; }

        /// <summary>
        /// 是否具有功能项权限
        /// </summary>
        public bool HaveButtonPermission { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 是否显示到模块
        /// </summary>
        public bool IsShowMenu { get; set; }

        /// <summary>
        /// 是否为应用菜单
        /// </summary>
        public bool IsAgileMenu { get; set; } = false;

        /// <summary>
        /// 应用菜单类型1,分组，2工作表，3自定义界面
        /// </summary>
        public int? AgileMenuType { get; set; }

        /// <summary>
        /// 嵌套页地址
        /// </summary>
        public string IframePath { get; set; }

    }
}