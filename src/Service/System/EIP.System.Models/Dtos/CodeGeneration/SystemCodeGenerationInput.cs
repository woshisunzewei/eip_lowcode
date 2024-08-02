/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
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
using EIP.System.Models.Dtos.DataBase;
/// <summary>
/// 基础信息
/// </summary>
public class SystemCodeGenerationBaseInput : SystemDataBaseTableDoubleWay
{
    /// <summary>
    /// 实体类名
    /// </summary>
    public string Entity { get; set; }

    /// <summary>
    /// 控制器
    /// </summary>
    public string Controller { get; set; }

    /// <summary>
    /// Logic接口
    /// </summary>
    public string Logic { get; set; }

    /// <summary>
    /// Logic实现
    /// </summary>
    public string LogicImpl { get; set; }

    /// <summary>
    /// Repository接口
    /// </summary>
    public string Repository { get; set; }

    /// <summary>
    /// Repository实现
    /// </summary>
    public string RepositoryImpl { get; set; }

    /// <summary>
    /// 控制器
    /// </summary>
    public string ControllerPath { get; set; }

    /// <summary>
    /// DataAccess接口路径
    /// </summary>
    public string LogicPath { get; set; }

    /// <summary>
    /// Business接口路径
    /// </summary>
    public string RepositoryPath { get; set; }

    /// <summary>
    /// 实体路径
    /// </summary>
    public string EntityPath { get; set; }

}