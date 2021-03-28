using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace TodaysManna
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
        public string Id { get; set; }
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

        private Color _color = Color.White;
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

        private Color _dateColor = Color.Black;
        public Color DateColor 
        {
            get => _dateColor;
            set
            {
                if (_dateColor != value)
                {
                    _dateColor = value;
                    OnPropertyChanged(nameof(DateColor));
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
