using System;
using System.Collections.ObjectModel;
using TodaysManna.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna.Managers
{
    public static class MccheyneCheckListManager
    {
        public static ObservableCollection<MccheyneCheckListContent> MccheyneCheckList = new ObservableCollection<MccheyneCheckListContent>();

        public static void InitCheckList()
        {
            var checkList = new ObservableCollection<MccheyneCheckListContent>();

            var id = 0;
            foreach (var range in MccheyneDataManager.MccheyneRangeList)
            {
                var dateColor = range.Date == DateTime.Now.ToString("M-d") ? Colors.PrimaryColor : Color.Gray;
                var isRange5Exist = true;
                if (string.IsNullOrWhiteSpace(range.Range5))
                {
                    isRange5Exist = false;
                }

                var itemId1 = id.ToString();
                var itemId2 = (++id).ToString();
                var itemId3 = (++id).ToString();
                var itemId4 = (++id).ToString();
                var itemId5 = (++id).ToString();

                var isChecked1 = Preferences.Get(itemId1, false);
                var isChecked2 = Preferences.Get(itemId2, false);
                var isChecked3 = Preferences.Get(itemId3, false);
                var isChecked4 = Preferences.Get(itemId4, false);
                var isChecked5 = Preferences.Get(itemId5, false);

                var mccheyneCheckContent = new MccheyneCheckListContent
                {
                    Date = range.Date,
                    DateColor = dateColor,
                    IsRange5Exist = isRange5Exist,
                    Ranges = new ObservableRangeCollection<MccheyneOneRange>
                    {
                        new MccheyneOneRange
                        {
                            Id=itemId1,
                            RangeText =range.Range1,
                            ItemIsVisible = true
                        },
                        new MccheyneOneRange
                        {
                            Id=itemId2,
                            RangeText=range.Range2,
                            ItemIsVisible = true
                        },
                        new MccheyneOneRange
                        {
                            Id=itemId3,
                            RangeText=range.Range3,
                            ItemIsVisible = true
                        },
                        new MccheyneOneRange
                        {
                            Id=itemId4,
                            RangeText=range.Range4,
                            ItemIsVisible = true
                        },
                        new MccheyneOneRange
                        {
                            Id=itemId5,
                            RangeText=range.Range5,
                            ItemIsVisible = isRange5Exist
                        }
                    }
                };
                checkList.Add(mccheyneCheckContent);
                ++id;
            }

            MccheyneCheckList = checkList;
        }
    }
}
