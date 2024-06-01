using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HotelReservationSystem.Pages
{
    public class CreateReservationModel : PageModel
    {
        private readonly HotelReservationContext _context;
        private readonly ILogger<CreateReservationModel> _logger;

        public CreateReservationModel(HotelReservationContext context, ILogger<CreateReservationModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public HotelReservationSystem.Models.Reservation Reservation { get; set; }

        public IList<Room> Rooms { get; set; }

        public async Task<IActionResult> OnGetAsync(int roomId)
        {
            _logger.LogInformation("OnGetAsync called with roomId: {RoomId}", roomId);

            Rooms = await _context.Rooms.ToListAsync();
            Reservation = new HotelReservationSystem.Models.Reservation
            {
                RoomId = roomId,
                UserName = HttpContext.Session.GetString("UserEmail") ?? string.Empty
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPostAsync called with Reservation: {@Reservation}", Reservation);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid.");
                Rooms = await _context.Rooms.ToListAsync();
                return Page();
            }

            // DateTime değerlerini UTC olarak ayarla
            Reservation.StartDate = DateTime.SpecifyKind(Reservation.StartDate, DateTimeKind.Utc);
            Reservation.EndDate = DateTime.SpecifyKind(Reservation.EndDate, DateTimeKind.Utc);

            _context.Reservations.Add(Reservation);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Reservation created successfully.");
            return RedirectToPage("/Index");
        }
    }
}
