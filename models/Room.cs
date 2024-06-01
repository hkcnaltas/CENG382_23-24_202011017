namespace HotelReservationSystem.Models
{
    public class Accommodation
    {
        public int AccommodationId { get; set; }
        public string RoomName { get; set; } = string.Empty; // Non-nullable property
        public int MaxOccupancy { get; set; }
        public string ScenicView { get; set; } = string.Empty; // Non-nullable property
        public List<Booking> Bookings { get; set; } = new List<Booking>(); // Non-nullable property
    }
}
