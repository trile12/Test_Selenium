using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace TestToolLogin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IWebDriver driver;
        public MainWindow()
        {
            InitializeComponent();

            List<User> users = new List<User>
            {
                new User { Id = 1, Name = "Phúc Thiên (Test)", Url = "https://www.facebook.com/messages/t/100013640581333" },
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

            driver.Navigate().GoToUrl("https://www.facebook.com/");
            driver.FindElement(By.Id("email")).SendKeys(userName.Text);
            driver.FindElement(By.Id("pass")).SendKeys(passWord.Password);
            driver.FindElement(By.Name("login")).Click();
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(noiDungTb.Text))
                return;

            driver.Navigate().GoToUrl("https://www.facebook.com/messages/t/100013640581333");
            Thread.Sleep(TimeSpan.FromSeconds(3));
            var element = driver.FindElement(By.CssSelector(".notranslate"));
            element.SendKeys($"{noiDungTb.Text} \r\n");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            driver.Quit();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
