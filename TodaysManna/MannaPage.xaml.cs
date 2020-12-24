using System;
using Xamarin.Forms;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace TodaysManna
{
    public partial class MannaPage : ContentPage
    {
        private const string ConfirmUrl = "https://community.jbch.org/confirm.php";
        private const string MainUrl = "https://community.jbch.org/";

        private string _id;
        private string _passwd;


        public MannaPage()
        {
            InitializeComponent();

            BindingContext = new MannaViewModel();

            (BindingContext as MannaViewModel).TodayString = DateTime.Now.ToString("yyyy-MM-dd dddd") + "\n";

            if (DateTime.Now.ToString("dddd").Equals("일요일"))
            {
                (BindingContext as MannaViewModel).TitleString = "일요일은 지원하지 않습니다.";
                (BindingContext as MannaViewModel).AllString = "";
            }
            else
            {
                var task = GetMannaText();

                (BindingContext as MannaViewModel).ReloadCommand = new Command(async () =>
                {
                    await GetMannaText();
                    (BindingContext as MannaViewModel).IsReloading = false;
                });
                (BindingContext as MannaViewModel).ShareCommand = new Command(async () => await ShareFunc());
            }
        }

        private async Task GetMannaText()
        {
            (BindingContext as MannaViewModel).AllString = "Loading...";
            (BindingContext as MannaViewModel).TitleString = "";

            var resp = await HttpWebResponse();

            var getAttr = await GetDetailPageUrl(resp);

            if (getAttr == null)
            {
                (BindingContext as MannaViewModel).AllString = "불러오기 실패, 재시도 해보세요.";
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
        private static async Task<HtmlNode> GetDetailPageUrl(HttpWebResponse resp)
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

        private static string SubstringUrl(HtmlNode getAttr)
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

            (BindingContext as MannaViewModel).TitleString = mannarange + "\n";
            (BindingContext as MannaViewModel).AllString = texts;
        }



        private async Task ShareFunc()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = (BindingContext as MannaViewModel).TodayString + (BindingContext as MannaViewModel).TitleString + (BindingContext as MannaViewModel).AllString,
                Title = "Share Manna"
            });
        }
    }
}
