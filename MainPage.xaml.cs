using System;
using Xamarin.Forms;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Text;
using System.Linq;

namespace htmlparsing
{

    public partial class MainPage : ContentPage
    {
        public Idpasswd idpasswd;
        public string _id, _passwd;
        public MainPage(string id, string passwd)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            _id = id;
            _passwd = passwd;
            dosomething();
        }
        public static CookieContainer cookie = new CookieContainer();


        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            //Contents contents = new Contents();
            // string content = loginImageURL(mainLogin());
            // label.Text = getText(content);
             dosomething();
        }
         public void dosomething()
        {
            string checkUrl = getMannaImage(mainLogin());
            if (checkUrl == "quit")
            {
                BackToLogin();
            }
            else
            {
                image.Source = checkUrl;
                image.Aspect = Aspect.AspectFit;
                image.HeightRequest = 500;
            }
        }

        public HttpWebResponse mainLogin()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://community.jbch.org/confirm.php");

            req.Method = "Post";
            // string s = "user_id=jinyeob07&saveid=1&passwd=wlsduq1004&mode=&go=yes&url=http://


            string s = "user_id=" + _id + "&saveid=1&passwd=" + _passwd + "&mode=&go=yes&url=http://community.jbch.org/&LoginButton=LoginButton";
            req.CookieContainer = new CookieContainer();
            req.ContentLength = s.Length;
            req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";

            TextWriter w = (TextWriter)new System.IO.StreamWriter(req.GetRequestStream());

            w.Write(s);
            w.Close();

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            TextReader r = (TextReader)new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));

            return resp;
        }

        public string loginImageURL(HttpWebResponse resp)
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

             var getAttr = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='conbox active']/div[@onclick]");


            var getURL = getAttr.GetAttributeValue("onclick", string.Empty);
            
            string thumUrlString = getURL
                            .Replace("getUrl('", "")
                            .Replace("', '')", "");

           // Console.WriteLine("thumUrlString : " + thumUrlString);

            String target = "?uid=";
            int target_num = thumUrlString.IndexOf(target);
            int target2_num = thumUrlString.IndexOf("&");
          //  Console.WriteLine("target_num : "+target_num);
         //   Console.WriteLine("target2_num : " + target2_num);

            String result;
           result = thumUrlString.Substring(target_num + 5, target2_num-target_num-5);

         //  Console.WriteLine(result);

            
            ////////////////////////////////////////////////

            string htmlBuffer2 = "";
            HttpWebResponse respBuffer2 = resp;
            HttpWebRequest req2 = (HttpWebRequest)WebRequest.Create("http://community.jbch.org/meditation/board/process.php");

            req2.CookieContainer = new CookieContainer();
            req2.CookieContainer.Add(respBuffer2.Cookies);

            string str = "mode=load_post&post_uid=" + result;
            TextWriter w2 = (TextWriter)new System.IO.StreamWriter(req2.GetRequestStream());

            w2.Write(str);
            w2.Close();

            respBuffer2 = (HttpWebResponse)req2.GetResponse();
            TextReader r2 = (TextReader)new StreamReader(respBuffer2.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            htmlBuffer2 = r2.ReadToEnd();
            


            return htmlBuffer;
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

            Console.WriteLine("@@@@@@@getAttr="+getAttr);

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

        async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new infoPage());
        }

        public void OnLogoutClicked(object sender, EventArgs e)
        {
            BackToLogin();
        }


        async public void BackToLogin()
        {
            Application.Current.Properties["ID"] = "";
            Application.Current.Properties["PASSWD"] = "";
            Application.Current.SavePropertiesAsync();
            await Navigation.PopToRootAsync();
        }
    }
}
