using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TodaysManna
{
    public class RestService
    {
        private readonly HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public async Task<MannaData> GetMannaDataAsync(string uri)
        {
            MannaData mannaData = new MannaData();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    mannaData = JsonConvert.DeserializeObject<MannaData>(content);
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
    }
}
