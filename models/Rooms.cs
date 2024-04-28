namespace ReservationSystem
{
    public class Room
    {
        public string RoomId { get; }
        public string RoomName { get; } 
        public int Capacity { get; set; }

        public Room(string roomId, string roomName, int capacity)
        {
            this.RoomId = roomId;
            this.RoomName = roomName;
            this.Capacity = capacity;
        }
    }
}