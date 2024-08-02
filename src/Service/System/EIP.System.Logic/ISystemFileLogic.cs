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
    /// 文件存储表
    /// </summary>
    public interface ISystemFileLogic : IAsyncLogic<SystemFile>
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">文件存储表</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemFileLogic_Cache")]
        Task<OperateStatus> Save(SystemFile input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemFileLogic_Cache")]
        Task<OperateStatus> DeleteFileByRelationId(IdInput<string> input);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemFileLogic_Cache")]
        Task<OperateStatus> UploadFile(IList<SystemFile> files);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemFileLogic_Cache")]
        Task<OperateStatus> DeleteFile(IdInput input);

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemFileLogic_Cache")]
        Task<IEnumerable<SystemFile>> FindFile(IdInput<string> input);

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemFileLogic_Cache")]
        Task<IList<SystemFile>> FindFileByCorrelationId(IdInput<string> input);
    }
}
