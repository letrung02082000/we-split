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
    }
}
