/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: BigScreenMapFindDto
* 描述: 地图数据维护表
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using System;

namespace EIP.Big.Models.Dtos.ScreenMap
{
    /// <summary>
    /// 地图数据维护表查询参数
    /// </summary>
    public class BigScreenMapFindInput : QueryParam
    {
        
    }

    /// <summary>
    /// 地图数据维护表查询输出
    /// </summary>
    public class BigScreenMapFindOutput
    {
                /// <summary>
        /// 父级地图id
        /// </summary>
        public string parent_id { get; set; }

        /// <summary>
        /// 地图编码
        /// </summary>
        public string map_code { get; set; }

        /// <summary>
        /// 地图名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 地图geoJson
        /// </summary>
        public String geo_json { get; set; }

        /// <summary>
        /// 地图级别 0-世界 1-国家 2-省 3-市 4-区县
        /// </summary>
        public byte[] level { get; set; }

        /// <summary>
        /// 是否已上传geoJson 0-否 1-是
        /// </summary>
        public byte[] uploaded_geo_json { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime update_date { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_date { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int? create_by { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public int? update_by { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public int del_flag { get; set; }


    }
}