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
using EIP.Base.Models.Entities.System;
using EIP.Base.Models.Entities.WeChat;
using EIP.Base.Models.Entities.Workflow;
using EIP.Common.Repository.MicroOrm;
using EIP.Common.Repository.MicroOrm.DbContext;

namespace EIP.Base.Repository.Fixture
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IDbContext : IDapperDbContext
    {
        #region 系统
        /// <summary>
        /// 
        /// </summary>
        IDapperRepository<SystemSearch> SystemSearch { get; }

        /// <summary>
        /// 自动化构建
        /// </summary>
        IDapperRepository<SystemAgileAutomation> SystemAgileAutomation { get; }

        /// <summary>
        /// 数据库维护
        /// </summary>
        IDapperRepository<SystemDataBase> SystemDataBase { get; }

        /// <summary>
        /// 短信维护
        /// </summary>
        IDapperRepository<SystemSmstemplate> SystemSmstemplate { get; }
        /// <summary>
        /// 短信配置
        /// </summary>
        IDapperRepository<SystemSmsconfig> SystemSmsconfig { get; }
        /// <summary>
        /// 系统配置
        /// </summary>
        IDapperRepository<SystemConfig> SystemConfig { get; }

        /// <summary>
        /// 数据权限规则定义表
        /// </summary>
        IDapperRepository<SystemData> SystemData { get; }

        /// <summary>
        /// 字典信息表
        /// </summary>
        IDapperRepository<SystemDictionary> SystemDictionary { get; }
        /// <summary>
        /// 省市区县
        /// </summary>
        IDapperRepository<SystemDistrict> SystemDistrict { get; }
        /// <summary>
        /// 异常日志记录
        /// </summary>
        IDapperRepository<SystemExceptionLog> SystemExceptionLog { get; }

        /// <summary>
        /// 组信息维护
        /// </summary>
        IDapperRepository<SystemGroup> SystemGroup { get; }
        /// <summary>
        /// 登录日志记录表
        /// </summary>
        IDapperRepository<SystemLoginLog> SystemLoginLog { get; }
        /// <summary>
        /// 系统菜单
        /// </summary>
        IDapperRepository<SystemMenu> SystemMenu { get; }

        /// <summary>
        /// 菜单按钮记录表
        /// </summary>
        IDapperRepository<SystemMenuButton> SystemMenuButton { get; }

        /// <summary>
        /// 操作日志记录
        /// </summary>
        IDapperRepository<SystemOperationLog> SystemOperationLog { get; }
        /// <summary>
        /// 组织机构信息表
        /// </summary>
        IDapperRepository<SystemOrganization> SystemOrganization { get; }

        /// <summary>
        /// 权限记录表
        /// </summary>
        IDapperRepository<SystemPermission> SystemPermission { get; }
        /// <summary>
        /// 权限用户记录表:组织机构、角色、岗位、组下的人员
        /// </summary>
        IDapperRepository<SystemPermissionUser> SystemPermissionUser { get; }
        /// <summary>
        /// 岗位信息记录表
        /// </summary>
        IDapperRepository<SystemPost> SystemPost { get; }
        /// <summary>
        /// 文件存储表
        /// </summary>
        IDapperRepository<SystemFile> SystemFile { get; }
        /// <summary>
        /// 系统角色记录表
        /// </summary>
        IDapperRepository<SystemRole> SystemRole { get; }

        /// <summary>
        /// 系统使用人员
        /// </summary>
        IDapperRepository<SystemUserInfo> SystemUserInfo { get; }

        /// <summary>
        /// 修改密码
        /// </summary>
        IDapperRepository<SystemChangePassword> SystemChangePassword { get; }

        /// <summary>
        /// 
        /// </summary>
        IDapperRepository<SystemUserLeader> SystemUserLeader { get; }

        /// <summary>
        /// 素材
        /// </summary>
        IDapperRepository<SystemMaterial> SystemMaterial { get; }

        /// <summary>
        /// 敏捷开发
        /// </summary>
        IDapperRepository<SystemAgile> SystemAgile { get; }

        /// <summary>
        /// 敏捷开发
        /// </summary>
        IDapperRepository<SystemAgileDataLog> SystemAgileDataLog { get; }

        /// <summary>
        /// 编号
        /// </summary>
        IDapperRepository<SystemSerialNo> SystemSerialNo { get; }

        /// <summary>
        /// 用户占用编号
        /// </summary>
        IDapperRepository<SystemSerialNoUser> SystemSerialNoUser { get; }

        /// <summary>
        /// 
        /// </summary>
        IDapperRepository<SystemDataSource> SystemDataSource { get; }

        /// <summary>
        /// 
        /// </summary>
        IDapperRepository<SystemOrganizationThree> SystemOrganizationThree { get; }

        /// <summary>
        /// 
        /// </summary>
        IDapperRepository<SystemUserInfoThree> SystemUserInfoThree { get; }
        #endregion

        #region 工作流

        /// <summary>
        /// 工作流按钮信息
        /// </summary>
        IDapperRepository<WorkflowButton> WorkflowButton { get; }

        /// <summary>
        /// 
        /// </summary>
        IDapperRepository<WorkflowPermission> WorkflowPermission { get; }

        /// <summary>
        /// 工作流定义表:流程设计器设计后保存此表
        /// </summary>
        IDapperRepository<WorkflowProcess> WorkflowProcess { get; }

        /// <summary>
        /// 工作流活动表
        /// </summary>
        IDapperRepository<WorkflowProcessActivity> WorkflowProcessActivity { get; }
        /// <summary>
        /// 流程实例:所有发送流程主表
        /// </summary>
        IDapperRepository<WorkflowProcessInstance> WorkflowProcessInstance { get; }
        /// <summary>
        /// 工作流运行活动表
        /// </summary>
        IDapperRepository<WorkflowProcessInstanceActivity> WorkflowProcessInstanceActivity { get; }
       
        /// <summary>
        /// 流程实例连线
        /// </summary>
        IDapperRepository<WorkflowProcessInstanceLink> WorkflowProcessInstanceLink { get; }
      
        /// <summary>
        /// 流程实例任务表
        /// </summary>
        IDapperRepository<WorkflowProcessInstanceTask> WorkflowProcessInstanceTask { get; }
        /// <summary>
        /// 流程实例连线
        /// </summary>
        IDapperRepository<WorkflowProcessLink> WorkflowProcessLink { get; }

        /// <summary>
        /// 业务归档表
        /// </summary>
        IDapperRepository<WorkflowArchive> WorkflowArchive { get; }
        #endregion

        #region 微信


        /// <summary>
        /// 
        /// </summary>
        IDapperRepository<WeChatMpTemplate> WeChatMpTemplate { get; }

        /// <summary>
        /// 
        /// </summary>
        IDapperRepository<WeChatMpTemplateSend> WeChatMpTemplateSend { get; }

        /// <summary>
        /// 
        /// </summary>
        IDapperRepository<WeChatMpTemplateSendUser> WeChatMpTemplateSendUser { get; }

        #endregion

    }
}