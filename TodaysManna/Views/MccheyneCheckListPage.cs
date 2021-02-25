using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using System.Linq;
using System;
using NavigationPage = Xamarin.Forms.NavigationPage;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using TodaysManna.Models;

namespace TodaysManna.Views
{
    public partial class MccheyneCheckListPage : ContentPage
    {
        private CollectionView _collectionView;
        private readonly OptionPopup _optionPopup;
        private MccheyneCheckContent _todayMccheyne;

        public MccheyneCheckListPage()
        {
            BindingContext = App.mccheyneCheckViewModel;

            Initialize();
            _todayMccheyne = App.mccheyneCheckViewModel.MccheyneCheckList.Where(x => x.Date == DateTime.Now.ToString("M-d")).FirstOrDefault();

            _optionPopup = new OptionPopup();
            _optionPopup.CheckButtonClicked += OnCheckButtonClicked;
            _optionPopup.ClearButtonClicked += OnClearButtonClicked;
        }

        protected override void OnAppearing()
        {
            ScrollToToday();
        }

        private void Initialize()
        {
            On<iOS>().SetUseSafeArea(true);
            On<iOS>().SetPrefersHomeIndicatorAutoHidden(true);
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.PageSheet);

            Title = "체크리스트";
            IconImageSource = "tab_mc";
            this.SetAppThemeColor(BackgroundColorProperty, Color.White, Color.FromHex("#2e2e2e"));

            NavigationPage.SetBackButtonTitle(this, "");
            NavigationPage.SetHasNavigationBar(this, true);

            var titleLabel = new Label
            {
                BackgroundColor = Color.Transparent,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Start,
                Text = "체크리스트",
                VerticalOptions = LayoutOptions.Center
            };

            var titleOptionButton = new ImageButton
            {
                Margin = new Thickness(0, 0, 5, 0),
                Padding = 8,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            var titleOptionButtonFontImageSource = new FontImageSource
            {
                Glyph = FontIcons.AppleKeyboardOption,
                FontFamily = "materialdesignicons",
            };

            titleOptionButtonFontImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);
            titleOptionButton.Source = titleOptionButtonFontImageSource;
            titleOptionButton.Clicked += OnOptionClicked;

            var titleStackLayout = new StackLayout
            {
                Padding = new Thickness(0, 0, 15, 0),
                Orientation = StackOrientation.Horizontal,
                Children = { titleLabel, titleOptionButton }
            };

            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    titleLabel.FontSize = 26;
                    titleOptionButtonFontImageSource.Size = 32;
                    titleStackLayout.Padding = new Thickness(0, 0, 15, 0);
                    break;
                case Device.iOS:
                    titleLabel.FontSize = 24;
                    titleOptionButtonFontImageSource.Size = 30;
                    titleStackLayout.Padding = new Thickness(15, 0, 15, 0);
                    break;
            };

            NavigationPage.SetTitleView(this, titleStackLayout);

            _collectionView = new CollectionView { Margin = 0 };
            _collectionView.SetBinding(CollectionView.ItemsSourceProperty, "MccheyneCheckList");

            _collectionView.ItemTemplate = new DataTemplate(() =>
                {
                    var collectionViewDataTemplateGrid = new Grid
                    {
                        Padding = 10,
                        RowDefinitions =
                        {
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        }
                    };

                    var checkDateLabel = new Label
                    {
                        Padding = new Thickness(5, 0, 0, 0),
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 25,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    };
                    checkDateLabel.SetBinding(Label.TextProperty, "Date");
                    checkDateLabel.SetBinding(Label.TextColorProperty, "Ranges[0].DateColor");
                    Grid.SetRow(checkDateLabel, 0);
                    collectionViewDataTemplateGrid.Children.Add(checkDateLabel);

                    var checkButtonGrid = new Grid
                    {
                        RowDefinitions =
                        {
                            new RowDefinition { Height = new GridLength(50) },
                        },
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        }
                    };

                    for (int i = 0; i < 5; i++)
                    {
                        var checkButton = new Button
                        {
                            Margin = 0,
                            Padding = 0,
                            BorderColor = Color.Black,
                            BorderWidth = 1,
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 14,
                            TextColor = Color.Black
                        };

                        checkButton.SetBinding(Button.BackgroundColorProperty, $"Ranges[{i}].Color");
                        checkButton.SetBinding(Button.TextProperty, $"Ranges[{i}].RangeText");
                        checkButton.SetBinding(Button.CommandProperty, new Binding() { Source = BindingContext as MccheyneCheckViewModel, Path = "command" });
                        checkButton.SetBinding(Button.CommandParameterProperty, $"Ranges[{i}].Id");

                        if (i == 4)
                        {
                            checkButton.SetBinding(Button.IsVisibleProperty, "Range5IsNull");
                        }

                        Grid.SetColumn(checkButton, i);
                        checkButtonGrid.Children.Add(checkButton);
                    }

                    Grid.SetRow(checkButtonGrid, 1);
                    collectionViewDataTemplateGrid.Children.Add(checkButtonGrid);

                    return collectionViewDataTemplateGrid;
                });
            Content = _collectionView;
        }

        public void ScrollToToday()
        {
            _collectionView.ScrollTo(_todayMccheyne, null, ScrollToPosition.Center, false);
        }

        private void OnScrollToToday(object sender, EventArgs e)
        {
            ScrollToToday();
        }

        private async void OnCheckButtonClicked(object sender, EventArgs e)
        {
            if (!(BindingContext is MccheyneCheckViewModel viewModel)) { return; }

            bool IsConfirmed = await DisplayAlert("오늘까지 체크", "정말 체크 하시겠습니까?", "확인", "취소");
            var today = DateTime.Now;
            if (IsConfirmed)
            {
                await PopupNavigation.Instance.PopAsync();

                viewModel.MccheyneCheckList.ForEach(x =>
                {
                    var month = int.Parse(x.Date.Substring(0, x.Date.IndexOf("-")));
                    var day = int.Parse(x.Date.Substring(x.Date.IndexOf("-") + 1));

                    if (month < today.Month || (month == today.Month && day <= today.Day))
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            x.Ranges[i].IsChecked = true;
                            Preferences.Set(x.Ranges[i].Id, x.Ranges[i].IsChecked);
                        }
                        x.Ranges[0].Color = x.Ranges[0].IsChecked == true ? Color.SkyBlue : Color.White;
                        x.Ranges[1].Color = x.Ranges[1].IsChecked == true ? Color.LightPink : Color.White;
                        x.Ranges[2].Color = x.Ranges[2].IsChecked == true ? Color.LightGreen : Color.White;
                        x.Ranges[3].Color = x.Ranges[3].IsChecked == true ? Color.Yellow : Color.White;
                        x.Ranges[4].Color = x.Ranges[4].IsChecked == true ? Color.MediumPurple : Color.White;
                    }
                });
                ScrollToToday();
            }
        }
        private async void OnClearButtonClicked(object sender, EventArgs e)
        {
            if (!(BindingContext is MccheyneCheckViewModel viewModel)) { return; }

            bool IsConfirmed = await DisplayAlert("초기화", "정말 초기화 하시겠습니까?", "확인", "취소");
            if (IsConfirmed)
            {
                await PopupNavigation.Instance.PopAsync();

                viewModel.MccheyneCheckList.ForEach(x =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        x.Ranges[i].IsChecked = false;
                        x.Ranges[i].Color = Color.White;
                        Preferences.Set(x.Ranges[i].Id, x.Ranges[i].IsChecked);
                    }
                });
            }

        }

        private async void OnOptionClicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(_optionPopup);
        }
    }
}