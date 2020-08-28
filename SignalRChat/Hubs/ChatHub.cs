using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using SignalRChat.Data;
using SignalRChat.Models;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IServiceProvider _sp;
        private UserInMemory _userInMemory;

        public ChatHub(IServiceProvider sp, UserInMemory userInMemory)
        {
            _sp = sp;
            _userInMemory = userInMemory;
        }

        [BindProperty]
        public Message Message { get; set; } = new Message();

        public async Task SendMessage(string user, string message, string receiver)
        {
            using (var scope = _sp.CreateScope())
            {
                var time = DateTime.Now;
                var timeString = time.ToString("HH:mm dd/MM/yyyy");
                if (receiver == null)
                {
                    await Clients.All.SendAsync("ReceiveMessage", user, message, timeString, false);
                }
                else
                {
                    await Clients.Client(_userInMemory.GetUserInfo(receiver).ConnectionId).SendAsync("ReceiveMessage", user, message, timeString, true);
                    await Clients.Caller.SendAsync("ReceiveMessage", user, message, timeString, true);
                }
                await SaveToDb(scope, user, message, time);
            }
        }


        private async Task SaveToDb(IServiceScope scope, string user, string message, DateTime time)
        {
            var context = scope.ServiceProvider.GetRequiredService<SignalRChatContext>();
            Message.User = user;
            Message.Text = message;
            Message.SendDate = time;
            context.Message.Add(Message);
            await context.SaveChangesAsync();
        }

        public async Task Join(string user)
        {
            _userInMemory.AddUpdate(user, Context.ConnectionId);
            await Clients.Others.SendAsync("AppendToUserList", user);
        }

    }
}