/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:29
* 文件名: DsLabelLogic
* 描述: 标签业务逻辑接口实现
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
using EIP.DataRoom.Repository.Impl;
using System.Collections.Generic;

namespace EIP.DataRoom.Logic.Impl
{
    /// <summary>
    /// 标签业务逻辑接口实现
    /// </summary>
    public class DsLabelLogic : DapperAsyncLogic<DsLabel>, IDsLabelLogic
    {
        #region 构造函数

        private readonly IDsLabelRepository _dsLabelRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsLabelRepository"></param>
        public DsLabelLogic(IDsLabelRepository dsLabelRepository)
        {
            _dsLabelRepository = dsLabelRepository;
        }

        #endregion

        #region 方法
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<DsLabelFindOutput>>> Find(DsLabelFindInput input)
        {
            return OperateStatus<PagedResults<DsLabelFindOutput>>.Success(await _dsLabelRepository.Find(input));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<DsLabelFindOutput>>> FindLabelList()
        {
            var datas = await FindAllAsync();
            List<DsLabelFindOutput> outputs = new List<DsLabelFindOutput>();
            foreach (var data in datas)
            {
                outputs.Add(new DsLabelFindOutput
                {
                    id = data.id,
                    labelDesc = data.label_desc,
                    labelType = data.label_type,
                    labelName = data.label_name,
                });
            }
            return OperateStatus<List<DsLabelFindOutput>>.Success(outputs);
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<string>>> FindLabelType()
        {
            var datas = await FindAllAsync();
            List<string> outputs = new List<string>();
            foreach (var data in datas)
            {
                outputs.Add(data.label_name);
            }
            return OperateStatus<List<string>>.Success(outputs);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(DsLabelSaveInput entity)
        {
            var edit = await FindAsync(f => f.id == entity.id);
            var currentUser = EipHttpContext.CurrentUser();
            if (edit != null)
            {
                edit.update_date = DateTime.Now;
                edit.label_desc = entity.labelDesc;
                edit.label_name = entity.labelName;
                edit.label_type = entity.labelType;
                return await UpdateAsync(edit);
            }
            edit = new DsLabel();
            edit.create_date = DateTime.Now;
            edit.update_date = DateTime.Now;
            edit.label_desc = entity.labelDesc;
            edit.label_name = entity.labelName;
            edit.label_type = entity.labelType;
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