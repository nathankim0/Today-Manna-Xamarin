using System;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Essentials;
using ListView = Xamarin.Forms.ListView;
using Rg.Plugins.Popup.Services;
using TodaysManna.ViewModel;
using TodaysManna.Models;
using System.Threading.Tasks;
using TodaysManna.Constants;

namespace TodaysManna
{
    public partial class MccheynePage : ContentPage
    {
        private readonly MannaTextClickSheet _mannaTextClickSheet;
        private readonly MemoPopup _memoPopup;

        private string shareRangeString = "";

        public MccheynePage()
        {
            InitializeComponent();
            BindingContext = new MccheyneViewModel();

            _mannaTextClickSheet = new MannaTextClickSheet();

            _mannaTextClickSheet.coppybuttonClicked += OnCoppyButtonClicked;
            _mannaTextClickSheet.memobuttonClicked += OnMemoButtonClicked;
            _mannaTextClickSheet.sharebuttonClicked += OnTextShareButtonClicked;
            _mannaTextClickSheet.savebuttonClicked += OnSaveButtonClicked;
            _mannaTextClickSheet.cancelbuttonClicked += OnCancelButtonClicked;

            _memoPopup = new MemoPopup();
            _memoPopup.SaveButtonClicked += OnSaveButtonClicked;

            MessagingCenter.Subscribe<MainTabbedPage>(this, MessagingCenterMessage.ScrollMccheyneToTop, (sender) =>
            {
                //MccheyneViewSctollToTop(true);
            });
        }

        private async void OnDatePickerDateSelected(object sender, DateChangedEventArgs e)
        {
            if (!(BindingContext is MccheyneViewModel viewModel)) return;

            viewModel.Today = e.NewDate.ToString("M_d");
            var thisDate = MccheyneViewModel.GetCorrectDateLeapYear(e.NewDate);

            viewModel.IsRefreshing = true;

            var task1 = viewModel.GetMccheyne(thisDate);
            var task2 = viewModel.GetMccheyneRange(thisDate);
            await Task.WhenAll(task1, task2);

            viewModel.IsRefreshing = false;
        }

        private void OnTodayButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_today");

            datepicker.Date = DateTime.Now;
        }

        private void OnDateButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_date");
            datepicker.Focus();
        }

        //private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    if (e.SelectedItem == null) return;

        //    var mccheyne = e.SelectedItem as MccheyneContent;
        //    shareRangeString = $"({mccheyne.Book}{mccheyne.Verse}) {mccheyne.Content}\n";

        //    _mannaTextClickSheet.textLabel.Text = shareRangeString;

        //    //_bottomSheet.Show();

        //    ((ListView)sender).SelectedItem = null;
        //}

        private async void OnReportTapped(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_report");

            var address = "jinyeob07@gmail.com";
            await Clipboard.SetTextAsync(address);
            await DisplayAlert("클립보드에 복사됨", address, "확인");
        }

        private void OnBackgroundTapped(object sender, EventArgs e)
        {
            datepicker.Unfocus();
        }

        private void OnDatepickerUnfocused(object sender, FocusEventArgs e)
        {
        }

        private async void OnMemoButtonClicked(object sender, EventArgs e)
        {
            //_bottomSheet.Hide();
            _memoPopup.SetBibleText(shareRangeString);
            await PopupNavigation.Instance.PushAsync(_memoPopup);
        }

        private async void OnCoppyButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_text_coppy");

            await Clipboard.SetTextAsync(shareRangeString);
            await DisplayAlert("클립보드에 복사됨", null, "확인");
        }

        private async void OnTextShareButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_text_share");

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = shareRangeString,
                Title = "공유"
            });
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            //_bottomSheet.Hide();

            if (!await DisplayAlert("", "저장하시겠습니까?", "저장", "취소"))
            {
                //_bottomSheet.Show();
                return;
            }

            var memoItem = new MemoItem
            {
                Date = DateTime.Now,
                Verse = shareRangeString,
                Note = ""
            };
            await App.Database.SaveItemAsync(memoItem);
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            //_bottomSheet.Hide();
        }

        private async void OnSaveButtonClicked(object sender, string memoText)
        {
            var memoItem = new MemoItem
            {
                Date = DateTime.Now,
                Verse = shareRangeString,
                Note = memoText
            };
            await App.Database.SaveItemAsync(memoItem);
        }
    }
}
