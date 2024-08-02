/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:39:27
* 文件名: BigScreenBizComponentFindDto
* 描述: 业务组件表
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;

namespace EIP.Big.Models.Dtos.ScreenBizComponent
{
    /// <summary>
    /// 业务组件表查询参数
    /// </summary>
    public class BigScreenBizComponentFindInput : QueryParam
    {

    }

    /// <summary>
    /// 业务组件表查询输出
    /// </summary>
    public class BigScreenBizComponentFindOutput
    {
        public int id { get; set; }
        /// <summary>
        /// 业务组件中文名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 业务组件编码，唯一标识符
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 封面图片文件路径
        /// </summary>
        public string coverPicture { get; set; }

        /// <summary>
        /// vue组件内容
        /// </summary>
        public string vueContent { get; set; }

        /// <summary>
        /// 组件配置内容
        /// </summary>
        public string settingContent { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int orderNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public string moduleCode { get; set; }

    }
}