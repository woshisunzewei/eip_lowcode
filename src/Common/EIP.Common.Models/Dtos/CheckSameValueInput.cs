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
    /// 是否具有相同值输入DTO
    /// </summary>
    public class CheckSameValueInput : IdInput
    {
        /// <summary>
        /// 值
        /// </summary>
        public string Param { get; set; }
    }

    /// <summary>
    /// 传递一个可为空的Id给服务方法
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public class CheckSameValueInput<TId>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public TId Id { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckSameValueInput()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        public CheckSameValueInput(TId id)
        {
            Id = id;
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Param { get; set; }

    }
}