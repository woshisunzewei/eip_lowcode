using System;

namespace EIP.System.Models.Dtos.AgileDataLog
{
    public class SystemAgileDataLogFindByRelationInput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid RelationId { get; set; }
    }

    public class SystemAgileDataLogFindByRelationOutput
    {
        /// <summary>
        /// 类型，1新增，2修改，3删除
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 内容:Json格式[{filed:'字段',name:'字段名称',before:'处理前',after:'处理后'}]
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建用户Id
        /// </summary>		
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public string CreateUserHeadImage { get; set; }
    }
}
