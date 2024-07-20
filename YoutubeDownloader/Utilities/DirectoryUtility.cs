using System.IO;

namespace YoutubeDownloader.Utilities
{
    public static class DirectoryUtility
    {
        public static void CreateDirectory(string DirectoryPath)
        {
            try
            {
                if(!Directory.Exists(DirectoryPath))
                    Directory.CreateDirectory(DirectoryPath);
            }
            catch { }
        }
    }
}
