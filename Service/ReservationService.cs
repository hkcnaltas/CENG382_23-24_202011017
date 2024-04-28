namespace ReservationSystem
{
    public class ReservationService : IReservationService
    {
        private readonly ReservationHandler reservationHandler;

        public ReservationService(ReservationHandler reservationHandler)
        {
            this.reservationHandler = reservationHandler;
        }

        public void AddReservation(Reservation reservation)
        {
            reservationHandler.AddReservation(reservation);
        }

        public void DeleteReservation(Reservation reservation)
        {
            reservationHandler.DeleteReservation(reservation);
        }

        public void DisplayWeeklySchedule()
        {
            reservationHandler.DisplayWeeklySchedule();
        }
    }
}