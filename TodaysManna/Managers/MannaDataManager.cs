using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Collections.Generic;
using TodaysManna.Models;

namespace TodaysManna.Managers
{
    public static class MannaDataManager
    {
        public static ObservableRangeCollection<MannaContent> KoreanMannaContents = new ObservableRangeCollection<MannaContent>();
        public static ObservableRangeCollection<MannaContent> EnglishMannaContents = new ObservableRangeCollection<MannaContent>();
        public static ObservableRangeCollection<MannaContent> SpanishMannaContents = new ObservableRangeCollection<MannaContent>();
        public static ObservableRangeCollection<MannaContent> ChineseMannaContents = new ObservableRangeCollection<MannaContent>();
        public static ObservableRangeCollection<MannaContent> JapaneseMannaContents = new ObservableRangeCollection<MannaContent>();
        public static ObservableRangeCollection<MannaContent> GermanMannaContents = new ObservableRangeCollection<MannaContent>();
        public static ObservableRangeCollection<MannaContent> FrenchMannaContents = new ObservableRangeCollection<MannaContent>();
        public static ObservableRangeCollection<MannaContent> HindiMannaContents = new ObservableRangeCollection<MannaContent>();


        public static KoreanManna KoreanMannaData = new KoreanManna();
        public static EnglishManna EnglishMannaData = new EnglishManna();
        public static SpanishManna SpanishMannaData = new SpanishManna();
        public static ChineseManna ChineseMannaData = new ChineseManna();
        public static JapaneseManna JapaneseMannaData = new JapaneseManna();
        public static GermanManna GermanMannaData = new GermanManna();
        public static FrenchManna FrenchMannaData = new FrenchManna();
        public static HindiManna HindiMannaData = new HindiManna();

        public static string Today { get; set; } = DateTime.Now.ToString("yyyy년 MM월 dd일 (ddd)");
        public static string DisplayDateRange { get; set; } = DateTime.Now.ToString("MM/dd");
        public static string AllMannaTexts { get; set; } = "";
        public static string MannaShareRange { get; set; } = "";

        public static string SpanishRange { get; set; } = "";
        public static string ChineseRange { get; set; } = "";
        public static string JapaneseRange { get; set; } = "";
        public static string GermanRange { get; set; } = "";
        public static string FrenchRange { get; set; } = "";
        public static string HindiRange { get; set; } = "";


        public static string BibleWebUrl = "";
        public static string BibleAppUrl = "";

        public static string BookKor;
        public static int Jang;
        public static string JeolRange;

        public static string EnglishMannaRange;

        public static async Task<bool> GetManna(DateTime dateTime)
        {
            AppManager.PrintStartText("GetManna()");

            Today = dateTime.ToString("yyyy년 MM월 dd일 (ddd)");
            DisplayDateRange = dateTime.ToString("MM/dd");

            var endPoint = Constants.MANNA_ENDPOINT + dateTime.ToString("yyyy-MM-dd");

            try
            {
                KoreanMannaData = await RestService.Instance.GetMannaDataAsync(endPoint);

                MannaShareRange = $"만나: {KoreanMannaData.Verse}";

                var bookAndJang = ExtractBookAndJang();

                SetBibleWebAndAppUrl(bookAndJang);
                SetMannaCollection(KoreanMannaData, bookAndJang);

                var currentLanguage = (Language)Enum.Parse(typeof(Language), AppManager.GetCurrentLanguageString());

                if (currentLanguage != Language.Korean)
                {
                    await GetManna(currentLanguage);
                }

                AppManager.PrintCompleteText("GetManna()");

                return true;
            }
            catch (Exception e)
            {
                AppManager.PrintException("GetManna()", e.Message);
                return false;
            }
        }

        public static async Task<bool> GetManna(Language language)
        {
            try
            {
                if (language == Language.English)
                {
                    EnglishMannaData = await RestService.Instance.GetEnglishManna(BookKor, Jang, JeolRange);
                    _ = SetMannaCollection(EnglishMannaData);
                }
                else if (language == Language.Spanish)
                {
                    SpanishMannaData = await RestService.Instance.GetSpanishManna(BookKor, Jang, JeolRange);
                    _ = SetMannaCollection(SpanishMannaData);
                }
                else if (language == Language.Chinese)
                {
                    ChineseMannaData = await RestService.Instance.GetChineseManna(BookKor, Jang, JeolRange);
                    _ = SetMannaCollection(ChineseMannaData);
                }
                else if (language == Language.Japanese)
                {
                    JapaneseMannaData = await RestService.Instance.GetJapaneseManna(BookKor, Jang, JeolRange);
                    _ = SetMannaCollection(JapaneseMannaData);
                }
                else if (language == Language.German)
                {
                    GermanMannaData = await RestService.Instance.GetGermanManna(BookKor, Jang, JeolRange);
                    _ = SetMannaCollection(GermanMannaData);
                }
                else if (language == Language.French)
                {
                    FrenchMannaData = await RestService.Instance.GetFrenchManna(BookKor, Jang, JeolRange);
                    _ = SetMannaCollection(FrenchMannaData);
                }
                else if (language == Language.Hindi)
                {
                    HindiMannaData = await RestService.Instance.GetHindiManna(BookKor, Jang, JeolRange);
                    _ = SetMannaCollection(HindiMannaData);
                }

                AppManager.PrintCompleteText($"GetManna {language.ToString()}");

                return true;
            }
            catch (Exception e)
            {
                AppManager.PrintException($"GetManna {language.ToString()}", e.Message);
                return false;
            }
        }

