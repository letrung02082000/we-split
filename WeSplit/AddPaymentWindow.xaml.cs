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
using System.Windows.Shapes;

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for AddPaymentWindow.xaml
    /// </summary>
    public partial class AddPaymentWindow : Window
    {
        public ObservableCollection<MemberModel> JourneyMemberList { get; set; }
        public JourneyModel JourneyInfo;
        public delegate void UpdatePayment();
        public event UpdatePayment EventHandler;
        public AddPaymentWindow(JourneyModel journeyInfo)
        {
            InitializeComponent();
            JourneyInfo = journeyInfo;
        }

        private void AddPaymentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            JourneyMemberList = new ObservableCollection<MemberModel>(DatabaseAccess.LoadJourneyMember(JourneyInfo.JourneyId));
            MemberComboBox.ItemsSource = JourneyMemberList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int paymentValue = 0;
            int.TryParse(PaymentValueTextBox.Text, out paymentValue);
            MemberModel member = MemberComboBox.SelectedItem as MemberModel;
            PaymentModel payment = new PaymentModel { JourneyId = JourneyInfo.JourneyId, MemberId = member.MemberId, PaymentContent = PaymentContentTextBox.Text, PaymentValue = paymentValue };
            DatabaseAccess.SavePayment(payment);
            EventHandler();
            this.Close();
        }

        
    }
}
