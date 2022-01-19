using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using System.Threading.Tasks;
using System.Diagnostics;
using TodaysManna.Models;
using Rg.Plugins.Popup.Extensions;
using TodaysManna.Controls.Popups;
using TodaysManna.Managers;
using TodaysManna.Views;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Linq;
using Rg.Plugins.Popup.Services;

namespace TodaysManna
{
    public partial class MannaPage : ContentPage
    {
        private List<MannaContent> selectedContentList = new List<MannaContent>();

        public MannaPage()
        {
            InitializeComponent();
            Padding = new Thickness(0, Constants.StatusBarHeight, 0, 0);

            var viewModel = new MannaViewModel();
            BindingContext = viewModel;

            viewModel.IsRefreshing = true;

            MessagingCenter.Subscribe<MainTabbedPage>(this, MessagingCenterMessage.ScrollMannaToTop, (sender) =>
            {
                outerScrollView.ScrollToAsync(0, 0, true);
            });

            if (VersionTracking.IsFirstLaunchEver)
            {
                var onboardingPage = new OnboardingPage();
                onboardingPage.LanguageChanged += GetMannaByLanguage;
                Navigation.PushModalAsync(onboardingPage, false);
            }

            SelectFeaturePopup.Instance.CopybuttonClicked += OnCopyCliked;
            SelectFeaturePopup.Instance.SharebuttonClicked += OnShareCliked;
            SelectFeaturePopup.Instance.MemobuttonClicked += OnMemoClicked;
            SelectFeaturePopup.Instance.CancelbuttonClicked += OnCancelClicked;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!(BindingContext is MannaViewModel viewModel)) return;
            viewModel.CustomFontSize = AppManager.GetCurrentTextSize();
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.IsLoadingServer = true;

            await ResetSelection();

            var result = await MannaDataManager.GetManna(DateTime.Now);

            viewModel.IsRefreshing = false;

            if (!result)
            {
                await Application.Current.MainPage.DisplayAlert("불러오기 실패", "다시 시도 해주세요", "확인");
            }
            else
            {
                SetContentsByLanguage(AppManager.GetCurrentLanguageString());
            }
            viewModel.IsLoadingServer = false;
        }

