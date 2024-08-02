/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/15 15:23:03
* 文件名: SystemCodeGenerationFindPagingBusinessData
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models;
using EIP.Common.Models.Paging;
using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 继承分页参数
    /// </summary>
    public class SystemDataBaseFindPagingBusinessDataInput : QueryParam
    {
        /// <summary>
        /// 代码生成Id
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 是否分页
        /// </summary>
        public bool IsPaging { get; set; }

        /// <summary>
        /// 路由信息
        /// </summary>
        public ViewRote Rote { get; set; }

        /// <summary>
        /// 是否具有数据权限
        /// </summary>
        public bool HaveDataPermission { get; set; }

        /// <summary>
        /// 导出列
        /// </summary>
        public IList<SystemDataBaseReportFindBusinessDataColsInput> Cols { get; set; } = new List<SystemDataBaseReportFindBusinessDataColsInput>();

        /// <summary>
        /// 导出文件名称
        /// </summary>
        public string ReportName { get; set; }
            
        /// <summary>
        /// 
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 是否删除，默认为未删除
        /// </summary>
        public int IsDelete { get; set; } = 0;
    }
    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseReportFindBusinessDataColsInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public List<SystemDataBaseReportFindBusinessDataColsStyleInput> Style { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseReportFindBusinessDataColsStyleInput
    {
        /// <summary>
        /// 内容
        /// </summary>
       public string Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 所有列表
    /// </summary>
    public class SystemDataBaseFindPagingBusinessDataTableInput
    {
        /// <summary>
        /// 列
        /// </summary>
        public IList<SystemDataBaseFindPagingBusinessDataTableColunmsInput> Columns { get; set; }
    }

    /// <summary>
    /// 列表名
    /// </summary>
    public class SystemDataBaseFindPagingBusinessDataTableColunmsInput
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否需要查询
        /// </summary>
        public bool IsSearch { get; set; }

        /// <summary>
        /// 是否统计合计
        /// </summary>
        public bool IsTotal { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 掩码显示
        /// </summary>
        public string Mask { get; set; }

        /// <summary>
        /// 格式化
        /// </summary>
        public string Format { get; set; }
    }
}
