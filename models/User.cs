namespace HotelReservationSystem.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty; // Non-nullable property
        public string PasswordHash { get; set; } = string.Empty; // Non-nullable property
        public string EmailAddress { get; set; } = string.Empty; // Non-nullable property
        public string GenderIdentity { get; set; } = string.Empty; // Non-nullable property
        public int Age { get; set; }
    }
}
