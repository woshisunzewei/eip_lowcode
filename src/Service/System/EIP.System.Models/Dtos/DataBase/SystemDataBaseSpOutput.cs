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
using System.Collections.Generic;
using System.Text;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 存储过程
    /// </summary>
    public class SystemDataBaseSpOutput 
    {
        /// <summary>
        /// 
        /// </summary>
        public string ClassName;
        /// <summary>
        /// 
        /// </summary>
        public string CleanName;
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public List<SystemDataBaseSpParamOutput> Parameters;

        /// <summary>
        /// 
        /// </summary>
        public SystemDataBaseSpOutput()
        {
            Parameters = new List<SystemDataBaseSpParamOutput>();
        }
        /// <summary>
        /// 
        /// </summary>
        public string ArgList
        {
            get
            {
                var sb = new StringBuilder();
                var loopCount = 1;
                foreach (var par in Parameters)
                {
                    sb.AppendFormat("{0} {1}", par.SysType, par.CleanName);
                    if (loopCount < Parameters.Count)
                        sb.Append(",");
                    loopCount++;
                }
                return sb.ToString();
            }
        }
    }
}