using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Xamarin.Forms;
using static TodaysManna.Models.BibleAtData;
using static TodaysManna.Models.MccheyneRangeData;

namespace TodaysManna.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly RestService _restService;
        private string bibleUrl = "https://www.bible.com/ko/bible/1/";
        private string appBibleUrl = "youversion://bible?reference=";
        public string _completeUrl { get; set; } = "";
        public string _completeAppUrl { get; set; } = "";
        private static string todayMccheyneRange;
        private int _jang;
        private string _bib;
        public ObservableCollection<MannaContent> _mannaContents = new ObservableCollection<MannaContent>();
        private MannaData _mannaData = new MannaData();
        private string _today = "";
        private string _allString = "";
        private bool _isRefreshing;
        private string _shareRange;

        public MainPage()
        {
            InitializeComponent();

            _today = DateTime.Now.ToString("yyyy년 MM월 dd일 dddd");

            _restService = new RestService();
            GetManna();

            var ranges = GetJsonMccheyneRange();
            var today = DateTime.Now.ToString("M-d");
            todayMccheyneRange = ranges.Find(x => x.Date.Equals(today)).Range;
        }


        private async void GetManna()
        {
            _mannaData = new MannaData();
            _mannaData = await _restService.GetMannaDataAsync(Constants.MannaEndpoint);

            var tmpBibleAt = _mannaData.Verse.Substring(0, _mannaData.Verse.IndexOf(":"));
            var tmpVerseNumRange = Regex.Replace(_mannaData.Verse.Substring(_mannaData.Verse.IndexOf(":") + 1), "~", "-");

            _bib = Regex.Replace(tmpBibleAt, @"\d", "");
            _jang = int.Parse(Regex.Replace(tmpBibleAt, @"\D", ""));

            var _bibles = new List<Bible>();
            _bibles = GetJsonBible();

            var engBib = _bibles.Find(x => x.Kor.Equals(_bib));

            var redirectUrl = $"{engBib.Eng}.{_jang}.{tmpVerseNumRange}.NKJV";

            _completeUrl = $"{bibleUrl}{redirectUrl}";
            _completeAppUrl = $"{appBibleUrl}{redirectUrl}";

            SetMannaContents();
            _shareRange = $"만나: {_mannaData.Verse}\n맥체인: {todayMccheyneRange}";
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

            foreach (var node in _mannaData.Contents)
            {
                var onlyNum = int.Parse(Regex.Replace(node, @"\D", ""));
                var onlyString = Regex.Replace(node, @"\d", "").Substring(1);

                _mannaContents.Add(new MannaContent
                {
                    Number = onlyNum,
                    MannaString = onlyString,
                });
                //flexLayout.Children.Add(new Label { BackgroundColor = Color.Red, Text = onlyNum.ToString() });
                //flexLayout.Children.Add(new Label { BackgroundColor = Color.Blue, Text = onlyString });
                System.Diagnostics.Debug.Write(onlyNum);
                System.Diagnostics.Debug.Write(onlyString);


                //var grid = new Grid()
                //{
                //    RowDefinitions =
                //    {
                //        new RowDefinition{Height=new GridLength(1,GridUnitType.Star)}
                //    },
                //    ColumnDefinitions =
                //    {
                //        new ColumnDefinition{Width=new GridLength(1,GridUnitType.Auto) },
                //        new ColumnDefinition{Width=new GridLength(1,GridUnitType.Auto) }
                //    }
                //};
                //var numLabel = new Label {VerticalOptions=LayoutOptions.StartAndExpand, CharacterSpacing = 0, Text = onlyNum.ToString() };
                //grid.Children.Add(numLabel, 0, 0);
                //var mannaLabel = new Label { VerticalOptions = LayoutOptions.StartAndExpand, CharacterSpacing = 0, Text = onlyString };
                //grid.Children.Add(mannaLabel, 1, 0);
                //flexLayout.Children.Add(grid);


                flexLayout.Children.Add( new StackLayout
                    {
                        Spacing = 0,
                        BackgroundColor = Color.Accent,
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label {CharacterSpacing=0, Text = onlyNum.ToString() },
                            new Label {CharacterSpacing=0, Text = onlyString }
                        }
                    
                });

                allContents += node + "\n\n";
            }
            _allString = allContents;
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
    }
}
