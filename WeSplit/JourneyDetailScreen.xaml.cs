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
    /// Interaction logic for JourneyDetailScreen.xaml
    /// </summary>
    public partial class JourneyDetailScreen : UserControl
    {
        public JourneyModel JourneyInfo;
        public ObservableCollection<MemberModel> JourneyMemberList;
        public JourneyDetailScreen(JourneyModel journeyInfo)
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

        private void JourneyDetailScreen_Loaded(object sender, RoutedEventArgs e)
        {
            JourneyMemberList = new ObservableCollection<MemberModel>(DatabaseAccess.LoadJourneyMember(JourneyInfo.JourneyId));
            JourneyMemberListView.ItemsSource = JourneyMemberList;
        }
    }
}
