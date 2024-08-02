using System;

namespace EIP.Common.Models.Tree
{
    /// <summary>
    /// 基础树形结构
    /// </summary>
    public class BaseTree
    {
        /// <summary>
        /// 主键
        /// </summary>
        public object id { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public object parent { get; set; }

        /// <summary>
        /// 父级集合
        /// </summary>
        public string parents { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string theme { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool disableCheckbox { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public bool disabled { get; set; } = false;

        /// <summary>
        /// 扩展属性
        /// </summary>
        public BaseTreeExtend extend { get; set; }
    }

    /// <summary>
    /// </summary>
    public class BaseTreeExtend
    {
        public Guid menuId { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public byte openType { get; set; }
        public string iframePath { get; set; }
        public bool haveDataPermission { get; set; }
        public int? agileMenuType { get; set; }
        public bool isShowMenu { get; set; }
        public string image { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal count { get; set; } = 2;
    }
}