        private async void OnSelectAllButtonClicked(object sender, EventArgs e)
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            if (viewModel.IsAllSelected) //다 선택되어 있을 때
            {
                viewModel.IsAllSelected = false;
                await ResetSelection();
            }
            else
            {
                viewModel.IsAllSelected = true;

                viewModel.MannaContents?.All(manna => manna.Selected = true);
                selectedContentList = viewModel.MannaContents.ToList();

                if (AppManager.IsPopupNavigationNullOrExist()) return;
                await Navigation.PushPopupAsync(SelectFeaturePopup.Instance);
            }
        }

        private async void OnSettingClicked(object sender, EventArgs e)
        {
            await ResetSelection();
            var settingPage = new SettingPage();
            settingPage.LanguageChanged += GetMannaByLanguage;
            await Navigation.PushAsync(settingPage);
        }

        private async void GetMannaByLanguage(object sender, Language language)
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.IsLoadingServer = true;

            await MannaDataManager.GetManna(language);
            SetContentsByLanguage(language.ToString());

            viewModel.IsLoadingServer = false;
        }

        private void SetContentsByLanguage(string language)
        {
            if (language == Language.Korean.ToString())
            {
                SetKoreanMannaContents();
            }
            else if (language == Language.Spanish.ToString())
            {
                SetSpanishMannaContents();
            }
            else if (language == Language.Chinese.ToString())
            {
                SetChineseMannaContents();
            }
            else if (language == Language.Japanese.ToString())
            {
                SetJapaneseMannaContents();
            }
            else if (language == Language.German.ToString())
            {
                SetGermanMannaContents();
            }
            else if (language == Language.French.ToString())
            {
                SetFrenchMannaContents();
            }
            else if (language == Language.Hindi.ToString())
            {
                SetHindiMannaContents();
            }
            else
            {
                SetEnglishMannaContents();
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            var selected = false;

            try
            {
                var mannaContent = ((TappedEventArgs)e).Parameter as MannaContent;
                selected = !mannaContent.Selected;
                mannaContent.Selected = selected;

                if (selected)
                {
                    selectedContentList?.Add(mannaContent);
                }
                else
                {
                    selectedContentList?.Remove(mannaContent);
                }
            }
            catch (Exception ex)
            {
                AppManager.PrintException("selectedContentList add/remove", ex.Message);
            }

            if (viewModel.MannaContents?.All(x => x.Selected) ?? false)
            {
                viewModel.IsAllSelected = true;
            }
            else
            {
                viewModel.IsAllSelected = false;
            }

            try
            {
                if (selected && (selectedContentList?.Count ?? 0) == 1)
                {
                    if (AppManager.IsPopupNavigationNullOrExist()) return;
                    await Navigation.PushPopupAsync(SelectFeaturePopup.Instance);
                }
                else if (!selected && (selectedContentList?.Count ?? 0) == 0)
                {
                    await Navigation.RemovePopupPageAsync(SelectFeaturePopup.Instance);
                }
            }
            catch (Exception ex)
            {
                AppManager.PrintException("popup push/remove", ex.Message);
            }
        }

        private async void OnCopyCliked(object sender, EventArgs e)
        {
            var selectedVersesText = GetSelectedMannaShareVersesText();
            var selectedContentsText = GetSelectedMannaShareContentsText();

            await ResetSelection();

            await Clipboard.SetTextAsync(selectedContentsText);

            string title = "";
            string ok = "";
            if (AppManager.GetCurrentLanguageString() == Language.Korean.ToString())
            {
                title = "클립보드에 복사됨";
                ok = "확인";
            }
            else
            {
                title = "Copied to clipboard";
                ok = "Ok";
            }

            await DisplayAlert(title, selectedVersesText, ok);
        }

        private async void OnShareCliked(object sender, EventArgs e)
        {
            var selectedContentsText = GetSelectedMannaShareContentsText();
            await ResetSelection();

            string title = "";
            if (AppManager.GetCurrentLanguageString() == Language.Korean.ToString())
            {
                title = "공유";
            }
            else
            {
                title = "Share";
            }

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = DateTime.Today.ToString("yy-MM-dd") + "\n\n" + selectedContentsText,
                Title = title
            });
        }

        private async void OnMemoClicked(object sender, EventArgs e)
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            await ResetSelection();

            var memoPage = new MemoAddPage();
            memoPage.SetBibleText(GetSelectedMannaShareContentsText());
            memoPage.SaveButtonClicked += OnMemoPopupSaveButtonClicked;
           
            await Navigation.PushAsync(memoPage);
        }

        private async void OnMemoPopupSaveButtonClicked(object sender, (string, string) memoText)
        {
            var memoItem = new MemoItem
            {
                Date = DateTime.Now,
                Verse = memoText.Item1,
                Note = memoText.Item2
            };
            await DatabaseManager.Database.SaveItemAsync(memoItem);
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await ResetSelection();
        }
        

        private string GetSelectedMannaShareVersesText()
        {
            try
            {
                var selectedVersesText = string.Join(", ", selectedContentList.Select(x => $"{x.BookAndJang}:{x.Jeol}").ToArray());
                Debug.WriteLine(selectedVersesText);
                return selectedVersesText;
            }
            catch (Exception ex)
            {
                AppManager.PrintException("GetSelectedMannaShare Verses Text()", ex.Message);
                return string.Empty;
            }
        }

        private string GetSelectedMannaShareContentsText()
        {
            if (!(BindingContext is MannaViewModel viewModel)) return string.Empty;

            try
            {
                var selectedContentsText = string.Join("\n", selectedContentList.Select(x => $"{x.BookAndJang}:{x.Jeol} {x.MannaString}").ToArray());

                if (viewModel?.IsAllSelected ?? false)
                {
                    var contentsTextWithOnlyJeol = string.Join("\n", selectedContentList.Select(x => $"{x.Jeol} {x.MannaString}").ToArray());
                    selectedContentsText = $"{viewModel.MannaRange}\n{contentsTextWithOnlyJeol}";
                }

                Debug.WriteLine(selectedContentsText);
                return selectedContentsText;
            }
            catch (Exception ex)
            {
                AppManager.PrintException("GetSelectedMannaShare Contents Text()", ex.Message);
                return string.Empty;
            }
        }

        private void SetEnglishMannaContents()
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.MannaContents = MannaDataManager.EnglishMannaContents;
            viewModel.MannaRange = MannaDataManager.EnglishMannaData.Reference;
        }

        private void SetKoreanMannaContents()
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.MannaContents = MannaDataManager.KoreanMannaContents;
            viewModel.MannaRange = MannaDataManager.KoreanMannaData.Verse;
        }

        private void SetSpanishMannaContents()
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.MannaContents = MannaDataManager.SpanishMannaContents;
            viewModel.MannaRange = MannaDataManager.SpanishRange;
        }

        private void SetChineseMannaContents()
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.MannaContents = MannaDataManager.ChineseMannaContents;
            viewModel.MannaRange = MannaDataManager.ChineseRange;
        }

        private void SetJapaneseMannaContents()
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.MannaContents = MannaDataManager.JapaneseMannaContents;
            viewModel.MannaRange = MannaDataManager.JapaneseRange;
        }

        private void SetGermanMannaContents()
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.MannaContents = MannaDataManager.GermanMannaContents;
            viewModel.MannaRange = MannaDataManager.GermanRange;
        }

        private void SetFrenchMannaContents()
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.MannaContents = MannaDataManager.FrenchMannaContents;
            viewModel.MannaRange = MannaDataManager.FrenchRange;
        }

        private void SetHindiMannaContents()
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.MannaContents = MannaDataManager.HindiMannaContents;
            viewModel.MannaRange = MannaDataManager.HindiRange;
        }

        private async Task ResetSelection()
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            try
            {
                selectedContentList = new List<MannaContent>();

                var mannaContents = viewModel.MannaContents ?? (viewModel.MannaContents = new ObservableRangeCollection<MannaContent>());
                mannaContents.ToList().ForEach(manna => manna.Selected = false);

                viewModel.IsAllSelected = false;

                try
                {
                    await Navigation.PopAllPopupAsync();
                }
                catch { }
            }
            catch (Exception ex)
            {
                AppManager.PrintException("ResetSelection", ex.Message);
            }
        }

        //protected override void OnDisappearing()
        //{
        //    ResetSelectedItemsAndPopPopups();
        //    base.OnDisappearing();
        //}

        //private async void OnCopyButtonClicked(object sender, EventArgs e)
        //{
        //    DependencyService.Get<IHapticFeedback>().Run();
        //    FirebaseEventService.SendEventOnPlatformSpecific("manna_text_copy");

        //    await Clipboard.SetTextAsync(shareRangeString);
        //    await DisplayAlert("클립보드에 복사됨", null, "확인");

        //    ResetSelectedItemsAndPopPopups();
        //}

        //private async void OnMemoButtonClicked(object sender, EventArgs e)
        //{
        //    DependencyService.Get<IHapticFeedback>().Run();
        //    FirebaseEventService.SendEventOnPlatformSpecific("manna_text_memo");

        //    var memoPage = new MemoAddPage();
        //    memoPage.SetBibleText(shareRangeString);
        //    memoPage.SaveButtonClicked += OnMemoPopupSaveButtonClicked;

        //    if (PopupNavigation.Instance.PopupStack.Count > 0)
        //    {
        //        try
        //        {
        //            await Navigation.PopAllPopupAsync();
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.Fail($"{this}\n{ex.Message}");
        //        }
        //    }

        //    await Navigation.PushAsync(memoPage);
        //}

        //private async void OnTextShareButtonClicked(object sender, EventArgs e)
        //{
        //    DependencyService.Get<IHapticFeedback>().Run();
        //    FirebaseEventService.SendEventOnPlatformSpecific("manna_text_share");

        //    await Share.RequestAsync(new ShareTextRequest
        //    {
        //        Text = shareRangeString,
        //        Title = "공유"
        //    });
        //}

        //private void OnCancelButtonClicked(object sender, EventArgs e)
        //{
        //    DependencyService.Get<IHapticFeedback>().Run();
        //    ResetSelectedItemsAndPopPopups();
        //}

        //private async void OnMemoPopupSaveButtonClicked(object sender, (string,string) memoText)
        //{
        //    DependencyService.Get<IHapticFeedback>().Run();
        //    var memoItem = new MemoItem
        //    {
        //        Date = DateTime.Now,
        //        Verse = memoText.Item1,
        //        Note = memoText.Item2
        //    };
        //    await DatabaseManager.Database.SaveItemAsync(memoItem);

        //    //mannaCollectionView.SelectedItems.Clear();
        //}

        //private async void ResetSelectedItemsAndPopPopups()
        //{
        //    //mannaCollectionView.SelectedItems.Clear();
        //    if (PopupNavigation.Instance.PopupStack.Count > 0)
        //    {
        //        try
        //        {
        //            await Navigation.PopAllPopupAsync();
        //        }
        //        catch(Exception ex)
        //        {
        //            Debug.Fail($"{this}\n{ex.Message}");
        //        }
        //    }
        //}

        //private async void OnMannaTextTapped(object sender, EventArgs e)
        //{
        //    var selected = false;

        //    try
        //    {
        //        var mannaContent = ((TappedEventArgs)e).Parameter as MannaContent;
        //        selected = !mannaContent.Selected;
        //        mannaContent.Selected = selected;

        //        if (selected)
        //        {
        //            selectedContentList?.Add(mannaContent);
        //        }
        //        else
        //        {
        //            selectedContentList?.Remove(mannaContent);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AppManager.PrintException("selectedContentList add/remove", ex.Message);
        //    }

        //    if (!(BindingContext is MannaViewModel viewModel)) return;
        //    if (viewModel.MannaContents?.All(x => x.Selected) ?? false)
        //    {
        //        viewModel.IsAllSelected = true;
        //    }
        //    else
        //    {
        //        viewModel.IsAllSelected = false;
        //    }

        //    try
        //    {
        //        if (selected && (selectedContentList?.Count ?? 0) == 1)
        //        {
        //            if (AppManager.IsPopupNavigationNullOrExist()) return;
        //            await Navigation.PushPopupAsync(SelectFeaturePopup.Instance);
        //        }
        //        else if (!selected && (selectedContentList?.Count ?? 0) == 0)
        //        {
        //            await Navigation.RemovePopupPageAsync(SelectFeaturePopup.Instance);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AppManager.PrintException("popup push/remove", ex.Message);
        //    }
        //}
    }
}