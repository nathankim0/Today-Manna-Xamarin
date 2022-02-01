using System.Collections.ObjectModel;
using TodaysManna.Models;
using TodaysManna.Managers;

namespace TodaysManna.ViewModel
{
    public class MccheyneCheckViewModel : PageBaseViewModel
    {
        public ObservableCollection<MccheyneCheckListContent> MccheyneCheckList
        {
            get => MccheyneCheckListManager.MccheyneCheckList;
            set => SetProperty(ref MccheyneCheckListManager.MccheyneCheckList, value);
        }
    }
}
