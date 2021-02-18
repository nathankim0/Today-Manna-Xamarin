using System;
using System.Collections.Generic;
using TodaysManna.ViewModel;
using Xamarin.Forms;

namespace TodaysManna.Views
{
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();

            var navMannaPage = new NavigationPage(new MannaPage()) { Title="만나", IconImageSource= "tab_manna" };
            var navMccheynePage = new NavigationPage(new MccheynePage()) { Title = "맥체인", IconImageSource = "tab_mc" };
            var navMccheyneCheckListPage = new NavigationPage(App.mccheyneCheckListPage) { Title = "체크리스트", IconImageSource = "tab_mc" };
            var navMyPage = new NavigationPage(new MyPage()) { Title = "나의 만나", IconImageSource = "tab_manna" };
            
            Children.Add(navMannaPage);
            Children.Add(navMccheynePage);
            Children.Add(navMccheyneCheckListPage);
            Children.Add(navMyPage);

        }
    }
}