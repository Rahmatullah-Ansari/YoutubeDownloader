using System.IO;

namespace YoutubeDownloader.Utilities
{
    public static class FileUtility
    {
        public static bool IsExistFile(string FilePath)
        {
            return !string.IsNullOrEmpty(FilePath) && File.Exists(FilePath);
        }
        public static void CreateFile(string FilePath)
        {
            if(!string.IsNullOrEmpty(FilePath)&&!File.Exists(FilePath))
            {
                File.Create(FilePath);
            }
        }
    }
}
