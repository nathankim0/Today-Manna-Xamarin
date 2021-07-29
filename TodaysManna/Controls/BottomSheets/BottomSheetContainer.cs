using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using TodaysManna.Constants;

namespace TodaysManna
    {
    /// <summary>
    /// 상속하여 BottomSheet을 구현합니다.
    /// </summary>
    public class BottomSheet : Grid
    {
        public EventHandler<double> RatioChanged;
        public EventHandler Hided;

        protected readonly BoxView _bottomSheetBackground;
        private readonly SheetContainer _bottomSheetContainer;

        public double HeightRatio = 0.7;
        protected double BottomSheetMaxY;

        protected BottomSheet()
        {
            IsVisible = false;

            _bottomSheetContainer = new SheetContainer
            {
                VerticalOptions = LayoutOptions.Fill
            };

            BottomSheetMaxY = Values.height * HeightRatio;

            _bottomSheetContainer.BottomSheetPulledDown += OnBottomSheetDisappearred;
            _bottomSheetContainer.MaxY = BottomSheetMaxY;
            _bottomSheetContainer.SheetFrame.TranslationY = BottomSheetMaxY;

            var backgroundGesture = new TapGestureRecognizer();
            backgroundGesture.Tapped += OnBottomSheetDisappearred;

            _bottomSheetBackground = new BoxView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Opacity = 0,
                BackgroundColor = Color.FromRgba(0, 0, 0, 0.5),
                GestureRecognizers = { backgroundGesture }
            };
            //_bottomSheetBackground.SetAppThemeColor(BackgroundColorProperty, Color.FromRgba(0, 0, 0, 0.5), Color.FromRgba(255, 255, 255, 0.5));

            Children.Add(_bottomSheetBackground);
            Children.Add(_bottomSheetContainer);
        }

        public void SetHeightRatio(double ratio)
        {
            if (ratio > 1)
            {
                ratio = 1;
            }
            else if (ratio < 0.2)
            {
                ratio = 0.2;
            }
            HeightRatio = ratio;
            BottomSheetMaxY = Values.height * HeightRatio;
            _bottomSheetContainer.MaxY = BottomSheetMaxY;
            _bottomSheetContainer.SheetFrame.TranslationY = BottomSheetMaxY;
            _bottomSheetContainer.Layout(new Rectangle(0, Height * (1 - HeightRatio), Width, Height * HeightRatio));

            RatioChanged?.Invoke(this, ratio);
        }

        private void OnBottomSheetDisappearred(object sender, EventArgs e)
        {
            if (IsVisible) { Hide(); }
        }

        /// <summary>
        /// Picker를 화면에 띄웁니다. (상단으로 올라오는 애니메이션)
        /// </summary>
        public virtual void Show()
        {
            if (IsVisible) { return; }
            IsVisible = true;
            RaiseChild(this);

            ShowAnimation();
        }

        /// <summary>
        /// Picker를 화면에서 숨깁니다. (하단으로 내려가는 애니메이션)
        /// </summary>
        public virtual void Hide()
        {
            HideAnimation();
        }

        protected void ShowAnimation()
        {
            new Animation
            {
                {
                    0, 0.5, new Animation(v => _bottomSheetBackground.Opacity = v, 0, 1)
                },
                {
                    0, 1, new Animation(v => _bottomSheetContainer.SheetFrame.TranslationY = v,
                        BottomSheetMaxY,
                        _bottomSheetContainer.CurrentY = 0,
                        Easing.CubicOut)
                }
            }.Commit(this, "VisiblePicker", 10, 400);
        }

        protected void HideAnimation()
        {
            new Animation
            {
                {
                    0, 0.5, new Animation(v => _bottomSheetBackground.Opacity = v, 1, 0)
                },
                {
                    0, 1, new Animation(v => _bottomSheetContainer.SheetFrame.TranslationY = v,
                        _bottomSheetContainer.CurrentY,
                        _bottomSheetContainer.CurrentY = BottomSheetMaxY,
                        Easing.CubicOut)
                }
            }.Commit(this, "InVisiblePicker", 10, 400, null, (v, c) => { IsVisible = false; Hided?.Invoke(this, EventArgs.Empty); }, () => false);
        }

        public void AddSheetContent(View layout)
        {
            _bottomSheetContainer.ContentLayout.Children.Add(layout);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            _bottomSheetBackground.Layout(new Rectangle(x, y - Values.StatusBarHeight, width, height));
            _bottomSheetContainer.Layout(new Rectangle(x, y + height * (1 - HeightRatio), width, height * HeightRatio));
        }

        private class SheetContainer : ContentView
        {
            public event EventHandler BottomSheetPulledDown;

            public readonly PancakeView SheetFrame;
            public readonly StackLayout ContentLayout;

            private double _maxY;
            public double MaxY
            {
                get => _maxY;
                set
                {
                    _maxY = value;
                }
            }

            private double _currentY;
            public double CurrentY
            {
                get => _currentY;
                set
                {
                    _currentY = value;
                }
            }

            private bool _up, _down;
            private bool _fullDown;

            public SheetContainer()
            {
                var panGesture = new PanGestureRecognizer();
                panGesture.PanUpdated += OnPanUpdated;
                GestureRecognizers.Add(panGesture);

                ContentLayout = new StackLayout
                {
                    Margin = 0,
                    Spacing = 0
                };

                SheetFrame = new PancakeView
                {
                    BackgroundColor = Color.White,
                    CornerRadius = new CornerRadius(25, 25, 0, 0),
                    IsClippedToBounds = true,
                    Margin = 0,
                    Padding = 0,
                    Content = ContentLayout
                };
                //SheetFrame.SetAppThemeColor(BackgroundColorProperty, Color.White, Color.Black);

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

                        _fullDown = e.TotalY >= _maxY / 3;

                        break;

                    case GestureStatus.Completed:

                        _currentY = SheetFrame.TranslationY;


                        if (_up)
                        {
                            SheetFrame.TranslateTo(SheetFrame.X, _currentY = 0, 250, Easing.CubicOut);
                        }

                        else if (_down)
                        {
                            if (_fullDown)
                            {
                                BottomSheetPulledDown?.Invoke(this, EventArgs.Empty);
                            }
                            else // 완전히 내리지 않았을 경우 -> 올라감
                            {
                                SheetFrame.TranslateTo(SheetFrame.X, CurrentY = 0, 250, Easing.CubicOut);
                            }
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
}
