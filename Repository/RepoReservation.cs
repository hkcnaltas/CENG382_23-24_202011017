using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ReservationSystem
{
    public class RepoReservation : IRepoReservation
    {
        private readonly string filePath;

        public RepoReservation(string filePath)
        {
            this.filePath = filePath;
            LoadReservations();
        }

        public List<Reservation> reservations;

        public void LoadReservations()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var reservationsObject = JsonConvert.DeserializeObject<ReservationList>(json);
                if(reservationsObject != null)
                {
                    reservations = reservationsObject.Reservations;
                }
                
            }
            else
            {
                reservations = new List<Reservation>();
            }
        }

 public void SaveReservations(List<Reservation> reservations)
{
    var reservationList = new ReservationList { Reservations = reservations };
    var json = JsonConvert.SerializeObject(reservationList, Formatting.Indented);
    File.WriteAllText("reservation.json", json);
}

        public void AddReservation(Reservation reservation)
        {
            reservations.Add(reservation);
            SaveReservations(reservations);
        }

        public void DeleteReservation(Reservation reservation)
        {
            reservations.Remove(reservation);
            SaveReservations(reservations);
        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            return reservations;
        }
    }

}