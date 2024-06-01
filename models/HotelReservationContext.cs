using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Data
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().ToTable("RoomDetails");
            modelBuilder.Entity<Reservation>().ToTable("BookingDetails");
            modelBuilder.Entity<User>().ToTable("UserDetails");

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("RoomID");
                entity.Property(e => e.Name).HasColumnName("RoomName");
                entity.Property(e => e.Capacity).HasColumnName("RoomCapacity");
                entity.Property(e => e.View).HasColumnName("RoomView");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("BookingID");
                entity.Property(e => e.RoomId).HasColumnName("RoomRefID");
                entity.Property(e => e.UserName).HasColumnName("UserRefName");
                entity.Property(e => e.StartDate).HasColumnName("CheckInDate");
                entity.Property(e => e.EndDate).HasColumnName("CheckOutDate");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Room_Bookings");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("UserID");
                entity.Property(e => e.UserName).HasColumnName("UserName");
                entity.Property(e => e.Password).HasColumnName("UserPassword");
                entity.Property(e => e.Email).HasColumnName("UserEmail");
                entity.Property(e => e.Gender).HasColumnName("UserGender");
                entity.Property(e => e.Age).HasColumnName("UserAge");
            });
        }
    }
}

