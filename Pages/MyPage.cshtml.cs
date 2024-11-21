using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleApp.Data;
using SimpleApp.Models;

namespace SimpleApp.Pages
{
    public class MyPageModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        
        public MyPageModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public string FirstName { get; set; }
        public int Points{ get; set; }
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                FirstName = user.FirstName; // Eller anv�nd FirstName om det finns
                Points = user.Points ?? 0; // H�mtar po�ng, eller 0 om inga po�ng �r angivna
            }
        }
        
    }
}
