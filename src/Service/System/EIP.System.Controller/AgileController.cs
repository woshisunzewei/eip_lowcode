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

namespace EIP.System.Controller
{
    /// <summary>
    /// 敏捷开发
    /// </summary>
    public class AgileController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemAgileLogic _agileConfigLogic;
        /// <summary>
        /// 
        /// </summary>
        public AgileController(
            ISystemAgileLogic agileConfigLogic
            )
        {
            _agileConfigLogic = agileConfigLogic;
        }

        #endregion

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-根据父级查询所有子集", RemarkFrom.System)]
        [Route("/system/agile/config/list")]
        public async Task<ActionResult> Find(SystemAgileFindInput paging)
        {
            return JsonForGridPaging(await _agileConfigLogic.Find(paging));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("界面按钮-方法-冻结", RemarkFrom.System)]
        [Route("/system/agile/config/isfreeze")]
        public async Task<ActionResult> IsFreeze(IdInput input)
        {
            return Ok(await _agileConfigLogic.IsFreeze(input));
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-编辑-根据Id获取", RemarkFrom.System)]
        [Route("/system/agile/config/{id}")]
        public async Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _agileConfigLogic.FindById(input));
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-编辑-根据Id获取", RemarkFrom.System)]
        [Route("/system/agile/config/menu")]
        public async Task<ActionResult> FindByMenuId(SystemAgileFindByMenuIdInput input)
        {
            return Ok(await _agileConfigLogic.FindByMenuId(input));
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-根据类型查询表单配置", RemarkFrom.System)]
        [Route("/system/agile/config/base")]
        public async Task<ActionResult> FindBase(SystemAgileFindBaseInput paging)
        {
            return Ok(await _agileConfigLogic.FindBase(paging));
        }

        /// <summary>
        /// 根据表名获取配置
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-根据表名获取配置", RemarkFrom.System)]
        [Route("/system/agile/config/columnjson/datafromname")]
        public async Task<ActionResult> FindColumnJsonByDataFromName(SystemAgileFindColumnJsonDataFromNameInput input)
        {
            return Ok(await _agileConfigLogic.FindColumnJsonByDataFromName(input));
        }

        /// <summary>
        /// 根据MenuId获取Colunm
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-根据表名获取配置", RemarkFrom.System)]
        [Route("/system/agile/config/columnjson/menuid")]
        public async Task<ActionResult> FindColumnJsonByMenuId(SystemAgileFindColumnJsonByMenuIdInput input)
        {
            return Ok(await _agileConfigLogic.FindColumnJsonByMenuId(input));
        }

        /// <summary>
        /// 获取表单字段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-获取表单字段", RemarkFrom.System)]
        [Route("/system/agile/config/columns/{id}")]
        public async Task<ActionResult> FindFormColumns([FromRoute] IdInput input)
        {
            return Ok(await _agileConfigLogic.FindFormColumns(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-编辑-保存", RemarkFrom.System)]
        [Route("/system/agile/config")]
        public async Task<ActionResult> Save(SystemAgile input)
        {
            return Ok(await _agileConfigLogic.Save(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-编辑-保存", RemarkFrom.System)]
        [Route("/system/agile/config/type")]
        public async Task<ActionResult> SaveType(SystemAgile input)
        {
            return Ok(await _agileConfigLogic.SaveType(input));
        }

        /// <summary>
        /// 保存设计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-保存设计信息", RemarkFrom.Workflow)]
        [Route("/system/agile/config/save/json")]
        public async Task<ActionResult> SaveJson(SystemAgile input)
        {
            return Ok(await _agileConfigLogic.SaveJson(input));
        }

        /// <summary>
        /// 发布设计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-发布设计信息", RemarkFrom.Workflow)]
        [Route("/system/agile/config/public/json")]
        public async Task<ActionResult> Public(SystemAgile input)
        {
            return Ok(await _agileConfigLogic.PublicJson(input));
        }

        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-保存缩略图", RemarkFrom.Workflow)]
        [Route("/system/agile/config/save/thumbnail")]
        public async Task<ActionResult> SaveThumbnail(SystemAgile input)
        {
            return Ok(await _agileConfigLogic.SaveThumbnail(input));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键集合</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/agile/config/delete")]
        public async Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _agileConfigLogic.Delete(input));
        }
        /// <summary>
        /// 根据关键词查询相关发布设计内容
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-根据关键词查询相关发布设计内容", RemarkFrom.Workflow)]
        [Route("/system/agile/config/key")]
        public async Task<ActionResult> Key(SystemAgileFindByKeyInput input)
        {
            return Ok(await _agileConfigLogic.Key(input));
        }

        /// <summary>
        /// 根据Api处理事件
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("事件处理-方法-根据Api处理事件", RemarkFrom.System, true)]
        [Route("/system/agile/event/api")]
        public async Task<object> EventDoByApi(SystemAgileDoByApiInput input)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", Request.Headers["Authorization"]);
            input.Header = "Authorization:" + Request.Headers["Authorization"];
            switch (input.Type.ToLower())
            {
                case "post":
                    return await RequestUtil.HttpPost(input.Url, input.Param, headers, input.ContentType);
                default:
                    return await RequestUtil.HttpGet(input.Url, input.Param, headers, input.ContentType);
            }
        }
    }
}