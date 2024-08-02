/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: BigScreenMapLogic
* 描述: 地图数据维护表业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Big.Models.Dtos.ScreenMap;
using EIP.Common.Core.Context;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Repository;

namespace EIP.DataRoom.Logic.Impl
{
    /// <summary>
    /// 地图数据维护表业务逻辑接口实现
    /// </summary>
    public class BigScreenMapLogic : DapperAsyncLogic<BigScreenMap>, IBigScreenMapLogic
    {
        #region 构造函数

        private readonly IBigScreenMapRepository _bigScreenMapRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bigScreenMapRepository"></param>
        public BigScreenMapLogic(IBigScreenMapRepository bigScreenMapRepository)
        {
            _bigScreenMapRepository = bigScreenMapRepository;
        }

        #endregion

        #region 方法
        
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<BigScreenMapFindOutput>>> Find(BigScreenMapFindInput input)
        {
            return OperateStatus<PagedResults<BigScreenMapFindOutput>>.Success(await _bigScreenMapRepository.Find(input));
        }
         

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(BigScreenMap entity)
        {
            var edit = await FindAsync(f => f.id == entity.id);
            var currentUser = EipHttpContext.CurrentUser();
            if (edit != null)
            {
                return await UpdateAsync(entity);
            }
            return await InsertAsync(entity);
        }

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
                operateStatus = await DeleteAsync(f => f.id == Int32.Parse(item));
            }
            return operateStatus;
        }

        #endregion
    }
}