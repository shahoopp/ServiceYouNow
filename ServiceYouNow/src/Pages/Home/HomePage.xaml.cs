using System.Windows;
using System.Windows.Controls;
using ServiceYouNow.src.pages.login;
using ServiceYouNow.src.pages.triagedetails;
using ServiceYouNow.src.pages.dateselection;
using ServiceYouNow.src.automation;
using ServiceYouNow.Helpers;


namespace ServiceYouNow.src.pages.home
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new LoginPage());
            }
        }

        private void TriageDetails_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new TriageDetails());
            }
        }

        private void DateSelection_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new DateSelectionPage());
            }
        }

        private async void Run_Click(object sender, RoutedEventArgs e)
        {
            var test = new Automation();
            await test.LaunchBrowserAsync();
        }

    }
}
