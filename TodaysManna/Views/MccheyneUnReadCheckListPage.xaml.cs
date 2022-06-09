using System;
using System.Diagnostics;
using TodaysManna.Models;
using TodaysManna.ViewModel;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class MccheyneUnReadCheckListPage : ContentPage
    {
        public MccheyneUnReadCheckListPage()
        {
            InitializeComponent();
            var viewModel = new MccheyneUnReadCheckViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            if (!(BindingContext is MccheyneUnReadCheckViewModel viewModel)) return;
            viewModel.InitCheckList();
            base.OnAppearing();
        }

        private void OnDateTapped(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("checklist_go_to_read");

            var mccheyneCheckListContent = ((TappedEventArgs)e).Parameter as MccheyneCheckListContent;
            var date = mccheyneCheckListContent.Date;

            try
            {
                var masterPage = Application.Current.MainPage.Navigation.NavigationStack[0] as TabbedPage;
                masterPage.CurrentPage = masterPage.Children[1];

                var toConvertDateTime = $"{DateTime.Today.Year}-{date}";
                MessagingCenter.Send(this, "goToReadTapped", Convert.ToDateTime(toConvertDateTime));
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        void OnMccheyneCheckTapped(object sender, EventArgs e)
        {
            var mccheyneOneRange = ((TappedEventArgs)e).Parameter as MccheyneOneRange;
            mccheyneOneRange.IsChecked = !mccheyneOneRange.IsChecked;
        }
    }
}
