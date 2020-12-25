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

        private const string Id = "";
        private const string Password = "";

        public ICommand ReloadCommand { set; get; }

        private string _todayString;
        private string _titleString;
        private string _allString;
        private bool _isReloading;

        public MannaViewModel()
        {
            TodayString = DateTime.Now.ToString("yyyy/MM/dd dddd") + "\n";

            

            if (DateTime.Now.DayOfWeek==DayOfWeek.Sunday)
            {
                TitleString = "히10:30-39";
                AllString = "30 원수 갚는 것이 내게 있으니 내가 갚으리라 하시고 또 다시 주께서 그의 백성을 심판하리라 말씀하신 것을 우리가 아노니\n\n31 살아계신 하나님의 손에 빠져 들어가는 것이 무서울진저\n\n32 전날에 너희가 빛을 받은 후에 고난의 큰 싸움에 참은 것을 생각하라\n\n33 혹 비방과 환난으로써 사람에게 구경거리가 되고 혹 이런 형편에 있는 자들로 사귀는 자 되었으니\n\n34 너희가 갇힌 자를 동정하고 너희 산업을 빼앗기는 것도 기쁘게 당한 것은 더 낫고 영구한 산업이 있는줄 앎이라\n\n35 그러므로 너희 담대함을 버리지 말라 이것이 큰 상을 얻느니라\n\n36 너희에게 인내가 필요함은 너희가 하나님의 뜻을 행한 후에 약속을 받기 위함이라\n\n37 잠시 잠깐 후면 오실 이가 오시리니 지체하지 아니하시리라\n\n38 오직 나의 의인은 믿음으로 말미암아 살리라 또한 뒤로 물러가면 내 마음이 저를 기뻐하지 아니하리라 하셨느니라\n\n39 우리는 뒤로 물러가 침륜에 빠질 자가 아니요 오직 영혼을 구원함에 이르는 믿음을 가진 자니라\n\n";

                ReloadCommand = new Command( () =>
                {
                    IsReloading = false;
                });
            }
            else
            {
                var task = GetMannaText();

                ReloadCommand = new Command(async () =>
                {
                    await GetMannaText();

                    IsReloading = false;
                });
            }
        }

        private async Task GetMannaText()
        {
            //AllString = "Loading...";
            //TitleString = "Loading...";

            var resp = await HttpWebResponse();

            var getAttr = await GetDetailPageUrl(resp);

            if (getAttr == null)
            {
                AllString = "불러오기 실패, 재시도 해보세요.";
                MessagingCenter.Send(this, "unloaded");

                return;
            }

            var detailPageUrl = SubstringUrl(getAttr);

            await GetDetailTexts(resp, detailPageUrl);

            MessagingCenter.Send(this, "loaded");
        }

        /*********************************
             * 
             * 초기 로그인 쿠기 얻는 작업.
             * 
            *********************************/

        private async Task<HttpWebResponse> HttpWebResponse()
        {
            var req = (HttpWebRequest) WebRequest.Create(ConfirmUrl);

            req.Method = "Post";
            req.CookieContainer = new CookieContainer();
            req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";

            var w = new StreamWriter(await req.GetRequestStreamAsync());

            await w.WriteAsync("user_id=" + Id + "&saveid=1&passwd=" + Password +
                               "&mode=&go=yes&url=http://community.jbch.org/&LoginButton=LoginButton");
            w.Close();

            var resp = (HttpWebResponse) (await req.GetResponseAsync());
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

            var req = (HttpWebRequest) WebRequest.Create(MainUrl);

            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(respBuffer.Cookies);
            respBuffer = (HttpWebResponse) (await req.GetResponseAsync());

            var r = new StreamReader(respBuffer.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.GetEncoding("UTF-8"));

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

            var req = (HttpWebRequest) WebRequest.Create("http://community.jbch.org/meditation/board/process.php/");
            req.Method = "Post";

            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(respBuffer.Cookies);
            req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";

            var w2 = new StreamWriter(await req.GetRequestStreamAsync());

            w2.Write("mode=load_post&post_uid=" + result);
            w2.Close();

            respBuffer = (HttpWebResponse) (await req.GetResponseAsync());
            var r2 = new StreamReader(respBuffer.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.GetEncoding("UTF-8"));

            var htmlDoc2 = new HtmlDocument();
            htmlDoc2.LoadHtml(await r2.ReadToEndAsync());

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


        public async Task ShareFunc()
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