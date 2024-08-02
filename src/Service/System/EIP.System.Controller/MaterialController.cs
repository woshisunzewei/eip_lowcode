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
using EIP.System.Models.Dtos.Material;

namespace EIP.System.Controller
{
    /// <summary>
    /// 素材
    /// </summary>
    public class MaterialController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemMaterialLogic _systemMaterialLogic;
        /// <summary>
        /// 
        /// </summary>
        public MaterialController(ISystemMaterialLogic systemMaterialLogic)
        {
            _systemMaterialLogic = systemMaterialLogic;
        }
        #endregion

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("素材-方法-列表-分页获取", RemarkFrom.System)]
        [Route("/system/material/list")]
        public async  Task<ActionResult> Find(SystemMaterialFindInput paging)
        {
            return JsonForGridPaging(await _systemMaterialLogic.Find(paging));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("素材-方法-编辑-保存", RemarkFrom.System)]
        [Route("/system/material")]
        [RequestSizeLimit(100_000_000)]//最大100m左右
        public async  Task<ActionResult> Save()
        {
            SystemMaterialSaveInput input=new SystemMaterialSaveInput
            {
                Files= Request.Form.Files,
                MaterialId =Guid.Parse( Request.Form["MaterialId"]),
                Type = Convert.ToInt32(Request.Form["Type"]),
                Suffix =Request.Form["Suffix"].ToString(),
                Name = Request.Form["Name"].ToString()
            };
            var parentId = Request.Form["ParentId"].ToString();
            if (!parentId.IsNullOrEmpty()&&parentId!="null")
            {
                input.ParentId = Guid.Parse(Request.Form["ParentId"]);
            }
            return Ok(await _systemMaterialLogic.Save(input));
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("素材-方法-编辑-根据Id获取", RemarkFrom.System)]
        [Route("/system/material/{id}")]
        public async  Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _systemMaterialLogic.FindById(input));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键集合</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("素材-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/material/delete")]
        public async  Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _systemMaterialLogic.Delete(input));
        }
    }
}