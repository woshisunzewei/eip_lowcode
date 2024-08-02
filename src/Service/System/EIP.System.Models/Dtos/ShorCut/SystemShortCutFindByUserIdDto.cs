using System;

namespace EIP.System.Models.Dtos.ShortCut
{
    /// <summary>
    /// 快捷方式
    /// </summary>
    public class SystemShortCutFindByUserIdInput
    {
        /// <summary>
        /// 快捷方式类型
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
    }

    /// <summary>
    /// 根据用户获取快捷方式输出参数
    /// </summary>
    public class SystemShortCutFindByUserIdOutput
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        public Guid MenuId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 打开地址
        /// </summary>
        public string OpenUrl { get; set; }

        /// <summary>
        /// 打开方式
        /// </summary>
        public string OpenType { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 背景图标
        /// </summary>		
		public string BgColor { get; set; }

        /// <summary>
        /// 图标图标
        /// </summary>		
		public string IconColor { get; set; }
    }
}
