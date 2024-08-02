/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:27
* 文件名: DashboardTypeLogic
* 描述: 大屏、资源库、组件库分类业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Common.Core.Context;
using EIP.Common.Core.Extension;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.DataRoom.Models.Dtos;
using EIP.DataRoom.Repository;

namespace EIP.DataRoom.Logic.Impl
{
    /// <summary>
    /// 大屏、资源库、组件库分类业务逻辑接口实现
    /// </summary>
    public class DashboardTypeLogic : DapperAsyncLogic<DashboardType>, IDashboardTypeLogic
    {
        #region 构造函数

        private readonly IDashboardTypeRepository _dashboardTypeRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dashboardTypeRepository"></param>
        public DashboardTypeLogic(IDashboardTypeRepository dashboardTypeRepository)
        {
            _dashboardTypeRepository = dashboardTypeRepository;
        }

        #endregion

        #region 方法
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<DashboardTypeFindOutput>>> FindType(string type)
        {
            var data = await FindAllAsync(f=>f.type== type);
             List<DashboardTypeFindOutput> output = new List<DashboardTypeFindOutput>();
            foreach (var item in data)
            {
                output.Add(new DashboardTypeFindOutput
                {
                    id = item.id,
                    code = item.code,
                    name = item.name,
                    orderNum = item.order_num,
                    type = item.type,
                });
            }
            return OperateStatus<List<DashboardTypeFindOutput>>.Success(output);
        }
       
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<DashboardType>> FindById(IdInput<int> input)
        {
            return OperateStatus<DashboardType>.Success(await FindAsync(f => f.id == input.Id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(DashboardTypeSaveInput entity)
        {
            var edit = await FindAsync(f => f.id == entity.id);
            var currentUser = EipHttpContext.CurrentUser();
            if (edit != null)
            {
                edit.update_date = DateTime.Now;
                edit.code = entity.code;
                edit.name = entity.name;
                edit.order_num = entity.orderNum;
                edit.type = entity.type;
                return await UpdateAsync(edit);
            }
            edit = new DashboardType();
            edit.create_date = DateTime.Now;
            edit.update_date = DateTime.Now;
            edit.code = entity.code;
            edit.name = entity.name;
            edit.order_num = entity.orderNum;
            edit.type = entity.type;
            return await InsertAsync(edit);
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