using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using TodaysManna.Models;
using static TodaysManna.Models.MccheyneCheckData;

namespace TodaysManna.Views
{
    public partial class SettingPage : ContentPage
    {
        List<MccheyneCheckRange> mccheyneCheckRangeList;

        public List<MccheyneCheckContent> _mccheyneCheckList = new List<MccheyneCheckContent>();


        public SettingPage()
        {
          //  On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.PageSheet);

            //InitializeComponent();
            //BindingContext = new MccheyneCheckViewModel();

            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width=new GridLength(30) }
                }
            };
            for (int i = 0; i < 367; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            }
            for (int i = 0; i < 5; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
            }
            mccheyneCheckRangeList = new List<MccheyneCheckRange>();
            try
            {
                mccheyneCheckRangeList = GetJsonMccheyneRange();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("GetJsonMccheyneRange() Error");
            }
            int row = 0;
            foreach (var range in mccheyneCheckRangeList)
            {
                grid.Children.Add(new Label
                {
                    Text = range.Date,
                    BackgroundColor = Color.LightBlue
                }, 0, row);

                _mccheyneCheckList.Add(new MccheyneCheckContent
                {
                    Date = range.Date,
                    IsChecked = false,
                    Range = range.Range1
                });
                grid.Children.Add(new Label
                {
                    Text = range.Range1
                }, 1, row);

                _mccheyneCheckList.Add(new MccheyneCheckContent
                {
                    Date = range.Date,
                    IsChecked = false,
                    Range = range.Range2
                });
                grid.Children.Add(new Label
                {
                    Text = range.Range1
                }, 2, row);

                _mccheyneCheckList.Add(new MccheyneCheckContent
                {
                    Date = range.Date,
                    IsChecked = false,
                    Range = range.Range3
                });
                grid.Children.Add(new Label
                {
                    Text = range.Range1
                }, 3, row);

                _mccheyneCheckList.Add(new MccheyneCheckContent
                {
                    Date = range.Date,
                    IsChecked = false,
                    Range = range.Range4
                });
                grid.Children.Add(new Label
                {
                    Text = range.Range1
                }, 4, row);

                _mccheyneCheckList.Add(new MccheyneCheckContent
                {
                    Date = range.Date,
                    IsChecked = false,
                    Range = range.Range5
                });
                grid.Children.Add(new Label
                {
                    Text = range.Range1
                }, 5, row);

                row++;
            }
            System.Diagnostics.Debug.WriteLine("Done!");

            Content = new Xamarin.Forms.ScrollView
            {
              Content=  new StackLayout
                {
                    Children = { grid }
                }
            };
        }

        private List<MccheyneCheckRange> GetJsonMccheyneRange()
        {
            string jsonFileName = "MccheyneRange2.json";
            MccheyneCheckRangeList ObjContactList = new MccheyneCheckRangeList();


            var assembly = typeof(SettingPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();

                //Converting JSON Array Objects into generic list    
                ObjContactList = JsonConvert.DeserializeObject<MccheyneCheckRangeList>(jsonString);
            }

            return ObjContactList.CheckRanges;
        }

        private async void MaterialChip_ActionImageTapped(System.Object sender, System.EventArgs e)
        {
            await DisplayAlert("dasd", null, "ok");
        }
    }
}