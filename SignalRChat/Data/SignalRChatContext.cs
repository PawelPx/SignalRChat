using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Models;

namespace SignalRChat.Data
{
    public class SignalRChatContext : DbContext
    {
        public SignalRChatContext (DbContextOptions<SignalRChatContext> options)
            : base(options)
        {
        }

        public DbSet<SignalRChat.Models.Message> Message { get; set; }
        public AppUser AppUser { get; set; } = new AppUser();

    }
}
