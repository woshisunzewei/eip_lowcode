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

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// System_District表实体类
    /// </summary>
    [Serializable]
    [Table("System_District")]
    public class SystemDistrict
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public string DistrictId { get; set; }

        /// <summary>
        /// 省市区名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>		
        public string ParentId { get; set; }

        /// <summary>
        /// 简称
        /// </summary>		
        public string ShortName { get; set; } 

        /// <summary>
        /// 级别:0,中国；1，省分；2，市；3，区、县
        /// </summary>		
        public byte? LevelType { get; set; }

        /// <summary>
        /// 城市代码
        /// </summary>		
        public string CityCode { get; set; } 

        /// <summary>
        /// 邮编
        /// </summary>		
        public string ZipCode { get; set; } 

        /// <summary>
        /// 经度
        /// </summary>		
        public string Lng { get; set; } 

        /// <summary>
        /// 纬度
        /// </summary>		
        public string Lat { get; set; } 

        /// <summary>
        /// 拼音
        /// </summary>		
        public string PinYin { get; set; } 

        /// <summary>
        /// 状态
        /// </summary>		
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }
    }
}
