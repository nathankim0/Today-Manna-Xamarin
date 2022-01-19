using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TodaysManna.Models;

namespace TodaysManna
{
    public class RestService
    {
        private static RestService _instance;
        public static RestService Instance => _instance ?? (_instance = new RestService());

        private readonly HttpClient _client;
        Random random;

        public RestService()
        {
            random = new Random();

            _client = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            });
        }

        public async Task<KoreanManna> GetMannaDataAsync(string uri)
        {
            var mannaData = new KoreanManna();

            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    mannaData = JsonConvert.DeserializeObject<KoreanManna>(content);

                    Console.WriteLine($"@@@@@{mannaData.Verse}");
                    foreach (var node in mannaData.Contents)
                    {
                        Console.WriteLine($"@@@@@{node}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return mannaData;
        }

        private string GetApiUrl(string bible, string bookKor, int jang, string jeolRange)
        {
            // bible : kjv
            // bookKor : 창
            // jang : 1
            // jeolRange : 1-10
            var endpoint = "https://api.biblesupersearch.com/";
            return $"{endpoint}api?bible={bible}&reference={bookKor.BibleBookKorToEng()}{jang}:{jeolRange}&data_format=minimal";
        }

        private string GetApiUrlForGettingBook(string bible, string bookKor, int jang, string jeolRange)
        {
            // bible : kjv
            // bookKor : 창
            // jang : 1
            // jeolRange : 1-10
            var endpoint = "https://api.biblesupersearch.com/";
            return $"{endpoint}api?bible={bible}&reference={bookKor.BibleBookKorToEng()}{jang}:{jeolRange}&data_format=passage";
        }

        public async Task<EnglishManna> GetEnglishManna(string bookKor, int jang, string jeolRange)
        {
            var url = $"https://bible-api.com/{bookKor.BibleBookKorToEng()}+{jang}:{jeolRange}?translation=kjv";
            try
            {
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var mannaData = JsonConvert.DeserializeObject<EnglishManna>(content);
                    foreach (var node in mannaData.Verses)
                    {
                        try
                        {
                            node.Text = node.Text.TrimStart().TrimEnd().Replace("\n", "");
                            Debug.WriteLine(node.Text);
                        }
                        catch (Exception e) { AppManager.PrintException("GetMultilanguageManna() trim", e.Message); }
                    }

                    return mannaData;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return new EnglishManna();
        }

        public async Task<SpanishManna> GetSpanishManna(string bookKor, int jang, string jeolRange)
        {
            var url = GetApiUrl("rv_1909", bookKor, jang, jeolRange);
            var urlForBook = GetApiUrlForGettingBook("rv_1909", bookKor, jang, jeolRange);
            try
            {
                var task1 = _client.GetAsync(urlForBook);
                var task2 = _client.GetAsync(url);
                await Task.WhenAll(task1, task2);

                var book = "";
                if (task1.Result.IsSuccessStatusCode)
                {
                    var content = await task1.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var bookResult = JsonConvert.DeserializeObject<BookResult>(content);
                    if (bookResult != null && bookResult.Results != null && (bookResult?.Results?.Count ?? 0) != 0)
                    {
                        book = bookResult.Results[0].BookName;
                    }
                }

                if (task2.Result.IsSuccessStatusCode)
                {
                    var content = await task2.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var mannaData = JsonConvert.DeserializeObject<SpanishManna>(content);
                    foreach (var node in mannaData.Results.Content)
                    {
                        Debug.WriteLine(node.Text);
                        node.BookName = book;
                    }
                    return mannaData;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return new SpanishManna();
        }

        public async Task<ChineseManna> GetChineseManna(string bookKor, int jang, string jeolRange)
        {
            var url = GetApiUrl("ckjv_sdt", bookKor, jang, jeolRange);
            var urlForBook = GetApiUrlForGettingBook("ckjv_sdt", bookKor, jang, jeolRange);

            try
            {
                var task1 = _client.GetAsync(urlForBook);
                var task2 = _client.GetAsync(url);
                await Task.WhenAll(task1, task2);

                var book = "";
                if (task1.Result.IsSuccessStatusCode)
                {
                    var content = await task1.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var bookResult = JsonConvert.DeserializeObject<BookResult>(content);
                    if (bookResult != null && bookResult.Results != null && (bookResult?.Results?.Count ?? 0) != 0)
                    {
                        book = bookResult.Results[0].BookName;
                    }
                }

                if (task2.Result.IsSuccessStatusCode)
                {
                    var content = await task2.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var mannaData = JsonConvert.DeserializeObject<ChineseManna>(content);
                    foreach (var node in mannaData.Results.Content)
                    {
                        Debug.WriteLine(node.Text);
                        node.BookName = book;
                    }
                    return mannaData;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return new ChineseManna();
        }

        public async Task<JapaneseManna> GetJapaneseManna(string bookKor, int jang, string jeolRange)
        {
            var url = GetApiUrl("kougo", bookKor, jang, jeolRange);
            var urlForBook = GetApiUrlForGettingBook("kougo", bookKor, jang, jeolRange);

            try
            {
                var task1 = _client.GetAsync(urlForBook);
                var task2 = _client.GetAsync(url);
                await Task.WhenAll(task1, task2);

                var book = "";
                if (task1.Result.IsSuccessStatusCode)
                {
                    var content = await task1.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var bookResult = JsonConvert.DeserializeObject<BookResult>(content);
                    if (bookResult != null && bookResult.Results != null && (bookResult?.Results?.Count ?? 0) != 0)
                    {
                        book = bookResult.Results[0].BookName;
                    }
                }

                if (task2.Result.IsSuccessStatusCode)
                {
                    var content = await task2.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var mannaData = JsonConvert.DeserializeObject<JapaneseManna>(content);
                    foreach (var node in mannaData.Results.Content)
                    {
                        Debug.WriteLine(node.Text);
                        node.BookName = book;
                    }
                    return mannaData;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return new JapaneseManna();
        }

        public async Task<GermanManna> GetGermanManna(string bookKor, int jang, string jeolRange)
        {
            var url = GetApiUrl("luther", bookKor, jang, jeolRange);
            var urlForBook = GetApiUrlForGettingBook("luther", bookKor, jang, jeolRange);

            try
            {
                var task1 = _client.GetAsync(urlForBook);
                var task2 = _client.GetAsync(url);
                await Task.WhenAll(task1, task2);

                var book = "";
                if (task1.Result.IsSuccessStatusCode)
                {
                    var content = await task1.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var bookResult = JsonConvert.DeserializeObject<BookResult>(content);
                    if (bookResult != null && bookResult.Results != null && (bookResult?.Results?.Count ?? 0) != 0)
                    {
                        book = bookResult.Results[0].BookName;
                    }
                }

                if (task2.Result.IsSuccessStatusCode)
                {
                    var content = await task2.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var mannaData = JsonConvert.DeserializeObject<GermanManna>(content);
                    foreach (var node in mannaData.Results.Content)
                    {
                        Debug.WriteLine(node.Text);
                        node.BookName = book;
                    }
                    return mannaData;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return new GermanManna();
        }

        public async Task<FrenchManna> GetFrenchManna(string bookKor, int jang, string jeolRange)
        {
            var url = GetApiUrl("segond_1910", bookKor, jang, jeolRange);
            var urlForBook = GetApiUrlForGettingBook("segond_1910", bookKor, jang, jeolRange);

            try
            {
                var task1 = _client.GetAsync(urlForBook);
                var task2 = _client.GetAsync(url);
                await Task.WhenAll(task1, task2);

                var book = "";
                if (task1.Result.IsSuccessStatusCode)
                {
                    var content = await task1.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var bookResult = JsonConvert.DeserializeObject<BookResult>(content);
                    if (bookResult != null && bookResult.Results != null && (bookResult?.Results?.Count ?? 0) != 0)
                    {
                        book = bookResult.Results[0].BookName;
                    }
                }

                if (task2.Result.IsSuccessStatusCode)
                {
                    var content = await task2.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var mannaData = JsonConvert.DeserializeObject<FrenchManna>(content);
                    foreach (var node in mannaData.Results.Content)
                    {
                        Debug.WriteLine(node.Text);
                        node.BookName = book;
                    }
                    return mannaData;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return new FrenchManna();
        }

        public async Task<HindiManna> GetHindiManna(string bookKor, int jang, string jeolRange)
        {
            var url = GetApiUrl("irv", bookKor, jang, jeolRange);
            var urlForBook = GetApiUrlForGettingBook("irv", bookKor, jang, jeolRange);

            try
            {
                var task1 = _client.GetAsync(urlForBook);
                var task2 = _client.GetAsync(url);
                await Task.WhenAll(task1, task2);

                var book = "";
                if (task1.Result.IsSuccessStatusCode)
                {
                    var content = await task1.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var bookResult = JsonConvert.DeserializeObject<BookResult>(content);
                    if (bookResult != null && bookResult.Results != null && (bookResult?.Results?.Count ?? 0) != 0)
                    {
                        book = bookResult.Results[0].BookName;
                    }
                }

                if (task2.Result.IsSuccessStatusCode)
                {
                    var content = await task2.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    var mannaData = JsonConvert.DeserializeObject<HindiManna>(content);
                    foreach (var node in mannaData.Results.Content)
                    {
                        Debug.WriteLine(node.Text);
                        node.BookName = book;
                    }
                    return mannaData;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return new HindiManna();
        }
    }
}
