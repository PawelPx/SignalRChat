using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Data;
using SignalRChat.Models;

namespace SignalRChat.Pages.Messages
{
    public class IndexModel : PageModel
    {
        private readonly SignalRChat.Data.SignalRChatContext _context;

        public IndexModel(SignalRChat.Data.SignalRChatContext context)
        {
            _context = context;
        }

        public IList<Message> Message { get;set; }

        public async Task OnGetAsync()
        {
            Message = await _context.Message.ToListAsync();
        }
    }
}
