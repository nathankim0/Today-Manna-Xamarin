
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace TodaysManna.Views
{
    public partial class FatherPage : ContentPage
    {
        public FatherPage()
        {
            InitializeComponent();
            BindingContext = new FatherPageViewModel();
        }
    }

    public class FatherPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<FatherPageModel> fatherPictures = new ObservableCollection<FatherPageModel>();
        public ObservableCollection<FatherPageModel> FatherPictures
        {
            get => fatherPictures;
            set
            {
                if (fatherPictures != value)
                {
                    fatherPictures = value;
                    OnPropertyChanged(nameof(fatherPictures));
                }
            }
        }

        public FatherPageViewModel()
        {
            for (int i = 0; i < 8; i++)
            {
                FatherPictures.Add(new FatherPageModel
                {
                    FaImage = $"fa{i}"
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FatherPageModel
    {
        public ImageSource FaImage { get; set; }
    }
}
