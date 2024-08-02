using EIP.System.Models.Dtos.Material;

namespace EIP.System.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISystemMaterialRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<PagedResults<SystemMaterial>> Find(SystemMaterialFindInput paging);
    }
}