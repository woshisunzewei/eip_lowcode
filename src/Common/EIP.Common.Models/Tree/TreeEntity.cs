using System.Collections.Generic;

namespace EIP.Common.Models.Tree
{
    /// <summary>
    /// 树实体
    /// </summary>
    public class TreeEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TreeEntitySlots Slots { get; set; } = new TreeEntitySlots();

        /// <summary>
        /// 
        /// </summary>
        public bool IsLeaf { get; set; }
        /// <summary>
        /// 级别:0,中国；1，省分；2，市；3，区、县
        /// </summary>		
        public byte? LevelType { get; set; }
        /// <summary>
        /// 子元素
        /// </summary>
        public List<TreeEntity> Children { get; set; }=new List<TreeEntity>();

        /// <summary>
        /// 
        /// </summary>
        public bool DisableCheckbox { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public bool Disabled { get; set; } = false;
    }

    /// <summary>
    /// Slots
    /// </summary>
    public class TreeEntitySlots
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; } = "file";

        /// <summary>
        /// 
        /// </summary>
        public string Theme { get; set; } = "outlined";

        /// <summary>
        /// 扩展属性
        /// </summary>
        public object Extend { get; set; }
    }
}
