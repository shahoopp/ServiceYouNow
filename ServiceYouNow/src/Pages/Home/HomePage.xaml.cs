using System.Windows;
using System.Windows.Controls;
using ServiceYouNow.src.pages.login;
using ServiceYouNow.src.pages.catalog;
using ServiceYouNow.src.pages.mytemplates;
using ServiceYouNow.src.automation;

namespace ServiceYouNow.src.pages.home
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private async void Run_Click(object sender, RoutedEventArgs e)
        {
            var test = new Automation();
            await test.LaunchBrowserAsync();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new LoginPage());
            }
        }

        private void Catalog_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new CatalogPage());
            }
        }

        private void MyTemplates_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new MyTemplatesPage());
            }
        }
    }
}
