using System;
using System.Collections.ObjectModel;
using TodaysManna.Models;
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
                var range5IsNull = true;
                if (range.Range5 == "")
                {
                    range5IsNull = false;
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

                checkList.Add(new MccheyneCheckListContent
                {
                    Date = range.Date,
                    Ranges = new MccheyneOneRange[]
                    {
                        new MccheyneOneRange
                        {
                            Id=itemId1,
                            DateColor=dateColor,
                            //IsChecked= isChecked1,
                            //Color= isChecked1 == true ? Colors.MccheyneColor1 : Color.White,
                            RangeText =range.Range1,
                            ItemIsVisible = true
                        },
                        new MccheyneOneRange
                        {
                            Id=itemId2,
                            DateColor=dateColor,
                            //IsChecked= isChecked2,
                            //Color=isChecked2 == true ? Colors.MccheyneColor2 : Color.White,
                            RangeText=range.Range2,
                            ItemIsVisible = true
                        },
                        new MccheyneOneRange
                        {
                            Id=itemId3,
                            DateColor=dateColor,
                            //IsChecked= isChecked3,
                            //Color=isChecked3 == true ? Colors.MccheyneColor3 : Color.White,
                            RangeText=range.Range3,
                            ItemIsVisible = true
                        },
                        new MccheyneOneRange
                        {
                            Id=itemId4,
                            DateColor=dateColor,
                            //IsChecked= isChecked4,
                            //Color=isChecked4 == true ? Colors.MccheyneColor4 : Color.White,
                            RangeText=range.Range4,
                            ItemIsVisible = true
                        },
                        new MccheyneOneRange
                        {
                            Id=itemId5,
                            DateColor=dateColor,
                            //IsChecked= isChecked5,
                            //Color=isChecked5 == true ? Colors.MccheyneColor5 : Color.White,
                            RangeText=range.Range5,
                            ItemIsVisible = range5IsNull
                        }
                    },
                    Range5IsNull = range5IsNull
                });
                ++id;
            }

            MccheyneCheckList = checkList;
        }
    }
}
