using YoutubeDownloader.Utilities;

namespace YoutubeDownloader.Model
{
    public class QualityDetails:BindableBase
    {
        private bool _isSelected;
        private string _title;
        public bool IsSelected
        {
            get => _isSelected;
            set=>SetProperty(ref _isSelected, value);
        }
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
