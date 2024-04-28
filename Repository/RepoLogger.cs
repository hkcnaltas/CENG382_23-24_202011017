using System;
using System.IO;
using System.Text.Json;
using System.Globalization;
namespace ReservationSystem
{
    public class FileLogger : IRepoLogger  
    {
        private readonly string logFilePath;

        public FileLogger(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }

        public void Log(LogRecord log)
        {
            var json = JsonSerializer.Serialize(log);
            File.AppendAllText(logFilePath, json + Environment.NewLine);
        }
    }
}