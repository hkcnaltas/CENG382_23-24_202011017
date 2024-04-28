namespace ReservationSystem{
    public interface IRepoRoom
    {
        IEnumerable<Room> GetRooms();
        void SaveRooms(IEnumerable<Room> rooms);
    }
}