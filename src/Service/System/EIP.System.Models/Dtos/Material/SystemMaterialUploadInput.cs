using EIP.Base.Models.Entities.System;
using Microsoft.AspNetCore.Http;

namespace EIP.System.Models.Dtos.Material
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemMaterialSaveInput: SystemMaterial
    {
        /// <summary>
        /// 
        /// </summary>
        public IFormFileCollection Files { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemMaterialSaveOutput
    {
       
    }
}