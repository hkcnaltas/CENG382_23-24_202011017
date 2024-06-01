using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelReservationSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HotelReservationSystem.Data.HotelReservationContext _context;

        public LoginModel(HotelReservationSystem.Data.HotelReservationContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _context.Users
                .Where(u => u.Email == Email && u.Password == Password)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            // Kullanıcı oturum bilgilerini saklama (Session veya Cookie kullanılabilir)
            HttpContext.Session.SetString("UserEmail", user.Email);

            // Kullanıcı giriş yaptıktan sonra yönlendirme
            return RedirectToPage("/Index");
        }
    }
}
