using Xamarin.Forms;
using Xamarin.Essentials;

namespace TodaysManna.ViewModel
{
    public class SettingViewModel : PageBaseViewModel
    {
        private string _version;
        public string Version { get => _version; set => SetProperty(ref _version, value); }

        private string _isDatabase;
        public string IsDatabase { get => _isDatabase; set => SetProperty(ref _isDatabase, value); }

        public SettingViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "설정";

            Version = VersionTracking.CurrentVersion;
        }
    }
}
