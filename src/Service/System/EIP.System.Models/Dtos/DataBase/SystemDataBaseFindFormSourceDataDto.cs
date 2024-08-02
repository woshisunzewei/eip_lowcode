using EIP.Common.Models.Paging;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseFindFormSourceDataInput : QueryParam
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid DataSourceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InParams { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public dynamic FormValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Columns { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseFindFormSourceDataColumnsInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Mask { get; set; } = "";
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseFindFormSourceDataInParamsInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Condition { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseFindFormSourceDataOutput : object
    {
        /// <summary>
        /// 总数
        /// </summary>
        [JsonIgnore]
        [NotMapped]
        public long RecordCount { get; set; }
    }
}