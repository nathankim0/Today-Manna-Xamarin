using Xamarin.CommunityToolkit.ObjectModel;
namespace TodaysManna.Models
{
    public class MannaContent : ObservableObject
    {
        private bool selected = false;
        public bool Selected { get => selected; set => SetProperty(ref selected, value); }

        public string BookAndJang { get; set; }
        public int Jeol { get; set; }
        public string MannaString { get; set; }
    }
}
