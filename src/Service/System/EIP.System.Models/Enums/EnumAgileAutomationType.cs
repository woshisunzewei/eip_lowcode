namespace EIP.System.Models.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum EnumAgileAutomationType
    {
        /// <summary>
        /// 工作表新增或变更
        /// </summary>
        工作表新增或变更 = 1,

        /// <summary>
        /// 时间周期循环
        /// </summary>
        时间周期循环 = 2,

        /// <summary>
        /// 指定日期字段
        /// </summary>
        指定日期字段 = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EnumAgileAutomationTableTriggerType
    {
        /// <summary>
        /// 当新增或更新记录时
        /// </summary>
        当新增或更新记录时 = 1,

        /// <summary>
        /// 仅新增记录时
        /// </summary>
        仅新增记录时 = 2,

        /// <summary>
        /// 仅更新记录时
        /// </summary>
        仅更新记录时 = 3,

        /// <summary>
        /// 删除记录时
        /// </summary>
        删除记录时 = 4,
    }

    /// <summary>
    /// 控件类型
    /// </summary>
    public enum EnumAgileAutomationControlType
    {
        /// <summary>
        /// 数据改变
        /// </summary>
        数据改变 = 0,

        /// <summary>
        /// 新增数据
        /// </summary>
        新增数据 = 1,

        /// <summary>
        /// 更新数据
        /// </summary>
        更新数据 = 2,

        /// <summary>
        /// 获取单条数据
        /// </summary>
        获取单条数据 = 3,

        /// <summary>
        /// 获取多条数据
        /// </summary>
        获取多条数据 = 4,

        /// <summary>
        /// 删除数据
        /// </summary>
        删除数据 = 5,

        /// <summary>
        /// 发送站内通知
        /// </summary>
        发送站内通知 = 6,
        /// <summary>
        /// 发送短信
        /// </summary>
        发送短信 = 7,
        /// <summary>
        /// 发送邮件
        /// </summary>
        发送邮件 = 12,
        /// <summary>
        /// 发送服务号信息
        /// </summary>
        发送服务号信息 = 13,
        /// <summary>
        /// 条件分支
        /// </summary>
        条件分支 = 8,

        /// <summary>
        /// 并行分支
        /// </summary>
        并行分支 = 9,

        /// <summary>
        /// 发送API请求
        /// </summary>
        发送API请求 = 21,

        /// <summary>
        /// 代码块
        /// </summary>
        代码块 = 22
    }


    /// <summary>
    /// 通知人员类型枚举
    /// </summary>
    public enum EnumAgileAutomationNoticeUserType
    {
        /// <summary>
        /// 所有成员
        /// </summary>
        所有成员 = 0,
        /// <summary>
        /// 组织
        /// </summary>
        组织 = 2,
        /// <summary>
        /// 人员
        /// </summary>
        人员 = 4,
        /// <summary>
        /// 角色
        /// </summary>
        角色 = 6,
        /// <summary>
        /// 岗位
        /// </summary>
        岗位 = 7,
        /// <summary>
        /// 组
        /// </summary>
        组 = 10,
        /// <summary>
        /// 发起者
        /// </summary>
        发起者 = 22,
        /// <summary>
        /// 发起者直线领导
        /// </summary>
        发起者直线领导 = 24,
        /// <summary>
        /// 提交人直线领导
        /// </summary>
        提交人直线领导 = 26,

        /// <summary>
        /// 表单人员
        /// </summary>
        表单人员 = 28,
        /// <summary>
        /// 自定义
        /// </summary>
        自定义 = 90,
        /// <summary>
        /// 程序指定
        /// </summary>
        程序指定 = 100,
    }

}