        private static void SetMannaCollection(KoreanManna JsonMannaData, string bookAndJang)
        {
            AppManager.PrintStartText("KoreanManna SetMannaCollection()");

            var mannaContents = new List<MannaContent>();
            var allMannaTexts = string.Empty;

            foreach (var node in JsonMannaData.Contents)
            {
                var jeol = 0;
                var onlyString = "";

                try { onlyString = Regex.Replace(node, @"\d", "").Substring(1); }
                catch (Exception e) { AppManager.PrintException("SetMannaCollection onlystring", e.Message); }

                try { jeol = int.Parse(Regex.Replace(node, @"\D", "")); }
                catch (Exception e) { AppManager.PrintException("SetMannaCollection onlyNum", e.Message); }

                mannaContents.Add(new MannaContent
                {
                    BookAndJang = bookAndJang,
                    Jeol = jeol,
                    MannaString = onlyString,
                });

                allMannaTexts += node + "\n\n";
            }

            KoreanMannaContents = new ObservableRangeCollection<MannaContent>(mannaContents);
            AllMannaTexts = allMannaTexts;

            AppManager.PrintCompleteText("KoreanManna SetMannaCollection()");
        }

        private static Task SetMannaCollection(EnglishManna JsonMannaData)
        {
            AppManager.PrintStartText("EnglishManna SetMannaCollection()");

            var mannaContents = new List<MannaContent>();

            foreach (var node in JsonMannaData.Verses)
            {
                mannaContents.Add(new MannaContent
                {
                    BookAndJang = $"{node.BookName}{node.Chapter}",
                    Jeol = node.Verse,
                    MannaString = node.Text,
                });
            }

            EnglishMannaContents = new ObservableRangeCollection<MannaContent>(mannaContents);

            AppManager.PrintCompleteText("EnglishManna SetMannaCollection()");

            return Task.CompletedTask;
        }

        private static Task SetMannaCollection(SpanishManna JsonMannaData)
        {
            AppManager.PrintStartText("SpanishManna SetMannaCollection()");

            var mannaContents = new List<MannaContent>();

            foreach (var node in JsonMannaData.Results.Content)
            {
                mannaContents.Add(new MannaContent
                {
                    BookAndJang = $"{node.BookName}{node.Chapter}",
                    Jeol = node.Verse,
                    MannaString = node.Text,
                });
                SpanishRange = $"{node.BookName}{node.Chapter}:{node.Verse}";
            }

            SpanishMannaContents = new ObservableRangeCollection<MannaContent>(mannaContents);

            AppManager.PrintCompleteText("SpanishManna SetMannaCollection()");

            return Task.CompletedTask;
        }

        private static Task SetMannaCollection(ChineseManna JsonMannaData)
        {
            AppManager.PrintStartText("ChineseManna SetMannaCollection()");

            var mannaContents = new List<MannaContent>();

            foreach (var node in JsonMannaData.Results.Content)
            {
                mannaContents.Add(new MannaContent
                {
                    BookAndJang = $"{node.BookName}{node.Chapter}",
                    Jeol = node.Verse,
                    MannaString = node.Text,
                });
                ChineseRange = $"{node.BookName}{node.Chapter}:{node.Verse}";
            }

            ChineseMannaContents = new ObservableRangeCollection<MannaContent>(mannaContents);

            AppManager.PrintCompleteText("ChineseManna SetMannaCollection()");

            return Task.CompletedTask;
        }

        private static Task SetMannaCollection(JapaneseManna JsonMannaData)
        {
            AppManager.PrintStartText("JapaneseManna SetMannaCollection()");

            var mannaContents = new List<MannaContent>();

            foreach (var node in JsonMannaData.Results.Content)
            {
                mannaContents.Add(new MannaContent
                {
                    BookAndJang = $"{node.BookName}{node.Chapter}",
                    Jeol = node.Verse,
                    MannaString = node.Text,
                });
                JapaneseRange = $"{node.BookName}{node.Chapter}:{node.Verse}";
            }

            JapaneseMannaContents = new ObservableRangeCollection<MannaContent>(mannaContents);

            AppManager.PrintCompleteText("JapaneseManna SetMannaCollection()");

            return Task.CompletedTask;
        }

