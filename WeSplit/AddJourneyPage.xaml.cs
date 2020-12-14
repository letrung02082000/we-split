using System;
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

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for AddJourneyPage.xaml
    /// </summary>
    public partial class AddJourneyPage : Page
    {
        public AddJourneyPage()
        {
            InitializeComponent();
        }

        private void AddMemberBtn_Click(object sender, RoutedEventArgs e)
        {
            AddMemberWindow addMemberWindow = new AddMemberWindow();
            addMemberWindow.Show();
        }

        private void AddCoverImageBtn_Click(object sender, RoutedEventArgs e)
        {

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
            addDestinationWindow.Show();
        }
    }
}
