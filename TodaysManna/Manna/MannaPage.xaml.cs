using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
                var onlyNum =int.Parse(Regex.Replace(node, @"\D", ""));
                var onlystring = Regex.Replace(node, @"\d", "");
                //(int, string)contentTuple = (int.Parse(Regex.Replace(node.Substring(0, 2), @"\s", "")), node.Substring(2));
                (int, string) contentTuple = (onlyNum, onlystring);

                mannaData.Content.Add(contentTuple);
                System.Diagnostics.Debug.WriteLine(contentTuple.Item1);

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
