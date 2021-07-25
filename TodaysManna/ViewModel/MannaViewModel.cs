using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TodaysManna.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using TodaysManna.Constants;
using TodaysManna.ExtensionMethods;
using System.Diagnostics;

namespace TodaysManna.ViewModel
{
    public class MannaViewModel : PageBaseViewModel
    {
        private readonly RestService _restService;

        private string _webBibleUrl = "https://www.bible.com/ko/bible/1/"; //https://www.bible.com/ko/bible/GEN.1.KJV
        private string _appBibleUrl = "youversion://bible?reference=";

        public string _completeUrl { get; set; } = "";
        public string _completeAppUrl { get; set; } = "";

        public static string todayMccheyneRange;

        private string _bookKor = "창";
        private int _jang = 1;

        private ObservableCollection<MannaContent> _mannaContents = new ObservableCollection<MannaContent>();
        public ObservableCollection<MannaContent> MannaContents { get => _mannaContents; set => SetProperty(ref _mannaContents, value); }

        private JsonMannaModel _jsonMannaData = new JsonMannaModel();
        public JsonMannaModel JsonMannaData { get => _jsonMannaData; set => SetProperty(ref _jsonMannaData, value); }

        private string _today = "";
        public string Today { get => _today; set => SetProperty(ref _today, value); }

        private string _allString = "";
        public string AllString { get => _allString; set => SetProperty(ref _allString, value); }

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

            var rangeOfDate = App.mccheyneRanges.Find(x => x.Date.Equals(today));
            todayMccheyneRange = $"{rangeOfDate.Range1} {rangeOfDate.Range2} {rangeOfDate.Range3} {rangeOfDate.Range4} {rangeOfDate.Range5}";
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
                await GetJsonMannaAndSetContents(Rests.MannaEndpoint);
            }
            catch (Exception e)
            {
                Debug.Fail("# MannaViewModel GetManna() \n" + e.Message);
                await Application.Current.MainPage.DisplayAlert("만나 로드 실패", "", "확인");
            }
        }

        public async Task GetManna(DateTime dateTime)
        {
            Today = dateTime.ToString("yyyy년 MM월 dd일 (ddd)");

            var newDateString = dateTime.ToString("yyyy-MM-dd");
            var endPoint = Rests.MannaEndpoint + newDateString;

            var findMccheyneDate = dateTime.ToString("M-d");
            var rangeOfDate = App.mccheyneRanges.Find(x => x.Date.Equals(findMccheyneDate));
            todayMccheyneRange = $"{rangeOfDate.Range1} {rangeOfDate.Range2} {rangeOfDate.Range3} {rangeOfDate.Range4} {rangeOfDate.Range5}";

            try
            {
                await GetJsonMannaAndSetContents(endPoint);
            }
            catch (Exception e)
            {
                Debug.Fail("# MannaViewModel GetManna(DateTime dateTime) \n" + e.Message);
                await Application.Current.MainPage.DisplayAlert("만나 로드 실패", "", "확인");
            }
        }

        private async Task GetJsonMannaAndSetContents(string endPoint)
        {
            JsonMannaData = new JsonMannaModel();
            JsonMannaData = await _restService.GetMannaDataAsync(endPoint);

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("***** GetJsonMannaAndSetContents() *****\n");

            var tmpBibleAt = "창1";
            try
            {
                tmpBibleAt = JsonMannaData.Verse.Substring(0, JsonMannaData.Verse.IndexOf(":"));
            }
            catch (Exception e)
            {
                Debug.Fail(this + e.Message);
                Console.WriteLine("--------------------------------------------------------\n");
            }

            var tmpVerseNumRange= "1-10";
            try
            {
                tmpVerseNumRange = Regex.Replace(JsonMannaData.Verse.Substring(JsonMannaData.Verse.IndexOf(":") + 1), "~", "-");
            }
            catch (Exception e)
            {
                Debug.Fail(this + e.Message);
                Console.WriteLine("--------------------------------------------------------\n");
            }

            _bookKor = Regex.Replace(tmpBibleAt, @"\d", "");
            _jang = int.Parse(Regex.Replace(tmpBibleAt, @"\D", ""));

            var redirectUrl = $"{_bookKor.BibleBookKorToEng()}.{_jang}.{tmpVerseNumRange}.NKJV";

            _completeUrl = $"{_webBibleUrl}{redirectUrl}";
            _completeAppUrl = $"{_appBibleUrl}{redirectUrl}";

            SetMannaContents();

            MannaShareRange = $"만나: {JsonMannaData.Verse}";
            McShareRange = $"맥체인: {todayMccheyneRange}";

            Console.WriteLine("_bib: " + _bookKor);
            Console.WriteLine("_jang: " + _jang);
            Console.WriteLine("redirectUrl: " + redirectUrl);
            Console.WriteLine("_completeUrl: " + _completeUrl);
            Console.WriteLine("_completeAppUrl: " + _completeAppUrl);
            Console.WriteLine("MannaShareRange: " + MannaShareRange);
            Console.WriteLine("McShareRange: " + McShareRange);
            Console.WriteLine("--------------------------------------------------------\n");
        }

        private void SetMannaContents()
        {
            try
            {
                MannaContents.Clear();
            }
            catch (Exception e)
            {
                Debug.Fail("# MannaViewModel SetMannaContents \n" + e.Message);
            }

            var allContents = "";
            var bookAndJang = JsonMannaData.Verse.Substring(0, JsonMannaData.Verse.IndexOf(":") + 1);

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