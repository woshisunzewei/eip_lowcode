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
using EIP.Common.Models.Dtos;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 列
    /// </summary>
    public class SystemDataBaseColumnDto : IdInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
      
        /// <summary>
        /// 
        /// </summary>
        public string DefaultSetting { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IsNullable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MaxLength { get; set; }
      
        /// <summary>
        /// 
        /// </summary>
        public string IsIdentity { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ColumnKey { get; set; }
    }
}