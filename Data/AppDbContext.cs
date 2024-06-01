using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accommodation>().ToTable("Accommodations");
            modelBuilder.Entity<Booking>().ToTable("Bookings");
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<LogEntry>().ToTable("LogEntries");

            modelBuilder.Entity<Accommodation>(entity =>
            {
                entity.Property(e => e.AccommodationId).HasColumnName("accommodation_id");
                entity.Property(e => e.RoomName).HasColumnName("room_name");
                entity.Property(e => e.MaxOccupancy).HasColumnName("max_occupancy");
                entity.Property(e => e.ScenicView).HasColumnName("scenic_view");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.BookingId).HasColumnName("booking_id");
                entity.Property(e => e.AssignedRoomId).HasColumnName("assigned_room_id");
                entity.Property(e => e.ReservedBy).HasColumnName("reserved_by");
                entity.Property(e => e.CheckInDate).HasColumnName("check_in_date");
                entity.Property(e => e.CheckOutDate).HasColumnName("check_out_date");

                entity.HasOne(d => d.AssignedRoom)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.AssignedRoomId)
                    .HasConstraintName("FK_AssignedRoom_Bookings");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasColumnName("customer_id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.EmailAddress).HasColumnName("email_address");
                entity.Property(e => e.GenderIdentity).HasColumnName("gender_identity");
                entity.Property(e => e.Age).HasColumnName("age");
            });

            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.Property(e => e.LogId).HasColumnName("log_id");
                entity.Property(e => e.Timestamp).HasColumnName("timestamp");
                entity.Property(e => e.Level).HasColumnName("level");
                entity.Property(e => e.Message).HasColumnName("message");
                entity.Property(e => e.Exception).HasColumnName("exception");
            });
        }
    }
}
