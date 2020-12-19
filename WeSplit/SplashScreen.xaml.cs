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
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public ObservableCollection<DestinationModel> DestinationList { get; set; }
        public Random rsg = new Random();
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void SplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            DestinationList = new ObservableCollection<DestinationModel>(DatabaseAccess.LoadDestination());
            int index = rsg.Next(DestinationList.Count);
            this.DataContext = DestinationList[index];

            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            setting.SaveSetting("startup", "0");
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            setting.SaveSetting("startup", "1");
        }
    }
}
