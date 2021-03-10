<h1 align="center">오늘의 만나 iOS</h1>

![last commit badge](https://img.shields.io/github/last-commit/Jinyeob/Today-Manna-iOS)
<a href='https://developer.apple.com/kr/'><img height="20px" src='http://img.shields.io/badge/platform-iOS-blue.svg'/></a>

<p>
   Provides today's Bible scope, verses and M'Cheine scope.<br/>
   You can see also <a href="https://github.com/Jinyeob/Today-Manna-Android" target="_blank">Android</a> version.
</p>

## Download
<a href="https://apps.apple.com/app/id1547824358"><img src="https://upload.wikimedia.org/wikipedia/commons/3/3c/Download_on_the_App_Store_Badge.svg"/></a>
<br>
![png](https://user-images.githubusercontent.com/37360089/104084022-ce968b80-5286-11eb-8059-47f6dab0e32b.png)


<!--
## 📌 Codes
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
async public void LoginFunc()
        {
            if (string.IsNullOrWhiteSpace(entry1.Text) || string.IsNullOrWhiteSpace(entry2.Text))
            {
                await DisplayAlert("로그인 실패!", "아이디와 비밀번호를 확인해주세요.", "확인");
            }
            else
            {
                _Id = entry1.Text; 
                _Passwd = entry2.Text;
                _IsLogined = true;

                Application.Current.Properties["ID"] = _Id;
                Application.Current.Properties["PASSWD"] = _Passwd;
                Application.Current.Properties["ISLOGINED"] = _IsLogined;

                await Application.Current.SavePropertiesAsync();

                await Navigation.PushAsync(new MainPage());
            }
        }
```

## 📌 Using
* HttpWebResponse (System.net) -> Login to the homepage and parse HTML.
* htmlagilitypack (https://html-agility-pack.net/) -> Select HTML node from HtmlDocument.
* Xamarin.Plugins.Clipboard (https://www.nuget.org/packages/Xamarin.Plugins.Clipboard/) -> Coppy Text to Clipboard
-->

## 📌 Screenshots
<!--
### Application
| Main | M'Cheine | Info |
|:--------:|:--------:|:--------:|
| <img src="https://github.com/Jinyeob/Today-Manna-iOS/blob/master/screenshot/manna.png"/> | <img src="https://github.com/Jinyeob/Today-Manna-iOS/blob/master/screenshot/mchein.png"/> | <img src="https://github.com/Jinyeob/Today-Manna-iOS/blob/master/screenshot/info.png"/> |

| Login | Pull to Refresh | Share |
|:--------:|:--------:|:--------:|
| <img src="https://user-images.githubusercontent.com/37360089/94250945-dee75c80-ff5c-11ea-88dc-196f3dad3450.png"/> | <img src="https://github.com/Jinyeob/Today-Manna-iOS/blob/master/screenshot/refresh.png"/> | <img src="https://github.com/Jinyeob/Today-Manna-iOS/blob/master/screenshot/share.png"/> |

-->
![Simulator Screen Shot - iPhone 12 - 2021-02-23 at 19 13 31](https://user-images.githubusercontent.com/37360089/110575680-a7830a00-81a2-11eb-8ab0-43c095c7031a.png)
![Simulator Screen Shot - iPhone 12 - 2021-02-23 at 19 14 18](https://user-images.githubusercontent.com/37360089/110575682-a81ba080-81a2-11eb-9d8b-b5dc830c4917.png)
![Simulator Screen Shot - iPhone 12 - 2021-02-23 at 19 14 20](https://user-images.githubusercontent.com/37360089/110575684-a8b43700-81a2-11eb-8d4c-a2ffe3a34388.png)
![Simulator Screen Shot - iPhone 12 - 2021-02-23 at 19 14 27](https://user-images.githubusercontent.com/37360089/110575686-a8b43700-81a2-11eb-9598-beaddc799899.png)
![Simulator Screen Shot - iPhone 12 - 2021-02-23 at 19 14 57](https://user-images.githubusercontent.com/37360089/110575689-a94ccd80-81a2-11eb-9e8f-24f63d922888.png)


### 파싱한 웹페이지
| 홈 페이지 | 범위 | 세부 페이지 |
|:--------:|:--------:|:--------:|
| <img src="https://user-images.githubusercontent.com/37360089/92693284-5f646580-f380-11ea-899a-d29efc2d276a.png"/> | <img src="https://user-images.githubusercontent.com/37360089/92693166-3348e480-f380-11ea-9dd2-b0eade042aeb.png"/> | <img src="https://user-images.githubusercontent.com/37360089/92692980-ebc25880-f37f-11ea-8013-6cc41019d715.png"/> |

