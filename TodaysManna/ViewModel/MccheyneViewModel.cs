﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodaysManna.Models;
using TodaysManna.Models.JsonMccheyneContentModel;
using TodaysManna.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace TodaysManna.ViewModel
{
    public class MccheyneViewModel : PageBaseViewModel
    {
        public string Today;

        private ObservableRangeCollection<MccheyneContent> _mccheyneContents1 = new ObservableRangeCollection<MccheyneContent>();
        public ObservableRangeCollection<MccheyneContent> MccheyneContents1 { get => _mccheyneContents1; set => SetProperty(ref _mccheyneContents1, value); }

        private ObservableRangeCollection<MccheyneContent> _mccheyneContents2 = new ObservableRangeCollection<MccheyneContent>();
        public ObservableRangeCollection<MccheyneContent> MccheyneContents2 { get => _mccheyneContents2; set => SetProperty(ref _mccheyneContents2, value); }

        private ObservableRangeCollection<MccheyneContent> _mccheyneContents3 = new ObservableRangeCollection<MccheyneContent>();
        public ObservableRangeCollection<MccheyneContent> MccheyneContents3 { get => _mccheyneContents3; set => SetProperty(ref _mccheyneContents3, value); }

        private ObservableRangeCollection<MccheyneContent> _mccheyneContents4 = new ObservableRangeCollection<MccheyneContent>();
        public ObservableRangeCollection<MccheyneContent> MccheyneContents4 { get => _mccheyneContents4; set => SetProperty(ref _mccheyneContents4, value); }

        private string _mccheyneRange;
        public string MccheyneRange { get => _mccheyneRange; set => SetProperty(ref _mccheyneRange, value); }

        private string _mccheyneRange1;
        public string MccheyneRange1 { get => _mccheyneRange1; set => SetProperty(ref _mccheyneRange1, value); }

        private string _mccheyneRange2;
        public string MccheyneRange2 { get => _mccheyneRange2; set => SetProperty(ref _mccheyneRange2, value); }

        private string _mccheyneRange3;
        public string MccheyneRange3 { get => _mccheyneRange3; set => SetProperty(ref _mccheyneRange3, value); }

        private string _mccheyneRange4;
        public string MccheyneRange4 { get => _mccheyneRange4; set => SetProperty(ref _mccheyneRange4, value); }

        private string _displayDateRange;
        public string DisplayDateRange { get => _displayDateRange; set => SetProperty(ref _displayDateRange, value); }

        public MccheyneViewModel()
        {
            var thisDate = GetCorrectDateLeapYear(DateTime.Now);

            _ = GetMccheyneRange(thisDate);

            try
            {
                _ = GetMccheyne(thisDate);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("GetMccheyne() Error");
            }
        }

        public static DateTime GetCorrectDateLeapYear(DateTime newDate)
        {
            var dateNow = newDate;
            var thisDate = dateNow;
            if (DateTime.IsLeapYear(dateNow.Year) && ((dateNow.Month == 2 && dateNow.Day > 28) || (dateNow.Month > 2)))
            {
                thisDate = thisDate.AddDays(1);
                if (dateNow.Month == 12 && dateNow.Day == 31)
                {
                    thisDate = dateNow;
                }
            }

            return thisDate;
        }

        public Task GetMccheyne(DateTime dateTime)
        {
            MccheyneContents1.Clear();
            MccheyneContents2.Clear();
            MccheyneContents3.Clear();
            MccheyneContents4.Clear();

            var list1 = new List<MccheyneContent>();
            var list2 = new List<MccheyneContent>();
            var list3 = new List<MccheyneContent>();
            var list4 = new List<MccheyneContent>();

            DisplayDateRange = dateTime.ToString("MM/dd");
            var dateTimeString=dateTime.ToString("M_d");

            var daysOfMccheynes = GetJsonService.GetMccheyneBibleTextsFromJson();

            var todayProperty = "Mccheynes" + dateTimeString;

            foreach (var node in daysOfMccheynes)
            {
                var t= node.GetType().GetProperty(todayProperty).GetValue(node, null) as List<Mccheyne>;

                var a = "";
                var b = "";
                var c = "";
                var d = "";

                foreach (var node2 in t)
                {
                    var _firstNum = node2.Verse.Substring(0, node2.Verse.IndexOf(":"));
                    var _secondNum = node2.Verse.Substring(node2.Verse.IndexOf(":")+1);
                    var _fullVerse = $"{node2.Book} {node2.Verse}";
                    var _halfVerse = $"{node2.Book} {_firstNum}";
                    switch (node2.Id)
                    {
                        case "1":
                            if (a != _halfVerse)
                            {
                                list1.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber=_firstNum,
                                    SecondNumber=_secondNum,
                                    Verse = node2.Verse,
                                    FullVerse=_fullVerse + "\n\n",
                                    HalfVerse= _halfVerse,
                                    IsHalfVerseVisible=true,
                                    Content = node2.Content
                                });
                            }
                            else
                            {
                                list1.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = "",
                                    HalfVerse = "",
                                    IsHalfVerseVisible=false,
                                    Content = node2.Content
                                });
                            }
                            a = _halfVerse;
                           
                            break;
                        case "2":
                            if (b != _halfVerse)
                            {
                                list2.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = _fullVerse + "\n\n",
                                    HalfVerse = _halfVerse,
                                    IsHalfVerseVisible = true,
                                    Content = node2.Content
                                });
                            }
                            else
                            {
                                list2.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = "",
                                    HalfVerse = "",
                                    IsHalfVerseVisible = false,
                                    Content = node2.Content
                                });
                            }
                            b = _halfVerse;

                            break;
                        case "3":
                            if (c != _halfVerse)
                            {
                                list3.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = _fullVerse + "\n\n",
                                    HalfVerse = _halfVerse,
                                    IsHalfVerseVisible = true,
                                    Content = node2.Content
                                });
                            }
                            else
                            {
                                list3.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = "",
                                    HalfVerse = "",
                                    IsHalfVerseVisible = false,
                                    Content = node2.Content
                                });
                            }
                            c = _halfVerse;

                            break;
                        case "4":
                            if (d != _halfVerse)
                            {
                                list4.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse=_fullVerse + "\n\n",
                                    HalfVerse = _halfVerse,
                                    IsHalfVerseVisible = true,
                                    Content = node2.Content
                                });
                            }
                            else
                            {
                                list4.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    FullRange = _mccheyneRange,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse="",
                                    HalfVerse = "",
                                    IsHalfVerseVisible = false,
                                    Content = node2.Content
                                });
                            }
                            d = _halfVerse;

                            break;
                    }
                }
            }

            MccheyneContents1.AddRange(list1);
            MccheyneContents2.AddRange(list2);
            MccheyneContents3.AddRange(list3);
            MccheyneContents4.AddRange(list4);

            MccheyneRange1 = MccheyneContents1[0].Book;
            MccheyneRange2 = MccheyneContents2[0].Book;
            MccheyneRange3 = MccheyneContents3[0].Book;
            MccheyneRange4 = MccheyneContents4[0].Book;

            return Task.CompletedTask;
        }

        public Task GetMccheyneRange(DateTime thisDate)
        {
            var findMccheyneDate = thisDate.ToString("M-d");

            var rangeOfDate = App.mccheyneRanges.Find(x => x.Date.Equals(findMccheyneDate));
            MccheyneRange = $"{rangeOfDate.Range1} {rangeOfDate.Range2} {rangeOfDate.Range3} {rangeOfDate.Range4} {rangeOfDate.Range5}";

            return Task.CompletedTask;
        }
    }
}
