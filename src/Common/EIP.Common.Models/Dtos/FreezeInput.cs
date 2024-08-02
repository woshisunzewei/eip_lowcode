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
namespace EIP.Common.Models.Dtos
{
    /// <summary>
    /// 冻结输入参数
    /// </summary>
    public class FreezeInput
    {
         /// <summary>
        /// 唯一标识
        /// </summary>
        public bool? IsFreeze { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FreezeInput()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isFreeze"></param>
        public FreezeInput(bool? isFreeze)
        {
            IsFreeze = isFreeze;
        }
    }
}