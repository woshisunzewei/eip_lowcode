/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
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
using EIP.Common.Core.Auth;
using EIP.Common.Core.Context;
using EIP.Common.Core.Resource;
using EIP.Common.Core.Util;
using EIP.Common.Models;
using EIP.Common.Models.Paging;
using EIP.Common.Models.Tree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EIP.Common.Controller
{
    /// <summary>
    /// 继承BaseController都必须要继续授权验证
    /// </summary>
    [Route("api/[controller]/[action]")]
    [Authorize("permission")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly PrincipalUser CurrentUser;
        /// <summary>
        /// 用户构造函数
        /// </summary>
        public BaseController()
        {
            CurrentUser = EipHttpContext.CurrentUser();
        }
        #region Json

        /// <summary>
        /// 返回分页后信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pagedResults"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected ActionResult JsonForGridPaging<T>(OperateStatus<PagedResults<T>> pagedResults)
        {
            return Ok(new
            {
                code = pagedResults.Code,
                pagedResults.Msg,
                total = pagedResults.Data.PagerInfo?.RecordCount ?? 0,
                data = pagedResults.Data.Data
            });
        }

        /// <summary>
        /// 返回一次性数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected ActionResult JsonForGridLoadOnce<T>(IEnumerable<T> datas, string msg = "")
        {
            var enumerable = datas as IList<T> ?? datas.ToList();
            return Ok(new
            {
                code = 200,
                msg,
                total = enumerable.ToList().Count,
                data = enumerable
            });
        }

        /// <summary>
        ///  返回数据转换为JsTree
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        protected ActionResult JsonForTree(IEnumerable<BaseTree> datas)
        {
            List<TreeEntity> output = new List<TreeEntity>();
            datas = datas.ToList();
            foreach (var data in datas)
            {
                if (data.parent == null)
                {
                    data.parent = Guid.Empty.ToString();
                }
                else
                {
                    //判断有无父级
                    var ds = datas.Where(w => w.id.ToString() == data.parent.ToString()).ToList();
                    if (!ds.Any())
                    {
                        data.parent = Guid.Empty.ToString();
                    }
                }
            }

            foreach (var data in datas.Where(w => w.parent.ToString() == Guid.Empty.ToString()))
            {
                TreeEntity treeEntity = new TreeEntity
                {
                    Value = data.id,
                    DisableCheckbox = data.disableCheckbox,
                    Key = data.id,
                    Title = data.text,
                    Disabled = data.disabled,
                    Slots = new TreeEntitySlots
                    {
                        Icon = string.IsNullOrEmpty(data.icon) ? "hdd" : data.icon,
                        Theme = string.IsNullOrEmpty(data.theme) ? "outlined" : data.theme,
                        Extend = data.extend,
                    }
                };
                treeEntity.Children = FindJsonForTreeChildren(datas.ToList(), data);
                if (!treeEntity.Children.Any())
                {
                    treeEntity.IsLeaf = true;
                }
                output.Add(treeEntity);
            }
            OperateStatus<List<TreeEntity>> operateStatus = new OperateStatus<List<TreeEntity>>();
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            operateStatus.Data = output;
            return Ok(operateStatus);
        }

        /// <summary>
        /// 递归获取树形结构
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="treeEntity"></param>
        private List<TreeEntity> FindJsonForTreeChildren(List<BaseTree> datas, BaseTree treeEntity)
        {
            List<TreeEntity> treeEntities = new List<TreeEntity>();
            var children = datas.Where(w => w.parent.ToString() == treeEntity.id.ToString());
            foreach (var item in children)
            {
                TreeEntity treeEntityChidren = new TreeEntity
                {
                    Value = item.id,
                    DisableCheckbox = item.disableCheckbox,
                    Key = item.id,
                    Title = item.text,
                    Slots = new TreeEntitySlots
                    {
                        Icon = string.IsNullOrEmpty(item.icon) ? "hdd" : item.icon,
                        Theme = string.IsNullOrEmpty(item.theme) ? "outlined" : item.theme,
                        Extend = item.extend,
                    }
                };
                treeEntityChidren.Children = FindJsonForTreeChildren(datas, item);
                if (!treeEntityChidren.Children.Any())
                {
                    treeEntityChidren.IsLeaf = true;
                }
                treeEntities.Add(treeEntityChidren);
            }
            return treeEntities;
        }

        /// <summary>
        /// 检查值是否相同
        /// </summary>
        /// <param name="operateStatus"></param>
        /// <returns></returns>
        protected ActionResult JsonForCheckSameValue(OperateStatus operateStatus)
        {
            return Ok(new
            {
                info = operateStatus.Msg,
                status = operateStatus.Code == ResultCode.Success ? "y" : "n"
            });
        }

        #endregion

        #region 导入导出
        /// <summary>
        /// 生成模版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        protected FileResult GenerateTemplate<T>() where T : class, new()
        {
            var path = ConfigurationUtil.GetTempPath();
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = System.IO.Path.Combine(path, nameof(T) + ".xlsx");
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            var value = new List<T>();
            MiniExcel.SaveAs(path, value);
            var resultFile = File(FileUtil.ReadFileStream(path), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            FileUtil.DeleteFile(path);
            return resultFile;
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        protected OperateStatus<IList<T>> Import<T>() where T : class, new()
        {
            var result = new OperateStatus<IList<T>>()
            {
                Data = new List<T>()
            };
            var files = Request.Form.Files;
            if (files.Any())
            {
                var file = files[0];
                var uploadPath = ConfigurationUtil.GetTempPath();
                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
                string filepath = System.IO.Path.Combine(uploadPath, file.FileName);
                if (System.IO.File.Exists(filepath)) System.IO.File.Delete(filepath);
                using (FileStream fs = System.IO.File.Create(filepath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                result.Data = MiniExcel.Query<T>(filepath).ToList();
                FileUtil.DeleteFile(filepath);
            }
            result.Code = ResultCode.Success;
            result.Msg = Chs.Successful;

            return result;
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        protected FileResult Export<T>(ICollection<T> datas) where T : class, new()
        {
            var path = ConfigurationUtil.GetTempPath();
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = System.IO.Path.Combine(path, nameof(T) + ".xlsx");
            MiniExcel.SaveAs(path, datas);
            var resultFile = File(FileUtil.ReadFileStream(path), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            FileUtil.DeleteFile(path);
            return resultFile;
        }
        #endregion
    }
}