using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace TodaysManna.Models
{
    public class MccheyneCheckContent : INotifyPropertyChanged
    {
        public string Date { get; set; }
        public MccheyneOneRange[] Ranges { get; set; } = new MccheyneOneRange[5];
        public bool Range5IsNull { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class MccheyneOneRange : INotifyPropertyChanged
    {
        public string RangeText { get; set; }

        private bool _ischecked;
        public bool IsChecked
        {
            get => _ischecked;
            set
            {
                if (_ischecked != value)
                {
                    _ischecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        private Color _color;
        public Color Color {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
