using System;
using Xamarin.Forms;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Text;
using Plugin.Clipboard;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace TodaysManna
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string _id, _passwd;
        public static CookieContainer cookie = new CookieContainer();

        string todayString;
        public string TodayString
        {
            get
            {
                return todayString;
            }
            set
            {
                if (todayString != value)
                {
                    todayString = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("TodayString"));
                    }
                }
            }
        }

        string titleString;
        public string TitleString
        {
            get
            {
                return titleString;
            }
            set
            {
                if (titleString != value)
                {
                    titleString = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("TitleString"));
                    }
                }
            }
        }

        string allString;
        public string AllString
        {
            get
            {
                return allString;
            }
            set
            {
                if (allString != value)
                {
                    allString = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("AllString"));
                    }
                }
            }
        }

        bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsBusy"));
                    }
                }
            }
        }
        bool isReloading;
        public bool IsReloading
        {
            get { return isReloading; }
            set
            {
                if (isReloading != value)
                {
                    isReloading = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsReloading"));
                    }
                }
            }
        }
        public MainViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            DateTime dt = new DateTime();

            TodayString = DateTime.Now.ToString("yyyy-MM-dd") + "\n";

            this.ReloadCommand = new Command(async () =>
            {
                await GetMannaText(); IsReloading = false;
            });
            this.ShareCommand = new Command(async() => await ShareFunc());
            //this.CoppyCommand = new Command(() => CoppyFunc());
            this.ToolbarItem_Clicked_Command = new Command(async () => await ToolbarItem_Clicked_Func());

            _id = Application.Current.Properties["ID"] as string;
            _passwd = Application.Current.Properties["PASSWD"] as string;

            if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                TitleString = "일요일은 지원하지 않습니다.";
                AllString = "";
            }
            else
            {
               var task = GetMannaText();
            }
        }

        async Task GetMannaText()
        {
            IsBusy = true;
            AllString = "Loading...";
            TitleString = "";
            /*********************************
             * 
             * 초기 로그인 쿠기 얻는 작업.
             * 
            *********************************/

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://community.jbch.org/confirm.php");

            req.Method = "Post"; // Post 방식
            string s = "user_id=" + _id + "&saveid=1&passwd=" + _passwd + "&mode=&go=yes&url=http://community.jbch.org/&LoginButton=LoginButton"; // 'input'에 들어갈 내용
            req.CookieContainer = new CookieContainer(); // 쿠키 컨테이너 생성
            //req.ContentLength = s.Length; // 필요없는듯?
            req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";

            TextWriter w = (TextWriter)new System.IO.StreamWriter(req.GetRequestStream());

            w.Write(s);
            w.Close();

            HttpWebResponse resp = (HttpWebResponse)(await req.GetResponseAsync());

            TextReader r = (TextReader)new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));


            /*********************************
             * 
             * 로그인 쿠키 가지고 만나 세부페이지 url 가져오기.
             * 
            *********************************/

            string htmlBuffer = "";
            HttpWebResponse respBuffer = resp;

            //onsole.WriteLine("페이지 로드중");
            req = (HttpWebRequest)WebRequest.Create("https://community.jbch.org/");

            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(respBuffer.Cookies);
            respBuffer = (HttpWebResponse)(await req.GetResponseAsync());

            r = (TextReader)new StreamReader(respBuffer.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            htmlBuffer = r.ReadToEnd();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlBuffer);

            var getAttr = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='conbox active']/div[@onclick]");

            if (getAttr == null)
            {
                await BackToLogin();
            }
            else
            {
                var getURL = getAttr.GetAttributeValue("onclick", string.Empty);

                string thumUrlString = getURL
                                .Replace("getUrl('", "")
                                .Replace("', '')", "");

                String target = "?uid=";
                int target_num = thumUrlString.IndexOf(target);
                int target2_num = thumUrlString.IndexOf("&");

                String result;
                result = thumUrlString.Substring(target_num + 5, target2_num - target_num - 5);


                /*********************************
                 * 
                 * 만나 페이지 들어가서 범위, 구절 가져오기.
                 * 
                *********************************/

                string htmlBuffer2 = "";
                HttpWebResponse respBuffer2 = resp;
                string str = "mode=load_post&post_uid=" + result;

                HttpWebRequest req2 = (HttpWebRequest)WebRequest.Create("http://community.jbch.org/meditation/board/process.php/");
                req2.Method = "Post";

                req2.CookieContainer = new CookieContainer();
                req2.CookieContainer.Add(respBuffer2.Cookies);

                req2.ContentLength = str.Length;
                req2.ContentType = "application/x-www-form-urlencoded; charset=utf-8";

                TextWriter w2 = (TextWriter)new StreamWriter(req2.GetRequestStream());

                w2.Write(str);
                w2.Close();

                respBuffer2 = (HttpWebResponse)(await req2.GetResponseAsync());
                TextReader r2 = (TextReader)new StreamReader(respBuffer2.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                htmlBuffer2 = r2.ReadToEnd();

                var htmlDoc2 = new HtmlDocument();
                htmlDoc2.LoadHtml(htmlBuffer2);

                var mannarange = htmlDoc2.DocumentNode.SelectSingleNode("//div[@class='titlebox']/div[@class='title']").InnerText;

                var tmp = htmlDoc2.DocumentNode
                    .SelectNodes("//div[@class='contentbox fr-view']/p");

                string texts = "";

                foreach (HtmlNode node in tmp)
                {
                    texts += node.InnerHtml + "\n\n";
                }
                texts = Regex.Replace(texts, @"<br>", "\n\n");

                TitleString = "(" + mannarange + ")" + "\n";
                AllString = texts;
                //Console.WriteLine(htmlBuffer2);
                IsBusy = false;
            }
        }

        private async Task BackToLogin()
        {
            Application.Current.Properties["ID"] = "";
            Application.Current.Properties["PASSWD"] = "";
            Application.Current.Properties["ISLOGIN"] = false;
            await Application.Current.SavePropertiesAsync();
            await Navigation.PushAsync(new LoginPage());
        }

        private async Task ToolbarItem_Clicked_Func()
        {
            await Navigation.PushAsync(new infoPage());
        }


        public async Task ShareFunc()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = TodayString + TitleString + AllString,
                Title = "Share Manna"
            });
        }

        public ICommand ToolbarItem_Clicked_Command { protected set; get; }
        public ICommand ReloadCommand { protected set; get; }
        public ICommand ShareCommand { protected set; get; }
        public INavigation Navigation { get; set; }
        //public ICommand CoppyCommand { protected set; get; }


        /*

        async private void CoppyFunc()
        {
            CrossClipboard.Current.SetText(todayString + titleString + allString);
            await Application.Current.MainPage.DisplayAlert(null, "복사되었습니다.", "확인");
        }

        public void getMannaImage()
        {
            string checkUrl = getMannaImage(mainLogin());
            if (checkUrl == "quit")
            {
                BackToLogin();
            }
            else
            {
                image.Source = checkUrl;
            }
        }
        
        public string getMannaImage(HttpWebResponse resp)
        {
            string htmlBuffer = "";
            HttpWebResponse respBuffer = resp;
            Console.WriteLine("페이지 로드중");
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://community.jbch.org/");

            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(respBuffer.Cookies);
            respBuffer = (HttpWebResponse)req.GetResponse();
            TextReader r = (TextReader)new StreamReader(respBuffer.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            htmlBuffer = r.ReadToEnd();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlBuffer);

            if (htmlDoc == null)
            {
                BackToLogin();
            }

            var getAttr = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='conbox active']//img[@src]");

            Console.WriteLine("@@@@@@@getAttr=" + getAttr);

            if (getAttr == null)
            {
                return "quit";
            }
            else
            {
                var getURL = getAttr.GetAttributeValue("src", string.Empty);

                return getURL;
            }
        }
        */

    }
}
