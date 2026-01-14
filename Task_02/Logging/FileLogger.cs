using System;
using System.IO;

namespace Task_02.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(string message)
        {
            File.AppendAllText(_filePath, message + Environment.NewLine);
        }
    }
}
