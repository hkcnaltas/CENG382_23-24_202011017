﻿using System;
using System.IO;
using System.Text.Json;
using System.Globalization;

public class RoomData
{
    public Room[] Room { get; set; }
}

public class Room
{
    public string roomId { get; set; }
    public string roomName { get; set; }
    public int capacity { get; set; }
}

public class Reservation
{
    public DateTime time { get; set; }
    public DateTime date { get; set; }
    public string reserverName { get; set; }
    public Room room { get; set; }
}

public class ReservationHandler
{
    private Reservation[][] reservations;
    public RoomData roomData { get; set; }
    private DateTime[] dateData;

    public ReservationHandler(RoomData rooms, DateTime[] dates)
    {
        roomData = rooms;
        dateData = dates;
        reservations = new Reservation[roomData.Room.Length][];
        for (int i = 0; i < roomData.Room.Length; i++)
        {
            reservations[i] = new Reservation[dates.Length];
        }
    }

    public bool AddReservation(Reservation reservation)
    {
        int roomIndex = Array.FindIndex(roomData.Room, room => room.roomName == reservation.room.roomName);
        if (roomIndex == -1) 
        {
            Console.WriteLine("Room not found.");
            return false;
        }

        int dateIndex = Array.FindIndex(dateData, date => date.Date == reservation.date.Date);
        if (dateIndex == -1)
        {
            Console.WriteLine("Date not found.");
            return false;
        }

        if (reservations[roomIndex][dateIndex] == null)
        {
            reservations[roomIndex][dateIndex] = reservation;
            Console.WriteLine("Reservation added successfully.");
            return true;
        }
        else
        {
            Console.WriteLine($"Cannot add reservation. Room {reservation.room.roomName} is already booked for {reservation.date:dd.MM.yyyy}.");
            return false;
        }
    }

    public bool RemoveReservation(string roomName, DateTime date, string reserverName)
    {
        int roomIndex = Array.FindIndex(roomData.Room, room => room.roomName == roomName);
        if (roomIndex == -1) 
        {
            Console.WriteLine("Room not found.");
            return false;
        }

        int dateIndex = Array.FindIndex(dateData, d => d.Date == date.Date);
        if (dateIndex == -1)
        {
            Console.WriteLine("Date not found.");
            return false;
        }

        var reservation = reservations[roomIndex][dateIndex];
        if (reservation != null && reservation.reserverName == reserverName)
        {
            reservations[roomIndex][dateIndex] = null;
            Console.WriteLine("Reservation removed successfully.");
            return true;
        }
        else
        {
            Console.WriteLine("Reservation not found.");
            return false;
        }
    }

    public void DisplayWeeklySchedule()
    {
        Console.WriteLine("---Weekly Schedule---");
        foreach (var room in roomData.Room)
        {
            Console.WriteLine($"Room {room.roomName}, Capacity: {room.capacity}");
            foreach (var date in dateData)
            {
                int roomIndex = Array.FindIndex(roomData.Room, r => r.roomName == room.roomName);
                int dateIndex = Array.FindIndex(dateData, d => d.Date == date.Date);
                var reservation = reservations[roomIndex][dateIndex];

                string status = reservation == null ? "Empty" : $"Keeped by {reservation.reserverName}";
                Console.WriteLine($"Date: {date:dd/MM/yyyy}, Status: {status}");
            }
            Console.WriteLine(new string('-', 20));
        }
    }
}

class Program
{
    static void Main(string[] args)

    { 

        var reservation1 = new Reservation
        {
            room = new Room { roomName = "A-103" }, 
            date = DateTime.Today.AddDays(1),
            time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0),
            reserverName = "Alya"
        };
        
        var reservation2 = new Reservation
        {
            room = new Room { roomName = "A-107" },
            date = DateTime.Today.AddDays(2),
            time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 0, 0),
            reserverName = "Vehbi"
        };
        
        var reservation3 = new Reservation
        {
            room = new Room { roomName = "A-113" },
            date = DateTime.Today.AddDays(3),
            time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 0, 0),
            reserverName = "Sila"
        };
        
        string fileName = "./Data.json";
        string jsonString = File.ReadAllText(fileName);
        RoomData roomData = JsonSerializer.Deserialize<RoomData>(jsonString);

        DateTime[] dates = new DateTime[7];
        for (int i = 0; i < 7; i++)
        {
            dates[i] = DateTime.Today.AddDays(i);
        }

        ReservationHandler reservationHandler = new ReservationHandler(roomData, dates);
        reservationHandler.RemoveReservation("A-101", DateTime.Today.AddDays(1), "John Doe");
        reservationHandler.AddReservation(reservation1);
        reservationHandler.AddReservation(reservation2);
        reservationHandler.AddReservation(reservation3);


        reservationHandler.DisplayWeeklySchedule();

         Console.WriteLine("Give information for canceling reservation:");
        Console.Write("Room Number: ");
        string roomName = Console.ReadLine();
        Console.Write("Date (dd.mm.yyyy): ");
        DateTime date;
        while (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            Console.WriteLine("Invalid date format, Please use dd.MM.yyyy format:");
        }
        Console.Write("Visiter Full Name: ");
        string reserverName = Console.ReadLine();

        bool isRemoved = reservationHandler.RemoveReservation(roomName, date, reserverName);
        if (isRemoved)
        {
            Console.WriteLine("Reservation canceled successfully.");
        }
        else
        {
            Console.WriteLine("Reservation couldn't found or canceled.");
        }
        reservationHandler.DisplayWeeklySchedule();

    }

    
}