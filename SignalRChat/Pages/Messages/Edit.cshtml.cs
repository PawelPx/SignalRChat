using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Data;
using SignalRChat.Models;

namespace SignalRChat.Pages.Messages
{
    public class EditModel : PageModel
    {
        private readonly SignalRChat.Data.SignalRChatContext _context;

        public EditModel(SignalRChat.Data.SignalRChatContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Message Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Message = await _context.Message.FirstOrDefaultAsync(m => m.Id == id);

            if (Message == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(Message.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.Id == id);
        }
    }
}
