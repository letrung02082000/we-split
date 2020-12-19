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
using System.IO;
using Microsoft.Win32;

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
        public ObservableCollection<string> JourneyImageList { get; set; }
        public ObservableCollection<PaymentPerMemberModel> PaymentPerMemberList { get; set; }
        public ObservableCollection<AveragePaymentModel> AveragePaymentList { get; set; }
        public double AveragePayment { get; set; }
        public double RestMemberPayment { get; set; }
        public int CurrentImageIndex { get; set; }
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

            //Get journey image list
            JourneyImageList = new ObservableCollection<string>(DatabaseAccess.LoadJourneyImage(JourneyInfo.JourneyId));
            string directory = AppDomain.CurrentDomain.BaseDirectory + "\\image";

            if(JourneyImageList.Count > 0) {
                CurrentImageIndex = 0;
                JourneyImage.Source = new BitmapImage(new Uri($"{directory}\\{JourneyImageList[0]}"));
                JourneyImageList = new ObservableCollection<string>(DatabaseAccess.LoadJourneyImage(JourneyInfo.JourneyId));
            }

            //Get journey destination list
            DestinationList = new ObservableCollection<DestinationModel>(DatabaseAccess.LoadJourneyDestination(JourneyInfo.JourneyId));
            DestinationListView.ItemsSource = DestinationList;

            PaymentPerMemberList = new ObservableCollection<PaymentPerMemberModel>(DatabaseAccess.LoadPaymentPerMember(JourneyInfo.JourneyId));

            //Calculate average payment
            CalculateAveragePayment();

            PaymentChart.Series = new SeriesCollection();
            MemberChart.Series = new SeriesCollection();

            UpdateCharts();
        }

        private void CalculateAveragePayment()
        {
            AveragePaymentList = new ObservableCollection<AveragePaymentModel>();

            double paymentSum = 0;

            foreach (PaymentPerMemberModel paymentPerMember in PaymentPerMemberList)
            {
                paymentSum += paymentPerMember.PaymentValue;
            }

            AveragePayment = paymentSum / JourneyMemberList.Count;

            foreach (var paymentPerMember in PaymentPerMemberList)
            {
                double averageValue = Math.Round(paymentPerMember.PaymentValue - AveragePayment, 3);
                AveragePaymentModel averagePaymentPerMember = new AveragePaymentModel { MemberId = paymentPerMember.MemberId, MemberName = paymentPerMember.MemberName, PaymentValue = averageValue };
                AveragePaymentList.Add(averagePaymentPerMember);
            }

            RestMemberPayment = -AveragePayment;
            RestMemberPaymentTextBlock.Text = RestMemberPayment.ToString();

            AveragePaymentListView.ItemsSource = AveragePaymentList;
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

            CalculateAveragePayment();
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

        private void AddImageBtn_Click(object sender, RoutedEventArgs e)
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory + "\\image";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string extName = filePath.Split('.').Last();
                string fileName = $"{Guid.NewGuid()}.{extName}";
                DatabaseAccess.SaveJourneyImage(JourneyInfo.JourneyId, fileName);
                string newFilePath = directory + $"\\{fileName}";
                File.Copy(filePath, newFilePath, true);
            }

            JourneyImageList = new ObservableCollection<string>(DatabaseAccess.LoadJourneyImage(JourneyInfo.JourneyId));
        }

        private void NextImageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentImageIndex == JourneyImageList.Count - 1) return;

            ++CurrentImageIndex;
            string directory = AppDomain.CurrentDomain.BaseDirectory + "\\image";
            JourneyImage.Source = new BitmapImage(new Uri($"{directory}\\{JourneyImageList[CurrentImageIndex]}"));

        }

        private void BeforeImageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentImageIndex == 0) return;

            --CurrentImageIndex;
            string directory = AppDomain.CurrentDomain.BaseDirectory + "\\image";
            JourneyImage.Source = new BitmapImage(new Uri($"{directory}\\{JourneyImageList[CurrentImageIndex]}"));

        }
    }
}
