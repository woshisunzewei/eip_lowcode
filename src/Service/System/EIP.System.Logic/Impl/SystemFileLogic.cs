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

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 文件存储表
    /// </summary>
    public class SystemFileLogic : DapperAsyncLogic<SystemFile>, ISystemFileLogic
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">文件存储表</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemFile input)
        {
            //新增
            if (input.FileId.IsEmptyGuid())
            {
                input.FileId = CombUtil.NewComb();
                return await InsertAsync(input);
            }
            return await UpdateAsync(input);
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<OperateStatus> UploadFile(IList<SystemFile> files)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                foreach (var file in files)
                {
                    await InsertAsync(file);
                }
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.Successful;
            }
            catch (Exception e)
            {
                operateStatus.Msg = e.Message;
            }
            return operateStatus;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteFileByRelationId(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            var files = await DeleteAsync(f => f.CorrelationId == input.Id);
            operateStatus.Msg = Chs.Successful;
            operateStatus.Code = ResultCode.Success;
            return operateStatus;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteFile(IdInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            var file = await FindAsync(f => f.FileId == input.Id);
            if (file != null)
            {
                file.IsDelete = true;
                await UpdateAsync(file);
            }
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            return operateStatus;
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemFile>> FindFile(IdInput<string> input)
        {
            if (!input.Id.IsNotNullOrEmpty()) return new List<SystemFile>();
            using (var fixture = new SqlDatabaseFixture())
            {
                return (await fixture.Db.SystemFile.FindByIdsAsync(input.Id)).Where(w => w.IsDelete == false).OrderBy(o => o.CreateTime).ToList();
            }
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IList<SystemFile>> FindFileByCorrelationId(IdInput<string> input)
        {
            if (!input.Id.IsNotNullOrEmpty()) return new List<SystemFile>();
            using (var fixture = new SqlDatabaseFixture())
            {
                return (await fixture.Db.SystemFile.SetSelect(s => new { s.FileId, s.Name, s.Path }).FindAllAsync(f => f.CorrelationId == input.Id && f.IsDelete == false)).OrderBy(o => o.CreateTime).ToList(); ;
            }
        }
    }
}
