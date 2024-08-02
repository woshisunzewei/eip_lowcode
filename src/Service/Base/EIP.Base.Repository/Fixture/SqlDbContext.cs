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
using EIP.Common.Core.Util;
using EIP.Common.Models.Resx;
using EIP.Common.Repository.MicroOrm;
using EIP.Common.Repository.MicroOrm.DbContext;
using EIP.Common.Repository.MicroOrm.SqlGenerator;
using EIP.System.Models.Dtos.District;
using EIP.System.Models.Dtos.MenuButton;
using EIP.System.Models.Dtos.Permission;
using EIP.System.Models.Dtos.User;
using System.Data;

namespace EIP.Base.Repository.Fixture
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SqlDbContext : DapperDbContext, IDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        public SqlDbContext(IDbConnection dbConnection)
            : base(dbConnection) { }

        /// <summary>
        /// 得到驱动类型
        /// </summary>
        /// <returns></returns>
        private static SqlProvider GetSqlProvider()
        {
            switch (ConfigurationUtil.GetDbConnectionType())
            {
                case ResourceDataBaseType.Mysql:
                    return SqlProvider.MySQL;
                case ResourceDataBaseType.Postgresql:
                    return SqlProvider.PostgreSQL;
                case ResourceDataBaseType.Dm:
                    return SqlProvider.Dm;
                case ResourceDataBaseType.Kingbase:
                    return SqlProvider.Kingbase;
                default:
                    return SqlProvider.MSSQL;
            }
        }

        #region 系统

        #region 查询
        /// <summary>
        /// 
        /// </summary>
        private IDapperRepository<SystemSearch> _systemSearch;
        /// <summary>
        /// 
        /// </summary>
        public IDapperRepository<SystemSearch> SystemSearch => _systemSearch ??= new DapperRepository<SystemSearch>(Connection, new SqlGenerator<SystemSearch>(GetSqlProvider()));

        #endregion

        #region 数据库维护
        /// <summary>
        /// 数据库维护
        /// </summary>
        private IDapperRepository<SystemDataBase> _systemDataBase;
        /// <summary>
        /// 数据库维护
        /// </summary>
        public IDapperRepository<SystemDataBase> SystemDataBase => _systemDataBase ??= new DapperRepository<SystemDataBase>(Connection, new SqlGenerator<SystemDataBase>(GetSqlProvider()));

        #endregion

        #region 短信维护
        /// <summary>
        /// 短信配置
        /// </summary>
        private IDapperRepository<SystemSmsconfig> _systemSmsconfig;
        /// <summary>
        /// 短信配置
        /// </summary>
        public IDapperRepository<SystemSmsconfig> SystemSmsconfig => _systemSmsconfig ??= new DapperRepository<SystemSmsconfig>(Connection, new SqlGenerator<SystemSmsconfig>(GetSqlProvider()));

        /// <summary>
        /// 短信维护
        /// </summary>
        private IDapperRepository<SystemSmstemplate> _systemSmstemplate;
        /// <summary>
        /// 短信维护
        /// </summary>
        public IDapperRepository<SystemSmstemplate> SystemSmstemplate => _systemSmstemplate ??= new DapperRepository<SystemSmstemplate>(Connection, new SqlGenerator<SystemSmstemplate>(GetSqlProvider()));
        #endregion

        #region 系统配置
        private IDapperRepository<SystemConfig> _systemConfig;
        /// <summary>
        /// 系统配置
        /// </summary>
        public IDapperRepository<SystemConfig> SystemConfig => _systemConfig ??= new DapperRepository<SystemConfig>(Connection, new SqlGenerator<SystemConfig>(GetSqlProvider()));

        #endregion

        #region 数据权限规则定义表
        private IDapperRepository<SystemData> _systemData;
        /// <summary>
        /// 数据权限规则定义表
        /// </summary>
        public IDapperRepository<SystemData> SystemData => _systemData ??= new DapperRepository<SystemData>(Connection, new SqlGenerator<SystemData>(GetSqlProvider()));

        #endregion

        #region 字典信息表
        private IDapperRepository<SystemDictionary> _systemDictionary;
        /// <summary>
        /// 字典信息表
        /// </summary>
        public IDapperRepository<SystemDictionary> SystemDictionary => _systemDictionary ??= new DapperRepository<SystemDictionary>(Connection, new SqlGenerator<SystemDictionary>(GetSqlProvider()));
        #endregion

        #region 省市区县
        private IDapperRepository<SystemDistrict> _systemDistrict;
        /// <summary>
        /// 省市区县
        /// </summary>
        public IDapperRepository<SystemDistrict> SystemDistrict => _systemDistrict ??= new DapperRepository<SystemDistrict>(Connection, new SqlGenerator<SystemDistrict>(GetSqlProvider()));
        #endregion

        #region 异常日志记录
        private IDapperRepository<SystemExceptionLog> _systemExceptionLog;
        /// <summary>
        /// 异常日志记录
        /// </summary>
        public IDapperRepository<SystemExceptionLog> SystemExceptionLog => _systemExceptionLog ??= new DapperRepository<SystemExceptionLog>(Connection, new SqlGenerator<SystemExceptionLog>(GetSqlProvider()));
        #endregion

        #region 组信息维护
        private IDapperRepository<SystemGroup> _systemGroup;
        /// <summary>
        /// 组信息维护
        /// </summary>
        public IDapperRepository<SystemGroup> SystemGroup => _systemGroup ??= new DapperRepository<SystemGroup>(Connection, new SqlGenerator<SystemGroup>(GetSqlProvider()));
        #endregion

        #region 登录日志记录表
        private IDapperRepository<SystemLoginLog> _systemLoginLog;
        /// <summary>
        /// 登录日志记录表
        /// </summary>
        public IDapperRepository<SystemLoginLog> SystemLoginLog => _systemLoginLog ??= new DapperRepository<SystemLoginLog>(Connection, new SqlGenerator<SystemLoginLog>(GetSqlProvider()));
        #endregion

        #region 系统菜单
        private IDapperRepository<SystemMenu> _systemMenu;
        /// <summary>
        /// 系统菜单
        /// </summary>
        public IDapperRepository<SystemMenu> SystemMenu => _systemMenu ??= new DapperRepository<SystemMenu>(Connection, new SqlGenerator<SystemMenu>(GetSqlProvider()));
        #endregion

        #region 菜单按钮记录表
        private IDapperRepository<SystemMenuButton> _systemMenuButton;
        /// <summary>
        /// 菜单按钮记录表
        /// </summary>
        public IDapperRepository<SystemMenuButton> SystemMenuButton => _systemMenuButton ??= new DapperRepository<SystemMenuButton>(Connection, new SqlGenerator<SystemMenuButton>(GetSqlProvider()));
        #endregion

        #region 操作日志记录
        private IDapperRepository<SystemOperationLog> _systemOperationLog;
        /// <summary>
        /// 操作日志记录
        /// </summary>
        public IDapperRepository<SystemOperationLog> SystemOperationLog => _systemOperationLog ??= new DapperRepository<SystemOperationLog>(Connection, new SqlGenerator<SystemOperationLog>(GetSqlProvider()));
        #endregion

        #region 组织机构信息表
        private IDapperRepository<SystemOrganization> _systemOrganization;
        /// <summary>
        /// 组织机构信息表
        /// </summary>
        public IDapperRepository<SystemOrganization> SystemOrganization => _systemOrganization ??= new DapperRepository<SystemOrganization>(Connection, new SqlGenerator<SystemOrganization>(GetSqlProvider()));
        #endregion

        #region 权限记录表
        private IDapperRepository<SystemPermission> _systemPermission;
        /// <summary>
        /// 权限记录表
        /// </summary>
        public IDapperRepository<SystemPermission> SystemPermission => _systemPermission ??= new DapperRepository<SystemPermission>(Connection, new SqlGenerator<SystemPermission>(GetSqlProvider()));
        #endregion

        #region 权限用户记录表
        private IDapperRepository<SystemPermissionUser> _systemPermissionUser;
        /// <summary>
        /// 权限用户记录表:组织机构、角色、岗位、组下的人员
        /// </summary>
        public IDapperRepository<SystemPermissionUser> SystemPermissionUser => _systemPermissionUser ??= new DapperRepository<SystemPermissionUser>(Connection, new SqlGenerator<SystemPermissionUser>(GetSqlProvider()));
        #endregion

        #region 岗位信息记录表
        private IDapperRepository<SystemPost> _systemPost;
        /// <summary>
        /// 岗位信息记录表
        /// </summary>
        public IDapperRepository<SystemPost> SystemPost => _systemPost ??= new DapperRepository<SystemPost>(Connection, new SqlGenerator<SystemPost>(GetSqlProvider()));
        #endregion

        #region 系统角色记录表
        private IDapperRepository<SystemRole> _systemRole;
        /// <summary>
        /// 系统角色记录表
        /// </summary>
        public IDapperRepository<SystemRole> SystemRole => _systemRole ??= new DapperRepository<SystemRole>(Connection, new SqlGenerator<SystemRole>(GetSqlProvider()));
        #endregion

        #region 系统使用人员
        private IDapperRepository<SystemUserInfo> _systemUserInfo;
        /// <summary>
        /// 系统使用人员
        /// </summary>
        public IDapperRepository<SystemUserInfo> SystemUserInfo => _systemUserInfo ??= new DapperRepository<SystemUserInfo>(Connection, new SqlGenerator<SystemUserInfo>(GetSqlProvider()));
        #endregion

        #region 上级领导
        private IDapperRepository<SystemUserLeader> _systemUserLeader;
        /// <summary>
        /// 上级领导
        /// </summary>
        public IDapperRepository<SystemUserLeader> SystemUserLeader => _systemUserLeader ??= new DapperRepository<SystemUserLeader>(Connection, new SqlGenerator<SystemUserLeader>(GetSqlProvider()));
        #endregion

        #region 文件存储表
        private IDapperRepository<SystemFile> _systemFile;
        /// <summary>
        /// 文件存储表
        /// </summary>
        public IDapperRepository<SystemFile> SystemFile => _systemFile ??= new DapperRepository<SystemFile>(Connection, new SqlGenerator<SystemFile>(GetSqlProvider()));
        #endregion

        #region 素材
        private IDapperRepository<SystemMaterial> _systemMaterial;
        /// <summary>
        /// 类型
        /// </summary>
        public IDapperRepository<SystemMaterial> SystemMaterial => _systemMaterial ??= new DapperRepository<SystemMaterial>(Connection, new SqlGenerator<SystemMaterial>(GetSqlProvider()));
        #endregion

        #region 修改密码
        private IDapperRepository<SystemChangePassword> _systemChangePassword;
        /// <summary>
        /// 修改密码
        /// </summary>
        public IDapperRepository<SystemChangePassword> SystemChangePassword => _systemChangePassword ??= new DapperRepository<SystemChangePassword>(Connection, new SqlGenerator<SystemChangePassword>(GetSqlProvider()));
        #endregion
        #region 敏捷开发

        /// <summary>
        /// 自动化构建
        /// </summary>
        private IDapperRepository<SystemAgileAutomation> _systemAgileAutomation;
        /// <summary>
        /// 自动化构建
        /// </summary>
        public IDapperRepository<SystemAgileAutomation> SystemAgileAutomation => _systemAgileAutomation ??= new DapperRepository<SystemAgileAutomation>(Connection, new SqlGenerator<SystemAgileAutomation>(GetSqlProvider()));


        private IDapperRepository<SystemAgile> _systemAgile;
        /// <summary>
        /// 
        /// </summary>
        public IDapperRepository<SystemAgile> SystemAgile => _systemAgile ??= new DapperRepository<SystemAgile>(Connection, new SqlGenerator<SystemAgile>(GetSqlProvider()));
        #endregion

        #region 敏捷开发日志
        private IDapperRepository<SystemAgileDataLog> _systemAgileDataLog;
        /// <summary>
        /// 
        /// </summary>
        public IDapperRepository<SystemAgileDataLog> SystemAgileDataLog => _systemAgileDataLog ??= new DapperRepository<SystemAgileDataLog>(Connection, new SqlGenerator<SystemAgileDataLog>(GetSqlProvider()));
        #endregion

        #region 数据源
        private IDapperRepository<SystemDataSource> _systemDataSource;
        /// <summary>
        /// 
        /// </summary>
        public IDapperRepository<SystemDataSource> SystemDataSource => _systemDataSource ??= new DapperRepository<SystemDataSource>(Connection, new SqlGenerator<SystemDataSource>(GetSqlProvider()));
        #endregion

        #region 编号
        /// <summary>
        /// 编号规则
        /// </summary>
        private IDapperRepository<SystemSerialNo> _systemSerialNo;
        /// <summary>
        /// 编号规则
        /// </summary>
        public IDapperRepository<SystemSerialNo> SystemSerialNo => _systemSerialNo ??= new DapperRepository<SystemSerialNo>(Connection, new SqlGenerator<SystemSerialNo>(GetSqlProvider()));

        /// <summary>
        /// 用户占用编号
        /// </summary>
        private IDapperRepository<SystemSerialNoUser> _systemSerialNoUser;
        /// <summary>
        /// 用户占用编号
        /// </summary>
        public IDapperRepository<SystemSerialNoUser> SystemSerialNoUser => _systemSerialNoUser ??= new DapperRepository<SystemSerialNoUser>(Connection, new SqlGenerator<SystemSerialNoUser>(GetSqlProvider()));

        #endregion

        private IDapperRepository<SystemUserInfoThree> _systemUserInfoThree;
        /// <summary>
        /// 
        /// </summary>
        public IDapperRepository<SystemUserInfoThree> SystemUserInfoThree => _systemUserInfoThree ??= new DapperRepository<SystemUserInfoThree>(Connection, new SqlGenerator<SystemUserInfoThree>(GetSqlProvider()));
     
        private IDapperRepository<SystemOrganizationThree> _systemOrganizationThree;
        /// <summary>
        /// 
        /// </summary>
        public IDapperRepository<SystemOrganizationThree> SystemOrganizationThree => _systemOrganizationThree ??= new DapperRepository<SystemOrganizationThree>(Connection, new SqlGenerator<SystemOrganizationThree>(GetSqlProvider()));

        #endregion

        #region 消息
        private IDapperRepository<SystemMessageLog> _systemLogMessage;
        /// <summary>
        /// 消息记录表
        /// </summary>
        public IDapperRepository<SystemMessageLog> SystemMessageLog => _systemLogMessage ??= new DapperRepository<SystemMessageLog>(Connection, new SqlGenerator<SystemMessageLog>(GetSqlProvider()));

        private IDapperRepository<SystemMessageLogRead> _systemMessageLogRead;
        /// <summary>
        /// 消息阅读记录表,记录那些人员什么时候已经查看
        /// </summary>
        public IDapperRepository<SystemMessageLogRead> SystemMessageLogRead => _systemMessageLogRead ??= new DapperRepository<SystemMessageLogRead>(Connection, new SqlGenerator<SystemMessageLogRead>(GetSqlProvider()));
        #endregion

        #region 自定义
        #region 根据区域,控制器,方法获取对应菜单下按钮
        private IDapperRepository<SystemMenuButtonDataByMvcRote> _systemMenuButtons;
        /// <summary>
        /// 根据区域,控制器,方法获取对应菜单下按钮
        /// </summary>
        public IDapperRepository<SystemMenuButtonDataByMvcRote> SystemMenuButtons => _systemMenuButtons ??= new DapperRepository<SystemMenuButtonDataByMvcRote>(Connection, new SqlGenerator<SystemMenuButtonDataByMvcRote>(GetSqlProvider()));
        #endregion

        #region 根据县Id获取省市县Id
        private IDapperRepository<SystemDistrictFindDistrictByCountIdOutout> _systemDistrictFindDistrict;
        /// <summary>
        /// 根据县Id获取省市县Id
        /// </summary>
        public IDapperRepository<SystemDistrictFindDistrictByCountIdOutout> SystemDistrictFindDistrict => _systemDistrictFindDistrict ??= new DapperRepository<SystemDistrictFindDistrictByCountIdOutout>(Connection, new SqlGenerator<SystemDistrictFindDistrictByCountIdOutout>(GetSqlProvider()));
        #endregion

        #region 查询下级人员
        private IDapperRepository<SystemUserFindSubordinateOutput> _systemUserFindSubordinateOutput;
        /// <summary>
        /// 查询下级人员
        /// </summary>
        public IDapperRepository<SystemUserFindSubordinateOutput> SystemUserFindSubordinateOutput => _systemUserFindSubordinateOutput ??= new DapperRepository<SystemUserFindSubordinateOutput>(Connection, new SqlGenerator<SystemUserFindSubordinateOutput>(GetSqlProvider()));
        #endregion

        #region 查询上级人员
        private IDapperRepository<SystemUserFindLeaderOutput> _systemUserFindLeaderOutput;
        /// <summary>
        /// 查询上级人员
        /// </summary>
        public IDapperRepository<SystemUserFindLeaderOutput> SystemUserFindLeaderOutput => _systemUserFindLeaderOutput ??= new DapperRepository<SystemUserFindLeaderOutput>(Connection, new SqlGenerator<SystemUserFindLeaderOutput>(GetSqlProvider()));
        #endregion

        #region 根据人员归属类型及对应类型值获取对应人员
        private IDapperRepository<SystemPermissionUserFindByPrivilegeMasterAndValueOutput> _systemPermissionUserFindByPrivilegeMasterAndValueOutput;
        /// <summary>
        /// 根据人员归属类型及对应类型值获取对应人员
        /// </summary>
        public IDapperRepository<SystemPermissionUserFindByPrivilegeMasterAndValueOutput> SystemPermissionUserFindByPrivilegeMasterAndValueOutput => _systemPermissionUserFindByPrivilegeMasterAndValueOutput ??= new DapperRepository<SystemPermissionUserFindByPrivilegeMasterAndValueOutput>(Connection, new SqlGenerator<SystemPermissionUserFindByPrivilegeMasterAndValueOutput>(GetSqlProvider()));
        #endregion

        #endregion

        #region 工作流

        #region 工作流按钮信息
        private IDapperRepository<WorkflowButton> _workflowButton;
        /// <summary>
        /// 工作流按钮信息
        /// </summary>
        public IDapperRepository<WorkflowButton> WorkflowButton => _workflowButton ??= new DapperRepository<WorkflowButton>(Connection, new SqlGenerator<WorkflowButton>(GetSqlProvider()));
        #endregion

        #region 流程权限
        private IDapperRepository<WorkflowPermission> _workflowPermission;
        /// <summary>
        /// 
        /// </summary>
        public IDapperRepository<WorkflowPermission> WorkflowPermission => _workflowPermission ??= new DapperRepository<WorkflowPermission>(Connection, new SqlGenerator<WorkflowPermission>(GetSqlProvider()));
        #endregion

        #region 工作流定义表
        private IDapperRepository<WorkflowProcess> _workflowProcess;
        /// <summary>
        /// 工作流定义表:流程设计器设计后保存此表
        /// </summary>
        public IDapperRepository<WorkflowProcess> WorkflowProcess => _workflowProcess ??= new DapperRepository<WorkflowProcess>(Connection, new SqlGenerator<WorkflowProcess>(GetSqlProvider()));

        #endregion

        #region 工作流活动表
        private IDapperRepository<WorkflowProcessActivity> _workflowProcessActivity;
        /// <summary>
        /// 工作流活动表
        /// </summary>
        public IDapperRepository<WorkflowProcessActivity> WorkflowProcessActivity => _workflowProcessActivity ??= new DapperRepository<WorkflowProcessActivity>(Connection, new SqlGenerator<WorkflowProcessActivity>(GetSqlProvider()));
        #endregion

        #region 流程实例
        private IDapperRepository<WorkflowProcessInstance> _workflowProcessInstance;
        /// <summary>
        /// 流程实例:所有发送流程主表
        /// </summary>
        public IDapperRepository<WorkflowProcessInstance> WorkflowProcessInstance => _workflowProcessInstance ??= new DapperRepository<WorkflowProcessInstance>(Connection, new SqlGenerator<WorkflowProcessInstance>(GetSqlProvider()));
        #endregion

        #region 工作流运行活动表
        private IDapperRepository<WorkflowProcessInstanceActivity> _workflowProcessInstanceActivity;
        /// <summary>
        /// 工作流运行活动表
        /// </summary>
        public IDapperRepository<WorkflowProcessInstanceActivity> WorkflowProcessInstanceActivity => _workflowProcessInstanceActivity ??= new DapperRepository<WorkflowProcessInstanceActivity>(Connection, new SqlGenerator<WorkflowProcessInstanceActivity>(GetSqlProvider()));
        #endregion
     
        #region 流程实例连线
        private IDapperRepository<WorkflowProcessInstanceLink> _workflowProcessInstanceLink;
        /// <summary>
        /// 流程实例连线
        /// </summary>
        public IDapperRepository<WorkflowProcessInstanceLink> WorkflowProcessInstanceLink => _workflowProcessInstanceLink ??= new DapperRepository<WorkflowProcessInstanceLink>(Connection, new SqlGenerator<WorkflowProcessInstanceLink>(GetSqlProvider()));
        #endregion
       
        #region 流程实例任务表
        private IDapperRepository<WorkflowProcessInstanceTask> _workflowProcessInstanceTask;
        /// <summary>
        /// 流程实例任务表
        /// </summary>
        public IDapperRepository<WorkflowProcessInstanceTask> WorkflowProcessInstanceTask => _workflowProcessInstanceTask ??= new DapperRepository<WorkflowProcessInstanceTask>(Connection, new SqlGenerator<WorkflowProcessInstanceTask>(GetSqlProvider()));
        #endregion

        #region 流程实例连线
        private IDapperRepository<WorkflowProcessLink> _workflowProcessLink;
        /// <summary>
        /// 流程实例连线
        /// </summary>
        public IDapperRepository<WorkflowProcessLink> WorkflowProcessLink => _workflowProcessLink ??= new DapperRepository<WorkflowProcessLink>(Connection, new SqlGenerator<WorkflowProcessLink>(GetSqlProvider()));
        #endregion

        #region 业务归档表
        private IDapperRepository<WorkflowArchive> _workflowArchive;
        /// <summary>
        /// 业务归档表:若流程步骤启用了归档设置则将业务数据进行保存,以便于不依赖业务表也能够看当时流程审批情况
        /// </summary>
        public IDapperRepository<WorkflowArchive> WorkflowArchive => _workflowArchive ?? (_workflowArchive = new DapperRepository<WorkflowArchive>(Connection, new SqlGenerator<WorkflowArchive>(GetSqlProvider())));
        #endregion

        #endregion

        #region 微信

        private IDapperRepository<WeChatMpTemplateSend> _weChatMpTemplateSend;
        /// <summary>
        /// 
        /// </summary>
        public IDapperRepository<WeChatMpTemplateSend> WeChatMpTemplateSend => _weChatMpTemplateSend ??= new DapperRepository<WeChatMpTemplateSend>(Connection, new SqlGenerator<WeChatMpTemplateSend>(GetSqlProvider()));

        private IDapperRepository<WeChatMpTemplateSendUser> _weChatMpTemplateSendUser;
        /// <summary>
        /// 
        /// </summary>
        public IDapperRepository<WeChatMpTemplateSendUser> WeChatMpTemplateSendUser => _weChatMpTemplateSendUser ??= new DapperRepository<WeChatMpTemplateSendUser>(Connection, new SqlGenerator<WeChatMpTemplateSendUser>(GetSqlProvider()));

        private IDapperRepository<WeChatMpTemplate> _weChatMpTemplate;
        /// <summary>
        /// 
        /// </summary>
        public IDapperRepository<WeChatMpTemplate> WeChatMpTemplate => _weChatMpTemplate ??= new DapperRepository<WeChatMpTemplate>(Connection, new SqlGenerator<WeChatMpTemplate>(GetSqlProvider()));
        #endregion
    }
}