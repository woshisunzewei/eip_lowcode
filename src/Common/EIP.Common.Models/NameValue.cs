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
namespace EIP.Common.Models
{
    /// <summary>
    /// 名称值对象
    /// </summary>
    public class NameValue<T>
    {
        /// <summary>
        /// 名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 值
        /// </summary>
        private T _value;
        /// <summary>
        /// 
        /// </summary>
        public NameValue()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public NameValue(string name, T value)
        {
            _name = name;
            _value = value;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}