<h1 align="center">📖 오늘의 만나 📖 </h1>

> 홈페이지에 로그인 후 묵상 범위를 가져옵니다. </br>

## 📌 핵심코드
---
* HttpWebRequest
``` csharp
HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://community.jbch.org/confirm.php");
req.Method = "Post";
string s = "user_id=" + _id + "&saveid=1&passwd=" + _passwd + "&mode=&go=yes&url=http://community.jbch.org/&LoginButton=LoginButton";
req.CookieContainer = new CookieContainer();
req.ContentLength = s.Length;
req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";

TextWriter w = (TextWriter)new System.IO.StreamWriter(req.GetRequestStream());
w.Write(s);
w.Close();

HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
TextReader r = (TextReader)new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
```

* HTML select
``` csharp
var tmp = htmlDoc2.DocumentNode.SelectSingleNode("//div[@class='contentbox fr-view']/p").InnerHtml;
```


* Extract 'post_uid'
``` csharp
string thumUrlString = getURL
                  .Replace("getUrl('", "")
                  .Replace("', '')", "");
 String target = "?uid=";
int target_num = thumUrlString.IndexOf(target);
int target2_num = thumUrlString.IndexOf("&");

String result;
result = thumUrlString.Substring(target_num + 5, target2_num - target_num - 5);
```

* Change ```<br>```tag to newline
``` csharp
tmp = Regex.Replace(tmp, @"<br>", "\n\n");
```

* Login (Save ID and Password)
``` csharp
if (string.IsNullOrWhiteSpace(entry1.Text) || string.IsNullOrWhiteSpace(entry2.Text))
            {
                await DisplayAlert("Login Failed", "Please check your ID and Password.", "OK");
            }
            else
            {
                _Id = entry1.Text;
                _Passwd = entry2.Text;

                Application.Current.Properties["ID"] = _Id;
                Application.Current.Properties["PASSWD"] = _Passwd;
                Application.Current.SavePropertiesAsync();

                await Navigation.PushAsync(new MainPage(_Id, _Passwd));
            }
```

## 📌 사용기술
---
* HttpWebResponse (System.net) -> Login to the homepage and parse HTML.
* htmlagilitypack (https://html-agility-pack.net/) -> Select HTML node from HtmlDocument.
* Xamarin.Plugins.Clipboard (https://www.nuget.org/packages/Xamarin.Plugins.Clipboard/) -> Coppy Text to Clipboard

## 📌 데모
---
* 앱
<p>
<img src="https://github.com/Jinyeob/Today-Manna-Hybrid/blob/master/video.gif" width="40%"/>
</p>

* 파싱한 페이지 스크린샷
<p>
<img src="https://user-images.githubusercontent.com/37360089/92693284-5f646580-f380-11ea-899a-d29efc2d276a.png" width="50%"/><img src="https://user-images.githubusercontent.com/37360089/92693166-3348e480-f380-11ea-9dd2-b0eade042aeb.png" width="50%"/><img src="https://user-images.githubusercontent.com/37360089/92692980-ebc25880-f37f-11ea-8013-6cc41019d715.png" width="50%"/>
</p>
