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
        private readonly HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public async Task<JsonMannaModel> GetMannaDataAsync(string uri)
        {
            JsonMannaModel mannaData = new JsonMannaModel();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    mannaData = JsonConvert.DeserializeObject<JsonMannaModel>(content);
                    Console.WriteLine($"@@@@@{mannaData.Verse}");
                    foreach(var node in mannaData.Contents)
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
    }
}
