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
    /// Interaction logic for AddDestinationWindow.xaml
    /// </summary>
    public partial class AddDestinationWindow : Window
    {
        public delegate void AddDestination(DestinationModel destination);
        public event AddDestination EventHandler;
        public ObservableCollection<DestinationModel> DestinationList { get; set; }
        public AddDestinationWindow()
        {
            InitializeComponent();
        }

        private void AddDestinationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DestinationList = new ObservableCollection<DestinationModel>(DatabaseAccess.LoadDestination());
            DestinationListView.ItemsSource = DestinationList;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if(EventHandler != null)
            {
                DestinationModel destination = new DestinationModel { DesName = DesNameTextBox.Text, Province = ProvinceComboBox.Text };
                EventHandler(destination);
                this.Close();
            }
        }

        private void DestinationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DestinationModel selectedDestination = DestinationList[DestinationListView.SelectedIndex];
            if (EventHandler != null)
            {
                EventHandler(selectedDestination);
            }
            this.Close();
        }
    }
}
