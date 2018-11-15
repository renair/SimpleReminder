using System;
using System.IO;
using Tools;

namespace SimpleReminder.Tools
{
    internal static class FileFolderHelper
    {
        private static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        internal static readonly string ClientFolderPath =  Path.Combine(AppDataPath, "SimpleReminder");

        internal static readonly string LogFolderPath = Path.Combine(ClientFolderPath, "Log");

        internal static readonly string LogFilepath = Path.Combine(LogFolderPath, "Log_" + DateTime.Now.ToShortDateString() + ".txt");

        internal static readonly string StorageFilePath = Path.Combine(ClientFolderPath, "DataStorage.bin");

        internal static readonly string LastUserFilePath = Path.Combine(ClientFolderPath, "LastUser.bin");

        internal static void CheckAndCreateFile(string filePath)
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
            catch (Exception)
            {

                throw;
            }
        }

        internal static void RemoveLastUserCache()
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