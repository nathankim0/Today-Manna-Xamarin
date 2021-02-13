﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using TodaysManna.Models;
using TodaysManna.Popups;
using TodaysManna.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using static TodaysManna.Models.BibleAtData;
using static TodaysManna.Models.MccheyneRangeData;

namespace TodaysManna.ViewModel
{
    public class MannaViewModel : INotifyPropertyChanged
    {
        private ErrorPopup errorPopup;

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
                try
                {
                    GetManna();
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.Fail("GetManna() \n" + e.Message);
                    ShowErrorPopup("만나 불러오기 오류");
                }
            }
            else
            {
                ShowErrorPopup("만나 불러오기 오류");
            }
            
            mccheyneRanges = new List<MccheyneRange>();
            try
            {
                mccheyneRanges = GetJsonMccheyneRange();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.Fail("GetJsonMccheyneRange() \n" + e.Message);
                ShowErrorPopup("맥체인 불러오기 오류");
            }
            var today = DateTime.Now.ToString("M-d");
            todayMccheyneRange = mccheyneRanges.Find(x => x.Date.Equals(today)).Range;
        }

        private async void ShowErrorPopup(string message)
        {
            errorPopup = new ErrorPopup(message);
            await PopupNavigation.Instance.PushAsync(errorPopup);
        }
        private async void GetManna()
        {
            const string endPoint = Constants.MannaEndpoint;

            JsonMannaData = new MannaData();
            JsonMannaData = await _restService.GetMannaDataAsync(endPoint);

            try
            {
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

                MannaShareRange = $"만나: {JsonMannaData.Verse}";
                McShareRange = $"맥체인: {todayMccheyneRange}";
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.Fail("GetManna() \n" + e.Message);
                ShowErrorPopup("만나 불러오기 오류");
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
            var endPoint = Constants.MannaEndpoint+ newDateString;

            var findMccheyneDate = dateTime.ToString("M-d");
            todayMccheyneRange = mccheyneRanges.Find(x => x.Date.Equals(findMccheyneDate)).Range;

            JsonMannaData = new MannaData();
            JsonMannaData = await _restService.GetMannaDataAsync(endPoint);

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

            MannaShareRange = $"만나: {JsonMannaData.Verse}";
            McShareRange = $"맥체인: {todayMccheyneRange}";
        }


        private List<Bible> GetJsonBible()
        {
            var jsonFileName = "BibleAt.json";
            var ObjContactList = new BibleList();


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

        private List<MccheyneRange> GetJsonMccheyneRange()
        {
            var jsonFileName = "MccheyneRange.json";
            var ObjContactList = new MccheyneRangeList();


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