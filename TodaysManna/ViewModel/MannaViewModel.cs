using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using TodaysManna.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using static TodaysManna.Models.BibleAtData;

namespace TodaysManna.ViewModel
{
    public class MannaViewModel : BaseViewModel
    {
        private readonly RestService _restService;
        
        private string _webBibleUrl = "https://www.bible.com/ko/bible/1/"; //https://www.bible.com/ko/bible/GEN.1.KJV
        private string _appBibleUrl = "youversion://bible?reference=";

        public string _completeUrl { get; set; } = "";
        public string _completeAppUrl { get; set; } = "";

        public static string todayMccheyneRange;

        private string _bib = "창";
        private int _jang= 1;
        
        private ObservableCollection<MannaContent> _mannaContents = new ObservableCollection<MannaContent>();
        public ObservableCollection<MannaContent> MannaContents { get => _mannaContents; set => SetProperty(ref _mannaContents, value); }

        private JsonMannaModel _jsonMannaData = new JsonMannaModel();
        public JsonMannaModel JsonMannaData { get => _jsonMannaData; set => SetProperty(ref _jsonMannaData, value); }

        private string _today = "";
        public string Today { get => _today; set => SetProperty(ref _today, value); }

        private string _allString = "";
        public string AllString { get => _allString; set => SetProperty(ref _allString, value); }

        private bool _isRefreshing;
        public bool IsRefreshing { get => _isRefreshing; set => SetProperty(ref _isRefreshing, value); }

        private string _mannaShareRange;
        public string MannaShareRange { get => _mannaShareRange; set => SetProperty(ref _mannaShareRange, value); }

        private string _mcShareRange;
        public string McShareRange { get => _mcShareRange; set => SetProperty(ref _mcShareRange, value); }

        public MannaViewModel()
        {
            Today = DateTime.Now.ToString("yyyy년 MM월 dd일 (ddd)");

            _restService = new RestService();

            Connectivity.ConnectivityChanged += OnConnectivityChanged;

            InitDate();
        }

        public async void RefreshManna()
        {
            IsRefreshing = true;

            await Task.WhenAll(ClearMannaDataAndGetManna());

            IsRefreshing = false;
        }

        private async Task ClearMannaDataAndGetManna()
        {
            if (MannaContents.Count != 0)
            {
                for (int i = 0; i < MannaContents.Count; i++)
                {
                    MannaContents.Remove(MannaContents[i]);
                }
            }

            await GetManna(DateTime.Now);
        }

        private async void InitDate()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                GetManna();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("인터넷 연결을 확인해주세요.", "", "확인");
            }

            var today = DateTime.Now.ToString("M-d");
            todayMccheyneRange = App.mccheyneRanges.Find(x => x.Date.Equals(today)).Range;
        }

        private async void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("인터넷 연결을 확인해주세요.", "", "확인");
                return;
            }
            else
            {
                GetManna();
            }
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
                await Application.Current.MainPage.DisplayAlert("만나 로드 실패", "", "확인");
            }
        }

        /// <summary>
        /// 서버에서 만나 데이터 가져오기
        /// </summary>
        /// <param name="dateTime">지정 날짜</param>
        public async 
        /// <summary>
        /// 서버에서 만나 데이터 가져오기
        /// </summary>
        /// <param name="dateTime">지정 날짜</param>
        Task
GetManna(DateTime dateTime)
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
                await Application.Current.MainPage.DisplayAlert("만나 로드 실패", "", "확인");
            }
        }

        private async Task GetJsonMannaAndSetContents(string endPoint)
        {
            JsonMannaData = new JsonMannaModel();
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

            _completeUrl = $"{_webBibleUrl}{redirectUrl}";
            _completeAppUrl = $"{_appBibleUrl}{redirectUrl}";

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
            catch(Exception e)
            {
                System.Diagnostics.Debug.Fail("# MannaViewModel SetMannaContents \n" + e.Message);
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
    }
}