using System.ComponentModel;
using System.Windows.Input;
namespace TodaysManna
{
    class MannaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ReloadCommand { set; get; }
        public ICommand ShareCommand { set; get; }

        private string _todayString;
        private string _titleString;
        private string _allString;
        private bool _isReloading;

        public string TodayString
        {
            get => _todayString;
            set
            {
                if (_todayString == value) return;
                _todayString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TodayString"));
            }
        }

        public string TitleString
        {
            get => _titleString;
            set
            {
                if (_titleString == value) return;
                _titleString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TitleString"));
            }
        }

        public string AllString
        {
            get => _allString;
            set
            {
                if (_allString == value) return;
                _allString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllString"));
            }
        }

        public bool IsReloading
        {
            get => _isReloading;
            set
            {
                if (_isReloading == value) return;
                _isReloading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsReloading"));
            }
        }
    }
}