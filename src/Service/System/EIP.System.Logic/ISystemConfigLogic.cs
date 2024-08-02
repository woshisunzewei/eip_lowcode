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
namespace EIP.System.Logic
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public interface ISystemConfigLogic : IAsyncLogic<SystemConfig>
	{
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">系统配置</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemConfigLogic_Cache")]
		Task<OperateStatus> Save(SystemConfigSaveInput input);

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemConfigLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemConfigFindAllOutput>>> FindAll();

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemConfigLogic_Cache")]
        Task<string> FindByKey(string key);

        /// <summary>
        /// 获取附件配置
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemConfigLogic_Cache")]
        Task<OperateStatus<FilesStorageOptions>> FindFilesStorageOptions();

    }
}
