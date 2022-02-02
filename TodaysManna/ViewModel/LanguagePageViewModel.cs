using System.Collections.ObjectModel;

namespace TodaysManna.ViewModel
{
    public class Country : BaseViewModel
    {
        public Language LanguageValue { get; set; }
        public string Language { get; set; }
        public string Flag { get; set; }

        private bool selected;
        public bool Selected { get => selected; set => SetProperty(ref selected, value); }
    }

    public class LanguagePageViewModel : BaseViewModel
    {
        public ObservableCollection<Country> Countries { get; set; }

        private bool isSelected;
        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }

        public LanguagePageViewModel()
        {
            Countries = new ObservableCollection<Country>
            {
                new Country
                {
                    LanguageValue = Language.Korean,
                    Language = "한국어",
                    Flag = "🇰🇷"
                },
                new Country
                {
                    LanguageValue = Language.English,
                    Language = "English",
                    Flag = "🇺🇸"
                },
                new Country
                {
                    LanguageValue = Language.Spanish,
                    Language = "Español",
                    Flag = "🇪🇸"
                },
                new Country
                {
                    LanguageValue = Language.Chinese,
                    Language = "漢語",
                    Flag = "🇨🇳"
                },
                new Country
                {
                    LanguageValue = Language.Japanese,
                    Language = "日本語",
                    Flag = "🇯🇵"
                },
                new Country
                {
                    LanguageValue = Language.German,
                    Language = "Deutsch",
                    Flag = "🇩🇪"
                },
                new Country
                {
                    LanguageValue = Language.French,
                    Language = "français",
                    Flag = "🇫🇷"
                },
                new Country
                {
                    LanguageValue = Language.Hindi,
                    Language = "हिन्दी",
                    Flag = "🇮🇳"
                }
            };
        }
    }
}
