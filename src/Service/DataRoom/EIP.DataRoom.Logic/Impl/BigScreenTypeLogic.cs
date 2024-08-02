/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: BigScreenTypeLogic
* 描述: 大屏、资源库、组件库分类业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Big.Models.Dtos.ScreenType;
using EIP.Common.Core.Context;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.DataRoom.Models.Dtos;

namespace EIP.DataRoom.Logic.Impl
{
    /// <summary>
    /// 大屏、资源库、组件库分类业务逻辑接口实现
    /// </summary>
    public class BigScreenTypeLogic : DapperAsyncLogic<BigScreenType>, IBigScreenTypeLogic
    {
        #region 方法
        
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<BigScreenTypeFindOutput>>> FindType(string type)
        {
            var data = await FindAllAsync(f => f.type == type);
            List<BigScreenTypeFindOutput> output = new List<BigScreenTypeFindOutput>();
            foreach (var item in data)
            {
                output.Add(new BigScreenTypeFindOutput
                {
                    id = item.id,
                    code = item.code,
                    name = item.name,
                    orderNum = item.order_num,
                    type = item.type,
                });
            }
            return OperateStatus<List<BigScreenTypeFindOutput>>.Success(output);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(BigScreenTypeSaveInput entity)
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
            edit = new BigScreenType();
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