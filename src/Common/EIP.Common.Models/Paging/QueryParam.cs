/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
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
using Newtonsoft.Json;
namespace EIP.Common.Models.Paging
{
    /// <summary>
    /// 说  明:分页基础参数
    /// 备  注:用于分页实体继承,必须基础该实体才可进行分页
    /// 编写人:孙泽伟-2015/03/25
    /// </summary>
    public class QueryParam : SearchDto
    {
        /// <summary>
        /// 无参构造函数,提供默认值
        /// </summary>
        public QueryParam()
        {
            Size = 100;
            Current = 1;
            Sord = "desc";
        }

        /// <summary>
        /// 页码,如:1
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// 每页显示数量,如:100
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 排序字段(可多个),如:Title
        /// </summary>
        public string Sidx { get; set; }

        /// <summary>
        /// 默认排序方式,如:asc
        /// </summary>
        public string Sord { get; set; }

        /// <summary>
        /// 是否为导出
        /// </summary>
        [JsonIgnore]
        public bool IsReport { get; set; }

        /// <summary>
        /// 数据权限对应Sql
        /// </summary>
        [JsonIgnore]
        public string DataSql { get; set; }

        ///// <summary>
        ///// 字段权限对应的Sql
        ///// </summary>
        //[JsonIgnore]
        //public string FiledSql { get; set; }

        #region 存储过程分页使用
        /// <summary>
        /// 表名,多表是请使用 tA a inner join tB b On a.AID = b.AID
        /// </summary>
        [JsonIgnore]
        public string TableName { get; set; }

        /// <summary>
        /// 返回字段默认为*
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// where过滤条件(不带where)
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// Group语句(不带Group By)
        /// </summary>
        [JsonIgnore]
        public string Group { get; set; }
        #endregion

        /// <summary>
        /// 时间戳:可通过设置该值达到不读取缓存数据效果
        /// </summary>
        public string TimeStamp { get; set; }

    }

}