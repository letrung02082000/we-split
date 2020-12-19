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
        public JourneyModel JourneyInfo { get; set; }
        public AddJourneyPage()
        {
            InitializeComponent();
            DestinationList = new ObservableCollection<DestinationModel>();
            JourneyDestinationList = new ObservableCollection<JourneyDestinationModel>();
            MemberList = new ObservableCollection<MemberModel>();
            PaymentList = new ObservableCollection<PaymentModel>();
            JourneyInfo = new JourneyModel();
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
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string extName = filePath.Split('.').Last();
                string fileName = $"{Guid.NewGuid()}.{extName}";
                JourneyInfo.CoverImage = fileName;
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
            if (JourneyNameTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên chuyến đi");
                return;
            }

            if (DatePicker1.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập ngày bắt đầu");
                return;
            }

            if (DatePicker2.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập ngày kết thúc");
                return;
            }

            if (JourneyDescriptionTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mô tả chuyến đi");
                return;
            }

            if(DestinationList.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm địa danh");
                return;
            }

            if (MemberList.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm thành viên");
                return;
            }

            //add journey info
            string journeyName = JourneyNameTextBox.Text.Trim();
            string startDate = DatePicker1.SelectedDate.ToString().Split(' ').First();
            string endDate = DatePicker2.SelectedDate.ToString().Split(' ').First();
            string jouneyDescription = JourneyDescriptionTextBox.Text.Trim();

            if (journeyName != "" && startDate != "" && endDate != "")
            {
                JourneyInfo.JourneyName = journeyName;
                JourneyInfo.JourneyDescription = jouneyDescription;
                JourneyInfo.StartDate = startDate;
                JourneyInfo.EndDate = endDate;
                JourneyInfo.JourneyId = DatabaseAccess.SaveJourney(JourneyInfo);
            }

            if(JourneyInfo.CoverImage != null)
            {
                DatabaseAccess.SaveJourneyImage(JourneyInfo.JourneyId, JourneyInfo.CoverImage);
            }

            //add member
            foreach(MemberModel member in MemberList)
            {
                if (!member.OldMember)
                {
                    member.MemberId = DatabaseAccess.SaveMember(member);
                }

                DatabaseAccess.SaveJourneyMember(JourneyInfo.JourneyId, member.MemberId);
            }

            //add journey destination
            foreach(DestinationModel destination in DestinationList)
            {
                if (!destination.OldDestination)
                {
                    destination.DesId = DatabaseAccess.SaveDestination(destination);
                }

                JourneyDestinationModel journeyDestination = new JourneyDestinationModel { JourneyId = JourneyInfo.JourneyId, DesId = destination.DesId };
                DatabaseAccess.SaveJourneyDestination(journeyDestination);
            }

            //add payment
            foreach(PaymentModel payment in PaymentList)
            {
                payment.MemberId = payment.paymentMember.MemberId;
                payment.JourneyId = JourneyInfo.JourneyId;
                DatabaseAccess.SavePayment(payment);
            }

            MessageBox.Show("Đã thêm chuyến đi.");
            this.NavigationService.GoBack();
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
            if (paymentContentTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập nội dung khoản chi");
                return;
            }

            if (paymentValueTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập số tiền");
                return;
            }

            if (MemberComboBox.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng chọn người chi");
                return;
            }

            int paymentValue;

            if(!int.TryParse(paymentValueTextBox.Text, out paymentValue) || paymentValue < 0)
            {
                MessageBox.Show("Vui lòng nhập số tiền là một số nguyên dương");
                return;
            }

            MemberModel member = MemberComboBox.SelectedItem as MemberModel;
            
            PaymentModel payment = new PaymentModel {MemberId=member.MemberId, PaymentContent=paymentContentTextBox.Text, MemberName=member.MemberName , PaymentValue=paymentValue, paymentMember=member};
            PaymentList.Add(payment);
        }
    }
}
