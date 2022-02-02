using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using TodaysManna.ViewModel;

namespace TodaysManna.Views
{
    public partial class OnboardingPage : ContentPage
    {
        public EventHandler<Language> LanguageChanged;
        Language selectedLanguage = Language.Korean;

        public OnboardingPage()
        {
            InitializeComponent();
            BindingContext = new LanguagePageViewModel();

            if (!Constants.IsDeviceIOS)
            {
                Padding = new Thickness(0, Constants.StatusBarHeight, 0, 10);
            }

            SetSelectedLanguage();
        }

        public void SetSelectedLanguage()
        {
            var currentLanguage = AppManager.GetCurrentLanguageEnumValue();
            if (!(BindingContext is LanguagePageViewModel viewModel)) return;
            if (viewModel.Countries == null) return;
            foreach (var node in viewModel.Countries)
            {
                if (node.LanguageValue.Equals(currentLanguage))
                {
                    node.Selected = true;
                }
            }
            viewModel.IsSelected = true;
            button.IsEnabled = true;
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            if (!(BindingContext is LanguagePageViewModel viewModel)) return;
            if (viewModel.Countries == null) return;

            viewModel.IsSelected = true;
            button.IsEnabled = true;

            foreach (var node in viewModel.Countries)
            {
                node.Selected = false;
            }

            var country = ((TappedEventArgs)e).Parameter as Country;
            country.Selected = true;

            selectedLanguage = country.LanguageValue;
        }

        void Button_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("CurrentLanguage", selectedLanguage.ToString());

            LanguageChanged?.Invoke(this, selectedLanguage);
            Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
