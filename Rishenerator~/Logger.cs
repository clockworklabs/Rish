using System;
using System.IO;

namespace Rishenerator
{
    public static class Logger
    {
        private static readonly string FilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\RisheneratorLogs.txt";

        public static void Log(string message)
        {
            var writer = File.AppendText(FilePath);
            
            try
            {
                writer.WriteLine(message);
            }
            finally
            {
                writer.Close();
            }
        }
    }
}