/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:39:27
* 文件名: BigScreenBizComponentLogic
* 描述: 业务组件表业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Big.Models.Dtos.ScreenBizComponent;
using EIP.Common.Core.Context;
using EIP.Common.Core.Resource;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Models.Dtos;
using EIP.DataRoom.Repository;

namespace EIP.DataRoom.Logic.Impl
{
    /// <summary>
    /// 业务组件表业务逻辑接口实现
    /// </summary>
    public class BigScreenBizComponentLogic : DapperAsyncLogic<BigScreenBizComponent>, IBigScreenBizComponentLogic
    {
        #region 构造函数

        private readonly IBigScreenBizComponentRepository _bigScreenBizComponentRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bigScreenBizComponentRepository"></param>
        public BigScreenBizComponentLogic(IBigScreenBizComponentRepository bigScreenBizComponentRepository)
        {
            _bigScreenBizComponentRepository = bigScreenBizComponentRepository;
        }

        #endregion

        #region 方法
        
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<BigScreenBizComponentFindOutput>>> Find(BigScreenBizComponentFindInput input)
        {
            return OperateStatus<PagedResults<BigScreenBizComponentFindOutput>>.Success(await _bigScreenBizComponentRepository.Find(input));
        }
         
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(BigScreenBizComponentSaveInput entity)
        {
            var edit = await FindAsync(f => f.id == entity.id);
            var currentUser = EipHttpContext.CurrentUser();
            OperateStatus<string> operate = new OperateStatus<string>();
            OperateStatus operateStatus = new OperateStatus();
            if (edit != null)
            {
                edit.code = entity.code;
                edit.name = entity.name;
                edit.remark = entity.remark;
                edit.type = entity.type;
                edit.update_date = DateTime.Now;
                edit.cover_picture = entity.coverPicture;
                edit.vue_content = entity.vueContent;
                edit.setting_content = entity.settingContent;
                edit.order_num = entity.orderNum;
                edit.module_code = entity.moduleCode;
                operateStatus = await UpdateAsync(edit);
            }
            else
            {
                edit = new BigScreenBizComponent();
                edit.code = "bizComponent__" + Guid.NewGuid();
                edit.name = entity.name;
                edit.remark = entity.remark;
                edit.type = entity.type;
                edit.update_date = DateTime.Now;
                edit.create_date = DateTime.Now;
                edit.update_date = DateTime.Now;
                edit.cover_picture = entity.coverPicture;
                edit.vue_content = entity.vueContent;
                edit.setting_content = entity.settingContent;
                edit.order_num = entity.orderNum;
                edit.module_code = entity.moduleCode;
                operateStatus = await InsertAsync(edit);
            }
            if (operateStatus.Code != ResultCode.Success)
            {
                operate.Msg = operateStatus.Msg;
                return operate;
            }
            operate.Data = edit.code;
            operate.Code = ResultCode.Success;
            operate.Msg = Chs.Successful;
            return operate;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<BigScreenBizComponentInfoOutput>> Info(IdInput<string> input)
        {
            var data = await FindAsync(f => f.code == input.Id);
            BigScreenBizComponentInfoOutput output = new BigScreenBizComponentInfoOutput()
            {
                code = data.code,
                coverPicture = data.cover_picture,
                id = data.id,
                moduleCode = data.module_code,
                name = data.name,
                orderNum = data.order_num,
                remark = data.remark,
                settingContent = data.setting_content,
                type = data.type,
                vueContent = data.vue_content
            };
            return OperateStatus<BigScreenBizComponentInfoOutput>.Success(output);
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