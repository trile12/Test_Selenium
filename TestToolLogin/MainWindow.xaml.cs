using MaterialDesignThemes.Wpf;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

            users = new List<User>
            {
                new User { Id = 1, Name = "Phúc Thiên (Test)", Url = "https://www.facebook.com/messages/t/100013640581333" },
                //new User { Id = 2, Name = "Bao Ngoc (Test)", Url = "https://www.facebook.com/messages/t/100071894229494" },
                //new User { Id = 3, Name = "Tri Le (Test)", Url = "https://www.facebook.com/messages/t/100004393085762" },
            };

            dataGrid.ItemsSource = users;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-notifications");

            driver = new ChromeDriver(options);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(userName.Text) || string.IsNullOrEmpty(passWord.Password))
                return;
            try
            {
                Task.Run(() =>
                {
                    driver.Navigate().GoToUrl("https://www.facebook.com/");
                    Dispatcher.Invoke(async () =>
                    {
                        try
                        {
                            driver.FindElement(By.Id("email")).SendKeys(userName.Text);
                            driver.FindElement(By.Id("pass")).SendKeys(passWord.Password);
                            driver.FindElement(By.Name("login")).Click();

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

                            isLoginSucsess = true;
                            LockBorder.Visibility = Visibility.Collapsed;
                        }
                        catch { }
                    });
                });
            }
            catch { }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(noiDungTb.Text) || !isLoginSucsess)
                return;

            SendMessage();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
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
                    Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            SendMessageFuntion(user);
                        }
                        catch
                        {
                            SendMessageFuntion(user);
                        }
                    });
                }
            });
        }

        private void SendMessageFuntion(User user)
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
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
