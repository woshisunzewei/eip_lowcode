/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2020/2/9 19:15:39
* 文件名: SystemOrganizationImportDto
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using MiniExcelLibs.Attributes;

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 组织导入
    /// </summary>
    public class SystemOrganizationImportDto
    {
        /// <summary>
        /// 名称
        /// </summary>	
        [ExcelColumn(Name = "名称")]
        public string Name { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        [ExcelColumn(Name = "父级名称")]
        public string ParentName { get; set; }

    }
}
