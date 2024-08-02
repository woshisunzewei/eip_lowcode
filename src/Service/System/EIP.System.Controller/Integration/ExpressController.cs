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
using EIP.Common.Config;
using EIP.Common.Controller.Attribute;
using EIP.Common.Core.Extension;
using EIP.Common.Core.Util;
using EIP.Common.Models;
using EIP.System.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EIP.System.Controller
{
    /// <summary>
    /// 快递相关
    /// </summary>

    public class ExpressController : BaseSystemController
    {
        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        public ExpressController()
        {

        }
        #endregion

        #region 快递物流
        /// <summary>
        /// 查询快递公司列表
        /// </summary>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Remark("物流快递-方法-查询快递公司列表", RemarkFrom.System)]
        [HttpPost]
        [Route("/integration/express/company")]
        public async Task<ActionResult> GetCompany()
        {
            OperateStatus<object> operateStatus = new OperateStatus<object>();
            try
            {
                //https://www.showapi.com/apiGateway/view/64/20#tabs
                var globalParams = GlobalParams.GetValuesByName();
                var showApiAppid = globalParams.FirstOrDefault(f => f.Key == EnumConfigKey.expressAppId.ToString()).Value;
                var showApiSecret = globalParams.FirstOrDefault(f => f.Key == EnumConfigKey.expressSecret.ToString()).Value;
                var showApiTimesTamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var maxSize = 1500;
                var signStr = "maxSize" + maxSize + "showapi_appid" + showApiAppid + "showapi_timestamp" + showApiTimesTamp + showApiSecret;
                var md5Sign = signStr.Md5().ToLower();
                var url = "https://route.showapi.com/64-20?expName=&maxSize=1500&page=&showapi_appid=" + showApiAppid +
                          "&showapi_timestamp=" + showApiTimesTamp + "&showapi_sign=" + md5Sign;
                var result = await RequestUtil.Post(url, null);
                var data = result.JsonStringToObject<ExpressCompanyOutput>();
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = "查询成功";
                operateStatus.Data = data.showapi_res_body.expressList;
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return Ok(operateStatus);
        }

        /// <summary>
        /// 查询物流信息
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [CreateBy("孙泽伟")]
        [Remark("物流快递-方法-查询物流信息", RemarkFrom.System)]
        [HttpPost]
        [Route("/integration/express/trajectory")]
        public async Task<ActionResult> GetTrajectory(ExpressTrajectoryInput input)
        {
            OperateStatus<object> operateStatus = new OperateStatus<object>();
            try
            {
                if (input.phone.Length != 11)
                {
                    operateStatus.Msg = "手机号格式不正确";
                    return Ok(operateStatus);
                }
                //https://www.showapi.com/apiGateway/view/64/20#tabs
                var globalParams = GlobalParams.GetValuesByName();
                var showApiAppid = globalParams.FirstOrDefault(f => f.Key == EnumConfigKey.expressAppId.ToString()).Value;
                var showApiSecret = globalParams.FirstOrDefault(f => f.Key == EnumConfigKey.expressSecret.ToString()).Value;
                var showApiTimesTamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                input.phone= input.phone.Length >= 4 ? input.phone.Substring(input.phone.Length - 4, 4) : input.phone;
                var signStr = "com" + input.com + "nu" + input.nu + "phone" + input.phone + "showapi_appid" + showApiAppid + "showapi_timestamp" + showApiTimesTamp + showApiSecret;
                var md5Sign = signStr.Md5().ToLower();

                var url = "https://route.showapi.com/64-34?com=" + input.com + "&nu=" + input.nu + "&phone=" + input.phone + "&showapi_appid=" + showApiAppid +
                          "&showapi_timestamp=" + showApiTimesTamp + "&showapi_sign=" + md5Sign;
                var result = await RequestUtil.Post(url, null);
                var data = result.JsonStringToObject<ExpressTrajectoryOutput>();
                if (data.showapi_res_code != 0)
                {
                    operateStatus.Msg = data.showapi_res_error;
                    return Ok(operateStatus);
                }
                else
                {
                    switch (data.showapi_res_body.ret_code)
                    {
                        case 0:
                            operateStatus.Code = ResultCode.Success;
                            operateStatus.Msg = "查询成功";
                            operateStatus.Data = data.showapi_res_body;
                            break;
                        case 1:
                            operateStatus.Code = ResultCode.Error;
                            operateStatus.Msg = "输入参数错误";
                            operateStatus.Data = data.showapi_res_body;
                            break;
                        case 2:
                            operateStatus.Code = ResultCode.Error;
                            operateStatus.Msg = "查不到物流信息";
                            operateStatus.Data = data.showapi_res_body;
                            break;
                        case 3:
                            operateStatus.Code = ResultCode.Error;
                            operateStatus.Msg = "单号不符合规则";
                            operateStatus.Data = data.showapi_res_body;
                            break;
                        case 4:
                            operateStatus.Code = ResultCode.Error;
                            operateStatus.Msg = "快递公司编码不符合规则";
                            operateStatus.Data = data.showapi_res_body;
                            break;
                        case 5:
                            operateStatus.Code = ResultCode.Error;
                            operateStatus.Msg = "快递查询渠道异常";
                            operateStatus.Data = data.showapi_res_body;
                            break;
                        case 6:
                            operateStatus.Code = ResultCode.Error;
                            operateStatus.Msg = " auto时未查到单号对应的快递公司,请指定快递公司编码";
                            operateStatus.Data = data.showapi_res_body;
                            break;
                        case 7:
                            operateStatus.Code = ResultCode.Error;
                            operateStatus.Msg = "单号与手机号不匹配";
                            operateStatus.Data = data.showapi_res_body;
                            break;
                        default:
                            operateStatus.Code = ResultCode.Error;
                            operateStatus.Msg = "接口调用失败";
                            operateStatus.Data = data.showapi_res_body;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return Ok(operateStatus);
        }
        #endregion
    }
    #region 快递公司
    /// <summary>
    /// 
    /// </summary>
    public class ExpressCompanyOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public int showapi_res_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string showapi_res_error { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ExpressCompanyBodyOutput showapi_res_body { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExpressCompanyBodyOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public object expressList { get; set; }
    }
    #endregion

    #region 历史轨迹
    /// <summary>
    /// 
    /// </summary>
    public class ExpressTrajectoryOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public int showapi_res_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string showapi_res_error { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ExpressTrajectoryBodyOutput showapi_res_body { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExpressTrajectoryBodyOutput
    {
        /// <summary>
        /// 更新时间戳
        /// </summary>
        public string updateStr { get; set; }

        /// <summary>
        /// 提示信息，用于提醒用户可能出现的情况
        /// </summary>
        public string upgrade_info { get; set; }

        /// <summary>
        /// 快递状态
        ///1 暂无记录
        ///2 在途中
        ///3 派送中
        ///4 已签收(完结状态)
        ///5 用户拒签
        ///6 疑难件
        ///7 无效单(完结状态)
        ///8 超时单
        ///9 签收失败
        ///10 退回
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ExpressTrajectoryBodyDataOutput> data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        ///0查询成功 或 提交成功。
        ///1输入参数错误。
        ///2查不到物流信息。
        ///3单号不符合规则。
        ///4快递公司编码不符合规则。
        ///5快递查询渠道异常。
        ///6auto时未查到单号对应的快递公司,请指定快递公司编码。
        ///7单号与手机号不匹配
        ///其他参数：接口调用失败
        /// </summary>
        public int ret_code { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ExpressTrajectoryBodyDataOutput
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string context { get; set; }
    }
        /// <summary>
        /// 
        /// </summary>
        public class ExpressTrajectoryInput
    {
        /// <summary>
        /// 快递公司字母简称,可以从"快递公司列表"或"快递公司查询" 接口中查到该信息  如不知道快递公司名,可以使用"auto"代替,此时将自动识别快递单号 查询顺丰时，为了保证效率，请尽量提供寄件人或者收件人查询】    
        /// </summary>
        public string com { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string nu { get; set; }

        /// <summary>
        /// 手机尾号后四位，顺丰快递必须填写本字段【寄件人手机号或者收件人手机号】
        /// </summary>
        public string phone { get; set; }
    }
    #endregion

}