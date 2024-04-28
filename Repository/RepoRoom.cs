using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ReservationSystem;

namespace ReservationSystem
{
    public class RepoRoom : IRepoRoom
    {
        private readonly string filePath;

        public RepoRoom(string filePath)
        {
            this.filePath = filePath;
        }

        public IEnumerable<Room> GetRooms()
        {
            if (!File.Exists(filePath))
            {
                return new List<Room>();
            }

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<RoomList>(json).Room;
        }

        public void SaveRooms(IEnumerable<Room> rooms)
        {
            var json = JsonConvert.SerializeObject(new RoomList { Room = rooms.ToList() });
            File.WriteAllText(filePath, json);
        }
    }
}