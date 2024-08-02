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

namespace EIP.System.Controller
{
    /// <summary>
    /// 省市县管理控制器
    /// </summary>

    public class DistrictController : BaseSystemController
    {
        #region 构造函数

        private readonly ISystemDistrictLogic _districtLogic;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="districtLogic"></param>
        public DistrictController(ISystemDistrictLogic districtLogic)
        {
            _districtLogic = districtLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 异步获取省市区县
        /// </summary>
        /// <param name="input">父级</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("省市县维护-方法-列表-根据父级查询所有子集", RemarkFrom.System)]
        [Route("/system/district/sync/{id}")]
        public async  Task<ActionResult> FindSync([FromRoute] IdInput<string> input)
        {
            return Ok(await _districtLogic.FindSync(input));
        }

        /// <summary>
        /// 根据父级查询所有子集
        /// </summary>
        /// <param name="input">父级</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("省市县维护-方法-列表-根据父级查询所有子集", RemarkFrom.System)]
        [Route("/system/district/list/{id}")]
        public async  Task<ActionResult> FindByParentId([FromRoute] IdInput<string> input)
        {
            return Ok(await _districtLogic.FindByParentId(input));
        }

        /// <summary>
        /// 根据县Id获取省市县Id
        /// </summary>
        /// <param name="input">县Id</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("省市县维护-方法-列表-根据Id获取", RemarkFrom.System)]
        [Route("/system/district/{id}")]
        public async  Task<ActionResult> FindById([FromRoute] IdInput<string> input)
        {
            SystemDistrictFindByIdOutput output = new SystemDistrictFindByIdOutput();
            var dis = await _districtLogic.FindByIdAsync(input.Id);
            if (dis == null) return Ok(output);
            var parentDis = await _districtLogic.FindByIdAsync(dis.ParentId);
            if (parentDis != null)
            {
                output = dis.MapTo<SystemDistrict,SystemDistrictFindByIdOutput >();
                output.ParentName = parentDis.Name;
            }
            return Ok(output);
        }
        /// <summary>
        /// 根据县Id获取省市县Id
        /// </summary>
        /// <param name="input">县Id</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("省市县维护-方法-列表-根据县Id获取省市县Id", RemarkFrom.System)]
        [Route("/system/district/count/{id}")]
        public async  Task<ActionResult> FindByCountId([FromRoute] IdInput<string> input)
        {
            return Ok(input.Id.IsNullOrEmpty()
                ? new SystemDistrictFindDistrictByCountIdOutout()
                : await _districtLogic.FindByCountId(input));
        }

        /// <summary>
        /// 保存省市县信息
        /// </summary>
        /// <param name="district">省市县信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("省市县维护-方法-新增/编辑-保存省市县信息", RemarkFrom.System)]
        [Route("/system/district/save")]
        public async  Task<ActionResult> Save(SystemDistrict district)
        {
            return Ok(await _districtLogic.Save(district));
        }

        /// <summary>
        /// 检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("省市县维护-方法-新增/编辑-检测代码是否已经具有重复项", RemarkFrom.System)]
        [Route("/system/district/check")]
        public async  Task<ActionResult> Check(CheckSameValueInput input)
        {
            return JsonForCheckSameValue(await _districtLogic.Check(input));
        }

        /// <summary>
        /// 删除省市县及下级数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("省市县维护-方法-列表-删除省市县及下级数据", RemarkFrom.System)]
        [Route("/system/district/delete")]
        public async  Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _districtLogic.Delete(input));
        }

        #endregion
    }
}