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

        // Method for sending messages
        public async Task SendMessage(string user, string message, string receiver)
        {
            using (var scope = _sp.CreateScope())
            {
                var time = DateTime.Now;
                var timeString = time.ToString("HH:mm dd/MM/yyyy");

                // receiver equal null means message to all; else it's direct message
                if (receiver == null)
                {
                    await Clients.All.SendAsync("ReceiveMessage", user, message, timeString, false, receiver);
                }
                else
                {
                    await Clients.Client(_userInMemory.GetUserInfo(receiver).ConnectionId).SendAsync("ReceiveMessage", user, message, timeString, true, receiver);
                    await Clients.Caller.SendAsync("ReceiveMessage", user, message, timeString, true, receiver);
                }
                await SaveToDb(scope, user, message, time);
            }
        }

        // Method saving messages to database
        private async Task SaveToDb(IServiceScope scope, string user, string message, DateTime time)
        {
            var context = scope.ServiceProvider.GetRequiredService<SignalRChatContext>();
            Message.User = user;
            Message.Text = message;
            Message.SendDate = time;
            context.Message.Add(Message);
            await context.SaveChangesAsync();
        }

        // Joining the chat, activated after username is established and adds you to the users' list
        public async Task Join(string user)
        {
            _userInMemory.AddUpdate(user, Context.ConnectionId);
            await Clients.Others.SendAsync("AppendToUserList", user);
        }

        // Method creates a private chat you and chosen user on that user's side
        public async Task CreateChatToOtherUser(string receiver, string user)
        {
            await Clients.Client(_userInMemory.GetUserInfo(receiver).ConnectionId).SendAsync("StartDirectChat", user);
        }
    }
}