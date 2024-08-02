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
using EIP.System.Models.Dtos.ShortCut;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 系统快捷方式
    /// </summary>
    public class SystemShortCutLogic : DapperAsyncLogic<SystemShortCut>, ISystemShortCutLogic
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">系统快捷方式</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemShortCutSaveInput input)
        {
            OperateStatus operate = new OperateStatus();
            await DeleteAll(new SystemShortCutDeleteAllInput { UserId = input.UserId, Type = input.Type });
            foreach (var item in input.ShortCuts)
            {
                item.ShortCutId = Guid.NewGuid();
                item.UserId = input.UserId;
                item.Type = input.Type;
                item.MenuId = item.MenuId;
                item.CreateTime = DateTime.Now;
            }
            operate = await BulkInsertAsync(input.ShortCuts);
            return operate;
        }

        /// <summary>
        /// 保存排序号
        /// </summary>
        /// <param name="input">保存排序号</param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveOrderNo(IEnumerable<SystemShortCut> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            foreach (var shortCut in input)
            {
                operateStatus = await UpdateAsync(u => u.MenuId == shortCut.MenuId && u.UserId == shortCut.UserId, new { OrderNo = shortCut.OrderNo });
            }
            return operateStatus;
        }

        /// <summary>
        /// 根据用户Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemShortCutFindByUserIdOutput>>> FindByUserId(SystemShortCutFindByUserIdInput input)
        {
            IList<SystemShortCutFindByUserIdOutput> outputs = new List<SystemShortCutFindByUserIdOutput>();
            if (input.Type == EnumShortCutType.Pc.ToShort())
            {
                foreach (var shortCut in (await FindAllAsync<SystemMenu>(f => f.UserId == input.UserId && f.Type == input.Type, q => q.SystemMenu)).OrderBy(o => o.OrderNo))
                {
                    outputs.Add(new SystemShortCutFindByUserIdOutput
                    {
                        Icon = shortCut.SystemMenu.Icon,
                        MenuId = shortCut.MenuId,
                        Name = shortCut.SystemMenu.Name,
                        OpenUrl = shortCut.SystemMenu.Path,
                        OrderNo = shortCut.SystemMenu.OrderNo
                    });
                }
            }
            return OperateStatus<IEnumerable<SystemShortCutFindByUserIdOutput>>.Success(outputs);
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteAll(SystemShortCutDeleteAllInput input)
        {
            return await DeleteAsync(d => d.UserId == input.UserId && d.Type == input.Type);
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(SystemShortCut input)
        {
            return await DeleteAsync(d => d.UserId == input.UserId && d.MenuId == input.MenuId);
        }
    }
}
