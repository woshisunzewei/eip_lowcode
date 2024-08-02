using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.DingTalk
{
    /// <summary>
    /// 
    /// </summary>
    public class DingTalkDepartmentDto: DingTalkBaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public List<DingTalkDepartmentOutput> Result { get; set; } = new List<DingTalkDepartmentOutput>();
    }
    /// <summary>
    /// 
    /// </summary>
    public class DingTalkDepartmentDetailDto : DingTalkBaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public DingTalkDepartmentOutput Result { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DingTalkDepartmentOutput
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public string dept_id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string parent_id { get; set; }

        /// <summary>
        /// 父部门ID
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组织机构父id
        /// </summary>		
        public Guid? ParentId { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DingTalkDepartmentMfDto : DingTalkBaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public List<DingTalkDepartmentMfOutput> Result { get; set; } = new List<DingTalkDepartmentMfOutput>();
    }
    /// <summary>
    /// 
    /// </summary>
    public class DingTalkDepartmentDetailMfDto : DingTalkBaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public DingTalkDepartmentMfOutput Result { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DingTalkDepartmentMfOutput
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public string cubeOrgId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string parentId { get; set; }

        /// <summary>
        /// 父部门ID
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 组织机构父id
        /// </summary>		
        public Guid? ParentOrganizationId { get; set; }

        /// <summary>
        /// 性质:0集团,1公司,2分公司,3子公司,4部门
        /// </summary>
        public int? Nature { get; set; }

        /// <summary>
        /// 魔方组织Id，魔方教育使用,获取人员时直接使用
        /// </summary>
        public string CubeOrgId { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class DingTalkImDepartmentMfDto : DingTalkBaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public List<DingTalkImDepartmentMfOutput> Result { get; set; } = new List<DingTalkImDepartmentMfOutput>();
    }

    public class DingTalkImDepartmentMfOutput
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public string deptId { get; set; }

        /// <summary>
        /// 父部门ID
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string parentId { get; set; }
    }
}
