using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SignalRChat.Data;
using SignalRChat.Models;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IServiceProvider _sp;
        public ChatHub(IServiceProvider sp)
        {
            _sp = sp;
        }

        [BindProperty]
        public Message Message { get; set; } = new Message();

        public async Task SendMessage(string user, string message)
        {
            using (var scope = _sp.CreateScope())
            {
                var time = DateTime.Now;
                var timeString = time.ToString("HH:mm dd/MM/yyyy");
                await Clients.All.SendAsync("ReceiveMessage", user, message, timeString);

                var context = scope.ServiceProvider.GetRequiredService<SignalRChatContext>();
                Message.User = user;
                Message.Text = message;
                Message.SendDate = time;
                context.Message.Add(Message);
                await context.SaveChangesAsync();
            }
        }
    }
}