using System.Collections.ObjectModel;
using TodaysManna.Managers;
using TodaysManna.Models;
using Xamarin.CommunityToolkit.ObjectModel;

namespace TodaysManna.ViewModelV2
{
    public class HomePageViewModel : ObservableObject
    {
        public ObservableCollection<MannaContent> MannaContents => MannaDataManager.MannaContents;
        public string TodayMannaVerse => MannaDataManager.JsonMannaData.Verse;
    }
}
