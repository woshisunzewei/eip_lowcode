using EIP.System.Models.Dtos.DingTalk;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.UserThree
{
    /// <summary>
    /// 
    /// </summary>
    public class DingTalkUserDto : DingTalkBaseDto
    {

        /// <summary>
        /// 
        /// </summary>
        public DingTalkUserResult Result;
    }

    /// <summary>
    /// 
    /// </summary>
    public class DingTalkUserResult
    {
        /// <summary>
        /// 是否还有更多的数据
        /// </summary>
        public bool has_more { get; set; }

        /// <summary>
        /// 下一次分页的游标
        /// </summary>
        public int next_cursor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<DingTalkUserListOutput> list = new List<DingTalkUserListOutput>();
    }
    /// <summary>
    /// https://open.dingtalk.com/document/isvapp-server/queries-the-complete-information-of-a-department-user
    /// </summary>
    public class DingTalkUserListOutput
    {
        /// <summary>
        /// 用户的userId
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 用户在当前开发者企业帐号范围内的唯一标识
        /// </summary>
        public string unionid { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 国际电话区号
        /// </summary>
        public string state_code { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 员工工号
        /// </summary>
        public string job_number { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 员工邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 所属部门id列表
        /// </summary>
        public List<string> dept_id_list { get; set; }

        /// <summary>
        /// 所属部门id列表
        /// </summary>
        public string dept_id_list_string { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string deptId { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class DingTalkUserDetailMfDto : DingTalkBaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public DingTalkUserMfOutput Result { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DingTalkUserMfOutput
    {

        /// <summary>
        /// 
        /// </summary>
        public List<DingTalkUserMfDataOutput> data = new List<DingTalkUserMfDataOutput>();

        public int totalCount { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DingTalkUserMfDataOutput
    {
        public string sex { get; set; }

        public string mobile { get; set; }

        public string name { get; set; }

        public string cubeUserId { get; set; }

        public string avatarUrl { get; set; }
    }
}
