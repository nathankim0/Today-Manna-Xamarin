using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TodaysManna.Models;
using TodaysManna.Views;
using static TodaysManna.Models.MccheyneData;

namespace TodaysManna.ViewModel
{
    public class MccheyneViewModel : INotifyPropertyChanged
    {
        public string today;


        public ObservableCollection<MccheyneContent> _mccheyneContents1 = new ObservableCollection<MccheyneContent>();
        public ObservableCollection<MccheyneContent> MccheyneContents1
        {
            get
            {
                return _mccheyneContents1;
            }
        }

        public ObservableCollection<MccheyneContent> _mccheyneContents2 = new ObservableCollection<MccheyneContent>();
        public ObservableCollection<MccheyneContent> MccheyneContents2
        {
            get
            {
                return _mccheyneContents2;
            }
        }

        public ObservableCollection<MccheyneContent> _mccheyneContents3 = new ObservableCollection<MccheyneContent>();
        public ObservableCollection<MccheyneContent> MccheyneContents3
        {
            get
            {
                return _mccheyneContents3;
            }
        }

        public ObservableCollection<MccheyneContent> _mccheyneContents4 = new ObservableCollection<MccheyneContent>();
        public ObservableCollection<MccheyneContent> MccheyneContents4
        {
            get
            {
                return _mccheyneContents4;
            }
        }

        private MccheyneData _mccheyneData = new MccheyneData();
        public MccheyneData JsonMccheyneData
        {
            get => _mccheyneData;
            set
            {
                if (_mccheyneData != value)
                {
                    _mccheyneData = value;
                    OnPropertyChanged(nameof(JsonMccheyneData));
                }
            }
        }

        private string _verseRange;
        public string VerseRange
        {
            get => _verseRange;
            set
            {
                if (_verseRange != value)
                {
                    _verseRange = value;
                    OnPropertyChanged(nameof(VerseRange));
                }
            }
        }
        public MccheyneViewModel()
        {
            today = DateTime.Now.ToString("M_d");
            GetMccheyne();
        }

        public void GetMccheyne()
        {
            MccheyneContents1.Clear();
            MccheyneContents2.Clear();
            MccheyneContents3.Clear();
            MccheyneContents4.Clear();

            // var daysOfMccheynes = new List<Days>();
            var daysOfMccheynes = GetJsonBible();

            var todayProperty = "Mccheynes" + today;

            foreach (var node in daysOfMccheynes)
            {
                var t= node.GetType().GetProperty(todayProperty).GetValue(node, null) as List<Mccheyne>;

                foreach (var node2 in t)
                {
                    switch (node2.Id)
                    {
                        case "1":
                            MccheyneContents1.Add(new MccheyneContent
                            {
                                Id = node2.Id,
                                Book = node2.Book,
                                Verse = node2.Verse,
                                Content = node2.Content
                            });
                            break;
                        case "2":
                            MccheyneContents2.Add(new MccheyneContent
                            {
                                Id = node2.Id,
                                Book = node2.Book,
                                Verse = node2.Verse,
                                Content = node2.Content
                            });
                            break;
                        case "3":
                            MccheyneContents3.Add(new MccheyneContent
                            {
                                Id = node2.Id,
                                Book = node2.Book,
                                Verse = node2.Verse,
                                Content = node2.Content
                            });
                            break;
                        case "4":
                            MccheyneContents4.Add(new MccheyneContent
                            {
                                Id = node2.Id,
                                Book = node2.Book,
                                Verse = node2.Verse,
                                Content = node2.Content
                            });
                            break;
                    }

                    //System.Diagnostics.Debug.WriteLine(node2.Id);
                    //System.Diagnostics.Debug.WriteLine(node2.Book);
                    //System.Diagnostics.Debug.WriteLine(node2.Verse);
                    //System.Diagnostics.Debug.WriteLine(node2.Content);
                }
            }
        }


        private List<Days> GetJsonBible()
        {
            string jsonFileName = "mcc.json";
            MccheyneList ObjContactList = new MccheyneList();

            var assembly = typeof(MccheynePage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");

            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();  
                ObjContactList = JsonConvert.DeserializeObject<MccheyneList>(jsonString);
            }

            return ObjContactList.DaysOfMccheyne;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
