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
using ModelLibrary;

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for DestinationScreen.xaml
    /// </summary>
    public partial class DestinationScreen : UserControl
    {
        public ObservableCollection<DestinationModel> destinations;
        public ObservableCollection<string> Province = new ObservableCollection<string>() { "Gia Lai", "TP.HCM", "Hà Nội" };
        public DestinationScreen()
        {
            InitializeComponent();
            
        }

        private void DestinationScreen_Loaded(object sender, RoutedEventArgs e)
        {
            destinations = new ObservableCollection<DestinationModel>(DatabaseAccess.LoadDestination());
            DestinationListView.ItemsSource = destinations;
            ProvinceComboBox.ItemsSource = Province;
        }

        private void AddDestinationBtn_Click(object sender, RoutedEventArgs e)
        {
            DestinationModel destination = new DestinationModel { DesName = NameTextBox.Text, Province = ProvinceComboBox.Text };
            var output = DatabaseAccess.SaveDestination(destination);

            destinations = new ObservableCollection<DestinationModel>(DatabaseAccess.LoadDestination());
            DestinationListView.ItemsSource = destinations;
        }
    }
}
