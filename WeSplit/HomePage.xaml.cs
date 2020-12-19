using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public int InitIndex { get; set; }
        public ObservableCollection<SuperMegaJourneyInfo> SuperMegaJourneyList { get; set; }
        public ObservableCollection<JourneyModel> journeyList;
        public ObservableCollection<MemberModel> MemberList { get; set; }
        public ObservableCollection<DestinationModel> DestinationList { get; set; }
        public HomePage(int initIndex)
        {
            InitializeComponent();
            InitIndex = initIndex;
            
        }

        private void AllJourneyBtn_Click(object sender, RoutedEventArgs e)
        {

            //Set button color
            AllJourneyBorder.Background = Brushes.Black;
            AllJourneyBtn.Foreground = Brushes.White;
            NextJourneyBorder.Background = Brushes.White;
            NextJourneyBtn.Foreground = Brushes.Black;
            BeforeJourneyBorder.Background = Brushes.White;
            BeforeJourneyBtn.Foreground = Brushes.Black;

            journeyList = new ObservableCollection<JourneyModel>(DatabaseAccess.LoadJourney());

            foreach (var journey in journeyList)
            {
                string[] startDateArray = journey.StartDate.Trim().Split('/');
                journey.StartDateTime = new DateTime(int.Parse(startDateArray[2]), int.Parse(startDateArray[0]), int.Parse(startDateArray[1]));

                string[] endDateArray = journey.EndDate.Trim().Split('/');
                journey.EndDateTime = new DateTime(int.Parse(startDateArray[2]), int.Parse(startDateArray[0]), int.Parse(startDateArray[1]));
            }

            JourneyListView.ItemsSource = journeyList;
        }

        private void NextJourneyBtn_Click(object sender, RoutedEventArgs e)
        {

            //Set button color
            AllJourneyBorder.Background = Brushes.White;
            AllJourneyBtn.Foreground = Brushes.Black;
            NextJourneyBorder.Background = Brushes.Black;
            NextJourneyBtn.Foreground = Brushes.White;
            BeforeJourneyBorder.Background = Brushes.White;
            BeforeJourneyBtn.Foreground = Brushes.Black;

            journeyList = new ObservableCollection<JourneyModel>(DatabaseAccess.LoadJourney());

            foreach (var journey in journeyList)
            {
                string[] startDateArray = journey.StartDate.Trim().Split('/');
                journey.StartDateTime = new DateTime(int.Parse(startDateArray[2]), int.Parse(startDateArray[0]), int.Parse(startDateArray[1]));

                string[] endDateArray = journey.EndDate.Trim().Split('/');
                journey.EndDateTime = new DateTime(int.Parse(startDateArray[2]), int.Parse(startDateArray[0]), int.Parse(startDateArray[1]));
            }

            //filter next journey
            journeyList = new ObservableCollection<JourneyModel>(journeyList.Where(journey => journey.StartDateTime >= DateTime.Now).OrderBy(d=>d.StartDateTime).ToList());
            JourneyListView.ItemsSource = journeyList;

            if(journeyList.Count > 0)
            {
                JourneyListView.SelectedIndex = 0;
            }

            if (JourneyListView.SelectedIndex >= 0)
            {
                List<string> imageList = DatabaseAccess.LoadJourneyImage(journeyList[JourneyListView.SelectedIndex].JourneyId);
                if (imageList.Count > 0)
                {
                    string imageUrl = imageList[0];
                    imageUrl = $"{AppDomain.CurrentDomain.BaseDirectory}\\image\\{imageUrl}";
                    JourneyImage.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                }
                else
                {
                    JourneyImage.Source = null;
                }
            }
        }

        private void BeforeJourneyBtn_Click(object sender, RoutedEventArgs e)
        {
            //Set button color
            AllJourneyBorder.Background = Brushes.White;
            AllJourneyBtn.Foreground = Brushes.Black;
            NextJourneyBorder.Background = Brushes.White;
            NextJourneyBtn.Foreground = Brushes.Black;
            BeforeJourneyBorder.Background = Brushes.Black;
            BeforeJourneyBtn.Foreground = Brushes.White;

            journeyList = new ObservableCollection<JourneyModel>(DatabaseAccess.LoadJourney());

            foreach (var journey in journeyList)
            {
                string[] startDateArray = journey.StartDate.Trim().Split('/');
                journey.StartDateTime = new DateTime(int.Parse(startDateArray[2]), int.Parse(startDateArray[0]), int.Parse(startDateArray[1]));

                string[] endDateArray = journey.EndDate.Trim().Split('/');
                journey.EndDateTime = new DateTime(int.Parse(startDateArray[2]), int.Parse(startDateArray[0]), int.Parse(startDateArray[1]));
            }

            //filter before journey
            journeyList = new ObservableCollection<JourneyModel>(journeyList.Where(journey => journey.EndDateTime < DateTime.Now).OrderBy(d => d.StartDateTime).ToList());
            JourneyListView.ItemsSource = journeyList;

            if (journeyList.Count > 0)
            {
                JourneyListView.SelectedIndex = 0;
            }

            if (JourneyListView.SelectedIndex >= 0)
            {
                List<string> imageList = DatabaseAccess.LoadJourneyImage(journeyList[JourneyListView.SelectedIndex].JourneyId);
                if (imageList.Count > 0)
                {
                    string imageUrl = imageList[0];
                    imageUrl = $"{AppDomain.CurrentDomain.BaseDirectory}\\image\\{imageUrl}";
                    JourneyImage.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                }
                else
                {
                    JourneyImage.Source = null;
                }
            }
        }

        private void JourneyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(JourneyListView.SelectedIndex == -1)
            {
                //this.DataContext = journeyList[0];
                //JourneyListView.SelectedIndex = 0;
            }
            else
            {
                List<string> imageList = DatabaseAccess.LoadJourneyImage(journeyList[JourneyListView.SelectedIndex].JourneyId);
                if (imageList.Count > 0)
                {
                    string imageUrl = imageList[0];
                    imageUrl = $"{AppDomain.CurrentDomain.BaseDirectory}\\image\\{imageUrl}";
                    JourneyImage.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                }
                else
                {
                    JourneyImage.Source = null;
                }

                if(journeyList.Count > 0)
                {
                    this.DataContext = journeyList[JourneyListView.SelectedIndex];
                }
                
            }
        }
            

        private void DetailJourneyBtn_Click(object sender, RoutedEventArgs e)
        {
            if(JourneyListView.SelectedIndex >= 0)
            {
                this.NavigationService.Navigate(new DetailJourneyPage(journeyList[JourneyListView.SelectedIndex]));
            }
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            //Load Super Mega Journey list
            SuperMegaJourneyList = new ObservableCollection<SuperMegaJourneyInfo>(DatabaseAccess.LoadSuperMegaJourney());

            //Set button color
            AllJourneyBorder.Background = Brushes.Black;
            AllJourneyBtn.Foreground = Brushes.White;

            //Load journey list
            journeyList = new ObservableCollection<JourneyModel>(DatabaseAccess.LoadJourney());
            JourneyListView.ItemsSource = journeyList;
            //this.DataContext = journeyList[InitIndex];
            JourneyListView.SelectedIndex = InitIndex;

            //Convert datetime
            foreach(var journey in journeyList)
            {
                string[] startDateArray = journey.StartDate.Trim().Split('/');
                journey.StartDateTime = new DateTime(int.Parse(startDateArray[2]), int.Parse(startDateArray[0]), int.Parse(startDateArray[1]));

                string[] endDateArray = journey.EndDate.Trim().Split('/');
                journey.EndDateTime = new DateTime(int.Parse(startDateArray[2]), int.Parse(startDateArray[0]), int.Parse(startDateArray[1]));
            }

            //Load Member list
            MemberList = new ObservableCollection<MemberModel>(DatabaseAccess.LoadMember());

            //Load Destination list
            DestinationList = new ObservableCollection<DestinationModel>(DatabaseAccess.LoadDestination());
        }

        private void AddJourneyBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddJourneyPage());
        }

        private void ClearSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            FilterComboBox.SelectedIndex = 0;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox).Text.Trim();
            searchText = ConvertToUnSign(searchText);
            List<JourneyModel> resultList = new List<JourneyModel>();

            if (searchText.Trim() == "")
            {
                journeyList = new ObservableCollection<JourneyModel>(DatabaseAccess.LoadJourney());
            }
            else
            {
                if(FilterComboBox.SelectedIndex == 0)
                {
                    var searchResult = from journey in SuperMegaJourneyList
                                       where journey.JourneyName2.Contains(searchText) || journey.MemberName2.Contains(searchText) || journey.DesName2.Contains(searchText)
                                       select new { journey.JourneyId, journey.JourneyName, journey.JourneyDescription, journey.StartDate, journey.EndDate, journey.CoverImage, journey.JourneyName2 };
                    searchResult = searchResult.Distinct();
                    resultList.Clear();

                    foreach (var journey in searchResult)
                    {
                        JourneyModel journeyModel = new JourneyModel { JourneyId = journey.JourneyId, JourneyName = journey.JourneyName, StartDate = journey.StartDate, EndDate = journey.EndDate, JourneyDescription = journey.JourneyDescription, JourneyName2 = journey.JourneyName2 };
                        resultList.Add(journeyModel);
                    }

                    journeyList = new ObservableCollection<JourneyModel>(resultList);
                }
                else if(FilterComboBox.SelectedIndex == 1)
                {
                    var searchResult = from journey in SuperMegaJourneyList
                                       where journey.JourneyName2.Contains(searchText)
                                       select new { journey.JourneyId, journey.JourneyName, journey.JourneyDescription, journey.StartDate, journey.EndDate, journey.CoverImage, journey.JourneyName2 };
                    searchResult = searchResult.Distinct();
                    resultList.Clear();

                    foreach (var journey in searchResult)
                    {
                        JourneyModel journeyModel = new JourneyModel { JourneyId = journey.JourneyId, JourneyName = journey.JourneyName, StartDate = journey.StartDate, EndDate = journey.EndDate, JourneyDescription = journey.JourneyDescription, JourneyName2 = journey.JourneyName2 };
                        resultList.Add(journeyModel);
                    }

                    journeyList = new ObservableCollection<JourneyModel>(resultList);
                } else if (FilterComboBox.SelectedIndex == 2)
                {
                    var searchResult = from journey in SuperMegaJourneyList
                                       where journey.DesName2.Contains(searchText)
                                       select new { journey.JourneyId, journey.JourneyName, journey.JourneyDescription, journey.StartDate, journey.EndDate, journey.CoverImage, journey.JourneyName2 };
                    searchResult = searchResult.Distinct();


                    resultList.Clear();

                    foreach (var journey in searchResult)
                    {
                        JourneyModel journeyModel = new JourneyModel { JourneyId = journey.JourneyId, JourneyName = journey.JourneyName, StartDate = journey.StartDate, EndDate = journey.EndDate, JourneyDescription = journey.JourneyDescription, JourneyName2 = journey.JourneyName2 };
                        resultList.Add(journeyModel);
                    }

                    journeyList = new ObservableCollection<JourneyModel>(resultList);
                } else if (FilterComboBox.SelectedIndex == 3)
                {
                    var searchResult = from journey in SuperMegaJourneyList
                                       where journey.MemberName2.Contains(searchText)
                                       select new { journey.JourneyId, journey.JourneyName, journey.JourneyDescription, journey.StartDate, journey.EndDate, journey.CoverImage, journey.JourneyName2 };
                    searchResult = searchResult.Distinct();

                    resultList.Clear();

                    foreach (var journey in searchResult)
                    {
                        JourneyModel journeyModel = new JourneyModel { JourneyId = journey.JourneyId, JourneyName = journey.JourneyName, StartDate = journey.StartDate, EndDate = journey.EndDate, JourneyDescription = journey.JourneyDescription, JourneyName2 = journey.JourneyName2 };
                        resultList.Add(journeyModel);
                    }

                    journeyList = new ObservableCollection<JourneyModel>(resultList);
                }
                
            }

            JourneyListView.ItemsSource = journeyList;
        }

        public static string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2.ToLower();
        }
    }
}
