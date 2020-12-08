using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        public ObservableCollection<JourneyModel> journeyList;
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
        }

        private void JourneyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(JourneyListView.SelectedIndex == -1)
            {
                this.DataContext = journeyList[0];
                JourneyListView.SelectedIndex = 0;
            }
            else
            {
                this.DataContext = journeyList[JourneyListView.SelectedIndex];
            }
        }
            

        private void DetailJourneyBtn_Click(object sender, RoutedEventArgs e)
        {
            //JourneyDetailScreen journeyDetailScreen = new JourneyDetailScreen(journeyList[JourneyListView.SelectedIndex]);
            //DetailJourneyPage detailJourneyPage = new DetailJourneyPage(journeyList[JourneyListView.SelectedIndex]);
            this.NavigationService.Navigate(new DetailJourneyPage(journeyList[JourneyListView.SelectedIndex]));
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            //Set button color
            AllJourneyBorder.Background = Brushes.Black;
            AllJourneyBtn.Foreground = Brushes.White;
            //Load journey list
            journeyList = new ObservableCollection<JourneyModel>(DatabaseAccess.LoadJourney());
            JourneyListView.ItemsSource = journeyList;
            this.DataContext = journeyList[InitIndex];
            JourneyListView.SelectedIndex = InitIndex;
        }

        private void AddJourneyBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddJourneyPage());
        }
    }
}
