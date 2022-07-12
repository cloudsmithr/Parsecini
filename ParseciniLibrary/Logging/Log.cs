using System;
using System.IO;
using System.Text;

namespace ParseciniLibrary.Logging
{
    public static class Log
    {
        public static string LogPath { get; set; }
        public static string LogFileName { get; set; }
        private static bool LogEnabled { get; set; }

        private static StringBuilder stringBuilder;
        private static DateTime futureTime;

        public static void BeginLogging()
        {
            stringBuilder = new StringBuilder();
            LogEnabled = true;
            futureTime = DateTime.Now.AddSeconds(5);

            CheckDirectory(LogPath);
        }

        public static void LogFile(string message)
        {
            if(LogEnabled)
            {
                if (!message.EndsWith(Environment.NewLine))
                    message += Environment.NewLine;

                stringBuilder.Append(message);
                if (DateTime.Now > futureTime)
                {
                    FlushLog();
                    futureTime = DateTime.Now.AddSeconds(5);
                }
            }
        }

        private static void FlushLog()
        {
            File.AppendAllText(Path.Join(LogPath, LogFileName), stringBuilder.ToString());
            stringBuilder.Clear();
        }

        public static void EndLogging()
        {
            FlushLog();
            LogEnabled = false;
        }

        public static bool IsEnabled()
        {
            return LogEnabled;
        }

        private static void CheckDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                try
                {
                    DirectoryInfo di = Directory.CreateDirectory(directoryPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
