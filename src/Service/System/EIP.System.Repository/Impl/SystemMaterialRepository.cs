/**************************************************************
* Copyright (C) 2022 www.eipflow.com ����ΰ��Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 1039318332)
* ����ʱ��: 2022/01/12 22:40:15
* �ļ���: 
* ����: 
* 
* �޸���ʷ
* �޸��ˣ�
* ʱ�䣺
* �޸�˵����
*
**************************************************************/

using EIP.System.Models.Dtos.Material;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemMaterialRepository : ISystemMaterialRepository
    {
        /// <summary>
        /// ���Ӳ�ѯ��ҳ��ʽ
        /// </summary>
        /// <param name="input">��ѯ����</param>
        /// <returns>��ҳ</returns>
        public Task<PagedResults<SystemMaterial>> Find(SystemMaterialFindInput input)
        {
            var sql = new StringBuilder($"SELECT *,@rowNumber, @recordCount FROM System_Material @where and IsDelete=0 ");
                sql.Append(input.ParentId.HasValue ? $" and ParentId='{input.ParentId}' " : " and ParentId is null ");

            if (input.Name.Trim().IsNotNullOrEmpty())
            {
                sql.Append($" and Name like '%{input.Name.Trim()}%' ");
            }

            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemMaterial>(sql.ToString(), input);
        }
    }
}