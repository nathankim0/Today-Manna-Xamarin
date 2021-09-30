using System;
using System.Collections.Generic;
using TodaysManna.Managers;
using TodaysManna.Models;
using TodaysManna.ViewModelV2;
using Xamarin.Forms;

namespace TodaysManna.ViewsV2
{
    public partial class CheckListPage : BaseContentPage
    {
        public CheckListPage()
        {
            InitializeComponent();
            BindingContext = new MccheyneCheckListPageViewModel();
        }

        private async void OnCheckButtonTapped(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();

            var parameter = ((TappedEventArgs)e).Parameter;
            var val = parameter as CheckViewModel;

            val.IsChecked = !val.IsChecked;

            var mccheyneCheckListLocalContent = new MccheyneCheckListLocalContent
            {
                Date = val.Date,
                IsChecked = val.IsChecked,
                RangeText = val.RangeText
            };

            await DatabaseManager.CheckListDatabase.SaveItemAsync(mccheyneCheckListLocalContent);
        }
    }
}
