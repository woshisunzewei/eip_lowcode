/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:29
* 文件名: DsDatasourceFindDto
* 描述: 数据源配置表
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;

namespace EIP.DataRoom.Models.Dtos
{
    /// <summary>
    /// 数据源配置表查询参数
    /// </summary>
    public class DsDatasourceFindInput : QueryParam
    {

    }

    /// <summary>
    /// 数据源配置表查询输出
    /// </summary>
    public class DsDatasourceFindOutput
    {
        public int id { get; set; }
        /// <summary>
        /// 数据源名称
        /// </summary>
        public string sourceName { get; set; }

        /// <summary>
        /// 数据源类型
        /// </summary>
        public string sourceType { get; set; }

        /// <summary>
        /// 连接驱动
        /// </summary>
        public string driverClassName { get; set; }

        /// <summary>
        /// 连接url
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int? port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string tableName { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public string moduleCode { get; set; }

        /// <summary>
        /// 是否可编辑，0 不可编辑 1 可编辑
        /// </summary>
        public int editable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }


    }
}