using System.Collections.Generic;
using YoutubeDownloader.Utilities;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloader.Model
{
    public class VideoInfoModel: BindableBase
    {
        private string _VideoUrlPath;
        private string _downloadPath=YtConstant.GetDownloadPath();
        private string _status;
        private bool _EnableProgress;
        private bool _EnabledUI=true;
        private string _title="Get Details";
        private string _VideoTitle;
        private bool _IsResetUI=true;
        private List<QualityDetails> _quality=new List<QualityDetails>();
        public List<QualityDetails> Quality
        {
            get=> _quality;
            set=>SetProperty(ref _quality, value);
        }
        public bool IsResetUI
        {
            get=>_IsResetUI;
            set=>SetProperty(ref _IsResetUI, value);
        }
        public string VideoTitle
        {
            get=>_VideoTitle;
            set=>SetProperty(ref _VideoTitle, value);
        }
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private List<MuxedStreamInfo> _streams= new List<MuxedStreamInfo>();
        public List<MuxedStreamInfo> Streams
        {
            get=> _streams;
            set=>SetProperty(ref _streams, value);
        }
        public bool EnabledUI
        {
            get => _EnabledUI;
            set=>SetProperty(ref _EnabledUI, value);
        }
        public bool EnableProgress
        {
            get => _EnableProgress;
            set=>SetProperty(ref _EnableProgress, value);
        }
        public string VideoUrlPath
        {
            get => _VideoUrlPath;
            set=>SetProperty(ref _VideoUrlPath, value);
        }
        public string DownloadLocation
        {
            get=>_downloadPath;
            set=>SetProperty(ref _downloadPath, value);
        }
        public string Status
        {
            get => _status; set => SetProperty(ref _status, value);
        }
    }
}
