using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
namespace ReservationSystem
{
    public class ReservationHandler
    {
        private readonly IRepoReservation RepoReservation;
        private readonly IRepoRoom RepoRoom;
        private readonly IRepoLogger repoLogger;

        public ReservationHandler(IRepoReservation RepoReservation, IRepoRoom RepoRoom, IRepoLogger Repologger)
        {
            this.RepoReservation = RepoReservation;
            this.RepoRoom = RepoRoom;
            this.RepoLogger = RepoLogger;
        }

        public void AddReservation(Reservation reservation)
        {
            var room = RepoRoom.GetRooms().FirstOrDefault(r => r.RoomId == reservation.Room.RoomId);
            if (room != null)
            {
                room.Capacity--;
                RepoRoom.SaveRooms(RepoRoom.GetRooms());

                var reservations = RepoReservation.GetAllReservations();
                var list = reservations.ToList();
                list.Add(reservation);
                ((RepoReservation)RepoReservation).SaveReservations(list);

                Console.WriteLine(JsonConvert.SerializeObject(new ReservationList { Reservations = list }, Formatting.Indented));
                logger.Log(new LogRecord(DateTime.Now, reservation.ReserverName, reservation.Room.RoomName));
            }
        }

        public void DeleteReservation(Reservation reservation)
        {
            var room = RepoRoom.GetRooms().FirstOrDefault(r => r.RoomId == reservation.Room.RoomId);
            if (room != null)
            {
                room.Capacity++;
                RepoRoom.SaveRooms(RepoRoom.GetRooms());
                RepoReservation.DeleteReservation(reservation);
                logger.Log(new LogRecord(DateTime.Now, reservation.ReserverName, reservation.Room.RoomName));
            }
        }

        public void DisplayWeeklySchedule()
        {
            
        }
    }
}