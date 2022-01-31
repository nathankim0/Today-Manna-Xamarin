using System;
using TodaysManna.Managers;
using TodaysManna.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;

namespace TodaysManna.ViewModel
{
    public class MannaViewModel : PageBaseViewModel
    {
        private string _mannaRange;
        public string MannaRange { get => _mannaRange; set => SetProperty(ref _mannaRange, value); }

        private string _mccheyneRange;
        public string MccheyneRange { get => _mccheyneRange; set => SetProperty(ref _mccheyneRange, value); }

        private ObservableRangeCollection<MannaContent> mannaContents = new ObservableRangeCollection<MannaContent>();
        public ObservableRangeCollection<MannaContent> MannaContents { get => mannaContents; set => SetProperty(ref mannaContents, value); }

        private Color customBackgroundDimColor = Color.FromHex(Preferences.Get("CustomBackgroundDimColor", Constants.DEFAULT_BACKGROUND_DIM_COLOR));
        public Color CustomBackgroundDimColor { get => customBackgroundDimColor; set => SetProperty(ref customBackgroundDimColor, value); }

        private Color customTextColor = Color.FromHex(Preferences.Get("CustomTextColor", Constants.DEFAULT_TEXT_COLOR));
        public Color CustomTextColor { get => customTextColor; set => SetProperty(ref customTextColor, value); }

        private double customFontSize = double.TryParse(Preferences.Get("TextSize", "17"), out var font) ? font : 17;
        public double CustomFontSize { get => customFontSize; set => SetProperty(ref customFontSize, value); }

        public bool IsAndroid { get => !Constants.IsDeviceIOS; }

        private bool isAllSelected = false;
        public bool IsAllSelected { get => isAllSelected; set => SetProperty(ref isAllSelected, value); }

        private ImageSource imageSource = ImageSource.FromFile("image1.jpg");
        public ImageSource CurrentImageSource { get => imageSource; set => SetProperty(ref imageSource, value); }

        private bool isLoadingServer;
        public bool IsLoadingServer { get => isLoadingServer; set => SetProperty(ref isLoadingServer, value); }

        private string _displayDateRange;
        public string DisplayDateRange { get => _displayDateRange; set => SetProperty(ref _displayDateRange, value); }

        private ObservableRangeCollection<MccheyneOneRange> todayMccheyneCheckList = new ObservableRangeCollection<MccheyneOneRange>();
        public ObservableRangeCollection<MccheyneOneRange> TodayMccheyneCheckList { get => todayMccheyneCheckList; set => SetProperty(ref todayMccheyneCheckList, value); }

        public MannaViewModel()
        {
            var today = DateTime.Now.ToString("M-d");
            DisplayDateRange = DateTime.Now.ToString("yyyy.MM.dd.dddd");

            var rangeOfDate = MccheyneDataManager.MccheyneRangeList.Find(x => x.Date.Equals(today));
            MccheyneRange = $"{rangeOfDate.Range1} {rangeOfDate.Range2} {rangeOfDate.Range3} {rangeOfDate.Range4} {rangeOfDate.Range5}";

            SetTodayCheckList();
        }

        public void SetTodayCheckList()
        {
            var checkList = new List<MccheyneOneRange>();
            foreach (var node in MccheyneCheckListManager.MccheyneCheckList)
            {
                if (node.Date == DateTime.Now.ToString("M-d"))
                {
                    foreach (var checkContent in node.Ranges)
                    {
                        checkList.Add(checkContent);
                    }
                    break;
                }
            }
            TodayMccheyneCheckList = new ObservableRangeCollection<MccheyneOneRange>(checkList);
        }
    }
}
