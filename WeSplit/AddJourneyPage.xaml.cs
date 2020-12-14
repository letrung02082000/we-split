using System;
using System.IO;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.Collections.ObjectModel;
using ModelLibrary;

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for AddJourneyPage.xaml
    /// </summary>
    public partial class AddJourneyPage : Page
    {
        public ObservableCollection<JourneyDestinationModel> JourneyDestinationList { get; set; }
        public ObservableCollection<DestinationModel> DestinationList { get; set; }
        public ObservableCollection<MemberModel> MemberList { get; set; }
        public ObservableCollection<PaymentModel> PaymentList { get; set; }
        public AddJourneyPage()
        {
            InitializeComponent();
            DestinationList = new ObservableCollection<DestinationModel>();
            JourneyDestinationList = new ObservableCollection<JourneyDestinationModel>();
            MemberList = new ObservableCollection<MemberModel>();
            PaymentList = new ObservableCollection<PaymentModel>();
        }

        private void AddJourneyPage_Loaded(object sender, RoutedEventArgs e)
        {
            DestinationListView.ItemsSource = DestinationList;
            MemberListView.ItemsSource = MemberList;
            MemberComboBox.ItemsSource = MemberList;
            PaymentListView.ItemsSource = PaymentList;
        }

        private void AddMemberBtn_Click(object sender, RoutedEventArgs e)
        {
            AddMemberWindow addMemberWindow = new AddMemberWindow();
            addMemberWindow.EventHandler += AddMemberHandler;
            addMemberWindow.Show();
        }

        private void AddCoverImageBtn_Click(object sender, RoutedEventArgs e)
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory + "\\image";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string extName = filePath.Split('.').Last();
                string fileName = $"{Guid.NewGuid()}.{extName}";
                string newFilePath = directory + $"\\{fileName}";
                File.Copy(filePath, newFilePath, true);
                JourneyCoverImage.Source = new BitmapImage(new Uri(newFilePath));
            }

        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                this.NavigationService.GoBack();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddDestinationBtn_Click(object sender, RoutedEventArgs e)
        {
            AddDestinationWindow addDestinationWindow = new AddDestinationWindow();
            addDestinationWindow.EventHandler += AddDestinationHandler;
            addDestinationWindow.Show();
        }
        
        private void AddDestinationHandler(DestinationModel destination)
        {
            DestinationList.Add(destination);
        }

        private void AddMemberHandler(MemberModel member)
        {
            MemberList.Add(member);
        }

        private void AddPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            int paymentValue;
            MemberModel member = MemberComboBox.SelectedItem as MemberModel;
            int.TryParse(paymentValueTextBox.Text, out paymentValue);
            PaymentModel payment = new PaymentModel { PaymentContent=paymentContentTextBox.Text, MemberName=member.MemberName , PaymentValue=paymentValue};
            PaymentList.Add(payment);
        }
    }
}
