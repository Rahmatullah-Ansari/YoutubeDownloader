using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDownloader.Utilities
{
    public static class YtConstant
    {
        public static string ApplicationName { get; set; } = "YouTube Downloader";
        public static string GetDownloadPath()
        {
            var downloadPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\YoutubeDownloader";
            DirectoryUtility.CreateDirectory(downloadPath);
            downloadPath = $"{downloadPath}\\Videos";
            DirectoryUtility.CreateDirectory(downloadPath);
            return downloadPath;
        }
    }
}
