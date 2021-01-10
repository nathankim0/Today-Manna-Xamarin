using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class MannaPage : ContentPage
    {
        private readonly RestService _restService;
        private MannaData mannaData;

        public MannaPage()
        {
            InitializeComponent();
            _restService = new RestService();

            GetManna();
        }

        private async void GetManna()
        {
            mannaData = new MannaData();

            mannaData = await _restService.GetMannaDataAsync(Constants.MannaEndpoint);

            var contents = "";
            foreach(var node in mannaData.Contents)
            {
                contents += node+"\n\n";
            }

            mannaData.TodayString = DateTime.Now.ToString("yyyy/MM/dd dddd") + "\n";
            mannaData.AllString = contents;

            BindingContext = mannaData;
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = mannaData.TodayString + "\n\n" + mannaData.Verse + "\n\n" + mannaData.AllString,
                Title = "Share Manna"
            }); ;
        }
    }
}
