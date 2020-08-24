using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SignalRChat.Data;
using SignalRChat.Models;

namespace SignalRChat.Pages.Messages
{
    public class CreateModel : PageModel
    {
        private readonly SignalRChat.Data.SignalRChatContext _context;

        public CreateModel(SignalRChat.Data.SignalRChatContext context)
        {
            _context = context;
        }

        //public IActionResult OnGet()
        //{
        //    return Page();
        //}

        [BindProperty]
        public Message Message { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task OnPostAsync()
        {
            Message.SendDate = DateTime.Now;
            _context.Message.Add(Message);
            await _context.SaveChangesAsync();
        }
    }
}
