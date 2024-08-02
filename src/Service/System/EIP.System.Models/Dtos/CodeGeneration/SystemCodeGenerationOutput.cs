/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
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

namespace EIP.System.Models.Dtos.CodeGeneration
{
    /// <summary>
    /// 代码生成器输出参数
    /// </summary>
    public class SystemCodeGenerationOutput
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TableTitleOther { get; set; }
        /// <summary>
        /// 实体类名
        /// </summary>
        public string Entities { get; set; }

        /// <summary>
        /// 查询实体
        /// </summary>
        public string FindDto { get; set; }

        /// <summary>
        /// Business接口
        /// </summary>
        public string Logic { get; set; }

        /// <summary>
        /// Business实现
        /// </summary>
        public string LogicImpl { get; set; }

        /// <summary>
        /// Repository接口
        /// </summary>
        public string Repository { get; set; }

        /// <summary>
        /// Repository实现
        /// </summary>
        public string RepositoryImpl { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// IDbContext实现
        /// </summary>
        public string IDbContext { get; set; }

        /// <summary>
        /// SqlDbContext
        /// </summary>
        public string SqlDbContext { get; set; }
    }
}