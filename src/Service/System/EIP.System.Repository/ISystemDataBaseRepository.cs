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
namespace EIP.System.Repository
{
    /// <summary>
    /// 数据库
    /// </summary>
    public interface ISystemDataBaseRepository
    {
        /// <summary>
        /// 查看对应数据库空间占用情况
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SystemDataBaseSpaceOutput>> FindDataBaseSpaceused();

        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemDataBaseTableDto>> FindDataBaseTable(SystemDataBaseInput input);

        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemDataBaseTableDto>> FindDataBaseView(SystemDataBaseInput input);

        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemDataBaseTableDto>> FindDataBaseProc(SystemDataBaseInput input);

        /// <summary>
        /// 获取对应表列信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemDataBaseColumnDto>> FindDataBaseColumns(SystemDataBaseTableDto input);

        /// <summary>
        /// 获取外键信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemDataBaseFkColumnOutput>> FinddatabsefFkColumn(SystemDataBaseTableDto input);

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> IsTableExist(SystemDataBaseIsTableExistInput input);

    }
}