namespace ReservationSystem
{
    public interface IReservationService
    {
        void AddReservation(Reservation reservation);
        void DeleteReservation(Reservation reservation);
        void DisplayWeeklySchedule();
    }
}