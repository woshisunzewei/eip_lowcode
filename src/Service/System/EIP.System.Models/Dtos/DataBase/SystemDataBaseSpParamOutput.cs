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

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 存储过程参数
    /// </summary>
    public class SystemDataBaseSpParamOutput 
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public string CleanName;
        /// <summary>
        /// 
        /// </summary>
        public string SysType;
        /// <summary>
        /// 
        /// </summary>
        public string DbType;
 
        /// <summary>
        /// 修改说明：添加存储过程说明参数，用于判断该参数是否是返回值
        /// </summary>
        public string Direction;
    }
}