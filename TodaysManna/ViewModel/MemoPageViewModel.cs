using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace TodaysManna.ViewModel
{
    public class MemoPageViewModel : INotifyPropertyChanged
    {
        public EventHandler<MemoItem> deleted;
        public EventHandler<MemoItem> shared;

        public ICommand DeleteCommand => new Command<MemoItem>(RemoveItem);
        public ICommand ShareCommand => new Command<MemoItem>(ShareItem);

        private void RemoveItem(MemoItem memoItem)
        {
            deleted?.Invoke(this, memoItem);
        }
        private void ShareItem(MemoItem memoItem)
        {
            shared?.Invoke(this, memoItem);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
