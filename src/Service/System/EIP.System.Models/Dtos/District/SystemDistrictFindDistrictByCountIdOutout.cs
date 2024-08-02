/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/2 21:47:44
* 文件名: SystemDistrictFindDistrictByCountIdOutout
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Attributes.MicroOrm.Joins;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIP.System.Models.Dtos.District
{
    /// <summary>
    /// 根据县Id获取省市县Id
    /// </summary>
    [Table("System_District")]
    public class SystemDistrictFindDistrictByCountIdOutout
    {
        /// <summary>
        /// 县
        /// </summary>
        [Key]
        public string DistrictId { get; set; }

        /// <summary>
        /// 市
        /// </summary>		
        public string ParentId { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [NotMapped]
        public string ProvinceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LeftJoin("System_District", "ParentId", "DistrictId")]
        [JsonIgnore]
        public SystemDistrictFindDistrictByCountIdProvinceOutout District { get; set; }
    }

    /// <summary>
    /// 省输出
    /// </summary>
    public class SystemDistrictFindDistrictByCountIdProvinceOutout
    {
        /// <summary>
        /// 县
        /// </summary>
        [Key]
        public string DistrictId { get; set; }

        /// <summary>
        /// 市
        /// </summary>		
        public string ParentId { get; set; }
    }
}
