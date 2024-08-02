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
namespace EIP.System.Logic
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    public interface ISystemDataBaseLogic
    {
        /// <summary>
        /// 获取所有应用数据库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> FindTableTree();

        /// <summary>
        /// 查看对应数据库空间占用情况
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDataBaseSpaceOutput>>> FindDataBaseSpaceused();

        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDataBaseTableDto>>> FindDataBaseTable(IdInput input);

        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDataBaseTableDto>>> FindDataBaseView(IdInput input);
        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDataBaseTableDto>>> FindDataBaseProc(IdInput input);
        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDataBaseTableDto>>> FindDataBaseWorkflowTables(IdInput input);

        /// <summary>
        /// 获取对应表列信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDataBaseColumnDto>>> FindDataBaseColumns(SystemDataBaseTableDto input);

        /// <summary>
        /// 获取对应表列信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDataBaseColumnDto>>> FindWorkflowDataBaseColumnsList(SystemDataBaseTableDto input);

        /// <summary>
        /// 获取外键信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDataBaseFkColumnOutput>>> FindDataBasefFkColumn(SystemDataBaseTableDto input);

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<bool>> IsTableExist(SystemDataBaseIsTableExistInput input);

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataBaseLogic_Cache,ISystemConfigLogic_Cache")]
        OperateStatus SaveFormTable(SystemDataBaseSaveFormTableInput input);

        /// <summary>
        /// 修改表字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> SaveFormTableField(SystemDataBaseSaveFormTableFieldInput input);

        /// <summary>
        /// 保存表单数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> SaveBusinessData(SystemDataBaseSaveBusinessDataInput input);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<object>> FindBusinessDataById(SystemDataBaseFindBusinessDataByIdInput input);

        /// <summary>
        /// 获取业务数据
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<dynamic>> FindBusinessData(SystemDataBaseFindPagingBusinessDataInput paging);

        /// <summary>
        /// 获取左侧查询
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<List<SystemDataBaseFindBusinessDataFilterSearchOutput>>> FindBusinessDataFilterSearch(SystemDataBaseFindBusinessDataFilterSearchInput paging);

        /// <summary>
        /// 获取业务数据
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<OperateStatus<Dictionary<string, object>>> FindBusinessDataById(SystemDataBaseExportTemplateInput paging);

        /// <summary>
        /// 获取业务数据
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<dynamic>> FindBusinessDataPage(SystemDataBaseFindPagingBusinessDataInput paging);

        /// <summary>
        /// 获取表尾数据
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<dynamic>> FindBusinessDataFooter(SystemDataBaseFindPagingBusinessDataInput paging);

        /// <summary>
        /// 删除业务数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> DeleteBusinessData(SystemDataBaseDeleteBusinessDataInput input);

        /// <summary>
        /// 删除业务数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> DeleteBusinessDataPhysics(SystemDataBaseDeleteBusinessDataInput input);

        /// <summary>
        /// 删除业务数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> DeleteBusinessDataPhysicsAll(IdInput input);

        /// <summary>
        /// 恢复删除业务数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> RecoveryDeleteBusinessDataPhysics(SystemDataBaseDeleteBusinessDataInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<dynamic>> FindBatchData(DataBaseSubTableDto input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<dynamic>> FindFormSourceData(SystemDataBaseFindFormSourceDataInput input);

        /// <summary>
        /// 查询业务数据分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<dynamic>> FindFormSourceDataPaging(SystemDataBaseFindFormSourceDataInput input);
        /// <summary>
        /// 查询业务数据分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<dynamic>> FindFormSourceDataPagingTable(SystemDataBaseFindTableInput input);

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="input"></param>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> ImportBusinessData(SystemDataBaseImportInput input);

        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDataBase>>> FindAll();

        /// <summary>
        /// 保存数据库配置
        /// </summary>
        /// <param name="input">数据库配置</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> Save(SystemDataBase input);

        /// <summary>
        /// 获取外键信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> Delete(IdInput input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<SystemDataBase>> FindById(IdInput input);

    }
}