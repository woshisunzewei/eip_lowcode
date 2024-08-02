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
using System;
using System.ComponentModel.DataAnnotations;
using EIP.Common.Models.Resx;

namespace EIP.Common.Models.Dtos
{
    /// <summary>
    /// 以传递一个实体Id值给应用服务方法。
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public class IdInput<TId>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = ResourceRequiredErrorMessage.主键不能未空)]
        public TId Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IdInput()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public IdInput(TId id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Id类型为Guid的一个快捷实现
    /// </summary>
    public class IdInput : IdInput<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        public IdInput()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public IdInput(Guid id)
            : base(id)
        {

        }
    }
}