namespace ReservationSystem
{
    public class Reservation
    {
        public DateTime Time { get; }
        public DateTime Date { get; }
        public string ReserverName { get; }
        public Room Room { get; }

        public Reservation(DateTime time, DateTime date, string reserverName, Room room)
        {
            Time = time;
            Date = date;
            ReserverName = reserverName;
            Room = room;
        }
    }
}