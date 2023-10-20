using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TestToolLogin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double windowWidth = mainWindow.Width;
            double left = screenWidth - windowWidth;
            mainWindow.Left = left;
            mainWindow.Show();
        }
    }
}
