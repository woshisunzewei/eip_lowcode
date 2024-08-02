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

namespace EIP.System.Models.Dtos.Data
{
    /// <summary>
    /// 数据权限规则Json输出参数
    /// 1、不同系统需要的规则参数不一样,可添加此实体进行扩展
    /// </summary>
    public class SystemDataRuleJsonDoubleWay 
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public object Type { get; set; }
    }
}