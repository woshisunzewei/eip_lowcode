﻿using System;
using EIP.Common.OAuth.Enums;

namespace EIP.Common.OAuth.Models
{
    public class AuthResponse
    {
        /**
        * 授权响应状态码
        */
        public int code { get; set; }

        /**
         * 授权响应信息
         */
        public string msg { get; set; }

        /**
         * 授权响应数据，当且仅当 code = 2000 时返回
         */
        public object data { get; set; }

        /**
         * 是否请求成功
         *
         * @return true or false
         */
        public bool ok()
        {
            return this.code == Convert.ToInt32(AuthResponseStatus.SUCCESS);
        }

        public AuthResponse(int code, string msg, object data = null)
        {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }
    }

   
    public class AuthResponseWechatWorkMemberResult
    {
     
        public string key { get; set; }

        public string value { get; set; }
    }
}