/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/2/3 22:14:39
* 文件名: SystemAgileAutomationFindDto
* 描述: 自动化构建
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.AgileAutomation
{
    /// <summary>
    /// 自动化构建发布Json参数
    /// </summary>
    public class SystemAgileAutomationJsonOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SystemAgileAutomationJsonDataOutput Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SystemAgileAutomationJsonOutput> ConditionNodes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SystemAgileAutomationJsonOutput ChildNode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileAutomationJsonDataOutput
    {
        /// <summary>
        /// 条件
        /// </summary>
        public Filter ConditionFilters { get; set; }

        #region 表单触发节点
        /// <summary>
        /// 
        /// </summary>
        public Guid? Table { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TriggerType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> TriggerColumns { get; set; }


        #endregion

        #region 站内通知
        /// <summary>
        /// 通知类型
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SystemAgileAutomationJsonDataRangeOutput> Range { get; set; }
        #endregion

        #region 自定义请求
        /// <summary>
        /// 通知类型
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public string Headers { get; set; }
        /// <summary>
        /// 请求超时或请求失败时 1继续执行，2中止流程
        /// </summary>
        public int ErrorType { get; set; }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取类型1，工作表，2自定义请求
        /// </summary>
        public new int GetType { get; set; }

        /// <summary>
        /// 工作表id
        /// </summary>
        public Guid? GetTypeTable { get; set; }

        /// <summary>
        /// 自定义请求里面数据
        /// </summary>
        public Guid? GetCustomerRequest { get; set; }

        /// <summary>
        /// 排序规则
        /// </summary>
        public int SortRules { get; set; }

        /// <summary>
        /// 排序项
        /// </summary>
        public string SortRulesSelect { get; set; }
        #endregion

        #region 新增数据
        /// <summary>
        /// 添加类型
        /// </summary>
        public int AddType { get; set; }

        /// <summary>
        /// 添加多数据
        /// </summary>
        public Guid? AddMultipleData { get; set; }

        /// <summary>
        /// 添加字段
        /// </summary>
        public List<SystemAgileAutomationJsonAddDataOutput> AddData { get; set; } = new List<SystemAgileAutomationJsonAddDataOutput>();
        #endregion

        #region 更新数据
        /// <summary>
        /// 更新数据
        /// </summary>
        public Guid? UpdateObj { get; set; }

        /// <summary>
        /// 更新数据源
        /// </summary>
        public List<SystemAgileAutomationJsonUpdateDataOutput> UpdateData { get; set; } = new List<SystemAgileAutomationJsonUpdateDataOutput>();

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileAutomationJsonAddDataOutput
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileAutomationJsonUpdateDataOutput
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileAutomationJsonApiTestOutput
    {
        /// <summary>
        /// 数组
        /// </summary>
        public bool Array { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileAutomationJsonDataRangeOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 解析内容配置
    /// </summary>
    public class SystemAgileAutomationJsonDataContentSettingOutput
    {
        /// <summary>
        /// 组件Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 字段
        /// </summary>
        public string Model { get; set; }
    }

    /// <summary>
    /// 所有已执行过组件对应的id和数据
    /// </summary>
    public class SystemAgileAutomationJsonDataAllOutput
    {
        /// <summary>
        /// 组件Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public dynamic Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SystemAgileAutomationJsonOutput Json { get; set; }
    }
}