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
    /// Input值
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class Input<TValue> 
    {
        /// <summary>
        /// 
        /// </summary>
        public TValue Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Input()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public Input(TValue value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Id类型为Guid的一个快捷实现
    /// </summary>
    public class Input : Input<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public Input()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public Input(string id)
            : base(id)
        {

        }
    }
}