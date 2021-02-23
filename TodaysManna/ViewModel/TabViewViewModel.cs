using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit;
namespace TodaysManna.ViewModel
{
    public class TabViewViewModel : INotifyPropertyChanged
    {
        
        public TabViewViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
