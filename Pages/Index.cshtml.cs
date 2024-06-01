using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HotelReservationContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(HotelReservationContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Room Room { get; set; } = new Room();
        public IList<Room> Rooms { get; set; } = new List<Room>();
        [BindProperty]
        public HotelReservationSystem.Models.Reservation Reservation { get; set; } = new HotelReservationSystem.Models.Reservation();
        public IList<HotelReservationSystem.Models.Reservation> ReservedRooms { get; set; } = new List<HotelReservationSystem.Models.Reservation>();

        public string FilterRoomName { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public int? FilterCapacity { get; set; }

        public async Task OnGetAsync(string filterRoomName, DateTime? filterStartDate, DateTime? filterEndDate, int? filterCapacity)
        {
            FilterRoomName = filterRoomName;
            FilterStartDate = filterStartDate;
            FilterEndDate = filterEndDate;
            FilterCapacity = filterCapacity;

            var roomsQuery = _context.Rooms.Include(r => r.Reservations).AsQueryable();
            var reservationsQuery = _context.Reservations.Include(r => r.Room).AsQueryable();

            if (!string.IsNullOrEmpty(FilterRoomName))
            {
                roomsQuery = roomsQuery.Where(r => r.Name.Contains(FilterRoomName));
            }
            if (FilterStartDate.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.StartDate >= FilterStartDate.Value);
            }
            if (FilterEndDate.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.EndDate <= FilterEndDate.Value);
            }
            if (FilterCapacity.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.Capacity >= FilterCapacity.Value);
            }

            Rooms = await roomsQuery.ToListAsync();
            ReservedRooms = await reservationsQuery.ToListAsync();
        }

        public async Task<IActionResult> OnPostCreateRoomAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState geçersiz: {@ModelState}", ModelState);
                Rooms = await _context.Rooms.Include(r => r.Reservations).ToListAsync();
                ReservedRooms = await _context.Reservations.Include(r => r.Room).ToListAsync();
                return Page();
            }

            _logger.LogInformation("Oda ekleniyor: {@Room}", Room);
            _context.Rooms.Add(Room);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Oda kaydedildi: {@Room}", Room);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Oda kaydedilirken hata oluştu.");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditReservationAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState geçersiz: {@ModelState}", ModelState);
                Rooms = await _context.Rooms.Include(r => r.Reservations).ToListAsync();
                ReservedRooms = await _context.Reservations.Include(r => r.Room).ToListAsync();
                return Page();
            }

            var reservationToUpdate = await _context.Reservations.FindAsync(Reservation.Id);
            if (reservationToUpdate == null)
            {
                _logger.LogWarning("Rezervasyon bulunamadı: {ReservationId}", Reservation.Id);
                return NotFound();
            }

            reservationToUpdate.StartDate = DateTime.SpecifyKind(Reservation.StartDate, DateTimeKind.Utc);
            reservationToUpdate.EndDate = DateTime.SpecifyKind(Reservation.EndDate, DateTimeKind.Utc);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Rezervasyon güncellendi: {ReservationId}", Reservation.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon güncellenirken hata oluştu.");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteReservationAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                _logger.LogWarning("Rezervasyon bulunamadı: {ReservationId}", id);
                return NotFound();
            }

            _context.Reservations.Remove(reservation);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Rezervasyon silindi: {ReservationId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon silinirken hata oluştu.");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteRoomAsync(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                _logger.LogWarning("Oda bulunamadı: {RoomId}", roomId);
                return NotFound();
            }

            var reservations = _context.Reservations.Where(r => r.RoomId == roomId);
            _context.Reservations.RemoveRange(reservations);
            _context.Rooms.Remove(room);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Oda silindi: {RoomId}", roomId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Oda silinirken hata oluştu.");
            }

            return RedirectToPage();
        }

        public bool IsRoomAvailable(int roomId, DateTime startDate, DateTime endDate)
        {
            var reservations = _context.Reservations
                .Where(r => r.RoomId == roomId &&
                            ((r.StartDate <= startDate && r.EndDate >= startDate) ||
                             (r.StartDate <= endDate && r.EndDate >= endDate) ||
                             (r.StartDate >= startDate && r.EndDate <= endDate)))
                .ToList();

            return !reservations.Any();
        }
    }
}
