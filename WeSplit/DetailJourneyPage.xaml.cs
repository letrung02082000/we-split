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
    /// Interaction logic for DetailJourneyPage.xaml
    /// </summary>
    public partial class DetailJourneyPage : Page
    {
        public JourneyModel JourneyInfo { get; set; }
        public ObservableCollection<MemberModel> JourneyMemberList { get; set; }
        public ObservableCollection<PaymentModel> PaymentList { get; set; }
        public ObservableCollection<RouteModel> RouteList { get; set; }
        public ObservableCollection<DestinationModel> DestinationList { get; set; }
        public DetailJourneyPage(JourneyModel journeyInfo)
        {
            InitializeComponent();
            JourneyInfo = new JourneyModel
            {
                JourneyId = journeyInfo.JourneyId,
                JourneyName = journeyInfo.JourneyName,
                JourneyDescription = journeyInfo.JourneyDescription,
                StartDate = journeyInfo.StartDate,
                EndDate = journeyInfo.EndDate,
                CoverImage = journeyInfo.CoverImage
            };
        }

        private void DetailJourneyPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Get member list
            JourneyMemberList = new ObservableCollection<MemberModel>(DatabaseAccess.LoadJourneyMember(JourneyInfo.JourneyId));
            JourneyMemberListView.ItemsSource = JourneyMemberList;
            this.DataContext = JourneyInfo;

            //Get payment list
            PaymentList = new ObservableCollection<PaymentModel>(DatabaseAccess.LoadPayment(JourneyInfo.JourneyId));
            PaymentListView.ItemsSource = PaymentList;

            //Get route list
            RouteList = new ObservableCollection<RouteModel>(DatabaseAccess.LoadRoute(JourneyInfo.JourneyId));
            RouteListView.ItemsSource = RouteList;

            //Get journey destination list
            DestinationList = new ObservableCollection<DestinationModel>(DatabaseAccess.LoadJourneyDestination(JourneyInfo.JourneyId));
            DestinationListView.ItemsSource = DestinationList;
        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                int initIndex = JourneyInfo.JourneyId - 1;
                this.NavigationService.Navigate(new HomePage(initIndex));
            }
        }

        private void EditJourneyNameBtn_Click(object sender, RoutedEventArgs e)
        {
            JourneyLabelStackPanel.Visibility = Visibility.Hidden;
            EditLabelStackPanel.Visibility = Visibility.Visible;
            EditLabelTextBox.Text = JourneyInfo.JourneyName;

        }

        private void ConfirmJourneyNameBtn_Click(object sender, RoutedEventArgs e)
        {
            JourneyLabelStackPanel.Visibility = Visibility.Visible;
            EditLabelStackPanel.Visibility = Visibility.Hidden;
            JourneyLabel.Content = EditLabelTextBox.Text;
        }

        private void AddRouteBtn_Click(object sender, RoutedEventArgs e)
        {
            AddRouteStackPanel.Visibility = Visibility.Visible;
            AddRouteBtn.Visibility = Visibility.Hidden;
        }

        private void AddPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPaymentWindow addPaymentWindow = new AddPaymentWindow();
            addPaymentWindow.Show();
        }

        private void AddMemberBtn_Click(object sender, RoutedEventArgs e)
        {
            AddMemberWindow addMemberWindow = new AddMemberWindow();
            addMemberWindow.Show();
        }
    }
}
