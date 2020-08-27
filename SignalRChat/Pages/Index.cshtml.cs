using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SignalRChat.Data;
using SignalRChat.Hubs;
using SignalRChat.Models;

namespace SignalRChat.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private UserInMemory _userInMemory;

        public IndexModel(ILogger<IndexModel> logger, UserInMemory userInMemory)
        {
            _logger = logger;
            _userInMemory = userInMemory;
        }


        [BindProperty]
        public AppUser AppUser { get; set; }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _userInMemory.AddUpdate(AppUser.Username, null);
            return RedirectToPage("./Chat");
        }
    }
}
