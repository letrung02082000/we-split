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
        public ObservableCollection<string> Province { get; set; }
        public AddDestinationWindow()
        {
            InitializeComponent();
            Province = new ObservableCollection<string>()
            {
                "An Giang",
                "Bà Rịa-Vũng Tàu",
                "Bạc Liêu",
                "Bắc Kạn",
                "Bắc Giang",
                "Bắc Ninh",
                "Bến Tre",
                "Bình Dương",
                "Bình Định",
                "Bình Phước",
                "Bình Thuận",
                "Cà Mau",
                "Cao Bằng",
                "Cần Thơ",
                "Đà Nẵng",
                "Đắk Lắk",
                "Đắk Nông",
                "Điện Biên",
                "Đồng Nai",
                "Đồng Tháp",
                "Gia Lai",
                "TP HCM",
                "Hà Nội",
            };
        }

        private void AddDestinationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DestinationList = new ObservableCollection<DestinationModel>(DatabaseAccess.LoadDestination());
            DestinationListView.ItemsSource = DestinationList;
            ProvinceComboBox.ItemsSource = Province;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if(DesNameTextBox.Text.Trim() == "" || ProvinceComboBox.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin địa danh.");
                return;
            }

            if (EventHandler != null)
            {
                DestinationModel destination = new DestinationModel { DesName = DesNameTextBox.Text, Province = ProvinceComboBox.Text, OldDestination = false };
                EventHandler(destination);
                this.Close();
            }
        }

        private void DestinationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DestinationModel selectedDestination = DestinationList[DestinationListView.SelectedIndex];
            selectedDestination.OldDestination = true;
            if (EventHandler != null)
            {
                EventHandler(selectedDestination);
            }
            this.Close();
        }
    }
}
