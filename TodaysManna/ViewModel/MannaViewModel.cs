using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TodaysManna.ViewModel
{
    public class MannaViewModel : INotifyPropertyChanged
    {
        private readonly RestService _restService;

        public ObservableCollection<MannaContent> _mannaContents = new ObservableCollection<MannaContent>();
        public ObservableCollection<MannaContent> MannaContents { get { return _mannaContents; } }

        private MannaData _mannaData = new MannaData();
        public MannaData JsonMannaData
        {
            get => _mannaData;
            set
            {
                if (_mannaData != value)
                {
                    _mannaData = value;
                    OnPropertyChanged(nameof(JsonMannaData));
                }
            }
        }

        private string _today = "";
        public string Today
        {
            get=> _today;
            set
            {
                if (_today != value)
                {
                    _today = value;
                    OnPropertyChanged(nameof(Today));
                }
            }
        }

        private string _allString = "";
        public string AllString
        {
            get => _allString;
            set
            {
                if (_allString != value)
                {
                    _allString = value;
                    OnPropertyChanged(nameof(AllString));
                }
            }
        }

        //private bool _isRefreshing;
        //private bool IsRefreshing
        //{
        //    get => _isRefreshing;
        //    set
        //    {
        //        if (_isRefreshing != value)
        //        {
        //            _isRefreshing = value;
        //            OnPropertyChanged(nameof(IsRefreshing));
        //        }
        //    }
        //}

        //public ICommand RefreshCommand => new Command(async() =>
        //{
        //    MannaContents.Clear();
        //    await GetManna();
        //});

        public MannaViewModel()
        {
            Today = DateTime.Now.ToString("yyyy년 MM월 dd일 dddd");

            _restService = new RestService();
            _ = GetManna();
        }

        private async Task GetManna()
        {
           // IsRefreshing = true;

            JsonMannaData = new MannaData();
            JsonMannaData = await _restService.GetMannaDataAsync(Constants.MannaEndpoint);

            SetMannaContents();

           // IsRefreshing = false;
        }

        private void SetMannaContents()
        {
            var allContents = "";

            foreach (var node in JsonMannaData.Contents)
            {
                var onlyNum = int.Parse(Regex.Replace(node, @"\D", ""));
                var onlyString = Regex.Replace(node, @"\d", "").Substring(1);

                MannaContents.Add(new MannaContent
                {
                    Number = onlyNum,
                    MannaString = onlyString,
                });

                allContents += node + "\n\n";
            }
            AllString = allContents;
        }

      

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
