using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.Permission
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemPermissionFindMenuByUserIdOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Object Id { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public Object ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

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
        /// 打开方式
        /// </summary>
        public int OpenType { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        public string Router { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Params { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemPermissionFindMenuByUserIdRouterOutput
    {
        /// <summary>
        /// 路由
        /// </summary>
        public string router { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SystemPermissionFindMenuByUserIdRouterMetaOutput meta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SystemPermissionFindMenuByUserIdRouterOutput> children { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemPermissionFindMenuByUserIdRouterMetaOutput
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string theme { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public object? @params
        {
            get;
            set;
        }

        /// <summary>
        /// 连接地址
        /// </summary>
        public string link { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public object menuId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string image { get; set; }

        public decimal count { get; set; }
    }
}