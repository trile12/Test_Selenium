using MaterialDesignThemes.Wpf;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace TestToolLogin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IWebDriver driver;
        List<User> users;
        private bool isLoginSucsess;
        public MainWindow()
        {
            InitializeComponent();


            users = CsvHelper.ReadUserCSV();
            if (!users.Any())
            {
                users = new List<User>
                        {
                            new User { Name = "Phúc Thiên (Test)", Url = "https://www.facebook.com/messages/t/100013640581333" },
                        };
            }
            dataGrid.ItemsSource = users;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-notifications");
            driver = new ChromeDriver(options);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(userName.Text) || string.IsNullOrEmpty(passWord.Password))
                return;
            try
            {
                await Task.Run(async () =>
                 {
                     await Login();
                 });
                LockUI(false);
            }
            catch { }
        }

        private async Task Login()
        {
            try
            {
                await Task.Run(() =>
                {
                    driver.Navigate().GoToUrl("https://www.facebook.com");
                    Dispatcher.InvokeAsync(() =>
                    {
                        LockUI(true);
                        driver.FindElement(By.Id("email")).SendKeys(userName.Text);
                        driver.FindElement(By.Id("pass")).SendKeys(passWord.Password);
                        driver.FindElement(By.Name("login")).Click();
                    });
                });

                int checkLogin = 0;
                while (checkLogin <= 5)
                {
                    try
                    {
                        var checkMessenger = driver.FindElement(By.XPath("/html/body/div[1]/div/div[1]/div/div[2]/div[3]/div/div/div/div/div/label/input"));
                        break;
                    }
                    catch
                    {
                        await Task.Delay(1000);
                        checkLogin++;
                    }
                }
            }
            catch { }
        }
        private async void LockUI(bool isLocked)
        {
            isLoginSucsess = !isLocked;
            LockBorder.Visibility = isLocked ? Visibility.Visible : Visibility.Collapsed;
            Indicator.Visibility = isLocked ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(noiDungTb.Text) || !isLoginSucsess)
                return;

            SendMessage();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CsvHelper.WriteUserCSV(users);
            driver.Quit();
        }

        private void DarkModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            PaletteHelper palette = new PaletteHelper();

            ITheme theme = palette.GetTheme();

            if (DarkModeToggle.IsChecked.Value)
            {
                theme.SetBaseTheme(Theme.Dark);
            }
            else
            {
                theme.SetBaseTheme(Theme.Light);
            }
            palette.SetTheme(theme);
        }

        private async void SendMessage()
        {
            await Task.Run(() =>
            {
                foreach (var user in users)
                {
                    Dispatcher.InvokeAsync(async() =>
                    {
                        try
                        {
                            await SendMessageFuntion(user);
                        }
                        catch
                        {
                            await SendMessageFuntion(user);
                        }
                    });
                }
            });
        }

        private async Task SendMessageFuntion(User user)
        {
            try
            {
                IWebElement element = null;
                driver.Navigate().GoToUrl(user.Url);
                do
                {
                    try
                    {
                        element = driver.FindElement(By.CssSelector(".notranslate"));
                    }
                    catch { }

                }
                while (element == null);
                element.SendKeys($"{noiDungTb.Text} \r\n");
            }
            catch
            {
            }
        }

        private void Save_CSV_Click(object sender, RoutedEventArgs e)
        {
            CsvHelper.WriteUserCSV(users);
        }

        private void Load_CSV_Click(object sender, RoutedEventArgs e)
        {
            users = CsvHelper.ReadUserCSV();
            dataGrid.ItemsSource = users;
        }
    }

    public class User
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
