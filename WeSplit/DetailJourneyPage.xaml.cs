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
using LiveCharts;
using LiveCharts.Wpf;

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
        public ObservableCollection<PaymentPerMemberModel> PaymentPerMemberList { get; set; }
        public ObservableCollection<AveragePaymentModel> AveragePaymentList { get; set; }
        public double AveragePayment { get; set; }
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

            PaymentPerMemberList = new ObservableCollection<PaymentPerMemberModel>(DatabaseAccess.LoadPaymentPerMember(JourneyInfo.JourneyId));

            //Calculate average payment
            AveragePaymentList = new ObservableCollection<AveragePaymentModel>();
            
            double paymentSum = 0;

            foreach(PaymentPerMemberModel paymentPerMember in PaymentPerMemberList)
            {
                paymentSum += paymentPerMember.PaymentValue;
            }

            AveragePayment = paymentSum / PaymentPerMemberList.Count;

            foreach (var paymentPerMember in PaymentPerMemberList)
            {
                double averageValue = Math.Round(paymentPerMember.PaymentValue - AveragePayment, 3);
                AveragePaymentModel averagePaymentPerMember = new AveragePaymentModel { MemberId = paymentPerMember.MemberId, MemberName = paymentPerMember.MemberName, PaymentValue = averageValue };
                AveragePaymentList.Add(averagePaymentPerMember);
            }

            AveragePaymentListView.ItemsSource = AveragePaymentList;

            PaymentChart.Series = new SeriesCollection();
            MemberChart.Series = new SeriesCollection();

            UpdateCharts();
        }

        private void UpdateCharts()
        {
            foreach (var payment in PaymentList)
            {
                PieSeries pieSeries = new PieSeries { Title = payment.PaymentContent, Values = new ChartValues<int> { payment.PaymentValue }};
                PaymentChart.Series.Add(pieSeries);
            }

            foreach (var paymentPerMember in PaymentPerMemberList)
            {
                PieSeries pieSeries = new PieSeries { Title = paymentPerMember.MemberName, Values = new ChartValues<int> { paymentPerMember.PaymentValue } };
                MemberChart.Series.Add(pieSeries);
            }
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
            JourneyInfo.JourneyName = EditLabelTextBox.Text;
            DatabaseAccess.UpdateJourney(JourneyInfo);
        }

        private void AddRouteBtn_Click(object sender, RoutedEventArgs e)
        {
            RouteModel route = new RouteModel { JourneyId = JourneyInfo.JourneyId, RouteContent = AddRouteTextBox.Text };
            DatabaseAccess.SaveRoute(route);
            RouteList = new ObservableCollection<RouteModel>(DatabaseAccess.LoadRoute(JourneyInfo.JourneyId));
            RouteListView.ItemsSource = RouteList;
            AddRouteTextBox.Clear();
        }

        private void AddPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPaymentWindow addPaymentWindow = new AddPaymentWindow(JourneyInfo);
            addPaymentWindow.EventHandler += UpdatePayment;
            addPaymentWindow.Show();
        }

        private void AddMemberBtn_Click(object sender, RoutedEventArgs e)
        {
            AddMemberWindow addMemberWindow = new AddMemberWindow();
            addMemberWindow.EventHandler += AddMemberHandler;
            addMemberWindow.Show();
        }

        private void UpdatePayment()
        {
            PaymentList = new ObservableCollection<PaymentModel>(DatabaseAccess.LoadPayment(JourneyInfo.JourneyId));
            PaymentListView.ItemsSource = PaymentList;

            PaymentPerMemberList = new ObservableCollection<PaymentPerMemberModel>(DatabaseAccess.LoadPaymentPerMember(JourneyInfo.JourneyId));

            PaymentChart.Series = new SeriesCollection();
            MemberChart.Series = new SeriesCollection();

            UpdateCharts();
        }

        private void AddMemberHandler(MemberModel member)
        {
            if (member.OldMember)
            {
                DatabaseAccess.SaveJourneyMember(JourneyInfo.JourneyId, member.MemberId);
                
            }
            else
            {
                member.MemberId = DatabaseAccess.SaveMember(member);
                DatabaseAccess.SaveJourneyMember(JourneyInfo.JourneyId, member.MemberId);
            }

            JourneyMemberList = new ObservableCollection<MemberModel>(DatabaseAccess.LoadJourneyMember(JourneyInfo.JourneyId));
            JourneyMemberListView.ItemsSource = JourneyMemberList;
        }
    }
}
