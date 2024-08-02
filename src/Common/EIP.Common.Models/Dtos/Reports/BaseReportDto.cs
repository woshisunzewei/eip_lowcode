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
using System.IO;

namespace EIP.Common.Models.Dtos.Reports
{
    /// <summary>
    /// 基础导出类
    /// </summary>
    public class BaseReportDto
    {
        /// <summary>
        /// 模版地址
        /// </summary>
        public string TemplatePath { get; set; }

        /// <summary>
        /// 导出地址
        /// </summary>
        private string ExportPath
        {
            get
            {
                string exportPath = "D://Export";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                return exportPath;
            }
        }

        /// <summary>
        /// 下载模版地址
        /// </summary>
        public string DownTemplatePath { get; set; }
        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownPath
        {
            get { return ExportPath + DownTemplatePath; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
    }
}