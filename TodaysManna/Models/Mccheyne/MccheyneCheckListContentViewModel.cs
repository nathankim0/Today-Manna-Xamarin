using System;
using TodaysManna.ViewModel;
using Xamarin.Forms;

namespace TodaysManna.Models
{
    public class MccheyneCheckListContent : BaseViewModel
    {
        private string _toDisplayDate;
        public string ToDisplayDate
        {
            get => _toDisplayDate;
            set => SetProperty(ref _toDisplayDate, value);
        }

        private string _date;
        public string Date
        {
            get =>_date;
            set
            {
                string[] monthAndDay = value.Split('-');
                var dateTime = new DateTime(DateTime.Now.Year, int.Parse(monthAndDay[0]), int.Parse(monthAndDay[1]));
                ToDisplayDate = dateTime.ToString("MM월 dd일 (ddd)");
                SetProperty(ref _date, value);
            }
        }
        public MccheyneOneRange[] Ranges { get; set; } = new MccheyneOneRange[5];
        public bool Range5IsNull { get; set; }
    }

    public class MccheyneOneRange : BaseViewModel
    {
        public string Id { get; set; }
        public string RangeText { get; set; }

        private bool _ischecked;
        public bool IsChecked 
        {
            get => _ischecked;
            set => SetProperty(ref _ischecked, value);
        }

        private Color _color = Color.White;
        public Color Color {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        private Color _dateColor = Color.Black;
        public Color DateColor 
        {
            get => _dateColor;
            set => SetProperty(ref _dateColor, value);
        }
    }

    #region renew ui
    public class MccheyneCheckListLocalContent
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Date { get; set; }
        public string RangeText { get; set; }
        public bool IsChecked { get; set; }
    }

    public class MccheyneCheckListContentViewModel : BaseViewModel
    {
        private string _date;
        public string Date { get => _date; set => SetProperty(ref _date, value); }

        ObservableRangeCollection<CheckViewModel> _ranges = new ObservableRangeCollection<CheckViewModel>();
        public ObservableRangeCollection<CheckViewModel> Ranges
        {
            get => _ranges;
            set
            {
                SetProperty(ref _ranges, value);
                OnPropertyChanged(nameof(RangesArray));
            }
        }

        public CheckViewModel[] RangesArray => Ranges.ToArray();
    }

    public class CheckViewModel : BaseViewModel
    {
        private int _id;
        public int ID { get => _id; set => SetProperty(ref _id, value); }

        private string _date;
        public string Date { get => _date; set => SetProperty(ref _date, value); }

        private string _rangeText;
        public string RangeText { get => _rangeText; set => SetProperty(ref _rangeText, value); }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                SetProperty(ref _isChecked, value);
                OnPropertyChanged(nameof(CheckedColor));
            }
        }

        public Color CheckedColor => IsChecked ? Color.Orange : Color.White;

    }
    #endregion renew ui
}
