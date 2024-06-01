using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelReservationSystem.Models;
using System.Threading.Tasks;
using HotelReservationSystem.Data;  // Ensure this namespace is correct


namespace HotelReservationSystem.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly HotelReservationSystem.Data.HotelReservationContext _context;

        public RegisterModel(HotelReservationSystem.Data.HotelReservationContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Login");
        }
    }
}
