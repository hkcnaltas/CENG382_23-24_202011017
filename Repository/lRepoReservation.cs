namespace ReservationSystem
{
    public interface IRepoReservation
    {
        void AddReservation(Reservation reservation);
        void DeleteReservation(Reservation reservation);
        IEnumerable<Reservation> GetAllReservations();
    }
}