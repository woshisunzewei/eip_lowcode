/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:28
* 文件名: DsDatasetLabelLogic
* 描述: 数据集与标签关联表业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Common.Core.Context;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Models.Dtos;
using EIP.DataRoom.Repository;

namespace EIP.DataRoom.Logic.Impl
{
    /// <summary>
    /// 数据集与标签关联表业务逻辑接口实现
    /// </summary>
    public class DsDatasetLabelLogic : DapperAsyncLogic<DsDatasetLabel>, IDsDatasetLabelLogic
    {
        #region 构造函数

        private readonly IDsDatasetLabelRepository _dsDatasetLabelRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsDatasetLabelRepository"></param>
        public DsDatasetLabelLogic(IDsDatasetLabelRepository dsDatasetLabelRepository)
        {
            _dsDatasetLabelRepository = dsDatasetLabelRepository;
        }

        #endregion

        #region 方法
        
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<DsDatasetLabelFindOutput>>> Find(DsDatasetLabelFindInput input)
        {
            return OperateStatus<PagedResults<DsDatasetLabelFindOutput>>.Success(await _dsDatasetLabelRepository.Find(input));
        }
         
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<DsDatasetLabel>> FindById(IdInput<int> input)
        {
            return OperateStatus<DsDatasetLabel>.Success(await FindAsync(f => f.id == input.Id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(DsDatasetLabel entity)
        {
            var edit = await FindAsync(f => f.id== entity.id);
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
                operateStatus = await DeleteAsync(f => f.id ==Int32.Parse(item));
            }
            return operateStatus;
        }

        #endregion
    }
}