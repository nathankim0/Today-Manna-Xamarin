using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TodaysManna.Models;
using Xamarin.Forms;
using TodaysManna.Constants;
using TodaysManna.ExtensionMethods;
using System.Diagnostics;
using TodaysManna.Managers;

namespace TodaysManna.ViewModel
{
    public static class GetMannaService
    {
        private static readonly RestService _restService = new RestService();

        private static readonly string _webBibleUrl = "https://www.bible.com/ko/bible/1/"; //https://www.bible.com/ko/bible/GEN.1.KJV
        private static readonly string _appBibleUrl = "youversion://bible?reference=";

        private static string _completeUrl = "";
        private static string _completeAppUrl = "";
        private static string _bookKor = "창";
        private static int _jang = 1;

        public static async Task<bool> InitMannaData()
        {
            InitDate();
            return await GetManna();
        }

        public static async void RefreshManna()
        {
            await Task.WhenAll(ClearMannaDataAndGetManna());
        }

        private static async Task ClearMannaDataAndGetManna()
        {
            var mannaContents = MannaDataManager.MannaContents;
            if (mannaContents.Count != 0)
            {
                for (int i = 0; i < mannaContents.Count; i++)
                {
                    MannaDataManager.MannaContents.Remove(mannaContents[i]);
                }
            }

            await GetManna(DateTime.Now);
        }

        private static void InitDate()
        {
            var rangeOfDate = MccheyneDataManager.MccheyneRangeList.Find(x => x.Date.Equals(DateTime.Now.ToString("M-d")));
            MannaDataManager.TodayMccheyneRange = $"{rangeOfDate.Range1} {rangeOfDate.Range2} {rangeOfDate.Range3} {rangeOfDate.Range4} {rangeOfDate.Range5}";
        }

        private static async Task<bool> GetManna()
        {
            if (await GetJsonMannaAndSetContents(Rests.MannaEndpoint))
            {
                return true;
            }
            return false;
        }

        public static async Task<bool> GetManna(DateTime dateTime)
        {
            MannaDataManager.Today = dateTime.ToString("yyyy년 MM월 dd일 (ddd)");
            MannaDataManager.DisplayDateRange = dateTime.ToString("MM/dd");

            var newDateString = dateTime.ToString("yyyy-MM-dd");
            var endPoint = Rests.MannaEndpoint + newDateString;

            var findMccheyneDate = dateTime.ToString("M-d");
            var rangeOfDate = MccheyneDataManager.MccheyneRangeList.Find(x => x.Date.Equals(findMccheyneDate));
            MannaDataManager.TodayMccheyneRange = $"{rangeOfDate.Range1} {rangeOfDate.Range2} {rangeOfDate.Range3} {rangeOfDate.Range4} {rangeOfDate.Range5}";

            if (await GetJsonMannaAndSetContents(endPoint))
            {
                return true;
            }
            return false;
        }

        private static async Task<bool> GetJsonMannaAndSetContents(string endPoint)
        {
            try
            {
                MannaDataManager.JsonMannaData = new JsonMannaModel();
                MannaDataManager.JsonMannaData = await _restService.GetMannaDataAsync(endPoint);

                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("***** GetJsonMannaAndSetContents() *****\n");

                var tmpBibleAt = "창1";
                try
                {
                    tmpBibleAt = MannaDataManager.JsonMannaData.Verse.Substring(0, MannaDataManager.JsonMannaData.Verse.IndexOf(":"));
                }
                catch (Exception e)
                {
                    Debug.Fail(e.Message);
                    Console.WriteLine("--------------------------------------------------------\n");
                }

                var tmpVerseNumRange = "1-10";
                try
                {
                    tmpVerseNumRange = Regex.Replace(MannaDataManager.JsonMannaData.Verse.Substring(MannaDataManager.JsonMannaData.Verse.IndexOf(":") + 1), "~", "-");
                }
                catch (Exception e)
                {
                    Debug.Fail(e.Message);
                    Console.WriteLine("--------------------------------------------------------\n");
                }

                _bookKor = Regex.Replace(tmpBibleAt, @"\d", "");
                _jang = int.Parse(Regex.Replace(tmpBibleAt, @"\D", ""));

                var redirectUrl = $"{_bookKor.BibleBookKorToEng()}.{_jang}.{tmpVerseNumRange}.NKJV";

                _completeUrl = $"{_webBibleUrl}{redirectUrl}";
                _completeAppUrl = $"{_appBibleUrl}{redirectUrl}";

                SetMannaContents();

                MannaDataManager.MannaShareRange = $"만나: {MannaDataManager.JsonMannaData.Verse}";
                MannaDataManager.McShareRange = $"맥체인: {MannaDataManager.TodayMccheyneRange}";

                Console.WriteLine("_bib: " + _bookKor);
                Console.WriteLine("_jang: " + _jang);
                Console.WriteLine("redirectUrl: " + redirectUrl);
                Console.WriteLine("_completeUrl: " + _completeUrl);
                Console.WriteLine("_completeAppUrl: " + _completeAppUrl);
                Console.WriteLine("MannaShareRange: " + MannaDataManager.MannaShareRange);
                Console.WriteLine("McShareRange: " + MannaDataManager.McShareRange);
                Console.WriteLine("--------------------------------------------------------\n");
                return true;
            }
            catch (Exception e)
            {
                Debug.Fail("# MannaViewModel GetManna(DateTime dateTime) \n" + e.Message);
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("만나 로드 실패", "", "확인");
            }
            return false;
        }

        private static void SetMannaContents()
        {
            try
            {
                MannaDataManager.MannaContents.Clear();
            }
            catch (Exception e)
            {
                Debug.Fail("# MannaViewModel SetMannaContents \n" + e.Message);
            }

            var allContents = "";
            var bookAndJang = MannaDataManager.JsonMannaData.Verse.Substring(0, MannaDataManager.JsonMannaData.Verse.IndexOf(":") + 1);

            foreach (var node in MannaDataManager.JsonMannaData.Contents)
            {
                var onlyNum = int.Parse(Regex.Replace(node, @"\D", ""));
                var verse = bookAndJang + onlyNum;
                var onlyString = Regex.Replace(node, @"\d", "").Substring(1);

                MannaDataManager.MannaContents.Add(new MannaContent
                {
                    Verse = verse,
                    Number = onlyNum,
                    MannaString = onlyString,
                });

                allContents += node + "\n\n";
            }
            MannaDataManager.AllString = allContents;
        }
    }
}