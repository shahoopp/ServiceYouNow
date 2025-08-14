using System.Windows;
using System.Windows.Controls;
using ServiceYouNow.Helpers;
using ServiceYouNow.src.pages.home;

namespace ServiceYouNow.src.pages.catalog
{
    public partial class CatalogPage : Page
    {
        public CatalogPage()
        {
            InitializeComponent();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new HomePage());
        }

        private void PSGDatabaseAccessRequest_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame
                .Navigate(new CatalogItem("PSG_DB_AR"));
        }

        private void MicrosoftSQLDatabaseAccessISG_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame
                .Navigate(new CatalogItem("MS_SQL_DB_ISG"));
        }

    }
}