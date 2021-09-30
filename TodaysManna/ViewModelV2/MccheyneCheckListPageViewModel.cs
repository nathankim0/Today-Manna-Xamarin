using System.Collections.ObjectModel;
using TodaysManna.Models;
using Xamarin.Forms;
using TodaysManna.Managers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms.Internals;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System;

namespace TodaysManna.ViewModelV2
{
    public class MccheyneCheckListPageViewModel : ObservableObject
    {
        //public ICommand CheckCommand => new Command<CheckViewModel>(OnCheckButtonTapped);

        private ObservableCollection<MccheyneCheckListContentViewModel> _mccheyneCheckList = new ObservableCollection<MccheyneCheckListContentViewModel>();
        public ObservableCollection<MccheyneCheckListContentViewModel> MccheyneCheckList { get => _mccheyneCheckList; set => SetProperty(ref _mccheyneCheckList, value); }

        List<MccheyneCheckListLocalContent> dbList;

        public MccheyneCheckListPageViewModel()
        {
            InitList();
            SetList();
        }

        private void InitList()
        {
            foreach (var range in MccheyneDataManager.MccheyneRangeList)
            {
                var viewModel = new MccheyneCheckListContentViewModel();
                viewModel.Date = range.Date;
                viewModel.Ranges.Add(new CheckViewModel { Date = range.Date, IsChecked = false, RangeText = range.Range1 });
                viewModel.Ranges.Add(new CheckViewModel { Date = range.Date, IsChecked = false, RangeText = range.Range2 });
                viewModel.Ranges.Add(new CheckViewModel { Date = range.Date, IsChecked = false, RangeText = range.Range3 });
                viewModel.Ranges.Add(new CheckViewModel { Date = range.Date, IsChecked = false, RangeText = range.Range4 });
                viewModel.Ranges.Add(new CheckViewModel { Date = range.Date, IsChecked = false, RangeText = range.Range5 });

                MccheyneCheckList.Add(viewModel);
            }
        }

        private async void SetList()
        {
            dbList = await DatabaseManager.CheckListDatabase.GetItemsAsync();

            var dbDic = dbList.ToDictionary(db=>db.Date);
            MccheyneCheckList.ForEach(mccheyneCheckContent =>
            {
                var viewModel = dbDic.TryGetValue(mccheyneCheckContent.Date, out var value) ? value : new MccheyneCheckListLocalContent();
                mccheyneCheckContent.Ranges.ForEach(range =>
                {
                    range.IsChecked = range.RangeText == viewModel.RangeText ? viewModel.IsChecked : false;
                });
            });
        }
    }
}
