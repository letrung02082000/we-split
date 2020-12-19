using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string startupSetting = "1";
            bool openSplashWindow = true;

            setting.readSettingDB("startup", ref startupSetting);

            openSplashWindow = startupSetting == "1" ? true : false;

            if (openSplashWindow)
            {
                SplashScreen splashScreen = new SplashScreen();
                splashScreen.Show();
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
    }
}
