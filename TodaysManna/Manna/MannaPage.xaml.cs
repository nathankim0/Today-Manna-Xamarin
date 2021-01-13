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

        public List<MannaContent> mannaContents;
        private MannaData mannaData;

        private string _today="";
        private string _allString="";

        public MannaPage()
        {
            InitializeComponent();
            _restService = new RestService();
            mannaContents = new List<MannaContent>();

            _today = DateTime.Now.ToString("yyyy/MM/dd dddd") + "\n";
            today.Text = _today;

            GetManna();

            BindingContext = this;
        }

        private async void GetManna()
        {
            mannaData = new MannaData();
            mannaData = await _restService.GetMannaDataAsync(Constants.MannaEndpoint);

            SetMannaContents();
        }

        private void SetMannaContents()
        {
            var contents = "";

            foreach (var node in mannaData.Contents)
            {
                
                var onlyNum = int.Parse(Regex.Replace(node, @"\D", ""));
                var onlyString = Regex.Replace(node, @"\d", "");

                var tmpManna = new MannaContent
                {
                    Number = onlyNum,
                    MannaString = onlyString,
                };

                mannaContents.Add(tmpManna);

                //(int, string) contentTuple = (onlyNum, onlystring);
                // mannaData.Content.Add(contentTuple);
                // System.Diagnostics.Debug.WriteLine(contentTuple.Item1);

                contents += node + "\n\n";
            }
            listView.ItemsSource = mannaContents;
            listView.HeightRequest = mannaContents.Count * 70;
            _allString = contents;
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = _today + "\n\n" + mannaData.Verse + "\n\n" +_allString,
                Title = "공유"
            }); ;
        }
    }
}
