using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace TodaysManna
{
    class MannaViewModel : INotifyPropertyChanged
    {
        private const string ConfirmUrl = "https://community.jbch.org/confirm.php";
        private const string MainUrl = "https://community.jbch.org/";

        private string _id;
        private string _passwd;

        public ICommand ReloadCommand { set; get; }
        public ICommand ShareCommand { set; get; }

        private string _todayString;
        private string _titleString;
        private string _allString;
        private bool _isReloading;

        public MannaViewModel()
        {
            TodayString = DateTime.Now.ToString("yyyy-MM-dd dddd") + "\n";

            if (DateTime.Now.ToString("dddd").Equals("일요일"))
            {
                TitleString = "일요일은 지원하지 않습니다.";
                AllString = "";
            }
            else
            {
                var task = GetMannaText();

                ReloadCommand = new Command(async () =>
                {
                    await GetMannaText();
                    IsReloading = false;
                });
                ShareCommand = new Command(async () => await ShareFunc());
            }
        }

        private async Task GetMannaText()
        {
            AllString = "Loading...";
            TitleString = "";

            var resp = await HttpWebResponse();

            var getAttr = await GetDetailPageUrl(resp);

            if (getAttr == null)
            {
                AllString = "불러오기 실패, 재시도 해보세요.";
                return;
            }

            var detailPageUrl = SubstringUrl(getAttr);

            await GetDetailTexts(resp, detailPageUrl);
        }

        /*********************************
             * 
             * 초기 로그인 쿠기 얻는 작업.
             * 
            *********************************/

        private async Task<HttpWebResponse> HttpWebResponse()
        {
            var req = (HttpWebRequest)WebRequest.Create(ConfirmUrl);

            req.Method = "Post";
            req.CookieContainer = new CookieContainer();
            req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";

            var w = new StreamWriter(await req.GetRequestStreamAsync());

            w.Write("user_id=" + _id + "&saveid=1&passwd=" + _passwd +
                    "&mode=&go=yes&url=http://community.jbch.org/&LoginButton=LoginButton");
            w.Close();

            var resp = (HttpWebResponse)(await req.GetResponseAsync());
            return resp;
        }

        /*********************************
           * 
           * 로그인 쿠키 가지고 만나 세부페이지 url 가져오기.
           * 
          *********************************/

        private async Task<HtmlNode> GetDetailPageUrl(HttpWebResponse resp)
        {
            var respBuffer = resp;

            var req = (HttpWebRequest)WebRequest.Create(MainUrl);

            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(respBuffer.Cookies);
            respBuffer = (HttpWebResponse)(await req.GetResponseAsync());

            var r = new StreamReader(respBuffer.GetResponseStream(), Encoding.GetEncoding("UTF-8"));

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(await r.ReadToEndAsync());

            var getAttr = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='conbox active']/div[@onclick]");

            return getAttr;
        }

        private string SubstringUrl(HtmlNode getAttr)
        {
            var attrUrl = getAttr.GetAttributeValue("onclick", string.Empty);

            var thumUrlString = attrUrl
                .Replace("getUrl('", "")
                .Replace("', '')", "");

            var targetNum = thumUrlString.IndexOf("?uid=", StringComparison.Ordinal);
            var target2Num = thumUrlString.IndexOf("&", StringComparison.Ordinal);

            var detailPageUrl = thumUrlString.Substring(targetNum + 5, target2Num - targetNum - 5);
            return detailPageUrl;
        }


        /*********************************
           * 
           * 만나 페이지 들어가서 범위, 구절 가져오기.
           * 
           *********************************/

        private async Task GetDetailTexts(HttpWebResponse resp, string result)
        {

            var respBuffer = resp;

            var req = (HttpWebRequest)WebRequest.Create("http://community.jbch.org/meditation/board/process.php/");
            req.Method = "Post";

            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(respBuffer.Cookies);
            req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";

            var w2 = new StreamWriter(await req.GetRequestStreamAsync());

            w2.Write("mode=load_post&post_uid=" + result);
            w2.Close();

            respBuffer = (HttpWebResponse)(await req.GetResponseAsync());
            var r2 = new StreamReader(respBuffer.GetResponseStream(), Encoding.GetEncoding("UTF-8"));

            var htmlDoc2 = new HtmlDocument();
            htmlDoc2.LoadHtml(r2.ReadToEnd());

            var mannarange = htmlDoc2.DocumentNode.SelectSingleNode("//div[@class='titlebox']/div[@class='title']")
                .InnerText;

            var tmp = htmlDoc2.DocumentNode
                .SelectNodes("//div[@class='contentbox fr-view']/p");

            var texts = "";

            foreach (var node in tmp)
            {
                texts += node.InnerHtml + "\n\n";
            }

            texts = Regex.Replace(texts, @"<br>", "\n\n");
            texts = Regex.Replace(texts, @"&nbsp;", "");

            TitleString = mannarange + "\n";
            AllString = texts;
        }



        private async Task ShareFunc()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = TodayString + TitleString + AllString,
                Title = "Share Manna"
            });
        }


        public string TodayString
        {
            get => _todayString;
            set
            {
                if (_todayString == value) return;
                _todayString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TodayString"));
            }
        }

        public string TitleString
        {
            get => _titleString;
            set
            {
                if (_titleString == value) return;
                _titleString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TitleString"));
            }
        }

        public string AllString
        {
            get => _allString;
            set
            {
                if (_allString == value) return;
                _allString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllString"));
            }
        }

        public bool IsReloading
        {
            get => _isReloading;
            set
            {
                if (_isReloading == value) return;
                _isReloading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsReloading"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}