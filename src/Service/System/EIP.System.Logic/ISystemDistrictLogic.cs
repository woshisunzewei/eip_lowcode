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
using EIP.System.Models.Dtos.District;

namespace EIP.System.Logic
{
    /// <summary>
    /// 省市县行政区划
    /// </summary>
    public interface ISystemDistrictLogic : IAsyncLogic<SystemDistrict>
    {
        /// <summary>
        /// 根据父级查询所有子集树形结构
        /// </summary>
        /// <param name="input">父级Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDistrictLogic_Cache")]
        Task<OperateStatus<IEnumerable<TreeEntity>>> FindSync(IdInput<string> input);

        /// <summary>
        /// 根据父级查询所有子集
        /// </summary>
        /// <param name="input">父级Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDistrictLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDistrict>>> FindByParentId(IdInput<string> input);

        /// <summary>
        /// 根据县Id获取省市县Id
        /// </summary>
        /// <param name="input">县Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDistrictLogic_Cache")]
        Task<OperateStatus<SystemDistrictFindDistrictByCountIdOutout>> FindByCountId(IdInput<string> input);

        /// <summary>
        /// 检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDistrictLogic_Cache")]
        Task<OperateStatus> Check(CheckSameValueInput input);

        /// <summary>
        /// 保存省市县信息
        /// </summary>
        /// <param name="systemDistrict">省市县信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDistrictLogic_Cache")]
        Task<OperateStatus> Save(SystemDistrict systemDistrict);

        /// <summary>
        /// 删除省市县及下级数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDistrictLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);
    }
}