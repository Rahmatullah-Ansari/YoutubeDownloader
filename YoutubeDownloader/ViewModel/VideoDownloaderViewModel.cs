using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using YoutubeDownloader.Model;
using YoutubeDownloader.Utilities;
using YoutubeExplode;

namespace YoutubeDownloader.ViewModel
{
    public class VideoDownloaderViewModel
    {
        private static VideoDownloaderViewModel _viewModel;
        private VideoInfoModel InfoModel=new VideoInfoModel();
        private ObservableCollection<VideoInfoModel> _VideosCollections = new ObservableCollection<VideoInfoModel>();
        public ICommand StartDownload {  get; set; }
        public ICommand BrowserFolder { get; set; }
        public static VideoDownloaderViewModel GetInstance
        {
            get
            {
                return _viewModel ?? (_viewModel = new VideoDownloaderViewModel());
            }
        }
        public VideoDownloaderViewModel()
        {
            StartDownload = new BaseCommand<object>(CanDownload, DownloadVideoExecute);
            BrowserFolder = new BaseCommand<object>(sender => true, BrowserFolderForDownloadPath);
        }

        private bool CanDownload(object arg)
        {
            //return Model != null && !string.IsNullOrEmpty(Model.VideoUrlPath) && !string.IsNullOrEmpty(Model.DownloadLocation);
            return true;
        }

        private void BrowserFolderForDownloadPath(object obj)
        {
            try
            {
                var dialog = new FolderBrowserDialog();
                var result = dialog.ShowDialog();
                if(result == DialogResult.OK)
                {
                    if(dialog.SelectedPath != Model.DownloadLocation)
                        Model.DownloadLocation = dialog.SelectedPath;
                }
            }
            catch (Exception ex) { }
        }

        private async void DownloadVideoExecute(object obj)
        {
            await DownloadVideo(obj);
        }

        private async Task DownloadVideo(object obj)
        {
            try
            {
                var button = obj as Button;
                Model.EnabledUI = false;
                Model.EnableProgress = true;
                if(Model.Title == "Get Details")
                {
                    Model.IsResetUI = false;
                    Model.Status = $"Getting Video Details...";
                    var youtube = new YoutubeClient();
                    var video = await youtube.Videos.GetAsync(Model.VideoUrlPath);
                    // Sanitize the video title to remove invalid characters from the file name
                    Model.VideoTitle = video.Title;
                    // Get all available muxed streams
                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
                    Model.Streams = streamManifest.GetMuxedStreams().OrderByDescending(s => s.VideoQuality).ToList();
                    if(Model!=null && Model.Streams != null &&  Model.Streams.Count > 0)
                    {
                        var quality = Model.Streams.Select(s => s.VideoQuality.Label).ToList();
                        if(quality != null && quality.Count > 0)
                        {
                            quality.ForEach(x =>
                            {
                                Model.Quality.Add(new QualityDetails
                                {
                                    IsSelected = false,
                                    Title = x
                                });
                            });
                        }   
                    }
                    Model.Title = "Download";
                    Model.Status = "Successfully Got The Video Details . Please Select Video Quality And Hit Download To Proceed Download";
                    return;
                }
                else
                {
                    Model.IsResetUI = true;
                    if (Model.Streams != null && Model.Streams.Any())
                    {
                        Model.Status = $"Downlaoding,Please Wait....";
                        var SelectedQuality = Model.Quality.FindAll(x=>x.IsSelected).ToList();
                        if(SelectedQuality !=null && SelectedQuality.Count > 0)
                        {
                            Model.IsResetUI = true;
                            foreach(var videoData in SelectedQuality)
                            {
                                var videoInfo = Model.Streams.FirstOrDefault(z => z.VideoQuality.Label == videoData?.Title?.Trim());
                                if (videoInfo != null)
                                {
                                    var streamInfo = videoInfo;
                                    var httpClient = new HttpClient();
                                    var stream = await httpClient.GetStreamAsync(streamInfo.Url);
                                    var datetime = DateTime.Now;
                                    var sanitizedTitle = string.Join("_", Model.VideoTitle.Split(Path.GetInvalidFileNameChars()));
                                    string outputFilePath = Path.Combine(Model.DownloadLocation, $"{sanitizedTitle}_{streamInfo.VideoQuality}.{streamInfo.Container}");
                                    if (FileUtility.IsExistFile(outputFilePath))
                                    {
                                        Model.EnableProgress = false;
                                        Model.Status = $"This Video Is Already Downloaded At {outputFilePath}";
                                    }
                                    else
                                    {
                                        var outputStream = File.Create(outputFilePath);
                                        await stream.CopyToAsync(outputStream);
                                        Model.Status = $"Downloaded Successfully At {outputFilePath}";
                                        if (videoData != SelectedQuality.LastOrDefault())
                                            await Task.Delay(2000);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Model.Status = $"Please Selected Atleast One Video Quality To Download!";
                            Model.IsResetUI = false;
                        }
                    }
                    else
                    {
                        Model.Status = $"No suitable video stream found for {Model.VideoTitle}.";
                    }
                }
            }
            catch (Exception ex) {
                Model.Status = ex.Message;
            }
            finally
            {
                await ResetUI();
            }   
        }

        private async Task ResetUI()
        {
            if(Model != null && Model.IsResetUI)
            {
                await Task.Delay(5000);
                Model.Status = $"Please Wait While Updating UI After 5 Seconds";
                await Task.Delay(5000);
                Model.Status = string.Empty;
                Model.VideoUrlPath = string.Empty;
                Model.VideoTitle = string.Empty;
                Model.IsResetUI = false;
                Model.Streams = new System.Collections.Generic.List<YoutubeExplode.Videos.Streams.MuxedStreamInfo>();
                Model.Quality = new System.Collections.Generic.List<QualityDetails>();
                Model.Title = "Get Details";
            }
            Model.EnabledUI = true;
            Model.EnableProgress = false;
        }

        public VideoInfoModel Model => SelectedVideo;
        public VideoInfoModel SelectedVideo
        {
            get => InfoModel;
            set => InfoModel = value;
        }
        public ObservableCollection<VideoInfoModel> VideosCollections
        {
            get=> _VideosCollections;
            set => _VideosCollections = value;
        }
    }
}