        private static Task SetMannaCollection(GermanManna JsonMannaData)
        {
            AppManager.PrintStartText("GermanManna SetMannaCollection()");

            var mannaContents = new List<MannaContent>();

            foreach (var node in JsonMannaData.Results.Content)
            {
                mannaContents.Add(new MannaContent
                {
                    BookAndJang = $"{node.BookName}{node.Chapter}",
                    Jeol = node.Verse,
                    MannaString = node.Text,
                });
                GermanRange = $"{node.BookName}{node.Chapter}:{node.Verse}";
            }

            GermanMannaContents = new ObservableRangeCollection<MannaContent>(mannaContents);

            AppManager.PrintCompleteText("GermanManna SetMannaCollection()");

            return Task.CompletedTask;
        }

        private static Task SetMannaCollection(FrenchManna JsonMannaData)
        {
            AppManager.PrintStartText("FrenchManna SetMannaCollection()");

            var mannaContents = new List<MannaContent>();

            foreach (var node in JsonMannaData.Results.Content)
            {
                mannaContents.Add(new MannaContent
                {
                    BookAndJang = $"{node.BookName}{node.Chapter}",
                    Jeol = node.Verse,
                    MannaString = node.Text,
                });
                FrenchRange = $"{node.BookName}{node.Chapter}:{node.Verse}";
            }

            FrenchMannaContents = new ObservableRangeCollection<MannaContent>(mannaContents);

            AppManager.PrintCompleteText("FrenchManna SetMannaCollection()");

            return Task.CompletedTask;
        }

        private static Task SetMannaCollection(HindiManna JsonMannaData)
        {
            AppManager.PrintStartText("HindiManna SetMannaCollection()");

            var mannaContents = new List<MannaContent>();

            foreach (var node in JsonMannaData.Results.Content)
            {
                mannaContents.Add(new MannaContent
                {
                    BookAndJang = $"{node.BookName}{node.Chapter}",
                    Jeol = node.Verse,
                    MannaString = node.Text,
                });
                HindiRange = $"{node.BookName}{node.Chapter}:{node.Verse}";
            }

            HindiMannaContents = new ObservableRangeCollection<MannaContent>(mannaContents);

            AppManager.PrintCompleteText("HindiManna SetMannaCollection()");

            return Task.CompletedTask;
        }

        private static void SetBibleWebAndAppUrl(string bookAndJang)
        {
            BookKor = ExtractBookKor(bookAndJang);
            Jang = ExtractJang(bookAndJang);
            JeolRange = GetJeolRange();

            var redirectUrl = $"{BookKor.BibleBookKorToEng()}.{Jang}.{JeolRange}.NKJV";

            BibleWebUrl = $"{Constants.BIBLE_WEB_ENDPOINT}{redirectUrl}";
            BibleAppUrl = $"{Constants.BIBLE_APP_ENDPOINT}{redirectUrl}";
        }

        private static int ExtractJang(string guonAndJang)
        {
            var _jang = 1;
            try
            {
                _jang = int.Parse(Regex.Replace(guonAndJang, @"\D", ""));
            }
            catch (Exception e)
            {
                AppManager.PrintException("ExtractJang()", e.Message);
            }
            return _jang;
        }

        private static string ExtractBookKor(string guonAndJang)
        {
            var _bookKor = "창";
            try
            {
                _bookKor = Regex.Replace(guonAndJang, @"\d", "");
            }
            catch (Exception e)
            {
                AppManager.PrintException("ExtractBookKor()", e.Message);
            }
            return _bookKor;
        }

        private static string GetJeolRange()
        {
            var tmpVerseNumRange = "1-10";
            try
            {
                tmpVerseNumRange = Regex.Replace(KoreanMannaData.Verse.Substring(KoreanMannaData.Verse.IndexOf(":") + 1), "~", "-");
            }
            catch (Exception e)
            {
                AppManager.PrintException("GetJeolRange()", e.Message);
            }

            return tmpVerseNumRange;
        }

        private static string ExtractBookAndJang()
        {
            var tmpBibleAt = "창1";
            try
            {
                tmpBibleAt = KoreanMannaData.Verse.Substring(0, KoreanMannaData.Verse.IndexOf(":"));
            }
            catch (Exception e)
            {
                AppManager.PrintException("ExtractBookAndJang()", e.Message);
            }

            return tmpBibleAt;
        }
    }
}
