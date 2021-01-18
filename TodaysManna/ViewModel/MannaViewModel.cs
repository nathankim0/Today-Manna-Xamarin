using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TodaysManna.Models;
using Xamarin.Forms;
using static TodaysManna.Models.BibleAtData;

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

        private bool _isRefreshing;
        private bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        private int _jang;
        private string _bib;

        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            MannaContents.Clear();
            GetManna();

            IsRefreshing = false;
        });

        public MannaViewModel()
        {
            Today = DateTime.Now.ToString("yyyy년 MM월 dd일 dddd");

            _restService = new RestService();
            GetManna();
        }
        private string bibleUrl = "https://www.bible.com/ko/bible/";
        private const string sample = "https://www.bible.com/ko/bible/GEN.1.KJV";


        private async void GetManna()
        {

            JsonMannaData = new MannaData();
            JsonMannaData = await _restService.GetMannaDataAsync(Constants.MannaEndpoint);

            var tmpBibleAt = JsonMannaData.Verse.Substring(0, JsonMannaData.Verse.IndexOf(":"));

            _bib = Regex.Replace(tmpBibleAt, @"\d", "");
            _jang = int.Parse(Regex.Replace(tmpBibleAt, @"\D", ""));

            var _bibles = new List<Bible>();
            _bibles = GetJsonBible();

            var engBib = _bibles.Find(x => x.Kor.Equals(_bib));

            var _completeUrl = $"{bibleUrl}{engBib.Eng}.{_jang}.NKJV";

            System.Diagnostics.Debug.WriteLine($"**** url : {_completeUrl}");

            SetMannaContents();
        }


        private List<Bible> GetJsonBible()
        {
            string jsonFileName = "BibleAt.json";
            BibleList ObjContactList = new BibleList();


            var assembly = typeof(MannaPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();

                //Converting JSON Array Objects into generic list    
                ObjContactList = JsonConvert.DeserializeObject<BibleList>(jsonString);
            }

            return ObjContactList.Bibles;
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
