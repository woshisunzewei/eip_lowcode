/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/8/22 7:58:35
* 文件名: ISystemSerialNoLogic
* 描述: 编号规则业务逻辑接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.SerialNo;

namespace EIP.System.Logic
{
    /// <summary>
    /// 编号规则业务逻辑接口
    /// </summary>
    public interface ISystemSerialNoLogic : IAsyncLogic<SystemSerialNo>
    {
        /// <summary>
        /// 保存后，清除用户对应的单号信息
        /// </summary>
        /// <param name="prefix"></param>
        void Clear(Guid configId, string model);

        /// <summary>
        /// 获取单据编号
        /// </summary>
        /// <param name="input">单据类型</param>
        /// <returns></returns>
        Task<OperateStatus<string>> Create(SystemSerialCreateInput input);
    }
}