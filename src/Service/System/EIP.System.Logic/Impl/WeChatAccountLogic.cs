/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
namespace EIP.WeChat.Logic.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class WeChatAccountLogic : DapperAsyncLogic<SystemUserInfoThree>, IWeChatAccountLogic
    {

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            foreach (var item in input.Id.Split(','))
            {
                operateStatus = await DeleteAsync(f => f.ThreeUserId == Guid.Parse(item));
            }
            return operateStatus;
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemUserInfoThree>> FindById(IdInput input)
        {
            return OperateStatus<SystemUserInfoThree>.Success(await FindAsync(f => f.ThreeUserId == input.Id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemUserInfoThree input)
        {
            var editData = await FindAsync(f => f.ThreeUserId == input.ThreeUserId);
            if (editData != null)
            {
                editData.Id = input.Id;
                return await UpdateAsync(input);
            }
            input.CreateTime = DateTime.Now;
            input.ThreeUserId = CombUtil.NewComb();
            return await InsertAsync(input);
        }
    }
}