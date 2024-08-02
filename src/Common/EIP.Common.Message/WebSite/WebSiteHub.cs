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

using EIP.Common.Core.Auth;
using EIP.Common.Log.Handler;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EIP.Common.Message.WebSite
{
    /// <summary>
    /// 消息通讯
    /// </summary>
    public class WebSiteHub : Hub
    {
        /// <summary>
        /// 建立连接时触发:上线
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 离开连接时触发:离线
        /// </summary>b
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            //移除
            var user = OnlineUsers.Users.Where(w => w != null).FirstOrDefault(a => a.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                OnlineUsers.Users.Remove(user);
                //更新用户离线时间
                LoginOutLogHandler handler = new LoginOutLogHandler(new PrincipalUser
                { }, user.LoginId);
                handler.WriteLog();
            }
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// 添加用户上线数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public void SendUserOnline(OnlineUserInput input)
        {
            input.ConnectionId = Context.ConnectionId;
            //判断是否强制下线配置
            if (input.Sso)
            {
                //判断登录账号是否存在
                var user = OnlineUsers.Users.Where(w => w.UserId == input.UserId).ToList();
                //已存在
                if (user.Any())
                {
                    var userConn = user.Where(w => w.ConnectionId != input.ConnectionId);
                    //发送踢下线信息
                    foreach (var item in userConn)
                    {
                        OnlineUsers.Users.Remove(item);
                        if (input.LoginId != item.LoginId)
                        {
                            Clients.Client(item.ConnectionId).SendAsync("CompulsoryDownline", $"{JsonConvert.SerializeObject(input)}");
                        }
                    }
                }
                if (OnlineUsers.Users.Where(w => w != null).All(a => a.ConnectionId != input.ConnectionId))
                {
                    input.GoOnlineTime = DateTime.Now;
                    OnlineUsers.Users.Add(input);
                }
            }
            else
            {
                //删除所有登录Id一样连接
                var loginUsers=OnlineUsers.Users.Where(w => w != null&& w.LoginId == input.LoginId).ToList();
                foreach (var item in loginUsers)
                {
                    OnlineUsers.Users.Remove(item);
                }
                if (OnlineUsers.Users.Where(w => w != null).All(a => a.ConnectionId != input.ConnectionId))
                {
                    input.GoOnlineTime = DateTime.Now;
                    OnlineUsers.Users.Add(input);
                }
            }
        }

        /// <summary>
        /// 给特定组发送消息
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendToGroup(string groupName, string message)
        {
            return Clients.Group(groupName).SendAsync("ReceiveMessage:", $"{Context.ConnectionId}@{groupName}: {message}");
        }

        /// <summary>
        /// 加入组并发送信息
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage:", $"{Context.ConnectionId}joined{groupName}");
        }

        /// <summary>
        /// 移除指定组并向组推送消息
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} left {groupName}");
        }

        /// <summary>
        /// 向指定Id推送消息
        /// </summary>
        /// <param name="userid">要推送消息的对象</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task Echo(string userid, string message)
        {
            return Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", $"{Context.ConnectionId}: {message}");
        }

        /// <summary>
        /// 向指定人员推送消息
        /// </summary>
        /// <returns></returns>
        public Task SendMessage(Guid userId, string msg)
        {
            var user = OnlineUsers.Users.Where(w => w != null).FirstOrDefault(w => w.UserId == userId);
            if (user != null)
                return Clients.Client(user.ConnectionId).SendAsync("SendMessage", msg);
            return null;
        }

        /// <summary>
        /// 向所有人推送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAll(string message)
        {
            await Clients.All.SendAsync("SendMessageAll", Context.ConnectionId, message);
        }
    }
}
