using System;
using System.Linq;

namespace ReservationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var RepoRoom = new RepoRoom("Data.json");
            var RepoReservation = new RepoReservation("reservation.json");
            var RepoLogger = new RepoLogger("log.json");

            var reservationService = new ReservationHandler(RepoReservation, RepoRoom, RepoLogger);

            var targetRoom = repoRoom.GetRooms().FirstOrDefault(r => r.RoomId == "A-116");
            var newReservation = new Reservation(DateTime.Now, DateTime.Today, "Hilal Altaş", targetRoom);
            reservationService.AddReservation(newReservation);
            DisplayReservations(reservationRepo);
        }

        static void DisplayReservations(ReservationRepository repository)
        {
            var reservations = repository.GetAllReservations();
            foreach (var reservation in reservations)
            {
                Console.WriteLine($"Reservation: {reservation.ReserverName} on {reservation.Date} at {reservation.Time} in {reservation.Room.RoomName}");
            }
        }
    }
}
