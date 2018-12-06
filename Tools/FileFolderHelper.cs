using System;
using System.IO;

namespace Tools
{
    public static class FileFolderHelper
    {
        private static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static readonly string ClientFolderPath =  Path.Combine(AppDataPath, "SimpleReminder");

        public static readonly string LogFolderPath = Path.Combine(ClientFolderPath, "Log");

        public static readonly string LogFilepath = Path.Combine(LogFolderPath, "Log_" + DateTime.Now.ToShortDateString() + ".txt");

        public static readonly string LastUserFilePath = Path.Combine(ClientFolderPath, "LastUser.bin");

        public static void CheckAndCreateFile(string filePath)
        {
            try
            {
                FileInfo file = new FileInfo(filePath);
                if (file.Directory != null && !file.Directory.Exists)
                {
                    file.Directory.Create();
                }
                if (!file.Exists)
                {
                    file.Create().Close();
                }
            }
            // ReSharper disable once RedundantCatchClause
            catch (Exception)
            {
                throw;
            }
        }

        public static void RemoveLastUserCache()
        {
            try
            {
                new FileInfo(LastUserFilePath).Delete();
            }
            catch (Exception e)
            {
                Logger.Log("Can't remove las logged in user.", e);
            }
        }
    }
}