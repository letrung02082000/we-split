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
    /// Interaction logic for SettingScreen.xaml
    /// </summary>
    public partial class SettingScreen : UserControl
    {
        public SettingScreen()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string checkState = "1";
            setting.readSettingDB("startup", ref checkState);
            bool startupCheck = checkState == "1" ? true : false;
            startupCheckBox.IsChecked = startupCheck;

        }

        private void startupCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            setting.SaveSetting("startup", "1");
        }

        private void startupCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            setting.SaveSetting("startup", "0");
        }
    }
}
