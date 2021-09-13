using System.Collections.ObjectModel;
using TodaysManna.Models;
using Xamarin.Forms;
using TodaysManna.Managers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms.Internals;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;

namespace TodaysManna.ViewModelV2
{
    public class MccheyneCheckListPageViewModel : ObservableObject
    {
        public ICommand CheckCommand => new Command<CheckViewModel>(OnCheckButtonTapped);

        private ObservableCollection<MccheyneCheckListContentViewModel> _mccheyneCheckList = new ObservableCollection<MccheyneCheckListContentViewModel>();
        public ObservableCollection<MccheyneCheckListContentViewModel> MccheyneCheckList { get => _mccheyneCheckList; set => SetProperty(ref _mccheyneCheckList, value); }

        List<MccheyneCheckListLocalContent> dbList;

        public MccheyneCheckListPageViewModel()
        {
            SetList();
        }

        private async void SetList()
        {
            dbList = await DatabaseManager.CheckListDatabase.GetItemsAsync();

            var dbListGroupByDate = dbList.ToList().GroupBy((arg) => arg.Date);

            dbListGroupByDate.ForEach(ranges =>
            {
                var checkViewModelList = new List<CheckViewModel>();
                ranges.ForEach(x =>
                {
                    checkViewModelList.Add(new CheckViewModel
                    {
                        Date = x.Date,
                        ID = x.ID,
                        IsChecked = x.IsChecked
                    });
                });
                var viewModel = new MccheyneCheckListContentViewModel
                {
                    Date = ranges.Key,
                };
                viewModel.Ranges.AddRange(checkViewModelList);

                MccheyneCheckList.Add(viewModel);
            });
        }

        private async void OnCheckButtonTapped(CheckViewModel val)
        {
            DependencyService.Get<IHapticFeedback>().Run();

            val.IsChecked = !val.IsChecked;

            var mccheyneCheckListLocalContent = new MccheyneCheckListLocalContent
            {
                Date = val.Date,
                ID = val.ID,
                IsChecked = val.IsChecked
            };

            await DatabaseManager.CheckListDatabase.SaveItemAsync(mccheyneCheckListLocalContent);
        }
    }
}
