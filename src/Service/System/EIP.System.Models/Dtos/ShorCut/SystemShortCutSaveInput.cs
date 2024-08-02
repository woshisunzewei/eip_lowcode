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
using EIP.Base.Models.Entities.System;
using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.ShortCut
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemShortCutSaveInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short Type { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public IList<SystemShortCut> ShortCuts { get; set; } = new List<SystemShortCut>();
    }
}