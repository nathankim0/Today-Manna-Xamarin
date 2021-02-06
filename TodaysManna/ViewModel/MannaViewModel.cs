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
using TodaysManna.Views;
using Xamarin.Forms;
using static TodaysManna.Models.BibleAtData;
using static TodaysManna.Models.MccheyneRangeData;

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


        private int _jang;
        private string _bib;

        public static List<MccheyneRange> mccheyneRanges;

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

        private string _shareRange;
        public string ShareRange
        {
            get => _shareRange;
            set
            {
                if (_shareRange != value)
                {
                    _shareRange = value;
                    OnPropertyChanged(nameof(ShareRange));
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

            try
            {
                GetManna();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("GetManna() Error");
            }
            mccheyneRanges = new List<MccheyneRange>();
            try
            {
                mccheyneRanges = GetJsonMccheyneRange();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("GetJsonMccheyneRange() Error");
            }
            var today = DateTime.Now.ToString("M-d");
            todayMccheyneRange = mccheyneRanges.Find(x => x.Date.Equals(today)).Range;
        }

        private async void GetManna()
        {
            var endporint = Constants.MannaEndpoint;

            JsonMannaData = new MannaData();
            JsonMannaData = await _restService.GetMannaDataAsync(endporint);

            var tmpBibleAt = JsonMannaData.Verse.Substring(0, JsonMannaData.Verse.IndexOf(":"));
            var tmpVerseNumRange = Regex.Replace(JsonMannaData.Verse.Substring(JsonMannaData.Verse.IndexOf(":")+1), "~", "-");

            _bib = Regex.Replace(tmpBibleAt, @"\d", "");
            _jang = int.Parse(Regex.Replace(tmpBibleAt, @"\D", ""));

            var _bibles = new List<Bible>();
            _bibles = GetJsonBible();

            var engBib = _bibles.Find(x => x.Kor.Equals(_bib));

            var redirectUrl= $"{engBib.Eng}.{_jang}.{tmpVerseNumRange}.NKJV";

            _completeUrl = $"{bibleUrl}{redirectUrl}";
            _completeAppUrl= $"{appBibleUrl}{redirectUrl}";

            SetMannaContents();
            ShareRange = $"만나: {JsonMannaData.Verse}\n맥체인: {todayMccheyneRange}";
        }

        /// <summary>
        /// 서버에서 만나 데이터 가져오기
        /// </summary>
        /// <param name="dateTime">지정 날짜</param>
        public async void GetManna(DateTime dateTime)
        {
            Today = dateTime.ToString("yyyy년 MM월 dd일 (ddd)");

            var newDateString = dateTime.ToString("yyyy-MM-dd");
            var endporint = Constants.MannaEndpoint+ newDateString;

            var findMccheyneDate = dateTime.ToString("M-d");
            todayMccheyneRange = mccheyneRanges.Find(x => x.Date.Equals(findMccheyneDate)).Range;

            JsonMannaData = new MannaData();
            JsonMannaData = await _restService.GetMannaDataAsync(endporint);

            var tmpBibleAt = JsonMannaData.Verse.Substring(0, JsonMannaData.Verse.IndexOf(":"));
            var tmpVerseNumRange = Regex.Replace(JsonMannaData.Verse.Substring(JsonMannaData.Verse.IndexOf(":") + 1), "~", "-");

            _bib = Regex.Replace(tmpBibleAt, @"\d", "");
            _jang = int.Parse(Regex.Replace(tmpBibleAt, @"\D", ""));

            var _bibles = new List<Bible>();
            _bibles = GetJsonBible();

            var engBib = _bibles.Find(x => x.Kor.Equals(_bib));

            var redirectUrl = $"{engBib.Eng}.{_jang}.{tmpVerseNumRange}.NKJV";

            _completeUrl = $"{bibleUrl}{redirectUrl}";
            _completeAppUrl = $"{appBibleUrl}{redirectUrl}";

            SetMannaContents();
            ShareRange = $"만나: {JsonMannaData.Verse}\n맥체인: {todayMccheyneRange}";
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
            MannaContents.Clear();
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

        private List<MccheyneRange> GetJsonMccheyneRange()
        {
            string jsonFileName = "MccheyneRange.json";
            MccheyneRangeList ObjContactList = new MccheyneRangeList();


            var assembly = typeof(MannaPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();

                //Converting JSON Array Objects into generic list    
                ObjContactList = JsonConvert.DeserializeObject<MccheyneRangeList>(jsonString);
            }

            return ObjContactList.Ranges;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


//private void FormattingJsonFile()
//{
//    var jsonArray = new JArray();

//    foreach (var node in mccheyneRanges)
//    {
//        var json = new JObject();
//        json.Add($"date", node.Date);

//        char[] delimiterChars = { ',' };
//        var range = node.Range;
//        string[] words = range.Split(delimiterChars);

//        int i = 1;
//        foreach (var word in words)
//        {
//            json.Add($"range{i}", word);
//            i++;
//        }
//        jsonArray.Add(json);
//    }
//    string str_json = JsonConvert.SerializeObject(jsonArray);
//    System.Diagnostics.Debug.WriteLine(str_json);
//    JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
//    File.WriteAllText(@"/Users/jinyeob/Downloads/path2.json", str_json.ToString());

//}