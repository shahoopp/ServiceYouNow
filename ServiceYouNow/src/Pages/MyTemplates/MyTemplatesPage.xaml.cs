
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ServiceYouNow.Helpers;
using ServiceYouNow.Models;
using ServiceYouNow.src.pages.home;

namespace ServiceYouNow.src.pages.mytemplates
{
    public partial class MyTemplatesPage : Page
    {
        public MyTemplatesPage()
        {
            InitializeComponent();
            LoadTemplates();
        }

        private void LoadTemplates()
        {
            var prefs = PreferencesStorage.Load();
            if (prefs?.SavedRequests != null && prefs.SavedRequests.Count > 0)
            {
                // Add a Name property dynamically for display
                var displayList = new List<dynamic>();
                foreach (var req in prefs.SavedRequests)
                {
                    displayList.Add(new
                    {
                        Name = "PSG Database Access Request", // Hardcoded for now
                        req.Environment,
                        req.ServerName,
                        req.AccessLevel
                    });
                }

                TemplatesListView.ItemsSource = displayList;
            }

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new HomePage());
        }
    }
}
