/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:28
* 文件名: DsCategoryTreeLogic
* 描述: 数据集种类树业务逻辑接口实现
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
using EIP.Common.Models.Paging;
using EIP.Common.Models.Tree;
using EIP.Common.Repository;
using EIP.DataRoom.Models.Dtos;
using EIP.DataRoom.Repository;
using System.Collections.Generic;

namespace EIP.DataRoom.Logic.Impl
{
    /// <summary>
    /// 数据集种类树业务逻辑接口实现
    /// </summary>
    public class DsCategoryTreeLogic : DapperAsyncLogic<DsCategoryTree>, IDsCategoryTreeLogic
    {
        #region 构造函数

        private readonly IDsCategoryTreeRepository _dsCategoryTreeRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsCategoryTreeRepository"></param>
        public DsCategoryTreeLogic(IDsCategoryTreeRepository dsCategoryTreeRepository)
        {
            _dsCategoryTreeRepository = dsCategoryTreeRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<DsCategoryTreeFindOutput>>> Find(DsCategoryTreeFindInput input)
        {
            var datas = await FindAllAsync();
            if (input.type.IsNotNullOrEmpty())
            {
                datas = datas.Where(w => w.type == input.type);
            }
            List<DsCategoryTreeFindOutput> treeFindOutputs = new List<DsCategoryTreeFindOutput>();

            List<DsCategoryTreeFindOutput> treeConvert = new List<DsCategoryTreeFindOutput>();
            foreach (var data in datas)
            {
                var parent = datas.FirstOrDefault(f => f.id == data.parent_id);
                DsCategoryTreeFindOutput treeEntity = new DsCategoryTreeFindOutput
                {
                    id = data.id,
                    name = data.name,
                    parentId = data.parent_id,
                    parentName = parent?.name,
                    type = data.type,
                };
                treeConvert.Add(treeEntity);
            }
            foreach (var data in treeConvert.Where(w => w.parentId==0))
            {
                var parent = datas.FirstOrDefault(f => f.id == data.parentId);
                DsCategoryTreeFindOutput treeEntity = data;
                treeEntity.children = FindJsonForTreeChildren(treeConvert.ToList(), data);
                treeFindOutputs.Add(treeEntity);
            }
            return OperateStatus<List<DsCategoryTreeFindOutput>>.Success(treeFindOutputs.ToList());
        }
        /// <summary>
        /// 递归获取树形结构
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="treeEntity"></param>
        private List<DsCategoryTreeFindOutput> FindJsonForTreeChildren(List<DsCategoryTreeFindOutput> datas, DsCategoryTreeFindOutput treeEntity)
        {
            List<DsCategoryTreeFindOutput> treeEntities = new List<DsCategoryTreeFindOutput>();
            var children = datas.Where(w => w.parentId.ToString() == treeEntity.id.ToString());
            foreach (var item in children)
            {
                DsCategoryTreeFindOutput treeEntityChidren = item;
                treeEntityChidren.children = FindJsonForTreeChildren(datas, item);
                treeEntities.Add(treeEntityChidren);
            }
            return treeEntities;
        }
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<DsCategoryTree>> FindById(IdInput<int> input)
        {
            return OperateStatus<DsCategoryTree>.Success(await FindAsync(f => f.id == input.Id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(DsCategoryTreeSaveInput entity)
        {
            var edit = await FindAsync(f => f.id == entity.id);
            var currentUser = EipHttpContext.CurrentUser();
            if (edit != null)
            {
                edit.update_date = DateTime.Now;
                edit.name = entity.name;
                edit.parent_id = entity.parentId;
                edit.type = entity.type;
                return await UpdateAsync(edit);
            }
            edit = new DsCategoryTree();
            edit.create_date = DateTime.Now;
            edit.update_date = DateTime.Now;
            edit.parent_id = entity.parentId;
            edit.name = entity.name;
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