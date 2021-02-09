using System;
using Xamarin.Forms;

namespace TodaysManna.Views
{
    public class MannaTextClickSheet : ContentView
    {
        public EventHandler coppybuttonClicked;
        public EventHandler sharebuttonClicked;
        public EventHandler savebuttonClicked;

        private Grid contentGrid;

        public Label textLabel;
        public Editor memoEditor;

        //private StackLayout contentStackLayout;
        public MannaTextClickSheet()
        {
            textLabel = new Label();
            memoEditor = new Editor { BackgroundColor=Color.LightGray};

            var coppyButton = new Button
            {
                Text = "복사"
            };
            coppyButton.Clicked += OnCoppyButtonClicked;

            var shareButton = new Button
            {
                Text = "공유"
            };
            shareButton.Clicked += OnShareButtonClicked;
            var saveButton = new Button
            {
                Text = "저장"
            };
            saveButton.Clicked += OnSaveButtonClicked;
            contentGrid = new Grid
            {
                Padding=15,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(4, GridUnitType.Star) },
                    //new RowDefinition(),
                    //new RowDefinition { Height = new GridLength(100) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) }
                }
            };
            contentGrid.Children.Add(textLabel, 0, 0);
            Grid.SetColumnSpan(textLabel, 3);

            contentGrid.Children.Add(coppyButton, 0, 1);
            contentGrid.Children.Add(shareButton, 1, 1);
            contentGrid.Children.Add(saveButton, 2, 1);

            contentGrid.Children.Add(memoEditor, 0, 2);
            Grid.SetColumnSpan(memoEditor, 3);
            //Grid.SetRowSpan(memoEditor, 3);
            //contentStackLayout = new StackLayout
            //{
            //    Orientation = StackOrientation.Horizontal,
            //    Children =
            //    {
            //        coppyButton,
            //        shareButton,
            //        saveButton
            //    }
            //};

            Content = contentGrid;
        }

        private void OnCoppyButtonClicked(object sender, EventArgs e)
        {
            coppybuttonClicked?.Invoke(this, EventArgs.Empty);
        }
        private void OnShareButtonClicked(object sender, EventArgs e)
        {
            sharebuttonClicked?.Invoke(this, EventArgs.Empty);
        }
        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            savebuttonClicked?.Invoke(this, EventArgs.Empty);
        }

        

        
    }
}
