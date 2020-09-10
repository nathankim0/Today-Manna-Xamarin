<h1 align="center">📖 오늘의 만나 📖 </h1>

> 홈페이지에 로그인 후 묵상 범위를 가져옵니다. </br>

## 📌 핵심코드
---
* 웹페이지 로그인
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

* Html select
``` csharp
var tmp = htmlDoc2.DocumentNode.SelectSingleNode("//div[@class='contentbox fr-view']/p").InnerHtml;
```


* post_uid 얻기
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

* <br>태그 줄 바꿈 변환
``` csharp
tmp = Regex.Replace(tmp, @"<br>", "\n\n");
```

* login
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

## 📌 데모
---
<p>
<img src="https://github.com/Jinyeob/Today-Manna-Hybrid/blob/master/video.gif" width="40%"/>
</p>

