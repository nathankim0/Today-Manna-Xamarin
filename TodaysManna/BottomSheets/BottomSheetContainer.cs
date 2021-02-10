using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna
{
    public class BottomSheet : Grid
    {
        public EventHandler hided;
        public readonly BottomSheetContainer BottomSheetContainer;
        protected readonly BoxView BackgroundBox;
        double height = 0;
        public BottomSheet()
        {
            height = DeviceDisplay.MainDisplayInfo.Height/ DeviceDisplay.MainDisplayInfo.Density;
            IsVisible = false;

            BottomSheetContainer = new BottomSheetContainer();
            BottomSheetContainer.BottomSheetPulledDown += OnBottomSheetDisappear;
            BottomSheetContainer.MaxY = height;
            var backgroundGesture = new TapGestureRecognizer();
            backgroundGesture.Tapped += OnBottomSheetDisappear;

            BackgroundBox = new BoxView
            {
                Opacity = 0,
                BackgroundColor = Color.FromRgba(0, 0, 0, 0.5),
                GestureRecognizers = { backgroundGesture }
            };
            
            Children.Add(BackgroundBox, 0,0);
            Children.Add(BottomSheetContainer,0,0);
        }

        private void OnBottomSheetDisappear(object sender, EventArgs e)
        {
            if (IsVisible)
            {
                Hide();
            }
        }

        public virtual void Show()
        {
            if (IsVisible)
            {
                return;
            }

            IsVisible = true;

            new Animation
            {
                {
                    0, 0.5, new Animation(v => BackgroundBox.Opacity = v)
                },
                {
                    0, 1, new Animation(v => BottomSheetContainer.SheetFrame.TranslationY = v,
                        height,
                        BottomSheetContainer.CurrentY = height/2,
                        Easing.CubicOut)
                }
            }.Commit(this, "VisiblePicker", 10, 400);
        }

        protected async void Hide()
        {
            var backgroundFadeTask = BackgroundBox.FadeTo(0);

            var startingTranslateTask =
                BottomSheetContainer.SheetFrame.TranslateTo(BottomSheetContainer.SheetFrame.X,
                    BottomSheetContainer.CurrentY, 0);

            var destinationTranslateTask = BottomSheetContainer.SheetFrame.TranslateTo(
                BottomSheetContainer.SheetFrame.X,
                BottomSheetContainer.CurrentY = height,
                400, Easing.CubicOut);

            await Task.WhenAll(new Task[] { backgroundFadeTask, startingTranslateTask, destinationTranslateTask });

            IsVisible = false;

            hided?.Invoke(this, EventArgs.Empty);
        }
    }

    public class BottomSheetFrame : Frame
    {
        private new static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(BottomSheetFrame), typeof(CornerRadius), typeof(BottomSheetFrame));
        public BottomSheetFrame()
        {
            // MK Clearing default values (e.g. on iOS it's 5)
            base.CornerRadius = 0;
        }

        public new CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }

    public class BottomSheetContainer : ContentView
    {
        public event EventHandler BottomSheetPulledDown;

        public readonly BottomSheetFrame SheetFrame;
        public readonly StackLayout ContentStackLayout;

        public double MaxY;
        public double CurrentY;

        private bool _up, _down;
        private bool _fullDown;

        public BottomSheetContainer()
        {
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);

            //MaxY = DeviceDisplay.MainDisplayInfo.Height;

            ContentStackLayout = new StackLayout
            {
                Margin = 0,
                Spacing = 0
            };

            SheetFrame = new BottomSheetFrame
            {
                CornerRadius = new CornerRadius(10,10,0,0),
                IsClippedToBounds = true,
                HasShadow = false,
                Padding = 0,
                Content = ContentStackLayout
            };

            Content = SheetFrame;
        }


        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:

                    SheetFrame.TranslationY = Math.Max(CurrentY + e.TotalY, 0);

                    if (e.TotalY < 0)
                    {
                        _down = false;
                        _up = true;
                    }
                    else
                    {
                        _up = false;
                        _down = true;
                    }

                    _fullDown = e.TotalY >= MaxY / 2;

                    break;

                case GestureStatus.Completed:

                    CurrentY = SheetFrame.TranslationY;


                    if (_up)
                    {
                        SheetFrame.TranslateTo(SheetFrame.X, CurrentY = 0, 250, Easing.CubicOut);
                    }

                    else if (_down)
                    {
                        BottomSheetPulledDown?.Invoke(this, EventArgs.Empty);
                        //if (_fullDown)
                        //{
                        //    BottomSheetPulledDown?.Invoke(this, EventArgs.Empty);
                        //}
                        //else // 완전히 내리지 않았을 경우 -> 올라감
                        //{
                        //    SheetFrame.TranslateTo(SheetFrame.X, CurrentY = 0, 250, Easing.CubicOut);
                        //}
                    }

                    break;
                case GestureStatus.Started:
                    break;
                case GestureStatus.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
