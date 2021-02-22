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
using TodaysManna.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using static TodaysManna.Models.BibleAtData;

namespace TodaysManna.ViewModel
{
    public class MannaViewModel : INotifyPropertyChanged
    {

        private readonly RestService _restService;
        
        private string bibleUrl = "https://www.bible.com/ko/bible/1/";
        //  private const string sample = "https://www.bible.com/ko/bible/GEN.1.KJV";
        private string appBibleUrl = "youversion://bible?reference=";

        public string _completeUrl { get; set; } = "";
        public string _completeAppUrl { get; set; } = "";

        public static string todayMccheyneRange;

        private string _bib = "창";
        private int _jang= 1;
        

        private ObservableCollection<MannaContent> _mannaContents = new ObservableCollection<MannaContent>();
        public ObservableCollection<MannaContent> MannaContents
        {
            get =>_mannaContents;
            set
            {
                if (_mannaContents != value)
                {
                    _mannaContents = value;
                    OnPropertyChanged(nameof(MannaContents));
                }
            }
        }

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
        public bool IsRefreshing
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

        private string _mannaShareRange;
        public string MannaShareRange
        {
            get => _mannaShareRange;
            set
            {
                if (_mannaShareRange != value)
                {
                    _mannaShareRange = value;
                    OnPropertyChanged(nameof(MannaShareRange));
                }
            }
        }

        private string _mcShareRange;
        public string McShareRange
        {
            get => _mcShareRange;
            set
            {
                if (_mcShareRange != value)
                {
                    _mcShareRange = value;
                    OnPropertyChanged(nameof(McShareRange));
                }
            }
        }

        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            MannaContents.Clear();
            GetManna();

            IsRefreshing = false;
        });

        public MannaViewModel()
        {
            Today = DateTime.Now.ToString("yyyy년 MM월 dd일 (ddd)");

            _restService = new RestService();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                GetManna();
            }
            else
            {
                App.ShowErrorPopup("인터넷 연결을 확인해주세요.");
            }

            var today = DateTime.Now.ToString("M-d");
            todayMccheyneRange = App.mccheyneRanges.Find(x => x.Date.Equals(today)).Range;
        }

        private async void GetManna()
        {
            try
            {
                await GetJsonMannaAndSetContents(Constants.MannaEndpoint);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.Fail("# MannaViewModel GetManna() \n" + e.Message);
                App.ShowErrorPopup("만나 불러오기 오류");
            }
        }

        /// <summary>
        /// 서버에서 만나 데이터 가져오기
        /// </summary>
        /// <param name="dateTime">지정 날짜</param>
        public async void GetManna(DateTime dateTime)
        {
            Today = dateTime.ToString("yyyy년 MM월 dd일 (ddd)");

            var newDateString = dateTime.ToString("yyyy-MM-dd");
            var endPoint = Constants.MannaEndpoint + newDateString;

            var findMccheyneDate = dateTime.ToString("M-d");
            todayMccheyneRange = App.mccheyneRanges.Find(x => x.Date.Equals(findMccheyneDate)).Range;

            try
            {
                await GetJsonMannaAndSetContents(endPoint);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Fail("# MannaViewModel GetManna(DateTime dateTime) \n" + e.Message);
                App.ShowErrorPopup("만나 불러오기 오류");
            }
        }

        private async Task GetJsonMannaAndSetContents(string endPoint)
        {
            JsonMannaData = new MannaData();
            JsonMannaData = await _restService.GetMannaDataAsync(endPoint);

            string tmpBibleAt = "창1";
            string tmpVerseNumRange = "1-10";

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("***** GetJsonMannaAndSetContents *****");

            try
            {
                tmpBibleAt = JsonMannaData.Verse.Substring(0, JsonMannaData.Verse.IndexOf(":"));
                Console.WriteLine("tmpBibleAt: " + tmpBibleAt);
            }
            catch (Exception e)
            {
                tmpBibleAt = "창1";
                System.Diagnostics.Debug.Fail("# MannaViewModel GetJsonMannaAndSetContents tmpBibleAt \n" + e.Message);
            }

            try
            {
                tmpVerseNumRange = Regex.Replace(JsonMannaData.Verse.Substring(JsonMannaData.Verse.IndexOf(":") + 1), "~", "-");
                Console.WriteLine("tmpVerseNumRange: " + tmpVerseNumRange);
            }
            catch (Exception e)
            {
                tmpVerseNumRange = "1-10";
                System.Diagnostics.Debug.Fail("# MannaViewModel GetJsonMannaAndSetContents tmpVerseNumRange \n" + e.Message);
            }

            _bib = Regex.Replace(tmpBibleAt, @"\d", "");
            _jang = int.Parse(Regex.Replace(tmpBibleAt, @"\D", ""));

            var _bibles = new List<Bible>();
            _bibles = GetJsonBible();

            var engBib = _bibles.Find(x => x.Kor.Equals(_bib));

            var redirectUrl = $"{engBib.Eng}.{_jang}.{tmpVerseNumRange}.NKJV";

            _completeUrl = $"{bibleUrl}{redirectUrl}";
            _completeAppUrl = $"{appBibleUrl}{redirectUrl}";

            SetMannaContents();

            MannaShareRange = $"만나: {JsonMannaData.Verse}";
            McShareRange = $"맥체인: {todayMccheyneRange}";

            Console.WriteLine("_bib: " + _bib);
            Console.WriteLine("_jang: " + _jang);
            Console.WriteLine("_jang: " + engBib);
            Console.WriteLine("redirectUrl: " + redirectUrl);
            Console.WriteLine("_completeUrl: " + _completeUrl);
            Console.WriteLine("_completeAppUrl: " + _completeAppUrl);
            Console.WriteLine("MannaShareRange: " + MannaShareRange);
            Console.WriteLine("McShareRange: " + McShareRange);

            Console.WriteLine("--------------------------------------------------------");
        }

        private List<Bible> GetJsonBible()
        {
            var jsonFileName = "BibleAt.json";
            var ObjContactList = new BibleList();

            var assembly = typeof(MannaPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Datas.{jsonFileName}");
            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                ObjContactList = JsonConvert.DeserializeObject<BibleList>(jsonString);
            }
            
            return ObjContactList.Bibles;
        }

        private void SetMannaContents()
        {
            try
            {
                MannaContents.Clear();
            }
            catch
            {

            }
            var allContents = "";
            var bookAndJang = JsonMannaData.Verse.Substring(0, JsonMannaData.Verse.IndexOf(":")+1);

            foreach (var node in JsonMannaData.Contents)
            {
                var onlyNum = int.Parse(Regex.Replace(node, @"\D", ""));
                var verse = bookAndJang + onlyNum;
                var onlyString = Regex.Replace(node, @"\d", "").Substring(1);

                MannaContents.Add(new MannaContent
                {
                    Verse = verse,
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