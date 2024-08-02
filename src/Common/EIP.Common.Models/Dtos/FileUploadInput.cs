/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/17 21:28:58
* 文件名: FileUploadInput
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/

using System;

namespace EIP.Common.Models.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class FileUploadInput
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// 单个上传
        /// </summary>
        public bool Single { get; set; } = true;

        /// <summary>
        /// 自定义路径名称
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 文件标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Pdf显示
        /// </summary>
        public bool PdfShow { get; set; }
    }
}
