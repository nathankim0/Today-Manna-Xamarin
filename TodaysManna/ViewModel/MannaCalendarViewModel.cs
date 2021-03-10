using System;
using Syncfusion.SfCalendar.XForms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace TodaysManna.ViewModel
{
    public class MannaCalendarViewModel : INotifyPropertyChanged
    {
        private CalendarEventCollection _calendarInlineEvents = new CalendarEventCollection();
        public CalendarEventCollection CalendarInlineEvents
        {
            get => _calendarInlineEvents;
            set
            {
                if (_calendarInlineEvents != value)
                {
                    _calendarInlineEvents = value;
                }
            }
        }

        public MannaCalendarViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
