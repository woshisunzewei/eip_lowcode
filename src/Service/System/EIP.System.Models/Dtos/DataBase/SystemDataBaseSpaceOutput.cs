/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
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

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 数据库空间占用输出
    /// </summary>
    public class SystemDataBaseSpaceOutput
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 记录数
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// 保留空间
        /// </summary>
        public string Reserved { get; set; }

        /// <summary>
        /// 使用空间
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 索引使用空间
        /// </summary>
        public string IndexSize { get; set; }

        /// <summary>
        /// 未用空间
        /// </summary>
        public string Unused { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}