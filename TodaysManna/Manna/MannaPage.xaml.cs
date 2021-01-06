using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class MainPage : ContentPage
    {
        RestService _restService;
        private MannaData mannaData;

        public MainPage()
        {
            InitializeComponent();
            _restService = new RestService();

            mannaData.TodayString = DateTime.Now.ToString("yyyy/MM/dd dddd") + "\n";
            GetManna();
        }

        private async void GetManna()
        {
            mannaData = await _restService.GetMannaDataAsync(Constants.MannaEndpoint);
            BindingContext = mannaData;
        }
        public async Task ShareFunc()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = mannaData.TodayString + "\n\n" + mannaData.Verse + "\n\n" + mannaData.AllString,
                Title = "Share Manna"
            }); ;
        }
        //string GenerateRequestUri(string endpoint)
        //{
        //    string requestUri = endpoint;
        //    return requestUri;
        //}
    }
}
